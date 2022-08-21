// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology;

    public sealed class TodayProvider
    {
        private readonly SimpleCalendar _calendar;
        private readonly ITodayProvider _provider;

        public TodayProvider(SimpleCalendar calendar, ITodayProvider provider)
        {
            _calendar = calendar ?? throw new ArgumentNullException(nameof(provider));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
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
        /// Obtains the current <see cref="CalendarYear"/> on this machine,
        /// expressed in local time, not UTC.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by this
        /// calendar.</exception>
        [Pure]
        public CalendarYear GetCurrentYear()
        {
            var today = _provider.Today();
            if (IsUserDefined) { Domain.Validate(today); }
            int y = Schema.GetYear(today - Epoch);
            return new CalendarYear(y, Id);
        }

        /// <summary>
        /// Obtains the current <see cref="CalendarMonth"/> on this machine,
        /// expressed in local time, not UTC.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by this
        /// calendar.</exception>
        [Pure]
        public CalendarMonth GetCurrentMonth()
        {
            var today = _provider.Today();
            if (IsUserDefined) { Domain.Validate(today); }
            var ymd = Schema.GetDateParts(today - Epoch);
            return new CalendarMonth(ymd.Yemo, Id);
        }

        /// <summary>
        /// Obtains the current <see cref="CalendarDate"/> on this machine,
        /// expressed in local time, not UTC.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by this
        /// calendar.</exception>
        [Pure]
        public CalendarDate GetCurrentDate()
        {
            var today = _provider.Today();
            if (IsUserDefined) { Domain.Validate(today); }
            var ymd = Schema.GetDateParts(today - Epoch);
            return new CalendarDate(ymd, Id);
        }

        /// <summary>
        /// Obtains the current <see cref="CalendarDay"/> on this machine,
        /// expressed in local time, not UTC.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by this
        /// calendar.</exception>
        [Pure]
        public CalendarDay GetCurrentDay()
        {
            var today = _provider.Today();
            if (IsUserDefined) { Domain.Validate(today); }
            return new(today - Epoch, Id);
        }

        /// <summary>
        /// Obtains the current <see cref="OrdinalDate"/> on this machine,
        /// expressed in local time, not UTC.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by this
        /// calendar.</exception>
        [Pure]
        public OrdinalDate GetCurrentOrdinal()
        {
            var today = _provider.Today();
            if (IsUserDefined) { Domain.Validate(today); }
            var ydoy = Schema.GetOrdinalParts(today - Epoch);
            return new OrdinalDate(ydoy, Id);
        }
    }
}
