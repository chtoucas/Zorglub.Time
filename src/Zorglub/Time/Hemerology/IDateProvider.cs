// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

/// <summary>Defines a provider for dates.</summary>
/// <typeparam name="TDate">The type of date object to return.</typeparam>
public interface IDateProvider<out TDate>
{
    /// <summary>Enumerates the days in the specified year.</summary>
    /// <exception cref="AoorException">The year is not within the calendar boundaries.</exception>
    [Pure] IEnumerable<TDate> GetDaysInYear(int year);

    /// <summary>Enumerates the days in the specified month.</summary>
    /// <exception cref="AoorException">The month is not within the calendar boundaries.</exception>
    [Pure] IEnumerable<TDate> GetDaysInMonth(int year, int month);

    /// <summary>Obtains the date for the first supported day of the specified year.</summary>
    /// <exception cref="AoorException">The year is not within the calendar boundaries.</exception>
    [Pure] TDate GetStartOfYear(int year);

    /// <summary>Obtains the date for the last supported day of the specified year.</summary>
    /// <exception cref="AoorException">The year is not within the calendar boundaries.</exception>
    [Pure] TDate GetEndOfYear(int year);

    /// <summary>Obtains the date for the first supported day of the specified month.</summary>
    /// <exception cref="AoorException">The month is not within the calendar boundaries.</exception>
    [Pure] TDate GetStartOfMonth(int year, int month);

    /// <summary>Obtains the date for the last supported day of the specified month.</summary>
    /// <exception cref="AoorException">The month is not within the calendar boundaries.</exception>
    [Pure] TDate GetEndOfMonth(int year, int month);
}
