// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    using Zorglub.Time.Core.Intervals;

    // REVIEW(code): checked or unchecked factory?

    // Even if PlainArithmetic does not derive from FastArithmetic, it includes
    // some of the optimisations found there, see for instance
    // AddDaysViaDayOfYear(), nevertheless this is hidden.

    /// <summary>
    /// Provides a plain implementation for <see cref="IFastArithmetic"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed partial class PlainArithmetic : IFastArithmetic
    {
        /// <summary>
        /// Represents the earliest supported year.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _minYear;

        /// <summary>
        /// Represents the latest supported year.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _maxYear;

        /// <summary>
        /// Represents the earliest day number.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _minDaysSinceEpoch;

        /// <summary>
        /// Represents the latest day number.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _maxDaysSinceEpoch;

        /// <summary>
        /// Represents the maximum absolute value for a parameter "days" for the method
        /// <see cref="AddDaysViaDayOfYear(int, int, int)"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _maxDaysViaDayOfYear;

        /// <summary>
        /// Represents the schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalSchema _schema;

        /// <summary>
        /// Represents the factory for calendrical parts.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalPartsFactory _partsFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainArithmetic"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> and <see cref="Yemoda.SupportedYears"/> are disjoint.
        /// </exception>
        public PlainArithmetic(ICalendricalSchema schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
            _partsFactory = ICalendricalPartsFactory.Create(schema, @checked: false);

            var set = Interval.Intersect(schema.SupportedYears, Yemoda.SupportedYears);
            if (set.IsEmpty) Throw.Argument(nameof(schema));

            SupportedYears = set.Range;

            var minMaxYear = SupportedYears.Endpoints;
            (_minYear, _maxYear) = minMaxYear;
            (_minDaysSinceEpoch, _maxDaysSinceEpoch) =
                minMaxYear.Select(schema.GetStartOfYear, schema.GetEndOfYear);

            _maxDaysViaDayOfYear = schema.MinDaysInYear;
        }

        /// <inheritdoc/>
        public Range<int> SupportedYears { get; }

        Yemoda IFastArithmetic.AddDaysViaDayOfMonth(Yemoda ymd, int days) =>
            AddDays(ymd, days);

        Yedoy IFastArithmetic.AddDaysViaDayOfYear(Yedoy ydoy, int days) =>
            AddDaysViaDayOfYear(ydoy, days);
    }

    internal partial class PlainArithmetic // Operations on Yemoda.
    {
        /// <inheritdoc />
        [Pure]
        public Yemoda AddDays(Yemoda ymd, int days)
        {
            // Fast tracks.
            ymd.Unpack(out int y, out int m, out int d);

            if (-_maxDaysViaDayOfYear <= days && days <= _maxDaysViaDayOfYear)
            {
                int doy = _schema.GetDayOfYear(y, m, d);
                var (newY, newDoy) = AddDaysViaDayOfYear(new Yedoy(y, doy), days);
                return _partsFactory.GetDateParts(newY, newDoy);
            }

            // Slow track.
            int daysSinceEpoch = checked(_schema.CountDaysSinceEpoch(y, m, d) + days);
            if (daysSinceEpoch < _minDaysSinceEpoch || daysSinceEpoch > _maxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return _partsFactory.GetDateParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public Yemoda NextDay(Yemoda ymd)
        {
            ymd.Unpack(out int y, out int m, out int d);

            return d < _schema.CountDaysInMonth(y, m) ? new Yemoda(y, m, d + 1)
                : m < _schema.CountMonthsInYear(y) ? Yemoda.AtStartOfMonth(y, m + 1)
                : y < _maxYear ? Yemoda.AtStartOfYear(y + 1)
                : Throw.DateOverflow<Yemoda>();
        }

        /// <inheritdoc />
        [Pure]
        public Yemoda PreviousDay(Yemoda ymd)
        {
            ymd.Unpack(out int y, out int m, out int d);

            return d > 1 ? new Yemoda(y, m, d - 1)
                : m > 1 ? _partsFactory.GetEndOfMonthParts(y, m - 1)
                : y > _minYear ? _partsFactory.GetEndOfYearParts(y - 1)
                : Throw.DateOverflow<Yemoda>();
        }

        /// <inheritdoc />
        [Pure]
        public int CountDaysBetween(Yemoda start, Yemoda end)
        {
            if (end.Yemo == start.Yemo) { return end.Day - start.Day; }

            start.Unpack(out int y0, out int m0, out int d0);
            end.Unpack(out int y1, out int m1, out int d1);

            return _schema.CountDaysSinceEpoch(y1, m1, d1) - _schema.CountDaysSinceEpoch(y0, m0, d0);
        }
    }

    internal partial class PlainArithmetic // Operations on Yedoy.
    {
        /// <inheritdoc />
        [Pure]
        public Yedoy AddDays(Yedoy ydoy, int days)
        {
            // Fast track.
            if (-_maxDaysViaDayOfYear <= days && days <= _maxDaysViaDayOfYear)
            {
                return AddDaysViaDayOfYear(ydoy, days);
            }

            ydoy.Unpack(out int y, out int doy);

            // Slow track.
            int daysSinceEpoch = checked(_schema.CountDaysSinceEpoch(y, doy) + days);
            if (daysSinceEpoch < _minDaysSinceEpoch || daysSinceEpoch > _maxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return _partsFactory.GetOrdinalParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        private Yedoy AddDaysViaDayOfYear(Yedoy ydoy, int days)
        {
            Debug.Assert(-_maxDaysViaDayOfYear <= days);
            Debug.Assert(days <= _maxDaysViaDayOfYear);

            ydoy.Unpack(out int y, out int doy);

            doy += days;
            if (doy < 1)
            {
                if (y == _minYear) Throw.DateOverflow();
                y--;
                doy += _schema.CountDaysInYear(y);
            }
            else
            {
                int daysInYear = _schema.CountDaysInYear(y);
                if (doy > daysInYear)
                {
                    if (y == _maxYear) Throw.DateOverflow();
                    y++;
                    doy -= daysInYear;
                }
            }

            return new Yedoy(y, doy);
        }

        /// <inheritdoc />
        [Pure]
        public Yedoy NextDay(Yedoy ydoy)
        {
            ydoy.Unpack(out int y, out int doy);

            return
                doy < _maxDaysViaDayOfYear || doy < _schema.CountDaysInYear(y) ? new Yedoy(y, doy + 1)
                : y < _maxYear ? Yedoy.AtStartOfYear(y + 1)
                : Throw.DateOverflow<Yedoy>();
        }

        /// <inheritdoc />
        [Pure]
        public Yedoy PreviousDay(Yedoy ydoy)
        {
            ydoy.Unpack(out int y, out int doy);

            return doy > 1 ? new Yedoy(y, doy - 1)
                : y > _minYear ? GetEndOfYearOrdinalParts(y - 1)
                : Throw.DateOverflow<Yedoy>();

            [Pure]
            Yedoy GetEndOfYearOrdinalParts(int y)
            {
                int doy = _schema.CountDaysInYear(y);
                return new Yedoy(y, doy);
            }
        }

        /// <inheritdoc />
        [Pure]
        public int CountDaysBetween(Yedoy start, Yedoy end)
        {
            if (end.Year == start.Year) { return end.DayOfYear - start.DayOfYear; }

            start.Unpack(out int y0, out int doy0);
            end.Unpack(out int y1, out int doy1);

            return _schema.CountDaysSinceEpoch(y1, doy1) - _schema.CountDaysSinceEpoch(y0, doy0);
        }
    }
}
