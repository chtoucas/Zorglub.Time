// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology.Scopes;

// FIXME(fact): skip negative years.
// Use an ICalendarDataSet? StandardCalendarDataSet?
// What about ProlepticScopeFacts.

public static class StandardScopeFacts
{
    public static readonly TheoryData<int> InvalidYearData = new()
    {
        Int32.MinValue,
        StandardScope.MinYear - 1,
        StandardScope.MaxYear + 1,
        Int32.MaxValue,
    };

    public static readonly TheoryData<int> ValidYearData = new()
    {
        StandardScope.MinYear,
        StandardScope.MinYear + 1,
        StandardScope.MaxYear - 1,
        StandardScope.MaxYear
    };
}

/// <summary>
/// Provides data-driven tests for <see cref="StandardScope"/>.
/// </summary>
internal abstract class StandardScopeFacts<TDataSet> :
    CalendarScopeFacts<StandardScope, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected StandardScopeFacts(StandardScope scope) : base(scope)
    {
        StandardScopeView = scope;
    }

    public static TheoryData<int> InvalidYearData => StandardScopeFacts.InvalidYearData;
    public static TheoryData<int> ValidYearData => StandardScopeFacts.ValidYearData;

    /// <summary>
    /// Gets a <see cref="StandardScope"/> view of the scope under test.
    /// </summary>
    protected StandardScope StandardScopeView { get; }

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
