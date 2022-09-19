// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities;

/// <summary>Provides static methods for common unsigned mathematical operations.</summary>
/// <remarks>This class cannot be inherited.</remarks>
internal static partial class MathU { }

// CIL code sizes are given for when PATCH_DIVREM is set.
// No plain Divide or Modulo: just use the C# operator / and %.

internal partial class MathU // Division
{
    /// <summary>Calculates the Euclidian quotient of a 32-bit unsigned integer by a non-zero 32-bit
    /// unsigned integer and also returns the remainder in an output parameter.</summary>
    /// <remarks>
    /// <para>When dividing by a constant, performance-wise, it might be better to use the plain C#
    /// operators / and %.</para>
    /// <para>This method does NOT validate its parameters.</para>
    /// <para>The remainder <paramref name="r"/> is in the range from 0 to
    /// (<paramref name="n"/> - 1), both included.</para>
    /// </remarks>
    [Pure]
    // CIL code size = 9 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Divide(uint m, uint n, out uint r)
    {
        Debug.Assert(n > 0);

#if PATCH_DIVREM
        uint q = m / n;
        r = m - q * n;
        return q;
#else
        r = m % n;
        return m / n;
#endif
    }

    /// <summary>Calculates the adjusted Euclidian quotient of a 32-bit unsigned integer by a
    /// non-zero 32-bit unsigned integer.</summary>
    /// <remarks>This method does NOT validate its parameters.</remarks>
    [Pure]
    // CIL code size = 15 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint AdjustedDivide(uint m, uint n)
    {
        Debug.Assert(n != 0);

#if PATCH_DIVREM
        uint q = m / n;
        uint r = m - q * n;
        return r == 0 ? q : q + 1;
#else
        return m % n == 0 ? m / n : (m / n + 1);
#endif
    }

    /// <summary>Calculates the adjusted remainder of the Euclidian division of a 32-bit unsigned
    /// integer by a non-zero 32-bit unsigned integer.</summary>
    /// <remarks>
    /// <para>This method does NOT validate its parameters.</para>
    /// <para>The adjusted remainder is in the range from 1 to
    /// <paramref name="n"/>, both included.</para>
    /// </remarks>
    [Pure]
    // CIL code size = 11 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint AdjustedModulo(uint m, uint n)
    {
        Debug.Assert(n != 0);

        uint mod = m % n;
        return mod == 0 ? n : mod;
    }
}
