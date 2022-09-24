// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks;

using System.Security.Cryptography;

using Zorglub.Time;
using Zorglub.Time.Core;

public abstract class GJBenchmarkBase : BenchmarkBase
{
    protected GJBenchmarkBase() { }

    protected BenchmarkOption Option { get; init; }

    /// <summary>Gets the Gregorian/Julian year.</summary>
    protected int Year
    {
        get =>
            Option switch
            {
                BenchmarkOption.Now => Now.Year,
                BenchmarkOption.Fixed => Fixed.Year,
                BenchmarkOption.Slow => Slow.Year,
                BenchmarkOption.Random => Random.Year,
                _ => Now.Year,
            };
    }

    /// <summary>Gets the Gregorian/Julian month.</summary>
    protected int Month
    {
        get =>
            Option switch
            {
                BenchmarkOption.Now => Now.Month,
                BenchmarkOption.Fixed => Fixed.Month,
                BenchmarkOption.Slow => Slow.Month,
                BenchmarkOption.Random => Random.Month,
                _ => Now.Month,
            };
    }

    /// <summary>Gets the Gregorian/Julian day.</summary>
    protected int Day
    {
        get =>
            Option switch
            {
                BenchmarkOption.Now => Now.Day,
                BenchmarkOption.Fixed => Fixed.Day,
                BenchmarkOption.Slow => Slow.Day,
                BenchmarkOption.Random => Random.Day,
                _ => Now.Day,
            };
    }

    /// <summary>Gets the Gregorian day of the year.</summary>
    protected int DayOfYear
    {
        get =>
            Option switch
            {
                BenchmarkOption.Now => Now.DayOfYear,
                BenchmarkOption.Fixed => Fixed.DayOfYear,
                BenchmarkOption.Slow => Slow.DayOfYear,
                BenchmarkOption.Random => Random.DayOfYear,
                _ => Now.DayOfYear,
            };
    }

    /// <summary>Gets the Gregorian value for daysSinceEpoch.</summary>
    protected int DaysSinceEpoch
    {
        get =>
            Option switch
            {
                BenchmarkOption.Now => Now.DaysSinceEpoch,
                BenchmarkOption.Fixed => Fixed.DaysSinceEpoch,
                BenchmarkOption.Slow => Slow.DaysSinceEpoch,
                BenchmarkOption.Random => Random.DaysSinceEpoch,
                _ => Now.DaysSinceEpoch,
            };
    }

    /// <summary>Gets the Gregorian day number.</summary>
    protected DayNumber DayNumber => DayZero.NewStyle + DaysSinceEpoch;

    protected Yemoda Ymd => new(Year, Month, Day);
    protected Yemo Ym => new(Year, Month);

#pragma warning disable CA1810 // Initialize reference type static fields inline (Performance)

    private static class Now
    {
        internal static readonly int Year;
        internal static readonly int Month;
        internal static readonly int Day;
        internal static readonly int DayOfYear;
        internal static readonly int DaysSinceEpoch;

        static Now()
        {
            var now = DateTime.Now;
            Year = now.Year;
            Month = now.Month;
            Day = now.Day;
            DayOfYear = now.DayOfYear;
            DaysSinceEpoch = (int)TemporalArithmetic.DivideByTicksPerDay(now.Ticks);
        }
    }

    // WARNING: en utilisant des constantes, on triche un peu car
    // .NET va certainement effectuer des optimisations qu'il ne
    // ferait pas en temps normal. Ceci dit, le comportement est le
    // même pour tout le monde.

    /// <summary>Provides fixed Gregorian data (fast track).</summary>
    private static class Fixed
    {
        // NB : NodaTime utilise un cache pour les dates de 1900 à 2100 dans
        // le calendrier grégorien.

        internal const int Year = 2017;
        internal const int Month = 11;
        internal const int Day = 19;
        internal const int DayOfYear = 323;
        internal const int DaysSinceEpoch = 736_651;
    }

    /// <summary>Provides fixed Gregorian data (slow track).</summary>
    private static class Slow
    {
        internal const int Year = 2020;
        internal const int Month = 12;
        internal const int Day = 31;
        internal const int DayOfYear = 366;
        internal const int DaysSinceEpoch = 737_789;
    }

    private static class Random
    {
        internal static readonly int Year;
        internal static readonly int Month;
        internal static readonly int Day;
        internal static readonly int DayOfYear;
        internal static readonly int DaysSinceEpoch;

        static Random()
        {
            Year = RandomNumberGenerator.GetInt32(2000, 2100);
            Month = RandomNumberGenerator.GetInt32(1, 12);
            Day = RandomNumberGenerator.GetInt32(1, 28);

            // WARNING: les champs suivants ne correspondent aux autres.
            DayOfYear = RandomNumberGenerator.GetInt32(1, 365);
            DaysSinceEpoch = RandomNumberGenerator.GetInt32(600_000, 800_000);
        }
    }

#pragma warning restore CA1810
}
