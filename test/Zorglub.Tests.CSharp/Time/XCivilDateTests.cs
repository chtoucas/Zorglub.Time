// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

using System.Diagnostics.Contracts;
using System.Linq;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Bounded;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;

using static Zorglub.Time.Extensions.XCivilDateExtensions;

// NB: we use StandardGregorianDataSet which has the same limits as XCivilDate.

public sealed partial class XCivilDateTests : CalendarDataConsumer<StandardGregorianDataSet>
{
    public XCivilDateTests()
    {
        var supportedYears = Range.Create(XCivilDate.MinYear, XCivilDate.MaxYear);
        SupportedYearsTester = new SupportedYearsTester(supportedYears);
    }

    private SupportedYearsTester SupportedYearsTester { get; }

    [Pure]
    private static XCivilDate CreateDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return new XCivilDate(y, m, d);
    }

    // TODO(code): filter data.
    public static DataGroup<DateDiff> DateDiffData =>
        GregorianMathDataSetUnambiguous.DateDiffData;
    public static DataGroup<YemodaPairAnd<int>> AddYearsCutOffData =>
        GregorianMathDataSetCutOff.Instance.AddYearsData;
    public static DataGroup<YemodaPairAnd<int>> AddMonthsCutOffData =>
        GregorianMathDataSetCutOff.Instance.AddMonthsData;
    public static DataGroup<DateDiff> DateDiffCutOffData =>
        GregorianMathDataSetCutOff.Instance.DateDiffData;

    // IDayOfWeekDataSet
    public static DataGroup<YemodaAnd<DayOfWeek>> DayOfWeekData => DataSet.DayOfWeekData;
    public static DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_Before_Data => DataSet.DayOfWeek_Before_Data;
    public static DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_OnOrBefore_Data => DataSet.DayOfWeek_OnOrBefore_Data;
    public static DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_Nearest_Data => DataSet.DayOfWeek_Nearest_Data;
    public static DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_OnOrAfter_Data => DataSet.DayOfWeek_OnOrAfter_Data;
    public static DataGroup<YemodaPairAnd<DayOfWeek>> DayOfWeek_After_Data => DataSet.DayOfWeek_After_Data;
}

public partial class XCivilDateTests
{
    [Fact]
    public void Constructor_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => new XCivilDate(y, 1, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void Constructor_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => new XCivilDate(y, m, 1));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void Constructor_InvalidDay(int y, int m, int d) =>
        Assert.ThrowsAoorexn("day", () => new XCivilDate(y, m, d));

    [Theory, MemberData(nameof(DateInfoData))]
    public void Constructor(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        var date = new XCivilDate(y, m, d);
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
        var date = new XCivilDate(y, m, d);
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
        var date = new XCivilDate(y, m, d);
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
        var date = new XCivilDate(y, m, d);
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
        var date = new XCivilDate(y, m, d);
        // Act & Assert
        Assert.Equal(asString, date.ToString());
    }
}

public partial class XCivilDateTests // Properties
{
    [Fact]
    public static void MinValue()
    {
        Assert.Equal(XCivilDate.Domain.Min, XCivilDate.MinValue.DayNumber);
        Assert.Equal(DayZero.NewStyle, XCivilDate.MinValue.DayNumber);
    }

    [Fact]
    public static void MaxValue() =>
        Assert.Equal(XCivilDate.Domain.Max, XCivilDate.MaxValue.DayNumber);

    [Fact]
    public static void Today()
    {
        var exp = DateTime.Now;
        // Act
        var today = XCivilDate.Today();
        // Assert
        Assert.Equal(exp.Year, today.Year);
        Assert.Equal(exp.Month, today.Month);
        Assert.Equal(exp.Day, today.Day);
    }

    [Theory, MemberData(nameof(DayOfWeekData))]
    public void IsoWeekday(YemodaAnd<DayOfWeek> info)
    {
        var (y, m, d, dayOfWeek) = info;
        var date = new XCivilDate(y, m, d);
        var dow = dayOfWeek.ToIsoWeekday();
        // Act & Assert
        Assert.Equal(dow, date.IsoWeekday);
    }

    // TODO(fact): filter data.
    [Theory, MemberData(nameof(CalCalDataSet.DayNumberToDayOfWeekData), MemberType = typeof(CalCalDataSet))]
    public static void IsoWeekday_ViaDayNumbers(DayNumber dayNumber, DayOfWeek dayOfWeek)
    {
        // We filter out ordinals before the epoch.
        // TODO(code): we should be stricter and use the domain.
        if (dayNumber < DayZero.NewStyle) { return; }

        var date = XCivilDate.FromDayNumber(dayNumber);
        var dow = dayOfWeek.ToIsoWeekday();
        // Act & Assert
        Assert.Equal(dow, date.IsoWeekday);
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
        var date = new XCivilDate(y, m, d);
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
        var date = new XCivilDate(y, m, d);
        // Act & Assert
        Assert.Equal(weekOfYear, date.GetIsoWeekOfYear());
    }
}

public partial class XCivilDateTests // Binary data
{
    [Theory, MemberData(nameof(DateInfoData))]
    public static void FromBinary_InvalidData(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        int bin = new XCivilDate(y, m, d).ToBinary();
        // Act & Assert
        Assert.Throws<ArgumentException>("data", () => XCivilDate.FromBinary(-1 | bin));
        Assert.Throws<ArgumentException>("data", () => XCivilDate.FromBinary((1 << 23) | bin));
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
        Assert.Throws<ArgumentException>("data", () => XCivilDate.FromBinary(bindata));

    // We cannot use the prop InvalidYears because of Int32.MinValue (in the
    // code below we map y to (y - 1).
    [Theory]
    [InlineData(XCivilDate.MinYear - 1)]
    [InlineData(XCivilDate.MaxYear + 1)]
    public void FromBinary_InvalidYear(int y)
    {
        int bin = ((y - 1) << 9) | ((6 - 1) << 5) | (15 - 1);
        Assert.Throws<ArgumentException>("data", () => XCivilDate.FromBinary(bin));
    }

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public static void FromBinary_InvalidMonthOfYear(int y, int m)
    {
        int bin = ((y - 1) << 9) | ((m - 1) << 5) | (15 - 1);
        // Act & Assert
        Assert.Throws<ArgumentException>("data", () => XCivilDate.FromBinary(bin));
    }

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public static void FromBinary_InvalidDayOfMonth(int y, int m, int d)
    {
        int bin = ((y - 1) << 9) | ((m - 1) << 5) | (d - 1);
        // Act & Assert
        Assert.Throws<ArgumentException>("data", () => XCivilDate.FromBinary(bin));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public static void FromBinary_AfterToBinary(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = new XCivilDate(y, m, d);
        // Act
        var actual = XCivilDate.FromBinary(date.ToBinary());
        // Assert
        Assert.Equal(date, actual);
    }
}

public partial class XCivilDateTests // Conversions
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
        SupportedYearsTester.TestInvalidYear(y => XCivilDate.FromOrdinalDate(y, 1));

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public static void FromOrdinalDate_InvalidDayOfYear(int y, int doy) =>
        Assert.ThrowsAoorexn("dayOfYear",
            () => XCivilDate.FromOrdinalDate(y, doy));

    [Theory, MemberData(nameof(DateInfoData))]
    public static void FromOrdinalDate(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        var date = XCivilDate.FromOrdinalDate(y, doy);

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
        var date = new XCivilDate(y, m, d);
        // Act
        var actual = XCivilDate.FromDateTime(time);
        // Assert
        Assert.Equal(date, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public static void ToDateTime(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var time = new DateTime(y, m, d);
        var date = new XCivilDate(y, m, d);
        // Act
        var actual = date.ToDateTime();
        // Assert
        Assert.Equal(time, actual);
    }
}

public partial class XCivilDateTests // Enumerate days
{
    [Fact]
    public void GetDaysInYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(XCivilDate.GetDaysInYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetDaysInYear(YearInfo info)
    {
        int y = info.Year;
        IEnumerable<XCivilDate> list
            = from i in Enumerable.Range(1, info.DaysInYear)
              select XCivilDate.FromOrdinalDate(y, i);
        // Act
        IEnumerable<XCivilDate> actual = XCivilDate.GetDaysInYear(y);
        // Assert
        Assert.Equal(list, actual);
    }

    [Fact]
    public void GetDaysInMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => XCivilDate.GetDaysInMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public static void GetDaysInMonth_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => XCivilDate.GetDaysInMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public static void GetDaysInMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        IEnumerable<XCivilDate> list =
            from i in Enumerable.Range(1, info.DaysInMonth)
            select new XCivilDate(y, m, i);
        // Act
        IEnumerable<XCivilDate> actual = XCivilDate.GetDaysInMonth(y, m);
        // Assert
        Assert.Equal(list, actual);
    }
}
