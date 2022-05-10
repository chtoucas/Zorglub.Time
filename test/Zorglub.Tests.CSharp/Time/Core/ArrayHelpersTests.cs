// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using Zorglub.Time.Core.Utilities;

public static class ArrayHelpersTests
{
    [Fact]
    public static void ConvertToCumulativeArray()
    {
        var array = new[] { 365, 365, 365, 366 };
        var exp = new[] { 0, 365, 2 * 365, 3 * 365, 4 * 365 + 1 };
        // Act
        var actual = ArrayHelpers.ConvertToCumulativeArray(array);
        // Assert
        Assert.Equal(exp, actual);
    }
}
