// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

/// <summary>
/// Defines the (origin of the) different styles of numbering days.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class DayZero64
{
    /// <summary>
    /// The Monday 1st of January, 1 CE within the Gregorian calendar,
    /// ie the epoch of the Gregorian calendar.
    /// <para>Matches the epoch of the Common Era, Current Era or Vulgar Era.
    /// </para>
    /// <para>This field is read-only.</para>
    /// </summary>
    public static readonly DayNumber64 NewStyle = DayNumber64.Zero;

    /// <summary>
    /// The Saturday 1st of January, 1 CE within the Julian calendar,
    /// ie the epoch of the Julian calendar.
    /// <para>Two days before <see cref="NewStyle"/>, the Gregorian epoch.
    /// </para>
    /// <para>This field is read-only.</para>
    /// </summary>
    public static readonly DayNumber64 OldStyle = DayNumber64.Zero - 2;

    /// <summary>
    /// The day before <see cref="NewStyle"/>.
    /// <para>This field is read-only.</para>
    /// </summary>
    public static readonly DayNumber64 RataDie = DayNumber64.Zero - 1;
}
