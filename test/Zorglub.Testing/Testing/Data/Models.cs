// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

#region Developer Notes

// We rely on Yemoda/Yedoy/Yemo to define our models, which means that we
// cannot use (and test) parts (year, month, day, dayOfYear) outside the ranges
// supported by these types.
//
// Structs in TheoryData are boxed. Use reference types rather than value types
// to model data? Why bother, it's already the case with all primitive types.
// It might even be beneficial, we only box one single value instead of all of
// its components. It also simplifies a bunch of things (see e.g.
// BoundedCalendarDataSet).
// https://github.com/xunit/xunit/blob/v2/src/xunit.core/TheoryData.cs
//
// Implement IXunitSerializable to obtain a more accurate number of tests?
// Since we ignore some tests dynamically, the count will stay fuzzy even if we
// do that. Furthermore, FsCheck also reports only one test whatever the number
// of tries.
// https://github.com/xunit/xunit/issues/429

#endregion

public readonly record struct YearDaysSinceEpoch(int Year, int DaysSinceEpoch)
{
    [Pure]
    public YearDayNumber ToYearDayNumber(DayNumber epoch) =>
        new(Year, epoch + DaysSinceEpoch);
}

public readonly record struct YearDayNumber(int Year, DayNumber DayNumber);

public readonly record struct DaysSinceEpochInfo(int DaysSinceEpoch, int Year, int Month, int Day)
{
    public Yemoda Yemoda => new(Year, Month, Day);

    public void Deconstruct(out int daysSinceEpoch, out Yemoda ymd)
    {
        daysSinceEpoch = DaysSinceEpoch;
        ymd = Yemoda;
    }

    [Pure]
    public DayNumberInfo ToDayNumberInfo(DayNumber epoch) =>
        new(epoch + DaysSinceEpoch, Year, Month, Day);
}

public readonly record struct DaysSinceOriginInfo(int DaysSinceOrigin, int Year, int Month, int Day)
{
    [Pure]
    public DayNumberInfo ToDayNumberInfo(DayNumber origin) =>
        new(origin + DaysSinceOrigin, Year, Month, Day);
}

public readonly record struct DaysSinceRataDieInfo(int DaysSinceRataDie, int Year, int Month, int Day)
{
    [Pure]
    public DayNumberInfo ToDayNumberInfo() =>
        new(DayZero.RataDie + DaysSinceRataDie, Year, Month, Day);
}

public readonly record struct DayNumberInfo(DayNumber DayNumber, int Year, int Month, int Day)
{
    public Yemoda Yemoda => new(Year, Month, Day);

    public void Deconstruct(out DayNumber dayNumber, out Yemoda ymd)
    {
        dayNumber = DayNumber;
        ymd = Yemoda;
    }

    [Pure]
    public static DayNumberInfo FromDaysSinceRataDieInfo(DaysSinceRataDieInfo x) => x.ToDayNumberInfo();
}

// We use Yemoda, otherwise the struct is too big (24 bytes).
public readonly record struct DateInfo
{
    public DateInfo(int y, int m, int d, int doy, bool isIntercalary, bool isSupplementary)
    {
        Yemoda = new Yemoda(y, m, d);
        DayOfYear = doy;
        IsIntercalary = isIntercalary;
        IsSupplementary = isSupplementary;
    }

    public Yemoda Yemoda { get; }
    public Yedoy Yedoy => new(Yemoda.Year, DayOfYear);

    public int DayOfYear { get; }

    public bool IsIntercalary { get; }
    public bool IsSupplementary { get; }

    public void Deconstruct(out int year, out int month, out int day, out int dayOfYear)
    {
        (year, month, day) = Yemoda;
        dayOfYear = DayOfYear;
    }
}

// We use Yemo, otherwise the struct is too big (20 bytes).
public readonly record struct MonthInfo
{
    public MonthInfo(int y, int m, int daysInMonth, int daysInYearBeforeMonth, bool isIntercalary)
    {
        Yemo = new Yemo(y, m);
        DaysInMonth = daysInMonth;
        DaysInYearBeforeMonth = daysInYearBeforeMonth;
        IsIntercalary = isIntercalary;
    }

    public Yemo Yemo { get; }

    public int DaysInMonth { get; }
    public int DaysInYearBeforeMonth { get; }

    public bool IsIntercalary { get; }
}

public readonly record struct YearInfo(int Year, int MonthsInYear, int DaysInYear, bool IsLeap);

public readonly record struct DecadeOfCenturyInfo(int Year, int Century, int DecadeOfCentury, int YearOfDecade);

public readonly record struct DecadeInfo(int Year, int Decade, int YearOfDecade);

public readonly record struct CenturyInfo(int Year, int Century, int YearOfCentury);

public readonly record struct MillenniumInfo(int Year, int Millennium, int YearOfMillennium);

public readonly record struct EpagomenalDayInfo(int Year, int Month, int Day, int EpagomenalNumber);

public readonly record struct YemodaAnd<T> where T : struct
{
    public YemodaAnd(int y, int m, int d, T extra)
    {
        Yemoda = new Yemoda(y, m, d);
        Extra = extra;
    }

    public Yemoda Yemoda { get; }
    public T Extra { get; }

    public void Deconstruct(out int year, out int month, out int day, out T extra)
    {
        (year, month, day) = Yemoda;
        extra = Extra;
    }
}

public readonly record struct YemoAnd<T> where T : struct
{
    public YemoAnd(int y, int m, T extra)
    {
        Yemo = new Yemo(y, m);
        Extra = extra;
    }

    public Yemo Yemo { get; }
    public T Extra { get; }

    public void Deconstruct(out int year, out int month, out T extra)
    {
        (year, month) = Yemo;
        extra = Extra;
    }
}
