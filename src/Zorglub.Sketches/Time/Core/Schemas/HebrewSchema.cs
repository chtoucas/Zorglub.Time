// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas
{
    /// <summary>
    /// Represents the  Hebrew schema.
    /// <para>This class cannot be inherited.</para>
    /// <para>This class can ONLY be initialized from within friend assemblies.
    /// </para>
    /// </summary>
    public sealed partial class HebrewSchema : SystemSchema, IBoxable<HebrewSchema>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HebrewSchema"/> class.
        /// </summary>
        internal HebrewSchema() : base(353, 29) { }

        /// <inheritdoc />
        public sealed override CalendricalFamily Family => CalendricalFamily.Lunisolar;

        /// <inheritdoc />
        public sealed override CalendricalAdjustments PeriodicAdjustments =>
            CalendricalAdjustments.DaysAndMonths;

        /// <summary>
        /// Creates a new (boxed) instance of the <see cref="HebrewSchema"/> class.
        /// </summary>
        [Pure]
        public static Box<HebrewSchema> GetInstance() => Box.Create(new HebrewSchema());

        [Pure]
        internal static bool IsSabbaticalYear(int y) => MathZ.Modulo(y, 7) == 0;
    }

    public partial class HebrewSchema // Year, month or day infos
    {
        /// <inheritdoc />
        [Pure]
        public sealed override bool IsLeapYear(int y) => MathZ.Modulo(checked(7 * y + 1), 19) < 7;

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsIntercalaryMonth(int y, int m)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsIntercalaryDay(int y, int m, int d)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsSupplementaryDay(int y, int m, int d)
        {
            throw new NotImplementedException();
        }
    }

    public partial class HebrewSchema // Counting months and days within a year or a month
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int CountMonthsInYear(int y) => IsLeapYear(y) ? 13 : 12;

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInYear(int y)
        {
            // FIXME: temporary value for tests; GetArithmetic().
            return 353;
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInYearBeforeMonth(int y, int m)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInMonth(int y, int m)
        {
            throw new NotImplementedException();
        }
    }

    public partial class HebrewSchema // Conversions
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
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int GetYear(int daysSinceEpoch)
        {
            throw new NotImplementedException();
        }
    }

    public partial class HebrewSchema // Dates in a given year or month
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int GetStartOfYear(int y)
        {
            // FIXME(code): temporary value for tests.
            return 0;
        }

        [Pure]
        public sealed override void GetEndOfYearParts(int y, out int m, out int d)
        {
            throw new NotImplementedException();
        }
    }
}
