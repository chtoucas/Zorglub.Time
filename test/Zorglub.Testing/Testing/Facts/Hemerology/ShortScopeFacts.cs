// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology.Scopes;

/// <summary>
/// Provides data-driven tests for <see cref="ShortScope"/>.
/// </summary>
internal abstract class ShortScopeFacts<TScope, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TScope : ShortScope
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected ShortScopeFacts(TScope scope)
    {
        ScopeUT = scope ?? throw new ArgumentNullException(nameof(scope));
    }

    /// <summary>
    /// Gets the scope under test.
    /// </summary>
    protected TScope ScopeUT { get; }

    #region ValidateYear()

    [Theory] public abstract void ValidateYear_InvalidYear(int y);
    [Theory] public abstract void ValidateYear(int y);

    #endregion
    #region ValidateYearMonth()

    [Theory] public abstract void ValidateYearMonth_InvalidYear(int y);

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void ValidateYearMonth_InvalidMonth(int y, int m)
    {
        Assert.ThrowsAoorexn("month", () => ScopeUT.ValidateYearMonth(y, m));
        Assert.ThrowsAoorexn("m", () => ScopeUT.ValidateYearMonth(y, m, nameof(m)));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void ValidateYearMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        ScopeUT.ValidateYearMonth(y, m);
    }

    #endregion
    #region ValidateYearMonthDay()

    [Theory] public abstract void ValidateYearMonthDay_InvalidYear(int y);

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void ValidateYearMonthDay_InvalidMonth(int y, int m)
    {
        Assert.ThrowsAoorexn("month", () => ScopeUT.ValidateYearMonthDay(y, m, 1));
        Assert.ThrowsAoorexn("m", () => ScopeUT.ValidateYearMonthDay(y, m, 1, nameof(m)));
    }

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void ValidateYearMonthDay_InvalidDay(int y, int m, int d)
    {
        Assert.ThrowsAoorexn("day", () => ScopeUT.ValidateYearMonthDay(y, m, d));
        Assert.ThrowsAoorexn("d", () => ScopeUT.ValidateYearMonthDay(y, m, d, nameof(d)));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void ValidateYearMonthDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        ScopeUT.ValidateYearMonthDay(y, m, d);
    }

    #endregion
    #region ValidateOrdinal()

    [Theory] public abstract void ValidateOrdinal_InvalidYear(int y);

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void ValidateOrdinal_InvalidDayOfYear(int y, int doy)
    {
        Assert.ThrowsAoorexn("dayOfYear", () => ScopeUT.ValidateOrdinal(y, doy));
        Assert.ThrowsAoorexn("doy", () => ScopeUT.ValidateOrdinal(y, doy, nameof(doy)));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void ValidateOrdinal(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        ScopeUT.ValidateOrdinal(y, doy);
    }

    #endregion
}
