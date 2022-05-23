// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design) 👈 PreviewFeatures

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
