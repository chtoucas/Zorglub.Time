// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Testing.Data.Bounded;
using Zorglub.Time.Hemerology.Scopes;

public sealed class UserDefinedCalendarTests : CalendarFacts<Calendar, StandardGregorianDataSet>
{
    public UserDefinedCalendarTests() : base(CalendarCatalogTests.MyGregorian) { }

    protected override Calendar GetSingleton() => CalendarCatalogTests.MyGregorian;

    [Fact]
    public override void Id() =>
        Assert.Equal(CalendarCatalogTests.MyGregorian.Id, CalendarUT.Id);

    [Fact]
    public override void Math() => Assert.IsType<Regular12Math>(CalendarUT.Math);

    [Fact]
    public override void Scope() => Assert.IsType<GregorianStandardShortScope>(CalendarUT.Scope);
}
