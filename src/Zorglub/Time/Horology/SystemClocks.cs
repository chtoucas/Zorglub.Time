// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Horology;

/// <summary>Provides system clocks.</summary>
/// <remarks>This class cannot be inherited.</remarks>
public static class SystemClocks
{
    /// <summary>Gets an instance of the system clock using the current time zone setting on this
    /// machine.</summary>
    /// <remarks>This static property is thread-safe.</remarks>
    public static LocalSystemClock Local => LocalSystemClock.Instance;

    /// <summary>Gets an instance of the system clock using the Coordinated Universal Time (UTC).
    /// </summary>
    /// <remarks>This static property is thread-safe.</remarks>
    public static UtcSystemClock Utc => UtcSystemClock.Instance;
}
