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
    internal sealed partial class GregorianArithmetic : StandardArithmetic
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
        public GregorianArithmetic() : base(new GregorianSchema())
        {
            Debug.Assert(MinYear == MinSupportedYear);
            Debug.Assert(MaxYear == MaxSupportedYear);
            Debug.Assert(Solar.MinDaysInMonth >= MinMinDaysInMonth);
        }
    }

    internal partial class GregorianArithmetic // Operations on Yemoda
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
                return PartsFactory.GetDateParts(newY, newDoy);
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
                : m > 1 ? PartsFactory.GetEndOfMonthParts(y, m - 1)
                : y > MinSupportedYear ? PartsFactory.GetEndOfYearParts(y - 1)
                : Throw.DateOverflow<Yemoda>();
        }
    }

    internal partial class GregorianArithmetic // Operations on Yedoy
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

            return PartsFactory.GetOrdinalParts(daysSinceEpoch);
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

            return doy > 1 ? new Yedoy(y, doy - 1)
                : y > MinSupportedYear ? PartsFactory.GetEndOfYearOrdinalParts(y - 1)
                : Throw.DateOverflow<Yedoy>();
        }
    }
}
