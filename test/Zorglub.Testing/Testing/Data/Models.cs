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
// Value type or reference type? We use a simple rule here: use a value type if
// it has a size <= 16 bytes, otherwise use a reference type.
//
// The use of unsigned shorts and bytes is not necessary, but it helps to reduce
// the size of the structs. Instead, we could have used shorts everywhere but it
// doesn't change the final sizes and more specialized integral types help to
// catch obviously wrong data. For instance, unsigned shorts and bytes are >= 0
// which is a characteristic of all the properties for which we use them.
// On the downside, sometimes we have to cast the property to int.
// To further reduce the size of structs, we could use explicit layouts but it
// doesn't seem to be worth it.
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

#region YearDayXXX

public readonly record struct YearMonthsSinceEpoch(int Year, int MonthsSinceEpoch);

public readonly record struct YearDaysSinceEpoch(int Year, int DaysSinceEpoch)
{
    [Pure]
    public YearDayNumber ToYearDayNumber(DayNumber epoch) => new(Year, epoch + DaysSinceEpoch);
}

public readonly record struct YearDayNumber(int Year, DayNumber DayNumber);

#endregion
#region MonthsSinceEpochInfo, DaySinceXXXInfo and DayNumberInfo

public readonly record struct MonthsSinceEpochInfo(int MonthsSinceEpoch, Yemo Yemo)
{
    public MonthsSinceEpochInfo(int monthsSinceEpoch, int y, int m)
        : this(monthsSinceEpoch, new Yemo(y, m)) { }

    public void Deconstruct(out int monthsSinceEpoch, out int y, out int m)
    {
        monthsSinceEpoch = MonthsSinceEpoch;
        (y, m) = Yemo;
    }
}

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

    public static DayNumberInfo operator +(DayNumberInfo x, int value) => new(x.DayNumber + value, x.Yemoda);

    public static DaysSinceEpochInfo operator -(DayNumberInfo x, DayNumber epoch) => new(x.DayNumber - epoch, x.Yemoda);
}

// REVIEW(data): use Yeweda, does not exist yet... idem with DayNumberYewedaInfo
public readonly record struct DaysSinceEpochYewedaInfo(int DaysSinceEpoch, Yewe Yewe, DayOfWeek DayOfWeek)
{
    public DaysSinceEpochYewedaInfo(int daysSinceEpoch, int y, int woy, DayOfWeek dow)
        : this(daysSinceEpoch, new Yewe(y, woy), dow) { }

    public void Deconstruct(out int daysSinceEpoch, out int y, out int woy, out DayOfWeek dow)
    {
        daysSinceEpoch = DaysSinceEpoch;
        (y, woy) = Yewe;
        dow = DayOfWeek;
    }

    [Pure]
    public DayNumberYewedaInfo ToDayNumberYewedaInfo(DayNumber epoch) =>
        new(epoch + DaysSinceEpoch, Yewe, DayOfWeek);
}

public readonly record struct DayNumberYewedaInfo(DayNumber DayNumber, Yewe Yewe, DayOfWeek DayOfWeek)
{
    public DayNumberYewedaInfo(DayNumber dayNumber, int y, int woy, DayOfWeek dow)
        : this(dayNumber, new Yewe(y, woy), dow) { }

    public void Deconstruct(out DayNumber dayNumber, out int y, out int woy, out DayOfWeek dow)
    {
        dayNumber = DayNumber;
        (y, woy) = Yewe;
        dow = DayOfWeek;
    }
}

#endregion
#region DateInfo, MonthInfo, etc

// It would have been nice to include DaysInYearAfterDate and DaysInMonthAfterDate
// but then the type would have been too big to be a struct.
public readonly record struct DateInfo
{
    public DateInfo(int y, int m, int d, ushort doy, bool isIntercalary, bool isSupplementary)
        : this(new Yemoda(y, m, d), doy, isIntercalary, isSupplementary) { }

    // Do NOT make it a primary constructor, otherwise it will automatically
    // create a deconstructor with the same number of parameters as the one
    // defined below, which can be problematic from F#.
    public DateInfo(Yemoda ymd, ushort doy, bool isIntercalary, bool isSupplementary)
    {
        Yemoda = ymd;
        DayOfYear = doy;
        IsIntercalary = isIntercalary;
        IsSupplementary = isSupplementary;
    }

    public Yemoda Yemoda { get; }
    public Yedoy Yedoy => new(Yemoda.Year, DayOfYear);

    public ushort DayOfYear { get; }

    public bool IsIntercalary { get; }
    public bool IsSupplementary { get; }

    public void Deconstruct(out int y, out int m, out int d, out int doy)
    {
        (y, m, d) = Yemoda;
        doy = DayOfYear;
    }
}

public readonly record struct WeekInfo(Yewe Yewe, bool IsIntercalary)
{
    public WeekInfo(int y, int woy, bool isIntercalary)
        : this(new Yewe(y, woy), isIntercalary) { }
}

public readonly record struct MonthInfo(
    Yemo Yemo, byte DaysInMonth, ushort DaysInYearBeforeMonth, ushort DaysInYearAfterMonth, bool IsIntercalary)
{
    public MonthInfo(
        int y, int m,
        byte daysInMonth, ushort daysInYearBeforeMonth, ushort daysInYearAfterMonth,
        bool isIntercalary)
        : this(new Yemo(y, m), daysInMonth, daysInYearBeforeMonth, daysInYearAfterMonth, isIntercalary) { }
}

public readonly record struct YearInfo(int Year, byte MonthsInYear, ushort DaysInYear, bool IsLeap);

public readonly record struct DecadeInfo(int Year, int Decade, byte YearOfDecade);

public readonly record struct CenturyInfo(int Year, int Century, byte YearOfCentury);

public readonly record struct MillenniumInfo(int Year, int Millennium, ushort YearOfMillennium);

public readonly record struct DecadeOfCenturyInfo(int Year, int Century, byte DecadeOfCentury, byte YearOfDecade);

#endregion
#region YemodaAnd<T>, YemoAnd<T>, etc

public readonly record struct YemodaAnd<T>(Yemoda Yemoda, T Value) where T : struct
{
    public YemodaAnd(int y, int m, int d, T value) : this(new Yemoda(y, m, d), value) { }

    public void Deconstruct(out int y, out int m, out int d, out T value)
    {
        (y, m, d) = Yemoda;
        value = Value;
    }
}

public readonly record struct YemoAnd<T>(Yemo Yemo, T Value) where T : struct
{
    public YemoAnd(int y, int m, T value) : this(new Yemo(y, m), value) { }

    public void Deconstruct(out int y, out int m, out T value)
    {
        (y, m) = Yemo;
        value = Value;
    }
}

public readonly record struct YemoAnd<T1, T2>(Yemo Yemo, T1 Value1, T2 Value2)
    where T1 : struct
    where T2 : struct
{
    public YemoAnd(int y, int m, T1 value1, T2 value2) : this(new Yemo(y, m), value1, value2) { }

    public void Deconstruct(out int y, out int m, out T1 value1, out T2 value2)
    {
        (y, m) = Yemo;
        value1 = Value1;
        value2 = Value2;
    }
}

public readonly record struct YearAnd<T>(int Year, T Value) where T : struct
{
    public void Deconstruct(out int y, out T value)
    {
        y = Year;
        value = Value;
    }
}

public readonly record struct YemodaPair(Yemoda First, Yemoda Second);
public readonly record struct YemodaPairAnd<T>(Yemoda First, Yemoda Second, T Value) where T : struct;

public readonly record struct YedoyPair(Yedoy First, Yedoy Second);
public readonly record struct YedoyPairAnd<T>(Yedoy First, Yedoy Second, T Value) where T : struct;

public readonly record struct YemoPair(Yemo First, Yemo Second);
public readonly record struct YemoPairAnd<T>(Yemo First, Yemo Second, T Value) where T : struct;

#endregion
#region Parts

// Too big to be a struct (24 bytes).
public sealed record DatePartsPair(DateParts First, DateParts Second)
{
    public static explicit operator DatePartsPair(YemodaPair pair)
    {
        var (y, m, d) = pair.First;
        var first = new DateParts(y, m, d);
        (y, m, d) = pair.Second;
        var second = new DateParts(y, m, d);
        return new DatePartsPair(first, second);
    }
}
// Too big to be a struct (28 bytes).
public sealed record DatePartsPairAnd<T>(DateParts First, DateParts Second, T Value) where T : struct
{
    public static explicit operator DatePartsPairAnd<T>(YemodaPairAnd<T> pair)
    {
        var (y, m, d) = pair.First;
        var first = new DateParts(y, m, d);
        (y, m, d) = pair.Second;
        var second = new DateParts(y, m, d);
        return new DatePartsPairAnd<T>(first, second, pair.Value);
    }
}

public readonly record struct OrdinalPartsPair(OrdinalParts First, OrdinalParts Second)
{
    public static explicit operator OrdinalPartsPair(YedoyPair pair)
    {
        var (y, doy) = pair.First;
        var first = new OrdinalParts(y, doy);
        (y, doy) = pair.Second;
        var second = new OrdinalParts(y, doy);
        return new OrdinalPartsPair(first, second);
    }
}
// Too big to be a struct (20 bytes).
public sealed record OrdinalPartsPairAnd<T>(OrdinalParts First, OrdinalParts Second, T Value) where T : struct
{
    public static explicit operator OrdinalPartsPairAnd<T>(YedoyPairAnd<T> pair)
    {
        var (y, doy) = pair.First;
        var first = new OrdinalParts(y, doy);
        (y, doy) = pair.Second;
        var second = new OrdinalParts(y, doy);
        return new OrdinalPartsPairAnd<T>(first, second, pair.Value);
    }
}

public readonly record struct MonthPartsPair(MonthParts First, MonthParts Second)
{
    public static explicit operator MonthPartsPair(YemoPair pair)
    {
        var (y, m) = pair.First;
        var first = new MonthParts(y, m);
        (y, m) = pair.Second;
        var second = new MonthParts(y, m);
        return new MonthPartsPair(first, second);
    }
}
// Too big to be a struct (20 bytes).
public sealed record MonthPartsPairAnd<T>(MonthParts First, MonthParts Second, T Value) where T : struct
{
    public static explicit operator MonthPartsPairAnd<T>(YemoPairAnd<T> pair)
    {
        var (y, m) = pair.First;
        var first = new MonthParts(y, m);
        (y, m) = pair.Second;
        var second = new MonthParts(y, m);
        return new MonthPartsPairAnd<T>(first, second, pair.Value);
    }
}

#endregion
#region Math models

// Too big to be a struct (20 bytes).
public sealed record DateDiff(Yemoda Start, Yemoda End, int Years, int Months, int Days);

#endregion
