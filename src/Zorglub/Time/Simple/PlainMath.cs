// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Provides a plain implementation for <see cref="CalendarMath"/>.
    /// <para>This class uses the default <see cref="AdditionRules"/> to resolve ambiguities.</para>
    /// <para>In practice, we only use this mathematic with non-regular schema; see
    /// <see cref="CalendarMath.Create(Calendar)"/>.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed class PlainMath : CalendarMath
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlainMath"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        public PlainMath(Calendar calendar) : base(calendar, default) { }

        /// <inheritdoc />
        [Pure]
        protected internal override CalendarDate AddYearsCore(CalendarDate date, int years)
        {
            Debug.Assert(date.Cuid == Cuid);

            date.Parts.Unpack(out int y, out int m, out int d);
            y = checked(y + years);

            YearOverflowChecker.Check(y);

            // NB: DateAdditionRule.EndOfMonth.
            m = Math.Min(m, Schema.CountMonthsInYear(y));
            d = Math.Min(d, Schema.CountDaysInMonth(y, m));
            return new CalendarDate(new Yemoda(y, m, d), Cuid);
        }

        /// <inheritdoc />
        [Pure]
        protected internal override OrdinalDate AddYearsCore(OrdinalDate date, int years)
        {
            Debug.Assert(date.Cuid == Cuid);

            date.Parts.Unpack(out int y, out int doy);
            y = checked(y + years);

            YearOverflowChecker.Check(y);

            // NB: OrdinalAdditionRule.EndOfYear.
            doy = Math.Min(doy, Schema.CountDaysInYear(y));
            return new OrdinalDate(new Yedoy(y, doy), Cuid);
        }

        /// <inheritdoc />
        [Pure]
        protected internal override CalendarMonth AddYearsCore(CalendarMonth month, int years)
        {
            Debug.Assert(month.Cuid == Cuid);

            month.Parts.Unpack(out int y, out int m);
            y = checked(y + years);

            YearOverflowChecker.Check(y);

            // NB: MonthAdditionRule.EndOfYear.
            int monthsInYear = Schema.CountMonthsInYear(y);
            var ym = new Yemo(y, Math.Min(m, monthsInYear));
            return new CalendarMonth(ym, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        protected internal override CalendarDate AddMonthsCore(CalendarDate date, int months)
        {
            //var month = AddMonthsCore(date.CalendarMonth, months);

            //// NB: DateAdditionRule.EndOfMonth.
            //int d = Math.Min(date.Day, month.CountDaysInMonth());
            //return month.GetDayOfMonth(d);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [Pure]
        protected internal override int CountMonthsBetweenCore(CalendarDate start, CalendarDate end)
        {
            //int months = CountMonthsBetweenCore(start.CalendarMonth, end.CalendarMonth);
            //var newStart = AddMonthsCore(start, months);

            //if (start.CompareFast(end) < 0)
            //{
            //    if (newStart.CompareFast(end) > 0) { months--; }
            //}
            //else
            //{
            //    if (newStart.CompareFast(end) < 0) { months++; }
            //}

            //return months;
            throw new NotImplementedException();
        }
    }
}
