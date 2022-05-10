// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas
{
    /// <summary>
    /// Represents the Gregorian schema.
    /// <para>This class cannot be inherited.</para>
    /// <para>This class can ONLY be initialized from within friend assemblies.</para>
    /// </summary>
    public sealed partial class GregorianSchema : GJSchema, IBoxable<GregorianSchema>
    {
        /// <summary>
        /// Represents the earliest supported year.
        /// <para>This field is a constant equal to -999_998.</para>
        /// </summary>
        internal const int MinYear = DefaultMinYear;

        /// <summary>
        /// Represents the latest supported year.
        /// <para>This field is a constant equal to 999_999.</para>
        /// </summary>
        internal const int MaxYear = DefaultMaxYear;

        /// <summary>
        /// Represents the number of days per 400-year cycle.
        /// <para>This field is a constant equal to 146_097.</para>
        /// </summary>
        /// <remarks>
        /// <para>On average, a year is 365.2425 days long.</para>
        /// </remarks>
        public const int DaysPer400YearCycle = 400 * DaysInCommonYear + 97;

        /// <summary>
        /// Represents the <i>average</i> number of days per 100-year subcycle.
        /// <para>This field is a constant equal to 36_524.</para>
        /// </summary>
        public const int DaysPer100YearSubcycle = 100 * DaysInCommonYear + 24;

        /// <summary>
        /// Represents the <i>average</i> number of days per 4-year subcycle.
        /// <para>This field is a constant equal to 1461.</para>
        /// </summary>
        /// <remarks>
        /// <para>On average, a year is 365.25 days long.</para>
        /// </remarks>
        public const int DaysPer4YearSubcycle = CalendricalConstants.DaysPer4JulianYearCycle;

        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianSchema"/> class.
        /// </summary>
        internal GregorianSchema() { }

        /// <summary>
        /// Creates a new (boxed) instance of the <see cref="GregorianSchema"/> class.
        /// </summary>
        [Pure]
        public static Box<GregorianSchema> GetInstance() => Box.Create(new GregorianSchema());

        /// <summary>
        /// Gets the number of days in each century of the first 4-century cycle, the one starting
        /// at century 0.
        /// <para>The span index matches the century number (0 to 3).</para>
        /// </summary>
        [Pure]
        internal static ReadOnlySpan<ushort> DaysIn4CenturyCycle =>
            new ushort[4] { 36_525, 36_524, 36_524, 36_524 };
    }

    public partial class GregorianSchema // Year, month or day infos
    {
        /// <inheritdoc />
        [Pure]
        public sealed override bool IsLeapYear(int y) =>
            // year is a multiple of 4 and, if it is a centennial year, only keep
            // those multiple of 400; a centennial year is common if it is not a
            // multiple of 400.
            (y & 3) == 0 && (y % 100 != 0 || y % 400 == 0);
    }

    public partial class GregorianSchema // Conversions
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysSinceEpoch(int y, int m, int d)
        {
            // Troesch formula but keeping DaysInYearAfterFebruary separated.
            // We pretend that the first month of the year is March
            // (numbering starting at zero, not one).

            if (m < 3)
            {
                y--;
                m += 9;
            }
            else
            {
                m -= 3;
            }

            int C = MathZ.Divide(y, 100, out int Y);

            return -DaysInYearAfterFebruary
                + (DaysPer400YearCycle * C >> 2) + (DaysPer4YearSubcycle * Y >> 2)
                + (int)((uint)(153 * m + 2) / 5) + d - 1;
        }

        /// <inheritdoc />
        public sealed override void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
        {
            daysSinceEpoch += DaysInYearAfterFebruary;

            // Position within the 400-year cycle.
            int C = MathZ.Divide((daysSinceEpoch << 2) + 3, DaysPer400YearCycle);
            int D = daysSinceEpoch - (DaysPer400YearCycle * C >> 2);
            // Position within the 4-year cycle.
            int Y = (int)((uint)((D << 2) + 3) / DaysPer4YearSubcycle);
            int d0y = D - (DaysPer4YearSubcycle * Y >> 2);
            // Position within the year.
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
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int GetYear(int daysSinceEpoch)
        {
            // Int64 to prevent overflows.
            int y = (int)MathZ.Divide(400L * (daysSinceEpoch + 2), DaysPer400YearCycle);
            int c = MathZ.Divide(y, 100);
            int startOfYearAfter = DaysInCommonYear * y + (y >> 2) - c + (c >> 2);

            return daysSinceEpoch < startOfYearAfter ? y : y + 1;
        }
    }

    public partial class GregorianSchema // Dates in a given year or month
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int GetStartOfYear(int y)
        {
            y--;
            int c = MathZ.Divide(y, 100);
            return DaysInCommonYear * y + (y >> 2) - c + (c >> 2);
        }
    }
}
