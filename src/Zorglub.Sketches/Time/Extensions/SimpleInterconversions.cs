// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Extensions;

using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Simple;

/// <summary>
/// Provides interconversion helpers as extension methods.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static partial class SimpleInterconversions { }

public partial class SimpleInterconversions // Core types
{
    /// <summary>
    /// Interconverts the specified date to a date within a different calendar.
    /// </summary>
    /// <remarks>
    /// <para>This method always performs the conversion whether it's necessary or not. To avoid
    /// an expensive operation, it's better to check before that <paramref name="newCalendar"/>
    /// is actually different from the calendar of the current instance.</para>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
    /// </exception>
    /// <exception cref="AoorException">The specified date cannot be converted into the new
    /// calendar, the resulting date would be outside its range of years.</exception>
    [Pure]
    public static CalendarDay WithCalendar(this CalendarDate date, SimpleCalendar newCalendar)
    {
        Requires.NotNull(newCalendar);

        return newCalendar.GetDate(date.DayNumber);
    }

    /// <summary>
    /// Interconverts the specified date to a date within a different calendar.
    /// </summary>
    /// <remarks>
    /// <para>This method always performs the conversion whether it's necessary or not. To avoid
    /// an expensive operation, it's better to check before that <paramref name="newCalendar"/>
    /// is actually different from the calendar of the current instance.</para>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
    /// </exception>
    /// <exception cref="AoorException">The specified date cannot be converted into the new
    /// calendar, the resulting date would be outside its range of years.</exception>
    [Pure]
    public static CalendarDay WithCalendar(this CalendarDay date, SimpleCalendar newCalendar)
    {
        Requires.NotNull(newCalendar);

        return newCalendar.GetDate(date.DayNumber);
    }

    /// <summary>
    /// Interconverts the specified date to a date within a different calendar.
    /// </summary>
    /// <remarks>
    /// <para>This method always performs the conversion whether it's necessary or not. To avoid
    /// an expensive operation, it's better to check before that <paramref name="newCalendar"/>
    /// is actually different from the calendar of the current instance.</para>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
    /// </exception>
    /// <exception cref="AoorException">The specified date cannot be converted into the new
    /// calendar, the resulting date would be outside its range of years.</exception>
    [Pure]
    public static CalendarDay WithCalendar(this OrdinalDate date, SimpleCalendar newCalendar)
    {
        Requires.NotNull(newCalendar);

        return newCalendar.GetDate(date.DayNumber);
    }

    /// <summary>
    /// Interconverts the specified month to a range of days within a different calendar.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
    /// </exception>
    [Pure]
    public static Range<CalendarDay> WithCalendar(this CalendarMonth month, SimpleCalendar newCalendar)
    {
        Requires.NotNull(newCalendar);

        var min = newCalendar.GetDate(month.FirstDay.DayNumber);
        var max = newCalendar.GetDate(month.LastDay.DayNumber);

        return Range.Create(min, max);
    }

    /// <summary>
    /// Interconverts the specified year to a range of days within a different calendar.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
    /// </exception>
    [Pure]
    public static Range<CalendarDay> WithCalendar(this CalendarYear year, SimpleCalendar newCalendar)
    {
        Requires.NotNull(newCalendar);

        var min = newCalendar.GetDate(year.FirstDay.DayNumber);
        var max = newCalendar.GetDate(year.LastDay.DayNumber);

        return Range.Create(min, max);
    }
}

public partial class SimpleInterconversions // Ranges
{
    /// <summary>
    /// Interconverts the specified range of days to a range within a different calendar.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
    /// </exception>
    [Pure]
    public static Range<CalendarDay> WithCalendar(this Range<CalendarDate> range, SimpleCalendar newCalendar)
    {
        Requires.NotNull(newCalendar);

        var (min, max) = range.Endpoints;

        var start = newCalendar.GetDate(min.DayNumber);
        var end = newCalendar.GetDate(max.DayNumber);

        return Range.Create(start, end);
    }

    /// <summary>
    /// Interconverts the specified range of days to a range within a different calendar.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
    /// </exception>
    [Pure]
    public static Range<CalendarDay> WithCalendar(this Range<CalendarDay> range, SimpleCalendar newCalendar)
    {
        Requires.NotNull(newCalendar);

        var (min, max) = range.Endpoints;

        var start = newCalendar.GetDate(min.DayNumber);
        var end = newCalendar.GetDate(max.DayNumber);

        return Range.Create(start, end);
    }

    /// <summary>
    /// Interconverts the specified range of days to a range within a different calendar.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
    /// </exception>
    [Pure]
    public static Range<CalendarDay> WithCalendar(this Range<OrdinalDate> range, SimpleCalendar newCalendar)
    {
        Requires.NotNull(newCalendar);

        var (min, max) = range.Endpoints;

        var start = newCalendar.GetDate(min.DayNumber);
        var end = newCalendar.GetDate(max.DayNumber);

        return Range.Create(start, end);
    }

    // In general, it's not possible to interconvert a range of years, one must
    // first convert it to a range of days.

    /// <summary>
    /// Interconverts the specified range of months to a range of days within a different calendar.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
    /// </exception>
    [Pure]
    public static Range<CalendarDay> WithCalendar(this Range<CalendarMonth> range, SimpleCalendar newCalendar)
    {
        Requires.NotNull(newCalendar);

        var (min, max) = range.Endpoints;

        var start = newCalendar.GetDate(min.FirstDay.DayNumber);
        var end = newCalendar.GetDate(max.LastDay.DayNumber);

        return Range.Create(start, end);
    }

    /// <summary>
    /// Interconverts the specified range of years to a range of days within a different calendar.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
    /// </exception>
    [Pure]
    public static Range<CalendarDay> WithCalendar(this Range<CalendarYear> range, SimpleCalendar newCalendar)
    {
        Requires.NotNull(newCalendar);

        var (min, max) = range.Endpoints;

        var start = newCalendar.GetDate(min.FirstDay.DayNumber);
        var end = newCalendar.GetDate(max.LastDay.DayNumber);

        return Range.Create(start, end);
    }
}
