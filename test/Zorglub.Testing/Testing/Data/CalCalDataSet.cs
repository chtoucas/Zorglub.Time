// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using System.Linq;

using Zorglub.Testing.Data.Schemas;

// TODO(data): filter CalCalDataSet, IDateFacts, CalendarFacts, etc.

public static partial class CalCalDataSet { }

public partial class CalCalDataSet // Interconversion
{
    // Lazy initialized mainly to be sure that there isn't a circular
    // dependency problem (CalCalDataSet and Gregorian(Julian)Data)
    // during the initialization of static props.
    private static volatile TheoryData<Yemoda, Yemoda>? s_GregorianJulianData;
    /// <summary>Gregorian date, Julian date.</summary>
    public static TheoryData<Yemoda, Yemoda> GregorianJulianData =>
        s_GregorianJulianData ??= InitGregorianJulianData();

    [Pure]
    private static TheoryData<Yemoda, Yemoda> InitGregorianJulianData()
    {
        var lookup = GregorianDataSet.RataDieInfos.ToLookup(x => x.RataDie);

        var data = new TheoryData<Yemoda, Yemoda>();
        foreach (var (jrd, jy, jm, jd) in JulianDataSet.RataDieInfos)
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

// To be moved to CalendarDataSet.
public partial class CalCalDataSet // Day of the week
{
    private static volatile TheoryData<DayNumber, DayOfWeek>? s_DayNumberToDayOfWeekData;
    /// <summary>Day number, day of the week.</summary>
    public static TheoryData<DayNumber, DayOfWeek> DayNumberToDayOfWeekData =>
        s_DayNumberToDayOfWeekData ??= MapToDayNumberToDayOfWeekData(s_DayNumberToDayOfWeek);

    private static volatile TheoryData<DayNumber64, DayOfWeek>? s_DayNumber64ToDayOfWeekData;
    /// <summary>Day number 64-bit, day of the week.</summary>
    public static TheoryData<DayNumber64, DayOfWeek> DayNumber64ToDayOfWeekData =>
        s_DayNumber64ToDayOfWeekData ??= MapToDayNumber64ToDayOfWeekData(s_DayNumberToDayOfWeek);

    [Pure]
    private static TheoryData<DayNumber, DayOfWeek> MapToDayNumberToDayOfWeekData(
        IEnumerable<(int RataDie, DayOfWeek DayOfWeek)> source)
    {
        var data = new TheoryData<DayNumber, DayOfWeek>();
        foreach (var (rd, dayOfWeek) in source)
        {
            data.Add(DayZero.RataDie + rd, dayOfWeek);
        }
        return data;
    }

    [Pure]
    private static TheoryData<DayNumber64, DayOfWeek> MapToDayNumber64ToDayOfWeekData(
        IEnumerable<(int RataDie, DayOfWeek DayOfWeek)> source)
    {
        var data = new TheoryData<DayNumber64, DayOfWeek>();
        foreach (var (rd, dayOfWeek) in source)
        {
            data.Add(DayZero64.RataDie + rd, dayOfWeek);
        }
        return data;
    }

    private static readonly List<(int RataDie, DayOfWeek)> s_DayNumberToDayOfWeek = new()
    {
        (1, DayOfWeek.Monday),

        // D.&R. Annexe C.
        (-214_193, DayOfWeek.Sunday),
        (-61_387, DayOfWeek.Wednesday),
        (25_469, DayOfWeek.Wednesday),
        (49_217, DayOfWeek.Sunday),
        (171_307, DayOfWeek.Wednesday),
        (210_155, DayOfWeek.Monday),
        (253_427, DayOfWeek.Saturday),
        (369_740, DayOfWeek.Sunday),
        (400_085, DayOfWeek.Sunday),
        (434_355, DayOfWeek.Friday),
        (452_605, DayOfWeek.Saturday),
        (470_160, DayOfWeek.Friday),
        (473_837, DayOfWeek.Sunday),
        (507_850, DayOfWeek.Sunday),
        (524_156, DayOfWeek.Wednesday),
        (544_676, DayOfWeek.Saturday),
        (567_118, DayOfWeek.Saturday),
        (569_477, DayOfWeek.Saturday),
        (601_716, DayOfWeek.Wednesday),
        (613_424, DayOfWeek.Sunday),
        (626_596, DayOfWeek.Friday),
        (645_554, DayOfWeek.Sunday),
        (664_224, DayOfWeek.Monday),
        (671_401, DayOfWeek.Wednesday),
        (694_799, DayOfWeek.Sunday),
        (704_424, DayOfWeek.Sunday),
        (708_842, DayOfWeek.Monday),
        (709_409, DayOfWeek.Monday),
        (709_580, DayOfWeek.Thursday),
        (727_274, DayOfWeek.Tuesday),
        (728_714, DayOfWeek.Sunday),
        (744_313, DayOfWeek.Wednesday),
        (764_652, DayOfWeek.Sunday),
    };
}
