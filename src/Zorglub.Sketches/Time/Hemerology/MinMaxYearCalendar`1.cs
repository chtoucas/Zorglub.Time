// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Zorglub.Time.Hemerology.Scopes;

    // REVIEW(code): each time we call TDate.FromDayNumber() we re-validate the
    // input, which is very unefficient. Can we change that?

    public class MinMaxYearCalendar<TDate> : BasicCalendar, ICalendar<TDate>
        where TDate : IFixedDay<TDate>
    {
        public MinMaxYearCalendar(string name, CalendarScope scope) : base(name, scope)
        {
            Debug.Assert(scope != null);

            if (scope.IsComplete == false) Throw.Argument(nameof(scope));
        }

        //
        // Year, month or day infos
        //

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

        //
        // Dates in a given year or month
        //

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<TDate> GetDaysInYear(int year)
        {
            SupportedYears.Validate(year);

            int startOfYear = Schema.GetStartOfYear(year);
            int daysInYear = Schema.CountDaysInYear(year);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfYear, daysInYear)
                   select TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<TDate> GetDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);

            int startOfMonth = Schema.GetStartOfMonth(year, month);
            int daysInMonth = Schema.CountDaysInMonth(year, month);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfMonth, daysInMonth)
                   select TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public TDate GetStartOfYear(int year)
        {
            SupportedYears.Validate(year);
            int daysSinceEpoch = Schema.GetStartOfYear(year);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public TDate GetEndOfYear(int year)
        {
            SupportedYears.Validate(year);
            int daysSinceEpoch = Schema.GetEndOfYear(year);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public TDate GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetStartOfMonth(year, month);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public TDate GetEndOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetEndOfMonth(year, month);
            return TDate.FromDayNumber(Epoch + daysSinceEpoch);
        }
    }
}