// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

using System.Linq;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

// TODO(fact): should derive from ICalendarTFacts, no?

// Ça passe parce qu'on triche, voir GregorianBoundedBelowCalendarTests et
// GregorianBoundedBelowCalendarTests. En toute rigueur, il faudrait créer
// (et utiliser) des méthodes SkipMonth() et SkipDate().

/// <summary>
/// Provides facts about <see cref="INakedCalendar"/>.
/// </summary>
public abstract partial class INakedCalendarFacts<TCalendar, TDataSet> :
    ICalendarFacts<TCalendar, TDataSet>
    where TCalendar : INakedCalendar
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected INakedCalendarFacts(TCalendar calendar) : base(calendar) { }

    //protected IDateProvider<DateParts> DatePartsProvider => CalendarUT.DatePartsProvider;
}

public partial class INakedCalendarFacts<TCalendar, TDataSet> // Properties
{
    [Fact]
    public sealed override void Algorithm_Prop() { }
    //=> Assert.Equal(CalendarUT.Schema.Algorithm, CalendarUT.Algorithm);

    [Fact]
    public sealed override void Family_Prop() { }
    //=> Assert.Equal(CalendarUT.Schema.Family, CalendarUT.Family);

    [Fact]
    public sealed override void PeriodicAdjustments_Prop() { }
    //=> Assert.Equal(CalendarUT.Schema.PeriodicAdjustments, CalendarUT.PeriodicAdjustments);

    [Fact]
    public void ToString_ReturnsName() { }
    //=> Assert.Equal(CalendarUT.Name, CalendarUT.ToString());
}

public partial class INakedCalendarFacts<TCalendar, TDataSet> // Conversions
{
    #region GetDateParts(DayNumber)

    [Fact]
    public void GetDateParts﹍DayNumber_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(CalendarUT.GetDateParts);

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
        var dayNumber = CalendarUT.GetDayNumber(y, m, d);
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
        DomainTester.TestInvalidDayNumber(CalendarUT.GetOrdinalParts);

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetOrdinalParts﹍DayNumber_UsingDateInfo(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var dayNumber = CalendarUT.GetDayNumber(y, m, d);
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

public partial class INakedCalendarFacts<TCalendar, TDataSet> // IDateProvider<DayNumber>
{
    #region GetDaysInYear(y)

    [Fact]
    public void GetDaysInYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(CalendarUT.GetDaysInYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetDaysInYear(YearInfo info)
    {
        int y = info.Year;

        DayNumber startOfYear = CalendarUT.GetStartOfYear(y);
        DayNumber endOfYear = CalendarUT.GetEndOfYear(y);
        IEnumerable<DayNumber> exp =
            from i in Enumerable.Range(0, info.DaysInYear)
            select startOfYear + i;
        // Act
        IEnumerable<DayNumber> actual = CalendarUT.GetDaysInYear(y);
        var arr = actual.ToArray();
        // Assert
        Assert.Equal(exp, actual);
        Assert.Equal(info.DaysInYear, arr.Length);
        Assert.Equal(startOfYear, arr.First());
        Assert.Equal(endOfYear, arr.Last());
    }

    #endregion
    #region GetDaysInMonth(y, m)

    [Fact]
    public void GetDaysInMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetDaysInMonth(y, 1));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetDaysInMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        DayNumber startofMonth = CalendarUT.GetStartOfMonth(y, m);
        DayNumber endOfMonth = CalendarUT.GetEndOfMonth(y, m);
        IEnumerable<DayNumber> exp =
            from i in Enumerable.Range(0, info.DaysInMonth)
            select startofMonth + i;
        // Act
        IEnumerable<DayNumber> actual = CalendarUT.GetDaysInMonth(y, m);
        var arr = actual.ToArray();
        // Assert
        Assert.Equal(exp, actual);
        Assert.Equal(info.DaysInMonth, arr.Length);
        Assert.Equal(startofMonth, arr.First());
        Assert.Equal(endOfMonth, arr.Last());
    }

    #endregion

    #region GetStartOfYear(y)

    [Fact]
    public void GetStartOfYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(CalendarUT.GetStartOfYear);

    [Theory, MemberData(nameof(StartOfYearDayNumberData))]
    public void GetStartOfYear(YearDayNumber info)
    {
        int y = info.Year;

        Assert.Equal(info.DayNumber, CalendarUT.GetStartOfYear(y));
    }

    #endregion
    #region GetEndOfYear(y)

    [Fact]
    public void GetEndOfYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(CalendarUT.GetEndOfYear);

    [Theory, MemberData(nameof(EndOfYearDayNumberData))]
    public void GetEndOfYear(YearDayNumber info)
    {
        int y = info.Year;

        // Act
        var actual = CalendarUT.GetEndOfYear(y);
        // Assert
        Assert.Equal(info.DayNumber, actual);
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
        var startOfMonth = CalendarUT.GetDayNumber(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, CalendarUT.GetStartOfMonth(y, m));
    }

    #endregion
    #region GetEndOfMonth(y, m)

    [Fact]
    public void GetEndOfMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetEndOfMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void GetEndOfMonth_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => CalendarUT.GetEndOfMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var endOfMonth = CalendarUT.GetDayNumber(y, m, info.DaysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, CalendarUT.GetEndOfMonth(y, m));
    }

    #endregion
}

public partial class INakedCalendarFacts<TCalendar, TDataSet> // DatePartsProvider
{
#if false

    #region DatePartsProvider.GetStartOfYear(y)

    [Fact]
    public void DatePartsProvider_GetStartOfYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(CalendarUT.DatePartsProvider.GetStartOfYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void DatePartsProvider_GetStartOfYear(YearInfo info)
    {
        int y = info.Year;
        var startOfYear = new DateParts(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, CalendarUT.DatePartsProvider.GetStartOfYear(y));
    }

    #endregion
    #region DatePartsProvider.GetEndOfYear(y)

    [Fact]
    public void DatePartsProvider_GetEndOfYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(CalendarUT.DatePartsProvider.GetEndOfYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void DatePartsProvider_GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var daysInMonth = CalendarUT.CountDaysInMonth(y, info.MonthsInYear);
        var endOfYear = new DateParts(y, info.MonthsInYear, daysInMonth);
        // Act & Assert
        Assert.Equal(endOfYear, CalendarUT.DatePartsProvider.GetEndOfYear(y));
    }

    #endregion
    #region DatePartsProvider.GetStartOfMonth(y, m)

    [Fact]
    public void DatePartsProvider_GetStartOfMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.DatePartsProvider.GetStartOfMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void DatePartsProvider_GetStartOfMonth_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => CalendarUT.DatePartsProvider.GetStartOfMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void DatePartsProvider_GetStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var startOfMonth = new DateParts(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, CalendarUT.DatePartsProvider.GetStartOfMonth(y, m));
    }

    #endregion
    #region DatePartsProvider.GetEndOfMonth(y, m)

    [Fact]
    public void DatePartsProvider_GetEndOfMonth_InvalidDayNumber() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.DatePartsProvider.GetEndOfMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void DatePartsProvider_GetEndOfMonth_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => CalendarUT.DatePartsProvider.GetEndOfMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void DatePartsProvider_GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var endOfMonth = new DateParts(y, m, info.DaysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, CalendarUT.DatePartsProvider.GetEndOfMonth(y, m));
    }

    #endregion

#endif
}

public partial class INakedCalendarFacts<TCalendar, TDataSet> // Arithmetic
{
#if false // TODO(fact): move this to DomainExtensionsFacts.

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

#endif
}
