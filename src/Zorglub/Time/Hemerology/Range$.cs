// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Provides extension methods for <see cref="Range{T}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class RangeExtensions
    {
        /// <summary>
        /// Validates the specified <see cref="DayNumber"/> value.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        public static void Validate(this Range<DayNumber> @this, DayNumber dayNumber)
        {
            if (dayNumber < @this.Min || dayNumber > @this.Max)
            {
                Throw.ArgumentOutOfRange(nameof(dayNumber));
            }
        }

        /// <summary>
        /// Checks that the specified <see cref="DayNumber"/> value does not overflow the range of
        /// supported values.
        /// </summary>
        /// <exception cref="OverflowException"><paramref name="dayNumber"/> would overflow the
        /// range of supported values.</exception>
        internal static void CheckOverflow(this Range<DayNumber> @this, DayNumber dayNumber)
        {
            if (dayNumber < @this.Min || dayNumber > @this.Max)
            {
                Throw.DateOverflow();
            }
        }
    }
}
