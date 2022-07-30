// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System.Diagnostics.Contracts;

    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Represents a basic calendar with dates within a range of years.
    /// </summary>
    public class MinMaxYearCalendar : BasicCalendar
    {
        public MinMaxYearCalendar(string name, CalendarScope scope) : base(name, scope)
        {
            Debug.Assert(scope != null);

            if (scope.IsComplete == false) Throw.Argument(nameof(scope));
        }

        /// <inheritdoc/>
        [Pure]
        public sealed override int CountMonthsInYear(int year)
        {
            SupportedYears.Validate(year);
            return Schema.CountMonthsInYear(year);
        }

        /// <inheritdoc/>
        [Pure]
        public sealed override int CountDaysInYear(int year)
        {
            SupportedYears.Validate(year);
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
