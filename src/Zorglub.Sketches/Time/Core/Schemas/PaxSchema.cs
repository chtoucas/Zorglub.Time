// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas
{
    // Month 12 = Columbus
    // Common year:
    // - month 13 = December
    // Leap year:
    // - month 13 = Pax (1 week), intercalary week/month
    // - month 14 = December
    //
    // This is an early proposal for a leap-week calendar, certainly not a very
    // good one.
    //
    // Main flaws: 13 or 14 months, complex leap rule without any improvement
    // over the Gregorian one, position of the intercalary week.
    //
    // References:
    // - http://myweb.ecu.edu/mccartyr/colligan.html
    // - org.threeten.extra.chrono.PaxChronology

    /// <summary>
    /// Represents the Pax schema proposed by James A. Colligan, S.J. (1930).
    /// <para>The Pax schema is a leap-week schema.</para>
    /// <para>This class cannot be inherited.</para>
    /// <para>This class can ONLY be initialized from within friend assemblies.
    /// </para>
    /// </summary>
    public sealed partial class PaxSchema : LeapWeekSchema, IBoxable<PaxSchema>
    {
        /// <summary>
        /// Represents the number of days per 400-year cycle.
        /// <para>This field is a constant equal to 146_097.</para>
        /// </summary>
        /// <remarks>
        /// On average, a year is 365.2425 days long.
        /// </remarks>
        public const int DaysPer400YearCycle = 400 * 364 + 71 * 7;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaxSchema"/> class.
        /// </summary>
        internal PaxSchema() : base(DefaultSupportedYears.WithMin(1), 364, 7) { }

        /// <inheritdoc />
        public sealed override CalendricalFamily Family => CalendricalFamily.Other;

        /// <inheritdoc />
        public sealed override CalendricalAdjustments PeriodicAdjustments => CalendricalAdjustments.Weeks;

        /// <summary>
        /// Creates a new (boxed) instance of the <see cref="PaxSchema"/> class.
        /// </summary>
        [Pure]
        public static Box<PaxSchema> GetInstance() => Box.Create(new PaxSchema());

        /// <summary>
        /// Determines whether the specified month is the Pax month of the year or not.
        /// </summary>
        [Pure]
        public bool IsPaxMonth(int y, int m) => m == 13 && IsLeapYear(y);

        /// <summary>
        /// Determines whether the specified month is the last month of the year or not.
        /// </summary>
        [Pure]
        public bool IsLastMonthOfYear(int y, int m) => m == 14 || (m == 13 && !IsLeapYear(y));
    }

    public partial class PaxSchema // LeapWeekSchema
    {
        /// <inheritdoc />
        public sealed override DayOfWeek FirstDayOfWeek => DayOfWeek.Sunday;

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsIntercalaryWeek(int y, int woy) =>
            // Intercalary week = the week of the Pax month on a leap year.
            woy == 49 && IsLeapYear(y);

        /// <inheritdoc />
        [Pure]
        public sealed override int CountWeeksInYear(int y) => IsLeapYear(y) ? 53 : 52;

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysSinceEpoch(int y, int woy, DayOfWeek dow) =>
            // The first day of the week is a Sunday not a Monday, therefore
            // we do not have to use the adjusted day of the week.
            GetStartOfYear(y) + 7 * (woy - 1) + (int)dow;

        /// <inheritdoc />
        public sealed override void GetWeekdateParts(int daysSinceEpoch, out int y, out int woy, out DayOfWeek dow)
        {
            throw new NotImplementedException();
        }
    }

    public partial class PaxSchema // Year, month or day infos
    {
        /// <inheritdoc />
        [Pure]
        public sealed override bool IsLeapYear(int y)
        {
            int Y = y % 100;
            // - last two digits of y = 99 or is a multiple of 6.
            // - a century always satisfies (Y % 6 == 0), but we only keep it
            //   if it is not a multiple of 400.
            return Y == 99 || (Y % 6 == 0 && (Y != 0 || y % 400 != 0));
        }

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsIntercalaryMonth(int y, int m) => false;

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsIntercalaryDay(int y, int m, int d) => false;

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsSupplementaryDay(int y, int m, int d) => false;
    }

    public partial class PaxSchema // Counting months and days within a year or a month
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int CountMonthsInYear(int y) => IsLeapYear(y) ? 14 : 13;

        /// <inheritdoc />
        //
        // 364 days = 13 months of 28 days
        // 371 days = 364 days + 7 days (Pax month)
        [Pure]
        public sealed override int CountDaysInYear(int y) => IsLeapYear(y) ? 371 : 364;

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInYearBeforeMonth(int y, int m) => m == 14 ? 343 : 28 * (m - 1);

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInMonth(int y, int m) => IsPaxMonth(y, m) ? 7 : 28;
    }

    public partial class PaxSchema // Conversions
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int CountMonthsSinceEpoch(int y, int m)
        {
            // FIXME(code): temporary value for tests.
            return 0;
        }

        /// <inheritdoc />
        public sealed override void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int GetMonth(int y, int doy, out int d)
        {
            int m;

            if (doy < 337 || !IsLeapYear(y))
            {
                int d0y = doy - 1;
                m = 1 + d0y / 28;
                d = 1 + d0y % 28;
            }
            else if (doy < 344)
            {
                m = 13;
                d = doy - 336;
            }
            else
            {
                m = 14;
                d = doy - 343;
            }

            return m;
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int GetYear(int daysSinceEpoch)
        {
            throw new NotImplementedException();
        }
    }

    public partial class PaxSchema // Dates in a given year or month
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int GetStartOfYear(int y)
        {
            // In a century we have 18 leap years, except if the century is a
            // multiple of 400, in which case we only have 17 leap years.
            y--;
            int C = y / 100;
            int Y = y % 100;
            return 364 * y + 7 * (18 * C - (C >> 2) + Y / 6 + Y / 99);
        }

        /// <inheritdoc />
        public sealed override void GetDatePartsAtEndOfYear(int y, out int m, out int d)
        {
            m = IsLeapYear(y) ? 14 : 13;
            d = 28;
        }
    }
}
