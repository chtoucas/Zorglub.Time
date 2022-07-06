// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology.Scopes;

public static class ProlepticShortScopeFacts
{
    public static readonly TheoryData<int> InvalidYearData = new()
    {
        Int32.MinValue,
        ProlepticShortScope.MinYear - 1,
        ProlepticShortScope.MaxYear + 1,
        Int32.MaxValue,
    };

    public static readonly TheoryData<int> ValidYearData = new()
    {
        ProlepticShortScope.MinYear,
        ProlepticShortScope.MinYear + 1,
        -1,
        0,
        1,
        ProlepticShortScope.MaxYear - 1,
        ProlepticShortScope.MaxYear
    };
}

/// <summary>
/// Provides data-driven tests for <see cref="ProlepticShortScope"/>.
/// </summary>
internal abstract class ProlepticShortScopeFacts<TDataSet> :
    ICalendarScopeFacts<ProlepticShortScope, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected ProlepticShortScopeFacts(ProlepticShortScope scope) : base(scope)
    {
        ProlepticScopeView = scope;
    }

    public static TheoryData<int> InvalidYearData => ProlepticShortScopeFacts.InvalidYearData;
    public static TheoryData<int> ValidYearData => ProlepticShortScopeFacts.ValidYearData;

    /// <summary>
    /// Gets a <see cref="ProlepticShortScope"/> view of the scope under test.
    /// </summary>
    protected ProlepticShortScope ProlepticScopeView { get; }

    [Theory, MemberData(nameof(ValidYearData))]
    public sealed override void ValidateYear(int y) => ProlepticScopeView.ValidateYear(y);

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void ValidateYear_InvalidYear(int y)
    {
        Assert.ThrowsAoorexn("year", () => ProlepticScopeView.ValidateYear(y));
        Assert.ThrowsAoorexn("y", () => ProlepticScopeView.ValidateYear(y, nameof(y)));
    }

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void ValidateYearMonth_InvalidYear(int y)
    {
        Assert.ThrowsAoorexn("year", () => ProlepticScopeView.ValidateYearMonth(y, 1));
        Assert.ThrowsAoorexn("y", () => ProlepticScopeView.ValidateYearMonth(y, 1, nameof(y)));
    }

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void ValidateYearMonthDay_InvalidYear(int y)
    {
        Assert.ThrowsAoorexn("year", () => ProlepticScopeView.ValidateYearMonthDay(y, 1, 1));
        Assert.ThrowsAoorexn("y", () => ProlepticScopeView.ValidateYearMonthDay(y, 1, 1, nameof(y)));
    }

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void ValidateOrdinal_InvalidYear(int y)
    {
        Assert.ThrowsAoorexn("year", () => ProlepticScopeView.ValidateOrdinal(y, 1));
        Assert.ThrowsAoorexn("y", () => ProlepticScopeView.ValidateOrdinal(y, 1, nameof(y)));
    }
}
