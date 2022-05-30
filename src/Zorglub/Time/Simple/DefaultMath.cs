// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    // FIXME(code): impl DefaultMath.

    using Zorglub.Time.Core;

    /// <summary>
    /// Provides a default implementation for <see cref="CalendarMath"/>.
    /// <para>In practice, we only use this mathematic with non-regular schema; see
    /// <see cref="CalendarMath.Create(Calendar)"/>.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class DefaultMath : CalendarMath
    {
        /// <summary>
        /// Represents the schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly SystemSchema _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMath"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        public DefaultMath(Calendar calendar) : base(calendar, AddAdjustment.EndOfMonth)
        {
            Debug.Assert(calendar != null);

            _schema = calendar.Schema;
        }

        #region CalendarDate

        /// <inheritdoc />
        [Pure]
        protected internal override CalendarDate AddYearsCore(CalendarDate date, int years)
        {
            Debug.Assert(date.Cuid == Cuid);

            //date.Parts.Unpack(out int y, out int m, out int d);
            //y = checked(y + years);

            //// Années complètes : on doit juste vérifier l'année.
            //ShortScope.CheckYearOverflowImpl(y);

            //// Ce n'est pas bon.
            //m = Math.Min(m, _schema.CountMonthsInYear(y));
            //int daysInMonth = _schema.CountDaysInMonth(y, m);
            //var ymd = new Yemoda(y, m, Math.Min(d, daysInMonth));
            //return new CalendarDate(ymd, Cuid);

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [Pure]
        protected internal override CalendarDate AddMonthsCore(CalendarDate date, int months)
        {
            Debug.Assert(date.Cuid == Cuid);

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [Pure]
        protected internal override int CountYearsBetweenCore(CalendarDate start, CalendarDate end)
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

        /// <inheritdoc />
        [Pure]
        protected internal override int CountMonthsBetweenCore(CalendarDate start, CalendarDate end)
        {
            Debug.Assert(start.Cuid == Cuid);
            Debug.Assert(end.Cuid == Cuid);

            start.Parts.Unpack(out int y0, out int m0);
            end.Parts.Unpack(out int y1, out int m1);

            // THIS IS WRONG. See CountMonthsBetweenCore() for CalendarMonth.
            int months = m1 - m0;
            for (int y = y0; y < y1; y++)
            {
                months += _schema.CountMonthsInYear(y);
            }

            var newStart = AddMonthsCore(start, months);

            if (start.CompareFast(end) < 0)
            {
                if (newStart.CompareFast(end) > 0) { months--; }
            }
            else
            {
                if (newStart.CompareFast(end) < 0) { months++; }
            }

            return months;
        }

        #endregion
        #region OrdinalDate

        /// <inheritdoc />
        [Pure]
        protected internal override OrdinalDate AddYearsCore(OrdinalDate date, int years)
        {
            Debug.Assert(date.Cuid == Cuid);

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [Pure]
        protected internal override int CountYearsBetweenCore(OrdinalDate start, OrdinalDate end)
        {
            Debug.Assert(start.Cuid == Cuid);
            Debug.Assert(end.Cuid == Cuid);

            throw new NotImplementedException();
        }

        #endregion
        #region CalendarMonth

        /// <inheritdoc />
        [Pure]
        protected internal override CalendarMonth AddYearsCore(CalendarMonth month, int years)
        {
            Debug.Assert(month.Cuid == Cuid);

            month.Parts.Unpack(out int y, out int m);
            y = checked(y + years);

            YearOverflowChecker.Check(y);

            // NB: MonthAdditionRule.EndOfYear.
            int monthsInYear = _schema.CountMonthsInYear(y);
            var ym = new Yemo(y, Math.Min(m, monthsInYear));
            return new CalendarMonth(ym, Cuid);
        }

        /// <inheritdoc />
        [Pure]
        protected internal override CalendarMonth AddMonthsCore(CalendarMonth month, int months)
        {
            Debug.Assert(month.Cuid == Cuid);

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [Pure]
        protected internal override int CountYearsBetweenCore(CalendarMonth start, CalendarMonth end)
        {
            Debug.Assert(start.Cuid == Cuid);
            Debug.Assert(end.Cuid == Cuid);

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
                start.Parts.Unpack(out int y0, out int m0);
                end.Parts.Unpack(out int y1, out int m1);

                int months = _schema.CountMonthsInYear(y0) - m0;
                for (int y = y0 + 1; y < y1; y++)
                {
                    months += _schema.CountMonthsInYear(y);
                }
                months += m1;
                return months;
            }
        }

        #endregion
    }
}
