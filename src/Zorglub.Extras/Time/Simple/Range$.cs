﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Core.Intervals;

// TODO(code): optimize enumeration.
// - Add Contains().
// - Add tests to certify that it's not possible to create a range with
//   endpoints in different calendars.

// A range of a simple type is finite and enumerable:
// - Count(); no need to provide a method LongCount(), Count() never
//   overflows the capacity of Int32.
// - ToEnumerable()
// Other extensions:
// - GetCalendar()

/// <summary>
/// Provides extension methods for <see cref="Range{T}"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static partial class RangeExtensions { }

public partial class RangeExtensions // Range<CalendarYear>
{
    /// <summary>
    /// Obtains the number of years in the specified range.
    /// </summary>
    [Pure]
    public static int Count(this Range<CalendarYear> range) => range.Max - range.Min + 1;

    /// <summary>
    /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range of years.
    /// </summary>
    [Pure]
    public static IEnumerable<CalendarYear> ToEnumerable(this Range<CalendarYear> range)
    {
        var min = range.Min;
        var max = range.Max;

        for (var year = min; year <= max; year++)
        {
            yield return year;
        }
    }

    /// <summary>
    /// Obtains the calendar to which belongs the specified range of years.
    /// </summary>
    [Pure]
    public static SimpleCalendar GetCalendar(this Range<CalendarYear> range) => range.Min.Calendar;
}

public partial class RangeExtensions // Range<CalendarMonth>
{
    /// <summary>
    /// Obtains the number of months in the specified range.
    /// </summary>
    [Pure]
    public static int Count(this Range<CalendarMonth> range) => range.Max - range.Min + 1;

    /// <summary>
    /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range of months.
    /// </summary>
    [Pure]
    public static IEnumerable<CalendarMonth> ToEnumerable(this Range<CalendarMonth> range)
    {
        var min = range.Min;
        var max = range.Max;

        for (var month = min; month <= max; month++)
        {
            yield return month;
        }
    }

    /// <summary>
    /// Obtains the calendar to which belongs the specified range of months.
    /// </summary>
    [Pure]
    public static SimpleCalendar GetCalendar(this Range<CalendarMonth> range) => range.Min.Calendar;
}

public partial class RangeExtensions // Range<CalendarDate>
{
    /// <summary>
    /// Obtains the number of days in the specified range.
    /// </summary>
    [Pure]
    public static int Count(this Range<CalendarDate> range) => range.Max - range.Min + 1;

    /// <summary>
    /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range of days.
    /// </summary>
    [Pure]
    public static IEnumerable<CalendarDate> ToEnumerable(this Range<CalendarDate> range)
    {
        var min = range.Min;
        var max = range.Max;

        for (var date = min; date <= max; date++)
        {
            yield return date;
        }
    }

    /// <summary>
    /// Obtains the calendar to which belongs the specified range of days.
    /// </summary>
    [Pure]
    public static SimpleCalendar GetCalendar(this Range<CalendarDate> range) => range.Min.Calendar;
}

public partial class RangeExtensions // Range<CalendarDay>
{
    /// <summary>
    /// Obtains the number of days in the specified range.
    /// </summary>
    [Pure]
    public static int Count(this Range<CalendarDay> range) => range.Max - range.Min + 1;

    /// <summary>
    /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range of days.
    /// </summary>
    [Pure]
    public static IEnumerable<CalendarDay> ToEnumerable(this Range<CalendarDay> range)
    {
        var min = range.Min;
        var max = range.Max;

        for (var date = min; date <= max; date++)
        {
            yield return date;
        }
    }

    /// <summary>
    /// Obtains the calendar to which belongs the specified range of days.
    /// </summary>
    [Pure]
    public static SimpleCalendar GetCalendar(this Range<CalendarDay> range) => range.Min.Calendar;
}

public partial class RangeExtensions // Range<OrdinalDate>
{
    /// <summary>
    /// Obtains the number of days in the specified range.
    /// </summary>
    [Pure]
    public static int Count(this Range<OrdinalDate> range) => range.Max - range.Min + 1;

    /// <summary>
    /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range of days.
    /// </summary>
    [Pure]
    public static IEnumerable<OrdinalDate> ToEnumerable(this Range<OrdinalDate> range)
    {
        var min = range.Min;
        var max = range.Max;

        for (var date = min; date <= max; date++)
        {
            yield return date;
        }
    }

    /// <summary>
    /// Obtains the calendar to which belongs the specified range of days.
    /// </summary>
    [Pure]
    public static SimpleCalendar GetCalendar(this Range<OrdinalDate> range) => range.Min.Calendar;
}
