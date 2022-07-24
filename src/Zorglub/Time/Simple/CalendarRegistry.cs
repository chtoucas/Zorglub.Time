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

    // More or less, CalendarRegistry behaves like a concurrent keyed collection
    // whose keys are readable and writable but not updatable.

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
        /// <exception cref="ArgumentException"><paramref name="calendars"/> contains a user-defined
        /// calendar -or- the index of a (system) calendar in the array is NOT given by its ID.
        /// </exception>
        public CalendarRegistry(Calendar[] calendars) : this(MinMinId, MaxMaxId)
        {
            InitializeFromSystemCalendars(calendars);
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

        /// <summary>
        /// Returns true if the registry is full; otherwise returns false.
        /// </summary>
        public bool IsFull => _lastId >= MaxId;

        // Disable fail fast. Only for testing.
        public bool DisableFailFast { get; set; }

        // _lastId is incremented very late in the registration process which
        // means that CanAdd may return true even if, in the end, it's not
        // truely the case. The upper limit has been reached but the process
        // has not yet completed. What's important is that any attempt to register
        // a new calendar will correctly fail. In a multi-threaded environment,
        // it is to be expected. In a single-threaded environment, the problem
        // does not exist.
        private bool CanAdd => DisableFailFast || _lastId < MaxId;

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
        /// Gets the raw number of calendars, including <i>dirty</i> calendars.
        /// </summary>
        public int RawCount => _calendarsByKey.Count;

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

        #region Initialization

        // It's the duty of CalendarCatalog to ensure that the members
        // of "calendars" are added to s_SystemCalendars and to s_Calendars.
        // In particular, we don't call CalendarCreated for user-defined calendars.
        // It makes sense since the callback is called CalendarCreated, not
        // CalendarAdded.
        //
        // Only call these methods from a ctor and only once.
        // If called twice, an initializer may mess up NumberOfSystemCalendars.
        // For the same reason, a system calendar cannot appear twice.

        private void InitializeFromSystemCalendars(Calendar[] calendars)
        {
            // In fact, it's the context in which this method is used
            // (minId = MinMinId) that implies that "calendars" can only contain
            // system calendars; be sure to read the comments in InitializeFromArray().
            Requires.NotNull(calendars);

            // In InitializeFromArray(), we check that chr.Id < MinId within the
            // loop. The following check does it once and for all, the reason
            // being that the index of a calendar in "calendars" is given by its ID.
            if (calendars.Length > MinMinId) Throw.Argument(nameof(calendars));

            for (int id = 0; id < calendars.Length; id++)
            {
                var chr = calendars[id];
                if ((int)chr.Id != id) Throw.Argument(nameof(calendars));

                _calendarsByKey[chr.Key] = new Lazy<Calendar>(chr);
                NumberOfSystemCalendars++;
            }
        }

#if false
        // FIXME(code): CountUserCalendars() will be wrong if "calendars"
        // contains user-defined calendars.
        private void InitializeFromArray(Calendar[] calendars)
        {
            Requires.NotNull(calendars);

            // First requirement.
            // For _lastId to work properly, all IDs >= MinId must stay free,
            // therefore a calendar in "calendars" MUST satisfy the
            // condition chr.Id < MinId:
            // - system calendars, their IDs being in [0, MinMinId - 1],
            //   they always satisfy the condition.
            // - user-defined calendars with an ID in [MinMinId, MinId] are
            //   also permitted.
            // NB: if MinId = MinMinId, then the condition is equivalent to:
            // no user-defined calendar is allowed.
            //
            // Second requirement.
            // A system calendar cannot appear more than one time, otherwise
            // NumberOfSystemCalendars will be wrong. A simple way to enforce
            // this is to require that the index of a system calendar in
            // "calendars" is given by its ID.
            for (int id = 0; id < calendars.Length; id++)
            {
                var chr = calendars[id];
                int cuid = (int)chr.Id;
                if (cuid >= MinId) Throw.Argument(nameof(calendars));
                if (chr.IsUserDefined == false && cuid != id) Throw.Argument(nameof(calendars));

                // Indexer instead of TryAdd(): unconditional add.
                _calendarsByKey[chr.Key] = new Lazy<Calendar>(chr);
                if (chr.IsUserDefined == false) NumberOfSystemCalendars++;
            }
        }

        // TODO(code): clean up.
        //private void InitializeSystemCalendars(Calendar[] calendars)
        //{
        //    Requires.NotNull(calendars);
        //    // While not strictly necessary, we shall verify that the index of a
        //    // calendar in "calendars" equals its ID.
        //    // The ID of the first created user-defined calendar is MinId.
        //    // If "count" is > MinId, MinId is de facto a valid ID for a system
        //    // calendars, therefore we will have at least two calendars with
        //    // the same ID. In fact, it's not possible to create a system
        //    // calendar with an ID >= MinMinId, but we don't know here what's
        //    // inside "calendars", we will have to wait for
        //    // InitializeSystemCalendars() for that.
        //    if (calendars.Length > MinMinId) Throw.Argument(nameof(calendars));

        //    for (int id = 0; id < calendars.Length; id++)
        //    {
        //        var chr = calendars[id];
        //        // The tests below ensure that the calendars array does not
        //        // contain any user-defined calendar and that the index of a
        //        // calendar in "calendars" is given by its ID.
        //        // As a side effect, a calendar cannot appear twice. In fact,
        //        // it would not matter if it was the case, as it does not
        //        // change the result, it is just a "waste" of resources.
        //        // WARNING: do not change the order of the checks below,
        //        // otherwise it's harder to achieve full code coverage.
        //        // Indeed, a user-defined calendar has an ID >= MinMinId
        //        // which therefore cannot match the index of an array whose
        //        // last index is < MinMinId.
        //        if (chr.IsUserDefined) Throw.Argument(nameof(calendars));
        //        if ((int)chr.Id != id) Throw.Argument(nameof(calendars));

        //        // Indexer instead of TryAdd(): unconditional add.
        //        _calendarsByKey[chr.Key] = new Lazy<Calendar>(chr);
        //        NumberOfSystemCalendars++;
        //    }
        //}
#endif

        #endregion
    }

    internal sealed partial class CalendarRegistry // Snapshots & Lookup
    {
        // We MUST filter out dirty calendars. As explained in CreateCalendar(),
        // a dirty calendar may always be referenced in _calendarsByKey.
        // Furthermore, we don't verify whether the removal of a dirty calendar
        // is successful or not; see the section "Clean up" in the "Add"-methods.

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
        /// <summary>
        /// Adds a calendar to the current instance.
        /// <para><i>For testing purposes only</i>.</para>
        /// <para>This method will unconditionally override an existing key.</para>
        /// <para>Beware, if you decide to use this method, you SHOULD NOT call a counting method
        /// afterwards. Only <see cref="RawCount"/> will continue to work properly.</para>
        /// </summary>
        public void AddRaw(Calendar calendar)
        {
            // Currently, we only use this method to add a user-defined calendar
            // with an invalid ID in order to be able to achieve full code coverage.

            Requires.NotNull(calendar);

            // After having called this method, counting methods no longer works.
            // For CountCalendars() and CountUserCalendars(), there is nothing
            // we can do about it. Don't try to fix NumberOfSystemCalendars either.
            // To do it would mean that we first check that the specified (system)
            // calendar is not already registered, something which is not that
            // straightforward to do in a thread-safe way.
            _calendarsByKey[calendar.Key] = new Lazy<Calendar>(calendar);
        }

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
