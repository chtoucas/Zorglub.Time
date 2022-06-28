// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    internal abstract class MonthHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonthHelper"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        protected MonthHelper(ICalendricalSchema schema)
        {
            Debug.Assert(schema != null);

            Schema = schema;
        }

        protected ICalendricalSchema Schema { get; }

        public static MonthHelper Create(ICalendricalSchema schema)
        {
            Requires.NotNull(schema);

            _ = schema.IsRegular(out int monthsInYear);

            return monthsInYear switch
            {
                12 => new Regular12Case(schema),
                13 => new Regular13Case(schema),
                > 0 => new RegularCase(schema, monthsInYear),
                _ => new PlainCase(schema)
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

        public static class Regular12
        {
            private const int MonthsInYear = 12;

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
        }

        public static class Regular13
        {
            private const int MonthsInYear = 13;

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
        }

        private sealed class Regular12Case : MonthHelper
        {
            private const int MonthsInYear = 12;

            public Regular12Case(ICalendricalSchema schema) : base(schema) { }

            [Pure]
            public override int GetStartOfYear(int y) => MonthsInYear * (y - 1);

            [Pure]
            public override int GetEndOfYear(int y) => MonthsInYear * y - 1;
        }

        private sealed class Regular13Case : MonthHelper
        {
            private const int MonthsInYear = 13;

            public Regular13Case(ICalendricalSchema schema) : base(schema) { }

            [Pure]
            public override int GetStartOfYear(int y) => MonthsInYear * (y - 1);

            [Pure]
            public override int GetEndOfYear(int y) => MonthsInYear * y - 1;
        }

        private sealed class RegularCase : MonthHelper
        {
            private readonly int _monthsInYear;

            public RegularCase(ICalendricalSchema schema, int monthsInYear) : base(schema)
            {
                _monthsInYear = monthsInYear;
            }

            [Pure]
            public override int GetStartOfYear(int y) => _monthsInYear * (y - 1);

            [Pure]
            public override int GetEndOfYear(int y) => _monthsInYear * y - 1;
        }

        private sealed class PlainCase : MonthHelper
        {
            public PlainCase(ICalendricalSchema schema) : base(schema) { }

            [Pure]
            public override int GetStartOfYear(int y) => Schema.CountMonthsSinceEpoch(y, 1);

            [Pure]
            public override int GetEndOfYear(int y) =>
                Schema.CountMonthsSinceEpoch(y, 1) + Schema.CountMonthsInYear(y) - 1;
        }
    }
}
