// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas
{
    /// <summary>
    /// Provides a base for the "Tropicália" schemas.
    /// <para>This class can ONLY be initialized from within friend assemblies.</para>
    /// </summary>
    public abstract partial class TropicalistaSchema : SystemSchema, IRegularSchema
    {
        /// <summary>
        /// Represents the number of months in a year.
        /// <para>This field is a constant equal to 12.</para>
        /// </summary>
        private const int MonthsPerYear = 12;

        /// <summary>
        /// Represents the number of days per 128-year cycle.
        /// <para>This field is a constant equal to 46_751.</para>
        /// </summary>
        /// <remarks>
        /// On average, a year is 365.2421875 days long.
        /// </remarks>
        public const int DaysPer128YearCycle = 128 * DaysInCommonYear + 31;

        /// <summary>
        /// Represents the <i>average</i> number of days per 4-year subcycle.
        /// <para>This field is a constant equal to 1461.</para>
        /// </summary>
        /// <remarks>
        /// On average, a year is 365.25 days long.
        /// </remarks>
        public const int DaysPer4YearSubcycle = CalendricalConstants.DaysPer4JulianYearCycle;

        /// <summary>
        /// Represents the number of days in a common year.
        /// <para>This field is a constant equal to 365.</para>
        /// </summary>
        public const int DaysInCommonYear = CalendricalConstants.DaysInWanderingYear;

        /// <summary>
        /// Represents the number of days in a leap year.
        /// <para>This field is a constant equal to 366.</para>
        /// </summary>
        public const int DaysInLeapYear = DaysInCommonYear + 1;

        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="TropicalistaSchema"/> class.
        /// </summary>
        private protected TropicalistaSchema(int minDaysInMonth) : base(DaysInCommonYear, minDaysInMonth) { }

        /// <inheritdoc />
        public sealed override CalendricalFamily Family => CalendricalFamily.Solar;

        /// <inheritdoc />
        public sealed override CalendricalAdjustments PeriodicAdjustments =>
            CalendricalAdjustments.Days;

        /// <inheritdoc />
        public int MonthsInYear => MonthsPerYear;
    }

    public partial class TropicalistaSchema // Year, month or day infos
    {
        /// <summary>
        /// Determines whether the specified year is leap or not.
        /// </summary>
        [Pure]
        // CIL code size = 15 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // NB: year zero is not leap.
        internal static bool IsLeapYearImpl(int y) => (y & 3) == 0 && (y & 127) != 0;

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsLeapYear(int y) => IsLeapYearImpl(y);

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsIntercalaryMonth(int y, int m) => false;

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsSupplementaryDay(int y, int m, int d) => false;
    }

    public partial class TropicalistaSchema // Counting months and days within a year or a month
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int CountMonthsInYear(int y) => MonthsPerYear;

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInYear(int y) =>
            IsLeapYearImpl(y) ? DaysInLeapYear : DaysInCommonYear;
    }

    public partial class TropicalistaSchema // Conversions
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int CountMonthsSinceEpoch(int y, int m) =>
            MonthsCalculator.Regular12.CountMonthsSinceEpoch(y, m);

        /// <inheritdoc />
        public sealed override void GetMonthParts(int monthsSinceEpoch, out int y, out int m) =>
            MonthsCalculator.Regular12.GetMonthParts(monthsSinceEpoch, out y, out m);

        /// <inheritdoc />
        [Pure]
        public sealed override int GetYear(int daysSinceEpoch)
        {
            int C = MathZ.Divide(daysSinceEpoch, DaysPer128YearCycle, out int D);
            return 1 + (C << 7) + ((D << 2) + 3) / DaysPer4YearSubcycle;
        }
    }

    public partial class TropicalistaSchema // Dates in a given year or month
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int GetStartOfYear(int y)
        {
            y--;
            return 365 * y + (y >> 2) - (y >> 7);
        }
    }
}
