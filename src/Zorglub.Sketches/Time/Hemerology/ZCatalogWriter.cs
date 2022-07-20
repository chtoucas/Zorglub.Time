// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;

    using Zorglub.Time.Hemerology.Scopes;

    internal sealed class ZCatalogWriter
    {
        /// <summary>
        /// Represents an invalid ID.
        /// <para>This field is a constant equal to 2_147_483_647.</para>
        /// </summary>
        private const int InvalidId = Int32.MaxValue;

        /// <summary>
        /// Represents the array of fully constructed calendars, indexed by their internal IDs.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ZCalendar?[] _calendarsById;

        /// <summary>
        /// Represents the dictionary of (lazy) calendars, indexed by their keys.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ConcurrentDictionary<string, Lazy<ZCalendar>> _calendarsByKey;

        private int _lastId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZCatalogWriter"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendarsByKey"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="calendarsById"/> is null.</exception>
        public ZCatalogWriter(
            ConcurrentDictionary<string, Lazy<ZCalendar>> calendarsByKey,
            ZCalendar?[] calendarsById,
            int minUserId)
        {
            Requires.NotNull(calendarsByKey);
            Requires.NotNull(calendarsById);

            MaxId = calendarsById.Length - 1;
            // We could have used an equality (InvalidId = Int32.MaxValue), but
            // an inequality is better because it will continue to work even if
            // we change the value of InvalidId.
            if (MaxId >= InvalidId) Throw.Argument(nameof(calendarsById));
            if (minUserId > MaxId) Throw.ArgumentOutOfRange(nameof(minUserId));

            _calendarsByKey = calendarsByKey;
            _calendarsById = calendarsById;

            MinUserId = minUserId;
            _lastId = minUserId - 1;
        }

        /// <summary>
        /// Gets the minimum value for the ID of a user-defined calendar.
        /// </summary>
        public int MinUserId { get; }

        /// <summary>
        /// Gets the maximun value for the ID of a calendar.
        /// </summary>
        public int MaxId { get; }

        // Only for testing.
        public ICollection<string> Keys => _calendarsByKey.Keys;

        // Only for testing.
        [Pure]
        public IEnumerable<ZCalendar> GetCurrentCalendars() =>
            from chr in _calendarsById where chr is not null select chr;

        /// <summary>
        /// Counts the number of user-defined calendars at the time of the request.
        /// </summary>
        // We use Math.Min() because CreateCalendar() might increment _lastId
        // one step too far.
        [Pure]
        public int CountUserCalendars() => Math.Min(_lastId, MaxId) - MinUserId + 1;

        [Pure]
        public ZCalendar GetOrAdd(string key, CalendarScope scope)
        {
            if (_lastId >= MaxId && _calendarsByKey.ContainsKey(key) == false)
            {
                Throw.CatalogOverflow();
            }

            var tmp = new TmpCalendar(key, scope);

            var lazy = new Lazy<ZCalendar>(() => CreateCalendar(tmp));
            var lazy1 = _calendarsByKey.GetOrAdd(key, lazy);

            var chr = lazy1.Value;
            if (chr is TmpCalendar)
            {
                if (ReferenceEquals(lazy1, lazy))
                {
                    _calendarsByKey.TryRemove(key, out _);
                }

                Throw.CatalogOverflow();
            }

            return chr;
        }

        [Pure]
        public ZCalendar Add(string key, CalendarScope scope)
        {
            if (_lastId >= MaxId) Throw.CatalogOverflow();

            var tmp = new TmpCalendar(key, scope);

            var lazy = new Lazy<ZCalendar>(() => CreateCalendar(tmp));

            if (_calendarsByKey.TryAdd(key, lazy) == false)
            {
                Throw.KeyAlreadyExists(nameof(key), key);
            }

            var chr = lazy.Value;
            if (chr is TmpCalendar)
            {
                _calendarsByKey.TryRemove(key, out _);

                Throw.CatalogOverflow();
            }

            return chr;
        }

        [Pure]
        public bool TryAdd(
            string key, CalendarScope scope,
            [NotNullWhen(true)] out ZCalendar? calendar)
        {
            if (_lastId >= MaxId) { goto FAILED; }

            Requires.NotNull(key);
            Requires.NotNull(scope);

            TmpCalendar tmp;
            try { tmp = new TmpCalendar(key, scope); }
            catch (ArgumentException) { goto FAILED; }

            var lazy = new Lazy<ZCalendar>(() => CreateCalendar(tmp));

            if (_calendarsByKey.TryAdd(key, lazy))
            {
                var chr = lazy.Value;
                if (chr is TmpCalendar)
                {
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

        [Pure]
        private ZCalendar CreateCalendar(TmpCalendar tmpCalendar)
        {
            Debug.Assert(tmpCalendar != null);

            int id = Interlocked.Increment(ref _lastId);
            if (id > MaxId) { return tmpCalendar; }

            var chr = new ZCalendar(
                id,
                tmpCalendar.Key,
                tmpCalendar.Scope,
                userDefined: true);

            return _calendarsById[id] = chr;
        }

        internal sealed class TmpCalendar : ZCalendar
        {
            public TmpCalendar(string key, CalendarScope scope)
                : base(InvalidId, key, scope, userDefined: true) { }

            [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Hides inherited member.")]
            internal new int Id => Throw.InvalidOperation<int>();
        }
    }
}
