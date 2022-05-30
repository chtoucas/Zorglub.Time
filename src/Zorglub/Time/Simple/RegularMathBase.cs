// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Defines the mathematical operations suitable for use by regular calendars and provides a
    /// base for derived classes.
    /// </summary>
    internal abstract class RegularMathBase : CalendarMath
    {
        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="RegularMathBase"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="calendar"/> is not regular.
        /// </exception>
        protected RegularMathBase(Calendar calendar) : base(calendar, AddAdjustment.EndOfMonth)
        {
            Debug.Assert(calendar != null);

            if (calendar.IsRegular(out int monthsInYear) == false)
            {
                Throw.Argument(nameof(calendar));
            }

            MonthsInYear = monthsInYear;
        }

        /// <summary>
        /// Gets the total number of months in a year.
        /// </summary>
        protected int MonthsInYear { get; }

        #region CalendarDate

        /// <inheritdoc />
        [Pure]
        protected internal sealed override CalendarDate AddYearsCore(CalendarDate date, int years)
        {
            Debug.Assert(date.Cuid == Cuid);

            date.Parts.Unpack(out int y, out int m, out int d);
            y = checked(y + years);

            YearOverflowChecker.Check(y);

            int daysInMonth = Schema.CountDaysInMonth(y, m);
            var ymd = new Yemoda(y, m, Math.Min(d, daysInMonth));
            return new CalendarDate(ymd, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        protected internal sealed override int CountYearsBetweenCore(CalendarDate start, CalendarDate end)
        {
            Debug.Assert(start.Cuid == Cuid);
            Debug.Assert(end.Cuid == Cuid);

            int years = end.Year - start.Year;
            CalendarDate newStart = AddYearsCore(start, years);

            if (start.CompareFast(end) < 0)
            {
                if (newStart.CompareFast(end) > 0) { years--; }
            }
            else
            {
                if (newStart.CompareFast(end) < 0) { years++; }
            }

            return years;
        }

        #endregion
        #region OrdinalDate

        /// <inheritdoc />
        [Pure]
        protected internal sealed override OrdinalDate AddYearsCore(OrdinalDate date, int years)
        {
            Debug.Assert(date.Cuid == Cuid);

            date.Parts.Unpack(out int y, out int doy);
            y = checked(y + years);

            YearOverflowChecker.Check(y);

            int daysInYear = Schema.CountDaysInYear(y);
            var ydoy = new Yedoy(y, Math.Min(doy, daysInYear));
            return new OrdinalDate(ydoy, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        protected internal sealed override int CountYearsBetweenCore(OrdinalDate start, OrdinalDate end)
        {
            Debug.Assert(start.Cuid == Cuid);
            Debug.Assert(end.Cuid == Cuid);

            int years = end.Year - start.Year;
            OrdinalDate newStart = AddYearsCore(start, years);

            if (start.CompareFast(end) < 0)
            {
                if (newStart.CompareFast(end) > 0) { years--; }
            }
            else
            {
                if (newStart.CompareFast(end) < 0) { years++; }
            }

            return years;
        }

        #endregion
        #region CalendarMonth

        /// <inheritdoc />
        [Pure]
        protected internal sealed override CalendarMonth AddYearsCore(CalendarMonth month, int years)
        {
            Debug.Assert(month.Cuid == Cuid);

            month.Parts.Unpack(out int y, out int m);
            y = checked(y + years);

            YearOverflowChecker.Check(y);

            var ym = new Yemo(y, m);
            return new CalendarMonth(ym, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        protected internal sealed override int CountYearsBetweenCore(CalendarMonth start, CalendarMonth end)
        {
            Debug.Assert(start.Cuid == Cuid);
            Debug.Assert(end.Cuid == Cuid);

            return CountYearsBetweenCore(start.FirstDay, end.FirstDay);
        }

        #endregion
    }
}
