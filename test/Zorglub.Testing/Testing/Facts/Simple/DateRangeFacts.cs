// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology.Scopes;
using Zorglub.Time.Simple;

/// <summary>
/// Provides facts about <see cref="DateRange"/>.
/// </summary>
public abstract partial class DateRangeFacts<TDataSet> :
    IDateRangeFacts<CalendarDate, DateRange, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected DateRangeFacts(Calendar calendar, Calendar otherCalendar)
    {
        Requires.NotNull(calendar);
        Requires.NotNull(otherCalendar);
        // NB: calendars of type Calendar are singletons.
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
    protected Calendar CalendarUT { get; }

    protected Calendar OtherCalendar { get; }

    protected sealed override CalendarDate CreateDate(int y, int m, int d) =>
        CalendarUT.GetCalendarDate(y, m, d);
    protected sealed override CalendarDate CreateDate(int y, int doy) =>
        CalendarUT.GetOrdinalDate(y, doy).ToCalendarDate();

    protected sealed override DateRange CreateRange(CalendarDate start, CalendarDate end) =>
        DateRange.Create(start, end);
    protected sealed override DateRange CreateRange(CalendarDate start, int length) =>
        DateRange.Create(start, length);

    protected sealed override bool CheckCalendar(DateRange range)
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
        var start = CalendarUT.GetCalendarDate(3, 4, 5);
        var end = OtherCalendar.GetCalendarDate(3, 4, 6);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => DateRange.Create(start, end));
    }

    [Fact]
    public void Deconstructor()
    {
        var start = CalendarUT.GetCalendarDate(3, 4, 5);
        var end = start + 9;
        var range = DateRange.Create(start, end);
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
        var start = CalendarUT.GetCalendarDate(3, 4, 5);
        var end = start + 9;
        var range = DateRange.Create(start, end);
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
        var start = CalendarUT.GetCalendarDate(3, 4, 5);
        var range = DateRange.Create(start, 2);
        // Act
        string str = range.ToString();
        // Assert
        Assert.False(String.IsNullOrEmpty(str));
    }

    #region Contains()

    [Fact]
    public void Contains_InvalidDate()
    {
        var start = CalendarUT.GetCalendarDate(3, 4, 5);
        var range = DateRange.Create(start, 25);
        var date = OtherCalendar.GetCalendarDate(3, 4, 5);
        // Act & Assert
        Assert.Throws<ArgumentException>("date", () => range.Contains(date));
    }

    [Fact]
    public void IsSupersetOf_InvalidRange()
    {
        var start1 = CalendarUT.GetCalendarDate(3, 4, 5);
        var range1 = DateRange.Create(start1, 25);
        var start2 = OtherCalendar.GetCalendarDate(3, 4, 5);
        var range2 = DateRange.Create(start2, 25);
        // Act & Assert
        Assert.Throws<ArgumentException>("range", () => range1.IsSupersetOf(range2));
    }

    [Fact]
    public void Contains_InvalidMonth()
    {
        var start = CalendarUT.GetCalendarDate(3, 4, 5);
        var range = DateRange.Create(start, 25);
        var month = OtherCalendar.GetCalendarMonth(3, 4);
        // Act & Assert
        Assert.Throws<ArgumentException>("month", () => range.Contains(month));
    }

    [Fact]
    public void Contains_InvalidYear()
    {
        var start = CalendarUT.GetCalendarDate(3, 4, 5);
        var range = DateRange.Create(start, 25);
        var cyear = OtherCalendar.GetCalendarYear(3);
        // Act & Assert
        Assert.Throws<ArgumentException>("year", () => range.Contains(cyear));
    }

    #endregion

    #region Contains(month)

    [Fact]
    public void Contains_Month_EndsBeforeMonth()
    {
        var start = CalendarUT.GetCalendarDate(3, 3, 1);
        var range = DateRange.Create(start, 2);
        var month = CalendarUT.GetCalendarMonth(3, 4);

        Assert.True(range.End < CalendarDate.AtStartOfMonth(month), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_OverlapsStartOfMonth()
    {
        var start = CalendarUT.GetCalendarDate(3, 3, 31);
        var range = DateRange.Create(start, 10);
        var month = CalendarUT.GetCalendarMonth(3, 4);

        Assert.True(range.Start < CalendarDate.AtStartOfMonth(month), "Self-check");
        Assert.True(range.End > CalendarDate.AtStartOfMonth(month), "Self-check");
        Assert.True(range.End < CalendarDate.AtEndOfMonth(month), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_SameStartEndsBeforeMonth()
    {
        var start = CalendarUT.GetCalendarDate(3, 4, 1);
        var range = DateRange.Create(start, 2);
        var month = CalendarUT.GetCalendarMonth(3, 4);

        Assert.Equal(range.Start, CalendarDate.AtStartOfMonth(month), "Self-check");
        Assert.True(range.End < CalendarDate.AtEndOfMonth(month), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_StrictlyInMonth()
    {
        var start = CalendarUT.GetCalendarDate(3, 4, 5);
        var range = DateRange.Create(start, 2);
        var month = CalendarUT.GetCalendarMonth(3, 4);

        Assert.True(range.Start > CalendarDate.AtStartOfMonth(month), "Self-check");
        Assert.True(range.End < CalendarDate.AtEndOfMonth(month), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_StartsAfterStartOfMonthSameEnd()
    {
        var start = CalendarUT.GetCalendarDate(3, 4, 5);
        var range = DateRange.Create(start, 26);
        var month = CalendarUT.GetCalendarMonth(3, 4);

        Assert.True(range.Start > CalendarDate.AtStartOfMonth(month), "Self-check");
        Assert.Equal(range.End, CalendarDate.AtEndOfMonth(month), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_OverlapsEndOfMonth()
    {
        var start = CalendarUT.GetCalendarDate(3, 4, 5);
        var range = DateRange.Create(start, 100);
        var month = CalendarUT.GetCalendarMonth(3, 4);

        Assert.True(range.Start > CalendarDate.AtStartOfMonth(month), "Self-check");
        Assert.True(range.Start < CalendarDate.AtEndOfMonth(month), "Self-check");
        Assert.True(range.End > CalendarDate.AtEndOfMonth(month), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_StartsAfterMonth()
    {
        var start = CalendarUT.GetCalendarDate(3, 5, 1);
        var range = DateRange.Create(start, 2);
        var month = CalendarUT.GetCalendarMonth(3, 4);

        Assert.True(range.Start > CalendarDate.AtEndOfMonth(month), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(month));
    }

    [Fact]
    public void Contains_Month()
    {
        var start = CalendarUT.GetCalendarDate(3, 4, 1);
        var range = DateRange.Create(start, 100);
        var month = CalendarUT.GetCalendarMonth(3, 5);

        Assert.True(range.Start < CalendarDate.AtStartOfMonth(month), "Self-check");
        Assert.True(range.End > CalendarDate.AtEndOfMonth(month), "Self-check");

        // Act & Assert
        Assert.True(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_SameStart()
    {
        var start = CalendarUT.GetCalendarDate(3, 5, 1);
        var range = DateRange.Create(start, 100);
        var month = CalendarUT.GetCalendarMonth(3, 5);

        Assert.Equal(range.Start, CalendarDate.AtStartOfMonth(month), "Self-check");
        Assert.True(range.End > CalendarDate.AtEndOfMonth(month), "Self-check");

        // Act & Assert
        Assert.True(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_SameEnd()
    {
        var start = CalendarUT.GetCalendarDate(3, 4, 30);
        var range = DateRange.Create(start, 32);
        var month = CalendarUT.GetCalendarMonth(3, 5);

        Assert.True(range.Start < CalendarDate.AtStartOfMonth(month), "Self-check");
        Assert.Equal(range.End, CalendarDate.AtEndOfMonth(month), "Self-check");

        // Act & Assert
        Assert.True(range.Contains(month));
    }

    [Fact]
    public void Contains_Month_SameStartEnd()
    {
        var start = CalendarUT.GetCalendarDate(3, 4, 1);
        var range = DateRange.Create(start, 30);
        var month = CalendarUT.GetCalendarMonth(3, 4);

        Assert.Equal(range.Start, CalendarDate.AtStartOfMonth(month), "Self-check");
        Assert.Equal(range.End, CalendarDate.AtEndOfMonth(month), "Self-check");

        // Act & Assert
        Assert.True(range.Contains(month));
    }

    #endregion

    #region Contains(year)

    [Fact]
    public void Contains_Year_EndsBeforeYear()
    {
        var start = CalendarUT.GetCalendarDate(3, 4, 5);
        var range = DateRange.Create(start, 2);
        var cyear = CalendarUT.GetCalendarYear(4);

        Assert.True(range.Start < CalendarDate.AtStartOfYear(cyear), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_OverlapsStartOfYear()
    {
        var start = CalendarUT.GetCalendarDate(3, 12, 1);
        var range = DateRange.Create(start, 100);
        var cyear = CalendarUT.GetCalendarYear(4);

        Assert.True(range.Start < CalendarDate.AtStartOfYear(cyear), "Self-check");
        Assert.True(range.End > CalendarDate.AtStartOfYear(cyear), "Self-check");
        Assert.True(range.End < CalendarDate.AtEndOfYear(cyear), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_SameStartEndsBeforeYear()
    {
        var start = CalendarUT.GetCalendarDate(4, 1, 1);
        var range = DateRange.Create(start, 100);
        var cyear = CalendarUT.GetCalendarYear(4);

        Assert.Equal(range.Start, CalendarDate.AtStartOfYear(cyear), "Self-check");
        Assert.True(range.End < CalendarDate.AtEndOfYear(cyear), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_StrictlyInYear()
    {
        var start = CalendarUT.GetCalendarDate(4, 2, 1);
        var range = DateRange.Create(start, 100);
        var cyear = CalendarUT.GetCalendarYear(4);

        Assert.True(range.Start > CalendarDate.AtStartOfYear(cyear), "Self-check");
        Assert.True(range.End < CalendarDate.AtEndOfYear(cyear), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_StartsAfterStartOfYearSameEnd()
    {
        var start = CalendarUT.GetCalendarDate(4, 1, 2);
        var range = DateRange.Create(start, 365);
        var cyear = CalendarUT.GetCalendarYear(4);

        Assert.True(range.Start > CalendarDate.AtStartOfYear(cyear), "Self-check");
        Assert.Equal(range.End, CalendarDate.AtEndOfYear(cyear), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_OverlapsEndOfYear()
    {
        var start = CalendarUT.GetCalendarDate(4, 1, 2);
        var range = DateRange.Create(start, 400);
        var cyear = CalendarUT.GetCalendarYear(4);

        Assert.True(range.Start > CalendarDate.AtStartOfYear(cyear), "Self-check");
        Assert.True(range.Start < CalendarDate.AtEndOfYear(cyear), "Self-check");
        Assert.True(range.End > CalendarDate.AtEndOfYear(cyear), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_StartsAfterYear()
    {
        var start = CalendarUT.GetCalendarDate(5, 1, 1);
        var range = DateRange.Create(start, 2);
        var cyear = CalendarUT.GetCalendarYear(4);

        Assert.True(range.Start > CalendarDate.AtEndOfYear(cyear), "Self-check");

        // Act & Assert
        Assert.False(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year()
    {
        var start = CalendarUT.GetCalendarDate(3, 3, 1);
        var range = DateRange.Create(start, 1000);
        var cyear = CalendarUT.GetCalendarYear(4);

        Assert.True(range.Start < CalendarDate.AtStartOfYear(cyear), "Self-check");
        Assert.True(range.End > CalendarDate.AtEndOfYear(cyear), "Self-check");

        // Act & Assert
        Assert.True(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_SameStart()
    {
        var start = CalendarUT.GetCalendarDate(3, 1, 1);
        var range = DateRange.Create(start, 1000);
        var cyear = CalendarUT.GetCalendarYear(3);

        Assert.Equal(range.Start, CalendarDate.AtStartOfYear(cyear), "Self-check");
        Assert.True(range.End > CalendarDate.AtEndOfYear(cyear), "Self-check");

        // Act & Assert
        Assert.True(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_SameEnd()
    {
        var start = CalendarUT.GetCalendarDate(2, 12, 31);
        var range = DateRange.Create(start, 366);
        var cyear = CalendarUT.GetCalendarYear(3);

        Assert.True(range.Start < CalendarDate.AtStartOfYear(cyear), "Self-check");
        Assert.Equal(range.End, CalendarDate.AtEndOfYear(cyear), "Self-check");

        // Act & Assert
        Assert.True(range.Contains(cyear));
    }

    [Fact]
    public void Contains_Year_SameStartEnd()
    {
        var start = CalendarUT.GetCalendarDate(3, 1, 1);
        var range = DateRange.Create(start, 365);
        var cyear = CalendarUT.GetCalendarYear(3);

        Assert.Equal(range.Start, CalendarDate.AtStartOfYear(cyear), "Self-check");
        Assert.Equal(range.End, CalendarDate.AtEndOfYear(cyear), "Self-check");

        // Act & Assert
        Assert.True(range.Contains(cyear));
    }

    #endregion

    #region WithCalendar()

    [Fact]
    public void WithCalendar_InvalidCalendar()
    {
        var date = CalendarUT.GetCalendarDate(3, 4, 5);
        var range = DateRange.Create(date, 6);
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
        if (y >= ShortScope.MaxYear) { return; }

        var start = CalendarUT.GetCalendarDate(y, m, d);
        // Act
        var range1 = DateRange.Create(start, 29);
        var range2 = DateRange.Create(start, 29);
        // Assert
        Assert.True(range1 == range2);
        Assert.False(range1 != range2);
    }

#pragma warning disable CA1508 // Avoid dead conditional code (Maintainability) 👈 Tests

    [Fact]
    public void Equality_Null()
    {
#nullable disable
        var start = CalendarUT.GetCalendarDate(3, 4, 5);
        var range = DateRange.Create(start, 29);
        var nullRange = (DateRange)null!;
        var nullRange1 = (DateRange)null!;
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
