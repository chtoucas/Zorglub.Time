// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas
{
    // Not known at the time of its creation, a similar proposal, under the name
    // "Georgian Calendar", was made by Rev. Hugh Jones but much earlier, in 1745.
    // See http://myweb.ecu.edu/mccartyr/hirossa.html and
    // https://en.wikipedia.org/wiki/Hugh_Jones_(professor).
    //
    // Main flaws: blank-days, 13 months.

    /// <summary>
    /// Represents the Positivist schema proposed by Auguste Comte (1849).
    /// <para>The Positivist calendar is a blank-day calendar using a 13-months
    /// schema of identical length (28 days).</para>
    /// <para>The blank-days are added after December: first "The Festival of
    /// All the Dead" (la Fête universelle des MORTS), then "The Festival of
    /// Holy Women" (la Fête générale des SAINTES FEMMES) on leap years.</para>
    /// <para>This class cannot be inherited.</para>
    /// <para>This class can ONLY be initialized from within friend assemblies.
    /// </para>
    /// </summary>
    /// <remarks>
    /// For technical reasons, the blank-days are attached to the month
    /// preceding them.
    /// </remarks>
    public sealed partial class PositivistSchema :
        SystemSchema,
        IRegularSchema,
        IBlankDayFeaturette,
        IBoxable<PositivistSchema>
    {
        /// <summary>
        /// Represents the number of months in a year.
        /// <para>This field is a constant equal to 13.</para>
        /// </summary>
        private const int MonthsPerYear = 13;

        /// <summary>
        /// Represents the number of days per 400-year cycle.
        /// <para>This field is a constant equal to 146_097.</para>
        /// </summary>
        /// <remarks>
        /// On average, a year is 365.2425 days long.
        /// </remarks>
        public const int DaysPer400YearCycle = GregorianSchema.DaysPer400YearCycle;

        /// <summary>
        /// Represents the number of days in a common year.
        /// <para>This field is a constant equal to 365.</para>
        /// </summary>
        public const int DaysInCommonYear = GJSchema.DaysInCommonYear;

        /// <summary>
        /// Represents the number of days in a leap year.
        /// <para>This field is a constant equal to 366.</para>
        /// </summary>
        public const int DaysInLeapYear = DaysInCommonYear + 1;

        /// <summary>
        /// Represents the genuine number of days in a month (excluding the
        /// blank days that are formally outside any month).
        /// <para>This field is constant equal to 28.</para>
        /// <para>See also <seealso cref="CountDaysInMonth(int, int)"/>.</para>
        /// </summary>
        public const int DaysInPositivistMonth = 28;

        /// <summary>
        /// Initializes a new instance of the <see cref="PositivistSchema"/>
        /// class.
        /// </summary>
        internal PositivistSchema() : base(DaysInCommonYear, 28) { }

        /// <inheritdoc />
        public sealed override CalendricalFamily Family => CalendricalFamily.Solar;

        /// <inheritdoc />
        public sealed override CalendricalAdjustments PeriodicAdjustments => CalendricalAdjustments.Days;

        /// <inheritdoc />
        public int MonthsInYear => MonthsPerYear;

        /// <summary>
        /// Creates a new (boxed) instance of the <see cref="PositivistSchema"/>
        /// class.
        /// </summary>
        [Pure]
        public static Box<PositivistSchema> GetInstance() => Box.Create(new PositivistSchema());
    }

    public partial class PositivistSchema // Year, month or day infos
    {
        /// <inheritdoc />
        [Pure]
        public static bool IsBlankDay(int y, int m, int d) => d > 28;

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsLeapYear(int y) => GregorianFormulae.IsLeapYear(y);

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsIntercalaryMonth(int y, int m) => false;

        /// <inheritdoc />
        //
        // Il n'est pas nécessaire de vérifier le mois car c'est le seul
        // jour numéroté 30.
        [Pure]
        public sealed override bool IsIntercalaryDay(int y, int m, int d) => d == 30;

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsSupplementaryDay(int y, int m, int d) => d > 28;
    }

    public partial class PositivistSchema // Counting months and days within a year or a month
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int CountMonthsInYear(int y) => MonthsPerYear;

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInYear(int y) =>
            GregorianFormulae.IsLeapYear(y) ? DaysInLeapYear : DaysInCommonYear;

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInYearBeforeMonth(int y, int m) => 28 * (m - 1);

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInMonth(int y, int m) =>
            m == 13 ? 28 + (GregorianFormulae.IsLeapYear(y) ? 2 : 1) : 28;
    }

    public partial class PositivistSchema // Conversions
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int CountMonthsSinceEpoch(int y, int m) =>
            IRegularSchema.Thirteen.CountMonthsSinceEpoch(y, m);

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysSinceEpoch(int y, int m, int d) =>
            GregorianFormulae.GetStartOfYear(y) + 28 * (m - 1) + d - 1;

        /// <inheritdoc />
        public sealed override void GetMonthParts(int monthsSinceEpoch, out int y, out int m) =>
            IRegularSchema.Thirteen.GetMonthParts(monthsSinceEpoch, out y, out m);

        /// <inheritdoc />
        [Pure]
        public sealed override int GetMonth(int y, int doy, out int d)
        {
            int m;
            if (doy > 336)
            {
                m = 13;
                d = doy - 336;
            }
            else
            {
                int d0y = doy - 1;
                m = 1 + d0y / 28;
                d = 1 + d0y % 28;
            }
            return m;
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int GetYear(int daysSinceEpoch) =>
            GregorianFormulae.GetYear(daysSinceEpoch);
    }

    public partial class PositivistSchema // Dates in a given year or month
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int GetStartOfYear(int y) => GregorianFormulae.GetStartOfYear(y);

        /// <inheritdoc />
        public sealed override void GetDatePartsAtEndOfYear(int y, out int m, out int d)
        {
            m = MonthsPerYear;
            d = GregorianFormulae.IsLeapYear(y) ? 30 : 29;
        }
    }
}
