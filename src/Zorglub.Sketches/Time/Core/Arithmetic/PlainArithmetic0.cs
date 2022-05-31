// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#define DEFAULT_ADDITION_MINDAYSINMONTH

namespace Zorglub.Time.Core.Arithmetic
{
    [Obsolete("Use PlainArithmetic instead.")]
    internal sealed partial class PlainArithmetic0 : ICalendricalArithmetic
    {
        private readonly int _minDaysSinceEpoch;
        private readonly int _maxDaysSinceEpoch;
        private readonly int _minYear;
        private readonly int _maxYear;
        private readonly int _maxDaysViaDayOfYear;
#if DEFAULT_ADDITION_MINDAYSINMONTH
        private readonly int _maxDaysViaDayOfMonth;
#endif
        private readonly SystemSchema _schema;

        internal PlainArithmetic0(SystemSchema schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));

            (_minYear, _maxYear) = schema.SupportedYears.Endpoints;
            (_minDaysSinceEpoch, _maxDaysSinceEpoch) = schema.Domain.Endpoints;

            _maxDaysViaDayOfYear = schema.MinDaysInYear;
#if DEFAULT_ADDITION_MINDAYSINMONTH
            _maxDaysViaDayOfMonth = schema.MinDaysInMonth;
#endif
        }

        public int MinYear => _minYear;
        public int MaxYear => _maxYear;
    }

    internal partial class PlainArithmetic0 // Operations on Yemoda.
    {
        [Pure]
        public Yemoda AddDays(Yemoda ymd, int days)
        {
            // Fast tracks.
            ymd.Unpack(out int y, out int m, out int d);

#if DEFAULT_ADDITION_MINDAYSINMONTH
            // If _maxDaysViaDayOfMonth is very small, using
            // AddDaysViaDayOfMonth() might not bring any perf improvement,
            // it might even be the opposite. For instance, for calendars with
            // a virtual thirteen month, _maxDaysViaDayOfMonth = 5. Anyway, this
            // is very rare and we opt to optimize for the most common case for
            // which _maxDaysViaDayOfMonth >= 28.
            if (-_maxDaysViaDayOfMonth <= days && days <= _maxDaysViaDayOfMonth)
            {
                return AddDaysViaDayOfMonth(y, m, d, days);
            }
#endif

            if (-_maxDaysViaDayOfYear <= days && days <= _maxDaysViaDayOfYear)
            {
                int doy = _schema.GetDayOfYear(y, m, d);
                var (newY, newDoy) = AddDaysViaDayOfYear(y, doy, days);
                return _schema.GetDateParts(newY, newDoy);
            }

            // Slow track.
            int daysSinceEpoch = checked(_schema.CountDaysSinceEpoch(y, m, d) + days);
            if (daysSinceEpoch < _minDaysSinceEpoch || daysSinceEpoch > _maxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return _schema.GetDateParts(daysSinceEpoch);
        }

#if DEFAULT_ADDITION_MINDAYSINMONTH
        [Pure]
        private Yemoda AddDaysViaDayOfMonth(int y, int m, int d, int days)
        {
            Debug.Assert(-_maxDaysViaDayOfMonth <= days);
            Debug.Assert(days <= _maxDaysViaDayOfMonth);

            int dom = d + days;
            if (dom < 1)
            {
                if (m == 1)
                {
                    if (y == _minYear) Throw.DateOverflow();
                    y--;
                    _schema.GetEndOfYearParts(y, out int m0, out int d0);
                    m = m0;
                    dom += d0;
                }
                else
                {
                    m--;
                    dom += _schema.CountDaysInMonth(y, m);
                }
            }

            int daysInMonth = _schema.CountDaysInMonth(y, m);
            if (dom > daysInMonth)
            {
                dom -= daysInMonth;
                if (m == _schema.CountMonthsInYear(y))
                {
                    if (y == _maxYear) Throw.DateOverflow();
                    y++;
                    m = 1;
                }
                else
                {
                    m++;
                }
            }

            return new Yemoda(y, m, dom);
        }
#endif

        [Pure]
        public Yemoda NextDay(Yemoda ymd)
        {
            ymd.Unpack(out int y, out int m, out int d);

            return d < _schema.CountDaysInMonth(y, m) ? new Yemoda(y, m, d + 1)
                : m < _schema.CountMonthsInYear(y) ? Yemoda.AtStartOfMonth(y, m + 1)
                : y < _maxYear ? Yemoda.AtStartOfYear(y + 1)
                : Throw.DateOverflow<Yemoda>();
        }

        [Pure]
        public Yemoda PreviousDay(Yemoda ymd)
        {
            ymd.Unpack(out int y, out int m, out int d);

            return d > 1 ? new Yemoda(y, m, d - 1)
                : m > 1 ? _schema.GetEndOfMonthParts(y, m - 1)
                : y > _minYear ? _schema.GetEndOfYearParts(y - 1)
                : Throw.DateOverflow<Yemoda>();
        }

        [Pure]
        public int CountDaysBetween(Yemoda start, Yemoda end)
        {
            if (end.Yemo == start.Yemo) { return end.Day - start.Day; }

            start.Unpack(out int y0, out int m0, out int d0);
            end.Unpack(out int y1, out int m1, out int d1);

            return _schema.CountDaysSinceEpoch(y1, m1, d1) - _schema.CountDaysSinceEpoch(y0, m0, d0);
        }
    }

    internal partial class PlainArithmetic0 // Operations on Yedoy.
    {
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

            return _schema.GetOrdinalParts(daysSinceEpoch);
        }

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

        [Pure]
        public Yedoy NextDay(Yedoy ydoy)
        {
            ydoy.Unpack(out int y, out int doy);

            return
                doy < _maxDaysViaDayOfYear || doy < _schema.CountDaysInYear(y) ? new Yedoy(y, doy + 1)
                : y < _maxYear ? Yedoy.AtStartOfYear(y + 1)
                : Throw.DateOverflow<Yedoy>();
        }

        [Pure]
        public Yedoy PreviousDay(Yedoy ydoy)
        {
            ydoy.Unpack(out int y, out int doy);

            return doy > 1 ? new Yedoy(y, doy - 1)
                : y > _minYear ? _schema.GetEndOfYearOrdinalParts(y - 1)
                : Throw.DateOverflow<Yedoy>();
        }

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
