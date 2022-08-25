// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    // Timescale: internally the BCL uses the UTC timescale but it ignores leap
    // seconds.

    /// <summary>
    /// Provides system clocks.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class SystemClocks
    {
        /// <summary>
        /// Gets an instance of the system clock using the current time zone setting on this machine.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static LocalSystemClock Local => LocalSystemClock.Instance;

        /// <summary>
        /// Gets an instance of the system clock using the UTC time zone.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static UtcSystemClock Utc => UtcSystemClock.Instance;
    }
}
