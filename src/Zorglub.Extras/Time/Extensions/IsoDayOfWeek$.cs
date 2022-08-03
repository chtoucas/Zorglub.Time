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
        // REVIEW(code): replace IsInvalid() by a Requires.Defined(). Not
        // "possible" right now since Requires is defined in the main assembly
        // which does not know anything about IsoDayOfWeek.

        /// <summary>
        /// Returns true if the specified ISO day of the week is invalid; otherwise returns false.
        /// </summary>
        internal static bool IsInvalid(this IsoDayOfWeek @this) =>
            @this < IsoDayOfWeek.Monday || @this > IsoDayOfWeek.Sunday;

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
