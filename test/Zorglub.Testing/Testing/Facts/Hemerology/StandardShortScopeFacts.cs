// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology.Scopes;

// FIXME(fact): skip negative years.

public static class StandardShortScopeFacts
{
    public static readonly TheoryData<int> InvalidYearData = new()
    {
        Int32.MinValue,
        StandardShortScope.MinYear - 1,
        ShortScope.MaxYear + 1,
        Int32.MaxValue,
    };

    public static readonly TheoryData<int> ValidYearData = new()
    {
        StandardShortScope.MinYear,
        StandardShortScope.MinYear + 1,
        ShortScope.MaxYear - 1,
        ShortScope.MaxYear
    };
}

/// <summary>
/// Provides data-driven tests for <see cref="StandardShortScope"/>.
/// </summary>
[TestExcludeFrom(TestExcludeFrom.Smoke)]
internal abstract class StandardShortScopeFacts<TDataSet> :
    ShortScopeFacts<StandardShortScope, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected StandardShortScopeFacts(StandardShortScope scope) : base(scope)
    {
        StandardScopeView = scope;
    }

    public static TheoryData<int> InvalidYearData => StandardShortScopeFacts.InvalidYearData;
    public static TheoryData<int> ValidYearData => StandardShortScopeFacts.ValidYearData;

    /// <summary>
    /// Gets a <see cref="StandardShortScope"/> view of the scope under test.
    /// </summary>
    protected StandardShortScope StandardScopeView { get; }

    [Theory, MemberData(nameof(ValidYearData))]
    public sealed override void ValidateYear(int y) => StandardScopeView.ValidateYear(y);

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void ValidateYear_InvalidYear(int y)
    {
        Assert.ThrowsAoorexn("year", () => StandardScopeView.ValidateYear(y));
        Assert.ThrowsAoorexn("y", () => StandardScopeView.ValidateYear(y, nameof(y)));
    }

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void ValidateYearMonth_InvalidYear(int y)
    {
        Assert.ThrowsAoorexn("year", () => StandardScopeView.ValidateYearMonth(y, 1));
        Assert.ThrowsAoorexn("y", () => StandardScopeView.ValidateYearMonth(y, 1, nameof(y)));
    }

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void ValidateYearMonthDay_InvalidYear(int y)
    {
        Assert.ThrowsAoorexn("year", () => StandardScopeView.ValidateYearMonthDay(y, 1, 1));
        Assert.ThrowsAoorexn("y", () => StandardScopeView.ValidateYearMonthDay(y, 1, 1, nameof(y)));
    }

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void ValidateOrdinal_InvalidYear(int y)
    {
        Assert.ThrowsAoorexn("year", () => StandardScopeView.ValidateOrdinal(y, 1));
        Assert.ThrowsAoorexn("y", () => StandardScopeView.ValidateOrdinal(y, 1, nameof(y)));
    }
}
