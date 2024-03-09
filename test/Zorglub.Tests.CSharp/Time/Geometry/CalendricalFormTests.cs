// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Geometry;

using Zorglub.Time.Geometry.Forms;

public static class CalendricalFormTests
{
    [Fact]
    public static void CalendricalForm_InvalidA() =>
        Assert.ThrowsAoorexn("A", () => new CalendricalForm(-1, 1, 1));

    [Fact]
    public static void CalendricalForm_InvalidB() =>
        Assert.ThrowsAoorexn("B", () => new CalendricalForm(1, -1, 1));

    [Fact]
    public static void Deconstructor()
    {
        var form = new CalendricalForm(3, 4, 5);
        // Act
        var (a, b, r) = form;
        // Assert
        Assert.Equal(3, a);
        Assert.Equal(4, b);
        Assert.Equal(5, r);
    }
}
