// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1725 // Parameter names should match base declaration (Naming) ✓
// The base interface is internal. Anyway, we prefer to use domain-specific parameter names.

namespace Zorglub.Time.Core.Validation;

using Zorglub.Time.Core.Intervals;

/// <summary>
/// Represents a validator for a range of months, that is the range of supported numbers of
/// consecutive months from the epoch.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class MonthsValidator : IRangeValidator<int>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MonthsValidator"/> class.
    /// </summary>
    public MonthsValidator(Range<int> range)
    {
        Range = range;
        (MinMonthsSinceEpoch, MaxMonthsSinceEpoch) = range.Endpoints;
    }

    /// <summary>
    /// Gets the range of supported numbers of consecutive months from the epoch.
    /// </summary>
    public Range<int> Range { get; }

    /// <summary>
    /// Gets the minimum number of consecutive months from the epoch.
    /// </summary>
    public int MinMonthsSinceEpoch { get; }

    /// <summary>
    /// Gets the maximum number of consecutive months from the epoch.
    /// </summary>
    public int MaxMonthsSinceEpoch { get; }

    /// <summary>
    /// Returns a culture-independent string representation of the current instance.
    /// </summary>
    [Pure]
    public sealed override string ToString() => Range.ToString();

    /// <summary>
    /// Validates the specified number of consecutive months from the epoch.
    /// </summary>
    /// <exception cref="AoorException">The validation failed.</exception>
    public void Validate(int monthsSinceEpoch, string? paramName = null)
    {
        if (monthsSinceEpoch < MinMonthsSinceEpoch || monthsSinceEpoch > MaxMonthsSinceEpoch)
        {
            Throw.ArgumentOutOfRange(paramName ?? nameof(monthsSinceEpoch));
        }
    }

    /// <summary>
    /// Checks whether the specified number of consecutive months from the epoch is outside the
    /// range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="monthsSinceEpoch"/> is outside the
    /// range of supported values.
    /// </exception>
    public void CheckOverflow(int monthsSinceEpoch)
    {
        if (monthsSinceEpoch < MinMonthsSinceEpoch || monthsSinceEpoch > MaxMonthsSinceEpoch)
        {
            Throw.MonthOverflow();
        }
    }

    /// <summary>
    /// Checks whether the specified number of consecutive months from the epoch is greater than
    /// the upper bound of the range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="monthsSinceEpoch"/> is greater than
    /// the upper bound of the range of supported values.</exception>
    public void CheckUpperBound(int monthsSinceEpoch)
    {
        if (monthsSinceEpoch > MaxMonthsSinceEpoch) Throw.MonthOverflow();
    }

    /// <summary>
    /// Checks whether the specified number of consecutive months from the epoch is less than the
    /// lower bound of the range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="monthsSinceEpoch"/> is less than the
    /// lower bound of the range of supported values.</exception>
    public void CheckLowerBound(int monthsSinceEpoch)
    {
        if (monthsSinceEpoch < MinMonthsSinceEpoch) Throw.MonthOverflow();
    }
}
