// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Intervals
{
    // We use the french notations for intervals.

    internal static class IntervalFormat
    {
        public const string LeftOpen = "]";
        public const string LeftClosed = "[";

        public const string RightOpen = "[";
        public const string RightClosed = "]";

        // Includes the space after the comma.
        public const string Sep = ", ";

        public const string Unbounded = LeftOpen + "-∞" + Sep + "+∞" + RightOpen;
        public const string LeftUnbounded = LeftOpen + "-∞";
        public const string RightUnbounded = "+∞" + RightOpen;

        public const string Empty = "{}";
    }
}
