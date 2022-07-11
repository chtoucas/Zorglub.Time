// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1725 // Parameter names should match base declaration (Naming)

namespace Zorglub.Time.Core.Domains
{
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Represents a range of years, or more precisely the range of supported numbers of
    /// consecutive years from the epoch.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class SupportedYears : IDomain<int>
    {
        private readonly int _min;
        private readonly int _max;

        /// <summary>
        /// Initializes a new instance of the <see cref="AffineDomain"/> class.
        /// </summary>
        public SupportedYears(Range<int> range)
        {
            Range = range;
            (_min, _max) = range.Endpoints;
        }

        /// <summary>
        /// Gets the range of supported years.
        /// </summary>
        public Range<int> Range { get; }

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString() => Range.ToString();

        /// <summary>
        /// Validates the specified year.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        public void Validate(int year)
        {
            if (year < _min || year > _max) Throw.ArgumentOutOfRange(nameof(year));
        }

        /// <summary>
        /// Checks whether the specified year is outside the range of supported values or not.
        /// </summary>
        /// <exception cref="OverflowException"><paramref name="year"/> is outside the range of
        /// supported values.</exception>
        public void Check(int year)
        {
            if (year < _min || year > _max) Throw.DateOverflow();
        }

        /// <summary>
        /// Checks whether the specified year is outside the range of supported values or not.
        /// </summary>
        /// <exception cref="OverflowException"><paramref name="year"/> is outside the range of
        /// supported values.</exception>
        public void CheckForMonth(int year)
        {
            if (year < _min || year > _max) Throw.MonthOverflow();
        }

        /// <summary>
        /// Checks whether the specified year is greater than the upper bound of the range of
        /// supported values or not.
        /// </summary>
        /// <exception cref="OverflowException"><paramref name="year"/> is greater than the upper
        /// bound of the range of supported values.</exception>
        public void CheckUpperBound(int year)
        {
            if (year > _max) Throw.DateOverflow();
        }

        /// <summary>
        /// Checks whether the specified year is less than the lower bound of the range of
        /// supported values or not.
        /// </summary>
        /// <exception cref="OverflowException"><paramref name="year"/> is less than the lower
        /// bound of the range of supported values.</exception>
        public void CheckLowerBound(int year)
        {
            if (year < _max) Throw.DateOverflow();
        }
    }
}
