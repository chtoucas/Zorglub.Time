// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Validation;

    // Complete years only!

    /// <summary>
    /// Provides a generic implementation for <see cref="ICalendricalArithmeticPlus"/> for when the
    /// schema is regular.
    /// <para>The length of a month must be greater than or equal to 7.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed partial class RegularArithmetic : ICalendricalArithmeticPlus
    {
        /// <summary>
        /// Represents the absolute minimum value admissible for the minimum total number of days
        /// there is at least in a month.
        /// <para>This field is a constant equal to 7.</para>
        /// </summary>
        internal const int MinMinDaysInMonth = 7;

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
        /// Initializes a new instance of the <see cref="RegularArithmetic"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="schema"/> contains at least one
        /// month whose length is strictly less than <see cref="MinMinDaysInMonth"/>.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="schema"/> is not regular.</exception>
        /// <exception cref="AoorException"><paramref name="supportedYears"/> is NOT a subinterval
        /// of the range of supported years by <paramref name="schema"/>.</exception>
        public RegularArithmetic(ICalendricalSchema schema, Range<int> supportedYears)
        {
            Requires.NotNull(schema);
            if (schema.MinDaysInMonth < MinMinDaysInMonth) Throw.Argument(nameof(schema));
            if (schema.IsRegular(out int monthsInYear) == false) Throw.Argument(nameof(schema));
            _schema = schema;

            var seg = CalendricalSegment.Create(schema, supportedYears);

            // FIXME(code): use MinMaxYearValidator.
            Segment = seg;
            SupportedDays = new SupportedDays(seg.SupportedDays);
            //SupportedMonths = new SupportedMonths(seg.SupportedMonths);
            SupportedYears = new SupportedYears(seg.SupportedYears);

            _partsAdapter = new PartsAdapter(_schema);

            MonthsInYear = monthsInYear;

            MaxDaysViaDayOfYear = _schema.MinDaysInYear;
            MaxDaysViaDayOfMonth = _schema.MinDaysInMonth;
        }

        public int MonthsInYear { get; }

        /// <inheritdoc/>
        public CalendricalSegment Segment { get; }

        /// <summary>
        /// Gets the range of supported days.
        /// </summary>
        private SupportedDays SupportedDays { get; }

        ///// <summary>
        ///// Gets the range of supported months.
        ///// </summary>
        //private SupportedMonths SupportedMonths { get; }

        /// <summary>
        /// Gets the range of supported years.
        /// </summary>
        private SupportedYears SupportedYears { get; }

        /// <summary>
        /// Gets the earliest supported year.
        /// </summary>
        private int MinYear { get; }

        /// <summary>
        /// Gets the latest supported year.
        /// </summary>
        private int MaxYear { get; }

        /// <summary>
        /// Gets the maximum absolute value for a parameter "days" for the method
        /// <see cref="AddDaysViaDayOfYear(OrdinalParts, int)"/>.
        /// </summary>
        public int MaxDaysViaDayOfYear { get; init; }

        public int MaxDaysViaDayOfMonth { get; init; }
    }

    public partial class RegularArithmetic // Operations on DateParts
    {
        /// <inheritdoc />
        [Pure]
        public DateParts AddDays(DateParts parts, int days)
        {
            // Fast tracks.
            var (y, m, d) = parts;

            // No change of month.
            int dom = checked(d + days);
            if (1 <= dom && (dom <= MaxDaysViaDayOfMonth || dom <= _schema.CountDaysInMonth(y, m)))
            {
                return new DateParts(y, m, dom);
            }

            if (-MaxDaysViaDayOfYear <= days && days <= MaxDaysViaDayOfYear)
            {
                int doy = _schema.GetDayOfYear(y, m, d);
                var (newY, newDoy) = AddDaysViaDayOfYear(new OrdinalParts(y, doy), days);
                return _partsAdapter.GetDateParts(newY, newDoy);
            }

            // Slow track.
            int daysSinceEpoch = checked(_schema.CountDaysSinceEpoch(y, m, d) + days);
            SupportedDays.Check(daysSinceEpoch);

            return _partsAdapter.GetDateParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public DateParts NextDay(DateParts parts)
        {
            var (y, m, d) = parts;

            return
                d < MaxDaysViaDayOfMonth || d < _schema.CountDaysInMonth(y, m) ? new DateParts(y, m, d + 1)
                : m < MonthsInYear ? DateParts.AtStartOfMonth(y, m + 1)
                : y < MaxYear ? DateParts.AtStartOfYear(y + 1)
                : Throw.DateOverflow<DateParts>();
        }

        /// <inheritdoc />
        [Pure]
        public DateParts PreviousDay(DateParts parts)
        {
            var (y, m, d) = parts;

            return
                d > 1 ? new DateParts(y, m, d - 1)
                : m > 1 ? _partsAdapter.GetDatePartsAtEndOfMonth(y, m - 1)
                : y > MinYear ? _partsAdapter.GetDatePartsAtEndOfYear(y - 1)
                : Throw.DateOverflow<DateParts>();
        }

        [Pure]
        public int CountDaysBetween(DateParts start, DateParts end)
        {
            if (end.MonthParts == start.MonthParts) { return end.Day - start.Day; }

            var (y0, m0, d0) = start;
            var (y1, m1, d1) = end;

            return _schema.CountDaysSinceEpoch(y1, m1, d1) - _schema.CountDaysSinceEpoch(y0, m0, d0);
        }
    }

    public partial class RegularArithmetic // Operations on OrdinalParts
    {
        /// <inheritdoc />
        [Pure]
        public OrdinalParts AddDays(OrdinalParts parts, int days)
        {
            // Fast track.
            if (-MaxDaysViaDayOfYear <= days && days <= MaxDaysViaDayOfYear)
            {
                return AddDaysViaDayOfYear(parts, days);
            }

            var (y, doy) = parts;

            // Slow track.
            int daysSinceEpoch = checked(_schema.CountDaysSinceEpoch(y, doy) + days);
            SupportedDays.Check(daysSinceEpoch);

            return _partsAdapter.GetOrdinalParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        private OrdinalParts AddDaysViaDayOfYear(OrdinalParts parts, int days)
        {
            Debug.Assert(-MaxDaysViaDayOfYear <= days);
            Debug.Assert(days <= MaxDaysViaDayOfYear);

            var (y, doy) = parts;

            // No need to use checked arithmetic here.
            doy += days;
            if (doy < 1)
            {
                if (y == MinYear) Throw.DateOverflow();
                y--;
                doy += _schema.CountDaysInYear(y);
            }
            else
            {
                int daysInYear = _schema.CountDaysInYear(y);
                if (doy > daysInYear)
                {
                    if (y == MaxYear) Throw.DateOverflow();
                    y++;
                    doy -= daysInYear;
                }
            }

            return new OrdinalParts(y, doy);
        }

        /// <inheritdoc />
        [Pure]
        public OrdinalParts NextDay(OrdinalParts parts)
        {
            var (y, doy) = parts;

            return
                doy < MaxDaysViaDayOfYear || doy < _schema.CountDaysInYear(y) ? new OrdinalParts(y, doy + 1)
                : y < MaxYear ? OrdinalParts.AtStartOfYear(y + 1)
                : Throw.DateOverflow<OrdinalParts>();
        }

        /// <inheritdoc />
        [Pure]
        public OrdinalParts PreviousDay(OrdinalParts parts)
        {
            var (y, doy) = parts;

            return doy > 1 ? new OrdinalParts(y, doy - 1)
                : y > MinYear ? _partsAdapter.GetOrdinalPartsAtEndOfYear(y - 1)
                : Throw.DateOverflow<OrdinalParts>();
        }

        /// <inheritdoc />
        [Pure]
        public int CountDaysBetween(OrdinalParts start, OrdinalParts end)
        {
            var (y0, doy0) = start;
            var (y1, doy1) = end;

            return _schema.CountDaysSinceEpoch(y1, doy1) - _schema.CountDaysSinceEpoch(y0, doy0);
        }
    }

    public partial class RegularArithmetic // Operations on MonthParts
    {
        /// <inheritdoc />
        [Pure]
        public MonthParts AddMonths(MonthParts parts, int months)
        {
            var (y, m) = parts;

            m = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsInYear, out int y0);
            y += y0;
            SupportedYears.CheckForMonth(y);

            return new MonthParts(y, m);
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

            return (y1 - y0) * MonthsInYear + m1 - m0;
        }
    }

    public partial class RegularArithmetic // Non-standard operations
    {
        /// <inheritdoc />
        [Pure]
        public DateParts AddYears(DateParts parts, int years, out int roundoff)
        {
            var (y, m, d) = parts;

            y = checked(y + years);
            SupportedYears.Check(y);

            int daysInMonth = _schema.CountDaysInMonth(y, m);
            roundoff = Math.Max(0, d - daysInMonth);
            return new DateParts(y, m, roundoff > 0 ? daysInMonth : d);
        }

        /// <inheritdoc />
        [Pure]
        public DateParts AddMonths(DateParts parts, int months, out int roundoff)
        {
            var (y, m, d) = parts;

            m = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsInYear, out int y0);
            y += y0;
            SupportedYears.Check(y);

            int daysInMonth = _schema.CountDaysInMonth(y, m);
            roundoff = Math.Max(0, d - daysInMonth);
            return new DateParts(y, m, roundoff > 0 ? daysInMonth : d);
        }

        /// <inheritdoc />
        [Pure]
        public OrdinalParts AddYears(OrdinalParts parts, int years, out int roundoff)
        {
            var (y, doy) = parts;

            y = checked(y + years);
            SupportedYears.Check(y);

            int daysInYear = _schema.CountDaysInYear(y);
            roundoff = Math.Max(0, doy - daysInYear);
            return new OrdinalParts(y, roundoff > 0 ? daysInYear : doy);
        }

        /// <inheritdoc />
        [Pure]
        public MonthParts AddYears(MonthParts parts, int years, out int roundoff)
        {
            var (y, m) = parts;

            y = checked(y + years);
            SupportedYears.CheckForMonth(y);

            roundoff = 0;
            return new MonthParts(y, m);
        }
    }
}
