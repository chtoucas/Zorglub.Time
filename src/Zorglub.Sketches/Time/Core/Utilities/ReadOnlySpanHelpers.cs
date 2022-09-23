// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities;

/// <summary>
/// Provides helpers for <see cref="ReadOnlySpan{T}"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static class ReadOnlySpanHelpers
{
    [Pure]
    public static int[] Rotate(ReadOnlySpan<ushort> span, int start)
    {
        // Not mandatory, but then we would have to use the true math modulo.
        // We also exclude 0, since Rotate() would do nothing.
        Debug.Assert(start > 0);
        // This one too is not mandatory, but it seems more natural to
        // satisfy this condition too.
        Debug.Assert(start < span.Length);

        int len = span.Length;
        var arr = new int[len];
        for (int i = 0; i < len; i++)
        {
            arr[i] = span[(i + start) % len];
        }
        return arr;
    }

    [Pure]
    public static int[] ConvertToCumulativeArray(ReadOnlySpan<byte> span)
    {
        var arr = new int[span.Length];
        int count = 0;
        for (int i = 0; i < span.Length; i++)
        {
            arr[i] = count;
            count += span[i];
        }
        return arr;
    }
}
