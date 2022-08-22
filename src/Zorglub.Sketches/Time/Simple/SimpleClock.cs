// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Horology;

    public static class SimpleCalendarExtensions
    {
        public static SimpleClock GetLocalClock(this SimpleCalendar calendar) =>
            new(calendar, SystemLocalTimepiece.Instance);

        public static SimpleClock GetUtcClock(this SimpleCalendar calendar) =>
            new(calendar, SystemUtcTimepiece.Instance);
    }

    public sealed class SimpleClock
    {
        private readonly SimpleCalendar _calendar;
        private readonly ITimepiece _clock;

        public SimpleClock(SimpleCalendar calendar, ITimepiece clock)
        {
            _calendar = calendar ?? throw new ArgumentNullException(nameof(clock));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        /// <summary>
        /// Gets the ID of the current instance.
        /// </summary>
        private Cuid Id => _calendar.Id;

        /// <summary>
        /// Gets the calendrical schema.
        /// </summary>
        private SystemSchema Schema => _calendar.Schema;

        /// <summary>
        /// Returns true if the current instance is user-defined; otherwise returns false.
        /// </summary>
        private bool IsUserDefined => _calendar.IsUserDefined;

        private DayNumber Epoch => _calendar.Epoch;
        private Range<DayNumber> Domain => _calendar.Domain;

        // Identical to the conversion methods found in SimpleCalendar but
        // without the validation part for system calendars as we know for sure
        // that today is always admissible.

        /// <summary>
        /// Obtains a <see cref="CalendarYear"/> value representing the current year.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by the
        /// underlying calendar.</exception>
        [Pure]
        public CalendarYear GetCurrentYear()
        {
            var today = _clock.Today();
            if (IsUserDefined) { Domain.Validate(today); }
            int y = Schema.GetYear(today - Epoch);
            return new CalendarYear(y, Id);
        }

        /// <summary>
        /// Obtains a <see cref="CalendarMonth"/> value representing the current month.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by the
        /// underlying calendar.</exception>
        [Pure]
        public CalendarMonth GetCurrentMonth()
        {
            var today = _clock.Today();
            if (IsUserDefined) { Domain.Validate(today); }
            var ymd = Schema.GetDateParts(today - Epoch);
            return new CalendarMonth(ymd.Yemo, Id);
        }

        /// <summary>
        /// Obtains a <see cref="CalendarDate"/> value representing the current date.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by the
        /// underlying calendar.</exception>
        [Pure]
        public CalendarDate GetCurrentDate()
        {
            var today = _clock.Today();
            if (IsUserDefined) { Domain.Validate(today); }
            var ymd = Schema.GetDateParts(today - Epoch);
            return new CalendarDate(ymd, Id);
        }

        /// <summary>
        /// Obtains a <see cref="CalendarDay"/> value representing the current date.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by the
        /// underlying calendar.</exception>
        [Pure]
        public CalendarDay GetCurrentDay()
        {
            var today = _clock.Today();
            if (IsUserDefined) { Domain.Validate(today); }
            return new(today - Epoch, Id);
        }

        /// <summary>
        /// Obtains a <see cref="OrdinalDate"/> value representing the current date.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by the
        /// underlying calendar.</exception>
        [Pure]
        public OrdinalDate GetCurrentOrdinal()
        {
            var today = _clock.Today();
            if (IsUserDefined) { Domain.Validate(today); }
            var ydoy = Schema.GetOrdinalParts(today - Epoch);
            return new OrdinalDate(ydoy, Id);
        }
    }
}
