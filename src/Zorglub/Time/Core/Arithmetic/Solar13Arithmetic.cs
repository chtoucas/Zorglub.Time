// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    using __Solar13 = CalendricalConstants.Solar13;

    /// <summary>
    /// Provides the core mathematical operations on dates for schemas with profile
    /// <see cref="CalendricalProfile.Solar13"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed class Solar13Arithmetic : SolarArithmetic
    {
        private const int MonthsInYear = __Solar13.MonthsInYear;

        /// <summary>
        /// Initializes a new instance of the <see cref="Solar13Arithmetic"/> class with the
        /// specified schema.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="segment"/> is null.</exception>
        /// <exception cref="ArgumentException">The underlying schema does not have the expected
        /// profile <see cref="CalendricalProfile.Solar13"/>.</exception>
        public Solar13Arithmetic(SystemSegment segment) : base(segment)
        {
            Requires.Profile(Schema, CalendricalProfile.Solar13, nameof(segment));
        }

        //
        // Operations on Yemoda
        //

        /// <inheritdoc />
        [Pure]
        protected internal override Yemoda AddDaysViaDayOfMonth(Yemoda ymd, int days)
        {
            Debug.Assert(-MaxDaysViaDayOfMonth_ <= days);
            Debug.Assert(days <= MaxDaysViaDayOfMonth_);

            ymd.Unpack(out int y, out int m, out int d);

            int dom = d + days;
            if (dom < 1)
            {
                if (m == 1)
                {
                    if (y == MinYear) Throw.DateOverflow();
                    y--;
                    (_, m, int d0) = Schema.GetDatePartsAtEndOfYear(y);
                    dom += d0;
                }
                else
                {
                    m--;
                    dom += Schema.CountDaysInMonth(y, m);
                }
            }
            else if (dom > MinDaysInMonth)
            {
                int daysInMonth = Schema.CountDaysInMonth(y, m);
                if (dom > daysInMonth)
                {
                    dom -= daysInMonth;
                    if (m == MonthsInYear)
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
                d < MinDaysInMonth || d < Schema.CountDaysInMonth(y, m)
                    ? new Yemoda(y, m, d + 1)
                : m < MonthsInYear ? Yemoda.AtStartOfMonth(y, m + 1)
                : y < MaxYear ? Yemoda.AtStartOfYear(y + 1)
                : Throw.DateOverflow<Yemoda>();
        }

        //
        // Operations on Yemo
        //

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

        //
        // Non-standard operations
        //

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
    }
}
