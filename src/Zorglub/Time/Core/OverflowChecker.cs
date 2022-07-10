// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Provides helpers to check for overflows of a range of day-like values.
    /// </summary>
    internal sealed class OverflowChecker : IOverflowChecker<int>
    {
        private readonly int _lowerValue;
        private readonly int _upperValue;

        public OverflowChecker(Range<int> range)
        {
            (_lowerValue, _upperValue) = range.Endpoints;
        }

        public void Check(int value)
        {
            if (value < _lowerValue || value > _upperValue) Throw.DateOverflow();
        }

        public void CheckUpperBound(int value)
        {
            if (value > _upperValue) Throw.DateOverflow();
        }

        public void CheckLowerBound(int value)
        {
            if (value < _lowerValue) Throw.DateOverflow();
        }
    }
}
