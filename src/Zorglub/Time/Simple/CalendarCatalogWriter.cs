// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Collections.Concurrent;
    using System.Threading;

    using Zorglub.Time.Core;

    internal sealed class CalendarCatalogWriter
    {
        /// <summary>
        /// Represents the array of fully constructed calendars, indexed by their internal IDs.
        /// <para>A value is null if the calendar has not yet been fully constructed (obviously).
        /// </para>
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly Calendar?[] _calendarsById;

        /// <summary>
        /// Represents the dictionary of (lazy) calendars, indexed by their keys.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ConcurrentDictionary<string, Lazy<Calendar>> _calendarsByKey;

        /// <summary>
        /// Represents the absolute maximun value for the ID of a calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _maxIdent;

        private int _lastIdent;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarCatalogWriter"/> class.
        /// </summary>
        public CalendarCatalogWriter(
            ConcurrentDictionary<string, Lazy<Calendar>> calendarsByKey,
            Calendar?[] calendarsById,
            int startIdent)
        {
            Debug.Assert(calendarsByKey != null);
            Debug.Assert(calendarsById != null);
            Debug.Assert(startIdent < (int)Cuid.Invalid);

            _calendarsByKey = calendarsByKey;
            _calendarsById = calendarsById;

            _maxIdent = calendarsById.Length - 1;
            _lastIdent = startIdent - 1;
        }

        public int LastIdent => _lastIdent;

        [Pure]
        public Calendar GetOrAdd(string key, SystemSchema schema, DayNumber epoch, bool proleptic)
        {
            // Fail fast. It also guards the method against brute-force attacks.
            // This should only be done when the key is not already taken.
            if (_lastIdent >= _maxIdent && _calendarsByKey.ContainsKey(key) == false)
            {
                Throw.CatalogOverflow();
            }

            Calendar tmp = ValidateParameters(key, schema, epoch, proleptic);

            // The callback won't be executed until we query Value.
            var lazy = new Lazy<Calendar>(() => CreateCalendar(tmp));

            var lazy1 = _calendarsByKey.GetOrAdd(key, lazy);

            // If the key is already taken, obviously lazy1.Value returns the
            // pre-existing calendar, but the important point here is that
            // CreateCalendar() was not called and therefore s_LastIdent did
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
            if (_lastIdent >= _maxIdent) Throw.CatalogOverflow();

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
            if (_lastIdent >= _maxIdent) { goto FAILED; }

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
            //   En conséquence, s_LastIdent ne sera incrémenté qu'une fois.
            //   Bien entendu, avec deux clés différentes, le problème ne se
            //   pose pas.
            int ident = Interlocked.Increment(ref _lastIdent);
            if (ident > _maxIdent)
            {
                // We don't throw an OverflowException yet.
                // This will give the caller the opportunity to do some cleanup.
                // Objects of this type only exist for a very short period of
                // time within <see cref="s_CalendarsByKey"/> and MUST be
                // filtered out anytime we query a value from this dictionary.
                return tmpCalendar;
            }

            Debug.Assert(ident <= (int)Cuid.Max);

            // Notes:
            // - the cast to Cuid should always succeed.
            // - the ctor won't throw since we already validated the params
            //   when we created tmpCalendar.
            var chr = new Calendar(
                (Cuid)ident,
                tmpCalendar.Key,
                tmpCalendar.Schema,
                tmpCalendar.Epoch,
                tmpCalendar.IsProleptic);

            Debug.Assert(ident < _calendarsById.Length);

            return _calendarsById[ident] = chr;
        }
    }
}
