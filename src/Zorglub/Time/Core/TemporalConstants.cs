// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Core;

/// <summary>
/// Provides temporal constants.
/// </summary>
public static partial class TemporalConstants { }

public partial class TemporalConstants // Astronomical
{
    /// <summary>
    /// Represents the fixed difference between TT and TAI, ΔTAT = TT-TAI in seconds.
    /// <para>This field is a constant equal to 32.184.</para>
    /// </summary>
    private const double ΔTAT = 32.184;

    /// <summary>
    /// Represents the fixed difference between TT and TAI, ΔTAT = TT-TAI in days.
    /// <para>This field is a constant equal to 0.00037249999999999995.</para>
    /// </summary>
    internal const double DeltaTAT = ΔTAT / SecondsPerDay;
}

public partial class TemporalConstants // Hours, minutes, seconds
{
    //
    // Hours
    //

    /// <summary>
    /// Represents the number of hours in one day.
    /// <para>This field is a constant equal to 24.</para>
    /// </summary>
    public const int HoursPerDay = 24;

    //
    // Minutes
    //

    /// <summary>
    /// Represents the number of minutes in one hour.
    /// <para>This field is a constant equal to 60.</para>
    /// </summary>
    public const int MinutesPerHour = 60;

    /// <summary>
    /// Represents the number of minutes in one day.
    /// <para>This field is a constant equal to 1440.</para>
    /// </summary>
    public const int MinutesPerDay = MinutesPerHour * HoursPerDay;

    //
    // Seconds SI
    //

    /// <summary>
    /// Represents the number of seconds in one minute.
    /// <para>This field is a constant equal to 60.</para>
    /// </summary>
    public const int SecondsPerMinute = 60;

    /// <summary>
    /// Represents the number of seconds in one hour.
    /// <para>This field is a constant equal to 3600.</para>
    /// </summary>
    public const int SecondsPerHour = SecondsPerMinute * MinutesPerHour;

    /// <summary>
    /// Represents the number of seconds in one day.
    /// <para>This field is a constant equal to 86_400.</para>
    /// </summary>
    public const int SecondsPerDay = SecondsPerHour * HoursPerDay;
}

public partial class TemporalConstants // Subdivisions (SI)
{
    //
    // Milliseconds SI
    //

    /// <summary>
    /// Represents the number of milliseconds in one second.
    /// <para>This field is a constant equal to 1000.</para>
    /// </summary>
    public const int MillisecondsPerSecond = 1000;

    /// <summary>
    /// Represents the number of milliseconds in one minute.
    /// <para>This field is a constant equal to 60_000.</para>
    /// </summary>
    public const int MillisecondsPerMinute = MillisecondsPerSecond * SecondsPerMinute;

    /// <summary>
    /// Represents the number of milliseconds in one hour.
    /// <para>This field is a constant equal to 3_600_000.</para>
    /// </summary>
    public const int MillisecondsPerHour = MillisecondsPerSecond * SecondsPerHour;

    /// <summary>
    /// Represents the number of milliseconds in one day.
    /// <para>This field is a constant equal to 86_400_000.</para>
    /// </summary>
    public const int MillisecondsPerDay = MillisecondsPerSecond * SecondsPerDay;

    //
    // Microseconds SI
    //
    // Parfois, on pourrait utiliser des Int32's mais dans les faits on est
    // généralement obligé de convertir les constantes en Int64 pour éviter
    // des dépassements arithmétiques inattendus, donc autant le faire dès
    // le début.

    /// <summary>
    /// Represents the number of microseconds in one second.
    /// <para>This field is a constant equal to 1_000_000.</para>
    /// </summary>
    public const int MicrosecondsPerSecond = 1000 * MillisecondsPerSecond;

    /// <summary>
    /// Represents the number of microseconds in one minute.
    /// <para>This field is a constant equal to 60_000_000.</para>
    /// </summary>
    public const int MicrosecondsPerMinute = MicrosecondsPerSecond * SecondsPerMinute;

    /// <summary>
    /// Represents the number of microseconds in one hour.
    /// <para>This field is a constant equal to 3_600_000_000.</para>
    /// </summary>
    public const long MicrosecondsPerHour = (long)MicrosecondsPerSecond * SecondsPerHour;

    /// <summary>
    /// Represents the number of microseconds in one day.
    /// <para>This field is a constant equal to 86_400_000_000.</para>
    /// </summary>
    public const long MicrosecondsPerDay = (long)MicrosecondsPerSecond * SecondsPerDay;

    //
    // Nanoseconds SI
    //

    /// <summary>
    /// Represents the number of nanoseconds in one second.
    /// <para>This field is a constant equal to 1_000_000_000.</para>
    /// </summary>
    public const int NanosecondsPerSecond = 100 * TicksPerSecond;

    /// <summary>
    /// Represents the number of nanoseconds in one minute.
    /// <para>This field is a constant equal to 60_000_000_000.</para>
    /// </summary>
    public const long NanosecondsPerMinute = (long)NanosecondsPerSecond * SecondsPerMinute;

    /// <summary>
    /// Represents the number of nanoseconds in one hour.
    /// <para>This field is a constant equal to 3_600_000_000_000.</para>
    /// </summary>
    public const long NanosecondsPerHour = (long)NanosecondsPerSecond * SecondsPerHour;

    /// <summary>
    /// Represents the number of nanoseconds in one day.
    /// <para>This field is a constant equal to 86_400_000_000_000.</para>
    /// </summary>
    public const long NanosecondsPerDay = (long)NanosecondsPerSecond * SecondsPerDay;
}

public partial class TemporalConstants // Misc, ticks
{
    //
    // Ticks
    //

    /// <summary>
    /// Represents the number of ticks in one second.
    /// <para>This field is a constant equal to 10_000_000.</para>
    /// </summary>
    // 1 second = 10^7 ticks, see TimeSpan.
    public const int TicksPerSecond = 10 * MicrosecondsPerSecond;

    /// <summary>
    /// Represents the number of ticks in one minute.
    /// <para>This field is a constant equal to 600_000_000.</para>
    /// </summary>
    public const int TicksPerMinute = TicksPerSecond * SecondsPerMinute;

    /// <summary>
    /// Represents the number of ticks in one hour.
    /// <para>This field is a constant equal to 36_000_000_000.</para>
    /// </summary>
    public const long TicksPerHour = (long)TicksPerSecond * SecondsPerHour;

    /// <summary>
    /// Represents the number of ticks in one day.
    /// <para>This field is a constant equal to 864_000_000_000.</para>
    /// </summary>
    public const long TicksPerDay = (long)TicksPerSecond * SecondsPerDay;

    //
    // Misc constants
    //

    /// <summary>
    /// Represents the number of ticks in one millisecond.
    /// <para>This field is a constant equal to 10_000.</para>
    /// </summary>
    internal const int TicksPerMillisecond = TicksPerSecond / 1000;

    /// <summary>
    /// Represents the number of nanoseconds in one millisecond.
    /// <para>This field is a constant equal to 1_000_000.</para>
    /// </summary>
    internal const int NanosecondsPerMillisecond = NanosecondsPerSecond / 1000;

    /// <summary>
    /// Represents the number of nanoseconds in one microsecond.
    /// <para>This field is a constant equal to 1_000.</para>
    /// </summary>
    internal const int NanosecondsPerMicrosecond = NanosecondsPerMillisecond / 1000;

    /// <summary>
    /// Represents the number of nanoseconds in one tick.
    /// <para>This field is a constant equal to 100.</para>
    /// </summary>
    internal const int NanosecondsPerTick = NanosecondsPerMicrosecond / 10;
}
