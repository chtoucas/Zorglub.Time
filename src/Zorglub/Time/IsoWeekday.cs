// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

// Avoir une valeur __par défaut__ invalide ne me plaît pas des masses. En même
// temps, je ne vois pas comment faire autrement.
// Dans ce projet, on utilise en priorité l'énumération DayOfWeek.
// DayOfWeek ou Weekday ? Partout ailleurs, on utilise DayOfWeek, mais on adopte
// la terminologie ISO qui semble préférer "weekday" à "day of the week". On
// garde quand même le préfixe ISO pour être bien sûr qu'il n'y ait pas de risque
// de confusion entre les deux énumérations.

/// <summary>Specifies the ISO weekday.</summary>
/// <remarks>A legit value is in the range from 1 to 7, 1 being attributed to Monday.</remarks>
public enum IsoWeekday
{
    /// <summary>Indicates an unknown ISO weekday.</summary>
    /// <remarks>This value is considered to be <i>invalid</i>. We never use it, and neither should
    /// you.</remarks>
    None = 0,

    /// <summary>Indicates Monday.</summary>
    Monday = 1,

    /// <summary>Indicates Tuesday.</summary>
    Tuesday = 2,

    /// <summary>Indicates Wednesday.</summary>
    Wednesday = 3,

    /// <summary>Indicates Thursday.</summary>
    Thursday = 4,

    /// <summary>Indicates Friday.</summary>
    Friday = 5,

    /// <summary>Indicates Saturday.</summary>
    Saturday = 6,

    /// <summary>Indicates Sunday.</summary>
    Sunday = 7
}

/// <summary>Provides extension methods for <see cref="IsoWeekday"/> and <see cref="DayOfWeek"/>.
/// </summary>
/// <remarks>This class cannot be inherited.</remarks>
public static class IsoWeekdayExtensions
{
    /// <summary>Converts the value of the specified day of the week to the equivalent
    /// <see cref="IsoWeekday"/>.</summary>
    [Pure]
    public static IsoWeekday ToIsoWeekday(this DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        return dayOfWeek == DayOfWeek.Sunday ? IsoWeekday.Sunday : (IsoWeekday)dayOfWeek;
    }

    /// <summary>Converts the value of the specified <see cref="IsoWeekday"/> to the equivalent
    /// <see cref="DayOfWeek"/> value.</summary>
    [Pure]
    public static DayOfWeek ToDayOfWeek(this IsoWeekday isoWeekday)
    {
        Requires.Defined(isoWeekday);

        return (DayOfWeek)((int)isoWeekday % 7);
    }
}
