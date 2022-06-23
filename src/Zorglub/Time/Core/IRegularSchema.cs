// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    /// <summary>
    /// Defines a calendrical schema with a fixed number of months in a year.
    /// <para>We say that the schema is <i>regular</i>.</para>
    /// </summary>
    /// <remarks>
    /// <para>Most system calendars implement this interface.</para>
    /// </remarks>
    public interface IRegularSchema : ICalendricalKernel
    {
        /// <summary>
        /// Gets the total number of months in a year.
        /// </summary>
        int MonthsInYear { get; }

        internal static class Twelve
        {
            private const int MonthsInYear = 12;

            [Pure]
            // CIL code size = 11 bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int CountMonthsSinceEpoch(int y, int m) => MonthsInYear * (y - 1) + m - 1;

            // CIL code size = 20 bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
            {
                y = 1 + MathZ.Divide(monthsSinceEpoch, MonthsInYear, out int m0);
                m = 1 + m0;
            }
        }

        internal static class Thirteen
        {
            private const int MonthsInYear = 13;

            [Pure]
            // CIL code size = 11 bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int CountMonthsSinceEpoch(int y, int m) => MonthsInYear * (y - 1) + m - 1;

            // CIL code size = 20 bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
            {
                y = 1 + MathZ.Divide(monthsSinceEpoch, MonthsInYear, out int m0);
                m = 1 + m0;
            }
        }
    }
}
