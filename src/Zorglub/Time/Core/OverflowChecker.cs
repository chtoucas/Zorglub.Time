// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Provides helpers to check for overflows of a range of <i>date-like values</i>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed class OverflowChecker : IOverflowChecker<int>
    {
        private readonly int _lowerValue;
        private readonly int _upperValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="OverflowChecker"/> class.
        /// </summary>
        public OverflowChecker(Range<int> range)
        {
            (_lowerValue, _upperValue) = range.Endpoints;
        }

        /// <inheritdoc/>
        public void Check(int value)
        {
            if (value < _lowerValue || value > _upperValue) Throw.DateOverflow();
        }

        /// <inheritdoc/>
        public void CheckUpperBound(int value)
        {
            if (value > _upperValue) Throw.DateOverflow();
        }

        /// <inheritdoc/>
        public void CheckLowerBound(int value)
        {
            if (value < _lowerValue) Throw.DateOverflow();
        }
    }
}
