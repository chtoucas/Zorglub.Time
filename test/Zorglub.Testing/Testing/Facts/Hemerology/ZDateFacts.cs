﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

using static Zorglub.Time.Extensions.ZDateExtensions;

/// <summary>
/// Provides facts about <see cref="ZDate"/>.
/// </summary>
public abstract partial class ZDateFacts<TDataSet> :
    IDateFacts<ZDate, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected ZDateFacts(ZCalendar calendar, ZCalendar otherCalendar) : base(GetDomain(calendar))
    {
        Debug.Assert(calendar != null);
        Requires.NotNull(otherCalendar);
        // NB: calendars of type ZCalendar are singletons.
        if (ReferenceEquals(otherCalendar, calendar))
        {
            throw new ArgumentException(
                "\"otherCalendar\" MUST NOT be equal to \"calendar\"", nameof(otherCalendar));
        }

        CalendarUT = calendar;
        OtherCalendar = otherCalendar;

        (MinDate, MaxDate) = calendar.MinMaxDate;
    }

    protected ZCalendar CalendarUT { get; }
    protected ZCalendar OtherCalendar { get; }

    protected sealed override ZDate MinDate { get; }
    protected sealed override ZDate MaxDate { get; }

    protected sealed override ZDate GetDate(int y, int m, int d) => CalendarUT.GetDate(y, m, d);
}

public partial class ZDateFacts<TDataSet> // Prelude
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void Calendar_Prop(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(CalendarUT, date.Calendar);
        // We also test the internal prop Cuid.
        Assert.Equal(CalendarUT.Id, date.Cuid);
    }
}

public partial class ZDateFacts<TDataSet> // Calendar mismatch
{
    [Fact]
    public void Equality_OtherCalendar()
    {
        var date = CalendarUT.GetDate(3, 4, 5);
        var other = OtherCalendar.GetDate(3, 4, 5);
        // Act & Assert
        Assert.False(date == other);
        Assert.True(date != other);

        Assert.False(date.Equals(other));
        Assert.False(date.Equals((object)other));
    }

    [Fact]
    public void Comparison_OtherCalendar()
    {
        var date = CalendarUT.GetDate(3, 4, 5);
        var other = OtherCalendar.GetDate(3, 4, 5);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date > other);
        Assert.Throws<ArgumentException>("other", () => date >= other);
        Assert.Throws<ArgumentException>("other", () => date < other);
        Assert.Throws<ArgumentException>("other", () => date <= other);

        Assert.Throws<ArgumentException>("other", () => date.CompareTo(other));
    }

    [Fact]
    public void CountDaysSince_OtherCalendar()
    {
        var date = CalendarUT.GetDate(3, 4, 5);
        var other = OtherCalendar.GetDate(3, 4, 5);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CountDaysSince(other));
        Assert.Throws<ArgumentException>("other", () => date - other);
    }
}

public partial class ZDateFacts<TDataSet> // Conversions
{
    [Fact]
    public void WithCalendar_NullCalendar()
    {
        var date = CalendarUT.GetDate(3, 4, 5);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => date.WithCalendar(null!));
    }
}
