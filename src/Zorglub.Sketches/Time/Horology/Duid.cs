// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    // IMPORTANT :
    // Ce n'est jamais une bonne idée de changer les valeurs d'une énumération,
    // mais ici encore moins, même si cette énumération est interne, en effet
    // on utilise celles-ci pour construire l'identifiant publique ; De plus,
    // l'ordre ne doit pas non plus changer, cette fois ce serait
    // Dayscale.s_FactoryById qui ne fonctionnerait plus. Pour la même raison
    // la séquence des valeurs ne doit pas comporter de trous : 0, 1, 2, etc.
    // jusqu'à Size.
    //
    // Si on ajoute des Duid's, màj le test DayscaleIdTests.ToString_Fixed().

    /// <summary>
    /// Specifies the unique identifier of the core dayscale.
    /// <para>Duid = "Dayscale Unique Identifier"</para>
    /// </summary>
    internal enum Duid
    {
        Root = 0,
        Unix,

        AstronomicalJulian,
        Cnes,
        Ccsds,
        ChronologicalJulian,
        DublinJulian,
        Enecoloh,
        Holocene,
        J2000,
        Lilian,
        ModifiedJulian,
        Ntp,
        RataDie,
        ReducedJulian,
        TruncatedJulian,

        Size,
    }
}
