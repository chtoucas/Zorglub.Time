// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Zorglub.Time;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Hemerology.Scopes;

    public sealed class GregorianCalendar : BasicCalendar, ICalendar<GregorianDate>
    {
        public GregorianCalendar() : this(new GregorianSchema()) { }

        // Constructor for GregorianDate.
        internal GregorianCalendar(GregorianSchema schema)
            : base("Gregorian", MinMaxYearScope.WithMaximalRange(schema, DayZero.NewStyle)) { }

        //
        // Year, month or day infos
        //

        /// <inheritdoc/>
        [Pure]
        public override int CountMonthsInYear(int year)
        {
            SupportedYears.Validate(year);
            return Schema.CountMonthsInYear(year);
        }

        /// <inheritdoc/>
        [Pure]
        public override int CountDaysInYear(int year)
        {
            SupportedYears.Validate(year);
            return Schema.CountDaysInYear(year);
        }

        /// <inheritdoc/>
        [Pure]
        public override int CountDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return Schema.CountDaysInMonth(year, month);
        }

        //
        // Dates in a given year or month
        //

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<GregorianDate> GetDaysInYear(int year)
        {
            SupportedYears.Validate(year);

            int startOfYear = Schema.GetStartOfYear(year);
            int daysInYear = Schema.CountDaysInYear(year);

            return from daysSinceZero
                   in Enumerable.Range(startOfYear, daysInYear)
                   select new GregorianDate(daysSinceZero);
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<GregorianDate> GetDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);

            int startOfMonth = Schema.GetStartOfMonth(year, month);
            int daysInMonth = Schema.CountDaysInMonth(year, month);

            return from daysSinceZero
                   in Enumerable.Range(startOfMonth, daysInMonth)
                   select new GregorianDate(daysSinceZero);
        }

        /// <inheritdoc/>
        [Pure]
        public GregorianDate GetStartOfYear(int year)
        {
            SupportedYears.Validate(year);
            int daysSinceZero = Schema.GetStartOfYear(year);
            return new GregorianDate(daysSinceZero);
        }

        /// <inheritdoc/>
        [Pure]
        public GregorianDate GetEndOfYear(int year)
        {
            SupportedYears.Validate(year);
            int daysSinceZero = Schema.GetEndOfYear(year);
            return new GregorianDate(daysSinceZero);
        }

        /// <inheritdoc/>
        [Pure]
        public GregorianDate GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceZero = Schema.GetStartOfMonth(year, month);
            return new GregorianDate(daysSinceZero);
        }

        /// <inheritdoc/>
        [Pure]
        public GregorianDate GetEndOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceZero = Schema.GetEndOfMonth(year, month);
            return new GregorianDate(daysSinceZero);
        }
    }
}
