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
    public const string CustomKey = "MyFauxKey";
    public static readonly DayNumber CustomEpoch = DayZero.OldStyle;

    [Fact]
    public static void Constructor_Sys_NullSchema() =>
        Assert.ThrowsAnexn("schema", () => new FauxSystemCalendar(null!));

    [Fact]
    public static void Constructor_Usr_NullSchema() =>
        Assert.ThrowsAnexn("schema", () => new FauxUserCalendar(schema: null!));

    [Fact]
    public static void Constructor_Usr_NullKey() =>
        Assert.ThrowsAnexn("key", () => new FauxUserCalendar(key: null!));

    [Theory, MemberData(nameof(EnumDataSet.CalendarIdData), MemberType = typeof(EnumDataSet))]
    public static void Key_Sys(CalendarId id)
    {
        var chr = new FauxSystemCalendar(id);
        // Act & Assert
        Assert.Equal(id.ToCalendarKey(), chr.Key);
    }

    [Fact]
    public static void Key_Usr()
    {
        var chr = new FauxUserCalendar(CustomKey);
        // Act & Assert
        Assert.Equal(CustomKey, chr.Key);
    }

    [Fact]
    public static void IsUserDefined()
    {
        var sys = new FauxSystemCalendar();
        var usr = new FauxUserCalendar();
        // Act & Assert
        Assert.False(sys.IsUserDefined);
        Assert.True(usr.IsUserDefined);
    }

    [Theory, MemberData(nameof(EnumDataSet.CalendarIdData), MemberType = typeof(EnumDataSet))]
    public static void PermanentId_Sys(CalendarId id)
    {
        var chr = new FauxSystemCalendar(id);
        // Act & Assert
        Assert.Equal(id, chr.PermanentId);
    }

    [Fact]
    public static void PermanentId_Usr()
    {
        var chr = new FauxUserCalendar();
        // Act & Assert
        Assert.Throws<NotSupportedException>(() => chr.PermanentId);
    }

    [Fact]
    public static void Epoch()
    {
        var sys = new FauxSystemCalendar(CustomEpoch);
        var usr = new FauxUserCalendar(CustomEpoch);
        // Act & Assert
        Assert.Equal(CustomEpoch, sys.Epoch);
        Assert.Equal(CustomEpoch, usr.Epoch);
    }

    [Fact]
    public static void IsProleptic()
    {
        Assert.False(new FauxSystemCalendar().IsProleptic);
        Assert.False(new FauxUserCalendar().IsProleptic);
        Assert.True(new FauxSystemCalendar(proleptic: true).IsProleptic);
        Assert.True(new FauxUserCalendar(proleptic: true).IsProleptic);
    }

    //[Theory, MemberData(nameof(EnumDataSet.CalendricalAlgorithmData), MemberType = typeof(EnumDataSet))]
    //public static void Algorithm(CalendricalAlgorithm algorithm)
    //{
    //    var sys = new FauxSysCalendar_(FauxSchema.WithAlgorithm(algorithm));
    //    var usr = new FauxUsrCalendar_(FauxSchema.WithAlgorithm(algorithm));
    //    // Act & Assert
    //    Assert.Equal(algorithm, sys.Algorithm);
    //    Assert.Equal(algorithm, usr.Algorithm);
    //}

    [Theory, MemberData(nameof(EnumDataSet.CalendricalFamilyData), MemberType = typeof(EnumDataSet))]
    public static void Family(CalendricalFamily family)
    {
        var sys = new FauxSystemCalendar(new FauxSystemSchema(family));
        var usr = new FauxUserCalendar(new FauxSystemSchema(family));
        // Act & Assert
        Assert.Equal(family, sys.Family);
        Assert.Equal(family, usr.Family);
    }

    [Theory, MemberData(nameof(EnumDataSet.CalendricalAdjustmentsData), MemberType = typeof(EnumDataSet))]
    public static void PeriodicAdjustments_Sys(CalendricalAdjustments adjustments)
    {
        var sys = new FauxSystemCalendar(new FauxSystemSchema(adjustments));
        var usr = new FauxUserCalendar(new FauxSystemSchema(adjustments));
        // Act & Assert
        Assert.Equal(adjustments, sys.PeriodicAdjustments);
        Assert.Equal(adjustments, usr.PeriodicAdjustments);
    }

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
    public static void Id_Usr()
    {
        var chr = new FauxUserCalendar(Cuid.MinUser);
        // Act & Assert
        Assert.Equal(Cuid.MinUser, chr.Id);
    }

    [Fact]
    public static void ToString_ReturnsKey()
    {
        var sys = new FauxSystemCalendar();
        var usr = new FauxUserCalendar(CustomKey);
        // Act & Assert
        Assert.Equal(sys.Key, sys.ToString());
        Assert.Equal(usr.Key, usr.ToString());
    }

    [Fact]
    public static void ValidateCuid_Sys()
    {
        var chr = new FauxSystemCalendar();
        string paramName = "cuidParam";
        // Act & Assert
        chr.ValidateCuidDisclosed((Cuid)FauxSystemCalendar.DefaultId, paramName);

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
        chr.ValidateCuidDisclosed(FauxUserCalendar.DefaultId, paramName);

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
    public void Today()
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

    [Fact]
    public void NewGregorianDate_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => new CalendarDate(y, 1, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public static void NewGregorianDate_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => new CalendarDate(y, m, 1));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public static void NewGregorianDate_InvalidDay(int y, int m, int d) =>
        Assert.ThrowsAoorexn("day", () => new CalendarDate(y, m, d));

    [Theory, MemberData(nameof(DateInfoData))]
    public static void NewGregorianDate_UsingDates(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        var date = new CalendarDate(y, m, d);
        var (year, month, day) = date;

        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);

        Assert.Equal(y, year);
        Assert.Equal(m, month);
        Assert.Equal(d, day);

        Assert.Equal(doy, date.DayOfYear);
        Assert.Equal(info.IsIntercalary, date.IsIntercalary);
        Assert.False(date.IsSupplementary);
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public static void NewGregorianDate_UsingDayNumbers(DayNumberInfo info)
    {
        var (y, m, d) = info.Yemoda;

        // Act
        var date = new CalendarDate(y, m, d);
        var (year, month, day) = date;
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);

        Assert.Equal(y, year);
        Assert.Equal(m, month);
        Assert.Equal(d, day);
    }

    [Fact]
    public void NewGregorianMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => new CalendarMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public static void NewGregorianMonth_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => new CalendarMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public static void NewGregorianMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        var cmonth = new CalendarMonth(y, m);
        var (year, month) = cmonth;

        // Assert
        Assert.Equal(y, cmonth.Year);
        Assert.Equal(m, cmonth.Month);

        Assert.Equal(y, year);
        Assert.Equal(m, month);

        Assert.Equal(info.IsIntercalary, cmonth.IsIntercalary);
    }

    [Fact]
    public void NewGregorianYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => new CalendarYear(y));

    [Theory, MemberData(nameof(YearInfoData))]
    public static void NewGregorianYear(YearInfo info)
    {
        int y = info.Year;

        // Act
        var cyear = new CalendarYear(y);
        // Assert
        Assert.Equal(y, cyear.Year);
        Assert.Equal(info.IsLeap, cyear.IsLeap);
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
