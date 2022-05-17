// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System.Diagnostics.Contracts;
using System.Threading;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;

using static Zorglub.Time.Extensions.Unboxing;

// FIXME(code): volatile does not work here. Furthermore, I should verify that
// the other initializations are correct, they must happen only once (exception
// throw when the key is already in use). Maybe we should use TryCreateCalendar().
// https://stackoverflow.com/questions/72275/when-should-the-volatile-keyword-be-used-in-c

// On en profite pour explorer les différentes manières d'initialiser un
// champs statique en différé.
// - internal class (voir p.ex. WideCatalog)
//   This is the best possible choice. A bit verbose, but thread-safe and lazy.
// - Interlocked.CompareExchange()
//   Second best choice.
// - volatile field.
//   Less verbose but too magical and I'm not sure that I really understand
//   all the implications of it. Besides that, it does what it's supposed to.
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
public partial class CalendarZoo
{
    private static volatile BoundedBelowCalendar? s_GenuineGregorian;
    /// <summary>
    /// Gets the Gregorian calendar with dates on or after the 15th of
    /// October 1582 CE.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static BoundedBelowCalendar GenuineGregorian =>
        s_GenuineGregorian ??=
            (from x in GregorianSchema.GetInstance()
             select new BoundedBelowCalendar(
                 "Genuine Gregorian",
                 x,
                 DayZero.NewStyle,
                 1582, 10, 15)
             ).Unbox();

    private static volatile MinMaxYearCalendar? s_GenuineJulian;
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

    private static volatile MinMaxYearCalendar? s_FrenchRevolutionary;
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

// Proleptic calendars of type Calendar or WideCalendar.
// - Tropicalia
// - LongGregorian, see WideCalendar.Gregorian
// - LongJulian
public partial class CalendarZoo
{
    private static volatile Calendar? s_Tropicalia;
    /// <summary>
    /// Gets the proleptic Tropicália calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar Tropicalia =>
        s_Tropicalia ??= TropicaliaSchema.GetInstance().CreateCalendar(
            "Tropicália",
            DayZero.NewStyle,
            proleptic: true);

    private static volatile WideCalendar? s_LongJulian;
    /// <summary>
    /// Gets the (long) proleptic Julian calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static WideCalendar LongJulian =>
        s_LongJulian ??=
        (from x in JulianSchema.GetInstance()
         select WideCatalog.Add(
            "Long Julian",
            x,
            DayZero.OldStyle,
            widest: true)
         ).Unbox();
}

// Retropolated calendars of type Calendar.
// - Egyptian
// - FrenchRepublican
// - InternationalFixed
// - Persian2820
// - Positivist
// - RevisedWorld
// - World
public partial class CalendarZoo
{
    private static volatile Calendar? s_Egyptian;
    /// <summary>
    /// Gets the Egyptian calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar Egyptian =>
        s_Egyptian ??= Egyptian12Schema.GetInstance().CreateCalendar(
            "Egyptian",
            CalendarEpoch.Egyptian,
            proleptic: false);

    private static volatile Calendar? s_FrenchRepublican;
    /// <summary>
    /// Gets the French Republican calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar FrenchRepublican =>
        s_FrenchRepublican ??= FrenchRepublican12Schema.GetInstance().CreateCalendar(
            "French Republican",
            CalendarEpoch.FrenchRepublican,
            proleptic: false);

    private static volatile Calendar? s_InternationalFixed;
    /// <summary>
    /// Gets the International Fixed calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar InternationalFixed =>
        // The International Fixed calendar re-uses the Gregorian epoch.
        s_InternationalFixed ??= InternationalFixedSchema.GetInstance().CreateCalendar(
            "International Fixed",
            DayZero.NewStyle,
            proleptic: false);

    private static volatile Calendar? s_Persian2820;
    /// <summary>
    /// Gets the Persian calendar (proposed arithmetical form).
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar Persian2820 =>
        s_Persian2820 ??= Persian2820Schema.GetInstance().CreateCalendar(
            "Tabular Persian",
            CalendarEpoch.Persian,
            proleptic: false);

    private static volatile Calendar? s_Positivist;
    /// <summary>
    /// Gets the Positivist calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar Positivist =>
        s_Positivist ??= PositivistSchema.GetInstance().CreateCalendar(
            "Positivist",
            CalendarEpoch.Positivist,
            proleptic: false);

    private static volatile Calendar? s_RevisedWorld;
    /// <summary>
    /// Gets the revised World calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar RevisedWorld =>
        // The Revised World calendar re-uses the Gregorian epoch.
        s_RevisedWorld ??= WorldSchema.GetInstance().CreateCalendar(
            "Revised World",
            DayZero.NewStyle,
            proleptic: false);

    private static volatile Calendar? s_World;
    /// <summary>
    /// Gets the World calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar World =>
        s_World ??= WorldSchema.GetInstance().CreateCalendar(
            "World",
            CalendarEpoch.SundayBeforeGregorian,
            proleptic: false);
}

// Offset calendars of type WideCalendar.
// - Holocene
// - Minguo
// - ThaiSolar
public partial class CalendarZoo
{
    /// <summary>
    /// Gets the Holocene calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <remarks>
    /// The Holocene calendar differs from the Gregorian calendar only in
    /// the way years are numbered.
    /// </remarks>
    public static WideCalendar Holocene => s_Holocene ??= InitHolocene();

    private static WideCalendar? s_Holocene;
    [Pure]
    private static WideCalendar InitHolocene()
    {
        var chr =
            (from x in GregorianSchema.GetInstance()
             select WideCatalog.Add(
                 "Holocene",
                 new CalendricalSchemaOffsetted(x, 10_000),
                 DayZero.NewStyle,
                 widest: false)
             ).Unbox();
        Interlocked.CompareExchange(ref s_Holocene, chr, null);
        return s_Holocene;
    }

    /// <summary>
    /// Gets the Minguo calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <remarks>
    /// The Minguo calendar differs from the Gregorian calendar only in
    /// the way years are numbered.
    /// </remarks>
    public static WideCalendar Minguo => s_Minguo ??= InitMinguo();

    private static WideCalendar? s_Minguo;
    [Pure]
    private static WideCalendar InitMinguo()
    {
        var chr =
            (from x in GregorianSchema.GetInstance()
             select WideCatalog.Add(
                "Minguo",
                new CalendricalSchemaOffsetted(x, -1911),
                DayZero.NewStyle,
                widest: false)
             ).Unbox();
        Interlocked.CompareExchange(ref s_Minguo, chr, null);
        return s_Minguo;
    }

    /// <summary>
    /// Gets the Thai solar calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <remarks>
    /// The Minguo calendar differs from the Gregorian calendar only in
    /// the way years are numbered.
    /// </remarks>
    public static WideCalendar ThaiSolar => s_ThaiSolar ??= InitThaiSolar();

    private static WideCalendar? s_ThaiSolar;
    [Pure]
    private static WideCalendar InitThaiSolar()
    {
        var chr =
            (from x in GregorianSchema.GetInstance()
             select WideCatalog.Add(
                "Thai Solar",
                new CalendricalSchemaOffsetted(x, 543),
                DayZero.NewStyle,
                widest: false)
             ).Unbox();
        Interlocked.CompareExchange(ref s_ThaiSolar, chr, null);
        return s_ThaiSolar;
    }
}

// Other calendars of type WideCalendar.
// - Pax
public partial class CalendarZoo
{
#if false // TODO(code): unfinished Pax schema.
    private static volatile WideCalendar? s_Pax;
    /// <summary>
    /// Gets the Pax calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static WideCalendar Pax =>
        s_Pax ??= WideCatalog.Add(
            "Pax",
            new PaxSchema(),
            CalendarEpoch.SundayBeforeGregorian,
            widest: false);
#endif
}
