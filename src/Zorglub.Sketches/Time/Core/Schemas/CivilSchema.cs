// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas
{
    using Zorglub.Time.Core.Intervals;

    // year > 0 but SupportedYearsCore is still equal to Maximal32.

    /// <summary>
    /// Represents the Gregorian schema (year > 0).
    /// <para>This class cannot be inherited.</para>
    /// <para>This class can ONLY be initialized from within friend assemblies.</para>
    /// </summary>
    public sealed partial class CivilSchema : GJSchema, IBoxable<CivilSchema>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CivilSchema"/> class.
        /// </summary>
        internal CivilSchema() : base(DefaultSupportedYears.WithMin(1)) { }

        /// <summary>
        /// Creates a new (boxed) instance of the <see cref="CivilSchema"/> class.
        /// </summary>
        [Pure]
        public static Box<CivilSchema> GetInstance() => Box.Create(new CivilSchema());
    }

    public partial class CivilSchema // Year, month or day infos
    {
        /// <inheritdoc />
        [Pure]
        public sealed override bool IsLeapYear(int y) =>
            (y & 3) == 0 && (y % 100 != 0 || y % 400 == 0);
    }

    public partial class CivilSchema // Conversions
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysSinceEpoch(int y, int m, int d)
        {
            if (m < 3)
            {
                y--;
                m += 9;
            }
            else
            {
                m -= 3;
            }

            int C = MathN.Divide(y, 100, out int Y);

            return -DaysInYearAfterFebruary
                + (GregorianSchema.DaysPer400YearCycle * C >> 2)
                + (GregorianSchema.DaysPer4YearSubcycle * Y >> 2)
                + (int)((uint)(153 * m + 2) / 5) + d - 1;
        }

        /// <inheritdoc />
        public sealed override void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
        {
#if false
            uint daysSinceEpoch1 = (uint)daysSinceEpoch + DaysInYearAfterFebruary;

            uint C = ((daysSinceEpoch1 << 2) + 3) / GregorianSchema.DaysPer400YearCycle;
            uint D = daysSinceEpoch1 - (GregorianSchema.DaysPer400YearCycle * C >> 2);

            uint Y = ((D << 2) + 3) / GregorianSchema.DaysPer4YearSubcycle;
            uint d0y = D - (GregorianSchema.DaysPer4YearSubcycle * Y >> 2);

            m = (int)((5 * d0y + 2) / 153);
            d = (int)(1 + d0y - (uint)(153 * m + 2) / 5);

            if (m > 9)
            {
                Y++;
                m -= 9;
            }
            else
            {
                m += 3;
            }

            y = (int)(100 * C + Y);
#else
            daysSinceEpoch += DaysInYearAfterFebruary;

            int C = (int)((uint)((daysSinceEpoch << 2) + 3) / GregorianSchema.DaysPer400YearCycle);
            int D = daysSinceEpoch - (GregorianSchema.DaysPer400YearCycle * C >> 2);

            int Y = (int)((uint)((D << 2) + 3) / GregorianSchema.DaysPer4YearSubcycle);
            int d0y = D - (GregorianSchema.DaysPer4YearSubcycle * Y >> 2);

            m = (int)((uint)(5 * d0y + 2) / 153);
            d = 1 + d0y - (int)((uint)(153 * m + 2) / 5);

            if (m > 9)
            {
                Y++;
                m -= 9;
            }
            else
            {
                m += 3;
            }

            y = 100 * C + Y;
#endif
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int GetYear(int daysSinceEpoch)
        {
            int y = (int)(400L * (daysSinceEpoch + 2) / GregorianSchema.DaysPer400YearCycle);
            int c = y / 100;
            int startOfYearAfter = DaysInCommonYear * y + (y >> 2) - c + (c >> 2);

            return daysSinceEpoch < startOfYearAfter ? y : y + 1;
        }
    }

    public partial class CivilSchema // Dates in a given year or month
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int GetStartOfYear(int y)
        {
            y--;
            int c = y / 100;
            return DaysInCommonYear * y + (y >> 2) - c + (c >> 2);
        }
    }
}
