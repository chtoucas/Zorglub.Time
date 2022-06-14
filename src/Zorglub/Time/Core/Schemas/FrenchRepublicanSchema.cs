// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas
{
    // Si on n'utilisait pas un entier long dans GetDateParts() et GetYear(),
    // la valeur pour MaxYear serait plutôt 1024 (= 1 << 10), à condition bien
    // entendu de ne considérer que des puissances de 2.

    /// <summary>
    /// Represents a French Republican schema and provides a base for derived
    /// classes.
    /// <para>This class can ONLY be inherited from within friend assemblies.</para>
    /// </summary>
    public abstract partial class FrenchRepublicanSchema : PtolemaicSchema
    {
        /// <summary>
        /// Represents the number of days in a 4000-year cycle.
        /// <para>This field is a constant equal to 1_460_969.</para>
        /// </summary>
        /// <remarks>
        /// On average, a year is 365.24225 days long.
        /// </remarks>
        public const long DaysPer4000YearCycle = 1_460_969L;

        /// <summary>
        /// Represents the genuine number of days in a month (excluding the
        /// epagomenal days that are not formally part of the twelfth month).
        /// <para>This field is constant to 30.</para>
        /// </summary>
        public const int DaysInFrenchRepublicanMonth = 30;

        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="FrenchRepublicanSchema"/> class.
        /// </summary>
        private protected FrenchRepublicanSchema(int minDaysInMonth) : base(minDaysInMonth) { }
    }

    public partial class FrenchRepublicanSchema // Year, month or day infos
    {
        /// <inheritdoc />
        [Pure]
        public sealed override bool IsLeapYear(int y) =>
            // Same rule as for the Gregorian calendar but we also exclude the
            // multiples of 4000.
            // Beware, the year IV is the first leap year after the epoch and
            // the year "zero" is not leap --- with the (astronomical) French
            // Revolutionary calendar, the first leap year is the year III.
            (y & 3) == 0
            && (y % 100 != 0 || y % 400 == 0)
            && (y % 4000 != 0);
    }

    public partial class FrenchRepublicanSchema // Conversions
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysSinceEpoch(int y, int m, int d) =>
            GetStartOfYear(y) + 30 * (m - 1) + d - 1;

        /// <inheritdoc />
        [Pure]
        public sealed override int GetYear(int daysSinceEpoch, out int doy)
        {
            // Int64 to prevent overflows.
            int y = 1 + (int)MathZ.Divide(4000L * (daysSinceEpoch + 2), DaysPer4000YearCycle);
            int startOfYear = GetStartOfYear(y);

            if (daysSinceEpoch < startOfYear)
            {
                y--;
                doy = 1 + daysSinceEpoch - GetStartOfYear(y);
            }
            else
            {
                doy = 1 + daysSinceEpoch - startOfYear;
            }
            return y;
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int GetYear(int daysSinceEpoch)
        {
            // Int64 to prevent overflows.
            int y = 1 + (int)MathZ.Divide(4000L * (daysSinceEpoch + 2), DaysPer4000YearCycle);
            int startOfYear = GetStartOfYear(y);
            return daysSinceEpoch < startOfYear ? y - 1 : y;
        }
    }

    public partial class FrenchRepublicanSchema // Dates in a given year or month
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int GetStartOfYear(int y)
        {
            y--;
            int c = MathZ.Divide(y, 100);
            int millennium = MathZ.Divide(c, 10);
            return DaysInCommonYear * y + (y >> 2) - c + (c >> 2) - (millennium >> 2);
        }
    }
}
