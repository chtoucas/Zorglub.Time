// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Core;

public sealed partial class OrdinalDateTests : GregorianOnlyTesting
{
    private static readonly JulianCalendar s_Julian = JulianCalendar.Instance;

    public OrdinalDateTests() : base(GregorianCalendar.Instance) { }

    //public static TheoryData<Yemoda, Yemoda, bool, bool> MinMax => GregorianData.MinMax;
}

public partial class OrdinalDateTests
{
    //[Theory, MemberData(nameof(DateInfoData))]
    //public void Deconstructor(DateInfo info)
    //{
    //    var (y, doy) = info.Yedoy;
    //    var ordate = CalendarUT.GetOrdinalDate(y, doy);
    //    // Act
    //    var (year, dayOfYear) = ordate;
    //    // Assert
    //    Assert.Equal(y, year);
    //    Assert.Equal(doy, dayOfYear);
    //}

    [Theory]
    [InlineData(-1, 1, "001/-0001 (Gregorian)")]
    [InlineData(0, 1, "001/0000 (Gregorian)")]
    [InlineData(1, 1, "001/0001 (Gregorian)")]
    [InlineData(1, 3, "003/0001 (Gregorian)")]
    [InlineData(11, 254, "254/0011 (Gregorian)")]
    [InlineData(111, 26, "026/0111 (Gregorian)")]
    [InlineData(2019, 3, "003/2019 (Gregorian)")]
    [InlineData(9999, 365, "365/9999 (Gregorian)")]
    public void ToString_InvariantCulture(int y, int doy, string asString)
    {
        var ordate = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(asString, ordate.ToString());
    }
}

public partial class OrdinalDateTests // Properties
{
    //[Theory, MemberData(nameof(YearNumberingDataSet.CenturyInfoData), MemberType = typeof(YearNumberingDataSet))]
    //public void CenturyOfEra_Prop(CenturyInfo info)
    //{
    //    var (y, century, _) = info;
    //    var ordate = CalendarUT.GetOrdinalDate(y, 1);
    //    var centuryOfEra = Ord.Zeroth + century;
    //    // Act & Assert
    //    Assert.Equal(centuryOfEra, ordate.CenturyOfEra);
    //}

    //[Theory, MemberData(nameof(YearNumberingDataSet.CenturyInfoData), MemberType = typeof(YearNumberingDataSet))]
    //public void Century_Prop(CenturyInfo info)
    //{
    //    var (y, century, _) = info;
    //    var ordate = CalendarUT.GetOrdinalDate(y, 1);
    //    // Act & Assert
    //    Assert.Equal(century, ordate.Century);
    //}

    //[Theory, MemberData(nameof(YearNumberingDataSet.CenturyInfoData), MemberType = typeof(YearNumberingDataSet))]
    //public void YearOfEra_Prop(CenturyInfo info)
    //{
    //    int y = info.Year;
    //    var ordate = CalendarUT.GetOrdinalDate(y, 1);
    //    var yearOfEra = Ord.Zeroth + y;
    //    // Act & Assert
    //    Assert.Equal(yearOfEra, ordate.YearOfEra);
    //}

    //[Theory, MemberData(nameof(YearNumberingDataSet.CenturyInfoData), MemberType = typeof(YearNumberingDataSet))]
    //public void YearOfCentury_Prop(CenturyInfo info)
    //{
    //    var (y, _, yearOfCentury) = info;
    //    var ordate = CalendarUT.GetOrdinalDate(y, 1);
    //    // Act & Assert
    //    Assert.Equal(yearOfCentury, ordate.YearOfCentury);
    //}
}

public partial class OrdinalDateTests // Conversions
{
    #region ToDayNumber()

    // Calendar.GetDayNumber().
    [Theory, MemberData(nameof(DateInfoData))]
    public void ToDayNumber_ViaDates(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var dayNumber = CalendarUT.GetCalendarDate(y, m, d).ToDayNumber();
        var ordate = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(dayNumber, ordate.ToDayNumber());
    }

    // Calendar.GetDayNumber().
    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void ToDayNumber_ViaDayNumbers(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;

        var date = CalendarUT.GetCalendarDate(y, m, d);
        var ordate = CalendarUT.GetOrdinalDate(y, date.DayOfYear);
        // Act & Assert
        Assert.Equal(dayNumber, ordate.ToDayNumber());
    }

    #endregion
    #region ToCalendarDate()

    [Fact]
    public void ToCalendarDate_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetOrdinalDate(y, 1));

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void ToCalendarDate_InvalidDayOfYear(int y, int dayOfYear) =>
        Assert.ThrowsAoorexn("dayOfYear", () => CalendarUT.GetOrdinalDate(y, dayOfYear));

    #endregion
    #region WithCalendar()

    [Fact]
    public void WithCalendar_InvalidCalendar()
    {
        var ordate = CalendarUT.GetOrdinalDate(3, 45);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => ordate.WithCalendar(null!));
    }

    [Fact]
    public void WithCalendar_NotSupported()
    {
        // Julian MinDayNumber is not in the Gregorian range.
        var minDayNumber = s_Julian.Domain.Min;
        var ordate = s_Julian.GetCalendarDateOn(minDayNumber).ToOrdinalDate();
        // Act & Assert
        Assert.ThrowsAoorexn("dayNumber", () => ordate.WithCalendar(CalendarUT));
    }

    [Theory, MemberData(nameof(CalCalDataSet.GregorianJulianData), MemberType = typeof(CalCalDataSet))]
    public void WithCalendar_GregorianToJulian(Yemoda gregorian, Yemoda julian)
    {
        var source = CalendarUT.GetCalendarDate(gregorian.Year, gregorian.Month, gregorian.Day).ToOrdinalDate();
        var result = s_Julian.GetCalendarDate(julian.Year, julian.Month, julian.Day).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(result, source.WithCalendar(s_Julian));
    }

    [Theory, MemberData(nameof(CalCalDataSet.GregorianJulianData), MemberType = typeof(CalCalDataSet))]
    public void WithCalendar_JulianToGregorian(Yemoda gregorian, Yemoda julian)
    {
        var source = s_Julian.GetCalendarDate(julian.Year, julian.Month, julian.Day).ToOrdinalDate();
        var result = CalendarUT.GetCalendarDate(gregorian.Year, gregorian.Month, gregorian.Day).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(result, source.WithCalendar(CalendarUT));
    }

    #endregion
}

public partial class OrdinalDateTests
{
    [Fact]
    public void CompareTo_WithOtherCalendar()
    {
        var date = CalendarUT.GetOrdinalDate(3, 45);
        var other = s_Julian.GetOrdinalDate(3, 45);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CompareTo(other));
    }
}

#if false

public partial class OrdinalDateTests // IEquatable
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void Equality(int y, int _2, int _3, int doy, bool _5, bool _6)
    {
        var ordate = CalendarUT.GetOrdinalDate(y, doy);
        var same = CalendarUT.GetOrdinalDate(y, doy);
        var notSame = CalendarUT.GetOrdinalDate(y, doy == 1 ? 2 : 1);

        // Act & Assert
        Assert.True(ordate == same);
        Assert.True(same == ordate);
        Assert.False(ordate == notSame);
        Assert.False(notSame == ordate);

        Assert.False(ordate != same);
        Assert.False(same != ordate);
        Assert.True(ordate != notSame);
        Assert.True(notSame != ordate);

        Assert.True(ordate.Equals(ordate));
        Assert.True(ordate.Equals(same));
        Assert.True(same.Equals(ordate));
        Assert.False(ordate.Equals(notSame));
        Assert.False(notSame.Equals(ordate));

        Assert.True(ordate.Equals((object)same));
        Assert.False(ordate.Equals((object)notSame));

        Assert.False(ordate.Equals(new object()));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetHashCode_SanityChecks(int y, int _2, int _3, int doy, bool _5, bool _6)
    {
        var ordate = CalendarUT.GetOrdinalDate(y, doy);
        var same = CalendarUT.GetOrdinalDate(y, doy);
        var notSame = CalendarUT.GetOrdinalDate(y, doy == 1 ? 2 : 1);
        // Act & Assert
        Assert.Equal(ordate.GetHashCode(), ordate.GetHashCode());
        Assert.Equal(ordate.GetHashCode(), same.GetHashCode());
        Assert.NotEqual(ordate.GetHashCode(), notSame.GetHashCode());
    }
}

public partial class OrdinalDateTests // IComparable
{
    [Fact]
    public void CompareTo_WithNull()
    {
        var date = CalendarUT.GetOrdinalDate(3, 45);
        var comparable = (IComparable)date;
        // Act & Assert
        Assert.Equal(1, comparable.CompareTo(null));
    }

    [Fact]
    public void CompareTo_WithInvalidObject()
    {
        var date = CalendarUT.GetOrdinalDate(3, 45);
        var comparable = (IComparable)date;
        object other = new();
        // Act & Assert
        Assert.Throws<ArgumentException>("obj", () => comparable.CompareTo(other));
    }

    [Fact]
    public void CompareTo_WithOtherCalendar()
    {
        var date = CalendarUT.GetOrdinalDate(3, 45);
        var other = s_Julian.GetOrdinalDate(3, 45);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CompareTo(other));
    }

    [Theory, MemberData(nameof(MinMax))]
    public void CompareTo(
        Yemoda xleft, Yemoda xright, bool leftIsMax, bool areEqual)
    {
        var left = xleft.ToOrdinalDate(CalendarUT);
        var right = xright.ToOrdinalDate(CalendarUT);
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

    [Theory, MemberData(nameof(MinMax))]
    public void ComparisonOperators(
        Yemoda xleft, Yemoda xright, bool leftIsMax, bool areEqual)
    {
        var left = xleft.ToOrdinalDate(CalendarUT);
        var right = xright.ToOrdinalDate(CalendarUT);

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

    [Theory, MemberData(nameof(MinMax))]
    public void Min(Yemoda xleft, Yemoda xright, bool leftIsMax, bool _4)
    {
        var left = xleft.ToOrdinalDate(CalendarUT);
        var right = xright.ToOrdinalDate(CalendarUT);
        var min = leftIsMax ? right : left;
        // Act
        var actual = OrdinalDate.Min(left, right);
        // Assert
        Assert.Equal(min, actual);
    }

    [Theory, MemberData(nameof(MinMax))]
    public void Max(Yemoda xleft, Yemoda xright, bool leftIsMax, bool _4)
    {
        var left = xleft.ToOrdinalDate(CalendarUT);
        var right = xright.ToOrdinalDate(CalendarUT);
        var max = leftIsMax ? left : right;
        // Act
        var actual = OrdinalDate.Max(left, right);
        // Assert
        Assert.Equal(max, actual);
    }
}

#endif
