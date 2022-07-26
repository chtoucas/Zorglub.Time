// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas
{
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Represents the Gregorian schema.
    /// <para>This class cannot be inherited.</para>
    /// <para>This class can ONLY be initialized from within friend assemblies.</para>
    /// </summary>
    public sealed partial class GregorianSchemaAfterYear0 : GJSchema, IBoxable<GregorianSchemaAfterYear0>
    {
        /// <summary>
        /// Represents the earliest supported year.
        /// <para>This field is a constant equal to 1.</para>
        /// </summary>
        internal const int MinYear = 1;

        /// <summary>
        /// Represents the latest supported year.
        /// <para>This field is a constant equal to 999_999.</para>
        /// </summary>
        internal const int MaxYear = DefaultMaxYear;

        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianSchemaAfterYear0"/> class.
        /// </summary>
        internal GregorianSchemaAfterYear0() : base(Range.Create(1, DefaultMaxYear)) { }

        /// <summary>
        /// Creates a new (boxed) instance of the <see cref="GregorianSchemaAfterYear0"/> class.
        /// </summary>
        [Pure]
        public static Box<GregorianSchemaAfterYear0> GetInstance() => Box.Create(new GregorianSchemaAfterYear0());
    }

    public partial class GregorianSchemaAfterYear0 // Year, month or day infos
    {
        /// <inheritdoc />
        [Pure]
        public sealed override bool IsLeapYear(int y) =>
            (y & 3) == 0 && (y % 100 != 0 || y % 400 == 0);
    }

    public partial class GregorianSchemaAfterYear0 // Conversions
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

    public partial class GregorianSchemaAfterYear0 // Dates in a given year or month
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
