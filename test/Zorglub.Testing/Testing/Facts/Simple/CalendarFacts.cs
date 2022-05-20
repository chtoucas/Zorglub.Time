// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;
using Zorglub.Time.Simple;

/// <summary>
/// Provides facts about <see cref="Calendar"/>.
/// </summary>
public abstract partial class CalendarFacts<TCalendar, TDataSet> :
    ICalendarFacts<TCalendar, TDataSet>
    where TCalendar : Calendar
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarFacts(TCalendar calendar) : base(calendar) { }

    protected abstract TCalendar GetSingleton();

    [Fact] public abstract void Id();
    [Fact] public abstract void Math();
    [Fact] public abstract void Scope();
}

public partial class CalendarFacts<TCalendar, TDataSet> // Tests for CalendarCatalog
{
    // Referential equality is guaranteed, all calendars are singleton
    // (see comments in CalendarCatalog).

    [Fact]
    public void CalendarCatalog_GetSystemCalendar()
    {
        if (CalendarUT.IsUserDefined)
        {
            Assert.ThrowsAoorexn("id", () => CalendarCatalog.GetSystemCalendar((CalendarId)CalendarUT.Id));
        }
        else
        {
            Assert.Same(CalendarUT, CalendarCatalog.GetSystemCalendar(CalendarUT.PermanentId));
        }
    }

    [Fact]
    public void CalendarCatalog_GetCalendarUnchecked() =>
        Assert.Same(CalendarUT, CalendarCatalog.GetCalendarUnchecked((int)CalendarUT.Id));
}

public partial class CalendarFacts<TCalendar, TDataSet> // Properties
{
    [Fact]
    public void Singleton()
    {
        Assert.Same(CalendarUT, GetSingleton());
        Assert.Same(GetSingleton(), GetSingleton());
    }

    [Fact]
    public sealed override void Algorithm_Prop() =>
        Assert.Equal(CalendricalAlgorithm.Arithmetical, CalendarUT.Algorithm);

    [Fact]
    public sealed override void Family_Prop() =>
        Assert.Equal(CalendarUT.Schema.Family, CalendarUT.Family);

    [Fact]
    public sealed override void PeriodicAdjustments_Prop() =>
        Assert.Equal(CalendarUT.Schema.PeriodicAdjustments, CalendarUT.PeriodicAdjustments);

    [Fact]
    public sealed override void SupportedYears_Prop()
    {
        int minYear = CalendarUT.IsProleptic ? ProlepticShortScope.MinYear : StandardShortScope.MinYear;
        // Act
        var supportedYears = CalendarUT.SupportedYears;
        // Assert
        Assert.Equal(CalendarUT.Scope.SupportedYears, supportedYears);
        Assert.Equal(minYear, supportedYears.Min);
        Assert.Equal(ShortScope.MaxYear, supportedYears.Max);
    }

    [Fact]
    public void ToString_ReturnsKey()
    {
        Assert.Equal(CalendarUT.Key, CalendarUT.ToString());
        if (!CalendarUT.IsUserDefined)
        {
            Assert.Equal(CalendarUT.PermanentId.ToCalendarKey(), CalendarUT.ToString());
        }
    }
}

public partial class CalendarFacts<TCalendar, TDataSet> // Factories
{
    #region GetCalendarYear()

    [Fact]
    public void GetCalendarYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(CalendarUT.GetCalendarYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetCalendarYear(YearInfo info)
    {
        int y = info.Year;
        // Act
        var cyear = CalendarUT.GetCalendarYear(y);
        // Assert
        Assert.Equal(y, cyear.Year);
        Assert.Equal(CalendarUT.Id, cyear.Cuid);
    }

    #endregion
    #region GetCalendarMonth()

    [Fact]
    public void GetCalendarMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetCalendarMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void GetCalendarMonth_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => CalendarUT.GetCalendarMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetCalendarMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        var cmonth = CalendarUT.GetCalendarMonth(y, m);
        // Assert
        Assert.Equal(y, cmonth.Year);
        Assert.Equal(m, cmonth.MonthOfYear);
        Assert.Equal(CalendarUT.Id, cmonth.Cuid);
    }

    #endregion
    #region GetCalendarDate()

    [Fact]
    public void GetCalendarDate_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetCalendarDate(y, 1, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void GetCalendarDate_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => CalendarUT.GetCalendarDate(y, m, 1));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void GetCalendarDate_InvalidDay(int y, int m, int d) =>
        Assert.ThrowsAoorexn("day", () => CalendarUT.GetCalendarDate(y, m, d));

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetCalendarDate(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    #endregion
    #region GetOrdinalDate()

    [Fact]
    public void GetOrdinalDate_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetOrdinalDate(y, 1));

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void GetOrdinalDate_InvalidMonth(int y, int doy) =>
        Assert.ThrowsAoorexn("dayOfYear", () => CalendarUT.GetOrdinalDate(y, doy));

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetOrdinalDate(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        // Act
        var ordate = CalendarUT.GetOrdinalDate(y, doy);
        // Assert
        Assert.Equal(y, ordate.Year);
        Assert.Equal(doy, ordate.DayOfYear);
        Assert.Equal(CalendarUT.Id, ordate.Cuid);
    }

    #endregion
    #region GetCalendarDay()

    [Fact]
    public void GetCalendarDay_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(CalendarUT.GetCalendarDay);

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void GetCalendarDay(DayNumberInfo info)
    {
        var dayNumber = info.DayNumber;
        // Act
        var day = CalendarUT.GetCalendarDay(info.DayNumber);
        // Assert
        Assert.Equal(dayNumber, day.DayNumber);
    }

    #endregion
}

public partial class CalendarFacts<TCalendar, TDataSet> // Conversions
{
    #region GetCalendarYearOn()

    [Fact]
    public void GetCalendarYearOn_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(CalendarUT.GetCalendarYearOn);

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void GetCalendarYearOn(DayNumberInfo info)
    {
        var exp = CalendarUT.GetCalendarYear(info.Yemoda.Year);
        // Act
        var cyear = CalendarUT.GetCalendarYearOn(info.DayNumber);
        // Assert
        Assert.Equal(exp, cyear);
    }

    #endregion
    #region GetCalendarMonthOn()

    [Fact]
    public void GetCalendarMonthOn_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(CalendarUT.GetCalendarMonthOn);

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void GetCalendarMonthOn(DayNumberInfo info)
    {
        var (dayNumber, y, m, _) = info;
        var exp = CalendarUT.GetCalendarMonth(y, m);
        // Act
        var cmonth = CalendarUT.GetCalendarMonthOn(dayNumber);
        // Assert
        Assert.Equal(exp, cmonth);
    }

    #endregion
    #region GetCalendarDateOn()

    [Fact]
    public void GetCalendarDateOn_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(CalendarUT.GetCalendarDateOn);

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetCalendarDateOn(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var dayNumber = ((ICalendar)CalendarUT).GetDayNumberOn(y, m, d);
        var exp = CalendarUT.GetCalendarDate(y, m, d);
        // Act
        var date = CalendarUT.GetCalendarDateOn(dayNumber);
        // Assert
        Assert.Equal(exp, date);
    }

    #endregion
    #region GetOrdinalDateOn()

    [Fact]
    public void GetOrdinalDateOn_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(CalendarUT.GetOrdinalDateOn);

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetOrdinalDateOn(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var dayNumber = ((ICalendar)CalendarUT).GetDayNumberOn(y, m, d);
        var exp = CalendarUT.GetOrdinalDate(y, doy);
        // Act
        var ordate = CalendarUT.GetOrdinalDateOn(dayNumber);
        // Assert
        Assert.Equal(exp, ordate);
    }

    #endregion
}

public partial class CalendarFacts<TCalendar, TDataSet> // Internal helpers
{
    #region ValidateDayOfMonth()

    [Theory]
    [InlineData(Int32.MinValue)]
    [InlineData(ProlepticShortScope.MinYear - 1)]
    [InlineData(ShortScope.MaxYear + 1)]
    [InlineData(Int32.MaxValue)]
    public void ValidateDayOfMonth_IgnoresYear(int y)
    {
        try
        {
            CalendarUT.ValidateDayOfMonth(y, 1, 1);
        }
        catch (OverflowException) { }
    }

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void ValidateDayOfMonth_IgnoresMonth(int y, int m) =>
        CalendarUT.ValidateDayOfMonth(y, m, 1);

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void ValidateDayOfMonth_InvalidDay(int y, int m, int d)
    {
        Assert.ThrowsAoorexn("dayOfMonth", () => CalendarUT.ValidateDayOfMonth(y, m, d));
        Assert.ThrowsAoorexn("d", () => CalendarUT.ValidateDayOfMonth(y, m, d, nameof(d)));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void ValidateDayOfMonth(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        CalendarUT.ValidateDayOfMonth(y, m, d);
    }

    #endregion
    #region GetDayOfWeek

    [Theory, MemberData(nameof(CalCalDataSet.DayNumberToDayOfWeekData), MemberType = typeof(CalCalDataSet))]
    public void GetDayOfWeek(DayNumber dayNumber, DayOfWeek dayOfWeek)
    {
        if (Domain.Contains(dayNumber) == false) { return; }
        var date = CalendarUT.GetCalendarDateOn(dayNumber);
        // Act & Assert
        Assert.Equal(dayOfWeek, CalendarUT.GetDayOfWeek(date));
    }

    [Theory, MemberData(nameof(CalCalDataSet.DayNumberToDayOfWeekData), MemberType = typeof(CalCalDataSet))]
    public void GetDayOfWeek﹍Ordinal(DayNumber dayNumber, DayOfWeek dayOfWeek)
    {
        if (Domain.Contains(dayNumber) == false) { return; }
        var date = CalendarUT.GetOrdinalDateOn(dayNumber);
        // Act & Assert
        Assert.Equal(dayOfWeek, CalendarUT.GetDayOfWeek(date));
    }

    #endregion
}
