// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Represents the mathematical operations suitable for use by calendars with a fixed number of
    /// 13 months per year.
    /// <para>This class uses the default <see cref="AdditionRules"/> to resolve ambiguities.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed class Regular13Math : RegularMathBase
    {
        /// <summary>
        /// Represents the total number of months in a year.
        /// <para>This field is a constant equal to 13.</para>
        /// </summary>
        private const int MonthsPerYear = 13;

        /// <summary>
        /// Initializes a new instance of the <see cref="Regular13Math"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="calendar"/> is not regular with a
        /// fixed number of 13 months per year.</exception>
        public Regular13Math(Calendar calendar) : base(calendar)
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
            d = Math.Min(d, Schema.CountDaysInMonth(y, m));
            return new CalendarDate(new Yemoda(y, m, d), Cuid);
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

            return new CalendarMonth(new Yemo(y, m), Cuid);
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
