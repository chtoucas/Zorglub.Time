// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using System.Diagnostics.Contracts;
using System.Linq;

using Zorglub.Testing.Data.Bounded;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;

using static Zorglub.Time.Extensions.CivilDateExtensions;
using static Zorglub.Time.Extensions.DayOfWeekExtensions2;

// NB: We use StandardGregorianDataSet which has the same limits as CivilDate.

public sealed partial class CivilDateTests : IDateFacts<CivilDate, StandardGregorianDataSet>
{
    public CivilDateTests()
        : base(Range.Create(CivilDate.MinYear, CivilDate.MaxYear), CivilDate.Domain)
    {
        MinDate = CivilDate.MinValue;
        MaxDate = CivilDate.MaxValue;
    }

    protected override CivilDate MinDate { get; }
    protected override CivilDate MaxDate { get; }

    [Pure]
    protected override CivilDate GetDate(int y, int m, int d) => new(y, m, d);

    [Pure]
    private static CivilDate CreateDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return new(y, m, d);
    }

    public static TheoryData<Yemoda, Yemoda, int> AddYearsData => DataSet.AddYearsData;
    public static TheoryData<Yemoda, Yemoda, int> AddMonthsData => DataSet.AddMonthsData;
    public static TheoryData<Yemoda, Yemoda, int, int, int> DiffData => DataSet.DiffData;

    public static TheoryData<Yemoda, Yemoda, int> AddYearsCutOffData => GregorianDataSet.AddYearsCutOffData;
    public static TheoryData<Yemoda, Yemoda, int> AddMonthsCutOffData => GregorianDataSet.AddMonthsCutOffData;
    public static TheoryData<Yemoda, Yemoda, int, int, int> DiffCutOffData => GregorianDataSet.DiffCutOffData;
}

public partial class CivilDateTests
{
    [Fact]
    public void Constructor_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => new CivilDate(y, 1, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void Constructor_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => new CivilDate(y, m, 1));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void Constructor_InvalidDay(int y, int m, int d) =>
        Assert.ThrowsAoorexn("day", () => new CivilDate(y, m, d));

    [Theory, MemberData(nameof(DateInfoData))]
    public void Constructor(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        var date = new CivilDate(y, m, d);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public static void Constructor_More(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        var date = new CivilDate(y, m, d);
        var (year, month, day) = date;

        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);

        Assert.Equal(y, year);
        Assert.Equal(m, month);
        Assert.Equal(d, day);

        Assert.Equal(doy, date.DayOfYear);
        Assert.Equal(info.IsIntercalary, date.IsIntercalary);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Deconstructor_ViaDates(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        var date = new CivilDate(y, m, d);
        var (year, month, day) = date;
        // Assert
        Assert.Equal(y, year);
        Assert.Equal(m, month);
        Assert.Equal(d, day);
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void Deconstructor_ViaDayNumbers(DayNumberInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        var date = new CivilDate(y, m, d);
        var (year, month, day) = date;
        // Assert
        Assert.Equal(y, year);
        Assert.Equal(m, month);
        Assert.Equal(d, day);
    }

    [Theory]
    [InlineData(1, 1, 1, "0001-01-01")]
    [InlineData(1, 2, 3, "0001-02-03")]
    [InlineData(11, 12, 13, "0011-12-13")]
    [InlineData(111, 3, 6, "0111-03-06")]
    [InlineData(2019, 1, 3, "2019-01-03")]
    [InlineData(9999, 12, 31, "9999-12-31")]
    public static void ToString_InvariantCulture(int y, int m, int d, string asString)
    {
        var date = new CivilDate(y, m, d);
        // Act & Assert
        Assert.Equal(asString, date.ToString());
    }
}

public partial class CivilDateTests // Properties
{
    [Fact]
    public static void MinValue()
    {
        Assert.Equal(CivilDate.Domain.Min, CivilDate.MinValue.ToDayNumber());
        Assert.Equal(DayZero.NewStyle, CivilDate.MinValue.ToDayNumber());
    }

    [Fact]
    public static void MaxValue() =>
        Assert.Equal(CivilDate.Domain.Max, CivilDate.MaxValue.ToDayNumber());

    [Fact]
    public static void Today()
    {
        var exp = DateTime.Now;
        // Act
        var today = CivilDate.Today();
        // Assert
        Assert.Equal(exp.Year, today.Year);
        Assert.Equal(exp.Month, today.Month);
        Assert.Equal(exp.Day, today.Day);
    }

    [Theory, MemberData(nameof(DayOfWeekData))]
    public void IsoDayOfWeek(YemodaAnd<DayOfWeek> info)
    {
        var (y, m, d, dayOfWeek) = info;
        var date = new CivilDate(y, m, d);
        var dow = dayOfWeek.ToIsoDayOfWeek();
        // Act & Assert
        Assert.Equal(dow, date.IsoDayOfWeek);
    }

    [Theory, MemberData(nameof(CalCalDataSet.DayOfWeekData), MemberType = typeof(CalCalDataSet))]
    public static void IsoDayOfWeek_ViaDayNumbers(DayNumber dayNumber, DayOfWeek dayOfWeek)
    {
        // We filter out ordinals before the epoch.
        if (dayNumber < DayZero.NewStyle) { return; }

        var date = CivilDate.FromDayNumber(dayNumber);
        var dow = dayOfWeek.ToIsoDayOfWeek();
        // Act & Assert
        Assert.Equal(dow, date.IsoDayOfWeek);
    }

    // TODO: update data after we decide what to do w/ IsoWeekOfYear near
    // year boundaries. Idem w/ GetIsoWeekOfYear() below.

    [Theory]
    // 2000
    [InlineData(2000, 12, 24, 52)]
    [InlineData(2000, 12, 25, 53)]
    [InlineData(2000, 12, 31, 53)]
    // 2001: starts on a Monday.
    [InlineData(2001, 1, 1, 1)]
    [InlineData(2001, 1, 7, 1)]
    [InlineData(2001, 1, 8, 2)]
    [InlineData(2001, 12, 23, 51)]
    [InlineData(2001, 12, 24, 52)]
    [InlineData(2001, 12, 30, 52)]
    [InlineData(2001, 12, 31, 53)]
    // 2002
    [InlineData(2002, 1, 1, 1)]
    [InlineData(2002, 1, 6, 1)]
    [InlineData(2002, 1, 7, 2)]
    [InlineData(2002, 12, 22, 51)]
    [InlineData(2002, 12, 23, 52)]
    [InlineData(2002, 12, 29, 52)]
    [InlineData(2002, 12, 30, 53)]
    [InlineData(2002, 12, 31, 53)]
    // 2003
    [InlineData(2003, 1, 1, 1)]
    [InlineData(2003, 1, 5, 1)]
    [InlineData(2003, 1, 6, 2)]
    [InlineData(2003, 12, 21, 51)]
    [InlineData(2003, 12, 22, 52)]
    [InlineData(2003, 12, 28, 52)]
    [InlineData(2003, 12, 29, 53)]
    [InlineData(2003, 12, 31, 53)]
    // 2004
    [InlineData(2004, 1, 1, 1)]
    [InlineData(2004, 1, 4, 1)]
    [InlineData(2004, 1, 5, 2)]
    [InlineData(2004, 12, 20, 52)]
    [InlineData(2004, 12, 26, 52)]
    [InlineData(2004, 12, 27, 53)]
    [InlineData(2004, 12, 31, 53)]
    // 2005
    [InlineData(2005, 1, 1, 1)]
    [InlineData(2005, 1, 2, 1)]
    [InlineData(2005, 1, 3, 2)]
    [InlineData(2005, 1, 9, 2)]
    [InlineData(2005, 1, 10, 3)]
    [InlineData(2005, 12, 19, 52)]
    [InlineData(2005, 12, 25, 52)]
    [InlineData(2005, 12, 26, 53)]
    [InlineData(2005, 12, 31, 53)]
    // 2012: long year.
    [InlineData(2012, 1, 1, 1)]
    [InlineData(2012, 1, 2, 2)]
    [InlineData(2012, 1, 8, 2)]
    [InlineData(2012, 12, 24, 53)]
    [InlineData(2012, 12, 30, 53)]
    [InlineData(2012, 12, 31, 54)]
    public static void WeekOfYear(int y, int m, int d, int weekOfYear)
    {
        var date = new CivilDate(y, m, d);
        // Act & Assert
        Assert.Equal(weekOfYear, date.WeekOfYear);
    }

    [Theory]
    // 2000
    [InlineData(2000, 12, 24, 51)]
    [InlineData(2000, 12, 25, 52)]
    [InlineData(2000, 12, 31, 52)]
    // 2001
    [InlineData(2001, 1, 1, 1)]
    [InlineData(2001, 1, 7, 1)]
    [InlineData(2001, 1, 8, 2)]
    [InlineData(2001, 12, 23, 51)]
    [InlineData(2001, 12, 24, 52)]
    [InlineData(2001, 12, 30, 52)]
    [InlineData(2001, 12, 31, 53)] // 👈 wrong
    // 2002
    [InlineData(2002, 1, 1, 1)]
    [InlineData(2002, 1, 6, 1)]
    [InlineData(2002, 1, 7, 2)]
    [InlineData(2002, 12, 22, 51)]
    [InlineData(2002, 12, 23, 52)]
    [InlineData(2002, 12, 29, 52)]
    [InlineData(2002, 12, 30, 53)] // 👈 wrong
    [InlineData(2002, 12, 31, 53)] // 👈 wrong
    // 2003
    [InlineData(2003, 1, 1, 1)]
    [InlineData(2003, 1, 5, 1)]
    [InlineData(2003, 1, 6, 2)]
    [InlineData(2003, 12, 21, 51)]
    [InlineData(2003, 12, 22, 52)]
    [InlineData(2003, 12, 28, 52)]
    [InlineData(2003, 12, 29, 53)] // 👈 wrong
    [InlineData(2003, 12, 31, 53)] // 👈 wrong
    // 2004
    [InlineData(2004, 1, 1, 1)]
    [InlineData(2004, 1, 4, 1)]
    [InlineData(2004, 1, 5, 2)]
    [InlineData(2004, 12, 20, 52)]
    [InlineData(2004, 12, 26, 52)]
    [InlineData(2004, 12, 27, 53)]
    [InlineData(2004, 12, 31, 53)]
    // 2005
    [InlineData(2005, 1, 1, 0)] // 👈 wrong
    [InlineData(2005, 1, 2, 0)] // 👈 wrong
    [InlineData(2005, 1, 3, 1)]
    [InlineData(2005, 1, 9, 1)]
    [InlineData(2005, 1, 10, 2)]
    [InlineData(2005, 12, 19, 51)]
    [InlineData(2005, 12, 25, 51)]
    [InlineData(2005, 12, 26, 52)]
    [InlineData(2005, 12, 31, 52)]
    // 2012: long year.
    [InlineData(2012, 1, 1, 0)] // 👈 wrong
    [InlineData(2012, 1, 2, 1)]
    [InlineData(2012, 1, 8, 1)]
    [InlineData(2012, 12, 24, 52)]
    [InlineData(2012, 12, 30, 52)]
    [InlineData(2012, 12, 31, 53)] // 👈 wrong
    public static void GetIsoWeekOfYear(int y, int m, int d, int weekOfYear)
    {
        var date = new CivilDate(y, m, d);
        // Act & Assert
        Assert.Equal(weekOfYear, date.GetIsoWeekOfYear());
    }
}

public partial class CivilDateTests // Binary data
{
    [Theory, MemberData(nameof(DateInfoData))]
    public static void FromBinary_InvalidData(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        int bin = new CivilDate(y, m, d).ToBinary();
        // Act & Assert
        Assert.Throws<ArgumentException>("data", () => CivilDate.FromBinary(-1 | bin));
        Assert.Throws<ArgumentException>("data", () => CivilDate.FromBinary((1 << 23) | bin));
    }

    [Theory]
    [InlineData(1 << 23)]
    // All negative values are invalid.
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-1 << 3)]
    [InlineData(-1 << 4)]
    [InlineData(-1 << 5)]
    public static void FromBinary_InvalidData2(int bindata) =>
        Assert.Throws<ArgumentException>("data", () => CivilDate.FromBinary(bindata));

    // We cannot use the prop InvalidYears because of Int32.MinValue (in the
    // code below we map y to (y - 1).
    [Theory]
    [InlineData(CivilDate.MinYear - 1)]
    [InlineData(CivilDate.MaxYear + 1)]
    public void FromBinary_InvalidYear(int y)
    {
        int bin = ((y - 1) << 9) | ((6 - 1) << 5) | (15 - 1);
        Assert.Throws<ArgumentException>("data", () => CivilDate.FromBinary(bin));
    }

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public static void FromBinary_InvalidMonthOfYear(int y, int m)
    {
        int bin = ((y - 1) << 9) | ((m - 1) << 5) | (15 - 1);
        // Act & Assert
        Assert.Throws<ArgumentException>("data", () => CivilDate.FromBinary(bin));
    }

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public static void FromBinary_InvalidDayOfMonth(int y, int m, int d)
    {
        int bin = ((y - 1) << 9) | ((m - 1) << 5) | (d - 1);
        // Act & Assert
        Assert.Throws<ArgumentException>("data", () => CivilDate.FromBinary(bin));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public static void FromBinary_AfterToBinary(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = new CivilDate(y, m, d);
        // Act
        var actual = CivilDate.FromBinary((int)date.ToBinary());
        // Assert
        Assert.Equal(date, actual);
    }
}

public partial class CivilDateTests // Conversions
{
    //[Fact]
    //public void FromDayNumber_InvalidDayNumber() =>
    //    TestInvalidDayNumber(CivilDate.FromDayNumber);

    //[Theory, MemberData(nameof(DayInfoData))]
    //public void FromDayNumber(DayInfo info)
    //{
    //    var (dayNumber, y, m, d) = info;
    //    // Act
    //    var date = CivilDate.FromDayNumber(dayNumber);
    //    // Assert
    //    Assert.Equal(y, date.Year);
    //    Assert.Equal(m, date.Month);
    //    Assert.Equal(d, date.Day);
    //}

    #region FromOrdinalDate()

    [Fact]
    public void FromOrdinalDate_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CivilDate.FromOrdinalDate(y, 1));

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public static void FromOrdinalDate_InvalidDayOfYear(int y, int dayOfYear) =>
        Assert.ThrowsAoorexn("dayOfYear",
            () => CivilDate.FromOrdinalDate(y, dayOfYear));

    [Theory, MemberData(nameof(DateInfoData))]
    public static void FromOrdinalDate(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        var date = CivilDate.FromOrdinalDate(y, doy);

        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);

        Assert.Equal(doy, date.DayOfYear);
    }

    #endregion

    [Theory, MemberData(nameof(DateInfoData))]
    public static void FromDateTime(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var time = new DateTime(y, m, d);
        var date = new CivilDate(y, m, d);
        // Act
        var actual = CivilDate.FromDateTime(time);
        // Assert
        Assert.Equal(date, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public static void ToDateTime(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var time = new DateTime(y, m, d);
        var date = new CivilDate(y, m, d);
        // Act
        var actual = date.ToDateTime();
        // Assert
        Assert.Equal(time, actual);
    }
}

public partial class CivilDateTests // Enumerate days
{
    [Fact]
    public void GetDaysInYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(CivilDate.GetDaysInYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetDaysInYear(YearInfo info)
    {
        int y = info.Year;
        IEnumerable<CivilDate> list
            = from i in Enumerable.Range(1, info.DaysInYear)
              select CivilDate.FromOrdinalDate(y, i);
        // Act
        IEnumerable<CivilDate> actual = CivilDate.GetDaysInYear(y);
        // Assert
        Assert.Equal(list, actual);
    }

    [Fact]
    public void GetDaysInMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CivilDate.GetDaysInMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public static void GetDaysInMonth_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => CivilDate.GetDaysInMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public static void GetDaysInMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        IEnumerable<CivilDate> list =
            from i in Enumerable.Range(1, info.DaysInMonth)
            select new CivilDate(y, m, i);
        // Act
        IEnumerable<CivilDate> actual = CivilDate.GetDaysInMonth(y, m);
        // Assert
        Assert.Equal(list, actual);
    }
}
