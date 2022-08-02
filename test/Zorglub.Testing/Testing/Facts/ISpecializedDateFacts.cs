// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Specialized;

// TODO(fact): Calendar_Prop() un peu léger comme test...
// Améliorer ToString().

/// <summary>
/// Provides facts about <see cref="ISpecializedDate{TSelf, TCalendar}"/>.
/// </summary>
public abstract partial class ISpecializedDateFacts<TDate, TCalendar, TDataSet> :
    IDateFacts<TDate, TDataSet>
    // BasicCalendar for the prop Name.
    where TCalendar : BasicCalendar, ICalendar<TDate>
    where TDate : struct, ISpecializedDate<TDate, TCalendar>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected ISpecializedDateFacts(TCalendar calendar) : base(GetDomain(calendar)) { }

    [Fact]
    public void Calendar_Prop() => Assert.NotNull(TDate.Calendar);

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void DayNumber_Prop(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(dayNumber, date.DayNumber);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Deconstructor(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act
        var (y1, m1, d1) = date;
        // Assert
        Assert.Equal(y, y1);
        Assert.Equal(m, m1);
        Assert.Equal(d, d1);
    }

    [Fact]
    public void ToString_InvariantCulture()
    {
        var date = GetDate(1, 1, 1);
        var str = FormattableString.Invariant($"01/01/0001 ({TDate.Calendar.Name})");
        // Act & Assert
        Assert.Equal(str, date.ToString());
    }
}