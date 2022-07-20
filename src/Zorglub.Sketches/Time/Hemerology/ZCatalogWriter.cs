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

        private int _lastId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZCatalogWriter"/> class.
        /// </summary>
        public ZCatalogWriter(
            ConcurrentDictionary<string, Lazy<ZCalendar>> calendarsByKey,
            ZCalendar?[] calendarsById,
            int startId)
        {
            Debug.Assert(calendarsByKey != null);
            Debug.Assert(calendarsById != null);
            Debug.Assert(startId < InvalidId);

            CalendarsByKey = calendarsByKey;
            CalendarsById = calendarsById;

            MaxId = calendarsById.Length - 1;
            StartId = startId;
            _lastId = startId - 1;
        }

        /// <summary>
        /// Represents the array of fully constructed calendars, indexed by their internal IDs.
        /// <para>This field is read-only.</para>
        /// </summary>
        internal ZCalendar?[] CalendarsById { get; }

        /// <summary>
        /// Represents the dictionary of (lazy) calendars, indexed by their keys.
        /// <para>This field is read-only.</para>
        /// </summary>
        internal ConcurrentDictionary<string, Lazy<ZCalendar>> CalendarsByKey { get; }

        /// <summary>
        /// Represents the maximun value for the ident of a calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        internal int MaxId { get; }

        internal int StartId { get; }

        // Only for testing.
        internal ICollection<string> Keys => CalendarsByKey.Keys;

        // Only for testing.
        [Pure]
        internal IEnumerable<ZCalendar> GetAllCalendars() =>
            from chr in CalendarsById where chr is not null select chr;

        [Pure]
        public ZCalendar GetOrAdd(string key, CalendarScope scope)
        {
            if (_lastId >= MaxId && CalendarsByKey.ContainsKey(key) == false)
            {
                Throw.CatalogOverflow();
            }

            var tmp = new TmpCalendar(key, scope);

            var lazy = new Lazy<ZCalendar>(() => CreateCalendar(tmp));
            var lazy1 = CalendarsByKey.GetOrAdd(key, lazy);

            var chr = lazy1.Value;
            if (chr is TmpCalendar)
            {
                if (ReferenceEquals(lazy1, lazy))
                {
                    CalendarsByKey.TryRemove(key, out _);
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

            if (CalendarsByKey.TryAdd(key, lazy) == false)
            {
                Throw.KeyAlreadyExists(nameof(key), key);
            }

            var chr = lazy.Value;
            if (chr is TmpCalendar)
            {
                CalendarsByKey.TryRemove(key, out _);

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

            if (CalendarsByKey.TryAdd(key, lazy))
            {
                var chr = lazy.Value;
                if (chr is TmpCalendar)
                {
                    CalendarsByKey.TryRemove(key, out _);
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

            return CalendarsById[id] = chr;
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
