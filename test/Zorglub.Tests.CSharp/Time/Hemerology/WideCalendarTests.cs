// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using Zorglub.Testing.Data.Unbounded;
using Zorglub.Time.Simple;

public sealed partial class WideCalendarTests :
    ICalendarTFacts<WideDate, WideCalendar, UnboundedGregorianDataSet>
{
    public WideCalendarTests() : this(WideCalendar.Gregorian) { }

    private WideCalendarTests(WideCalendar calendar) : base(calendar) { }

    protected override WideDate GetDate(int y, int m, int d) => CalendarUT.GetWideDate(y, m, d);
    protected override WideDate GetDate(int y, int doy) => CalendarUT.GetWideDateOn(y, doy);
    protected override WideDate GetDate(DayNumber dayNumber) => CalendarUT.GetWideDateOn(dayNumber);
}

public partial class WideCalendarTests // Properties
{
    [Fact]
    public void Key() =>
        Assert.Equal(GregorianCalendar.Instance.Key, CalendarUT.Key);

    [Fact]
    public sealed override void Algorithm_Prop() =>
        Assert.Equal(CalendricalAlgorithm.Arithmetical, CalendarUT.Algorithm);

    [Fact]
    public sealed override void Family_Prop() =>
        Assert.Equal(CalendricalFamily.Solar, CalendarUT.Family);

    [Fact]
    public sealed override void PeriodicAdjustments_Prop() =>
        Assert.Equal(CalendricalAdjustments.Days, CalendarUT.PeriodicAdjustments);

    [Fact]
    public sealed override void SupportedYears_Prop()
    {
        Assert.Equal(CalendarUT.Schema.SupportedYears, CalendarUT.SupportedYears);
        Assert.Equal(CalendarUT.Scope.SupportedYears, CalendarUT.SupportedYears);
    }

    [Fact]
    public void ToString_ReturnsKey() =>
        Assert.Equal(CalendarUT.Key, CalendarUT.ToString());

    [Fact]
    public void Today()
    {
        var exp = DateTime.Now;
        // Act
        var today = CalendarUT.Today();
        // Assert
        Assert.Equal(exp.Year, today.Year);
        Assert.Equal(exp.Month, today.Month);
        Assert.Equal(exp.Day, today.Day);
    }
}

public partial class WideCalendarTests // Validation
{
    [Fact]
    public void ValidateYearMonthDay_InvalidYear()
    {
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.Scope.ValidateYearMonthDay(y, 1, 1));
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.Scope.ValidateYearMonthDay(y, 1, 1, "paramName"), "paramName");
    }

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void ValidateYearMonthDay_InvalidMonthOfYear(int y, int m)
    {
        Assert.ThrowsAoorexn("month",
            () => CalendarUT.Scope.ValidateYearMonthDay(y, m, 1));
        Assert.ThrowsAoorexn("paramName",
            () => CalendarUT.Scope.ValidateYearMonthDay(y, m, 1, "paramName"));
    }

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void ValidateYearMonthDay_InvalidDayOfMonth(int y, int m, int d)
    {
        Assert.ThrowsAoorexn("day",
            () => CalendarUT.Scope.ValidateYearMonthDay(y, m, d));
        Assert.ThrowsAoorexn("paramName",
            () => CalendarUT.Scope.ValidateYearMonthDay(y, m, d, "paramName"));
    }

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void ValidateMonthDay_InvalidMonthOfYear(int y, int m)
    {
        Assert.ThrowsAoorexn("paramName",
            () => CalendarUT.PreValidator.ValidateMonthDay(y, m, 1, "paramName"));
    }

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void ValidateMonthDay_InvalidDayOfMonth(int y, int m, int d)
    {
        Assert.ThrowsAoorexn("paramName",
            () => CalendarUT.PreValidator.ValidateMonthDay(y, m, d, "paramName"));
    }
}

public partial class WideCalendarTests // Conversions
{
    [Fact]
    public void CreationFromOrdinalDate_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetWideDateOn(y, 1));

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void CreationFromOrdinalDate_InvalidDayOfYear(int y, int doy) =>
        Assert.ThrowsAoorexn("dayOfYear", () => CalendarUT.GetWideDateOn(y, doy));

    [Theory, MemberData(nameof(DateInfoData))]
    public void CreationFromOrdinalDate(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        var date = CalendarUT.GetWideDateOn(y, doy);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
        Assert.Equal(doy, date.DayOfYear);
    }
}
