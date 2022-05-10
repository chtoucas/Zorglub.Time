// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using System.Linq;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides facts about <see cref="ICalendar{TDate}"/>.
/// </summary>
public abstract partial class ICalendarTFacts<TDate, TCalendar, TDataSet> :
    ICalendarFacts<TCalendar, TDataSet>
    where TDate : struct, IDate<TDate>
    where TCalendar : ICalendar<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected ICalendarTFacts(TCalendar calendar) : base(calendar) { }

    /// <summary>
    /// Gets an <see cref="ICalendar{TDate}"/> view of the calendar under test.
    /// </summary>
    private ICalendar<TDate> ICalendarTView => CalendarUT;

    protected abstract TDate CreateDate(int y, int m, int d);
    protected abstract TDate CreateDate(int y, int doy);
    protected abstract TDate CreateDate(DayNumber dayNumber);
}

// Factories.
public partial class ICalendarTFacts<TDate, TCalendar, TDataSet>
{
    // Hypothesis: concrete implementations provide a factory method.
    // For instance, with Calendar it is called GetCalendarDate().
    #region CreateDate(y, m, d)

    [Fact]
    public void CreateDate_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CreateDate(y, 1, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void CreateDate_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => CreateDate(y, m, 1));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void CreateDate_InvalidDay(int y, int m, int d) =>
        Assert.ThrowsAoorexn("day", () => CreateDate(y, m, d));

    [Theory, MemberData(nameof(DateInfoData))]
    public void CreateDate_WithSampleDates(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        TDate date = CreateDate(y, m, d);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void CreateDate_WithSampleDayNumbers(DayNumberInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        TDate date = CreateDate(y, m, d);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    #endregion
}

// Conversions.
public partial class ICalendarTFacts<TDate, TCalendar, TDataSet>
{
    // Hypothesis: concrete implementations provide these two converters.
    // For instance, with Calendar they are called GetCalendarDate().
    #region GetTDate(dayNumber)

    [Fact]
    public void GetTDate_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(CreateDate);

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void GetTDate(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        // Act
        TDate date = CreateDate(dayNumber);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    // For instance, Calendar offers GetCalendarDate().
    #endregion
    #region GetTDate(y, doy)

    [Fact]
    public void GetTDate_Ordinal_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CreateDate(y, 1));

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void GetTDate_Ordinal_InvalidDayOfYear(int y, int dayOfYear) =>
        Assert.ThrowsAoorexn("dayOfYear", () => CreateDate(y, dayOfYear));

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetTDate_Ordinal(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        TDate date = CreateDate(y, doy);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
        Assert.Equal(doy, date.DayOfYear);
    }

    #endregion
}

// Dates in a given year or month.
public partial class ICalendarTFacts<TDate, TCalendar, TDataSet>
{
    #region GetDaysInYear(y)

    [Fact]
    public void GetDaysInYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(ICalendarTView.GetDaysInYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetDaysInYear(YearInfo info)
    {
        int y = info.Year;
        // Arrange
        TDate startOfYear = ICalendarTView.GetStartOfYear(y);
        TDate endOfYear = ICalendarTView.GetEndOfYear(y);
        IEnumerable<TDate> exp =
            from i in Enumerable.Range(1, info.DaysInYear)
            select CreateDate(y, i);
        // Act
        IEnumerable<TDate> actual = ICalendarTView.GetDaysInYear(y);
        // Assert
        Assert.Equal(exp, actual);
        Assert.Equal(info.DaysInYear, actual.Count());
        Assert.Equal(startOfYear, actual.First());
        Assert.Equal(endOfYear, actual.Last());
    }

    #endregion
    #region GetDaysInMonth(y, m)

    [Fact]
    public void GetDaysInMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => ICalendarTView.GetDaysInMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void GetDaysInMonth_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => ICalendarTView.GetDaysInMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetDaysInMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Arrange
        TDate startofMonth = ICalendarTView.GetStartOfMonth(y, m);
        TDate endOfMonth = ICalendarTView.GetEndOfMonth(y, m);
        IEnumerable<TDate> exp =
            from i in Enumerable.Range(1, info.DaysInMonth)
            select CreateDate(y, m, i);
        // Act
        IEnumerable<TDate> actual = ICalendarTView.GetDaysInMonth(y, m);
        // Assert
        Assert.Equal(exp, actual);
        Assert.Equal(info.DaysInMonth, actual.Count());
        Assert.Equal(startofMonth, actual.First());
        Assert.Equal(endOfMonth, actual.Last());
    }

    #endregion

    #region GetStartOfYear(y)

    [Fact]
    public void GetStartOfYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(ICalendarTView.GetStartOfYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetStartOfYear(YearInfo info)
    {
        int y = info.Year;
        // Arrange
        TDate startOfYear = CreateDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, ICalendarTView.GetStartOfYear(y));
    }

    #endregion
    #region GetEndOfYear(y)

    [Fact]
    public void GetEndOfYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(ICalendarTView.GetEndOfYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        // Arrange
        TDate endOfYear = CreateDate(y, info.DaysInYear);
        // Act & Assert
        Assert.Equal(endOfYear, ICalendarTView.GetEndOfYear(y));
    }

    #endregion
    #region GetStartOfMonth(y, m)

    [Fact]
    public void GetStartOfMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => ICalendarTView.GetStartOfMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void GetStartOfMonth_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => ICalendarTView.GetStartOfMonth(y, m)!);

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Arrange
        TDate startOfMonth = CreateDate(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, ICalendarTView.GetStartOfMonth(y, m));
    }

    #endregion
    #region GetEndOfMonth(y, m)

    [Fact]
    public void GetEndOfMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => ICalendarTView.GetEndOfMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void GetEndOfMonth_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => ICalendarTView.GetEndOfMonth(y, m)!);

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Arrange
        TDate endOfMonth = CreateDate(y, m, info.DaysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, ICalendarTView.GetEndOfMonth(y, m));
    }

    #endregion

    //
    // ICalendar (explicit interface implementation)
    //

    // TODO(fact): to be removed.
#if false
    #region GetStartOfYear(y)

    [Fact]
    public void GetStartOfYear_ExplInterface_InvalidYear() =>
        TestInvalidYear(((ICalendar)CalendarUT).GetStartOfYear);

    [Theory, MemberData(nameof(StartOfYear))]
    public void GetStartOfYear_ExplInterface(int y, DayNumber startOfYear)
    {
        Assert.Equal(startOfYear, ((ICalendar)CalendarUT).GetStartOfYear(y));
    }

    #endregion
    #region GetEndOfYear(y)

    [Fact]
    public void GetEndOfYear_ExplInterface_InvalidYear() =>
        TestInvalidYear(((ICalendar)CalendarUT).GetEndOfYear);

    [Theory, MemberData(nameof(EndOfYear))]
    public void GetEndOfYear_ExplInterface(int y, int m, int d)
    {
        // Arrange
        ICalendar chr = CalendarUT;
        var endOfYear = chr.GetDayNumber(y, m, d);
        // Act
        var actual = chr.GetEndOfYear(y);
        // Assert
        Assert.Equal(endOfYear, actual);
    }

    #endregion
    #region GetStartOfMonth(y, m)

    [Fact]
    public void GetStartOfMonth_ExplInterface_InvalidYear() =>
        TestInvalidYear(y => ((ICalendar)CalendarUT).GetStartOfMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthOfYear))]
    public void GetStartOfMonth_ExplInterface_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => ((ICalendar)CalendarUT).GetStartOfMonth(y, m));

    [Theory, MemberData(nameof(SampleMonths))]
    public void GetStartOfMonth_ExplInterface(int y, int m, int _3, int _4, bool _5)
    {
        // Arrange
        ICalendar chr = CalendarUT;
        var startOfMonth = chr.GetDayNumber(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, chr.GetStartOfMonth(y, m));
    }

    #endregion
    #region GetEndOfMonth(y, m)

    [Fact]
    public void GetEndOfMonth_ExplInterface_InvalidYear() =>
        TestInvalidYear(y => ((ICalendar)CalendarUT).GetEndOfMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthOfYear))]
    public void GetEndOfMonth_ExplInterface_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => ((ICalendar)CalendarUT).GetEndOfMonth(y, m));

    [Theory, MemberData(nameof(SampleMonths))]
    public void GetEndOfMonth_ExplInterface(int y, int m, int daysInMonth, int _4, bool _5)
    {
        // Arrange
        ICalendar chr = CalendarUT;
        var endOfMonth = chr.GetDayNumber(y, m, daysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, chr.GetEndOfMonth(y, m));
    }

    #endregion
#endif
}
