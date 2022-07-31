// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design) 👈 PreviewFeatures

// TODO(api): I don't like these interfaces...
// - use raw params ("year" not "day") to be implemented by "single" date types.
// - create equivalent adjusters w/ DateParts & co to be used with an Adjust()
//   method on a date type.
//
// Le seul avantage à avoir ces méthodes sur un objet date est qu'on n'a pas à
// revalider les paramètres.
// On pourrait rajouter la méthode suivante à l'API de ICalendar<T>
// > T GetStartOfYear(T)
// Cela nous permettrait de gérer le cas de DayNumber pour lequel on ne dispose
// pas de méthode équivalente.

namespace Zorglub.Time.Hemerology
{
    using System;

    // These interfaces are meant to be implemented by specialized date types,
    // not by poly-calendar types.

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

    [Obsolete("TO BE REMOVED")]
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

    [Obsolete("TO BE REMOVED")]
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
}
