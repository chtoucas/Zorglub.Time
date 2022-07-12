﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1725 // Parameter names should match base declaration (Naming)

namespace Zorglub.Time.Core.Validation
{
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Represents a range of years, that is the range of supported numbers of consecutive years
    /// from the epoch.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class SupportedYears : IDomain<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SupportedYears"/> class.
        /// </summary>
        public SupportedYears(Range<int> range)
        {
            Range = range;
            (MinYear, MaxYear) = range.Endpoints;
        }

        /// <summary>
        /// Gets the range of supported years.
        /// </summary>
        public Range<int> Range { get; }

        /// <summary>
        /// Gets the ealiest supported year.
        /// </summary>
        public int MinYear { get; }

        /// <summary>
        /// Gets the latest supported year.
        /// </summary>
        public int MaxYear { get; }

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString() => Range.ToString();

        /// <summary>
        /// Validates the specified year.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        public void Validate(int year, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear) Throw.YearOutOfRange(year, paramName);
        }

        /// <summary>
        /// Checks whether the specified year is outside the range of supported values or not.
        /// </summary>
        /// <exception cref="OverflowException"><paramref name="year"/> is outside the range of
        /// supported values.</exception>
        public void Check(int year)
        {
            if (year < MinYear || year > MaxYear) Throw.DateOverflow();
        }

        /// <summary>
        /// Checks whether the specified year is outside the range of supported values or not.
        /// </summary>
        /// <exception cref="OverflowException"><paramref name="year"/> is outside the range of
        /// supported values.</exception>
        public void CheckForMonth(int year)
        {
            if (year < MinYear || year > MaxYear) Throw.MonthOverflow();
        }

        /// <summary>
        /// Checks whether the specified year is greater than the upper bound of the range of
        /// supported values or not.
        /// </summary>
        /// <exception cref="OverflowException"><paramref name="year"/> is greater than the upper
        /// bound of the range of supported values.</exception>
        public void CheckUpperBound(int year)
        {
            if (year > MaxYear) Throw.DateOverflow();
        }

        /// <summary>
        /// Checks whether the specified year is less than the lower bound of the range of
        /// supported values or not.
        /// </summary>
        /// <exception cref="OverflowException"><paramref name="year"/> is less than the lower
        /// bound of the range of supported values.</exception>
        public void CheckLowerBound(int year)
        {
            if (year < MaxYear) Throw.DateOverflow();
        }
    }
}