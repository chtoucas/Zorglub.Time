// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extensions;

using Zorglub.Time.Hemerology;

/// <summary>
/// Provides extension methods for <see cref="ZDate"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class ZDateExtensions
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
    public static ZDate WithCalendar(this ZDate date, ZCalendar newCalendar)
    {
        Requires.NotNull(newCalendar);

        return newCalendar.GetDate(date.DayNumber);
    }
}
