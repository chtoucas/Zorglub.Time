// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    internal sealed class PlainCalendricalMath : CalendricalMath
    {
        public PlainCalendricalMath(ICalendricalSchema schema) : base(schema) { }

        /// <inheritdoc />
        [Pure]
        public override Yemoda AddYears(Yemoda ymd, int years, out int roundoff)
        {
            ymd.Unpack(out int y0, out int m, out int d);

            int y = checked(y0 + years);

            if (SupportedYears.Contains(y) == false) Throw.DateOverflow();

            var sch = Schema;
            int monthsInYear = sch.CountMonthsInYear(y);
            if (m > monthsInYear)
            {
                // The target year y has less months than the year y0, we
                // return the end of the target year.
                // roundoff =
                //   "days" after the end of (y0, monthsInYear) until (y0, m, d) included
                //   + diff between end of (y0, monthsInYear) and (y, monthsInYear)
                roundoff = d;
                for (int i = monthsInYear + 1; i < m; i++)
                {
                    roundoff += sch.CountDaysInMonth(y0, i);
                }
                m = monthsInYear;
                int daysInMonth = sch.CountDaysInMonth(y, m);
                roundoff += Math.Max(0, d - daysInMonth);
                return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
            }
            else
            {
                int daysInMonth = sch.CountDaysInMonth(y, m);
                roundoff = Math.Max(0, d - daysInMonth);
                return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
            }
        }

        /// <inheritdoc />
        [Pure]
        public override Yemoda AddMonths(Yemoda ymd, int months, out int roundoff)
        {
            int d = ymd.Day;

            var sch = Schema;
            var (y, m) = sch.Arithmetic.AddMonths(ymd.Yemo, months);

            if (SupportedYears.Contains(y) == false) Throw.DateOverflow();

            int daysInMonth = sch.CountDaysInMonth(y, m);
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

            int monthsInYear = Schema.CountMonthsInYear(y);
            roundoff = Math.Max(0, m - monthsInYear);
            return new Yemo(y, roundoff > 0 ? monthsInYear : m);
        }
    }
}
