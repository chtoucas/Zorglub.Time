// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Represents a basic calendar with dates within a range of years.
    /// </summary>
    public class MinMaxYearCalendar : BasicCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public MinMaxYearCalendar(string name, MinMaxYearScope scope) : base(name, scope) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="scope"/> is not complete.</exception>
        public MinMaxYearCalendar(string name, CalendarScope scope) : base(name, scope)
        {
            Debug.Assert(scope != null);

            if (scope.IsComplete == false) Throw.Argument(nameof(scope));
        }

        /// <inheritdoc/>
        [Pure]
        public sealed override int CountMonthsInYear(int year)
        {
            YearsValidator.Validate(year);
            return Schema.CountMonthsInYear(year);
        }

        /// <inheritdoc/>
        [Pure]
        public sealed override int CountDaysInYear(int year)
        {
            YearsValidator.Validate(year);
            return Schema.CountDaysInYear(year);
        }

        /// <inheritdoc/>
        [Pure]
        public sealed override int CountDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return Schema.CountDaysInMonth(year, month);
        }
    }
}
