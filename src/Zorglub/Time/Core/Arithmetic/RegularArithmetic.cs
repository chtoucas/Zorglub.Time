// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Provides a plain implementation for <see cref="CalendricalArithmeticPlus"/>.
    /// <para>The length of a month must be greater than or equal to
    /// <see cref="CalendricalArithmeticPlus.MinMinDaysInMonth"/>.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed partial class RegularArithmetic : CalendricalArithmeticPlus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegularArithmetic"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> and <see cref="Yemoda"/> are disjoint.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="schema"/> contains at least one
        /// month whose length is strictly less than <see cref="CalendricalArithmeticPlus.MinMinDaysInMonth"/>.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="schema"/> is not regular.</exception>
        public RegularArithmetic(CalendricalSchema schema, Range<int>? supportedYears = null)
            : base(schema, supportedYears)
        {
            Debug.Assert(schema != null);
            if (schema.MinDaysInMonth < MinMinDaysInMonth) Throw.Argument(nameof(schema));
            if (schema.IsRegular(out int monthsInYear) == false) Throw.Argument(nameof(schema));

            MonthsInYear = monthsInYear;
        }

        public int MonthsInYear { get; }

        /// <inheritdoc />
        [Pure]
        public override CalendricalArithmeticPlus WithSupportedYears(Range<int> supportedYears) =>
            new RegularArithmetic(Schema, supportedYears);
    }

    internal partial class RegularArithmetic // Operations on Yemoda
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
                return PartsFactory.GetDateParts(newY, newDoy);
            }

            // Slow track.
            int daysSinceEpoch = checked(Schema.CountDaysSinceEpoch(y, m, d) + days);
            if (Domain.Contains(daysSinceEpoch) == false) Throw.DateOverflow();

            return PartsFactory.GetDateParts(daysSinceEpoch);
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
                    (_, m, int d0) = PartsFactory.GetEndOfYearParts(y);
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
                if (m == MonthsInYear)
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
                : m < MonthsInYear ? Yemoda.AtStartOfMonth(y, m + 1)
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
                : m > 1 ? PartsFactory.GetEndOfMonthParts(y, m - 1)
                // End of previous year...
                : y > MinYear ? PartsFactory.GetEndOfYearParts(y - 1)
                // ... or overflow.
                : Throw.DateOverflow<Yemoda>();
        }
    }

    internal partial class RegularArithmetic // Operations on Yedoy
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
            if (Domain.Contains(daysSinceEpoch) == false) Throw.DateOverflow();

            return PartsFactory.GetOrdinalParts(daysSinceEpoch);
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

            return
                doy < MaxDaysViaDayOfYear || doy < Schema.CountDaysInYear(y) ? new Yedoy(y, doy + 1)
                : y < MaxYear ? Yedoy.AtStartOfYear(y + 1)
                : Throw.DateOverflow<Yedoy>();
        }

        /// <inheritdoc />
        [Pure]
        public override Yedoy PreviousDay(Yedoy ydoy)
        {
            ydoy.Unpack(out int y, out int doy);

            return doy > 1 ? new Yedoy(y, doy - 1)
                : y > MinYear ? PartsFactory.GetEndOfYearOrdinalParts(y - 1)
                : Throw.DateOverflow<Yedoy>();
        }
    }

    internal partial class RegularArithmetic // Operations on Yemo
    {
        /// <inheritdoc />
        [Pure]
        public override Yemo AddMonths(Yemo ym, int months)
        {
            ym.Unpack(out int y, out int m);

            m = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsInYear, out int y0);
            y += y0;

            if (SupportedYears.Contains(y) == false) Throw.MonthOverflow();

            return new Yemo(y, m);
        }

        /// <inheritdoc />
        [Pure]
        public override int CountMonthsBetween(Yemo start, Yemo end)
        {
            start.Unpack(out int y0, out int m0);
            end.Unpack(out int y1, out int m1);

            return (y1 - y0) * MonthsInYear + m1 - m0;
        }
    }

    internal partial class RegularArithmetic // Non-standard operations
    {
        /// <inheritdoc />
        [Pure]
        public override Yemoda AddYears(Yemoda ymd, int years, out int roundoff)
        {
            ymd.Unpack(out int y, out int m, out int d);

            y = checked(y + years);

            if (SupportedYears.Contains(y) == false) Throw.DateOverflow();

            int daysInMonth = Schema.CountDaysInMonth(y, m);
            roundoff = Math.Max(0, d - daysInMonth);
            // On retourne le dernier jour du mois si d > daysInMonth.
            return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
        }

        /// <inheritdoc />
        [Pure]
        public override Yemoda AddMonths(Yemoda ymd, int months, out int roundoff)
        {
            ymd.Unpack(out int y, out int m, out int d);

            // On retranche 1 à "m" pour le rendre algébrique.
            m = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsInYear, out int y0);
            y += y0;

            if (SupportedYears.Contains(y) == false) Throw.DateOverflow();

            int daysInMonth = Schema.CountDaysInMonth(y, m);
            roundoff = Math.Max(0, d - daysInMonth);
            return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
        }

        /// <inheritdoc />
        [Pure]
        public override Yedoy AddYears(Yedoy ydoy, int years, out int roundoff)
        {
            ydoy.Unpack(out int y, out int doy);

            y = checked(y + years);

            if (SupportedYears.Contains(y) == false) Throw.DateOverflow();

            int daysInYear = Schema.CountDaysInYear(y);
            roundoff = Math.Max(0, doy - daysInYear);
            return new Yedoy(y, roundoff > 0 ? daysInYear : doy);
        }

        /// <inheritdoc />
        [Pure]
        public override Yemo AddYears(Yemo ym, int years, out int roundoff)
        {
            ym.Unpack(out int y, out int m);

            y = checked(y + years);

            if (SupportedYears.Contains(y) == false) Throw.MonthOverflow();

            roundoff = 0;
            return new Yemo(y, m);
        }
    }
}
