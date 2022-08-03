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
    // BasicCalendar for the prop Name.
    where TCalendar : BasicCalendar, ICalendar<TDate>
    where TDate : struct, IDate<TDate, TCalendar>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected IDateFacts(TCalendar calendar) : base(GetDomain(calendar)) { }

    [Fact]
    public void ToString_InvariantCulture()
    {
        var date = GetDate(1, 1, 1);
        var str = FormattableString.Invariant($"01/01/0001 ({TDate.Calendar.Name})");
        // Act & Assert
        Assert.Equal(str, date.ToString());
    }
}