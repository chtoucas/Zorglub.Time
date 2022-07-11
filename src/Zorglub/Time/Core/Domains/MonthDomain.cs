// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1725 // Parameter names should match base declaration (Naming)

namespace Zorglub.Time.Core.Domains
{
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Represents a range of months, or more precisely the range of supported numbers of
    /// consecutive months from the epoch.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class MonthDomain : IDomain<int>
    {
        private readonly int _min;
        private readonly int _max;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthDomain"/> class.
        /// </summary>
        public MonthDomain(Range<int> range)
        {
            Range = range;
            (_min, _max) = range.Endpoints;
        }

        /// <summary>
        /// Gets the range of supported numbers of consecutive months from the epoch.
        /// </summary>
        public Range<int> Range { get; }

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString() => Range.ToString();

        /// <summary>
        /// Validates the specified number of consecutive months from the epoch.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        public void Validate(int monthsSinceEpoch)
        {
            if (monthsSinceEpoch < _min || monthsSinceEpoch > _max)
            {
                Throw.ArgumentOutOfRange(nameof(monthsSinceEpoch));
            }
        }

        /// <summary>
        /// Checks whether the specified number of consecutive months from the epoch is outside the
        /// range of supported values or not.
        /// </summary>
        /// <exception cref="OverflowException"><paramref name="monthsSinceEpoch"/> is outside the
        /// range of supported values.
        /// </exception>
        public void Check(int monthsSinceEpoch)
        {
            if (monthsSinceEpoch < _min || monthsSinceEpoch > _max) Throw.MonthOverflow();
        }

        /// <summary>
        /// Checks whether the specified number of consecutive months from the epoch is greater than
        /// the upper bound of the range of supported values or not.
        /// </summary>
        /// <exception cref="OverflowException"><paramref name="monthsSinceEpoch"/> is greater than
        /// the upper bound of the range of supported values.</exception>
        public void CheckUpperBound(int monthsSinceEpoch)
        {
            if (monthsSinceEpoch > _max) Throw.MonthOverflow();
        }

        /// <summary>
        /// Checks whether the specified number of consecutive months from the epoch is less than the
        /// lower bound of the range of supported values or not.
        /// </summary>
        /// <exception cref="OverflowException"><paramref name="monthsSinceEpoch"/> is less than the
        /// lower bound of the range of supported values.</exception>
        public void CheckLowerBound(int monthsSinceEpoch)
        {
            if (monthsSinceEpoch < _min) Throw.MonthOverflow();
        }
    }
}
