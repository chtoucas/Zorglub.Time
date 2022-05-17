// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using System.Linq;
using System.Threading;

using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Core.Intervals;

public static partial class CalCalDataSet { }

public partial class CalCalDataSet // Interconversion
{
    // Lazy initialized mainly to ensure that there isn't a circular dependency
    // problem (CalCalDataSet and Gregorian(Julian)Data) during the init of
    // static props. I guess that this is is no longer necessary.
    private static TheoryData<Yemoda, Yemoda>? s_GregorianToJulianData;
    public static TheoryData<Yemoda, Yemoda> GregorianToJulianData =>
        s_GregorianToJulianData ??= InitGregorianToJulianData();

    [Pure]
    private static TheoryData<Yemoda, Yemoda> InitGregorianToJulianData()
    {
        var data = GetGregorianToJulianData();
        Interlocked.CompareExchange(ref s_GregorianToJulianData, data, null);
        return s_GregorianToJulianData;
    }

    [Pure]
    private static TheoryData<Yemoda, Yemoda> GetGregorianToJulianData()
    {
        var lookup = GregorianDataSet.DaysSinceRataDieInfos.ToLookup(x => x.DaysSinceRataDie);

        var data = new TheoryData<Yemoda, Yemoda>();
        foreach (var (jrd, jy, jm, jd) in JulianDataSet.DaysSinceRataDieInfos)
        {
            var gs = lookup[jrd].ToList();
            if (gs.Count == 1)
            {
                var (_, gy, gm, gd) = gs.Single();
                data.Add(new Yemoda(gy, gm, gd), new Yemoda(jy, jm, jd));
            }
        }

        data.Add(new Yemoda(-9998, 1, 1), new Yemoda(-9998, 3, 19));
        data.Add(new Yemoda(1970, 1, 1), new Yemoda(1969, 12, 19));

        return data;
    }
}

public partial class CalCalDataSet // Day of the week
{
    private static TheoryData<DayNumber, DayOfWeek>? s_DayNumberToDayOfWeekData;
    public static TheoryData<DayNumber, DayOfWeek> DayNumberToDayOfWeekData =>
        s_DayNumberToDayOfWeekData ??= InitDayNumberToDayOfWeekData();

    private static TheoryData<DayNumber64, DayOfWeek>? s_DayNumber64ToDayOfWeekData;
    public static TheoryData<DayNumber64, DayOfWeek> DayNumber64ToDayOfWeekData =>
        s_DayNumber64ToDayOfWeekData ??= InitDayNumber64ToDayOfWeekData();

    [Pure]
    public static TheoryData<DayNumber, DayOfWeek> GetDayNumberToDayOfWeekData(Range<DayNumber> domain)
    {
        var data = new TheoryData<DayNumber, DayOfWeek>();
        foreach (var (daysSinceRataDie, dayOfWeek) in DaysSinceRataDieToDayOfWeek)
        {
            var dayNumber = DayZero.RataDie + daysSinceRataDie;
            if (domain.Contains(dayNumber) == false) { continue; }
            data.Add(dayNumber, dayOfWeek);
        }
        return data;
    }

    [Pure]
    private static TheoryData<DayNumber, DayOfWeek> InitDayNumberToDayOfWeekData()
    {
        var data = MapToDayNumberToDayOfWeekData(DaysSinceRataDieToDayOfWeek);
        Interlocked.CompareExchange(ref s_DayNumberToDayOfWeekData, data, null);
        return s_DayNumberToDayOfWeekData;
    }

    [Pure]
    private static TheoryData<DayNumber64, DayOfWeek> InitDayNumber64ToDayOfWeekData()
    {
        var data = MapToDayNumber64ToDayOfWeekData(DaysSinceRataDieToDayOfWeek);
        Interlocked.CompareExchange(ref s_DayNumber64ToDayOfWeekData, data, null);
        return s_DayNumber64ToDayOfWeekData;
    }

    [Pure]
    private static TheoryData<DayNumber, DayOfWeek> MapToDayNumberToDayOfWeekData(
        IEnumerable<(int DaysSinceRataDie, DayOfWeek DayOfWeek)> source)
    {
        var data = new TheoryData<DayNumber, DayOfWeek>();
        foreach (var (daysSinceRataDie, dayOfWeek) in source)
        {
            data.Add(DayZero.RataDie + daysSinceRataDie, dayOfWeek);
        }
        return data;
    }

    [Pure]
    private static TheoryData<DayNumber64, DayOfWeek> MapToDayNumber64ToDayOfWeekData(
        IEnumerable<(int DaysSinceRataDie, DayOfWeek DayOfWeek)> source)
    {
        var data = new TheoryData<DayNumber64, DayOfWeek>();
        foreach (var (daysSinceRataDie, dayOfWeek) in source)
        {
            data.Add(DayZero64.RataDie + daysSinceRataDie, dayOfWeek);
        }
        return data;
    }

    private static IEnumerable<(int DaysSinceRataDie, DayOfWeek DayOfWeek)> DaysSinceRataDieToDayOfWeek
    {
        get
        {
            yield return (1, DayOfWeek.Monday);

            // D.&R. Annexe C.
            yield return (-214_193, DayOfWeek.Sunday);
            yield return (-61_387, DayOfWeek.Wednesday);
            yield return (25_469, DayOfWeek.Wednesday);
            yield return (49_217, DayOfWeek.Sunday);
            yield return (171_307, DayOfWeek.Wednesday);
            yield return (210_155, DayOfWeek.Monday);
            yield return (253_427, DayOfWeek.Saturday);
            yield return (369_740, DayOfWeek.Sunday);
            yield return (400_085, DayOfWeek.Sunday);
            yield return (434_355, DayOfWeek.Friday);
            yield return (452_605, DayOfWeek.Saturday);
            yield return (470_160, DayOfWeek.Friday);
            yield return (473_837, DayOfWeek.Sunday);
            yield return (507_850, DayOfWeek.Sunday);
            yield return (524_156, DayOfWeek.Wednesday);
            yield return (544_676, DayOfWeek.Saturday);
            yield return (567_118, DayOfWeek.Saturday);
            yield return (569_477, DayOfWeek.Saturday);
            yield return (601_716, DayOfWeek.Wednesday);
            yield return (613_424, DayOfWeek.Sunday);
            yield return (626_596, DayOfWeek.Friday);
            yield return (645_554, DayOfWeek.Sunday);
            yield return (664_224, DayOfWeek.Monday);
            yield return (671_401, DayOfWeek.Wednesday);
            yield return (694_799, DayOfWeek.Sunday);
            yield return (704_424, DayOfWeek.Sunday);
            yield return (708_842, DayOfWeek.Monday);
            yield return (709_409, DayOfWeek.Monday);
            yield return (709_580, DayOfWeek.Thursday);
            yield return (727_274, DayOfWeek.Tuesday);
            yield return (728_714, DayOfWeek.Sunday);
            yield return (744_313, DayOfWeek.Wednesday);
            yield return (764_652, DayOfWeek.Sunday);
        }
    }
}
