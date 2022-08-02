// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Specialized;

// TODO(fact): Calendar_Prop() un peu léger comme test...
// Améliorer ToString().

/// <summary>
/// Provides facts about <see cref="ISpecializedDate{TSelf, TCalendar}"/>.
/// </summary>
public abstract partial class ISpecializedDateFacts<TDate, TCalendar, TDataSet> :
    IDateFacts<TDate, TDataSet>
    where TCalendar : ICalendar<TDate>, INamedCalendar
    where TDate : struct, ISpecializedDate<TDate, TCalendar>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected ISpecializedDateFacts(TCalendar calendar) : base(GetDomain(calendar)) { }

    private static Range<DayNumber> GetDomain(TCalendar calendar)
    {
        Requires.NotNull(calendar);

        return calendar.Domain;
    }

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

    [Theory]
    [InlineData(1, 1, 1, "01/01/0001 ({Calendar})")]
    public void ToString_InvariantCulture(int y, int m, int d, string format)
    {
        Debug.Assert(format != null);

        var date = GetDate(y, m, d);
        var str = format.Replace("{Calendar}", TDate.Calendar.Name, StringComparison.InvariantCulture);
        // Act & Assert
        Assert.Equal(str, date.ToString());
    }
}