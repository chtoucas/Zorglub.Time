// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas
{
    internal static class RegularSchema
    {
        public static class Twelve
        {
            public const int MonthsInYear = 12;

            [Pure]
            // CIL code size = XXX bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int CountMonthsSinceEpoch(int y, int m) => MonthsInYear * (y - 1) + m - 1;

            public static void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
            {
                y = 1 + MathZ.Divide(monthsSinceEpoch, MonthsInYear, out int m0);
                m = 1 + m0;
            }

            [Pure]
            // CIL code size = XXX bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int GetStartOfYear(int y) => MonthsInYear * (y - 1);

            [Pure]
            // CIL code size = XXX bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int GetEndOfYear(int y) => MonthsInYear * y - 1;
        }

        public static class Thirteen
        {
            public const int MonthsInYear = 13;

            [Pure]
            // CIL code size = XXX bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int CountMonthsSinceEpoch(int y, int m) => MonthsInYear * (y - 1) + m - 1;

            public static void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
            {
                y = 1 + MathZ.Divide(monthsSinceEpoch, MonthsInYear, out int m0);
                m = 1 + m0;
            }

            [Pure]
            // CIL code size = XXX bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int GetStartOfYear(int y) => MonthsInYear * (y - 1);

            [Pure]
            // CIL code size = XXX bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int GetEndOfYear(int y) => MonthsInYear * y - 1;
        }
    }
}
