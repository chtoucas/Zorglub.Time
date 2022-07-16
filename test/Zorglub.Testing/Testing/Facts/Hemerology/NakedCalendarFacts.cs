// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

#if false
using System.Linq;
#endif

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// TODO(fact): should derive from ICalendarTFacts.

// Ça passe parce qu'on triche, voir GregorianBoundedBelowCalendarTests et
// GregorianBoundedBelowCalendarTests. En toute rigueur, il faudrait créer
// (et utiliser) des méthodes SkipMonth() et SkipDate().

/// <summary>
/// Provides facts about <see cref="NakedCalendar"/>.
/// </summary>
public abstract partial class NakedCalendarFacts<TCalendar, TScope, TDataSet> :
    ICalendarFacts<TCalendar, TDataSet>
    where TCalendar : NakedCalendar<TScope>
    where TScope : CalendarScope
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected NakedCalendarFacts(TCalendar calendar) : base(calendar)
    {
#if false
        if (calendar is not ICalendar<DayNumber> dayCalendar)
        {
            throw new ArgumentException(null, nameof(calendar));
        }

        DayCalendarUT = dayCalendar;
#endif
    }

#if false
    protected ICalendar<DayNumber> DayCalendarUT { get; }
#endif
}

public partial class NakedCalendarFacts<TCalendar, TScope, TDataSet> // Properties
{
    [Fact]
    public sealed override void Algorithm_Prop() =>
        Assert.Equal(CalendarUT.Schema.Algorithm, CalendarUT.Algorithm);

    [Fact]
    public sealed override void Family_Prop() =>
        Assert.Equal(CalendarUT.Schema.Family, CalendarUT.Family);

    [Fact]
    public sealed override void PeriodicAdjustments_Prop() =>
        Assert.Equal(CalendarUT.Schema.PeriodicAdjustments, CalendarUT.PeriodicAdjustments);

    [Fact]
    public void ToString_ReturnsName() =>
        Assert.Equal(CalendarUT.Name, CalendarUT.ToString());
}

public partial class NakedCalendarFacts<TCalendar, TScope, TDataSet> // Conversions
{
    #region GetDateParts(DayNumber)

    [Fact]
    public void GetDateParts﹍DayNumber_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(x => CalendarUT.GetDateParts(x));

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void GetDateParts﹍DayNumber(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        if (Domain.Contains(dayNumber) == false) { return; }
        var exp = new DateParts(y, m, d);
        // Act
        var actual = CalendarUT.GetDateParts(dayNumber);
        // Assert
        Assert.Equal(exp, actual);
    }

    [RedundantTest]
    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDateParts﹍DayNumber_ViaDateInfo(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var dayNumber = CalendarUT.GetDayNumberOn(y, m, d);
        var exp = new DateParts(y, m, d);
        // Act
        var actual = CalendarUT.GetDateParts(dayNumber);
        // Assert
        Assert.Equal(exp, actual);
    }

    #endregion
    #region GetDateParts(y, doy)

    [Fact]
    public void GetDateParts_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetDateParts(y, 1));

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void GetDateParts_InvalidDayOfYear(int y, int doy) =>
        Assert.ThrowsAoorexn("dayOfYear", () => CalendarUT.GetDateParts(y, doy));

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDateParts(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var exp = new DateParts(y, m, d);
        // Act
        var actual = CalendarUT.GetDateParts(y, doy);
        // Assert
        Assert.Equal(exp, actual);
    }

    #endregion
    #region GetOrdinalParts(DayNumber)

    [Fact]
    public void GetOrdinalParts﹍DayNumber_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(x => CalendarUT.GetOrdinalParts(x));

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetOrdinalParts﹍DayNumber_UsingDateInfo(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var dayNumber = CalendarUT.GetDayNumberOn(y, m, d);
        var exp = new OrdinalParts(y, doy);
        // Act
        var actual = CalendarUT.GetOrdinalParts(dayNumber);
        // Assert
        Assert.Equal(exp, actual);
    }

    #endregion
    #region GetOrdinalParts(y, m, d)

    [Fact]
    public void GetOrdinalParts_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetOrdinalParts(y, 1, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void GetOrdinalParts_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => CalendarUT.GetOrdinalParts(y, m, 1));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void GetOrdinalParts_InvalidDay(int y, int m, int d) =>
        Assert.ThrowsAoorexn("day", () => CalendarUT.GetOrdinalParts(y, m, d));

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetOrdinalParts(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var exp = new OrdinalParts(y, doy);
        // Act
        var actual = CalendarUT.GetOrdinalParts(y, m, d);
        // Assert
        Assert.Equal(exp, actual);
    }

    #endregion
}

// Dates in a given year or month.
public partial class NakedCalendarFacts<TCalendar, TScope, TDataSet> // IDayProvider
{
    //
    // ICalendar<DayNumber>.
    //
#if false
    #region GetDaysInYear(y)

    [Fact]
    public void GetDaysInYear_DayNumber_InvalidYear() =>
        TestInvalidYear(DayCalendarUT.GetDaysInYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetDaysInYear_DayNumber(YearInfo info)
    {
        int y = info.Year;

        DayNumber startOfYear = DayCalendarUT.GetStartOfYear(y);
        DayNumber endOfYear = DayCalendarUT.GetEndOfYear(y);
        IEnumerable<DayNumber> exp =
            from i in Enumerable.Range(0, info.DaysInYear)
            select startOfYear + i;
        // Act
        IEnumerable<DayNumber> actual = DayCalendarUT.GetDaysInYear(y);
        // Assert
        Assert.Equal(exp, actual);
        Assert.Equal(info.DaysInYear, actual.Count());
        Assert.Equal(startOfYear, actual.First());
        Assert.Equal(endOfYear, actual.Last());
    }

    #endregion
    #region GetDaysInMonth(y, m)

    [Fact]
    public void GetDaysInMonth_DayNumber_InvalidYear() =>
        TestInvalidYear(y => DayCalendarUT.GetDaysInMonth(y, 1));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetDaysInMonth_DayNumber(int y, int m, int daysInMonth, int _4, bool _5)
    {
        DayNumber startofMonth = DayCalendarUT.GetStartOfMonth(y, m);
        DayNumber endOfMonth = DayCalendarUT.GetEndOfMonth(y, m);
        IEnumerable<DayNumber> exp =
            from i in Enumerable.Range(0, daysInMonth)
            select startofMonth + i;
        // Act
        IEnumerable<DayNumber> actual = DayCalendarUT.GetDaysInMonth(y, m);
        // Assert
        Assert.Equal(exp, actual);
        Assert.Equal(daysInMonth, actual.Count());
        Assert.Equal(startofMonth, actual.First());
        Assert.Equal(endOfMonth, actual.Last());
    }

    #endregion

    #region GetStartOfYear(y)

    [Fact]
    public void GetStartOfYear_DayNumber_InvalidYear() =>
        TestInvalidYear(DayCalendarUT.GetStartOfYear);

    [Theory, MemberData(nameof(StartOfYearDayNumberData))]
    public void GetStartOfYear_DayNumber(YearDayNumber info)
    {
        int y = info.Year;

        Assert.Equal(info.DayNumber, DayCalendarUT.GetStartOfYear(y));
    }

    #endregion
    #region GetEndOfYear(y)

    [Fact]
    public void GetEndOfYear_DayNumber_InvalidYear() =>
        TestInvalidYear(DayCalendarUT.GetEndOfYear);

    [Theory, MemberData(nameof(EndOfYearData))]
    public void GetEndOfYear_DayNumber(Yemoda xdate)
    {
        var (y, m, d) = xdate;
        var endOfYear = DayCalendarUT.GetDayNumberOn(y, m, d);
        // Act
        var actual = DayCalendarUT.GetEndOfYear(y);
        // Assert
        Assert.Equal(endOfYear, actual);
    }

    #endregion
    #region GetStartOfMonth(y, m)

    [Fact]
    public void GetStartOfMonth_DayNumber_InvalidYear() =>
        TestInvalidYear(y => DayCalendarUT.GetStartOfMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void GetStartOfMonth_DayNumber_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => DayCalendarUT.GetStartOfMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetStartOfMonth_DayNumber(int y, int m, int _3, int _4, bool _5)
    {
        var startOfMonth = DayCalendarUT.GetDayNumberOn(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, DayCalendarUT.GetStartOfMonth(y, m));
    }

    #endregion
    #region GetEndOfMonth(y, m)

    [Fact]
    public void GetEndOfMonth_DayNumber_InvalidYear() =>
        TestInvalidYear(y => DayCalendarUT.GetEndOfMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void GetEndOfMonth_DayNumber_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => DayCalendarUT.GetEndOfMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth_DayNumber(int y, int m, int daysInMonth, int _4, bool _5)
    {
        var endOfMonth = DayCalendarUT.GetDayNumberOn(y, m, daysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, DayCalendarUT.GetEndOfMonth(y, m));
    }

    #endregion
#endif

    //
    // ICalendar<DateParts>.
    //

    #region GetStartOfYear(y)

    [Fact]
    public void GetStartOfYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(CalendarUT.GetStartOfYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetStartOfYear(YearInfo info)
    {
        int y = info.Year;
        var startOfYear = new DateParts(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, CalendarUT.GetStartOfYear(y));
    }

    #endregion
    #region GetEndOfYear(y)

    [Fact]
    public void GetEndOfYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(CalendarUT.GetEndOfYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var daysInMonth = CalendarUT.CountDaysInMonth(y, info.MonthsInYear);
        var endOfYear = new DateParts(y, info.MonthsInYear, daysInMonth);
        // Act & Assert
        Assert.Equal(endOfYear, CalendarUT.GetEndOfYear(y));
    }

    #endregion
    #region GetStartOfMonth(y, m)

    [Fact]
    public void GetStartOfMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetStartOfMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void GetStartOfMonth_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => CalendarUT.GetStartOfMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var startOfMonth = new DateParts(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, CalendarUT.GetStartOfMonth(y, m));
    }

    #endregion
    #region GetEndOfMonth(y, m)

    [Fact]
    public void GetEndOfMonth_InvalidDayNumber() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetEndOfMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void GetEndOfMonth_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => CalendarUT.GetEndOfMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var endOfMonth = new DateParts(y, m, info.DaysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, CalendarUT.GetEndOfMonth(y, m));
    }

    #endregion
}

// Arithmetic.
public partial class NakedCalendarFacts<TCalendar, TScope, TDataSet>
{
    #region AddDays(dayNumber, days)

    [Fact]
    public void AddDays_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(x => CalendarUT.AddDays(x, 1));

    [Fact]
    public void AddDays_IntegerOverflow() =>
        Assert.Overflows(() => CalendarUT.AddDays(DayZero.NewStyle + 345, Int32.MaxValue));

    [Fact]
    public void AddDays_MaxDays_ToMinValue()
    {
        var (min, max) = CalendarUT.Domain.Endpoints;
        int days = max - min;

        // Act & Assert
        // (min - 1) overflows
        Assert.Overflows(() => CalendarUT.AddDays(min, -1));
        // min + 0 = min
        Assert.Equal(min, CalendarUT.AddDays(min, 0));
        // min + (max - min) = max
        Assert.Equal(max, CalendarUT.AddDays(min, days));
        // min + (max - min + 1) overflows
        Assert.Overflows(() => CalendarUT.AddDays(min, days + 1));
    }

    [Fact]
    public void AddDays_MaxDays_ToMaxValue()
    {
        var (min, max) = CalendarUT.Domain.Endpoints;
        int days = max - min;

        // Act & Assert
        // max + (min - max - 1) overflows
        Assert.Overflows(() => CalendarUT.AddDays(max, -days - 1));
        // max + (min - max) = min
        Assert.Equal(min, CalendarUT.AddDays(max, -days));
        // max + 0 = max
        Assert.Equal(max, CalendarUT.AddDays(max, 0));
        // max + 1 overflows
        Assert.Overflows(() => CalendarUT.AddDays(max, 1));
    }

    [Fact]
    public void AddDays_MaxDays()
    {
        var dayNumber = DayZero.NewStyle + 345;
        var (min, max) = CalendarUT.Domain.Endpoints;
        int minDays = min - dayNumber;
        int maxDays = max - dayNumber;

        // Act & Assert
        // D + (min - D - 1) overflows
        Assert.Overflows(() => CalendarUT.AddDays(dayNumber, minDays - 1));
        // D + (min - D) = min
        Assert.Equal(min, CalendarUT.AddDays(dayNumber, minDays));
        // D + (max - D) = max
        Assert.Equal(max, CalendarUT.AddDays(dayNumber, maxDays));
        // D + (max - D + 1) overflows
        Assert.Overflows(() => CalendarUT.AddDays(dayNumber, maxDays + 1));
    }

    [Fact]
    public void AddDays()
    {
        var dayNumber = DayZero.NewStyle + 345;
        var result = DayZero.NewStyle + 435;
        // Act & Assert
        Assert.Equal(result, CalendarUT.AddDays(dayNumber, 435 - 345));
    }

    #endregion
}
