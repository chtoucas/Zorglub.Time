// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology;

using Zorglub.Time.Core;

using static Zorglub.Time.Core.TemporalConstants;

// See the comments in UtcSystemClock.

/// <summary>Represents the system clock using the current time zone setting on this machine.
/// </summary>
/// <remarks>
/// <para>See <see cref="SystemClocks.Local"/>.</para>
/// <para>This class cannot be inherited.</para>
/// </remarks>
public sealed class LocalSystemClock : IClock
{
    /// <summary>Represents a singleton instance of the <see cref="LocalSystemClock"/> class.
    /// </summary>
    /// <remarks>This field is read-only.</remarks>
    internal static readonly LocalSystemClock Instance = new();

    private LocalSystemClock() { }

    /// <inheritdoc/>
    [Pure]
    public Moment Now()
    {
        var now = DateTime.Now;
        ulong ticksSinceZero = (ulong)now.Ticks;
        ulong daysSinceZero = TemporalArithmetic.DivideByTicksPerDay(ticksSinceZero, out ulong tickOfDay);
        ulong millisecondOfDay = tickOfDay / TicksPerMillisecond;

        var dayNumber = new DayNumber((int)daysSinceZero);
        var timeOfDay = TimeOfDay.FromMillisecondOfDay((int)millisecondOfDay);

        return new Moment(dayNumber, timeOfDay);
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber Today()
    {
        int daysSinceZero = (int)TemporalArithmetic.DivideByTicksPerDay(DateTime.Now.Ticks);
        return new DayNumber(daysSinceZero);
    }
}
