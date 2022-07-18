// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;

public static partial class CalendricalScopeTests
{
    private static readonly int DefaultMinYear = SystemSchema.DefaultSupportedYears.Min;
    private static readonly int DefaultMaxYear = SystemSchema.DefaultSupportedYears.Max;

    private static readonly ICalendricalSchema s_Schema = new GregorianSchema();

    // On teste le "scope" maximal pour le schéma grégorien.
    public static readonly TheoryData<int> InvalidGregorianYear = new()
    {
        DefaultMinYear - 1,
        DefaultMaxYear + 1
    };
    public static readonly TheoryData<int> ValidGregorianYear = new()
    {
        DefaultMinYear,
        DefaultMinYear + 1,
        -1,
        0,
        1,
        DefaultMaxYear - 1,
        DefaultMaxYear
    };

    [Fact]
    public static void TestTest() =>
        Assert.Equal(SystemSchema.DefaultSupportedYears, s_Schema.SupportedYears);

    [Fact]
    public static void Constructor_InvalidMinYear()
    {
        var (minYear, maxYear) = s_Schema.SupportedYears.Endpoints;
        // Act
        Assert.ThrowsAoorexn("year",
            () => new FauxCalendarScope(s_Schema, minYear - 1, maxYear));
    }

    [Fact]
    public static void Constructor_InvalidMaxYear()
    {
        var (minYear, maxYear) = s_Schema.SupportedYears.Endpoints;
        // Act
        Assert.ThrowsAoorexn("year",
            () => new FauxCalendarScope(s_Schema, minYear, maxYear + 1));
        Assert.ThrowsAoorexn("max",
            () => new FauxCalendarScope(s_Schema, 2, 1));
    }

    [Fact]
    public static void Constructor()
    {
        // Act
        var scope = new FauxCalendarScope(s_Schema, 1, 2);
        // Assert
        Assert.NotNull(scope);
        Assert.Equal(1, scope.SupportedYears.MinYear);
        Assert.Equal(2, scope.SupportedYears.MaxYear);
    }

    // FIXME
#if false

    [Fact]
    public static void Constructor_NullMinMaxYear()
    {
        // Act
        var scope = new FauxCalendarScope(Schema, null, null);
        // Assert
        Assert.NotNull(scope);
        Assert.Equal(Schema.MinYear, scope.MinYear);
        Assert.Equal(Schema.MaxYear, scope.MaxYear);
    }

    [Theory, MemberData(nameof(InvalidGregorianYear))]
    public static void ValidateYear_InvalidYear(int y)
    {
        ICalendricalScope scope = new FauxCalendarScope(Schema, null, null);
        // Act
        Assert.ThrowsAoorexn("year", () => scope.ValidateYear(y));
        Assert.ThrowsAoorexn("y", () => scope.ValidateYear(y, nameof(y)));
    }

    [Theory, MemberData(nameof(ValidGregorianYear))]
    public static void ValidateYear(int y)
    {
        ICalendricalScope scope = new FauxCalendarScope(Schema, null, null);
        // Act
        scope.ValidateYear(y);
    }

    // Valeurs pour le schéma grégorien.
    [Theory]
    [InlineData(Int32.MinValue, true)]
    [InlineData(SystemSchema.DefaultMinYear - 1, true)]
    [InlineData(SystemSchema.DefaultMinYear, false)]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(SystemSchema.DefaultMaxYear, false)]
    [InlineData(SystemSchema.DefaultMaxYear + 1, true)]
    [InlineData(Int32.MaxValue, true)]
    public static void CheckYearOverflowOrUnderflow(int y, bool overflow)
    {
        ICalendricalScope scope = new FauxCalendarScope(Schema, null, null);
        // Act
        if (overflow)
        {
            Assert.Overflows(() => scope.CheckYearOverflowOrUnderflow(y));
        }
        else
        {
            scope.CheckYearOverflowOrUnderflow(y);
        }
    }

#endif
}

public static partial class CalendricalScopeTests // ICalendricalScope
{
    //[Fact]
    //public static void GetMaximalScope()
    //{
    //    // Gregorian case.
    //    Assert.IsType<GregorianMaximalScope>(GetMaximalScope(new GregorianSchema()));

    //    // MinMaxYearScope.
    //    Assert.IsType<MinMaxYearScope>(GetMaximalScope(new Coptic12Schema()));
    //    Assert.IsType<MinMaxYearScope>(GetMaximalScope(new Egyptian12Schema()));
    //    Assert.IsType<MinMaxYearScope>(GetMaximalScope(new FrenchRepublican12Schema()));
    //    Assert.IsType<MinMaxYearScope>(GetMaximalScope(new JulianSchema()));
    //    Assert.IsType<MinMaxYearScope>(GetMaximalScope(new Persian2820Schema()));
    //    Assert.IsType<MinMaxYearScope>(GetMaximalScope(new Tropicalia3031Schema()));
    //    Assert.IsType<MinMaxYearScope>(GetMaximalScope(new Tropicalia3130Schema()));
    //    Assert.IsType<MinMaxYearScope>(GetMaximalScope(new TropicaliaSchema()));
    //    Assert.IsType<MinMaxYearScope>(GetMaximalScope(new WorldSchema()));

    //    // Solar13.
    //    Assert.IsType<MinMaxYearScope>(GetMaximalScope(new InternationalFixedSchema()));
    //    Assert.IsType<MinMaxYearScope>(GetMaximalScope(new PositivistSchema()));

    //    // Lunar.
    //    Assert.IsType<MinMaxYearScope>(GetMaximalScope(new TabularIslamicSchema()));

    //    // Lunisolar.
    //    //TODO: Assert.IsType<MinMaxYearScope>(GetMaximalScope(HebrewSchema.Instance));

    //    // Other.
    //    Assert.IsType<MinMaxYearScope>(GetMaximalScope(new Coptic13Schema()));
    //    Assert.IsType<MinMaxYearScope>(GetMaximalScope(new Egyptian13Schema()));
    //    Assert.IsType<MinMaxYearScope>(GetMaximalScope(new FrenchRepublican13Schema()));
    //    Assert.IsType<MinMaxYearScope>(GetMaximalScope(new PaxSchema()));

    //    static ICalendricalScope GetMaximalScope<TSchema>(TSchema schema)
    //        where TSchema : SystemSchema
    //    {
    //        return ICalendricalScope.GetMaximalScope(schema, default, true);
    //    }
    //}

    //[Fact]
    //public static void GetMaximalScope_WidestIsTrue()
    //{
    //    // Act
    //    var scope = ICalendricalScope.GetMaximalScope(Schema, DayZero.NewStyle, widest: true);
    //    // Assert
    //    Assert.NotNull(scope);
    //    Assert.Equal(Schema.MinYear, scope.MinYear);
    //    Assert.Equal(Schema.MaxYear, scope.MaxYear);
    //}

    //[Fact]
    //public static void GetMaximalScope_WidestIsFalse()
    //{
    //    // Act
    //    var scope = ICalendricalScope.GetMaximalScope(Schema, DayZero.NewStyle, widest: false);
    //    // Assert
    //    Assert.NotNull(scope);
    //    Assert.Equal(1, scope.MinYear);
    //    Assert.Equal(Schema.MaxYear, scope.MaxYear);
    //}
}
