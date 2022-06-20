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
            ymd.Unpack(out int y, out int m, out int d);

            y = checked(y + years);

            CheckYearOverflow(y);

            // FIXME(code): WIP, roundoff is not correct.
            int monthsInYear = Schema.CountMonthsInYear(y);
            m = Math.Min(m, monthsInYear);
            int daysInMonth = Schema.CountDaysInMonth(y, m);
            roundoff = Math.Max(0, d - daysInMonth);
            return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
        }

        /// <inheritdoc />
        [Pure]
        public override Yemoda AddMonths(Yemoda ymd, int months, out int roundoff)
        {
            int d = ymd.Day;

            var (y, m) = Schema.Arithmetic.AddMonths(ymd.Yemo, months);

            CheckYearOverflow(y);

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

            CheckYearOverflow(y);

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

            CheckYearOverflow(y);

            int monthsInYear = Schema.CountMonthsInYear(y);
            roundoff = Math.Max(0, m - monthsInYear);
            return new Yemo(y, roundoff > 0 ? monthsInYear : m);
        }
    }
}
