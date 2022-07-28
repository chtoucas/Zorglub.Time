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

    public sealed class CivilCalendar : BasicCalendar, ICalendar<CivilDate>
    {
        public CivilCalendar() : this(new CivilSchema()) { }

        public CivilCalendar(CivilSchema schema)
            : base("Gregorian", new StandardScope(schema, DayZero.NewStyle)) { }

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
        // Factories
        //

        /// <inheritdoc/>
        [Pure]
        CivilDate ICalendar<CivilDate>.Today() => CivilDate.Today();

        //
        // Dates in a given year or month
        //

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<CivilDate> GetDaysInYear(int year)
        {
            SupportedYears.Validate(year);

            int startOfYear = Schema.GetStartOfYear(year);
            int daysInYear = Schema.CountDaysInYear(year);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfYear, daysInYear)
                   select new CivilDate(daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<CivilDate> GetDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);

            int startOfMonth = Schema.GetStartOfMonth(year, month);
            int daysInMonth = Schema.CountDaysInMonth(year, month);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfMonth, daysInMonth)
                   select new CivilDate(daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public CivilDate GetStartOfYear(int year)
        {
            SupportedYears.Validate(year);
            int daysSinceEpoch = Schema.GetStartOfYear(year);
            return new CivilDate(daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public CivilDate GetEndOfYear(int year)
        {
            SupportedYears.Validate(year);
            int daysSinceEpoch = Schema.GetEndOfYear(year);
            return new CivilDate(daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public CivilDate GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetStartOfMonth(year, month);
            return new CivilDate(daysSinceEpoch);
        }

        /// <inheritdoc/>
        [Pure]
        public CivilDate GetEndOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetEndOfMonth(year, month);
            return new CivilDate(daysSinceEpoch);
        }
    }
}