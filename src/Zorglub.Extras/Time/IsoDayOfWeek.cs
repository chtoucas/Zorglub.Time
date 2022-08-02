// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    // REVIEW(api): avoir une valeur par défaut illégale ne me plaît pas.
    // De plus, comme on a déjà DayOfWeek à notre disposition, cela me semble
    // inutile d'ajouter un nouveau type qui remplit exactement le même rôle.
    // Enfin, on peut toujours utiliser DayOfWeek.ToWeekday().
    // Quant au nom, j'hésite toujours entre IsoDayOfWeek et IsoWeekday.
    // En même temps, j'imagine un autre usage à Weekday (CalendarWeekday).

    /// <summary>
    /// Specifies the ISO day of the week.
    /// <para>The value is in the range from 1 to 7, 1 being attributed to
    /// Monday.</para>
    /// </summary>
    public enum IsoDayOfWeek
    {
        /// <summary>
        /// Indicates an unknown ISO day of the week.
        /// <para>We never use this value, and neither should you.</para>
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates Monday.
        /// </summary>
        Monday = 1,

        /// <summary>
        /// Indicates Tuesday.
        /// </summary>
        Tuesday = 2,

        /// <summary>
        /// Indicates Wednesday.
        /// </summary>
        Wednesday = 3,

        /// <summary>
        /// Indicates Thursday.
        /// </summary>
        Thursday = 4,

        /// <summary>
        /// Indicates Friday.
        /// </summary>
        Friday = 5,

        /// <summary>
        /// Indicates Saturday.
        /// </summary>
        Saturday = 6,

        /// <summary>
        /// Indicates Sunday.
        /// </summary>
        Sunday = 7
    }

    /// <summary>
    /// Provides extension methods for <see cref="IsoDayOfWeek"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class IsoDayOfWeekExtensions
    {
        /// <summary>
        /// Returns true if the specified ISO day of the week is invalid;
        /// otherwise returns false.
        /// </summary>
        // FIXME(code): replace by a Requires.Defined().
        internal static bool IsInvalid(this IsoDayOfWeek @this) =>
            @this < IsoDayOfWeek.Monday || @this > IsoDayOfWeek.Sunday;

        /// <summary>
        /// Converts the specified ISO day of the week to the equivalent
        /// <see cref="DayOfWeek"/> value.
        /// </summary>
        [Pure]
        public static DayOfWeek ToDayOfWeek(this IsoDayOfWeek @this)
        {
            if (@this.IsInvalid()) Throw.ArgumentOutOfRange(nameof(@this));

            return (DayOfWeek)((int)@this % 7);
        }
    }
}
