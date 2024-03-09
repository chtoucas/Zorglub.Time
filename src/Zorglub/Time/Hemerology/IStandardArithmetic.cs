// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Hemerology;

/// <summary>Defines the standard mathematical operations on a date, those related to the day unit.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IStandardArithmetic<TSelf>
    where TSelf : IStandardArithmetic<TSelf>
{
    /// <summary>Counts the number of days elapsed since the specified date.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="other"/> does not belong to the calendar
    /// of the current instance.</exception>
    [Pure] int CountDaysSince(TSelf other);

    /// <summary>Adds a number of days to this date instance, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow either the capacity of
    /// <see cref="Int32"/> or the range of supported dates.</exception>
    [Pure] TSelf PlusDays(int days);

    /// <summary>Obtains the day after this date instance.</summary>
    /// <exception cref="OverflowException">The operation would overflow the latest supported date.
    /// </exception>
    [Pure] TSelf NextDay();

    /// <summary>Obtains the day before this date instance.</summary>
    /// <exception cref="OverflowException">The operation would overflow the earliest supported date.
    /// </exception>
    [Pure] TSelf PreviousDay();
}
