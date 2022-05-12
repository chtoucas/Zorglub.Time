﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;

// REVIEW(fact): ISimpleDate is internal, create and use a SimpleDateProxy?

/// <summary>
/// Provides facts about <see cref="ISimpleDate{TSelf}"/>.
/// </summary>
[TestExcludeFrom(TestExcludeFrom.Smoke)]
public abstract partial class SimpleDateFacts<TDate, TDataSet> : IDateFacts<TDate, TDataSet>
    // ISimpleDate being internal, we cannot use in a type constraint.
    where TDate : struct, IDate<TDate>, ISerializable<TDate, int>
    where TDataSet :
        ICalendarDataSet,
        IDaysAfterDataSet,
        IMathDataSet,
        IDayOfWeekDataSet,
        ISingleton<TDataSet>
{
    protected SimpleDateFacts(Calendar calendar!!, Calendar otherCalendar!!)
        : base(calendar.SupportedYears, calendar.Domain)
    {
        if (otherCalendar == calendar)
        {
            throw new ArgumentException(
                "\"otherCalendar\" MUST NOT be equal to \"calendar\"", nameof(otherCalendar));
        }

        CalendarUT = calendar;
        OtherCalendar = otherCalendar;
    }

    protected Calendar CalendarUT { get; }
    protected Calendar OtherCalendar { get; }
}

public partial class SimpleDateFacts<TDate, TDataSet> // Serialization
{
    // FIXME(code): skip if CalendarUT is not a system calendar.

    [Theory, MemberData(nameof(DateInfoData))]
    public void Serialization_Roundtrip1(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, TDate.FromBinary(date.ToBinary()));
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void Serialization_Roundtrip2(DayNumberInfo info)
    {
        var date = TDate.FromDayNumber(info.DayNumber);
        // Act & Assert
        Assert.Equal(date, TDate.FromBinary(date.ToBinary()));
    }
}
