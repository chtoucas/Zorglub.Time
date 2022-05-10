// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Formulae;

/// <summary>
/// Provides the core capabilities necessary to interconvert dates between calendars.
/// <para>At calendar level, it means that we can convert a date to a
/// <see cref="Zorglub.Time.DayNumber"/> and vice-versa.</para>
/// </summary>
public interface IInterconversionFormulae
{
    /// <summary>
    /// Counts the number of consecutive days from the epoch to the specified date.
    /// </summary>
    /// <remarks>
    /// <para>Conversion year/month/day -&gt; daysSinceEpoch.</para>
    /// </remarks>
    [Pure] int CountDaysSinceEpoch(int y, int m, int d);

    /// <summary>
    /// Obtains the date parts for the specified day count (the number of consecutive days from
    /// the epoch to a date); the results are given in output parameters.
    /// </summary>
    /// <remarks>
    /// <para>Conversion <paramref name="daysSinceEpoch"/> -&gt; year/month/day.</para>
    /// </remarks>
    void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d);
}
