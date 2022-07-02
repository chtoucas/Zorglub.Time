// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides facts about <see cref="WideDate"/>.
/// </summary>
public abstract partial class WideDateFacts<TDataSet> :
    IDateFacts<WideDate, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected WideDateFacts(WideCalendar calendar, WideCalendar otherCalendar)
        : this(calendar, otherCalendar, BaseCtorArgs.Create(calendar)) { }

    private WideDateFacts(WideCalendar calendar, WideCalendar otherCalendar, BaseCtorArgs args)
        : base(args.SupportedYears, args.Domain)
    {
        Debug.Assert(calendar != null);
        Requires.NotNull(otherCalendar);
        // NB: calendars of type WideCalendar are singletons.
        if (ReferenceEquals(otherCalendar, calendar))
        {
            throw new ArgumentException(
                "\"otherCalendar\" MUST NOT be equal to \"calendar\"", nameof(otherCalendar));
        }

        CalendarUT = calendar;
        OtherCalendar = otherCalendar;

        (MinDate, MaxDate) = calendar.MinMaxDate;
    }

    protected WideCalendar CalendarUT { get; }
    protected WideCalendar OtherCalendar { get; }

    protected sealed override WideDate MinDate { get; }
    protected sealed override WideDate MaxDate { get; }

    protected sealed override WideDate GetDate(int y, int m, int d) => CalendarUT.GetDate(y, m, d);

    private sealed record BaseCtorArgs(Range<int> SupportedYears, Range<DayNumber> Domain)
    {
        public static BaseCtorArgs Create(WideCalendar calendar)
        {
            Requires.NotNull(calendar);
            return new BaseCtorArgs(calendar.SupportedYears, calendar.Domain);
        }
    }
}

public partial class WideDateFacts<TDataSet> // Prelude
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void Deconstruct(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetDate(y, m, d);
        // Act
        var (year, month, day) = date;
        // Assert
        Assert.Equal(y, year);
        Assert.Equal(m, month);
        Assert.Equal(d, day);
    }

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

public partial class WideDateFacts<TDataSet> // Calendar mismatch
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

public partial class WideDateFacts<TDataSet> // Conversions
{
    [Fact]
    public void WithCalendar_NullCalendar()
    {
        var date = CalendarUT.GetDate(3, 4, 5);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => date.WithCalendar(null!));
    }
}
