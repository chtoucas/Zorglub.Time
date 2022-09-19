// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities;

/// <summary>Provides static methods for common mathematical operations in ℕ.</summary>
/// <remarks>This class cannot be inherited.</remarks>
internal static partial class MathN { }

// CIL code sizes are given for when PATCH_DIVREM is set.
// No plain Divide or Modulo: just use the C# operator / and %.
// Euclidian division: methods in this class should only be used when
// the dividend is a positive integers (the divisor is always > 0).

internal partial class MathN // Division
{
    /// <summary>Calculates the Euclidian quotient of a positive 32-bit signed integer by a strictly
    /// positive 32-bit signed integer and also returns the remainder in an output parameter.
    /// </summary>
    /// <remarks>
    /// <para>When dividing by a constant, performance-wise, it might be better to use the plain C#
    /// operators / and %.</para>
    /// <para>This method does NOT validate its parameters.</para>
    /// <para>The remainder <paramref name="r"/> is in the range from 0 to
    /// (<paramref name="n"/> - 1), both included.</para>
    /// </remarks>
    [Pure]
    // CIL code size = 13 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Divide(int m, int n, out int r)
    {
        // Two differences with Math.DivRem(): this method is decorated with
        // MethodImplOptions and it expects m and n to be in \N --- remember
        // that, here, we want the Euclidian division...

        Debug.Assert(m >= 0);
        Debug.Assert(n > 0);

#if PATCH_DIVREM
        int q = m / n;
        r = m - q * n;
        return q;
#else
        r = m % n;
        return m / n;
#endif
    }

    /// <summary>Calculates the adjusted Euclidian quotient of a positive 32-bit signed integer by a
    /// strictly positive 32-bit signed integer.</summary>
    /// <remarks>This method does NOT validate its parameters.</remarks>
    [Pure]
    // CIL code size = 17 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AdjustedDivide(int m, int n)
    {
        Debug.Assert(m >= 0);
        Debug.Assert(n > 0);

#if PATCH_DIVREM
        int q = m / n;
        int r = m - q * n;
        return r == 0 ? q : q + 1;
#else
        return m % n == 0 ? m / n : (m / n + 1);
#endif
    }

    /// <summary>Calculates the adjusted remainder of the Euclidian division of a positive 32-bit
    /// signed integer by a strictly positive 32-bit signed integer.</summary>
    /// <remarks>
    /// <para>This method does NOT validate its parameters.</para>
    /// <para>The adjusted remainder is in the range from 1 to <paramref name="n"/>, both
    /// included.</para>
    /// </remarks>
    [Pure]
    // CIL code size = 11 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AdjustedModulo(int m, int n)
    {
        Debug.Assert(m >= 0);
        Debug.Assert(n > 0);

        int mod = m % n;
        return mod == 0 ? n : mod;
    }

    /// <summary>Calculates the Euclidian quotient augmented by 1 of a positive 32-bit signed
    /// integer by a strictly positive 32-bit signed integer and also returns the remainder
    /// augmented by 1 in an output parameter.</summary>
    /// <remarks>
    /// <para>This method does NOT validate its parameters.</para>
    /// <para>The augmented remainder <paramref name="r"/> is in the range from 1 to
    /// (<paramref name="n"/>), both included.</para>
    /// </remarks>
    [Pure]
    // CIL code size = 17 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AugmentedDivide(int m, int n, out int r)
    {
        Debug.Assert(m >= 0);
        Debug.Assert(n > 0);

#if PATCH_DIVREM
        int q = m / n;
        r = 1 + m - q * n;
        return q + 1;
#else
        r = 1 + m % n;
        return m / n + 1;
#endif
    }
}
