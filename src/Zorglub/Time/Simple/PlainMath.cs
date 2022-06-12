// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    // FIXME(code): impl PlainMath. Improve impl.
    // Do we need stricter validation? or is YearOverflowChecker enough?

    using Zorglub.Time.Core;

    /// <summary>
    /// Provides a plain implementation for <see cref="CalendarMath"/>.
    /// <para>This class uses the default <see cref="AdditionRules"/> to resolve ambiguities.</para>
    /// <para>In practice, we only use this mathematic with non-regular schema; see
    /// <see cref="CalendarMath.Create(Calendar)"/>.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class PlainMath : CalendarMath
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
        protected internal override CalendarMonth AddMonthsCore(CalendarMonth month, int months)
        {
            // First approximation of target year: months / MinMonthsInYear.
            // Then goes backwards in time until we reach the correct result.

            //Debug.Assert(month.Cuid == Cuid);

            //month.Parts.Unpack(out int y, out int m);

            //int monthsInYear = Schema.CountMonthsInYear(y);
            //if (m + months <= monthsInYear)
            //{
            //    return new CalendarMonth(new Yemo(y, m + months), Cuid);
            //}
            //else
            //{
            //    y++;
            //    months -= monthsInYear - m;
            //}

            //while (months > 0)
            //{
            //    monthsInYear = Schema.CountMonthsInYear(y);
            //    if (months > monthsInYear)
            //    {
            //        y++;
            //        months -= monthsInYear;
            //    }
            //}

            //return new CalendarMonth(new Yemo(y, months), Cuid);
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

        /// <inheritdoc />
        [Pure]
        protected internal override int CountMonthsBetweenCore(CalendarMonth start, CalendarMonth end)
        {
            Debug.Assert(start.Cuid == Cuid);
            Debug.Assert(end.Cuid == Cuid);

            return end.Year == start.Year ? end.Month - start.Month
                : start < end ? CountCore(start, end)
                : -CountCore(end, start);

            int CountCore(CalendarMonth start, CalendarMonth end)
            {
                Debug.Assert(start < end);

                start.Parts.Unpack(out int y0, out int m0);
                end.Parts.Unpack(out int y1, out int m1);

                // This can certainly be optimized for calendars for which we
                // know the number of months in a leap-cycle. For instance, this
                // is the case of Lunisolar calendars.
                // Of course, if the calendar is regular, the loop can be
                // replaced by a simple formula.
                int months = Schema.CountMonthsInYear(y0) - m0;
                for (int y = y0 + 1; y < y1; y++)
                {
                    months += Schema.CountMonthsInYear(y);
                }
                months += m1;
                return months;
            }
        }
    }
}
