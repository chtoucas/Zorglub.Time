// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using System.Linq;

using Zorglub.Time.Core;
using Zorglub.Time.Hemerology;

public sealed partial class CalendarMonthTests : GregorianOnlyTesting
{
    private static readonly JulianCalendar s_Julian = JulianCalendar.Instance;

    public CalendarMonthTests() : base(GregorianCalendar.Instance) { }

    public static DataGroup<YemodaPairAnd<int>> AddYearsData => DataSet.AddYearsData;
    public static DataGroup<YemodaPairAnd<int>> AddMonthsData => DataSet.AddMonthsData;

    // Pour simplifier on utilise un SYearMonthDay, mais le champs "day" doit
    // être complètement ignoré (toujours égal à 1).
    // left, right, leftIsMax, areEqual
    public static TheoryData<Yemoda, Yemoda, bool, bool> MinMaxMonths { get; } = new()
    {
        // Mois.
        { new(3, 4, 1), new(3, 3, 1), true, false },
        { new(3, 4, 1), new(3, 4, 1), true, true },
        { new(3, 4, 1), new(3, 5, 1), false, false },
        // Année.
        { new(3, 4, 1), new(2, 4, 1), true, false },
        { new(3, 4, 1), new(5, 4, 1), false, false },
    };
}

// Construction/deconstruction.
public partial class CalendarMonthTests
{
    //[Theory, MemberData(nameof(SampleDates))]
    //public void Constructor_WithDay(int y, int m, int d, int _4, bool _5, bool _6)
    //{
    //    var date = CalendarUT.NewCalendarDate(y, m, d);
    //    var cmonth = CalendarUT.NewCalendarMonth(y, m);
    //    // Act
    //    var actual = new CalendarMonth(date);
    //    // Assert
    //    Assert.Equal(cmonth, actual);
    //}

    [Theory, MemberData(nameof(MonthInfoData))]
    public void Deconstructor(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        // Act
        var (year, month) = cmonth;
        // Assert
        Assert.Equal(y, year);
        Assert.Equal(m, month);
    }
}

// Properties.
public partial class CalendarMonthTests
{
    [Theory, MemberData(nameof(YearNumberingDataSet.CenturyInfoData), MemberType = typeof(YearNumberingDataSet))]
    public void CenturyOfEra(CenturyInfo info)
    {
        var (y, century, _) = info;
        var cmonth = CalendarUT.GetCalendarMonth(y, 1);
        var centuryOfEra = Ord.Zeroth + century;
        // Act & Assert
        Assert.Equal(centuryOfEra, cmonth.CenturyOfEra);
    }

    [Theory, MemberData(nameof(YearNumberingDataSet.CenturyInfoData), MemberType = typeof(YearNumberingDataSet))]
    public void Century(CenturyInfo info)
    {
        var (y, century, _) = info;
        var cmonth = CalendarUT.GetCalendarMonth(y, 1);
        // Act & Assert
        Assert.Equal(century, cmonth.Century);
    }

    [Theory, MemberData(nameof(YearNumberingDataSet.CenturyInfoData), MemberType = typeof(YearNumberingDataSet))]
    public void YearOfEra(CenturyInfo info)
    {
        int y = info.Year;
        var cmonth = CalendarUT.GetCalendarMonth(y, 1);
        var yearOfEra = Ord.Zeroth + y;
        // Act & Assert
        Assert.Equal(yearOfEra, cmonth.YearOfEra);
    }

    [Theory, MemberData(nameof(YearNumberingDataSet.CenturyInfoData), MemberType = typeof(YearNumberingDataSet))]
    public void YearOfCentury(CenturyInfo info)
    {
        var (y, _, yearOfCentury) = info;
        var cmonth = CalendarUT.GetCalendarMonth(y, 1);
        // Act & Assert
        Assert.Equal(yearOfCentury, cmonth.YearOfCentury);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void IsIntercalary(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        // Assert
        Assert.Equal(info.IsIntercalary, cmonth.IsIntercalary);
    }
}

// Methods.
public partial class CalendarMonthTests
{
    [Fact]
    public void WithCalendar_InvalidCalendar()
    {
        var cmonth = CalendarUT.GetCalendarMonth(3, 4);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => DateRange.FromMonth(cmonth).WithCalendar(null!));
    }

    [Fact]
    public void WithCalendar()
    {
        var cmonth = CalendarUT.GetCalendarMonth(1970, 1);
        var date = s_Julian.GetCalendarDate(1969, 12, 19);
        var range = DateRange.Create(date, 31);
        // Act
        var actual = DateRange.FromMonth(cmonth).WithCalendar(s_Julian);
        // Assert
        Assert.Equal(range, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountElapsedDaysInYear(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        // Act
        int actual = cmonth.CountElapsedDaysInYear();
        // Assert
        Assert.Equal(info.DaysInYearBeforeMonth, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountRemainingDaysInYear(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        // Act
        int actual = cmonth.CountRemainingDaysInYear();
        //
        Assert.Equal(info.DaysInYearAfterMonth, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        // Act
        int actual = cmonth.CountDaysInMonth();
        // Assert
        Assert.Equal(info.DaysInMonth, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetStartOfYear(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        var startOfYear = CalendarUT.GetCalendarDate(y, 1, 1);
        // Act
        var actual = CalendarDate.AtStartOfYear(cmonth.CalendarYear);
        // Assert
        Assert.Equal(startOfYear, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfYear(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        int daysInYear = ((ICalendar)CalendarUT).CountDaysInYear(y);
        var endOfYear = CalendarUT.GetOrdinalDate(y, daysInYear).ToCalendarDate();
        // Act
        var actual = CalendarDate.AtEndOfYear(cmonth.CalendarYear);
        // Assert
        Assert.Equal(endOfYear, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        var startOfMonth = CalendarUT.GetCalendarDate(y, m, 1);
        // Act
        var actual = CalendarDate.AtStartOfMonth(cmonth);
        // Assert
        Assert.Equal(startOfMonth, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDayOfMonth(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act
        var actual = CalendarDate.AtDayOfMonth(cmonth, d);
        // Assert
        Assert.Equal(date, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        int daysInMonth = ((ICalendar)CalendarUT).CountDaysInMonth(y, m);
        var endOfMonth = CalendarUT.GetCalendarDate(y, m, daysInMonth);
        // Act
        var actual = CalendarDate.AtEndOfMonth(cmonth);
        // AssertB
        Assert.Equal(endOfMonth, actual);
    }
}

// Range stuff.
public partial class CalendarMonthTests
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void ToDateRange(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var start = CalendarUT.GetCalendarDate(y, m, 1);
        var end = CalendarUT.GetCalendarDate(y, m, info.DaysInMonth);
        // Act
        var range = DateRange.FromMonth(month);
        // Assert
        Assert.Equal(start, range.Start);
        Assert.Equal(end, range.End);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void Length(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act
        var range = DateRange.FromMonth(month);
        // Assert
        Assert.Equal(info.DaysInMonth, range.Length);
    }

    #region IsSupersetOf()

    [Fact]
    public void IsSupersetOf_RangeA()
    {
        var range1 = DateRange.FromMonth(CalendarUT.GetCalendarMonth(3, 4));
        var range2 = DateRange.FromMonth(CalendarUT.GetCalendarMonth(3, 5));
        // Act & Assert
        Assert.False(range1.IsSupersetOf(range2));
    }

    [Fact]
    public void IsSupersetOf_RangeB()
    {
        var range1 = DateRange.FromMonth(CalendarUT.GetCalendarMonth(3, 4));
        var range2 = DateRange.FromMonth(CalendarUT.GetCalendarMonth(4, 4));
        // Act & Assert
        Assert.False(range1.IsSupersetOf(range2));
    }

    [Fact]
    public void IsSupersetOf_RangeC()
    {
        var range1 = DateRange.FromMonth(CalendarUT.GetCalendarMonth(3, 4));
        var range2 = DateRange.Create(CalendarUT.GetCalendarDate(3, 4, 2), 2);
        // Act & Assert
        Assert.True(range1.IsSupersetOf(range2));
    }

    #endregion
    #region Contains()

    [Theory]
    [InlineData(2, 12, 31, false)]
    [InlineData(3, 2, 28, false)]
    [InlineData(3, 3, 1, true)]
    [InlineData(3, 3, 31, true)]
    [InlineData(3, 4, 1, false)]
    [InlineData(4, 1, 1, false)]
    public void Contains_Date(int y, int m, int d, bool inRange)
    {
        var month = CalendarUT.GetCalendarMonth(3, 3);
        var range = DateRange.FromMonth(month);
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(inRange, range.Contains(date));
    }

    [Theory]
    [InlineData(2, 12, false)]
    [InlineData(3, 2, false)]
    [InlineData(3, 3, true)]
    [InlineData(3, 4, false)]
    [InlineData(4, 1, false)]
    public void Contains_Month(int y, int m, bool inRange)
    {
        var month = CalendarUT.GetCalendarMonth(3, 3);
        var range = DateRange.FromMonth(month);
        var other = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(inRange, range.Contains(other));
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void Contains_Year(int y)
    {
        var month = CalendarUT.GetCalendarMonth(3, 3);
        var range = DateRange.FromMonth(month);
        var year = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.False(range.Contains(year));
    }

    #endregion

    #region IEnumerable

    [Fact]
    public void Enumerate()
    {
        IEnumerable<CalendarDate> listE =
            from i in Enumerable.Range(1, 30)
            select CalendarUT.GetCalendarDate(3, 4, i);
        // Act
        var month = CalendarUT.GetCalendarMonth(3, 4);
        var actual = DateRange.FromMonth(month);
        // Assert
        Assert.Equal(listE, actual);
    }

    #endregion
}

// Math ops.
public partial class CalendarMonthTests
{
    [Fact]
    public void CountMonthsSince_InvalidMonth()
    {
        var left = CalendarUT.GetCalendarMonth(3, 4);
        var right = s_Julian.GetCalendarMonth(3, 4);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => left - right);
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void PlusMonths(YemodaPairAnd<int> info)
    {
        var (xstart, xend, months) = info;
        var start = new CalendarDate(xstart, CalendarUT.Id).CalendarMonth;
        var end = new CalendarDate(xend, CalendarUT.Id).CalendarMonth;
        // Act & Assert
        Assert.Equal(end, start.PlusMonths(months));
        Assert.Equal(end, start + months);
        Assert.Equal(end, start - (-months));

        Assert.Equal(months, end.CountMonthsSince(start));
        Assert.Equal(months, end - start);
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void PlusYears(YemodaPairAnd<int> info)
    {
        var (xstart, xend, years) = info;
        var start = new CalendarDate(xstart, CalendarUT.Id).CalendarMonth;
        var end = new CalendarDate(xend, CalendarUT.Id).CalendarMonth;
        // Act & Assert
        Assert.Equal(end, start.PlusYears(years));
        Assert.Equal(years, end.CountYearsSince(start));
    }
}

// IEquatable<>.
public partial class CalendarMonthTests
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void Equality(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        var same = CalendarUT.GetCalendarMonth(y, m);
        var notSame = CalendarUT.GetCalendarMonth(y, m == 12 ? 2 : 12);

        // Act & Assert
        Assert.True(cmonth == same);
        Assert.True(same == cmonth);
        Assert.False(cmonth == notSame);
        Assert.False(notSame == cmonth);

        Assert.False(cmonth != same);
        Assert.False(same != cmonth);
        Assert.True(cmonth != notSame);
        Assert.True(notSame != cmonth);

        Assert.True(cmonth.Equals(cmonth));
        Assert.True(cmonth.Equals(same));
        Assert.True(same.Equals(cmonth));
        Assert.False(cmonth.Equals(notSame));
        Assert.False(notSame.Equals(cmonth));

        Assert.True(cmonth.Equals((object)same));
        Assert.False(cmonth.Equals((object)notSame));

        Assert.False(cmonth.Equals(new object()));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetHashCode_SanityChecks(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        var same = CalendarUT.GetCalendarMonth(y, m);
        var notSame = CalendarUT.GetCalendarMonth(y, m == 12 ? 2 : 12);
        // Act & Assert
        Assert.Equal(cmonth.GetHashCode(), cmonth.GetHashCode());
        Assert.Equal(cmonth.GetHashCode(), same.GetHashCode());
        Assert.NotEqual(cmonth.GetHashCode(), notSame.GetHashCode());
    }
}

// IComparable<>.
public partial class CalendarMonthTests
{
    [Fact]
    public void CompareTo_WithNull()
    {
        var date = CalendarUT.GetCalendarMonth(3, 4);
        var comparable = (IComparable)date;
        // Act & Assert
        Assert.Equal(1, comparable.CompareTo(null));
    }

    [Fact]
    public void CompareTo_WithInvalidObject()
    {
        var date = CalendarUT.GetCalendarMonth(3, 4);
        var comparable = (IComparable)date;
        object other = new();
        // Act & Assert
        Assert.Throws<ArgumentException>("obj", () => comparable.CompareTo(other));
    }

    [Fact]
    public void CompareTo_WithOtherCalendar()
    {
        var date = CalendarUT.GetCalendarMonth(3, 4);
        var other = s_Julian.GetCalendarMonth(3, 4);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CompareTo(other));
    }

    [Theory, MemberData(nameof(MinMaxMonths))]
    public void CompareTo(Yemoda xleft, Yemoda xright, bool leftIsMax, bool areEqual)
    {
        var left = CalendarUT.GetCalendarMonth(xleft.Year, xleft.Month);
        var right = CalendarUT.GetCalendarMonth(xright.Year, xright.Month);
        var comparable = (IComparable)left;

        // Act & Assert
        if (areEqual)
        {
            Assert.Equal(0, left.CompareTo(right));
            Assert.Equal(0, right.CompareTo(left));
            Assert.Equal(0, comparable.CompareTo(right));
        }
        else if (leftIsMax)
        {
            Assert.True(left.CompareTo(right) > 0);
            Assert.True(right.CompareTo(left) < 0);
            Assert.True(comparable.CompareTo(right) > 0);
        }
        else
        {
            Assert.True(left.CompareTo(right) < 0);
            Assert.True(right.CompareTo(left) > 0);
            Assert.True(comparable.CompareTo(right) < 0);
        }
    }

    [Theory, MemberData(nameof(MinMaxMonths))]
    public void ComparisonOperators(
        Yemoda xleft, Yemoda xright, bool leftIsMax, bool areEqual)
    {
        var left = CalendarUT.GetCalendarMonth(xleft.Year, xleft.Month);
        var right = CalendarUT.GetCalendarMonth(xright.Year, xright.Month);

        // Act & Assert
        if (areEqual)
        {
            Assert.False(left > right);
            Assert.False(left < right);
            Assert.True(left >= right);
            Assert.True(left <= right);

            Assert.False(right > left);
            Assert.False(right < left);
            Assert.True(right >= left);
            Assert.True(right <= left);
        }
        else if (leftIsMax)
        {
            Assert.True(left > right);
            Assert.False(left < right);
            Assert.True(left >= right);
            Assert.False(left <= right);

            Assert.True(right < left);
            Assert.False(right > left);
            Assert.True(right <= left);
            Assert.False(right >= left);
        }
        else
        {
            Assert.False(left > right);
            Assert.True(left < right);
            Assert.False(left >= right);
            Assert.True(left <= right);

            Assert.False(right < left);
            Assert.True(right > left);
            Assert.False(right <= left);
            Assert.True(right >= left);
        }
    }

    [Theory, MemberData(nameof(MinMaxMonths))]
    public void Min(Yemoda xleft, Yemoda xright, bool leftIsMax, bool _4)
    {
        var left = CalendarUT.GetCalendarMonth(xleft.Year, xleft.Month);
        var right = CalendarUT.GetCalendarMonth(xright.Year, xright.Month);
        var min = leftIsMax ? right : left;
        // Act
        var actual = CalendarMonth.Min(left, right);
        // Assert
        Assert.Equal(min, actual);
    }

    [Theory, MemberData(nameof(MinMaxMonths))]
    public void Max(Yemoda xleft, Yemoda xright, bool leftIsMax, bool _4)
    {
        var left = CalendarUT.GetCalendarMonth(xleft.Year, xleft.Month);
        var right = CalendarUT.GetCalendarMonth(xright.Year, xright.Month);
        var max = leftIsMax ? left : right;
        // Act
        var actual = CalendarMonth.Max(left, right);
        // Assert
        Assert.Equal(max, actual);
    }
}

// Formatting.
public partial class CalendarMonthTests
{
    [Theory]
    [InlineData(-1, 1, "01/-0001 (Gregorian)")]
    [InlineData(0, 1, "01/0000 (Gregorian)")]
    [InlineData(1, 1, "01/0001 (Gregorian)")]
    [InlineData(1, 2, "02/0001 (Gregorian)")]
    [InlineData(11, 12, "12/0011 (Gregorian)")]
    [InlineData(111, 3, "03/0111 (Gregorian)")]
    [InlineData(2019, 1, "01/2019 (Gregorian)")]
    [InlineData(9999, 12, "12/9999 (Gregorian)")]
    public void ToString_InvariantCulture(int y, int m, string asString)
    {
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(asString, cmonth.ToString());
    }
}
