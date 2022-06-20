// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;

    // FIXME(code): Impl PlainMath. Improve impl.
    // Do we need stricter validation? or is YearOverflowChecker enough?
    //
    // To be done:
    // * Lunisolar arithmetic tests
    // * CIL code size
    // * Schemas: partial impl for archetypal, Pax and Hebrew schemas
    //
    // Count...Between()
    // - Give definition in terms of addition <- therefore depends on the addition rules
    // - Default impl works if there is no extremely short years
    //
    // Difference, add Add(Period). NextMonth() & co. Week ops. Quarter ops.
    //
    // Add tests related to the warning below. Question: should we provide an
    // engine for which the operations always give the same result for dates and
    // ordinal dates? and how could this be done? Hum no, use conversion from
    // date to ordinal repr.

    // Only handle calendrical objects related to the Calendar system; for other
    // systems, see CalendricalMath.
    //
    // WARNING: adding years to the date and to the ordinal date of the -same-
    // day may lead to different results. For instance, in the Gregorian
    // calendar, if we consider the February 29 of a leap year, adding one year
    // to the ordinal date of this day is unambiguous, whereas the same operation
    // applied to the date of this day overflows.

    /// <summary>
    /// Defines the non-standard mathematical operations suitable for use with a given calendar and
    /// provides a base for derived classes.
    /// </summary>
    public abstract partial class CalendarMath
    {
        /// <summary>
        /// Called from constructors in derived classes to initialize the <see cref="CalendarMath"/>
        /// class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        protected CalendarMath(Calendar calendar, AdditionRules additionRules)
        {
            Calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
            AdditionRules = additionRules;

            Cuid = calendar.Id;
            Schema = calendar.Schema;
            YearOverflowChecker = calendar.YearOverflowChecker;
        }

        /// <summary>
        /// Gets the calendar.
        /// </summary>
        // NB: do not remove this property. For instance, if internally we can
        // use YearOverflowChecker, externally we cannot, but there is
        // Calendar.SupportedYears instead. It's also useful if we want to be
        // able to create new date objects...
        public Calendar Calendar { get; }

        /// <summary>
        /// Gets the strategy employed to resolve ambiguities.
        /// </summary>
        public AdditionRules AdditionRules { get; }

        /// <summary>
        /// Gets the checker for overflows of the range of years.
        /// </summary>
        private protected IOverflowChecker<int> YearOverflowChecker { get; }

        /// <summary>
        /// Gets the calendrical schema.
        /// </summary>
        protected SystemSchema Schema { get; }

        /// <summary>
        /// Gets the ID of the underlying calendar.
        /// </summary>
        internal Cuid Cuid { get; }

        /// <summary>
        /// Creates the default instance of the <see cref="CalendarMath"/> class for the specified
        /// calendar.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        [Pure]
        internal static CalendarMath Create(Calendar calendar)
        {
            Requires.NotNull(calendar);

            // This method could be public, but it would feel odd, indeed this
            // class has only two public method/prop: Calendar & AdditionRules.

            return calendar.IsRegular(out _) ? new RegularMath(calendar) : new PlainMath(calendar);
        }

        /// <summary>
        /// Validates the specified <see cref="Simple.Cuid"/>.
        /// </summary>
        /// <exception cref="ArgumentException">The validation failed.</exception>
        private void ValidateCuid(Cuid cuid, string paramName)
        {
            if (cuid != Cuid) Throw.BadCuid(paramName, Cuid, cuid);
        }
    }

    public partial class CalendarMath // CalendarDate
    {
        /// <summary>
        /// Adds a number of years to the year field of the specified date.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="date"/> does not belong to the
        /// underlying calendar.</exception>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported dates.</exception>
        [Pure]
        public CalendarDate AddYears(CalendarDate date, int years)
        {
            ValidateCuid(date.Cuid, nameof(date));
            return AddYearsCore(date, years);
        }

        /// <summary>
        /// Adds a number of years to the year field of the specified date.
        /// <para>This method does NOT validate the date parameter.</para>
        /// </summary>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported dates.</exception>
        [Pure]
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "date", Justification = "VB.NET Date.")]
        protected internal abstract CalendarDate AddYearsCore(CalendarDate date, int years);

        /// <summary>
        /// Adds a number of months to the month field of the specified date.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="date"/> does not belong to the
        /// underlying calendar.</exception>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported dates.</exception>
        [Pure]
        public CalendarDate AddMonths(CalendarDate date, int months)
        {
            ValidateCuid(date.Cuid, nameof(date));
            return AddMonthsCore(date, months);
        }

        /// <summary>
        /// Adds a number of months to the months field of the specified date.
        /// <para>This method does NOT validate the date parameter.</para>
        /// </summary>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported dates.</exception>
        [Pure]
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "date", Justification = "VB.NET Date.")]
        protected internal abstract CalendarDate AddMonthsCore(CalendarDate date, int months);

        /// <summary>
        /// Counts the number of years between the two specified dates.
        /// </summary>
        /// <exception cref="ArgumentException">One of the paramaters does not belong to the
        /// underlying calendar.</exception>
        [Pure]
        public int CountYearsBetween(CalendarDate start, CalendarDate end)
        {
            ValidateCuid(start.Cuid, nameof(start));
            ValidateCuid(end.Cuid, nameof(end));
            return CountYearsBetweenCore(start, end);
        }

        /// <summary>
        /// Counts the number of months between the two specified dates.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        [Pure]
        protected internal int CountYearsBetweenCore(CalendarDate start, CalendarDate end)
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

        /// <summary>
        /// Counts the number of months between the two specified dates.
        /// </summary>
        /// <exception cref="ArgumentException">One of the paramaters does not belong to the
        /// underlying calendar.</exception>
        [Pure]
        public int CountMonthsBetween(CalendarDate start, CalendarDate end)
        {
            ValidateCuid(start.Cuid, nameof(start));
            ValidateCuid(end.Cuid, nameof(end));
            return CountMonthsBetweenCore(start, end);
        }

        /// <summary>
        /// Counts the number of months between the two specified dates.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        [Pure]
        protected internal int CountMonthsBetweenCore(CalendarDate start, CalendarDate end)
        {
            Debug.Assert(start.Cuid == Cuid);
            Debug.Assert(end.Cuid == Cuid);

            int months = Schema.Arithmetic.CountMonthsBetween(start.Parts.Yemo, end.Parts.Yemo);
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
    }

    public partial class CalendarMath // OrdinalDate
    {
        /// <summary>
        /// Adds a number of years to the year field of the specified ordinal date.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="date"/> does not belong to the
        /// underlying calendar.</exception>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported dates.</exception>
        [Pure]
        public OrdinalDate AddYears(OrdinalDate date, int years)
        {
            ValidateCuid(date.Cuid, nameof(date));
            return AddYearsCore(date, years);
        }

        /// <summary>
        /// Adds a number of years to the year field of the specified ordinal date.
        /// <para>This method does NOT validate the date parameter.</para>
        /// </summary>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported dates.</exception>
        [Pure]
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "date", Justification = "VB.NET Date.")]
        protected internal abstract OrdinalDate AddYearsCore(OrdinalDate date, int years);

        /// <summary>
        /// Counts the number of years between the two specified ordinal dates.
        /// </summary>
        /// <exception cref="ArgumentException">One of the paramaters does not belong to the
        /// underlying calendar.</exception>
        [Pure]
        public int CountYearsBetween(OrdinalDate start, OrdinalDate end)
        {
            ValidateCuid(start.Cuid, nameof(start));
            ValidateCuid(end.Cuid, nameof(end));
            return CountYearsBetweenCore(start, end);
        }

        /// <summary>
        /// Counts the number of months between the two specified ordinal dates.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        [Pure]
        protected internal int CountYearsBetweenCore(OrdinalDate start, OrdinalDate end)
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
    }

    public partial class CalendarMath // CalendarMonth
    {
        /// <summary>
        /// Adds a number of years to the year field of the specified month.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="month"/> does not belong to the
        /// underlying calendar.</exception>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported months.</exception>
        [Pure]
        public CalendarMonth AddYears(CalendarMonth month, int years)
        {
            ValidateCuid(month.Cuid, nameof(month));
            return AddYearsCore(month, years);
        }

        /// <summary>
        /// Adds a number of years to the year field of the specified month.
        /// <para>This method does NOT validate the date parameter.</para>
        /// </summary>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported months.</exception>
        [Pure]
        protected internal abstract CalendarMonth AddYearsCore(CalendarMonth month, int years);

        /// <summary>
        /// Counts the number of years between the two specified months.
        /// </summary>
        /// <exception cref="ArgumentException">One of the paramaters does not belong to the
        /// underlying calendar.</exception>
        [Pure]
        public int CountYearsBetween(CalendarMonth start, CalendarMonth end)
        {
            ValidateCuid(start.Cuid, nameof(start));
            ValidateCuid(end.Cuid, nameof(end));
            return CountYearsBetweenCore(start, end);
        }

        /// <summary>
        /// Counts the number of years between the two specified months.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        [Pure]
        protected internal int CountYearsBetweenCore(CalendarMonth start, CalendarMonth end)
        {
            Debug.Assert(start.Cuid == Cuid);
            Debug.Assert(end.Cuid == Cuid);

            int years = end.Year - start.Year;
            CalendarMonth newStart = AddYearsCore(start, years);

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
    }
}
