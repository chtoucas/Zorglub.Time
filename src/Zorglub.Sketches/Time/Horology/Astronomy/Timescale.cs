// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Astronomy
{
    // Les échelles de temps modernes (d'après François Vermotte, augmenté de
    // commentaires personnels).
    // http://perso.utinam.cnrs.fr/~vernotte/echelles_de_temps.pdf
    //
    // 1. Temps Universel (UT).
    // La seconde est la 1/86400-ème partie du jour solaire moyen.
    // Le temps Universel UT est le temps solaire moyen pour le méridien origine
    // augmenté de 12 heures.
    // UT0 : temps universel brut, exactitude de l'ordre de 0,1 seconde.
    // UT1 : temps universel brut corrigé (2 mois plus tard), avec une
    //       incertitude de l'ordre de 0,1 ms.
    // UT1R : temps universel régularisé.
    // UT2 : temps universel régularisé.
    //
    // 2. Temps des Éphémérides (TE).
    // La seconde est la fraction 1/31.556.925.9747 de l'année tropique pour
    // 1900 hanvier 0 à 12 heures de temps des éphémérides.
    //
    // 3. Temps Atomique International (TAI).
    // Il s'agit de l'échelle de temps à l'heure actuelle la plus précise.
    // Sa mesure implique la coordination entre plusieurs centaines d'horloges
    // atomiques réparties dans différents endroits du globe.
    //
    // 4. Temps Terrestre (TT).
    // Le Temps Terrestre est une échelle abstraite dont une réalisation
    // concrète est donnée par la formule TT = TAI + 32.184 secondes.
    //
    // 5. Temps Universel Coordonné (UTC) à la base de notre temps civil.
    // Compromis entre UT1 et TAI. On suit le TAI mais pas tout à fait sinon
    // on s'éloignerait progressivement de l'échelle de temps qui rythme
    // naturellement notre vie, c'est-à-dire le Temps Universel qui par
    // définition s'accorde avec le jour solaire moyen.
    // UTC est telle que
    // * |UTC - UT1| < 0,9 seconde.
    // * |UTC - TAI| = nombre entier de secondes SI.
    // Le temps local (ou temps civil) est l'UTC dans une zone géographique
    // donnée (fuseau horaire) éventuellement corrigé en fonction de la saison
    // (heure d'hiver, heure d'été).
    //
    // Liste très complète d'échelles de temps.
    // https://www.ucolick.org/~sla/leapsecs/timescales.html
    // https://www.ucolick.org/~sla/leapsecs/

    /// <summary>
    /// Specifies the timescale.
    /// </summary>
    [SuppressMessage("Design", "CA1028:Enum Storage should be Int32")]
    public enum Timescale : byte
    {
        /// <summary>
        /// Unspecified scale.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Universal Time (UT1).
        /// </summary>
        Universal,

        /// <summary>
        /// International Atomic Time (TAI).
        /// </summary>
        Atomic,

        /// <summary>
        /// Terrestrial Time (TT).
        /// </summary>
        Terrestrial,

        /// <summary>
        /// Coordinated Universal Time (UTC).
        /// </summary>
        Utc,
    }
}
