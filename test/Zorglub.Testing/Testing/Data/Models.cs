// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

#region Developer Notes

// We rely on Yemoda/Yedoy/Yemo to define our models, which means that we
// cannot use (and test) parts (year, month, day, dayOfYear) outside the ranges
// supported by these types.
//
// We assume that the data is valid, e.g. MonthInfo.DaysInMonth must be > 0.
//
// The use of unsigned shorts and bytes is not necessary, but it helps to reduce
// the size of the structs. Instead, we could have use shorts everywhere but it
// doesn't change the final sizes and more specialized integral types help to
// catch obviously wrong data. For instance, unsigned shorts and bytes are >= 0
// which is a characteristic of all the properties for which we use them.
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

public readonly record struct YearDaysSinceEpoch(int Year, int DaysSinceEpoch);

public readonly record struct YearDayNumber(int Year, DayNumber DayNumber);

public readonly record struct DaysSinceEpochInfo(int DaysSinceEpoch, Yemoda Yemoda)
{
    public DaysSinceEpochInfo(int daysSinceEpoch, int y, int m, int d)
        : this(daysSinceEpoch, new Yemoda(y, m, d)) { }

    public void Deconstruct(out int daysSinceEpoch, out int y, out int m, out int d)
    {
        daysSinceEpoch = DaysSinceEpoch;
        (y, m, d) = Yemoda;
    }

    [Pure]
    public DayNumberInfo ToDayNumberInfo(DayNumber epoch) => new(epoch + DaysSinceEpoch, Yemoda);
}

public readonly record struct DaysSinceZeroInfo(int DaysSinceZero, Yemoda Yemoda) :
    IConvertibleToDayNumberInfo
{
    public DaysSinceZeroInfo(int daysSinceZero, int y, int m, int d)
        : this(daysSinceZero, new Yemoda(y, m, d)) { }

    public void Deconstruct(out int daysSinceZero, out int y, out int m, out int d)
    {
        daysSinceZero = DaysSinceZero;
        (y, m, d) = Yemoda;
    }

    [Pure]
    public DayNumberInfo ToDayNumberInfo() => new(DayNumber.Zero + DaysSinceZero, Yemoda);
}

public readonly record struct DaysSinceRataDieInfo(int DaysSinceRataDie, Yemoda Yemoda) :
    IConvertibleToDayNumberInfo
{
    public DaysSinceRataDieInfo(int daysSinceRataDie, int y, int m, int d)
        : this(daysSinceRataDie, new Yemoda(y, m, d)) { }

    public void Deconstruct(out int daysSinceRataDie, out int y, out int m, out int d)
    {
        daysSinceRataDie = DaysSinceRataDie;
        (y, m, d) = Yemoda;
    }

    [Pure]
    public DayNumberInfo ToDayNumberInfo() => new(DayZero.RataDie + DaysSinceRataDie, Yemoda);
}

public readonly record struct DayNumberInfo(DayNumber DayNumber, Yemoda Yemoda)
{
    public DayNumberInfo(DayNumber dayNumber, int y, int m, int d)
        : this(dayNumber, new Yemoda(y, m, d)) { }

    public void Deconstruct(out DayNumber dayNumber, out int y, out int m, out int d)
    {
        dayNumber = DayNumber;
        (y, m, d) = Yemoda;
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "<Pending>")]
    public static DayNumberInfo operator +(DayNumberInfo x, int value) =>
        new(x.DayNumber + value, x.Yemoda);

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "<Pending>")]
    public static DaysSinceEpochInfo operator -(DayNumberInfo x, DayNumber epoch) =>
        new(x.DayNumber - epoch, x.Yemoda);
}

// We use Yemoda, otherwise the struct is too big (24 bytes).
public readonly record struct DateInfo
{
    public DateInfo(int y, int m, int d, int doy, bool isIntercalary, bool isSupplementary)
        : this(new Yemoda(y, m, d), doy, isIntercalary, isSupplementary) { }

    // Do NOT make it a primary constructor, otherwise it will automatically create a deconstructor
    // which the same number of parameters as the one defined below, which can be problematic with F#.
    public DateInfo(Yemoda ymd, int doy, bool isIntercalary, bool isSupplementary)
    {
        Yemoda = ymd;
        DayOfYear = doy;
        IsIntercalary = isIntercalary;
        IsSupplementary = isSupplementary;
    }

    public Yemoda Yemoda { get; }
    public Yedoy Yedoy => new(Yemoda.Year, DayOfYear);

    public int DayOfYear { get; }

    public bool IsIntercalary { get; }
    public bool IsSupplementary { get; }

    public void Deconstruct(out int y, out int m, out int d, out int doy)
    {
        (y, m, d) = Yemoda;
        doy = DayOfYear;
    }
}

// We use Yemo, otherwise the struct is too big (20 bytes).
public readonly record struct MonthInfo(Yemo Yemo, byte DaysInMonth, ushort DaysInYearBeforeMonth, bool IsIntercalary)
{
    public MonthInfo(int y, int m, byte daysInMonth, ushort daysInYearBeforeMonth, bool isIntercalary)
        : this(new Yemo(y, m), daysInMonth, daysInYearBeforeMonth, isIntercalary) { }
}

public readonly record struct YearInfo(int Year, byte MonthsInYear, ushort DaysInYear, bool IsLeap);

public readonly record struct DecadeOfCenturyInfo(int Year, int Century, byte DecadeOfCentury, byte YearOfDecade);

public readonly record struct DecadeInfo(int Year, int Decade, byte YearOfDecade);

public readonly record struct CenturyInfo(int Year, int Century, byte YearOfCentury);

public readonly record struct MillenniumInfo(int Year, int Millennium, short YearOfMillennium);

public readonly record struct YemodaAnd<T>(Yemoda Yemoda, T Value) where T : struct
{
    public YemodaAnd(int y, int m, int d, T value)
        : this(new Yemoda(y, m, d), value) { }

    public void Deconstruct(out int y, out int m, out int d, out T value)
    {
        (y, m, d) = Yemoda;
        value = Value;
    }
}

public readonly record struct YemoAnd<T>(Yemo Yemo, T Value) where T : struct
{
    public YemoAnd(int y, int m, T value)
        : this(new Yemo(y, m), value) { }

    public void Deconstruct(out int y, out int m, out T value)
    {
        (y, m) = Yemo;
        value = Value;
    }
}

public readonly record struct YemodaPair(Yemoda First, Yemoda Second);

public readonly record struct YemodaPairAnd<T>(Yemoda First, Yemoda Second, T Value) where T : struct;
