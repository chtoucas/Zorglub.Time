// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides facts about <see cref="WideDate"/>.
/// </summary>
public abstract partial class WideDateFacts<TDataSet> : IDateFacts<WideDate, TDataSet>
    where TDataSet :
        ICalendarDataSet,
        IDaysAfterDataSet,
        IMathDataSet,
        IDayOfWeekDataSet,
        ISingleton<TDataSet>
{
    protected WideDateFacts(WideCalendar calendar, WideCalendar otherCalendar)
        : this(calendar, otherCalendar, CreateCtorArgs(calendar)) { }

    private WideDateFacts(WideCalendar calendar, WideCalendar otherCalendar!!, CtorArgs args) : base(args)
    {
        if (otherCalendar == calendar)
        {
            throw new ArgumentException(
                "\"otherCalendar\" should not be equal to \"calendar\"", nameof(otherCalendar));
        }

        CalendarUT = calendar;
        OtherCalendar = otherCalendar;

        (MinDate, MaxDate) = calendar.MinMaxDate;
    }

    protected WideCalendar CalendarUT { get; }
    protected WideCalendar OtherCalendar { get; }

    protected sealed override WideDate MinDate { get; }
    protected sealed override WideDate MaxDate { get; }

    protected sealed override WideDate CreateDate(int y, int m, int d) => CalendarUT.GetWideDate(y, m, d);
}

public partial class WideDateFacts<TDataSet>
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void Deconstruct(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        var date = CalendarUT.GetWideDate(y, m, d);
        var (year, month, day) = date;
        // Assert
        Assert.Equal(y, year);
        Assert.Equal(m, month);
        Assert.Equal(d, day);
    }
}

public partial class WideDateFacts<TDataSet> // Properties
{
    // WideCalendar.ForId().
    [Fact]
    public void Calendar_Prop()
    {
        // Arrange
        var date = CalendarUT.GetWideDate(3, 4, 5);
        // Act & Assert
        Assert.Equal(CalendarUT, date.Calendar);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Cuid_Prop(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        var date = CalendarUT.GetWideDate(y, m, d);
        // Assert
        Assert.Equal(CalendarUT.Id, date.Cuid);
    }
}

public partial class WideDateFacts<TDataSet> // Conversions
{
    [Fact]
    public void WithCalendar_NullCalendar()
    {
        // Arrange
        var date = CalendarUT.GetWideDate(3, 4, 5);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => date.WithCalendar(null!));
    }
}

public partial class WideDateFacts<TDataSet> // Math ops
{
    [Fact]
    public void CountDaysSince_InvalidDate()
    {
        // Arrange
        var date = CalendarUT.GetWideDate(3, 4, 5);
        var other = OtherCalendar.GetWideDate(3, 4, 5);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CountDaysSince(other));
        Assert.Throws<ArgumentException>("other", () => date - other);
    }
}

public partial class WideDateFacts<TDataSet> // IEquatable
{
    [Fact]
    public void Equals_OtherCalendar()
    {
        // Arrange
        var date = CalendarUT.GetWideDate(3, 4, 5);
        var other = OtherCalendar.GetWideDate(3, 4, 5);
        // Act & Assert
        Assert.False(date == other);
        Assert.True(date != other);

        Assert.False(date.Equals(other));
        Assert.False(date.Equals((object)other));
    }
}

public partial class WideDateFacts<TDataSet> // IComparable
{
    [Fact]
    public void CompareTo_InvalidCalendar()
    {
        // Arrange
        var date = CalendarUT.GetWideDate(3, 4, 5);
        var other = OtherCalendar.GetWideDate(3, 4, 5);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date > other);
        Assert.Throws<ArgumentException>("other", () => date >= other);
        Assert.Throws<ArgumentException>("other", () => date < other);
        Assert.Throws<ArgumentException>("other", () => date <= other);

        Assert.Throws<ArgumentException>("other", () => date.CompareTo(other));
    }
}
