// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Testing.Facts.Hemerology;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;
using Zorglub.Time.Simple;

/// <summary>
/// Provides facts about <see cref="SimpleCalendar"/>.
/// </summary>
public abstract partial class SimpleCalendarFacts<TDataSet> :
    ICalendarFacts<SimpleCalendar, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected SimpleCalendarFacts(SimpleCalendar calendar) : base(calendar) { }

    protected abstract SimpleCalendar GetSingleton();

    [Fact] public abstract void Id();
    [Fact] public abstract void Math();
    [Fact] public abstract void Scope();
}

public partial class SimpleCalendarFacts<TDataSet> // Properties
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
    public void YearsValidatorImpl_Prop()
    {
        if (CalendarUT.IsProleptic)
        {
            Assert.Equal(ProlepticScope.YearsValidatorImpl, CalendarUT.YearsValidator);
        }
        else
        {
            Assert.Equal(StandardScope.YearsValidatorImpl, CalendarUT.YearsValidator);
        }
    }

    [Fact]
    public void ToString_ReturnsKey()
    {
        Assert.Equal(CalendarUT.Key, CalendarUT.ToString());
        if (CalendarUT.IsUserDefined == false)
        {
            Assert.Equal(CalendarUT.PermanentId.ToCalendarKey(), CalendarUT.ToString());
        }
    }
}

public partial class SimpleCalendarFacts<TDataSet> // Factories
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
        var year = CalendarUT.GetCalendarYear(y);
        // Assert
        Assert.Equal(y, year.Year);
        Assert.Equal(CalendarUT.Id, year.Cuid);
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
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Assert
        Assert.Equal(y, month.Year);
        Assert.Equal(m, month.Month);
        Assert.Equal(CalendarUT.Id, month.Cuid);
    }

    #endregion
    #region GetCalendarDate()

    [Fact]
    public void GetCalendarDate_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetDate(y, 1, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void GetCalendarDate_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => CalendarUT.GetDate(y, m, 1));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void GetCalendarDate_InvalidDay(int y, int m, int d) =>
        Assert.ThrowsAoorexn("day", () => CalendarUT.GetDate(y, m, d));

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetCalendarDate(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        var date = CalendarUT.GetDate(y, m, d);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    #endregion
    #region GetOrdinalDate()

    [Fact]
    public void GetOrdinalDate_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetDate(y, 1));

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void GetOrdinalDate_InvalidMonth(int y, int doy) =>
        Assert.ThrowsAoorexn("dayOfYear", () => CalendarUT.GetDate(y, doy));

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetOrdinalDate(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        // Act
        var ordate = CalendarUT.GetDate(y, doy);
        // Assert
        Assert.Equal(y, ordate.Year);
        Assert.Equal(doy, ordate.DayOfYear);
        Assert.Equal(CalendarUT.Id, ordate.Cuid);
    }

    #endregion
    #region GetCalendarDay()

    [Fact]
    public void GetCalendarDay_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(CalendarUT.GetDate);

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void GetCalendarDay(DayNumberInfo info)
    {
        var dayNumber = info.DayNumber;
        // Act
        var day = CalendarUT.GetDate(info.DayNumber);
        // Assert
        Assert.Equal(dayNumber, day.DayNumber);
    }

    #endregion
}

public partial class SimpleCalendarFacts<TDataSet> // Conversions
{
#if false
    #region GetCalendarYear()

    [Fact]
    public void GetCalendarYear﹍DayNumber_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(CalendarUT.GetCalendarYear);

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void GetCalendarYear﹍DayNumber(DayNumberInfo info)
    {
        var exp = CalendarUT.GetCalendarYear(info.Yemoda.Year);
        // Act
        var year = CalendarUT.GetCalendarYear(info.DayNumber);
        // Assert
        Assert.Equal(exp, year);
    }

    #endregion
    #region GetCalendarMonth()

    [Fact]
    public void GetCalendarMonthOn﹍DayNumber_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(CalendarUT.GetCalendarMonth);

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void GetCalendarMonth﹍DayNumber(DayNumberInfo info)
    {
        var (dayNumber, y, m, _) = info;
        var exp = CalendarUT.GetCalendarMonth(y, m);
        // Act
        var month = CalendarUT.GetCalendarMonth(dayNumber);
        // Assert
        Assert.Equal(exp, month);
    }

    #endregion
    #region GetCalendarDate()

    [Fact]
    public void GetCalendarDate﹍DayNumber_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(CalendarUT.GetCalendarDate);

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetCalendarDate﹍DayNumber(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var dayNumber = ((ICalendar)CalendarUT).GetDayNumber(y, m, d);
        var exp = CalendarUT.GetDate(y, m, d);
        // Act
        var date = CalendarUT.GetCalendarDate(dayNumber);
        // Assert
        Assert.Equal(exp, date);
    }

    #endregion
    #region GetOrdinalDate()

    [Fact]
    public void GetOrdinalDate﹍DayNumber_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(CalendarUT.GetOrdinalDate);

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetOrdinalDate﹍DayNumber(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var dayNumber = ((ICalendar)CalendarUT).GetDayNumber(y, m, d);
        var exp = CalendarUT.GetDate(y, doy);
        // Act
        var ordate = CalendarUT.GetOrdinalDate(dayNumber);
        // Assert
        Assert.Equal(exp, ordate);
    }

    #endregion
#endif
}

public partial class SimpleCalendarFacts<TDataSet> // Internal helpers
{
    #region ValidateDayOfMonth()

    [Theory]
    [InlineData(Int32.MinValue)]
    [InlineData(ProlepticScope.MinYear - 1)]
    [InlineData(ProlepticScope.MaxYear + 1)]
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

    // TODO(fact): filter data.

    //[Theory, MemberData(nameof(CalCalDataSet.DayNumberToDayOfWeekData), MemberType = typeof(CalCalDataSet))]
    //public void GetDayOfWeek(DayNumber dayNumber, DayOfWeek dayOfWeek)
    //{
    //    if (Domain.Contains(dayNumber) == false) { return; }
    //    var date = CalendarUT.GetCalendarDate(dayNumber);
    //    // Act & Assert
    //    Assert.Equal(dayOfWeek, CalendarUT.GetDayOfWeek(date));
    //}

    //[Theory, MemberData(nameof(CalCalDataSet.DayNumberToDayOfWeekData), MemberType = typeof(CalCalDataSet))]
    //public void GetDayOfWeek﹍Ordinal(DayNumber dayNumber, DayOfWeek dayOfWeek)
    //{
    //    if (Domain.Contains(dayNumber) == false) { return; }
    //    var date = CalendarUT.GetOrdinalDate(dayNumber);
    //    // Act & Assert
    //    Assert.Equal(dayOfWeek, CalendarUT.GetDayOfWeek(date));
    //}

    #endregion
}
