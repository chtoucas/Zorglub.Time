// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    // TODO(code): randomization.
    // https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca5394

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
