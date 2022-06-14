// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    internal sealed class MonthsSinceEpochHelper
    {
        private readonly ICalendricalSchema _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthsSinceEpochHelper"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        public MonthsSinceEpochHelper(ICalendricalSchema schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));

            Domain = schema.SupportedYears.Endpoints.Select(GetStartOfYear, GetEndOfYear);
        }

        /// <summary>
        /// Gets the range of supported months, or more precisely the range of supported numbers of
        /// consecutive months from the epoch.
        /// </summary>
        /// <returns>The range from the first month of the first supported year to the last month of
        /// the last supported year.</returns>
        public OrderedPair<int> Domain { get; }

        /// <summary>
        /// Counts the number of consecutive months from the epoch to the first month of the
        /// specified year.
        /// </summary>
        [Pure]
        public int GetStartOfYear(int y) => _schema.CountMonthsSinceEpoch(y, 1);

        /// <summary>
        /// Counts the number of consecutive months from the epoch to the last month of the
        /// specified year.
        /// </summary>
        [Pure]
        public int GetEndOfYear(int y) =>
            _schema.CountMonthsSinceEpoch(y, 1) + _schema.CountMonthsInYear(y) - 1;
    }
}
