// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry;

using Zorglub.Time.Geometry.Discrete;

public static class QuasiAffineFormTests
{
    [Fact]
    public static void Constructor_InvalidA() =>
        Assert.ThrowsAoorexn("A", () => new QuasiAffineForm(0, 1, 1));

    [Fact]
    public static void Constructor_InvalidB() =>
        Assert.ThrowsAoorexn("B", () => new QuasiAffineForm(1, 0, 1));

    [Fact]
    public static void Deconstructor()
    {
        // Arrange
        var form = new QuasiAffineForm(3, 4, 5);
        // Act
        var (a, b, r) = form;
        // Assert
        Assert.Equal(3, a);
        Assert.Equal(4, b);
        Assert.Equal(5, r);
    }

    [Fact]
    public static void Init_InvalidA() =>
        Assert.ThrowsAoorexn("value", () => new QuasiAffineForm(1, 1, 1) with { A = 0 });

    [Fact]
    public static void Init_InvalidB() =>
        Assert.ThrowsAoorexn("value", () => new QuasiAffineForm(1, 1, 1) with { B = 0 });
}
