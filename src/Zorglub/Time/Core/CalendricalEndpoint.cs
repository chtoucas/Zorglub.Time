// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    // By keeping this record internal, we can ensure that the properties are
    // coherent, ie that they represent the same day. Furthemore, an endpoint
    // does not keep track of the schema, which makes it incomplete.
    internal sealed record CalendricalEndpoint
    {
        public int DaysSinceEpoch { get; init; }
        public Yemoda DateParts { get; init; }
        public Yedoy OrdinalParts { get; init; }

        // NB: comparison w/ null always returns false, even null >= null and null <= null.
        public static bool operator <(CalendricalEndpoint? left, CalendricalEndpoint? right) =>
            left is not null && right is not null && left.CompareTo(right) < 0;
        public static bool operator <=(CalendricalEndpoint? left, CalendricalEndpoint? right) =>
            left is not null && right is not null && left.CompareTo(right) <= 0;
        public static bool operator >(CalendricalEndpoint? left, CalendricalEndpoint? right) =>
            left is not null && right is not null && left.CompareTo(right) > 0;
        public static bool operator >=(CalendricalEndpoint? left, CalendricalEndpoint? right) =>
            left is not null && right is not null && left.CompareTo(right) >= 0;

        public int CompareTo(CalendricalEndpoint other!!) =>
            DaysSinceEpoch.CompareTo(other.DaysSinceEpoch);
    }
}
