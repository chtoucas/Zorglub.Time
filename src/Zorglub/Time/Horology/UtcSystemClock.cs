// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Horology;

using Zorglub.Time.Core;

using static Zorglub.Time.Core.TemporalConstants;

// Beware, we use DateTime.Ticks but
// > "It does not include the number of ticks that are attributable to leap seconds."
// See
// - https://docs.microsoft.com/en-us/dotnet/api/system.datetime.ticks
// - https://github.com/dotnet/dotnet-api-docs/issues/966
//
// Consequences?
// Regarding DateTime,
// - In case of a positive leap second, 23:59:59 is repeated.
// - In case of a negative leap second, 23:59:59 is not valid but I don't
//   know how DateTime actually handles this case. Notice that no negative
//   leap second has ever occured so far.
//
// Being based on the OS clock, this clock is NOT monotonic.
// In fact, it does not matter, one SHOULD NOT use this clock for timing.
// See also
// - https://github.com/dotnet/runtime/issues/15207
// - https://github.com/dotnet/runtime/issues/5883

/// <summary>Represents the system clock using the Coordinated Universal Time (UTC).</summary>
/// <remarks>
/// <para>See <see cref="SystemClocks.Utc"/>.</para>
/// <para>This class cannot be inherited.</para>
/// </remarks>
public sealed class UtcSystemClock : IClock
{
    /// <summary>Represents a singleton instance of the <see cref="UtcSystemClock"/> class.</summary>
    /// <remarks>This field is read-only.</remarks>
    internal static readonly UtcSystemClock Instance = new();

    private UtcSystemClock() { }

    /// <inheritdoc/>
    [Pure]
    public Moment Now()
    {
        // This method works only because DateTime.Ticks does not account
        // for leap seconds!
        var now = DateTime.UtcNow;
        // We could get tickOfDay directly from now.TimeOfDay.Ticks but to
        // build TimeOfDay, DateTime does exactly what we do below.
        ulong ticksSinceZero = (ulong)now.Ticks;
        ulong daysSinceZero = TemporalArithmetic.DivideByTicksPerDay(ticksSinceZero, out ulong tickOfDay);
        ulong millisecondOfDay = tickOfDay / TicksPerMillisecond;

        // NB: the casts should always succeed.
        var dayNumber = new DayNumber((int)daysSinceZero);
        var timeOfDay = TimeOfDay.FromMillisecondOfDay((int)millisecondOfDay);

        return new Moment(dayNumber, timeOfDay);
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber Today()
    {
        // NB: the cast should always succeed.
        int daysSinceZero = (int)TemporalArithmetic.DivideByTicksPerDay(DateTime.UtcNow.Ticks);
        return new DayNumber(daysSinceZero);
    }
}
