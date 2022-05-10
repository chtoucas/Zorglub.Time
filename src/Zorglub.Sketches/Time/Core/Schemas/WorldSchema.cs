// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas
{
    // World calendar or Universal calendar, attempt to improve on the
    // fixed calendars which themselves descents from the Positivist (Georgian)
    // calendar.
    // Four identical quarters: 91 = 31+30+30 days, 13 weeks, 3 months.
    // Each year begins on Sunday.
    // Each quarter begins on Sunday, ends on Saturday.
    // Promoted by Camille Flammarion in France, and lobbied by Elisabeth Achelis
    // to be adopted by the United Nations.
    // See http://myweb.ecu.edu/mccartyr/world-calendar.html
    //
    // Main flaws: blank-days, position of the intercalary day.

    /// <summary>
    /// Represents the World schema proposed by Gustave Armelin and Émile Manin (1887), and
    /// Elisabeth Achelis (1930).
    /// <para>The world calendar is a blank-day calendar using a 12-months schema with four
    /// identical quarters: one month of 31 days followed by two months of 30 days.</para>
    /// <para>The two blank-days are the Leapyear Day following June on leap years, and the
    /// Worldsday following December.</para>
    /// <para>This class cannot be inherited.</para>
    /// <para>This class can ONLY be initialized from within friend assemblies.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>For technical reasons, the blank-days are attached to the month preceding them.</para>
    /// </remarks>
    public sealed partial class WorldSchema :
        SystemSchema,
        IRegularSchema,
        IBlankDayFeaturette,
        IBoxable<WorldSchema>
    {
        /// <summary>
        /// Represents the number of months in a year.
        /// <para>This field is a constant equal to 12.</para>
        /// </summary>
        private const int MonthsPerYear = 12;

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
        /// Initializes a new instance of the <see cref="WorldSchema"/> class.
        /// </summary>
        private WorldSchema() : base(DaysInCommonYear, 30) { }

        /// <summary>
        /// Gets a singleton instance of the <see cref="WorldSchema"/> class.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        internal static WorldSchema Instance { get; } = new();

        /// <inheritdoc />
        public sealed override CalendricalFamily Family => CalendricalFamily.Solar;

        /// <inheritdoc />
        public sealed override CalendricalAdjustments PeriodicAdjustments => CalendricalAdjustments.Days;

        /// <inheritdoc />
        public int MonthsInYear => MonthsPerYear;

        /// <summary>
        /// Creates a new (boxed) instance of the <see cref="WorldSchema"/> class.
        /// </summary>
        [Pure]
        public static Box<WorldSchema> GetInstance() => Box.Create(new WorldSchema());

        /// <summary>
        /// Obtains the genuine number of days in a month (excluding the blank days that are
        /// formally outside any month).
        /// <para>See also <seealso cref="CountDaysInMonth(int, int)"/>.</para>
        /// </summary>
        [Pure]
        internal static int CountDaysInWorldMonth(int m) =>
            // m = 1, 4, 7, 10              -> 31 days
            // m = 2, 3, 5, 6, 8, 9, 11, 12 -> 30 days
            (m - 1) % 3 == 0 ? 31 : 30;
    }

    // Year, month or day infos.
    public partial class WorldSchema
    {
        /// <inheritdoc />
        [Pure]
        public static bool IsBlankDay(int y, int m, int d) =>
            d == 31 && (m == 6 || m == 12);

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsLeapYear(int y) => GregorianFormulae.IsLeapYear(y);

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsIntercalaryMonth(int y, int m) => false;

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsIntercalaryDay(int y, int m, int d) =>
            // We check the day first since it is the rarest case.
            d == 31 && m == 6;

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsSupplementaryDay(int y, int m, int d) => IsBlankDay(y, m, d);
    }

    // Counting months and days within a year or a month.
    public partial class WorldSchema
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
        public sealed override int CountDaysInYearBeforeMonth(int y, int m)
        {
            m--;
            int count = 91 * (m / 3);
            if (m > 5 && GregorianFormulae.IsLeapYear(y)) { count++; }
            m %= 3;
            return m == 0 ? count : count + 1 + 30 * m;
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInMonth(int y, int m) =>
            // m = 1, 4, 7, 10, 12   -> 31 days
            // m = 2, 3, 5, 8, 9, 11 -> 30 days
            // m = 6 -> 31 days (leap year) or 30 days (common year)
            m == 12 || checked(m - 1) % 3 == 0 ? 31
            : m == 6 && GregorianFormulae.IsLeapYear(y) ? 31
            : 30;
    }

    // Conversions.
    public partial class WorldSchema
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysSinceEpoch(int y, int m, int d) =>
            GregorianFormulae.GetStartOfYear(y) + CountDaysInYearBeforeMonth(y, m) + d - 1;

        /// <inheritdoc />
        [Pure]
        public sealed override int GetMonth(int y, int doy, out int d)
        {
            if (GregorianFormulae.IsLeapYear(y))
            {
                // On évacue d'emblée le cas du jour intercalaire.
                if (doy == 183) { d = 31; return 6; }
                if (doy > 183) { doy--; }
            }

            // Worldsday.
            if (doy == 365) { d = 31; return 12; }

            // Four identical quarter: 31, 30 and 30.
            int m;
            doy--;
            int q = doy / 91;
            int j = doy % 91;
            if (j < 31)
            {
                m = 1 + 3 * q;
                d = 1 + j % 31;
            }
            else
            {
                j--;
                m = 1 + 3 * q + j / 30;
                d = 1 + j % 30;
            }
            return m;
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int GetYear(int daysSinceEpoch) =>
            GregorianFormulae.GetYear(daysSinceEpoch);
    }

    // Dates in a given year or month.
    public partial class WorldSchema
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int GetStartOfYear(int y) => GregorianFormulae.GetStartOfYear(y);

        /// <inheritdoc />
        public sealed override void GetEndOfYearParts(int y, out int m, out int d)
        {
            m = 12; d = 31;
        }
    }
}