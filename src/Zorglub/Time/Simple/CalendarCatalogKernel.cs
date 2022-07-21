// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;

    using Zorglub.Time.Core;

    internal sealed partial class CalendarCatalogKernel
    {
        /// <summary>
        /// Represents the absolute maximun value for <see cref="MaxId"/>.
        /// <para>This field is a constant equal to 254.</para>
        /// </summary>
        /// <remarks>
        /// This upper limit exists to ensure that a calendar with ID = Cuid.Invalid cannot be added
        /// to the array of calendars (calendarsById).
        /// </remarks>
        public const int MaxMaxId = (int)Cuid.Invalid - 1;

        /// <summary>
        /// Represents the minimum value for <see cref="MinUserId"/>.
        /// <para>This field is a constant equal to 64.</para>
        /// </summary>
        /// <remarks>
        /// This lower limit exists to ensure that a user-defined calendar with a system ID cannot
        /// be added to the array of calendars (calendarsById).
        /// </remarks>
        public const int MinMinUserId = (int)Cuid.MinUser;

        /// <summary>
        /// Represents the array of fully constructed calendars, indexed by their IDs.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly Calendar?[] _calendarsById;

        /// <summary>
        /// Represents the dictionary of (lazy) calendars, indexed by their keys.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ConcurrentDictionary<string, Lazy<Calendar>> _calendarsByKey;

        private int _lastId;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarCatalogKernel"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendarsByKey"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="calendarsById"/> is null.</exception>
        /// <exception cref="ArgumentException">The size of <paramref name="calendarsById"/> is not
        /// within the range [(1 + <see cref="MinMinUserId"/>)..(1 + <see cref="MaxMaxId"/>)].</exception>
        /// <exception cref="AoorException"><paramref name="minUserId"/> is not
        /// within the range [<see cref="MinMinUserId"/>..<see cref="MaxId"/>].</exception>
        public CalendarCatalogKernel(
            ConcurrentDictionary<string, Lazy<Calendar>> calendarsByKey,
            Calendar?[] calendarsById,
            int minUserId)
        {
            Requires.NotNull(calendarsByKey);
            Requires.NotNull(calendarsById);

            int size = calendarsById.Length;
            if (size < 1 + MinMinUserId || size > 1 + MaxMaxId) Throw.Argument(nameof(calendarsById));
            MaxId = size - 1;

            if (minUserId < MinMinUserId || minUserId > MaxId) Throw.ArgumentOutOfRange(nameof(minUserId));
            MinUserId = minUserId;

            _calendarsByKey = calendarsByKey;
            _calendarsById = calendarsById;

            _lastId = minUserId - 1;
        }

        /// <summary>
        /// Gets the minimum value for the ID of a user-defined calendar.
        /// </summary>
        public int MinUserId { get; }

        /// <summary>
        /// Gets the absolute maximun value for the ID of a calendar.
        /// </summary>
        public int MaxId { get; }

        ///// <summary>
        ///// Returns true if XXX; otherwise returns false.
        ///// </summary>
        //public bool Is => _lastId < MinUserId;

        /// <summary>
        /// Gets the list of keys of all fully constructed calendars at the time of the request.
        /// <para>This collection may also contain a few bad keys, those paired with a calendar with
        /// an ID equal to <see cref="Cuid.Invalid"/>.</para>
        /// </summary>
        public ICollection<string> Keys => _calendarsByKey.Keys;

        /// <summary>
        /// Gets the absolute maximun number of user-defined calendars.
        /// </summary>
        public int MaxNumberOfUserCalendars => MaxId - MinUserId + 1;

        /// <summary>
        /// Counts the number of user-defined calendars at the time of the request.
        /// </summary>
        // We use Math.Min() because CreateCalendar() might increment _lastId
        // one step too far.
        [Pure]
        public int CountUserCalendars() => Math.Min(_lastId, MaxId) - MinUserId + 1;
    }

    internal sealed partial class CalendarCatalogKernel // Lookup
    {
        [Pure]
        public Calendar GetCalendarByKey(string key)
        {
            if (_calendarsByKey.TryGetValue(key, out Lazy<Calendar>? calendar))
            {
                var chr = calendar.Value;
                return chr.Id == Cuid.Invalid ? Throw.KeyNotFound<Calendar>(key) : chr;
            }

            return Throw.KeyNotFound<Calendar>(key);
        }
    }

    internal sealed partial class CalendarCatalogKernel
    {
        [Pure]
        public Calendar GetOrAdd(string key, SystemSchema schema, DayNumber epoch, bool proleptic)
        {
            // Fail fast. It also guards the method against brute-force attacks.
            // This should only be done when the key is not already taken, in
            // which case we return the calendar having this key.
            if (_lastId >= MaxId && _calendarsByKey.ContainsKey(key) == false)
            {
                Throw.CatalogOverflow();
            }

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
                // Beware, it is possible to end up here even if lazy1.Value
                // returned a pre-existing calendar. Indeed, there is a very
                // short window of time during which a TmpCalendar is still
                // referenced by s_CalendarsByKey.
                // The next check ensures that we only try to unlist the local
                // instance of TmpCalendar.
                // Actually, this is not mandatory, we could try to remove the
                // same key twice...
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
            if (_lastId >= MaxId) Throw.CatalogOverflow();

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
            if (_lastId >= MaxId) { goto FAILED; }

            // Null parameters validation.
            Requires.NotNull(key);
            Requires.NotNull(schema);

            // Other parameters validation (exceptions catched).
            Calendar tmp;
            try { tmp = ValidateParameters(key, schema, epoch, proleptic); }
            catch (ArgumentException) { goto FAILED; }

            // The callback won't be executed until we query Value.
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
            // FIXME(code): Invalid was a bad idea. If the Calendar ctor check
            // the value, it will fail hard. It would be the case if for
            // instance we initialized a date,
            // > var date = new CalendarDate(..., id);
            return new Calendar(Cuid.Invalid, key, schema, epoch, proleptic);
        }

        /// <summary>
        /// Creates a new <see cref="Calendar"/> instance from the specified temporary calendar then
        /// add it to <see cref="_calendarsById"/>.
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
                // time within <see cref="s_CalendarsByKey"/> and MUST be
                // filtered out anytime we query a value from this dictionary.
                return tmpCalendar;
            }

            Debug.Assert(id <= (int)Cuid.Max);

            // Notes:
            // - the cast to Cuid should always succeed.
            // - the ctor won't throw since we already validated the params
            //   when we created tmpCalendar.
            var chr = new Calendar(
                (Cuid)id,
                tmpCalendar.Key,
                tmpCalendar.Schema,
                tmpCalendar.Epoch,
                tmpCalendar.IsProleptic);

            Debug.Assert(id < _calendarsById.Length);

            return _calendarsById[id] = chr;
        }
    }
}
