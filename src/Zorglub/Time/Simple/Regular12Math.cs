﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Represents the mathematical operations suitable for use by calendars with a fixed number of
    /// 12 months per year.
    /// <para>This class uses the default <see cref="AdditionRules"/> to resolve ambiguities.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed class Regular12Math : RegularMathBase
    {
        /// <summary>
        /// Represents the total number of months in a year.
        /// <para>This field is a constant equal to 12.</para>
        /// </summary>
        private const int MonthsPerYear = 12;

        /// <summary>
        /// Initializes a new instance of the <see cref="Regular12Math"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="calendar"/> is not regular with a
        /// fixed number of 12 months per year.</exception>
        public Regular12Math(Calendar calendar) : base(calendar)
        {
            if (MonthsInYear != MonthsPerYear)
            {
                Throw.Argument(nameof(calendar));
            }
        }

        #region CalendarDate

        /// <inheritdoc />
        [Pure]
        protected internal override CalendarDate AddMonthsCore(CalendarDate date, int months)
        {
            Debug.Assert(date.Cuid == Cuid);

            date.Parts.Unpack(out int y, out int m, out int d);
            m = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsPerYear, out int y0);
            y += y0;

            YearOverflowChecker.Check(y);

            // NB: DateAdditionRule.EndOfMonth.
            int daysInMonth = Schema.CountDaysInMonth(y, m);
            var ymd = new Yemoda(y, m, Math.Min(d, daysInMonth));
            return new CalendarDate(ymd, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        protected internal override int CountMonthsBetweenCore(CalendarDate start, CalendarDate end)
        {
            Debug.Assert(start.Cuid == Cuid);
            Debug.Assert(end.Cuid == Cuid);

            start.Parts.Unpack(out int y0, out int m0);
            end.Parts.Unpack(out int y, out int m);

            return (y - y0) * MonthsPerYear + m - m0;
        }

        #endregion
        #region CalendarMonth

        /// <inheritdoc />
        [Pure]
        protected internal override CalendarMonth AddMonthsCore(CalendarMonth month, int months)
        {
            Debug.Assert(month.Cuid == Cuid);

            month.Parts.Unpack(out int y, out int m);
            m = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsPerYear, out int y0);
            y += y0;

            YearOverflowChecker.Check(y);

            var ym = new Yemo(y, m);
            return new CalendarMonth(ym, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        protected internal override int CountMonthsBetweenCore(CalendarMonth start, CalendarMonth end)
        {
            Debug.Assert(start.Cuid == Cuid);
            Debug.Assert(end.Cuid == Cuid);

            start.Parts.Unpack(out int y0, out int m0);
            end.Parts.Unpack(out int y, out int m);

            return (y - y0) * MonthsPerYear + m - m0;
        }

        #endregion
    }
}
