// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System.Security.Cryptography;

    public interface IRandomGenerator
    {
        /// <summary>
        /// Generates a random integer between 0 (inclusive) and <paramref name="toExclusive"/>
        /// (exclusive).
        /// </summary>
        int GetInt32(int toExclusive);

        /// <summary>
        /// Generates a random integer between <paramref name="fromInclusive"/> (inclusive) and
        /// <paramref name="toExclusive"/> (exclusive).
        /// </summary>
        int GetInt32(int fromInclusive, int toExclusive);
    }

    internal sealed class DefaultRandomGenerator : IRandomGenerator
    {
        /// <summary>
        /// Generates a random integer between 0 (inclusive) and a specified exclusive upper bound
        /// using a cryptographically strong random number generator.
        /// </summary>
        public int GetInt32(int toExclusive) =>
            RandomNumberGenerator.GetInt32(toExclusive);

        /// <summary>
        /// Generates a random integer between a specified inclusive lower bound and a specified
        /// exclusive upper bound using a cryptographically strong random number generator.
        /// </summary>
        public int GetInt32(int fromInclusive, int toExclusive) =>
            RandomNumberGenerator.GetInt32(fromInclusive, toExclusive);
    }
}
