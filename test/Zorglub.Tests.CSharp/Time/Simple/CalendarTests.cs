// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Bounded;
using Zorglub.Time.Core;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

public static class CalendarTests
{
    // NB: We use the Armenian calendar to not disturb the other (calculator)
    // tests. Remember that this change is global.

    [Fact]
    public static void Math_NullValue() =>
        Assert.ThrowsAnexn("value", () => ArmenianCalendar.Instance.Math = null!);

    [Fact]
    public static void Math_InvalidValue() =>
        Assert.Throws<ArgumentException>("value",
            () =>
                ArmenianCalendar.Instance.Math =
                    new FauxCalendarMath(ZoroastrianCalendar.Instance));

    [Fact]
    public static void Math()
    {
        var chr = ArmenianCalendar.Instance;
        var ops = new FauxCalendarMath(chr);

        // Act & Assert
        Assert.IsType<Regular12Math>(chr.Math);

        chr.Math = ops;

        Assert.Equal(ops, chr.Math);
        // Just to be clear about the type, even if it is obvious.
        Assert.IsType<FauxCalendarMath>(chr.Math);
    }

    [Theory, MemberData(nameof(EnumDataSet.CalendarIdData), MemberType = typeof(EnumDataSet))]
    public static void Id_Sys(CalendarId id)
    {
        var chr = new FauxSystemCalendar(id);
        // Act & Assert
        Assert.Equal((Cuid)id, chr.Id);
    }

    [Fact]
    public static void ValidateCuid_Sys()
    {
        var chr = new FauxSystemCalendar();
        string paramName = "cuidParam";
        // Act & Assert
        chr.ValidateCuidDisclosed((Cuid)FauxSystemCalendar.FauxIdent, paramName);

        Assert.Throws<ArgumentException>(paramName,
            () => chr.ValidateCuidDisclosed(Cuid.Gregorian, paramName));
        Assert.Throws<ArgumentException>(paramName,
            () => chr.ValidateCuidDisclosed(Cuid.MinUser, paramName));
    }

    [Fact]
    public static void ValidateCuid_Usr()
    {
        var chr = new FauxUserCalendar();
        string paramName = "cuidParam";
        // Act & Assert
        chr.ValidateCuidDisclosed(FauxUserCalendar.FauxId, paramName);

        Assert.Throws<ArgumentException>(paramName,
            () => chr.ValidateCuidDisclosed(Cuid.Gregorian, paramName));
        Assert.Throws<ArgumentException>(paramName,
            () => chr.ValidateCuidDisclosed(Cuid.MinUser, paramName));
    }
}

public sealed class JulianCalendarTests
{
    public static readonly JulianCalendar CalendarUT = JulianCalendar.Instance;

    // Pour effectuer les tests, on ré-utilise les données collectées pour
    // le calendrier grégorien.
    public static DataGroup<YemodaAnd<DayOfWeek>> GregorianDayOfWeekData =>
        ProlepticGregorianDataSet.Instance.DayOfWeekData;

    [Theory, MemberData(nameof(GregorianDayOfWeekData))]
    public void GetDayOfWeek_UsingDates(YemodaAnd<DayOfWeek> info)
    {
        var (y, m, d, dayOfWeek) = info;
        ICalendar chr = GregorianCalendar.Instance;
        var dayNumber = chr.GetDayNumberOn(y, m, d);
        var date = CalendarUT.GetCalendarDateOn(dayNumber);
        // Act & Assert
        Assert.Equal(dayOfWeek, date.DayOfWeek);
    }
}

public sealed class GregorianCalendarTests :
    CalendarFacts<GregorianCalendar, ProlepticGregorianDataSet>
{
    public GregorianCalendarTests() : base(GregorianCalendar.Instance) { }

    public static DataGroup<YemodaAnd<DayOfWeek>> DayOfWeekData => DataSet.DayOfWeekData;

    protected override GregorianCalendar GetSingleton() => GregorianCalendar.Instance;

    [Fact]
    public override void Id() => Assert.Equal(Cuid.Gregorian, CalendarUT.Id);

    [Fact]
    public override void Math() => Assert.IsType<Regular12Math>(CalendarUT.Math);

    [Fact]
    public override void Scope() => Assert.IsType<GregorianProlepticShortScope>(CalendarUT.Scope);

    [Fact]
    public void GetCurrentDate()
    {
        var exp = DateTime.Now;
        // Act
        var today = CalendarUT.GetCurrentDate();
        // Assert
        Assert.Equal(exp.Year, today.Year);
        Assert.Equal(exp.Month, today.Month);
        Assert.Equal(exp.Day, today.Day);
    }

    [Theory, MemberData(nameof(DayOfWeekData))]
    public void GetDayOfWeek_UsingDates(YemodaAnd<DayOfWeek> info)
    {
        var (y, m, d, dayOfWeek) = info;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(dayOfWeek, date.DayOfWeek);
    }
}

public sealed class UserDefinedCalendarTests : CalendarFacts<Calendar, StandardGregorianDataSet>
{
    public UserDefinedCalendarTests() : base(CalendarCatalogTests.MyGregorian) { }

    protected override Calendar GetSingleton() => CalendarCatalogTests.MyGregorian;

    [Fact]
    public override void Id() => Assert.Equal(CalendarCatalogTests.MyGregorian.Id, CalendarUT.Id);

    [Fact]
    public override void Math() => Assert.IsType<Regular12Math>(CalendarUT.Math);

    [Fact]
    public override void Scope() => Assert.IsType<GregorianStandardShortScope>(CalendarUT.Scope);
}
