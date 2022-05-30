// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using Zorglub.Time.Core.Arithmetic;
using Zorglub.Time.Core.Schemas;

public static class Regular12MathTests
{
    [Fact]
    public static void Constructor_NullSchema() =>
        Assert.ThrowsAnexn("schema", () => new Regular12Math(null!));

    [Fact]
    public static void AdditionRules_Prop()
    {
        var math = new Regular12Math(new GregorianSchema());
        // Act & Assert
        Assert.Equal(default, math.AdditionRules);
    }
}
