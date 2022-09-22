// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Validation;

using Zorglub.Time.Core.Intervals;

/// <summary>
/// Provides extension methods for <see cref="Range{DayNumber}"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class DomainExtensions
{
    /// <summary>
    /// Validates the specified <see cref="DayNumber"/> value.
    /// </summary>
    /// <exception cref="AoorException">The validation failed.</exception>
    public static void Validate(this Range<DayNumber> domain, DayNumber dayNumber, string? paramName = null)
    {
        if (dayNumber < domain.Min || dayNumber > domain.Max)
        {
            Throw.ArgumentOutOfRange(paramName ?? nameof(dayNumber));
        }
    }

    /// <summary>
    /// Determines whether the specified <see cref="DayNumber"/> is outside the range of
    /// supported values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="dayNumber"/> would overflow the
    /// range of supported values.</exception>
    public static void CheckOverflow(this Range<DayNumber> domain, DayNumber dayNumber)
    {
        if (dayNumber < domain.Min || dayNumber > domain.Max) Throw.DateOverflow();
    }

    /// <summary>
    /// Determines whether the specified <see cref="DayNumber"/> is greater than the upper bound
    /// of the range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException">The value is greater than the upper bound of the
    /// range of supported values.</exception>
    public static void CheckUpperBound(this Range<DayNumber> domain, DayNumber dayNumber)
    {
        if (dayNumber > domain.Max) Throw.DateOverflow();
    }

    /// <summary>
    /// Determines whether the specified <see cref="DayNumber"/> is less than the lower bound of
    /// the range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException">The value is less than the lower bound of the range
    /// of supported values.</exception>
    public static void CheckLowerBound(this Range<DayNumber> domain, DayNumber dayNumber)
    {
        if (dayNumber < domain.Min) Throw.DateOverflow();
    }
}
