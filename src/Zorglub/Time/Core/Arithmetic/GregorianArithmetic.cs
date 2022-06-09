// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    using Zorglub.Time.Core.Schemas;

    using static Zorglub.Time.Core.CalendricalConstants;

    /// <summary>
    /// Provides the core mathematical operations on dates within the Gregorian calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed partial class GregorianArithmetic : FastArithmetic
    {
        /// <summary>
        /// Represents the earliest supported year.
        /// <para>This field is a constant equal to -999_998.</para>
        /// </summary>
        private const int MinSupportedYear = GregorianSchema.MinYear;

        /// <summary>
        /// Represents the latest supported year.
        /// <para>This field is a constant equal to 999_999.</para>
        /// </summary>
        private const int MaxSupportedYear = GregorianSchema.MaxYear;

        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianArithmetic"/> class.
        /// </summary>
        public GregorianArithmetic(GregorianSchema schema) : base(schema)
        {
            Debug.Assert(MinYear == MinSupportedYear);
            Debug.Assert(MaxYear == MaxSupportedYear);
        }

        /// <inheritdoc />
        public override int MaxDaysViaDayOfYear => Solar.MinDaysInYear;

        /// <inheritdoc />
        public override int MaxDaysViaDayOfMonth => Solar.MinDaysInMonth;
    }

    internal partial class GregorianArithmetic // Operations on Yemoda.
    {
        /// <inheritdoc />
        [Pure]
        public override Yemoda AddDays(Yemoda ymd, int days)
        {
            // Fast tracks.
            if (-Solar.MinDaysInMonth <= days && days <= Solar.MinDaysInMonth)
            {
                return AddDaysViaDayOfMonth(ymd, days);
            }

            ymd.Unpack(out int y, out int m, out int d);
            if (-Solar.MinDaysInYear <= days && days <= Solar.MinDaysInYear)
            {
                int doy = Schema.GetDayOfYear(y, m, d);
                var (newY, newDoy) = AddDaysViaDayOfYear(new Yedoy(y, doy), days);
                return Schema.GetDateParts(newY, newDoy);
            }

            // Slow track.
            int daysSinceEpoch = checked(GregorianFormulae.CountDaysSinceEpoch(y, m, d) + days);
            if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return GregorianFormulae.GetDateParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        protected internal override Yemoda AddDaysViaDayOfMonth(Yemoda ymd, int days)
        {
            Debug.Assert(-Solar.MinDaysInMonth <= days);
            Debug.Assert(days <= Solar.MinDaysInMonth);

            ymd.Unpack(out int y, out int m, out int d);

            int dom = d + days;
            if (dom < 1)
            {
                if (m == 1)
                {
                    if (y == MinSupportedYear) Throw.DateOverflow();
                    y--;
                    m = Solar12.MonthsInYear;
                    dom += 31;
                }
                else
                {
                    m--;
                    dom += GregorianFormulae.CountDaysInMonth(y, m);
                }
            }
            else if (dom > Solar.MinDaysInMonth)
            {
                int daysInMonth = GregorianFormulae.CountDaysInMonth(y, m);
                if (dom > daysInMonth)
                {
                    dom -= daysInMonth;
                    if (m == Solar12.MonthsInYear)
                    {
                        if (y == MaxSupportedYear) Throw.DateOverflow();
                        y++;
                        m = 1;
                    }
                    else
                    {
                        m++;
                    }
                }
            }

            return new Yemoda(y, m, dom);
        }

        /// <inheritdoc />
        [Pure]
        public override Yemoda NextDay(Yemoda ymd)
        {
            ymd.Unpack(out int y, out int m, out int d);

            return
                d < Solar.MinDaysInMonth || d < GregorianFormulae.CountDaysInMonth(y, m)
                    ? new Yemoda(y, m, d + 1)
                : m < Solar12.MonthsInYear ? Yemoda.AtStartOfMonth(y, m + 1)
                : y < MaxSupportedYear ? Yemoda.AtStartOfYear(y + 1)
                : Throw.DateOverflow<Yemoda>();
        }

        /// <inheritdoc />
        [Pure]
        public override Yemoda PreviousDay(Yemoda ymd)
        {
            ymd.Unpack(out int y, out int m, out int d);

            return
                d > 1 ? new Yemoda(y, m, d - 1)
                : m > 1 ? Schema.GetEndOfMonthParts(y, m - 1)
                : y > MinSupportedYear ? Schema.GetEndOfYearParts(y - 1)
                : Throw.DateOverflow<Yemoda>();
        }

        /// <inheritdoc />
        [Pure]
        public override int CountDaysBetween(Yemoda start, Yemoda end)
        {
            if (end.Yemo == start.Yemo) { return end.Day - start.Day; }

            start.Unpack(out int y0, out int m0, out int d0);
            end.Unpack(out int y1, out int m1, out int d1);

            return GregorianFormulae.CountDaysSinceEpoch(y1, m1, d1)
                - GregorianFormulae.CountDaysSinceEpoch(y0, m0, d0);
        }
    }

    internal partial class GregorianArithmetic // Operations on Yedoy.
    {
        /// <inheritdoc />
        [Pure]
        public override Yedoy AddDays(Yedoy ydoy, int days)
        {
            // Fast track.
            if (-Solar.MinDaysInYear <= days && days <= Solar.MinDaysInYear)
            {
                return AddDaysViaDayOfYear(ydoy, days);
            }

            ydoy.Unpack(out int y, out int doy);

            // Slow track.
            int daysSinceEpoch = checked(Schema.CountDaysSinceEpoch(y, doy) + days);
            if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return Schema.GetOrdinalParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        protected internal override Yedoy AddDaysViaDayOfYear(Yedoy ydoy, int days)
        {
            Debug.Assert(-Solar.MinDaysInYear <= days);
            Debug.Assert(days <= Solar.MinDaysInYear);

            ydoy.Unpack(out int y, out int doy);

            doy += days;
            if (doy < 1)
            {
                if (y == MinSupportedYear) Throw.DateOverflow();
                y--;
                doy += GregorianFormulae.CountDaysInYear(y);
            }
            else
            {
                int daysInYear = GregorianFormulae.CountDaysInYear(y);
                if (doy > daysInYear)
                {
                    if (y == MaxSupportedYear) Throw.DateOverflow();
                    y++;
                    doy -= daysInYear;
                }
            }

            return new Yedoy(y, doy);
        }

        /// <inheritdoc />
        [Pure]
        public override Yedoy NextDay(Yedoy ydoy)
        {
            ydoy.Unpack(out int y, out int doy);

            return
                doy < Solar.MinDaysInYear || doy < GregorianFormulae.CountDaysInYear(y)
                    ? new Yedoy(y, doy + 1)
                : y < MaxSupportedYear ? Yedoy.AtStartOfYear(y + 1)
                : Throw.DateOverflow<Yedoy>();
        }

        /// <inheritdoc />
        [Pure]
        public override Yedoy PreviousDay(Yedoy ydoy)
        {
            ydoy.Unpack(out int y, out int doy);

            return
                doy > 1 ? new Yedoy(y, doy - 1)
                : y > MinSupportedYear ? Schema.GetEndOfYearOrdinalParts(y - 1)
                : Throw.DateOverflow<Yedoy>();
        }

        /// <inheritdoc />
        [Pure]
        public override int CountDaysBetween(Yedoy start, Yedoy end)
        {
            if (end.Year == start.Year) { return end.DayOfYear - start.DayOfYear; }

            start.Unpack(out int y0, out int doy0);
            end.Unpack(out int y1, out int doy1);

            return Schema.CountDaysSinceEpoch(y1, doy1) - Schema.CountDaysSinceEpoch(y0, doy0);
        }
    }
}
