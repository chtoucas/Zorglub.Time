// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System.Collections.Concurrent;
    using System.Threading;

    using Zorglub.Time.Hemerology.Scopes;

    internal sealed partial class ZCatalogWriter
    {
        /// <summary>
        /// Represents an invalid ID.
        /// <para>This field is a constant equal to 2_147_483_647.</para>
        /// </summary>
        private const int InvalidId = Int32.MaxValue;

        /// <summary>
        /// Represents the maximun value for the ident of a calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _maxId;

        /// <summary>This field is initially set to 127.</summary>
        private int _lastId;

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

        public ZCatalogWriter(
            ConcurrentDictionary<string, Lazy<ZCalendar>> calendarsByKey,
            ZCalendar?[] calendarsById,
            int startId)
        {
            Debug.Assert(calendarsByKey != null);
            Debug.Assert(calendarsById != null);

            _calendarsByKey = calendarsByKey;
            _calendarsById = calendarsById;

            _maxId = calendarsById.Length - 1;
            _lastId = startId - 1;
        }
    }

    internal partial class ZCatalogWriter
    {
        [Pure]
        public ZCalendar GetOrAdd(string key, CalendarScope scope)
        {
            if (_lastId >= _maxId && _calendarsByKey.ContainsKey(key) == false)
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
            if (_lastId >= _maxId) Throw.CatalogOverflow();

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
            if (_lastId >= _maxId) { goto FAILED; }

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
            if (id > _maxId) { return tmpCalendar; }

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
