// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System;

    using Zorglub.Time.Hemerology.Scopes;

    // TODO(api): adjusters. Adjust()
    // Merge IAdjustableDate w/ IDate? Hum no because we have also IAffineDate.
    // Le seul avantage à avoir ces méthodes sur un objet date est qu'on peut ne
    // pas pas avoir à revalider les paramètres.
    //
    // Static or not? If not static, property or not?
    // On utilise non pas des propriétés mais des méthodes car en général on
    // ne peut pas dire si le résultat est dans les limites du calendrier
    // sous-jacent, on peut donc être amené à lever une exception.
    // De plus, GetEndOfYear() n'est pas une opération totalement
    // élémentaire. Quant à GetStartOfYear(), pour des questions de symétrie
    // on va aussi opter pour une méthode, même si utiliser une propriété
    // aurait été plus appropriée.
    //
    // Namespace Simple. We have three date types: ordinal, normal, day count.
    // Only CalendarDate implements IAdjustableDate. The two others have their
    // own adjustment methods. For instance, OrdinalDate has WithYear() and
    // WithDayOfYear(), and to change the month, first we convert the ordinal
    // date to a CalendarDate, then call WithMonth() on the result.

    // IDateAdjuster<TDate> provides a different API for the part of
    // ICalendar<TDate> dealing with the creation of new (single) TDate instances.
    //
    // We don't include GetDayOfMonth(TDate, dayOfMonth) and other methods
    // adjusting a single date component, they should be methods on TDate itself,
    // e.g. WithDayOfMonth(). The same goes with the adjustment of the day of
    // the week.
    //
    // It would be natural to constraint TDate to IDateable, but we currently
    // intent to impl this interface with (fixed) date types like DayNumber.
    // This is also in sync with ICalendar<TDate> which does put any constraint
    // on the date type.
    //
    // For a default implementation, see DateAdjuster.
    // A custom implementation SHOULD only be done when we can avoid to validate
    // the result; see for instance SpecialAdjuster where we know in
    // advance that the result is guaranteed to be within the calendar boundaries.

    /// <summary>
    /// Defines an adjuster for <typeparamref name="TDate"/>.
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public interface IDateAdjuster<TDate>
    {
        /// <summary>
        /// Gets the calendar scope.
        /// </summary>
        CalendarScope Scope { get; }

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified date.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.</exception>
        /// <exception cref="AoorException">The result would overflow the range of supported dates.
        /// </exception>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
        [Pure] TDate GetStartOfYear(TDate date);

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified date.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.</exception>
        /// <exception cref="AoorException">The result would overflow the range of supported dates.
        /// </exception>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
        [Pure] TDate GetEndOfYear(TDate date);

        /// <summary>
        /// Obtains the first day of the month to which belongs the specified date.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.</exception>
        /// <exception cref="AoorException">The result would overflow the range of supported dates.
        /// </exception>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
        [Pure] TDate GetStartOfMonth(TDate date);

        /// <summary>
        /// Obtains the last day of the month to which belongs the specified date.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="date"/> is null.</exception>
        /// <exception cref="AoorException">The result would overflow the range of supported dates.
        /// </exception>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
        [Pure] TDate GetEndOfMonth(TDate date);

        //
        // Adjustments for the core parts
        //

        /// <summary>
        /// Adjusts the year field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
        [Pure] TDate AdjustYear(TDate date, int newYear);

        /// <summary>
        /// Adjusts the month field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
        [Pure] TDate AdjustMonth(TDate date, int newMonth);

        /// <summary>
        /// Adjusts the day of the month field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
        [Pure] TDate AdjustDay(TDate date, int newDay);

        /// <summary>
        /// Adjusts the day of the year field to the specified value, yielding a new date.
        /// </summary>
        /// <exception cref="AoorException">The resulting date would be invalid.</exception>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
        [Pure] TDate AdjustDayOfYear(TDate date, int newDayOfYear);
    }
}
