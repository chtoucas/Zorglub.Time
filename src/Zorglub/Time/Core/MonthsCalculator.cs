// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    internal abstract class MonthsCalculator : ISchemaBound
    {
        private readonly ICalendricalSchema _schema;

        protected MonthsCalculator(ICalendricalSchema schema)
        {
            Debug.Assert(schema != null);

            _schema = schema;
        }

        protected ICalendricalSchema Schema => _schema;

        ICalendricalSchema ISchemaBound.Schema => _schema;

        public static MonthsCalculator Create(ICalendricalSchema schema)
        {
            Requires.NotNull(schema);

            _ = schema.IsRegular(out int monthsInYear);

            return monthsInYear switch
            {
                12 => new Regular12(schema),
                13 => new Regular13(schema),
                > 0 => new Regular(schema, monthsInYear),
                _ => new Plain(schema)
            };
        }

        /// <summary>
        /// Counts the number of consecutive months from the epoch to the first month of the
        /// specified year.
        /// </summary>
        [Pure] public abstract int GetStartOfYear(int y);

        /// <summary>
        /// Counts the number of consecutive months from the epoch to the last month of the
        /// specified year.
        /// </summary>
        [Pure] public abstract int GetEndOfYear(int y);

        internal sealed class Regular12 : MonthsCalculator
        {
            private const int MonthsInYear = 12;

            public Regular12(ICalendricalSchema schema) : base(schema)
            {
                Debug.Assert(schema.IsRegular(out int monthsInYear));
                Debug.Assert(monthsInYear == MonthsInYear);
            }

            [Pure]
            // CIL code size = 11 bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int CountMonthsSinceEpoch(int y, int m) => MonthsInYear * (y - 1) + m - 1;

            // CIL code size = 20 bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
            {
                y = 1 + MathZ.Divide(monthsSinceEpoch, MonthsInYear, out int m0);
                m = 1 + m0;
            }

            [Pure]
            public override int GetStartOfYear(int y) => MonthsInYear * (y - 1);

            [Pure]
            public override int GetEndOfYear(int y) => MonthsInYear * y - 1;
        }

        internal sealed class Regular13 : MonthsCalculator
        {
            private const int MonthsInYear = 13;

            public Regular13(ICalendricalSchema schema) : base(schema)
            {
                Debug.Assert(schema.IsRegular(out int monthsInYear));
                Debug.Assert(monthsInYear == MonthsInYear);
            }

            [Pure]
            // CIL code size = 11 bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int CountMonthsSinceEpoch(int y, int m) => MonthsInYear * (y - 1) + m - 1;

            // CIL code size = 20 bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
            {
                y = 1 + MathZ.Divide(monthsSinceEpoch, MonthsInYear, out int m0);
                m = 1 + m0;
            }

            [Pure]
            public override int GetStartOfYear(int y) => MonthsInYear * (y - 1);

            [Pure]
            public override int GetEndOfYear(int y) => MonthsInYear * y - 1;
        }

        internal sealed class Regular : MonthsCalculator
        {
            private readonly int _monthsInYear;

            public Regular(ICalendricalSchema schema, int monthsInYear) : base(schema)
            {
                Debug.Assert(schema.IsRegular(out int monthsInYear_));
                Debug.Assert(monthsInYear_ == monthsInYear);

                _monthsInYear = monthsInYear;
            }

            [Pure]
            public override int GetStartOfYear(int y) => _monthsInYear * (y - 1);

            [Pure]
            public override int GetEndOfYear(int y) => _monthsInYear * y - 1;
        }

        internal sealed class Plain : MonthsCalculator
        {
            public Plain(ICalendricalSchema schema) : base(schema) { }

            [Pure]
            public override int GetStartOfYear(int y) => Schema.CountMonthsSinceEpoch(y, 1);

            [Pure]
            public override int GetEndOfYear(int y) =>
                Schema.CountMonthsSinceEpoch(y, 1) + Schema.CountMonthsInYear(y) - 1;
        }
    }
}
