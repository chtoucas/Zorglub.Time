// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IsoDayOfWeek"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class IsoDayOfWeekExtensions
    {
        /// <summary>
        /// Returns true if the specified ISO day of the week is invalid; otherwise returns false.
        /// </summary>
        // FIXME(code): replace by a Requires.Defined().
        internal static bool IsInvalid(this IsoDayOfWeek @this) =>
            @this < IsoDayOfWeek.Monday || @this > IsoDayOfWeek.Sunday;

        /// <summary>
        /// Obtains the value of the specified <see cref="IsoDayOfWeek"/> as an ISO weekday number.
        /// </summary>
        /// <exception cref="AoorException">The specified day of the week is not valid.</exception>
        [Pure]
        public static int ToIsoWeekday(this IsoDayOfWeek @this)
        {
            if (@this.IsInvalid()) Throw.ArgumentOutOfRange(nameof(@this));

            return @this == IsoDayOfWeek.Sunday ? 7 : (int)@this;
        }

        /// <summary>
        /// Converts the value of the specified <see cref="IsoDayOfWeek"/> to the equivalent
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
