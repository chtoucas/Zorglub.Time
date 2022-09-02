// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    public interface IRandomProvider
    {
        int NextInt32();
    }

    internal sealed class DefaultRandomProvider : IRandomProvider
    {
        private readonly Random _random = new();

        public int NextInt32() => _random.Next();
    }
}
