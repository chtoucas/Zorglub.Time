// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Represents a calendar with dates within a range of years.
    /// </summary>
    public sealed class MinMaxYearDayCalendar : MinMaxYearCalendar<DayNumber>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearDayCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="scope"/> is not complete.</exception>
        public MinMaxYearDayCalendar(string name, CalendarScope scope) : base(name, scope) { }

        /// <summary>
        /// Obtains the current day number on this machine.
        /// </summary>
        /// <exception cref="AoorException">Today is not within the calendar boundaries.</exception>
        [Pure]
        public DayNumber Today()
        {
            // We do not know in advance if today is valid.
            var today = DayNumber.Today();
            Domain.Validate(today);
            return today;
        }
    }
}
