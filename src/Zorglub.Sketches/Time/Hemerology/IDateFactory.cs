﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Hemerology;

// Implemented by poly-calendar systems with a single companion date type.
// Could be implemented when the system supports more than one companion
// date type but then we would have to pick up a default date type.

/// <summary>
/// Defines a factory for dates.
/// </summary>
/// <typeparam name="TDate">The type of date object to return.</typeparam>
public interface IDateFactory<out TDate>
{
    /// <summary>
    /// Creates a new date instance of type <typeparamref name="TDate"/>.
    /// </summary>
    /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
    /// values supported by this calendar.</exception>
    [Pure] TDate GetDate(DayNumber dayNumber);

    /// <summary>
    /// Creates a new date instance of type <typeparamref name="TDate"/>.
    /// </summary>
    /// <exception cref="AoorException">The date is either invalid or outside the range of
    /// supported dates.</exception>
    [Pure] TDate GetDate(int year, int month, int day);

    /// <summary>
    /// Creates a new date instance of type <typeparamref name="TDate"/>.
    /// </summary>
    /// <exception cref="AoorException">The ordinal date is either invalid or outside the range
    /// of supported dates.</exception>
    [Pure] TDate GetDate(int year, int dayOfYear);
}
