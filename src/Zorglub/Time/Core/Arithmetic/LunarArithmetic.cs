// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    using static Zorglub.Time.Core.CalendricalConstants;

    /// <summary>
    /// Provides the core mathematical operations on dates for schemas with profile
    /// <see cref="CalendricalProfile.Lunar"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed partial class LunarArithmetic : StandardArithmetic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LunarArithmetic"/> class with the specified
        /// schema.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> and <see cref="Yemoda.SupportedYears"/> are disjoint.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="schema"/> does not have the expected
        /// profile <see cref="CalendricalProfile.Lunar"/>.</exception>
        public LunarArithmetic(CalendricalSchema schema) : base(schema)
        {
            Debug.Assert(schema != null);
            Debug.Assert(Lunar.MinDaysInMonth >= MinMinDaysInMonth);

            Requires.Profile(schema, CalendricalProfile.Lunar);
        }
    }

    internal partial class LunarArithmetic // Operations on Yemoda
    {
        /// <inheritdoc />
        [Pure]
        public override Yemoda AddDays(Yemoda ymd, int days)
        {
            // Fast tracks.
            if (-Lunar.MinDaysInMonth <= days && days <= Lunar.MinDaysInMonth)
            {
                return AddDaysViaDayOfMonth(ymd, days);
            }

            ymd.Unpack(out int y, out int m, out int d);
            if (-Lunar.MinDaysInYear <= days && days <= Lunar.MinDaysInYear)
            {
                int doy = Schema.GetDayOfYear(y, m, d);
                var (newY, newDoy) = AddDaysViaDayOfYear(new Yedoy(y, doy), days);
                return PartsFactory.GetDateParts(newY, newDoy);
            }

            // Slow track.
            int daysSinceEpoch = checked(Schema.CountDaysSinceEpoch(y, m, d) + days);
            if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return PartsFactory.GetDateParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        protected internal override Yemoda AddDaysViaDayOfMonth(Yemoda ymd, int days)
        {
            Debug.Assert(-Lunar.MinDaysInMonth <= days);
            Debug.Assert(days <= Lunar.MinDaysInMonth);

            ymd.Unpack(out int y, out int m, out int d);

            int dom = d + days;
            if (dom < 1)
            {
                if (m == 1)
                {
                    if (y == MinYear) Throw.DateOverflow();
                    y--;
                    (_, m, int d0) = PartsFactory.GetEndOfYearParts(y);
                    dom += d0;
                }
                else
                {
                    m--;
                    dom += Schema.CountDaysInMonth(y, m);
                }
            }
            else if (dom > Lunar.MinDaysInMonth)
            {
                int daysInMonth = Schema.CountDaysInMonth(y, m);
                if (dom > daysInMonth)
                {
                    dom -= daysInMonth;
                    if (m == Lunar.MonthsInYear)
                    {
                        if (y == MaxYear) Throw.DateOverflow();
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
                d < Lunar.MinDaysInMonth || d < Schema.CountDaysInMonth(y, m)
                    ? new Yemoda(y, m, d + 1)
                : m < Lunar.MonthsInYear ? Yemoda.AtStartOfMonth(y, m + 1)
                : y < MaxYear ? Yemoda.AtStartOfYear(y + 1)
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
                : y > MinYear ? PartsFactory.GetEndOfYearParts(y - 1)
                : Throw.DateOverflow<Yemoda>();
        }

        /// <inheritdoc />
        [Pure]
        public override int CountDaysBetween(Yemoda start, Yemoda end)
        {
            if (end.Yemo == start.Yemo) { return end.Day - start.Day; }

            start.Unpack(out int y0, out int m0, out int d0);
            end.Unpack(out int y1, out int m1, out int d1);

            return Schema.CountDaysSinceEpoch(y1, m1, d1) - Schema.CountDaysSinceEpoch(y0, m0, d0);
        }
    }

    internal partial class LunarArithmetic // Operations on Yedoy
    {
        /// <inheritdoc />
        [Pure]
        public override Yedoy AddDays(Yedoy ydoy, int days)
        {
            // Fast track.
            if (-Lunar.MinDaysInYear <= days && days <= Lunar.MinDaysInYear)
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
            Debug.Assert(-Schema.MinDaysInYear <= days);
            Debug.Assert(days <= Schema.MinDaysInYear);

            ydoy.Unpack(out int y, out int doy);

            doy += days;
            if (doy < 1)
            {
                if (y == MinYear) Throw.DateOverflow();
                y--;
                doy += Schema.CountDaysInYear(y);
            }
            else
            {
                int daysInYear = Schema.CountDaysInYear(y);
                if (doy > daysInYear)
                {
                    if (y == MaxYear) Throw.DateOverflow();
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
                doy < Lunar.MinDaysInYear || doy < Schema.CountDaysInYear(y)
                    ? new Yedoy(y, doy + 1)
                : y < MaxYear ? Yedoy.AtStartOfYear(y + 1)
                : Throw.DateOverflow<Yedoy>();
        }

        /// <inheritdoc />
        [Pure]
        public override Yedoy PreviousDay(Yedoy ydoy)
        {
            ydoy.Unpack(out int y, out int doy);

            return
                doy > 1 ? new Yedoy(y, doy - 1)
                : y > MinYear ? PartsFactory.GetEndOfYearOrdinalParts(y - 1)
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
