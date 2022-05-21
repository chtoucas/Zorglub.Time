// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using System.Linq;

using Zorglub.Time.Core;

public sealed partial class CalendarYearTests : GregorianOnlyTesting
{
    private static readonly JulianCalendar s_Julian = JulianCalendar.Instance;

    public CalendarYearTests() : base(GregorianCalendar.Instance) { }

    public static DataGroup<YemodaPairAnd<int>> AddYearsData => DataSet.AddYearsData;
}

// Construction.
public partial class CalendarYearTests
{
    //[Theory, MemberData(nameof(SampleMonths))]
    //public void Constructor_WithMonth(int y, int m, int _3, int _4, bool _5)
    //{
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
    //    var ordate = CalendarUT.NewOrdinalDate(y, doy);
    //    var cyear = CalendarUT.NewCalendarYear(y);
    //    // Act
    //    var actual = new CalendarYear(ordate);
    //    // Assert
    //    Assert.Equal(cyear, actual);
    //}
}

// Methods.
public partial class CalendarYearTests
{
    [Fact]
    public void WithCalendar_InvalidCalendar()
    {
        var year = CalendarUT.GetCalendarYear(3);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => DateRange.FromYear(year).WithCalendar(null!));
    }

    [Fact]
    public void WithCalendar()
    {
        var year = CalendarUT.GetCalendarYear(1970);
        var date = s_Julian.GetCalendarDate(1969, 12, 19);
        var range = DateRange.Create(date, 365);
        // Act
        var actual = DateRange.FromYear(year).WithCalendar(s_Julian);
        // Assert
        Assert.Equal(range, actual);
    }

    //[Theory, MemberData(nameof(SampleDates))]
    //public void GetOrdinalDate(int y, int _2, int _3, int doy, bool _5, bool _6)
    //{
    //    var cyear = CalendarUT.NewCalendarYear(y);
    //    var ordate = CalendarUT.NewOrdinalDate(y, doy);
    //    // Act
    //    var actual = cyear.GetOrdinalDate(doy);
    //    // Assert
    //    Assert.Equal(ordate, actual);
    //}
}

// Range stuff.
public partial class CalendarYearTests
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void ToDateRange(YearInfo info)
    {
        int y = info.Year;

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
        var cyear = CalendarUT.GetCalendarYear(3);
        // Act & Assert
        Assert.Overflows(() => cyear.PlusYears(Int32.MinValue));
        Assert.Overflows(() => cyear.PlusYears(Int32.MaxValue));
    }

    [Fact]
    public void PlusYears_WithLimitValues_AtMinValue()
    {
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
    //    var start = new CalendarYear(xstart.ToCalendarDate(CalendarUT));
    //    var end = new CalendarYear(xend.ToCalendarDate(CalendarUT));
    //    // Act & Assert
    //    Assert.Equal(years, end.CountYearsSince(start));
    //    Assert.Equal(years, end - start);
    //}

    [Theory, MemberData(nameof(AddYearsData))]
    public void PlusYears(YemodaPairAnd<int> info)
    {
        var (xstart, xend, years) = info;
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
        var cyear = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(asString, cyear.ToString());
    }
}
