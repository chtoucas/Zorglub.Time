// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities;

// REVIEW(perf): do we still need to patch div-rem?
// Only meaningful when we need to calculate both quotient and remainder.
// Instead of
// > q = m / n;
// > r = m % n;
// it could be better to write
// > q = m / n
// > r = m - q * n
// A multiplication is always faster than a modulo but the CIL code is a bit
// larger. To be honest, I can't seem to be able to measure the difference
// between the two myself.
// https://github.com/dotnet/runtime/issues/5213
// https://github.com/dotnet/runtime/issues/4155
// https://source.dot.net/#System.Private.CoreLib/Math.cs,368

/// <summary>Provides static methods for common mathematical operations in ℤ.</summary>
/// <remarks>This class cannot be inherited.</remarks>
internal static partial class MathZ { }

// CIL code sizes are given for when __PATCH_DIVREM__ is set.
// Euclidian division: methods in this class should only be used when the
// dividend could be a negative integer (the divisor is always > 0).

#region Euclidian division (doc)

// When the dividend is a negative integer, the C# division operator
// is a division with negative remainder. The methods here implement the
// standard Euclidian division by ensuring that the remainder is always an
// integer in the range from 0 to the divisor (excluded). Similarly,
// Math.DivRem does not follow the mathematical rules when the dividend is
// negative.
//
// For n a power of 2, n = 2^k = (1 << k), one can simply write:
//   q = m / n = m >> k
//   r = m [n] = m & (n - 1)
// and it works whether m is positive or negative. For instance,
//   q = m / 2  = m >> 1
//   r = m [2] = m & 1
// but also
//   q = m / 4  = m >> 2
//   r = m [4] = m & 3
// and
//   q = m / 8  = m >> 3
//   r = m [8] = m & 7
//
// Division euclidienne dans \N
// ----------------------------
// Si m, n dans \N, n != 0, alors il existe q, r dans \N (uniques) tels
// que :
//   m = q n + r
//   0 <= r < n
// En .NET, pour obtenir q et r, il suffit d'écrire :
//   [C#] q = m / n et r = m % n
//
// Division d'un entier relatif par un entier naturel non nul
// ----------------------------------------------------------
// Si m dans \Z et n dans \N, n!= 0, alors il existe q dans \Z et r dans
// \N (uniques) tels que :
//   m = q n + r
//   0 <= r < n
// Malheureusement, si m < 0, .NET utilise la version avec reste
// négatif : [C#] q' = m / n et r' = m % n correspond à
//   m = q' n + r'
//   -n < r' <= 0
// néanmoins récupérer r et n n'est pas bien difficile.
// Si r' = 0, il suffit de prendre r = 0 et q = q', sinon
//   q = q' - 1
//   r = r' + n
// pour le voir écrire m = (q' - 1) n + (r' + n), puis tirer avantage
// de l'unicité de la décomposition.
// Pour m < 0 et m % n != 0, le couple recherché est donc donné par :
//   q = (m / n) - 1
//   r = (m % n) + n
// si m < 0 et m % n = 0, alors q = m / n et r = 0.
//
// On ne s'intéresse pas à la division euclidienne dans \Z ; on y perd
// d'ailleurs l'unicité de la solution (au plus deux).
//
// Un autre manière (moins efficace) consiste à utiliser un Double ou un
// Decimal. Par exemple, q = Math.Floor((double)m/n).

#endregion

internal partial class MathZ // Divide
{
    /// <summary>Calculates the Euclidian quotient of a 32-bit signed integer by a strictly positive
    /// 32-bit signed integer.</summary>
    /// <remarks>This method does NOT validate its parameters.</remarks>
    [Pure]
    // CIL code size = 23 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Divide(int m, int n)
    {
        Debug.Assert(n > 0);

#if __PATCH_DIVREM__
        int q = m / n;
        int r = m - q * n;
        return m >= 0 || r == 0 ? q : q - 1;
#else
        return m >= 0 || m % n == 0 ? m / n : (m / n - 1);
#endif
    }

    /// <summary>Calculates the Euclidian quotient of a 32-bit signed integer by a strictly positive
    /// 32-bit signed integer and also returns the remainder in an output parameter.</summary>
    /// <remarks>
    /// <para>This method does NOT validate its parameters.</para>
    /// <para>The remainder <paramref name="r"/> is in the range from 0 to
    /// (<paramref name="n"/> - 1), both included.</para>
    /// </remarks>
    [Pure]
    // CIL code size = 31 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Divide(int m, int n, out int r)
    {
        Debug.Assert(n > 0);

        int q = m / n;
#if __PATCH_DIVREM__
        r = m - q * n;
#else
        r = m % n;
#endif
        if (m >= 0 || r == 0)
        {
            return q;
        }
        else
        {
            r += n;
            return q - 1;
        }
    }
}

internal partial class MathZ // Divide (long)
{
    /// <summary>Calculates the Euclidian quotient of a 64-bit signed integer by a strictly positive
    /// 64-bit signed integer.</summary>
    /// <remarks>This method does NOT validate its parameters.</remarks>
    [Pure]
    // CIL code size = 25 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Divide(long m, long n)
    {
        Debug.Assert(n > 0);

#if __PATCH_DIVREM__
        long q = m / n;
        long r = m - q * n;
        return m >= 0L || r == 0L ? q : q - 1L;
#else
        return m >= 0L || m % n == 0L ? m / n : (m / n - 1L);
#endif
    }

    /// <summary>Calculates the Euclidian quotient of a 64-bit signed integer by a strictly positive
    /// 64-bit signed and also returns the remainder in an output parameter.</summary>
    /// <remarks>
    /// <para>This method does NOT validate its parameters.</para>
    /// <para>The remainder <paramref name="r"/> is in the range from 0 to
    /// (<paramref name="n"/> - 1), both included.</para>
    /// </remarks>
    [Pure]
    // CIL code size = 33 bytes, can be > 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Divide(long m, long n, out long r)
    {
        Debug.Assert(n > 0);

        long q = m / n;
#if __PATCH_DIVREM__
        r = m - q * n;
#else
        r = m % n;
#endif
        if (m >= 0 || r == 0)
        {
            return q;
        }
        else
        {
            r += n;
            return q - 1;
        }
    }
}

internal partial class MathZ // Modulo
{
    /// <summary>Calculates the remainder of the Euclidian division of a 32-bit signed integer by a
    /// strictly positive 32-bit signed integer.</summary>
    /// <remarks>
    /// <para>This method does NOT validate its parameters.</para>
    /// <para>The remainder is in the range from 0 to (<paramref name="n"/> - 1), both included.
    /// </para>
    /// </remarks>
    [Pure]
    // CIL code size = 14 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Modulo(int m, int n)
    {
        Debug.Assert(n > 0);

        int r = m % n;
        return r >= 0 ? r : (r + n);
    }

    /// <summary>Calculates the remainder of the Euclidian division of a 32-bit signed integer by a
    /// strictly positive 32-bit signed integer and also returns the quotient in an output parameter.
    /// </summary>
    /// <remarks>
    /// <para>This method does NOT validate its parameters.</para>
    /// <para>This is equivalent to <see cref="Divide(int, int, out int)"/> but sometimes it just
    /// feels more natural to use a modulo.</para>
    /// </remarks>
    [Pure]
    // CIL code size = 31 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Modulo(int i, int n, out int q)
    {
        Debug.Assert(n > 0);

        q = i / n;
#if __PATCH_DIVREM__
        int r = i - q * n;
#else
        int r = i % n;
#endif
        if (i < 0 && r != 0)
        {
            q--;
            r += n;
        }
        return r;
    }
}

internal partial class MathZ // Modulo (long)
{
    /// <summary>Calculates the remainder of the Euclidian division of a 64-bit signed integer by a
    /// strictly positive 64-bit signed integer.</summary>
    /// <remarks>
    /// <para>This method does NOT validate its parameters.</para>
    /// <para>The remainder is in the range from 0 to (<paramref name="n"/> - 1), both included.
    /// </para>
    /// </remarks>
    [Pure]
    // CIL code size = 15 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Modulo(long m, long n)
    {
        Debug.Assert(n > 0);

        long r = m % n;
        return r >= 0 ? r : (r + n);
    }
}

internal partial class MathZ // AdjustedDivide
{
    /// <summary>Calculates the adjusted Euclidian quotient of a 32-bit signed integer by a strictly
    /// positive 32-bit signed integer.</summary>
    /// <remarks>This method does NOT validate its parameters.</remarks>
    [Pure]
    // CIL code size = 23 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AdjustedDivide(int m, int n)
    {
        Debug.Assert(n > 0);

#if __PATCH_DIVREM__
        int q = m / n;
        int r = m - q * n;
        return m < 0 || r == 0 ? q : q + 1;
#else
        return m < 0 || m % n == 0 ? m / n : (m / n + 1);
#endif
    }

    /// <summary>Calculates the adjusted Euclidian quotient of a 32-bit signed integer by a strictly
    /// positive 32-bit signed integer and also returns the adjusted remainder in an output
    /// parameter.</summary>
    /// <remarks>
    /// <para>This method does NOT validate its parameters.</para>
    /// <para>The adjusted remainder <paramref name="r"/> is in the range from 1 to
    /// <paramref name="n"/>, both included.</para>
    /// </remarks>
    [Pure]
    // CIL code size = 31 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AdjustedDivide(int m, int n, out int r)
    {
        Debug.Assert(n > 0);

        int q = m / n;
#if __PATCH_DIVREM__
        r = m - q * n;
#else
        r = m % n;
#endif

        if (m < 0 || r == 0)
        {
            r += n;
            return q;
        }
        else
        {
            return q + 1;
        }
    }
}

internal partial class MathZ // AdjustedModulo
{
    // Opération modulo rectifiée ("adjusted").

    /// <summary>Calculates the adjusted remainder of the Euclidian division of a 32-bit signed
    /// integer by a strictly positive 32-bit signed integer.</summary>
    /// <remarks>
    /// <para>This method does NOT validate its parameters.</para>
    /// <para>The adjusted remainder is in the range from 1 to <paramref name="n"/>, both included.
    /// </para>
    /// </remarks>
    [Pure]
    // CIL code size = 22 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AdjustedModulo(int m, int n)
    {
        Debug.Assert(n > 0);

        int r = m % n;
        int mod = r >= 0 ? r : (r + n);
        return mod == 0 ? n : mod;
    }
}

internal partial class MathZ // AdjustedModulo (long)
{
    /// <summary>Calculates the adjusted remainder of the Euclidian division of a 64-bit signed
    /// integer by a strictly positive 64-bit signed integer.</summary>
    /// <remarks>
    /// <para>This method does NOT validate its parameters.</para>
    /// <para>The adjusted remainder is in the range from 1 to
    /// <paramref name="n"/>, both included.</para>
    /// </remarks>
    [Pure]
    // CIL code size = 23 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long AdjustedModulo(long m, long n)
    {
        Debug.Assert(n > 0);

        long r = m % n;
        long mod = r >= 0 ? r : (r + n);
        return mod == 0 ? n : mod;
    }
}

internal partial class MathZ // AugmentedDivide
{
    /// <summary>Calculates the Euclidian quotient augmented by 1 of a 32-bit signed integer by a
    /// strictly positive 32-bit signed integer.</summary>
    /// <remarks>This method does NOT validate its parameters.</remarks>
    [Pure]
    // CIL code size = 23 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AugmentedDivide(int m, int n)
    {
        Debug.Assert(n > 0);

#if __PATCH_DIVREM__
        int q = m / n;
        int r = m - q * n;
        return m >= 0 || r == 0 ? q + 1 : q;
#else
        return m >= 0 || m % n == 0 ? (m / n + 1) : m / n;
#endif
    }

    /// <summary>Calculates the Euclidian quotient augmented by 1 of a 32-bit signed integer by a
    /// strictly positive 32-bit signed integer and also returns the remainder augmented by 1 in an
    /// output parameter.</summary>
    /// <remarks>
    /// <para>This method does NOT validate its parameters.</para>
    /// <para>The augmented remainder <paramref name="r"/> is in the range from 1 to
    /// (<paramref name="n"/>), both included.</para>
    /// </remarks>
    [Pure]
    // CIL code size = 34 bytes > 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AugmentedDivide(int m, int n, out int r)
    {
        Debug.Assert(n > 0);

        int q = m / n;
#if __PATCH_DIVREM__
        r = 1 + m - q * n;
#else
        r = 1 + m % n;
#endif
        if (m >= 0 || r == 1)
        {
            return q + 1;
        }
        else
        {
            r += n;
            return q;
        }
    }
}
