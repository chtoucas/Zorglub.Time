// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;

    // Works even if the boundary years are not complete.

    /// <summary>
    /// Provides a reference implementation for <see cref="ICalendricalArithmetic"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed partial class BasicArithmetic : ICalendricalArithmetic
    {
        /// <summary>
        /// Represents the schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalSchema _schema;

        /// <summary>
        /// Represents the adapter for calendrical parts.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly PartsAdapter _partsAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicArithmetic"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="schema"/> contains at least one
        /// month whose length is strictly less than <see cref="MinMinDaysInMonth"/>.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="schema"/> is not regular.</exception>
        /// <exception cref="AoorException"><paramref name="supportedYears"/> is NOT a subinterval
        /// of the range of supported years by <paramref name="schema"/>.</exception>
        public BasicArithmetic(ICalendricalSchema schema, Range<int> supportedYears)
            : this(CalendricalSegment.Create(schema, supportedYears)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicArithmetic"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="segment"/> is null.</exception>
        public BasicArithmetic(CalendricalSegment segment)
        {
            Segment = segment ?? throw new ArgumentNullException(nameof(segment));
            _schema = segment.Schema;

            _partsAdapter = new PartsAdapter(_schema);
        }

        /// <inheritdoc/>
        public CalendricalSegment Segment { get; }

        /// <summary>
        /// Gets the range of supported days.
        /// </summary>
        private Range<int> Domain => Segment.Domain;

        /// <summary>
        /// Gets the range of supported months.
        /// </summary>
        private Range<int> MonthDomain => Segment.MonthDomain;
    }

    public partial class BasicArithmetic // Operations on DateParts
    {
        /// <inheritdoc />
        [Pure]
        public DateParts AddDays(DateParts parts, int days)
        {
            var (y, m, d) = parts;
            int daysSinceEpoch = checked(_schema.CountDaysSinceEpoch(y, m, d) + days);

            if (Domain.Contains(daysSinceEpoch) == false) Throw.DateOverflow();

            return _partsAdapter.GetDateParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public DateParts NextDay(DateParts parts) => AddDays(parts, 1);

        /// <inheritdoc />
        [Pure]
        public DateParts PreviousDay(DateParts parts) => AddDays(parts, -1);

        /// <inheritdoc />
        [Pure]
        public int CountDaysBetween(DateParts start, DateParts end)
        {
            var (y0, m0, d0) = start;
            var (y1, m1, d1) = end;

            return _schema.CountDaysSinceEpoch(y1, m1, d1) - _schema.CountDaysSinceEpoch(y0, m0, d0);
        }
    }

    public partial class BasicArithmetic // Operations on OrdinalParts
    {
        /// <inheritdoc />
        [Pure]
        public OrdinalParts AddDays(OrdinalParts parts, int days)
        {
            var (y, doy) = parts;
            int daysSinceEpoch = checked(_schema.CountDaysSinceEpoch(y, doy) + days);

            if (Domain.Contains(daysSinceEpoch) == false) Throw.DateOverflow();

            return _partsAdapter.GetOrdinalParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public OrdinalParts NextDay(OrdinalParts parts) => AddDays(parts, 1);

        /// <inheritdoc />
        [Pure]
        public OrdinalParts PreviousDay(OrdinalParts parts) => AddDays(parts, -1);

        /// <inheritdoc />
        [Pure]
        public int CountDaysBetween(OrdinalParts start, OrdinalParts end)
        {
            var (y0, doy0) = start;
            var (y1, doy1) = end;

            return _schema.CountDaysSinceEpoch(y1, doy1) - _schema.CountDaysSinceEpoch(y0, doy0);
        }
    }

    public partial class BasicArithmetic // Operations on MonthParts
    {
        /// <inheritdoc />
        [Pure]
        public MonthParts AddMonths(MonthParts parts, int months)
        {
            var (y, m) = parts;
            int monthsSinceEpoch = checked(_schema.CountMonthsSinceEpoch(y, m) + months);

            if (MonthDomain.Contains(monthsSinceEpoch) == false) Throw.MonthOverflow();

            return _partsAdapter.GetMonthParts(monthsSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public MonthParts NextMonth(MonthParts parts) => AddMonths(parts, 1);

        /// <inheritdoc />
        [Pure]
        public MonthParts PreviousMonth(MonthParts parts) => AddMonths(parts, -1);

        /// <inheritdoc />
        [Pure]
        public int CountMonthsBetween(MonthParts start, MonthParts end)
        {
            var (y0, m0) = start;
            var (y1, m1) = end;

            return _schema.CountMonthsSinceEpoch(y1, m1) - _schema.CountMonthsSinceEpoch(y0, m0);
        }
    }
}
