// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry;

using Zorglub.Time.Geometry.Discrete;

public static partial class CodeArrayTests
{
    [Fact]
    public static void Constructor_NullArr() =>
        Assert.ThrowsAnexn("codes", () => new CodeArray(null!));

    [Fact]
    public static void Constructor_EmptyArr() =>
        Assert.Throws<ArgumentException>("codes", () => new CodeArray(Array.Empty<int>()));

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(99)]
    [InlineData(999)]
    public static void Singleton(int n)
    {
        var code = new CodeArray(n);
        var barr = new BoolArray(false);
        // Act & Assert
        Assert.True(code.Constant);
        Assert.True(code.Reducible);
        Assert.False(code.StrictlyReducible);
        Assert.Equal(n, code.Min);
        Assert.Equal(n, code.Max);
        Assert.Single(code);
        Assert.Equal(barr, code.ToBoolArray());
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(1, 3)]
    [InlineData(5, 2)]
    [InlineData(1, 99)]
    [InlineData(999, 345)]
    public static void Pair_NotSegment(int m, int n)
    {
        var code = new CodeArray(new[] { m, n });
        // Act & Assert
        Assert.False(code.Constant);
        Assert.False(code.Reducible);
        Assert.False(code.StrictlyReducible);
        Assert.Equal(Math.Min(m, n), code.Min);
        Assert.Equal(Math.Max(m, n), code.Max);
        Assert.Equal(2, code.Count);
        Assert.Throws<InvalidOperationException>(() => code.ToBoolArray());
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(3, 2)]
    [InlineData(2, 3)]
    [InlineData(99, 98)]
    [InlineData(98, 99)]
    public static void Pair_ConsecutiveElements(int m, int n)
    {
        var code = new CodeArray(new[] { m, n });
        var barr = m > n ? new BoolArray(new[] { true, false })
            : new BoolArray(new[] { false, true });
        // Act & Assert
        Assert.False(code.Constant);
        Assert.True(code.Reducible);
        Assert.True(code.StrictlyReducible);
        Assert.Equal(Math.Min(m, n), code.Min);
        Assert.Equal(Math.Max(m, n), code.Max);
        Assert.Equal(2, code.Count);
        Assert.Equal(barr, code.ToBoolArray());
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(99, 99)]
    [InlineData(999, 999)]
    public static void Pair_SameElements(int m, int n)
    {
        var code = new CodeArray(new[] { m, n });
        var barr = new BoolArray(new[] { false, false });
        // Act & Assert
        Assert.True(code.Constant);
        Assert.True(code.Reducible);
        Assert.False(code.StrictlyReducible);
        Assert.Equal(n, code.Min);
        Assert.Equal(n, code.Max);
        Assert.Equal(2, code.Count);
        Assert.Equal(barr, code.ToBoolArray());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(100)]
    public static void Constant(int count)
    {
        var code = new CodeArray(2, count);
        var barr = new BoolArray(false, count);
        // Act & Assert
        Assert.True(code.Constant);
        Assert.True(code.Reducible);
        Assert.False(code.StrictlyReducible);
        Assert.Equal(2, code.Min);
        Assert.Equal(2, code.Max);
        Assert.Equal(count, code.Count);
        Assert.Equal(barr, code.ToBoolArray());
    }
}

// IsAlmostReducible().
public partial class CodeArrayTests
{
    [Fact]
    public static void IsAlmostReducible_NonReduciblePair()
    {
        var code = new CodeArray(new[] { 5, 1 });
        var exp = new CodeArray(new[] { 5 });
        // Act
        var isAlmostReducible = code.IsAlmostReducible(out var newCode, out int start);
        // Assert
        Assert.True(isAlmostReducible);
        Assert.Equal(exp, newCode);
        Assert.Equal(0, start);

        Assert.Equal(code, exp.Append(code[^1]));
    }

    [Fact]
    public static void IsAlmostReducible_MaxAtStart()
    {
        var code = new CodeArray(new[] { 6, 2, 3, 3, 2 });
        var exp = new CodeArray(new[] { 2, 3, 3, 2 });
        // Act
        var isAlmostReducible = code.IsAlmostReducible(out var newCode, out int start);
        // Assert
        Assert.True(isAlmostReducible);
        Assert.Equal(exp, newCode);
        Assert.Equal(1, start);

        int i = start - 1;
        Assert.Equal(code, exp.Append(code[i]).Rotate(exp.Count - i));
    }

    [Fact]
    public static void IsAlmostReducible_MaxInTheMiddle()
    {
        var code = new CodeArray(new[] { 2, 3, 6, 3, 2 });
        var exp = new CodeArray(new[] { 3, 2, 2, 3 });
        // Act
        var isAlmostReducible = code.IsAlmostReducible(out var newCode, out int start);
        // Assert
        Assert.True(isAlmostReducible);
        Assert.Equal(exp, newCode);
        Assert.Equal(3, start);

        int i = start - 1;
        Assert.Equal(code, exp.Append(code[i]).Rotate(exp.Count - i));
    }

    [Fact]
    public static void IsAlmostReducible_MaxAtEnd()
    {
        var code = new CodeArray(new[] { 2, 3, 3, 2, 6 });
        var exp = new CodeArray(new[] { 2, 3, 3, 2 });
        // Act
        var isAlmostReducible = code.IsAlmostReducible(out var newCode, out int start);
        // Assert
        Assert.True(isAlmostReducible);
        Assert.Equal(exp, newCode);
        Assert.Equal(0, start);

        Assert.Equal(code, exp.Append(code[^1]));
    }

    [Fact]
    public static void IsAlmostReducible_MinAtStart()
    {
        var code = new CodeArray(new[] { 1, 2, 3, 3, 2 });
        var exp = new CodeArray(new[] { 2, 3, 3, 2 });
        // Act
        var isAlmostReducible = code.IsAlmostReducible(out var newCode, out int start);
        // Assert
        Assert.True(isAlmostReducible);
        Assert.Equal(exp, newCode);
        Assert.Equal(1, start);

        int i = start - 1;
        Assert.Equal(code, exp.Append(code[i]).Rotate(exp.Count - i));
    }

    [Fact]
    public static void IsAlmostReducible_MinInTheMiddle()
    {
        var code = new CodeArray(new[] { 2, 3, 1, 3, 2 });
        var exp = new CodeArray(new[] { 3, 2, 2, 3 });
        // Act
        var isAlmostReducible = code.IsAlmostReducible(out var newCode, out int start);
        // Assert
        Assert.True(isAlmostReducible);
        Assert.Equal(exp, newCode);
        Assert.Equal(3, start);

        int i = start - 1;
        Assert.Equal(code, exp.Append(code[i]).Rotate(exp.Count - i));
    }

    [Fact]
    public static void IsAlmostReducible_MinAtEnd()
    {
        var code = new CodeArray(new[] { 2, 3, 3, 2, 1 });
        var exp = new CodeArray(new[] { 2, 3, 3, 2 });
        // Act
        var isAlmostReducible = code.IsAlmostReducible(out var newCode, out int start);
        // Assert
        Assert.True(isAlmostReducible);
        Assert.Equal(exp, newCode);
        Assert.Equal(0, start);

        Assert.Equal(code, exp.Append(code[^1]));
    }

    [Fact]
    public static void IsAlmostReducible_AlmostConstant_AtStart()
    {
        var code = new CodeArray(new[] { 7, 2, 2, 2, 2, 2 });
        var exp = new CodeArray(new[] { 2, 2, 2, 2, 2 });
        // Act
        var isAlmostReducible = code.IsAlmostReducible(out var newCode, out int start);
        // Assert
        Assert.True(isAlmostReducible);
        Assert.Equal(exp, newCode);
        Assert.Equal(1, start);

        int i = start - 1;
        Assert.Equal(code, exp.Append(code[i]).Rotate(exp.Count - i));
    }

    [Fact]
    public static void IsAlmostReducible_AlmostConstant()
    {
        var code = new CodeArray(new[] { 2, 2, 2, 2, 7, 2 });
        var exp = new CodeArray(new[] { 2, 2, 2, 2, 2 });
        // Act
        var isAlmostReducible = code.IsAlmostReducible(out var newCode, out int start);
        // Assert
        Assert.True(isAlmostReducible);
        Assert.Equal(exp, newCode);
        Assert.Equal(5, start);

        int i = start - 1;
        Assert.Equal(code, exp.Append(code[i]).Rotate(exp.Count - i));
    }

    [Fact]
    public static void IsAlmostReducible_AlmostConstant_AtEnd()
    {
        var code = new CodeArray(new[] { 3, 3, 3, 3, 3, 1 });
        var exp = new CodeArray(new[] { 3, 3, 3, 3, 3 });
        // Act
        var isAlmostReducible = code.IsAlmostReducible(out var newCode, out int start);
        // Assert
        Assert.True(isAlmostReducible);
        Assert.Equal(exp, newCode);
        Assert.Equal(0, start);

        Assert.Equal(code, exp.Append(code[^1]));
    }

    [Fact]
    public static void IsAlmostReducible_KO()
    {
        // Reducible
        test(new[] { 2 });
        test(new[] { 2, 2 });
        test(new[] { 2, 3 });
        test(new[] { 2, 2, 2, 2, 2, 2 });
        test(new[] { 2, 3, 3, 2, 2, 3 });

        // Other non-reducible codes.
        test(new[] { 1, 3, 5 });
        test(new[] { 1, 2, 3, 4 });
        test(new[] { 2, 3, 5, 3, 2, 6 });

        static void test(int[] arr)
        {
            var code = new CodeArray(arr);
            // Act
            var isAlmostReducible = code.IsAlmostReducible(out var newCode, out int start);
            // Assert
            Assert.False(isAlmostReducible);
            Assert.Null(newCode);
            Assert.Equal(-1, start);
        }
    }
}
