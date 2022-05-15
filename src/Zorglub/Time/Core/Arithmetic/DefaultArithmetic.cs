// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    using Zorglub.Time.Core.Intervals;

    // FIXME(code): Yemoda/Yedoy validation. Use a CalendricalPartsFactory.

    // Even if DefaultArithmetic does not derive from FastArithmetic, it
    // includes some of the optimisations found there, see for instance
    // AddDaysViaDayOfYear(), nevertheless this is hidden.

    /// <summary>
    /// Provides a plain implementation for <see cref="ICalendricalArithmetic"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed partial class DefaultArithmetic : ICalendricalArithmetic
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
        /// Initializes a new instance of the <see cref="DefaultArithmetic"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> and <see cref="Yemoda.SupportedYears"/> are disjoint.
        /// </exception>
        public DefaultArithmetic(ICalendricalSchema schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));

            var set = Interval.Intersect(schema.SupportedYears, Yemoda.SupportedYears);
            if (set.IsEmpty) Throw.Argument(nameof(schema));
            var pair = set.Range.Value.Endpoints;

            (_minYear, _maxYear) = pair;
            (_minDaysSinceEpoch, _maxDaysSinceEpoch) =
                pair.Select(schema.GetStartOfYear, schema.GetEndOfYear);

            _maxDaysViaDayOfYear = schema.MinDaysInYear;
        }
    }

    internal partial class DefaultArithmetic // Operations on Yemoda.
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
                var (newY, newDoy) = AddDaysViaDayOfYear(y, doy, days);
                return GetDateParts1(newY, newDoy);
            }

            // Slow track.
            int daysSinceEpoch = checked(_schema.CountDaysSinceEpoch(y, m, d) + days);
            if (daysSinceEpoch < _minDaysSinceEpoch || daysSinceEpoch > _maxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return GetDateParts2(daysSinceEpoch);

            [Pure]
            Yemoda GetDateParts1(int y, int doy)
            {
                m = _schema.GetMonth(y, doy, out int d);
                return new Yemoda(y, m, d);
            }

            [Pure]
            Yemoda GetDateParts2(int daysSinceEpoch)
            {
                _schema.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
                return new Yemoda(y, m, d);
            }
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
                : m > 1 ? GetEndOfMonthParts(y, m - 1)
                : y > _minYear ? GetEndOfYearParts(y - 1)
                : Throw.DateOverflow<Yemoda>();

            [Pure]
            Yemoda GetEndOfMonthParts(int y, int m)
            {
                int d = _schema.CountDaysInMonth(y, m);
                return new Yemoda(y, m, d);
            }

            [Pure]
            Yemoda GetEndOfYearParts(int y)
            {
                int m = _schema.CountMonthsInYear(y);
                int d = _schema.CountDaysInMonth(y, m);
                return new Yemoda(y, m, d);
            }
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

    internal partial class DefaultArithmetic // Operations on Yedoy.
    {
        /// <inheritdoc />
        [Pure]
        public Yedoy AddDays(Yedoy ydoy, int days)
        {
            ydoy.Unpack(out int y, out int doy);

            // Fast track.
            if (-_maxDaysViaDayOfYear <= days && days <= _maxDaysViaDayOfYear)
            {
                return AddDaysViaDayOfYear(y, doy, days);
            }

            // Slow track.
            int daysSinceEpoch = checked(_schema.CountDaysSinceEpoch(y, doy) + days);
            if (daysSinceEpoch < _minDaysSinceEpoch || daysSinceEpoch > _maxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return GetOrdinalParts(daysSinceEpoch);

            [Pure]
            Yedoy GetOrdinalParts(int daysSinceEpoch)
            {
                int y = _schema.GetYear(daysSinceEpoch, out int doy);
                return new Yedoy(y, doy);
            }
        }

        /// <inheritdoc />
        [Pure]
        private Yedoy AddDaysViaDayOfYear(int y, int doy, int days)
        {
            Debug.Assert(-_maxDaysViaDayOfYear <= days);
            Debug.Assert(days <= _maxDaysViaDayOfYear);

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
