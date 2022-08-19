// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    // REVIEW(code): l'absence d'un objet date dédié fait qu'on doit revalider
    // les données à chaque fois.
    // - Today()? see also IDateFactory.
    // - add providers for parts and ordinal parts?
    // Add
    // - GetFirstMonth() -> MonthParts
    // - GetMonthsInYear() -> MonthParts
    // - GetLastMonth() -> MonthParts

    #region Developer Notes

    // To simplify, NakedCalendar do not implement ICalendar<OrdinalParts>.
    //
    // New props/methods not found in ICalendar<>.
    // - Properties
    //   - Name (could be useful for formatting)
    //   - Min/MaxDateParts
    // - Day info (no date type)
    //   - IsIntercalaryDay(y, m, d)
    //   - IsSupplementaryDay(y, m, d)
    // - Conversions
    //   - GetDateParts(dayNumber)
    //   - GetDateParts(y, doy)
    //   - GetOrdinalParts(dayNumber)
    //   - GetOrdinalParts(y, m, d)
    // - Arithmetic
    //   - AddDays(dayNumber, days)

    #endregion

    /// <summary>
    /// Defines a calendar without a dedicated companion date type.
    /// </summary>
    public partial interface INakedCalendar : ICalendar<DayNumber>
    {
        ///// <summary>
        ///// Gets the provider for date parts.
        ///// </summary>
        //IDateProvider<DateParts> DatePartsProvider { get; }
    }

    public partial interface INakedCalendar // Factories
    {
        ///// <summary>
        ///// Obtains the current date on this machine.
        ///// </summary>
        ///// <exception cref="AoorException">Today is not within the calendar boundaries.</exception>
        //[Pure]
        //public DateParts Today() => GetDateParts(DayNumber.Today());
    }

    public partial interface INakedCalendar // Conversions
    {
        /// <summary>
        /// Obtains the date parts for the specified day number.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by this calendar.</exception>
        [Pure] DateParts GetDateParts(DayNumber dayNumber);

        /// <summary>
        /// Obtains the date parts for the specified ordinal date.
        /// </summary>
        /// <exception cref="AoorException">The ordinal date is either invalid or outside the range
        /// of supported dates.</exception>
        [Pure] DateParts GetDateParts(int year, int dayOfYear);

        /// <summary>
        /// Obtains the ordinal date parts for the specified day number.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by this calendar.</exception>
        [Pure] OrdinalParts GetOrdinalParts(DayNumber dayNumber);

        /// <summary>
        /// Obtains the ordinal date parts for the specified date.
        /// </summary>
        /// <exception cref="AoorException">The date is either invalid or outside the range of
        /// supported dates.</exception>
        [Pure] OrdinalParts GetOrdinalParts(int year, int month, int day);
    }
}
