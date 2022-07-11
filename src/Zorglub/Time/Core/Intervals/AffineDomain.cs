// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Intervals
{
    /// <summary>
    /// Represents a range of days, or more precisely the range of supported numbers of consecutive
    /// days from the epoch.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class AffineDomain
    {
        private readonly int _min;
        private readonly int _max;

        /// <summary>
        /// Initializes a new instance of the <see cref="AffineDomain"/> class.
        /// </summary>
        public AffineDomain(Range<int> range)
        {
            Range = range;
            (_min, _max) = range.Endpoints;
        }

        /// <summary>
        /// Gets the range of supported numbers of consecutive days from the epoch.
        /// </summary>
        public Range<int> Range { get; }

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString() => Range.ToString();

        /// <summary>
        /// Converts the current instance to a domain, a range of day numbers.
        /// </summary>
        [Pure]
        public Range<DayNumber> ToDomain(DayNumber epoch) =>
            Intervals.Range.FromEndpoints(Range.Endpoints.Select(x => epoch + x));

        /// <summary>
        /// Validates the specified number of consecutive days from the epoch.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        public void Validate(int daysSinceEpoch)
        {
            if (daysSinceEpoch < _min || daysSinceEpoch > _max)
            {
                Throw.ArgumentOutOfRange(nameof(daysSinceEpoch));
            }
        }

        /// <summary>
        /// Checks whether the specified number of consecutive days from the epoch is outside the
        /// range of supported values or not.
        /// </summary>
        /// <exception cref="OverflowException"><paramref name="daysSinceEpoch"/> is outside the
        /// range of supported values.
        /// </exception>
        public void Check(int daysSinceEpoch)
        {
            if (daysSinceEpoch < _min || daysSinceEpoch > _max) Throw.DateOverflow();
        }

        /// <summary>
        /// Checks whether the specified number of consecutive days from the epoch is greater than
        /// the upper bound of the range of supported values or not.
        /// </summary>
        /// <exception cref="OverflowException"><paramref name="daysSinceEpoch"/> is greater than
        /// the upper bound of the range of supported values.</exception>
        public void CheckUpperBound(int daysSinceEpoch)
        {
            if (daysSinceEpoch > _max) Throw.DateOverflow();
        }

        /// <summary>
        /// Checks whether the specified number of consecutive days from the epoch is less than the
        /// lower bound of the range of supported values or not.
        /// </summary>
        /// <exception cref="OverflowException"><paramref name="daysSinceEpoch"/> is less than the
        /// lower bound of the range of supported values.</exception>
        public void CheckLowerBound(int daysSinceEpoch)
        {
            if (daysSinceEpoch < _min) Throw.DateOverflow();
        }
    }
}
