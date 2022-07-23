// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Collections.Concurrent;
    using System.Collections.ObjectModel;
    using System.Threading;

    using Zorglub.Time.Core;

    // FIXME(code): Invalid might have been a bad idea. If the Calendar ctor
    // checks the value, it will fail hard. It would be the case if for
    // instance we initialized a date,
    // > var date = new CalendarDate(..., id);
    // Improve CalendarCreated: use the standard event pattern. Async? <- no.
    // How to add a calendar with a dirty key? in fact, this is not a problem
    // because it means that we reached the max number of user-defined calendars.
    // Exception neutral code?

    internal sealed partial class CalendarRegistry
    {
        /// <summary>
        /// Represents the minimum value for the ID of a user-defined calendar.
        /// <para>It is also the absolute minimum value for <see cref="MinId"/>.</para>
        /// <para>This field is a constant equal to 64.</para>
        /// </summary>
        public const int MinMinId = (int)Cuid.MinUser;

        /// <summary>
        /// Represents the maximum value for the ID of a user-defined calendar.
        /// <para>It is also the absolute maximum value for <see cref="MaxId"/>.</para>
        /// <para>This field is a constant equal to 127.</para>
        /// </summary>
        public const int MaxMaxId = (int)Cuid.Max;

        /// <summary>
        /// Represents the dictionary of (lazy) calendars, indexed by their keys.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ConcurrentDictionary<string, Lazy<Calendar>> _calendarsByKey;

        private int _lastId;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarRegistry"/> class.
        /// </summary>
        public CalendarRegistry() : this(MinMinId, MaxMaxId) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarRegistry"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendars"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="calendars"/> contains more than
        /// <see cref="MinMinId"/> elements -or- contains a user-defined calendar.</exception>
        public CalendarRegistry(Calendar[] calendars) : this(MinMinId, MaxMaxId)
        {
            InitializeSystemCalendars(calendars);
        }

        private void InitializeSystemCalendars(Calendar[] calendars)
        {
            // Only call this method from a ctor and only once.
            // We keep this routine outside the ctor, in case we decide to add a
            // new ctor with params (minId, maxId, calendars).

            Requires.NotNull(calendars);
            // TODO(doc): clean up.
            // While not strictly necessary, we shall verify that the index of a
            // calendar in "calendars" equals its ID.
            // The ID of the first created user-defined calendar is MinId.
            // If "count" is > MinId, MinId is de facto a valid ID for a system
            // calendars, therefore we will have at least two calendars with
            // the same ID. In fact, it's not possible to create a system
            // calendar with an ID >= MinMinId, but we don't know here what's
            // inside "calendars", we will have to wait for
            // InitializeSystemCalendars() for that.
            if (calendars.Length > MinMinId) Throw.Argument(nameof(calendars));

            // NB: we don't call the callback CalendarCreated, we assume that
            // the caller has already taken care of it. It makes sense since the
            // callback is called CalendarCreated, not CalendarAdded
            for (int id = 0; id < calendars.Length; id++)
            {
                var chr = calendars[id];
                // The tests below ensure that the calendars array does not
                // contain any user-defined calendar and that the index of a
                // calendar in "calendars" is given by its ID.
                // As a side effect, a calendar cannot appear twice. In fact,
                // it would not matter if it was the case, as it does not
                // change the result, it is just a "waste" of resources.
                // WARNING: do not change the order of the checks below,
                // otherwise it's harder to achieve full code coverage.
                // Indeed, a user-defined calendar has an ID >= MinMinId
                // which therefore cannot match the index of an array whose
                // last index is < MinMinId.
                if (chr.IsUserDefined) Throw.Argument(nameof(calendars));
                if ((int)chr.Id != id) Throw.Argument(nameof(calendars));

                // Indexer instead of TryAdd(): unconditional add.
                _calendarsByKey[chr.Key] = new Lazy<Calendar>(chr);
                NumberOfSystemCalendars++;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarRegistry"/> class.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="minId"/> is not
        /// within the range [<see cref="MinMinId"/>..<see cref="MaxMaxId"/>].</exception>
        /// <exception cref="AoorException"><paramref name="maxId"/> is not
        /// within the range [<see cref="minId"/>..<see cref="MaxMaxId"/>].</exception>
        public CalendarRegistry(int minId, int maxId)
        {
            // First prime number >= max nbr of calendars (128 = MaxMaxId + 1).
            const int Capacity = 131;

            if (minId < MinMinId || minId > MaxMaxId) Throw.ArgumentOutOfRange(nameof(minId));
            if (maxId < minId || maxId > MaxMaxId) Throw.ArgumentOutOfRange(nameof(maxId));

            MinId = minId;
            MaxId = maxId;
            _lastId = minId - 1;

            Debug.Assert(Capacity > MaxMaxId);
            _calendarsByKey = new ConcurrentDictionary<string, Lazy<Calendar>>(
                // If I'm not mistaken, this is the default concurrency level.
                concurrencyLevel: Environment.ProcessorCount,
                Capacity);
        }

        /// <summary>
        /// Gets the minimum value for the ID of a user-defined calendar.
        /// </summary>
        public int MinId { get; }

        /// <summary>
        /// Gets the absolute maximum value for the ID of a calendar.
        /// </summary>
        public int MaxId { get; }

        /// <summary>
        /// Returns true if the registry is in its pristine state; otherwise returns false.
        /// </summary>
        public bool IsPristine => _lastId < MinId;

        // Disable fast track. Only for testing.
        public bool ForceCanAdd { get; set; }

        /// <summary>
        /// Returns false if the registry cannot add a new calendar to the current instance;
        /// otherwise returns true BUT one cannot assert that the registry is not full.
        /// </summary>
        // Fast track? Only use this property to check if the registry is full,
        // that is if (CanAdd == false).
        // Indeed, _lastId is incremented very late in the process which means
        // that CanAdd may return true even if, in the end, it's not truely the
        // case.
        private bool CanAdd => ForceCanAdd || _lastId < MaxId;

        public Action<Calendar>? CalendarCreated { get; init; }

        /// <summary>
        /// Gets the list of keys of all fully constructed calendars at the time of the request.
        /// <para>This collection may also contain a few dirty keys, those paired with a calendar
        /// with an ID equal to <see cref="Cuid.Invalid"/>.</para>
        /// </summary>
        public ICollection<string> Keys => _calendarsByKey.Keys;

        /// <summary>
        /// Gets the absolute maximum number of calendars.
        /// </summary>
        public int MaxNumberOfCalendars => MaxId + 1; // <= 128

        /// <summary>
        /// Gets the absolute maximum number of user-defined calendars.
        /// </summary>
        public int MaxNumberOfUserCalendars => MaxId - MinId + 1; // <= 64

        /// <summary>
        /// Gets or sets the number of system calendars.
        /// </summary>
        public int NumberOfSystemCalendars { get; private set; } // <= 64

        /// <summary>
        /// Gets the number of calendars, including <i>dirty</i> calendars.
        /// </summary>
        public int Count => _calendarsByKey.Count;

        /// <summary>
        /// Counts the number of calendars.
        /// </summary>
        [Pure]
        public int CountCalendars() =>
            // Beware, we cannot use _calendarsByKey.Count because it also
            // includes the dirty calendars. The correct formula is given by:
            // > _calendarsByKey.Where(x => x.Value.Value.Id != Cuid.Invalid).Count()
            // We prefer the faster formula:
            NumberOfSystemCalendars + CountUserCalendars();

        /// <summary>
        /// Counts the number of user-defined calendars.
        /// </summary>
        [Pure]
        public int CountUserCalendars() =>
            // The result is <= number of user-defined calendars found in
            // _calendarsByKey.
            // When one registers a new calendar, we first reserve the key, and
            // only after do we increment _lastId. What does it mean is that,
            // during a very short window of time, this property will return a
            // value less than the number of user-defined calendars found in the
            // dictionary _calendarsByKey. At the same time, we can say that a
            // calendar should not be counted until it is fully constructed,
            // that is until CreateCalendar() completes.
            // TODO(doc): we don't really count fully constructed calendars
            // because _lastId is incremented before the calendar's constructor
            // is called.
            //
            // We use Math.Min() because CreateCalendar() will eventually
            // increment _lastId to (1 + MaxId).
            Math.Min(_lastId, MaxId) - MinId + 1;

        /// <summary>
        /// Add a calendar to the current instance.
        /// <para><i>For testing purposes only</i>.</para>
        /// <para>One SHOULD use this method to add a user-defined calendar with an invalid ID.</para>
        /// <para>Beware, a user-defined calendar will be ignored by
        /// <see cref="CountCalendars()"/> and
        /// <see cref="CountUserCalendars()"/>, and a system calendar will be ignored by
        /// <see cref="NumberOfSystemCalendars"/>.</para>
        /// </summary>
        public void Add(Calendar calendar)
        {
            Requires.NotNull(calendar);

            _calendarsByKey[calendar.Key] = new Lazy<Calendar>(calendar);
        }
    }

    internal sealed partial class CalendarRegistry // Snapshot & lookup
    {
        // We don't verify whether the removal of a dirty calendar (see the
        // "Add"-methods) is successful or not, therefore we MUST filter them
        // out.

        [Pure]
        public IReadOnlyDictionary<string, Calendar> TakeSnapshot()
        {
            // Freeze the backing store.
            var arr = _calendarsByKey.ToArray();

            var dict = new Dictionary<string, Calendar>(arr.Length);
            foreach (var x in arr)
            {
                var chr = x.Value.Value;
                // Filter out dirty calendars.
                if (chr.Id == Cuid.Invalid) { continue; }
                dict.Add(x.Key, chr);
            }

            return new ReadOnlyDictionary<string, Calendar>(dict);
        }

        [Pure]
        public Calendar GetCalendar(string key)
        {
            if (_calendarsByKey.TryGetValue(key, out Lazy<Calendar>? calendar))
            {
                var chr = calendar.Value;
                // Filter out dirty calendars.
                return chr.Id == Cuid.Invalid ? Throw.KeyNotFound<Calendar>(key) : chr;
            }

            return Throw.KeyNotFound<Calendar>(key);
        }

        [Pure]
        public bool TryGetCalendar(string key, [NotNullWhen(true)] out Calendar? calendar)
        {
            if (_calendarsByKey.TryGetValue(key, out Lazy<Calendar>? chr))
            {
                var tmp = chr.Value;
                // Filter out dirty calendars.
                if (tmp.Id == Cuid.Invalid) { goto NOT_FOUND; }
                calendar = tmp;
                return true;
            }

        NOT_FOUND:
            calendar = null;
            return false;
        }
    }

    internal sealed partial class CalendarRegistry // Add
    {
        [Pure]
        public Calendar GetOrAdd(string key, SystemSchema schema, DayNumber epoch, bool proleptic)
        {
            // Fail fast. It also guards the method against brute-force attacks.
            // This should only be done when the key is not already taken, in
            // which case we return the calendar having this key.
            if (CanAdd == false && _calendarsByKey.ContainsKey(key) == false)
            {
                Throw.CatalogOverflow();
            }

            // We validate the params even if the key is already taken.
            // Reason: the params might be different and we wish to apply strict
            // validation rules.
            Calendar tmp = ValidateParameters(key, schema, epoch, proleptic);

            // The callback won't be executed until we query Value.
            var lazy = new Lazy<Calendar>(() => CreateCalendar(tmp));

            var lazy1 = _calendarsByKey.GetOrAdd(key, lazy);

            // If the key is already taken, obviously lazy1.Value returns the
            // pre-existing calendar, but the important point here is that
            // CreateCalendar() was not called and therefore _lastId did
            // NOT change. Beware, below this is NOT equivalent to lazy.Value.
            var chr = lazy1.Value;

            if (chr.Id == Cuid.Invalid)
            {
                // Clean up.
                // It's possible to end up here even if lazy1.Value returned a
                // pre-existing calendar. Indeed, there is a very short window
                // of time during which a temp Calendar is still referenced by
                // _calendarsByKey.
                // The next check ensures that we only try to unlist the local
                // instance of temp Calendar. Actually, this is not mandatory,
                // we could try to remove the same key twice...
                if (ReferenceEquals(lazy1, lazy))
                {
                    _calendarsByKey.TryRemove(key, out _);
                }

                Throw.CatalogOverflow();
            }

            return chr;
        }

        [Pure]
        public Calendar Add(string key, SystemSchema schema, DayNumber epoch, bool proleptic)
        {
            // Fail fast. It also guards the method against brute-force attacks.
            if (CanAdd == false) Throw.CatalogOverflow();

            Calendar tmp = ValidateParameters(key, schema, epoch, proleptic);

            // The callback won't be executed until we query Value.
            var lazy = new Lazy<Calendar>(() => CreateCalendar(tmp));

            if (_calendarsByKey.TryAdd(key, lazy) == false)
            {
                Throw.KeyAlreadyExists(nameof(key), key);
            }

            // NB: CreateCalendar() is only called NOW.
            var chr = lazy.Value;

            if (chr.Id == Cuid.Invalid)
            {
                // Clean up.
                _calendarsByKey.TryRemove(key, out _);

                Throw.CatalogOverflow();
            }

            return chr;
        }

        [Pure]
        public bool TryAdd(
            string key, SystemSchema schema, DayNumber epoch, bool proleptic,
            [NotNullWhen(true)] out Calendar? calendar)
        {
            // Fail fast. It also guards the method against brute-force attacks.
            if (CanAdd == false) { goto FAILED; }

            // Null parameters validation.
            Requires.NotNull(key);
            Requires.NotNull(schema);

            // Other parameters validation.
            Calendar tmp;
            try { tmp = ValidateParameters(key, schema, epoch, proleptic); }
            catch (ArgumentException) { goto FAILED; }

            // The callback won't be executed until we query lazy.Value.
            var lazy = new Lazy<Calendar>(() => CreateCalendar(tmp));

            if (_calendarsByKey.TryAdd(key, lazy))
            {
                // NB: CreateCalendar() is only called NOW.
                var chr = lazy.Value;

                if (chr.Id == Cuid.Invalid)
                {
                    // Clean up.
                    _calendarsByKey.TryRemove(key, out _);

                    goto FAILED;
                }
                else
                {
                    calendar = chr;
                    return true;
                }
            }

        FAILED:
            calendar = null;
            return false;
        }

        //
        // Helpers
        //

        /// <summary>
        /// Pre-validates the parameters.
        /// </summary>
        /// <returns>A calendar with an ID <see cref="Cuid.Invalid"/>.</returns>
        [Pure]
        private static Calendar ValidateParameters(
            string key,
            SystemSchema schema,
            DayNumber epoch,
            bool proleptic)
        {
            return new Calendar(Cuid.Invalid, key, schema, epoch, proleptic);
        }

        /// <summary>
        /// Creates a new <see cref="Calendar"/> instance from the specified temporary calendar then
        /// call <see cref="CalendarCreated"/>.
        /// <para>Returns a calendar with ID <see cref="Cuid.Invalid"/> if the system already
        /// reached the maximum number of calendars it can handle.</para>
        /// </summary>
        [Pure]
        private Calendar CreateCalendar(Calendar tmpCalendar)
        {
            Debug.Assert(tmpCalendar != null);

            // Lazy<T> :
            //   Quelque soit le nombre de fils d'exécution en cours,
            //   CreateCalendar() est exécutée en différé et au plus une fois
            //   (pour une même clé), et ce quand on demande la prop Value.
            //   En conséquence, _lastId ne sera incrémenté qu'une fois.
            //   Bien entendu, avec deux clés différentes, le problème ne se
            //   pose pas.
            int id = Interlocked.Increment(ref _lastId);
            if (id > MaxId)
            {
                // We don't throw an OverflowException yet.
                // This will give the caller the opportunity to do some cleanup.
                // Objects of this type only exist for a very short period of
                // time in _calendarsByKey and MUST be filtered out anytime we
                // query a value from this dictionary.
                return tmpCalendar;
            }

            Debug.Assert(id <= (int)Cuid.Max);

            // Notes:
            // - the cast to Cuid always succeed.
            // - the ctor won't throw since we already validated the params
            //   when we created tmpCalendar.
            var chr = new Calendar(
                (Cuid)id,
                tmpCalendar.Key,
                tmpCalendar.Schema,
                tmpCalendar.Epoch,
                tmpCalendar.IsProleptic);

            CalendarCreated?.Invoke(chr);

            return chr;
        }
    }
}
