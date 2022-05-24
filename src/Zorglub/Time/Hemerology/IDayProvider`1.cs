// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design) 👈 PreviewFeatures

// TODO(api): I don't like these interfaces...
// Put GetDaysInYear() and GetDaysInMonth() elsewhere?

namespace Zorglub.Time.Hemerology
{
    using System;

    //
    // Year and month boundaries
    //
    // Static or not? If not static, property or not?
    // On utilise non pas des propriétés mais des méthodes car en général on
    // ne peut pas dire si le résultat est dans les limites du calendrier
    // sous-jacent, on peut donc être amené à lever une exception.
    // De plus, GetEndOfYear() n'est pas une opération totalement
    // élémentaire. Quant à GetStartOfYear(), pour des questions de symétrie
    // on va aussi opter pour une méthode, même si utiliser une propriété
    // aurait été plus appropriée.

    public interface IMonthEndpointsProvider<TDate>
    {
        /// <summary>
        /// Obtains the first day of the month to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="day"/> is null.</exception>
        [Pure] static abstract TDate GetStartOfMonth(TDate day);

        /// <summary>
        /// Obtains the last day of the month to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="day"/> is null.</exception>
        [Pure] static abstract TDate GetEndOfMonth(TDate day);
    }

    public interface IYearEndpointsProvider<TDate>
    {
        /// <summary>
        /// Obtains the first day of the year to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="day"/> is null.</exception>
        [Pure] static abstract TDate GetStartOfYear(TDate day);

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="day"/> is null.</exception>
        [Pure] static abstract TDate GetEndOfYear(TDate day);
    }

    /// <summary>
    /// Provides a set of method to create the special dates of a year or a month, the two
    /// subdivisions of time used by most calendar systems.
    /// </summary>
    /// <typeparam name="TDate">The type of date object to return.</typeparam>
    public interface IDayProvider<out TDate>
    {
        /// <summary>
        /// Enumerates the days in the specified year.
        /// </summary>
        /// <exception cref="AoorException">The year is not within the calendar boundaries.
        /// </exception>
        [Pure] IEnumerable<TDate> GetDaysInYear(int year);

        /// <summary>
        /// Enumerates the days in the specified month.
        /// </summary>
        /// <exception cref="AoorException">The month is not within the calendar boundaries.
        /// </exception>
        [Pure] IEnumerable<TDate> GetDaysInMonth(int year, int month);

        /// <summary>
        /// Obtains the date for the first supported day of the specified year.
        /// </summary>
        /// <exception cref="AoorException">The year is not within the calendar boundaries.
        /// </exception>
        [Pure] TDate GetStartOfYear(int year);

        /// <summary>
        /// Obtains the date for the last supported day of the specified year.
        /// </summary>
        /// <exception cref="AoorException">The year is not within the calendar boundaries.
        /// </exception>
        [Pure] TDate GetEndOfYear(int year);

        /// <summary>
        /// Obtains the date for the first supported day of the specified month.
        /// </summary>
        /// <exception cref="AoorException">The month is not within the calendar boundaries.
        /// </exception>
        [Pure] TDate GetStartOfMonth(int year, int month);

        /// <summary>
        /// Obtains the date for the last supported day of the specified month.
        /// </summary>
        /// <exception cref="AoorException">The month is not within the calendar boundaries.
        /// </exception>
        [Pure] TDate GetEndOfMonth(int year, int month);
    }
}
