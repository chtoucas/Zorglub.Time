// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    internal abstract class MonthsSinceEpochHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonthsSinceEpochHelper"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        protected MonthsSinceEpochHelper(ICalendricalSchema schema)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        protected ICalendricalSchema Schema { get; }

        public static MonthsSinceEpochHelper Create(ICalendricalSchema schema)
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

        private sealed class Regular12 : MonthsSinceEpochHelper
        {
            private const int MonthsInYear = 12;

            public Regular12(ICalendricalSchema schema) : base(schema) { }

            [Pure]
            public override int GetStartOfYear(int y) => MonthsInYear * (y - 1);

            [Pure]
            public override int GetEndOfYear(int y) => MonthsInYear * y - 1;
        }

        private sealed class Regular13 : MonthsSinceEpochHelper
        {
            private const int MonthsInYear = 13;

            public Regular13(ICalendricalSchema schema) : base(schema) { }

            [Pure]
            public override int GetStartOfYear(int y) => MonthsInYear * (y - 1);

            [Pure]
            public override int GetEndOfYear(int y) => MonthsInYear * y - 1;
        }

        private sealed class Regular : MonthsSinceEpochHelper
        {
            private readonly int _monthsInYear;

            public Regular(ICalendricalSchema schema, int monthsInYear) : base(schema)
            {
                _monthsInYear = monthsInYear;
            }

            [Pure]
            public override int GetStartOfYear(int y) => _monthsInYear * (y - 1);

            [Pure]
            public override int GetEndOfYear(int y) => _monthsInYear * y - 1;
        }

        private sealed class Plain : MonthsSinceEpochHelper
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
