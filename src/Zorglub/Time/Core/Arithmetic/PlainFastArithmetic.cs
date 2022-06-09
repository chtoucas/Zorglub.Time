// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    /// <summary>
    /// Provides a plain implementation for <see cref="FastArithmetic"/>.
    /// <para>The length of a month must be greater than or equal to
    /// <see cref="FastArithmetic.MinMinDaysInMonth"/>.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed partial class PlainFastArithmetic : FastArithmetic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlainFastArithmetic"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="schema"/> contains at least one
        /// month whose length is strictly less than <see cref="FastArithmetic.MinMinDaysInMonth"/>.
        /// </exception>
        public PlainFastArithmetic(SystemSchema schema) : base(schema)
        {
            Debug.Assert(schema != null);

            MaxDaysViaDayOfYear = schema.MinDaysInYear;
            MaxDaysViaDayOfMonth = schema.MinDaysInMonth;
        }

        /// <inheritdoc />
        public override int MaxDaysViaDayOfYear { get; }

        /// <inheritdoc />
        /// <remarks>
        /// <para>The value is guaranteed to be greater than or equal to
        /// <see cref="FastArithmetic.MinMinDaysInMonth"/>.</para>
        /// </remarks>
        public override int MaxDaysViaDayOfMonth { get; }
    }

    internal partial class PlainFastArithmetic // Operations on Yemoda
    {
        /// <inheritdoc />
        [Pure]
        public override Yemoda AddDays(Yemoda ymd, int days)
        {
            // Fast tracks.
            ymd.Unpack(out int y, out int m, out int d);

            // No change of month.
            // In theory, the only thing we know at this point is that
            // MaxDaysViaDayOfMonth >= 7 (= MinMinDaysInMonth), but in fact we
            // know a bit more. Indeed, we know that the schema does not fit one
            // of the standard profiles (see Create()) which most certainly
            // means that there is at least one very short month, therefore I
            // don't think that AddDaysViaDayOfMonth() is that interesting here.
            // Notice the use of checked arithmetic here.
            int dom = checked(d + days);
            if (1 <= dom && (dom <= MaxDaysViaDayOfMonth || dom <= Schema.CountDaysInMonth(y, m)))
            {
                return new Yemoda(y, m, dom);
            }

            if (-MaxDaysViaDayOfYear <= days && days <= MaxDaysViaDayOfYear)
            {
                int doy = Schema.GetDayOfYear(y, m, d);
                var (newY, newDoy) = AddDaysViaDayOfYear(new Yedoy(y, doy), days);
                return Schema.GetDateParts(newY, newDoy);
            }

            // Slow track.
            int daysSinceEpoch = checked(Schema.CountDaysSinceEpoch(y, m, d) + days);
            if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return Schema.GetDateParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        protected internal override Yemoda AddDaysViaDayOfMonth(Yemoda ymd, int days)
        {
            Debug.Assert(-MaxDaysViaDayOfMonth <= days);
            Debug.Assert(days <= MaxDaysViaDayOfMonth);

            ymd.Unpack(out int y, out int m, out int d);

            // No need to use checked arithmetic here.
            int dom = d + days;
            if (dom < 1)
            {
                // Previous month.
                if (m == 1)
                {
                    // Last month of previous year.
                    if (y == MinYear) Throw.DateOverflow();
                    y--;
                    Schema.GetEndOfYearParts(y, out int m0, out int d0);
                    m = m0;
                    dom += d0;
                }
                else
                {
                    m--;
                    dom += Schema.CountDaysInMonth(y, m);
                }
            }

            int daysInMonth = Schema.CountDaysInMonth(y, m);
            if (dom > daysInMonth)
            {
                // Next month.
                dom -= daysInMonth;
                if (m == Schema.CountMonthsInYear(y))
                {
                    // First month of next year.
                    if (y == MaxYear) Throw.DateOverflow();
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

        /// <inheritdoc />
        [Pure]
        public override Yemoda NextDay(Yemoda ymd)
        {
            ymd.Unpack(out int y, out int m, out int d);

            return
                // Same month, the day after.
                d < MaxDaysViaDayOfMonth || d < Schema.CountDaysInMonth(y, m) ? new Yemoda(y, m, d + 1)
                // Same year, start of next month.
                : m < Schema.CountMonthsInYear(y) ? Yemoda.AtStartOfMonth(y, m + 1)
                // Start of next year...
                : y < MaxYear ? Yemoda.AtStartOfYear(y + 1)
                // ... or overflow.
                : Throw.DateOverflow<Yemoda>();
        }

        /// <inheritdoc />
        [Pure]
        public override Yemoda PreviousDay(Yemoda ymd)
        {
            ymd.Unpack(out int y, out int m, out int d);

            return
                // Same month, the day before.
                d > 1 ? new Yemoda(y, m, d - 1)
                // Same year, end of previous month.
                : m > 1 ? Schema.GetEndOfMonthParts(y, m - 1)
                // End of previous year...
                : y > MinYear ? Schema.GetEndOfYearParts(y - 1)
                // ... or overflow.
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

    internal partial class PlainFastArithmetic // Operations on Yedoy
    {
        /// <inheritdoc />
        [Pure]
        public override Yedoy AddDays(Yedoy ydoy, int days)
        {
            // Fast track.
            if (-MaxDaysViaDayOfYear <= days && days <= MaxDaysViaDayOfYear)
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
            Debug.Assert(-MaxDaysViaDayOfYear <= days);
            Debug.Assert(days <= MaxDaysViaDayOfYear);

            ydoy.Unpack(out int y, out int doy);

            // No need to use checked arithmetic here.
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

            return doy < MaxDaysViaDayOfYear || doy < Schema.CountDaysInYear(y) ? new Yedoy(y, doy + 1)
                : y < MaxYear ? Yedoy.AtStartOfYear(y + 1)
                : Throw.DateOverflow<Yedoy>();
        }

        /// <inheritdoc />
        [Pure]
        public override Yedoy PreviousDay(Yedoy ydoy)
        {
            ydoy.Unpack(out int y, out int doy);

            return doy > 1 ? new Yedoy(y, doy - 1)
                : y > MinYear ? Schema.GetEndOfYearOrdinalParts(y - 1)
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
