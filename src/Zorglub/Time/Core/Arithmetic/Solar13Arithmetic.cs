// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    using static Zorglub.Time.Core.CalendricalConstants;

    /// <summary>
    /// Provides the core mathematical operations on dates for schemas with profile
    /// <see cref="CalendricalProfile.Solar13"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed class Solar13Arithmetic : SolarArithmetic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Solar13Arithmetic"/> class with the
        /// specified schema.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> and <see cref="Yemoda"/> are disjoint.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="schema"/> does not have the expected
        /// profile <see cref="CalendricalProfile.Solar13"/>.</exception>
        public Solar13Arithmetic(CalendricalSchema schema) : base(schema)
        {
            Debug.Assert(schema != null);

            Requires.Profile(schema, CalendricalProfile.Solar13);
        }

        //
        // Operations on Yemoda
        //

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
            else if (dom > Solar.MinDaysInMonth)
            {
                int daysInMonth = Schema.CountDaysInMonth(y, m);
                if (dom > daysInMonth)
                {
                    dom -= daysInMonth;
                    if (m == Solar13.MonthsInYear)
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
                d < Solar.MinDaysInMonth || d < Schema.CountDaysInMonth(y, m)
                    ? new Yemoda(y, m, d + 1)
                : m < Solar13.MonthsInYear ? Yemoda.AtStartOfMonth(y, m + 1)
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

            m = 1 + MathZ.Modulo(checked(m - 1 + months), Solar13.MonthsInYear, out int y0);
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

            return (y1 - y0) * Solar13.MonthsInYear + m1 - m0;
        }
    }
}
