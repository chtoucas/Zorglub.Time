// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Utilities;
using Zorglub.Time.Hemerology.Scopes;
using Zorglub.Time.Simple;

/// <summary>
/// Provides facts about <see cref="DateRangeV0"/>.
/// </summary>
[Obsolete("DateRangeV0 is obsolete.")]
public abstract partial class DateRangeFacts<TDataSet> :
    IDateRangeFacts<CalendarDate, DateRangeV0, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected DateRangeFacts(SimpleCalendar calendar, SimpleCalendar otherCalendar)
    {
        Requires.NotNull(calendar);
        Requires.NotNull(otherCalendar);
        // NB: instances of type Calendar are singletons.
        if (ReferenceEquals(otherCalendar, calendar))
        {
            throw new ArgumentException(
                "\"otherCalendar\" must not be equal to \"calendar\"", nameof(otherCalendar));
        }

        CalendarUT = calendar;
        OtherCalendar = otherCalendar;
    }

    /// <summary>
    /// Gets the calendar under test.
    /// </summary>
    protected SimpleCalendar CalendarUT { get; }

    protected SimpleCalendar OtherCalendar { get; }

    protected sealed override CalendarDate GetDate(int y, int m, int d) =>
        CalendarUT.GetDate(y, m, d);
    protected sealed override CalendarDate GetDate(int y, int doy) =>
        CalendarUT.GetDate(y, doy).ToCalendarDate();

    protected sealed override DateRangeV0 CreateRange(CalendarDate start, CalendarDate end) =>
        DateRangeV0.Create(start, end);
    protected sealed override DateRangeV0 CreateRange(CalendarDate start, int length) =>
        DateRangeV0.Create(start, length);

    protected sealed override bool CheckCalendar(DateRangeV0 range)
    {
        Requires.NotNull(range);

        return range.Calendar.Id == CalendarUT.Id;
    }

    protected sealed override CalendarDate PlusDays(CalendarDate date, int days) =>
        date.PlusDays(days);
}

public partial class DateRangeFacts<TDataSet>
{
    [Fact]
    public void Create_InvalidCalendar()
    {
        var start = CalendarUT.GetDate(3, 4, 5);
        var end = OtherCalendar.GetDate(3, 4, 6);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => DateRangeV0.Create(start, end));
    }

    [Fact]
    public void Deconstructor()
    {
        var start = CalendarUT.GetDate(3, 4, 5);
        var end = start + 9;
        var range = DateRangeV0.Create(start, end);
        // Act
        var (from, to) = range;
        // Assert
        Assert.Equal(start, from);
        Assert.Equal(end, to);
        Assert.Equal(10, range.Length);
        Assert.Equal(CalendarUT.Id, range.Calendar.Id);
    }

    [Fact]
    public void Deconstructor_Length()
    {
        var start = CalendarUT.GetDate(3, 4, 5);
        var end = start + 9;
        var range = DateRangeV0.Create(start, end);
        // Act
        var (from, to, length) = range;
        // Assert
        Assert.Equal(start, from);
        Assert.Equal(end, to);
        Assert.Equal(10, length);
        Assert.Equal(CalendarUT.Id, range.Calendar.Id);
    }

    [Fact]
    public void ToString_DoesSomething()
    {
        var start = CalendarUT.GetDate(3, 4, 5);
        var range = DateRangeV0.Create(start, 2);
        // Act
        string str = range.ToString();
        // Assert
        Assert.False(String.IsNullOrEmpty(str));
    }

    #region Contains()

    [Fact]
    public void Contains_InvalidDate()
    {
        var start = CalendarUT.GetDate(3, 4, 5);
        var range = DateRangeV0.Create(start, 25);
        var date = OtherCalendar.GetDate(3, 4, 5);
        // Act & Assert
        Assert.Throws<ArgumentException>("date", () => range.Contains(date));
    }

    [Fact]
    public void IsSupersetOf_InvalidRange()
    {
        var start1 = CalendarUT.GetDate(3, 4, 5);
        var range1 = DateRangeV0.Create(start1, 25);
        var start2 = OtherCalendar.GetDate(3, 4, 5);
        var range2 = DateRangeV0.Create(start2, 25);
        // Act & Assert
        Assert.Throws<ArgumentException>("range", () => range1.IsSupersetOf(range2));
    }

    [Fact]
    public void Contains_InvalidMonth()
    {
        var start = CalendarUT.GetDate(3, 4, 5);
        var range = DateRangeV0.Create(start, 25);
        var month = OtherCalendar.GetCalendarMonth(3, 4);
        // Act & Assert
        Assert.Throws<ArgumentException>("month", () => range.Contains(month));
    }

    [Fact]
    public void Contains_InvalidYear()
    {
        var start = CalendarUT.GetDate(3, 4, 5);
        var range = DateRangeV0.Create(start, 25);
        var cyear = OtherCalendar.GetCalendarYear(3);
        // Act & Assert
        Assert.Throws<ArgumentException>("year", () => range.Contains(cyear));
    }

    #endregion

    #region Contains(month)

    [Fact]
    public void Contains_Month_EndsBeforeMonth()
    {
        var start = CalendarUT.GetDate(3, 3, 1);
        var range = DateRangeV0.Create(start, 2);
        var month = CalendarUT.GetCalendarMonth(3, 4);

        Assert.True(range.End < month.FirstDay, "Self-check");

        // Act & Assert
        Assert.False(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_OverlapsStartOfMonth()
    {
        var start = CalendarUT.GetDate(3, 3, 31);
        var range = DateRangeV0.Create(start, 10);
        var month = CalendarUT.GetCalendarMonth(3, 4);

        Assert.True(range.Start < month.FirstDay, "Self-check");
        Assert.True(range.End > month.FirstDay, "Self-check");
        Assert.True(range.End < month.LastDay, "Self-check");

        // Act & Assert
        Assert.False(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_SameStartEndsBeforeMonth()
    {
        var start = CalendarUT.GetDate(3, 4, 1);
        var range = DateRangeV0.Create(start, 2);
        var month = CalendarUT.GetCalendarMonth(3, 4);

        Assert.Equal(range.Start, month.FirstDay, "Self-check");
        Assert.True(range.End < month.LastDay, "Self-check");

        // Act & Assert
        Assert.False(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_StrictlyInMonth()
    {
        var start = CalendarUT.GetDate(3, 4, 5);
        var range = DateRangeV0.Create(start, 2);
        var month = CalendarUT.GetCalendarMonth(3, 4);

        Assert.True(range.Start > month.FirstDay, "Self-check");
        Assert.True(range.End < month.LastDay, "Self-check");

        // Act & Assert
        Assert.False(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_StartsAfterStartOfMonthSameEnd()
    {
        var start = CalendarUT.GetDate(3, 4, 5);
        var range = DateRangeV0.Create(start, 26);
        var month = CalendarUT.GetCalendarMonth(3, 4);

        Assert.True(range.Start > month.FirstDay, "Self-check");
        Assert.Equal(range.End, month.LastDay, "Self-check");

        // Act & Assert
        Assert.False(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_OverlapsEndOfMonth()
    {
        var start = CalendarUT.GetDate(3, 4, 5);
        var range = DateRangeV0.Create(start, 100);
        var month = CalendarUT.GetCalendarMonth(3, 4);

        Assert.True(range.Start > month.FirstDay, "Self-check");
        Assert.True(range.Start < month.LastDay, "Self-check");
        Assert.True(range.End > month.LastDay, "Self-check");

        // Act & Assert
        Assert.False(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_StartsAfterMonth()
    {
        var start = CalendarUT.GetDate(3, 5, 1);
        var range = DateRangeV0.Create(start, 2);
        var month = CalendarUT.GetCalendarMonth(3, 4);

        Assert.True(range.Start > month.LastDay, "Self-check");

        // Act & Assert
        Assert.False(range.Contains(month));
    }

    [Fact]
    public void Contains_Month()
    {
        var start = CalendarUT.GetDate(3, 4, 1);
        var range = DateRangeV0.Create(start, 100);
        var month = CalendarUT.GetCalendarMonth(3, 5);

        Assert.True(range.Start < month.FirstDay, "Self-check");
        Assert.True(range.End > month.LastDay, "Self-check");

        // Act & Assert
        Assert.True(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_SameStart()
    {
        var start = CalendarUT.GetDate(3, 5, 1);
        var range = DateRangeV0.Create(start, 100);
        var month = CalendarUT.GetCalendarMonth(3, 5);

        Assert.Equal(range.Start, month.FirstDay, "Self-check");
        Assert.True(range.End > month.LastDay, "Self-check");

        // Act & Assert
        Assert.True(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_SameEnd()
    {
        var start = CalendarUT.GetDate(3, 4, 30);
        var range = DateRangeV0.Create(start, 32);
        var month = CalendarUT.GetCalendarMonth(3, 5);

        Assert.True(range.Start < month.FirstDay, "Self-check");
        Assert.Equal(range.End, month.LastDay, "Self-check");

        // Act & Assert
        Assert.True(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_SameStartEnd()
    {
        var start = CalendarUT.GetDate(3, 4, 1);
        var range = DateRangeV0.Create(start, 30);
        var month = CalendarUT.GetCalendarMonth(3, 4);

        Assert.Equal(range.Start, month.FirstDay, "Self-check");
        Assert.Equal(range.End, month.LastDay, "Self-check");

        // Act & Assert
        Assert.True(range.Contains(month));
    }

    #endregion

    #region Contains(year)

    [Fact]
    public void Contains_Year_EndsBeforeYear()
    {
        var start = CalendarUT.GetDate(3, 4, 5);
        var range = DateRangeV0.Create(start, 2);
        var cyear = CalendarUT.GetCalendarYear(4);

        Assert.True(range.Start < cyear.FirstDay.ToCalendarDate(), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_OverlapsStartOfYear()
    {
        var start = CalendarUT.GetDate(3, 12, 1);
        var range = DateRangeV0.Create(start, 100);
        var cyear = CalendarUT.GetCalendarYear(4);

        Assert.True(range.Start < cyear.FirstDay.ToCalendarDate(), "Self-check");
        Assert.True(range.End > cyear.FirstDay.ToCalendarDate(), "Self-check");
        Assert.True(range.End < cyear.LastDay.ToCalendarDate(), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_SameStartEndsBeforeYear()
    {
        var start = CalendarUT.GetDate(4, 1, 1);
        var range = DateRangeV0.Create(start, 100);
        var cyear = CalendarUT.GetCalendarYear(4);

        Assert.Equal(range.Start, cyear.FirstDay.ToCalendarDate(), "Self-check");
        Assert.True(range.End < cyear.LastDay.ToCalendarDate(), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_StrictlyInYear()
    {
        var start = CalendarUT.GetDate(4, 2, 1);
        var range = DateRangeV0.Create(start, 100);
        var cyear = CalendarUT.GetCalendarYear(4);

        Assert.True(range.Start > cyear.FirstDay.ToCalendarDate(), "Self-check");
        Assert.True(range.End < cyear.LastDay.ToCalendarDate(), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_StartsAfterStartOfYearSameEnd()
    {
        var start = CalendarUT.GetDate(4, 1, 2);
        var range = DateRangeV0.Create(start, 365);
        var cyear = CalendarUT.GetCalendarYear(4);

        Assert.True(range.Start > cyear.FirstDay.ToCalendarDate(), "Self-check");
        Assert.Equal(range.End, cyear.LastDay.ToCalendarDate(), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_OverlapsEndOfYear()
    {
        var start = CalendarUT.GetDate(4, 1, 2);
        var range = DateRangeV0.Create(start, 400);
        var cyear = CalendarUT.GetCalendarYear(4);

        Assert.True(range.Start > cyear.FirstDay.ToCalendarDate(), "Self-check");
        Assert.True(range.Start < cyear.LastDay.ToCalendarDate(), "Self-check");
        Assert.True(range.End > cyear.LastDay.ToCalendarDate(), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_StartsAfterYear()
    {
        var start = CalendarUT.GetDate(5, 1, 1);
        var range = DateRangeV0.Create(start, 2);
        var cyear = CalendarUT.GetCalendarYear(4);

        Assert.True(range.Start > cyear.LastDay.ToCalendarDate(), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year()
    {
        var start = CalendarUT.GetDate(3, 3, 1);
        var range = DateRangeV0.Create(start, 1000);
        var cyear = CalendarUT.GetCalendarYear(4);

        Assert.True(range.Start < cyear.FirstDay.ToCalendarDate(), "Self-check");
        Assert.True(range.End > cyear.LastDay.ToCalendarDate(), "Self-check");

        // Act & Assert
        Assert.True(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_SameStart()
    {
        var start = CalendarUT.GetDate(3, 1, 1);
        var range = DateRangeV0.Create(start, 1000);
        var cyear = CalendarUT.GetCalendarYear(3);

        Assert.Equal(range.Start, cyear.FirstDay.ToCalendarDate(), "Self-check");
        Assert.True(range.End > cyear.LastDay.ToCalendarDate(), "Self-check");

        // Act & Assert
        Assert.True(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_SameEnd()
    {
        var start = CalendarUT.GetDate(2, 12, 31);
        var range = DateRangeV0.Create(start, 366);
        var cyear = CalendarUT.GetCalendarYear(3);

        Assert.True(range.Start < cyear.FirstDay.ToCalendarDate(), "Self-check");
        Assert.Equal(range.End, cyear.LastDay.ToCalendarDate(), "Self-check");

        // Act & Assert
        Assert.True(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_SameStartEnd()
    {
        var start = CalendarUT.GetDate(3, 1, 1);
        var range = DateRangeV0.Create(start, 365);
        var cyear = CalendarUT.GetCalendarYear(3);

        Assert.Equal(range.Start, cyear.FirstDay.ToCalendarDate(), "Self-check");
        Assert.Equal(range.End, cyear.LastDay.ToCalendarDate(), "Self-check");

        // Act & Assert
        Assert.True(range.Contains(cyear));
    }

    #endregion

    #region WithCalendar()

    [Fact]
    public void WithCalendar_InvalidCalendar()
    {
        var date = CalendarUT.GetDate(3, 4, 5);
        var range = DateRangeV0.Create(date, 6);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => range.WithCalendar(null!));
    }

    #endregion
}

public partial class DateRangeFacts<TDataSet> // IEquatable
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void Equality_Structural(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Autrement, on ne pourrait pas créer range1/2.
        // En réalité, seules les dates telles que date + 29 provoque un
        // dépassement arithmétique devraient être ignorées.
        if (y >= StandardScope.MaxSupportedYear) { return; }

        var start = CalendarUT.GetDate(y, m, d);
        // Act
        var range1 = DateRangeV0.Create(start, 29);
        var range2 = DateRangeV0.Create(start, 29);
        // Assert
        Assert.True(range1 == range2);
        Assert.False(range1 != range2);
    }

#pragma warning disable CA1508 // Avoid dead conditional code (Maintainability) 👈 Tests

    [Fact]
    public void Equality_Null()
    {
#nullable disable
        var start = CalendarUT.GetDate(3, 4, 5);
        var range = DateRangeV0.Create(start, 29);
        var nullRange = (DateRangeV0)null!;
        var nullRange1 = (DateRangeV0)null!;
        // Act & Assert
        // The order of statements is important otherwise Equals(null)
        // will fool the compiler, it will believe that "range" is null.
        // Simpler: disable nullables.
        Assert.True(range.Equals((object)range));
        Assert.False(range.Equals((object)null!));
        Assert.False(range.Equals(new object()));
        Assert.False(range == (object)null);
        Assert.False((object)null == range);
        Assert.True(range != (object)null);
        Assert.True((object)null != range);

        Assert.True(range != nullRange);
        Assert.True(nullRange != range);
        Assert.False(range == nullRange);
        Assert.False(nullRange == range);

        Assert.True(nullRange == nullRange1);
        Assert.False(nullRange != nullRange1);
#nullable restore
    }

#pragma warning restore CA1508
}
