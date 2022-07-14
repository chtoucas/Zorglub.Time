// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    /// <summary>
    /// Defines the (origin of the) different styles of numbering days.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class DayZero
    {
        /// <summary>
        /// Gets the Monday 1st of January, 1 CE within the Gregorian calendar,
        /// ie the epoch of the Gregorian calendar.
        /// <para>Matches the epoch of the Common Era, Current Era or Vulgar Era.
        /// </para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static DayNumber NewStyle { get; } = DayNumber.Zero;

        /// <summary>
        /// Gets the Saturday 1st of January, 1 CE within the Julian calendar,
        /// ie the epoch of the Julian calendar.
        /// <para>Two days before <see cref="NewStyle"/>, the Gregorian epoch.
        /// </para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static DayNumber OldStyle { get; } = DayNumber.Zero - 2;

        /// <summary>
        /// Gets the day before <see cref="NewStyle"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static DayNumber RataDie { get; } = DayNumber.Zero - 1;
    }
}
