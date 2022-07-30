// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;
using Zorglub.Time.Simple;

using static Zorglub.Time.Extensions.Unboxing;

// On en profite pour explorer les différentes manières d'initialiser un
// champs statique en différé.
// - internal field of a private class.
//   This is the best possible choice. A bit verbose, but thread-safe and lazy.
// - Interlocked.CompareExchange()
//   Second best choice.
// - Lazy<T> or (better?) LazyInitializer.EnsureInitialized

/// <summary>
/// Provides a compendium of calendars.
/// <para>Unless specified otherwise, the calendars listed here do not allow
/// dates prior their epochal origin.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public static partial class CalendarZoo { }

// More historically accurate calendars of type NakedCalendar.
// - GenuineGregorian
// - GenuineJulian
// - FrenchRevolutionary
// Lazy initialization, thread-safe because we can create the same calendar twice.
// While thread-safe, under heavy load, there is a chance that the field will be
// initialized more than once, which means that during a very short perido of
// time we might receive different references (these are not singleton calendars).
public partial class CalendarZoo
{
    private static BoundedBelowCalendar? s_GenuineGregorian;
    /// <summary>
    /// Gets the Gregorian calendar with dates on or after the 15th of
    /// October 1582 CE.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static BoundedBelowCalendar GenuineGregorian =>
        s_GenuineGregorian ??=
            (from x in CivilSchema.GetInstance()
             select new BoundedBelowCalendar(
                 "Genuine Gregorian",
                 x,
                 DayZero.NewStyle,
                 new DateParts(1582, 10, 15))
             ).Unbox();

    private static MinMaxYearCalendar? s_GenuineJulian;
    /// <summary>
    /// Gets the Julian calendar with dates on or after the year 8.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar GenuineJulian =>
        s_GenuineJulian ??=
            (from x in JulianSchema.GetInstance()
             select MinMaxYearCalendar.WithMinYear(
                "Genuine Julian",
                x,
                DayZero.OldStyle,
                8)
             ).Unbox();

    private static MinMaxYearCalendar? s_FrenchRevolutionary;
    /// <summary>
    /// Gets the French Revolutionary calendar with dates in the range from
    /// year I to year XIV.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar FrenchRevolutionary =>
        // Il s'agit d'une arithmétisation du calendrier révolutionnaire :
        // correspond à la réalité pour les années I à XIV, mais peut
        // diverger ensuite car on n'utilise pas la règle astronomique pour
        // déterminer le jour de l'an.
        //
        // Formellement, l'an XIV est incomplet, mais, pour simplifier, on
        // le fait aller jusqu'au bout, de même pour l'an I.
        // Entrée en vigueur le 15 vendémiaire de l'an II, c-à-d le 6
        // octobre 1793 (grégorien).
        // Abrogation le 10 nivôse de l'an XIV (10/4/XIV), c-à-d le 31
        // décembre 1805 (grégorien).
        s_FrenchRevolutionary ??=
            (from x in Coptic12Schema.GetInstance()
             select new MinMaxYearCalendar(
                "French Revolutionary",
                x,
                CalendarEpoch.FrenchRepublican,
                1,
                14)
             ).Unbox();
}

// Proleptic calendars of type Calendar or ZCalendar.
// - Tropicalia
// - LongGregorian
// - LongJulian
// Lazy initialization using Interlocked.CompareExchange(), but we must be
// careful when we create the calendar; see comments below.
// If I'm not mistaken, the calendars are singleton.
//
// We also declare the fields as volatile (read the latest value), but I'm not
// sure of its usefulness here, moreover I don't actually understand all the
// implications of it.
// https://stackoverflow.com/questions/72275/when-should-the-volatile-keyword-be-used-in-c
public partial class CalendarZoo
{
    /// <summary>
    /// Gets the proleptic Tropicália calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar Tropicalia => s_Tropicalia ?? InitTropicalia();

    private static volatile Calendar? s_Tropicalia;
    [Pure]
    private static Calendar InitTropicalia()
    {
        const string Key = "Tropicália";

        // The creation must be thread-safe, therefore we cannot use the simpler
        // CreateCalendar() which would throw if the key already exists.
        // NB: TryCreateCalendar() is thread-safe.
        var created = TropicaliaSchema.GetInstance().TryCreateCalendar(
            Key,
            DayZero.NewStyle,
            out var chr,
            proleptic: true);

        if (created == false)
        {
            // A calendar with the same key already exists, nevertheless
            // s_Tropicalia can still be null if the other process that reserved
            // the key in the catalog has not yet updated the field.
            return s_Tropicalia ?? CalendarCatalog.GetCalendar(Key);
        }

        Debug.Assert(chr != null);
        Interlocked.CompareExchange(ref s_Tropicalia, chr, null);
        // We return s_Tropicalia not "chr" to always obtain the correct
        // reference in case the process didn't initialize the field in the
        // previous line.
        return s_Tropicalia;
    }

    /// <summary>
    /// Gets the (long) proleptic Gregorian calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static ZCalendar LongGregorian => s_LongGregorian ?? InitLongGregorian();

    private static volatile ZCalendar? s_LongGregorian;
    [Pure]
    private static ZCalendar InitLongGregorian()
    {
        const string Key = "Long Gregorian";

        var sch = GregorianSchema.GetInstance().Unbox();
        var scope = MinMaxYearScope.WithMaximalRange(sch, DayZero.NewStyle);
        var chr = ZCatalog.GetOrAdd(Key, scope);

        Debug.Assert(chr != null);
        Interlocked.CompareExchange(ref s_LongGregorian, chr, null);
        return s_LongGregorian;
    }

    /// <summary>
    /// Gets the (long) proleptic Julian calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static ZCalendar LongJulian => s_LongJulian ?? InitLongJulian();

    private static volatile ZCalendar? s_LongJulian;
    [Pure]
    private static ZCalendar InitLongJulian()
    {
        const string Key = "Long Julian";

        var sch = JulianSchema.GetInstance().Unbox();
        var scope = MinMaxYearScope.WithMaximalRange(sch, DayZero.OldStyle);
        var chr = ZCatalog.GetOrAdd(Key, scope);

        Debug.Assert(chr != null);
        Interlocked.CompareExchange(ref s_LongJulian, chr, null);
        return s_LongJulian;
    }
}

// Retropolated calendars of type Calendar.
// - Egyptian
// - FrenchRepublican
// - InternationalFixed
// - Persian2820
// - Positivist
// - RevisedWorld
// - World
// Lazy initialization of a -group- of calendars.
public partial class CalendarZoo
{
    /// <summary>
    /// Gets the Egyptian calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar Egyptian => RetropolatedCalendars.Egyptian;

    /// <summary>
    /// Gets the French Republican calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar FrenchRepublican => RetropolatedCalendars.FrenchRepublican;

    /// <summary>
    /// Gets the International Fixed calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar InternationalFixed => RetropolatedCalendars.InternationalFixed;

    /// <summary>
    /// Gets the Persian calendar (proposed arithmetical form).
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar Persian2820 => RetropolatedCalendars.Persian2820;

    /// <summary>
    /// Gets the Positivist calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar Positivist => RetropolatedCalendars.Positivist;

    /// <summary>
    /// Gets the revised World calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar RevisedWorld => RetropolatedCalendars.RevisedWorld;

    /// <summary>
    /// Gets the World calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar World => RetropolatedCalendars.World;

    private class RetropolatedCalendars
    {
        // Static constructor to remove the "BeforeFieldInit" flag, but I'm not
        // sure that it's necessary here (only affects laziness).
        // With the flag (default), type initialization may be deferred or not.
        //   "Specifies that calling static methods of the type does not force the system to initialize the type."
        //   https://docs.microsoft.com/en-us/dotnet/api/system.reflection.typeattributes?view=net-6.0#system-reflection-typeattributes-beforefieldinit
        // Without the flag, the CLR is required to call the type initializer
        // before any member on the class is touched.
        static RetropolatedCalendars() { }

        internal static readonly Calendar Egyptian =
            Egyptian12Schema.GetInstance().CreateCalendar(
                "Egyptian",
                CalendarEpoch.Egyptian,
                proleptic: false);

        internal static Calendar FrenchRepublican =
            FrenchRepublican12Schema.GetInstance().CreateCalendar(
                "French Republican",
                CalendarEpoch.FrenchRepublican,
                proleptic: false);

        internal static Calendar InternationalFixed =
            // The International Fixed calendar re-uses the Gregorian epoch.
            InternationalFixedSchema.GetInstance().CreateCalendar(
                "International Fixed",
                DayZero.NewStyle,
                proleptic: false);

        internal static Calendar Persian2820 =
            Persian2820Schema.GetInstance().CreateCalendar(
                "Tabular Persian",
                CalendarEpoch.Persian,
                proleptic: false);

        internal static Calendar Positivist =
            PositivistSchema.GetInstance().CreateCalendar(
                "Positivist",
                CalendarEpoch.Positivist,
                proleptic: false);

        internal static Calendar RevisedWorld =
            // The Revised World calendar re-uses the Gregorian epoch.
            WorldSchema.GetInstance().CreateCalendar(
                "Revised World",
                DayZero.NewStyle,
                proleptic: false);

        internal static Calendar World =
            WorldSchema.GetInstance().CreateCalendar(
                "World",
                CalendarEpoch.SundayBeforeGregorian,
                proleptic: false);
    }
}

// Offseted calendars of type ZCalendar.
// - Holocene
// - Minguo
// - ThaiSolar
// Lazy initialization using Interlocked.CompareExchange().
public partial class CalendarZoo
{
    /// <summary>
    /// Gets the Holocene calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <remarks>
    /// The Holocene calendar differs from the Gregorian calendar only in the way years are numbered.
    /// </remarks>
    public static ZCalendar Holocene => s_Holocene ?? InitHolocene();

    private static volatile ZCalendar? s_Holocene;
    [Pure]
    private static ZCalendar InitHolocene()
    {
        const string Key = "Holocene";

        var sch = CreateOffsettedCivilSchema(10_000);
        var scope = MinMaxYearScope.WithMaximalRange(sch, DayZero.NewStyle, onOrAfterEpoch: true);
        var chr = ZCatalog.GetOrAdd(Key, scope);

        Debug.Assert(chr != null);
        Interlocked.CompareExchange(ref s_Holocene, chr, null);
        return s_Holocene;
    }

    /// <summary>
    /// Gets the Minguo calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <remarks>
    /// The Minguo calendar differs from the Gregorian calendar only in the way years are numbered.
    /// </remarks>
    public static ZCalendar Minguo => s_Minguo ?? InitMinguo();

    private static volatile ZCalendar? s_Minguo;
    [Pure]
    private static ZCalendar InitMinguo()
    {
        const string Key = "Minguo";

        var sch = CreateOffsettedCivilSchema(-1911);
        var scope = MinMaxYearScope.WithMaximalRange(sch, DayZero.NewStyle, onOrAfterEpoch: true);
        var chr = ZCatalog.GetOrAdd(Key, scope);

        Debug.Assert(chr != null);
        Interlocked.CompareExchange(ref s_Minguo, chr, null);
        return s_Minguo;
    }

    /// <summary>
    /// Gets the Thai solar calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <remarks>
    /// The Minguo calendar differs from the Gregorian calendar only in the way years are numbered.
    /// </remarks>
    public static ZCalendar ThaiSolar => s_ThaiSolar ?? InitThaiSolar();

    private static volatile ZCalendar? s_ThaiSolar;
    [Pure]
    private static ZCalendar InitThaiSolar()
    {
        const string Key = "Thai Solar";

        var sch = CreateOffsettedCivilSchema(543);
        var scope = MinMaxYearScope.WithMaximalRange(sch, DayZero.NewStyle, onOrAfterEpoch: true);
        var chr = ZCatalog.GetOrAdd(Key, scope);

        Debug.Assert(chr != null);
        Interlocked.CompareExchange(ref s_ThaiSolar, chr, null);
        return s_ThaiSolar;
    }

    private static CalendricalSchemaOffsetted CreateOffsettedCivilSchema(int offset)
    {
        var sch = CivilSchema.GetInstance().Unbox();
        return new CalendricalSchemaOffsetted(sch, offset);
    }
}

// Other calendars of type ZCalendar.
// - Pax (disabled)
public partial class CalendarZoo
{
#if false // TODO(code): unfinished Pax schema. When finished, make the code thread-safe...
    private static volatile ZCalendar? s_Pax;
    /// <summary>
    /// Gets the Pax calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static ZCalendar Pax =>
        s_Pax ??= ZCatalog.Add(
            "Pax",
            new PaxSchema(),
            CalendarEpoch.SundayBeforeGregorian,
            widest: false);
#endif
}
