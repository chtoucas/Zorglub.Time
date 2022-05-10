// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="DayOfWeek"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class DayOfWeekExtensions2
    {
        /// <summary>
        /// Converts the value of the specified day of the week to the equivalent ISO day of the week.
        /// </summary>
        [Pure]
        public static IsoDayOfWeek ToIsoDayOfWeek(this DayOfWeek @this)
        {
            Requires.Defined(@this);

            return @this == DayOfWeek.Sunday ? IsoDayOfWeek.Sunday : (IsoDayOfWeek)@this;
        }
    }
}
