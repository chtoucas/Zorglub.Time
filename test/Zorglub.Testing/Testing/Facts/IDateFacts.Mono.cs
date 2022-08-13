// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

// TODO(fact): améliorer ToString().

/// <summary>
/// Provides facts about <see cref="IDate{TSelf, TCalendar}"/>.
/// </summary>
public abstract partial class IDateFacts<TDate, TCalendar, TDataSet> :
    IDateFacts<TDate, TDataSet>
    where TCalendar : ICalendar<TDate>
    where TDate : struct, IDate<TDate, TCalendar>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected IDateFacts(TCalendar calendar) : base(GetDomain(calendar)) { }

    [Fact]
    public void ToString_InvariantCulture()
    {
        var date = GetDate(1, 1, 1);
        var str = FormattableString.Invariant($"01/01/0001 ({TDate.Calendar})");
        // Act & Assert
        Assert.Equal(str, date.ToString());
    }

    // Althought, we do not usually test static methods/props in a fact class,
    // the situation is a bit different here since this is a static method on a
    // __type__.

    [Fact]
    public void Today()
    {
        // This test may fail if there is a change of day between the two calls
        // to Today().
        var today = DayNumber.Today();
        // Act & Assert
        Assert.Equal(today, TDate.Today().ToDayNumber());
    }

    //[Theory, MemberData(nameof(DayNumberInfoData))]
    //public void FromDayNumber(DayNumberInfo info)
    //{
    //    var (dayNumber, y, m, d) = info;
    //    var date = GetDate(y, m, d);
    //    // Act & Assert
    //    Assert.Equal(date, TDate.FromDayNumber(dayNumber));
    //}
}