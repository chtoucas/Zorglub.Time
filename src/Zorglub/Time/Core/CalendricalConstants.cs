// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

/// <summary>Provides calendrical constants.</summary>
/// <remarks>This class cannot be inherited.</remarks>
public static class CalendricalConstants
{
    /// <summary>Represents the number of days in a wandering year.</summary>
    /// <remarks>This field is constant equal to 365.</remarks>
    // Nombre de jours dans une année vague ("wandering year"),
    // l'approximation la plus grossière de l'année tropique.
    public const int DaysInWanderingYear = 365;

    /// <summary>Represents the number of days per cycle of four julian years.</summary>
    /// <remarks>
    /// <para>This field is constant equal to 1461.</para>
    /// <para>On average, a year is 365.25 days long.</para>
    /// </remarks>
    // Année julienne = moyenne d'une année dans le calendrier julien.
    // Cycle de 4 ans = 1 année bissextile
    // Durée d'un cycle = (4 * 365 + 1) jours = 1461 jours.
    // Année julienne := 365,25 jours.
    public const int DaysPer4JulianYearCycle = 4 * DaysInWanderingYear + 1;

    /// <summary>Represents the number of days in a week.</summary>
    /// <remarks>This field is constant equal to 7.</remarks>
    public const int DaysInWeek = 7;

    /// <summary>Provides constants common to <see cref="CalendricalProfile.Solar12"/> and
    /// <see cref="CalendricalProfile.Solar13"/>.</summary>
    /// <remarks>This class cannot be inherited.</remarks>
    internal static class Solar
    {
        /// <summary>Represents the minimum total number of days there is at least in a year.
        /// </summary>
        /// <remarks>This field is a constant equal to 365.</remarks>
        public const int MinDaysInYear = 365;

        /// <summary>Represents the minimum total number of days there is at least in a month.
        /// </summary>
        /// <remarks>This field is a constant equal to 28.</remarks>
        public const int MinDaysInMonth = 28;
    }

    /// <summary>Provides constants related to <see cref="CalendricalProfile.Solar12"/>.</summary>
    /// <remarks>This class cannot be inherited.</remarks>
    internal static class Solar12
    {
        /// <summary>Represents the number of months in a year.</summary>
        /// <remarks>This field is a constant equal to 12.</remarks>
        public const int MonthsInYear = 12;
    }

    /// <summary>Provides constants related to <see cref="CalendricalProfile.Solar13"/>.</summary>
    /// <remarks>This class cannot be inherited.</remarks>
    internal static class Solar13
    {
        /// <summary>Represents the number of months in a year.</summary>
        /// <remarks>This field is a constant equal to 13.</remarks>
        public const int MonthsInYear = 13;
    }

    /// <summary>Provides constants related to <see cref="CalendricalProfile.Lunar"/>.</summary>
    /// <remarks>This class cannot be inherited.</remarks>
    internal static class Lunar
    {
        /// <summary>Represents the number of months in a year.</summary>
        /// <remarks>This field is a constant equal to 12.</remarks>
        public const int MonthsInYear = 12;

        /// <summary>Represents the minimum total number of days there is at least in a year.
        /// </summary>
        /// <remarks>This field is a constant equal to 354.</remarks>
        public const int MinDaysInYear = 354;

        /// <summary>Represents the minimum total number of days there is at least in a month.
        /// </summary>
        /// <remarks>
        /// <para>Usually, months are either "full" (30-day long) or "hollow" (29-day long).</para>
        /// <para>This field is a constant equal to 29.</para>
        /// </remarks>
        public const int MinDaysInMonth = 29;
    }

    /// <summary>Provides constants related to <see cref="CalendricalProfile.Lunisolar"/>.</summary>
    /// <remarks>This class cannot be inherited.</remarks>
    internal static class Lunisolar
    {
        /// <summary>Represents the minimum total number of days there is at least in a year.
        /// </summary>
        /// <remarks>This field is a constant equal to 353.</remarks>
        public const int MinDaysInYear = 353;

        /// <summary>Represents the minimum total number of days there is at least in a month.
        /// </summary>
        /// <remarks>This field is a constant equal to 29.</remarks>
        public const int MinDaysInMonth = 29;
    }
}
