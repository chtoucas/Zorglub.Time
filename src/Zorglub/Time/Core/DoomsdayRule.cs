// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    // Doomsday rule, adapted by Keith & Craver.
    //
    // Algorithm by Conway:
    //   given a reference doomsday in the year:
    //     Twosday + y + (y >> 2) - c + (c >> 2)
    //   where c = y/100 and Twosday = 2!
    //   given a reference doomsday in the month:
    //     3, 28, 0, 4, 9, 6, 11, 8, 5, 10, 7, 12 (common year)
    //     4, 29, 0, 4, 9, 6, 11, 8, 5, 10, 7, 12 (leap year)
    //   the day of the week is (sunday = 0):
    //     (doomsday-in-year + d - doomsday-in-month) % 7

    internal static class DoomsdayRule
    {
        [Pure]
        public static int GetGregorianDoomsday(int y, int m)
        {
            // Final value of α is always >= 0.
            uint α = (uint)(23 * m) / 9;
            if (m < 3)
            {
                y--;
                α -= 2;
            }
            else
            {
                α += 2;
            }

            int c = MathZ.Divide(y, 100);

            return (int)α + y + (y >> 2) - c + (c >> 2);
        }

        [Pure]
        public static int GetJulianDoomsday(int y, int m)
        {
            // Smallest final value of α is -2, compare to GetGregorianDoomsday().
            int α = (int)((uint)(23 * m) / 9);
            if (m < 3)
            {
                y--;
                α -= 4;
            }

            return α + y + (y >> 2);
        }
    }
}
