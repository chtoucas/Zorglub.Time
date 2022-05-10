// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using System.Linq;

using Zorglub.Time.Core;

public sealed partial class CalendarYearTests : GregorianOnlyTesting
{
    private static readonly JulianCalendar s_Julian = JulianCalendar.Instance;

    public CalendarYearTests() : base(GregorianCalendar.Instance) { }

    public static TheoryData<Yemoda, Yemoda, int> AddYearsData => DataSet.AddYearsData;

    public static TheoryData<int, int, bool, bool> MinMaxYears { get; } = new()
    {
        { 3, 2, true, false },
        { 3, 3, true, true },
        { 3, 4, false, false },
    };
}

// Construction.
public partial class CalendarYearTests
{
    //[Theory, MemberData(nameof(SampleMonths))]
    //public void Constructor_WithMonth(int y, int m, int _3, int _4, bool _5)
    //{
    //    // Arrange
    //    var cmonth = CalendarUT.NewCalendarMonth(y, m);
    //    var cyear = CalendarUT.NewCalendarYear(y);
    //    // Act
    //    var actual = new CalendarYear(cmonth);
    //    // Assert
    //    Assert.Equal(cyear, actual);
    //}

    //[Theory, MemberData(nameof(SampleDates))]
    //public void Constructor_WithDay(int y, int m, int d, int _4, bool _5, bool _6)
    //{
    //    // Arrange
    //    var date = CalendarUT.NewCalendarDate(y, m, d);
    //    var cyear = CalendarUT.NewCalendarYear(y);
    //    // Act
    //    var actual = new CalendarYear(date);
    //    // Assert
    //    Assert.Equal(cyear, actual);
    //}

    //[Theory, MemberData(nameof(SampleDates))]
    //public void Constructor_WithOrdinal(int y, int _2, int _3, int doy, bool _5, bool _6)
    //{
    //    // Arrange
    //    var ordate = CalendarUT.NewOrdinalDate(y, doy);
    //    var cyear = CalendarUT.NewCalendarYear(y);
    //    // Act
    //    var actual = new CalendarYear(ordate);
    //    // Assert
    //    Assert.Equal(cyear, actual);
    //}
}

// Properties.
public partial class CalendarYearTests
{
    [Theory, MemberData(nameof(YearNumberingDataSet.CenturyInfoData), MemberType = typeof(YearNumberingDataSet))]
    public void CenturyOfEra(CenturyInfo info)
    {
        var (y, century, _) = info;
        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        var centuryOfEra = Ord.Zeroth + century;
        // Act & Assert
        Assert.Equal(centuryOfEra, cyear.CenturyOfEra);
    }

    [Theory, MemberData(nameof(YearNumberingDataSet.CenturyInfoData), MemberType = typeof(YearNumberingDataSet))]
    public void Century(CenturyInfo info)
    {
        var (y, century, _) = info;
        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(century, cyear.Century);
    }

    [Theory, MemberData(nameof(YearNumberingDataSet.CenturyInfoData), MemberType = typeof(YearNumberingDataSet))]
    public void YearOfEra(CenturyInfo info)
    {
        int y = info.Year;
        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        var yearOfEra = Ord.Zeroth + y;
        // Act & Assert
        Assert.Equal(yearOfEra, cyear.YearOfEra);
    }

    [Theory, MemberData(nameof(YearNumberingDataSet.CenturyInfoData), MemberType = typeof(YearNumberingDataSet))]
    public void YearOfCentury(CenturyInfo info)
    {
        var (y, _, yearOfCentury) = info;
        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(yearOfCentury, cyear.YearOfCentury);
    }

    [Theory, MemberData(nameof(YearNumberingDataSet.CenturyInfoData), MemberType = typeof(YearNumberingDataSet))]
    public void Year(CenturyInfo info)
    {
        int y = info.Year;
        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(y, cyear.Year);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void IsLeap(YearInfo info)
    {
        int y = info.Year;

        // Act
        var cyear = CalendarUT.GetCalendarYear(y);
        // Assert
        Assert.Equal(info.IsLeap, cyear.IsLeap);
    }
}

// Methods.
public partial class CalendarYearTests
{
    [Fact]
    public void WithCalendar_InvalidCalendar()
    {
        // Arrange
        var cyear = CalendarUT.GetCalendarYear(3);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => DateRange.FromYear(cyear).WithCalendar(null!));
    }

    [Fact]
    public void WithCalendar()
    {
        // Arrange
        var cyear = CalendarUT.GetCalendarYear(1970);
        var date = s_Julian.GetCalendarDate(1969, 12, 19);
        var range = DateRange.Create(date, 365);
        // Act
        var actual = DateRange.FromYear(cyear).WithCalendar(s_Julian);
        // Assert
        Assert.Equal(range, actual);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountMonthsInYear(YearInfo info)
    {
        int y = info.Year;

        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(info.MonthsInYear, cyear.CountMonthsInYear());
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYear(YearInfo info)
    {
        int y = info.Year;

        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(info.DaysInYear, cyear.CountDaysInYear());
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetStartOfYear(YearInfo info)
    {
        int y = info.Year;

        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        var startOfYear = CalendarUT.GetCalendarDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, CalendarDate.AtStartOfYear(cyear));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDayOfYear(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        var exp = CalendarUT.GetCalendarDate(y, m, d);
        // Act
        var actual = CalendarDate.AtDayOfYear(cyear, doy);
        // Assert
        Assert.Equal(exp, actual);
    }

    //[Theory, MemberData(nameof(SampleDates))]
    //public void GetOrdinalDate(int y, int _2, int _3, int doy, bool _5, bool _6)
    //{
    //    // Arrange
    //    var cyear = CalendarUT.NewCalendarYear(y);
    //    var ordate = CalendarUT.NewOrdinalDate(y, doy);
    //    // Act
    //    var actual = cyear.GetOrdinalDate(doy);
    //    // Assert
    //    Assert.Equal(ordate, actual);
    //}

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear(YearInfo info)
    {
        int y = info.Year;

        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        var endOfYear = CalendarUT.GetOrdinalDate(y, info.DaysInYear).ToCalendarDate();
        // Act & Assert
        Assert.Equal(endOfYear, CalendarDate.AtEndOfYear(cyear));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetMonthOfYear(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(cmonth, cyear.GetMonthOfYear(m));
    }

    [Fact]
    public void GetMonthsInYear()
    {
        // Arrange
        var cyear = CalendarUT.GetCalendarYear(3);
        var list = from i in Enumerable.Range(1, 12)
                   select CalendarUT.GetCalendarMonth(3, i);
        // Act
        var actual = cyear.GetMonthsInYear();
        // Assert
        Assert.Equal(list, actual);
    }
}

// Range stuff.
public partial class CalendarYearTests
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void ToDateRange(YearInfo info)
    {
        int y = info.Year;

        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        var start = CalendarUT.GetCalendarDate(y, 1, 1);
        var end = CalendarUT.GetCalendarDate(y, 12, 31);
        // Act
        var range = DateRange.FromYear(cyear);
        // Assert
        Assert.Equal(start, range.Start);
        Assert.Equal(end, range.End);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void Length(YearInfo info)
    {
        int y = info.Year;

        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        // Act
        var range = DateRange.FromYear(cyear);
        // Assert
        Assert.Equal(info.DaysInYear, range.Length);
    }

    #region Contains()

    [Theory]
    [InlineData(2, 12, 31, false)]
    [InlineData(3, 1, 1, true)]
    [InlineData(3, 5, 10, true)]
    [InlineData(3, 12, 31, true)]
    [InlineData(4, 1, 1, false)]
    public void Contains_Date(int y, int m, int d, bool inRange)
    {
        // Arrange
        var range = DateRange.FromYear(CalendarUT.GetCalendarYear(3));
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(inRange, range.Contains(date));
    }

    [Theory]
    [InlineData(2, 12, false)]
    [InlineData(3, 1, true)]
    [InlineData(3, 2, true)]
    [InlineData(3, 3, true)]
    [InlineData(3, 4, true)]
    [InlineData(3, 5, true)]
    [InlineData(3, 6, true)]
    [InlineData(3, 7, true)]
    [InlineData(3, 8, true)]
    [InlineData(3, 9, true)]
    [InlineData(3, 10, true)]
    [InlineData(3, 11, true)]
    [InlineData(3, 12, true)]
    [InlineData(4, 1, false)]
    public void Contains_Month(int y, int m, bool inRange)
    {
        // Arrange
        var range = DateRange.FromYear(CalendarUT.GetCalendarYear(3));
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(inRange, range.Contains(month));
    }

    [Theory]
    [InlineData(2, 12, false)]
    [InlineData(3, 1, true)]
    [InlineData(3, 2, true)]
    [InlineData(3, 3, true)]
    [InlineData(3, 4, true)]
    [InlineData(3, 5, true)]
    [InlineData(3, 6, true)]
    [InlineData(3, 7, true)]
    [InlineData(3, 8, true)]
    [InlineData(3, 9, true)]
    [InlineData(3, 10, true)]
    [InlineData(3, 11, true)]
    [InlineData(3, 12, true)]
    [InlineData(4, 1, false)]
    public void IsSupersetOf_MonthRange(int y, int m, bool inRange)
    {
        // Arrange
        var range = DateRange.FromYear(CalendarUT.GetCalendarYear(3));
        var other = DateRange.FromMonth(CalendarUT.GetCalendarMonth(y, m));
        // Act & Assert
        Assert.Equal(inRange, range.IsSupersetOf(other));
    }

    [Theory]
    [InlineData(2, false)]
    [InlineData(3, true)]
    [InlineData(4, false)]
    public void Contains_Year(int y, bool inRange)
    {
        // Arrange
        var range = DateRange.FromYear(CalendarUT.GetCalendarYear(3));
        var year1 = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(inRange, range.Contains(year1));
    }

    [Theory]
    [InlineData(2, false)]
    [InlineData(3, true)]
    [InlineData(4, false)]
    public void IsSupersetOf_YearRange(int y, bool inRange)
    {
        // Arrange
        var range = DateRange.FromYear(CalendarUT.GetCalendarYear(3));
        var other = DateRange.FromYear(CalendarUT.GetCalendarYear(y));
        // Act & Assert
        Assert.Equal(inRange, range.IsSupersetOf(other));
    }

    // start, end, inRange
    public static TheoryData<Yemoda, Yemoda, bool> Start_End_InRange { get; } = new()
    {
        { new(1, 1, 1), new(2, 12, 31), false },    // Avant
        { new(2, 12, 31), new(3, 1, 1), false },    // À cheval
        { new(2, 12, 31), new(3, 5, 1), false },    // À cheval
        { new(3, 1, 1), new(3, 1, 1), true },       // OK
        { new(3, 2, 1), new(3, 2, 25), true },      // OK
        { new(3, 2, 1), new(3, 12, 31), true },     // OK
        { new(3, 2, 1), new(4, 1, 1), false },      // À cheval
        { new(3, 12, 31), new(4, 1, 1), false },    // À cheval
        { new(4, 1, 1), new(4, 1, 1), false },      // Après
        { new(4, 5, 2), new(4, 6, 1), false },      // Après
    };

    #endregion

    #region IEnumerable

    [Fact]
    public void Enumerate()
    {
        // Arrange
        IEnumerable<CalendarDate> listE
            = from i in Enumerable.Range(1, 365)
              select CalendarUT.GetOrdinalDate(3, i).ToCalendarDate();
        // Act
        var actual = DateRange.FromYear(CalendarUT.GetCalendarYear(3));
        // Assert
        Assert.Equal(listE, actual);
    }

    #endregion
}

// Math ops: PlusYears(), CountYearsSince(), NextYear(), PreviousYear().
public partial class CalendarYearTests
{
    [Fact]
    public void PlusYears_OverflowOrUnderflow()
    {
        // Arrange
        var cyear = CalendarUT.GetCalendarYear(3);
        // Act & Assert
        Assert.Overflows(() => cyear.PlusYears(Int32.MinValue));
        Assert.Overflows(() => cyear.PlusYears(Int32.MaxValue));
    }

    [Fact]
    public void PlusYears_WithLimitValues_AtMinValue()
    {
        // Arrange
        var scope = CalendarUT.Scope;
        var min = CalendarUT.GetCalendarYear(scope.SupportedYears.Min);
        int years = scope.SupportedYears.Max - scope.SupportedYears.Min;
        var exp = CalendarUT.GetCalendarYear(scope.SupportedYears.Max);
        // Act & Assert
        Assert.Overflows(() => min.PlusYears(-1));
        Assert.Equal(min, min.PlusYears(0));
        Assert.Equal(exp, min.PlusYears(years));
        Assert.Overflows(() => min.PlusYears(years + 1));
    }

    [Fact]
    public void PlusYears_WithLimitValues_AtMaxValue()
    {
        // Arrange
        var scope = CalendarUT.Scope;
        var max = CalendarUT.GetCalendarYear(scope.SupportedYears.Max);
        int years = scope.SupportedYears.Max - scope.SupportedYears.Min;
        var exp = CalendarUT.GetCalendarYear(scope.SupportedYears.Min);
        // Act & Assert
        Assert.Overflows(() => max.PlusYears(-years - 1));
        Assert.Equal(exp, max.PlusYears(-years));
        Assert.Equal(max, max.PlusYears(0));
        Assert.Overflows(() => max.PlusYears(1));
    }

    [Fact]
    public void PlusYears_WithLimitValues()
    {
        // Arrange
        var scope = CalendarUT.Scope;
        var cyear = CalendarUT.GetCalendarYear(3);
        int minYears = scope.SupportedYears.Min - cyear.Year;
        int maxYears = scope.SupportedYears.Max - cyear.Year;
        var earliest = CalendarUT.GetCalendarYear(scope.SupportedYears.Min);
        var latest = CalendarUT.GetCalendarYear(scope.SupportedYears.Max);
        // Act & Assert
        Assert.Overflows(() => cyear.PlusYears(minYears - 1));
        Assert.Equal(earliest, cyear.PlusYears(minYears));
        Assert.Equal(latest, cyear.PlusYears(maxYears));
        Assert.Overflows(() => cyear.PlusYears(maxYears + 1));
    }

    [Fact]
    public void CountYearsSince_DoesNotOverflow()
    {
        // Arrange
        var scope = CalendarUT.Scope;
        var min = CalendarUT.GetCalendarYear(scope.SupportedYears.Min);
        var max = CalendarUT.GetCalendarYear(scope.SupportedYears.Max);
        int years = scope.SupportedYears.Max - scope.SupportedYears.Min;
        // Act & Assert
        Assert.Equal(years, max.CountYearsSince(min));
        Assert.Equal(-years, min.CountYearsSince(max));
    }

    [Fact]
    public void CountYearsSince_InvalidYear()
    {
        // Arrange
        var left = CalendarUT.GetCalendarYear(3);
        var right = s_Julian.GetCalendarYear(3);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => left - right);
    }

    // TODO: (MATH) CountYearsSince(), itou avec CalendarMonthTests().
    // Utiliser aussi les données DiffCutoff (idem avec PlusYears).
    // Revoir tous les tests des "math ops" pour les objets calendaires.
    //[Theory, MemberData(nameof(GregorianData.Diff), MemberType = typeof(GregorianData))]
    //public void CountYearsSince(Yemoda xstart, Yemoda xend, int years, int months, int days)
    //{
    //    // Arrange
    //    var start = new CalendarYear(xstart.ToCalendarDate(CalendarUT));
    //    var end = new CalendarYear(xend.ToCalendarDate(CalendarUT));
    //    // Act & Assert
    //    Assert.Equal(years, end.CountYearsSince(start));
    //    Assert.Equal(years, end - start);
    //}

    [Theory, MemberData(nameof(AddYearsData))]
    public void PlusYears(Yemoda xstart, Yemoda xend, int years)
    {
        // Arrange
        var start = new CalendarDate(xstart, CalendarUT.Id).CalendarYear;
        var end = new CalendarDate(xend, CalendarUT.Id).CalendarYear;
        // Act & Assert
        Assert.Equal(end, start.PlusYears(years));
        Assert.Equal(end, start + years);
        Assert.Equal(end, start - (-years));

        Assert.Equal(years, end.CountYearsSince(start));
        Assert.Equal(years, end - start);
    }

    [Fact]
    public void NextYear_Overflows_AtMaxValue()
    {
        var max = CalendarUT.GetCalendarYear(CalendarUT.Scope.SupportedYears.Max);
        Assert.Overflows(() => max.NextYear());
        Assert.Overflows(() => max++);
    }

    [Fact]
    public void PreviousYear_Underflows_AtMinValue()
    {
        var min = CalendarUT.GetCalendarYear(CalendarUT.Scope.SupportedYears.Min);
        Assert.Overflows(() => min.PreviousYear());
        Assert.Overflows(() => min--);
    }
}

// IComparable<>.
public partial class CalendarYearTests
{
    [Fact]
    public void CompareTo_WithNull()
    {
        // Arrange
        var date = CalendarUT.GetCalendarYear(3);
        var comparable = (IComparable)date;
        // Act & Assert
        Assert.Equal(1, comparable.CompareTo(null));
    }

    [Fact]
    public void CompareTo_WithInvalidObject()
    {
        // Arrange
        var date = CalendarUT.GetCalendarYear(3);
        var comparable = (IComparable)date;
        object other = new();
        // Act & Assert
        Assert.Throws<ArgumentException>("obj", () => comparable.CompareTo(other));
    }

    [Fact]
    public void CompareTo_WithOtherCalendar()
    {
        // Arrange
        var date = CalendarUT.GetCalendarYear(3);
        var other = s_Julian.GetCalendarYear(3);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CompareTo(other));
    }

    [Theory, MemberData(nameof(MinMaxYears))]
    public void CompareTo(
        int start, int end, bool leftIsMax, bool areEqual)
    {
        // Arrange
        var left = CalendarUT.GetCalendarYear(start);
        var right = CalendarUT.GetCalendarYear(end);
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

    [Theory, MemberData(nameof(MinMaxYears))]
    public void ComparisonOperators(
        int start, int end, bool leftIsMax, bool areEqual)
    {
        // Arrange
        var left = CalendarUT.GetCalendarYear(start);
        var right = CalendarUT.GetCalendarYear(end);

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

    [Theory, MemberData(nameof(MinMaxYears))]
    public void Min(int start, int end, bool leftIsMax, bool _4)
    {
        // Arrange
        var left = CalendarUT.GetCalendarYear(start);
        var right = CalendarUT.GetCalendarYear(end);
        var min = leftIsMax ? right : left;
        // Act
        var actual = CalendarYear.Min(left, right);
        // Assert
        Assert.Equal(min, actual);
    }

    [Theory, MemberData(nameof(MinMaxYears))]
    public void Max(int start, int end, bool leftIsMax, bool _4)
    {
        // Arrange
        var left = CalendarUT.GetCalendarYear(start);
        var right = CalendarUT.GetCalendarYear(end);
        var max = leftIsMax ? left : right;
        // Act
        var actual = CalendarYear.Max(left, right);
        // Assert
        Assert.Equal(max, actual);
    }
}

// IEquatable<>.
public partial class CalendarYearTests
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void Equality(YearInfo info)
    {
        int y = info.Year;

        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        var same = CalendarUT.GetCalendarYear(y);
        var notSame = CalendarUT.GetCalendarYear(y + 1);

        // Act & Assert
        Assert.True(cyear == same);
        Assert.True(same == cyear);
        Assert.False(cyear == notSame);
        Assert.False(notSame == cyear);

        Assert.False(cyear != same);
        Assert.False(same != cyear);
        Assert.True(cyear != notSame);
        Assert.True(notSame != cyear);

        Assert.True(cyear.Equals(cyear));
        Assert.True(cyear.Equals(same));
        Assert.True(same.Equals(cyear));
        Assert.False(cyear.Equals(notSame));
        Assert.False(notSame.Equals(cyear));

        Assert.True(cyear.Equals((object)same));
        Assert.False(cyear.Equals((object)notSame));

        Assert.False(cyear.Equals(new object()));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetHashCode_SanityChecks(YearInfo info)
    {
        int y = info.Year;

        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        var same = CalendarUT.GetCalendarYear(y);
        var notSame = CalendarUT.GetCalendarYear(y + 1);
        // Act & Assert
        Assert.Equal(cyear.GetHashCode(), cyear.GetHashCode());
        Assert.Equal(cyear.GetHashCode(), same.GetHashCode());
        Assert.NotEqual(cyear.GetHashCode(), notSame.GetHashCode());
    }
}

// Formatting.
public partial class CalendarYearTests
{
    [Theory]
    [InlineData(-1, "-0001 (Gregorian)")]
    [InlineData(0, "0000 (Gregorian)")]
    [InlineData(1, "0001 (Gregorian)")]
    [InlineData(11, "0011 (Gregorian)")]
    [InlineData(111, "0111 (Gregorian)")]
    [InlineData(2019, "2019 (Gregorian)")]
    [InlineData(9999, "9999 (Gregorian)")]
    public void ToString_InvariantCulture(int y, string asString)
    {
        // Arrange
        var cyear = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(asString, cyear.ToString());
    }
}
