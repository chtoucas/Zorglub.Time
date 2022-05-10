﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Core;

public static partial class CalendarTests
{
    public const string CustomKey = "MyFauxKey";
    public static readonly DayNumber CustomEpoch = DayZero.OldStyle;
}

public partial class CalendarTests // Constructors
{
    [Fact]
    public static void Constructor_Sys_NullSchema() =>
        Assert.ThrowsAnexn("schema", () => new FauxSystemCalendar(null!));

    [Fact]
    public static void Constructor_Usr_NullSchema() =>
        Assert.ThrowsAnexn("schema", () => new FauxUserCalendar(schema: null!));

    [Fact]
    public static void Constructor_Usr_NullKey() =>
        Assert.ThrowsAnexn("key", () => new FauxUserCalendar(key: null!));
}

public partial class CalendarTests // Properties
{
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
}

public partial class CalendarTests // Methods
{
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
