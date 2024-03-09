// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Core.Validation;

using Zorglub.Time.Core.Intervals;

// WARNING: compared to IRangeValidator<T>, IYearsValidator is not the range
// of supported numbers of consecutive years from the epoch, indeed
// YearsSinceEpoch = Year - 1.

/// <summary>Defines a validator for a range of years. </summary>
public interface IYearsValidator
{
    /// <summary>Gets the raw range of values.</summary>
    Range<int> Range { get; }

    /// <summary>Validates the specified year.</summary>
    /// <exception cref="AoorException">The validation failed.</exception>
    void Validate(int year, string? paramName = null);

    /// <summary>Checks whether the specified year is outside the range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="year"/> is outside the range of
    /// supported values.</exception>
    void CheckOverflow(int year);

    /// <summary>Checks whether the specified year is greater than the upper bound of the range of
    /// supported values or not.</summary>
    /// <exception cref="OverflowException"><paramref name="year"/> is greater than the upper bound
    /// of the range of supported values.</exception>
    void CheckUpperBound(int year);

    /// <summary>Checks whether the specified year is less than the lower bound of the range of
    /// supported values or not.</summary>
    /// <exception cref="OverflowException"><paramref name="year"/> is less than the lower bound of
    /// the range of supported values.</exception>
    void CheckLowerBound(int year);
}
