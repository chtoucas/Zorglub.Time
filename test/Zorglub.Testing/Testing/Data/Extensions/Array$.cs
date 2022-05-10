// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Extensions;

/// <summary>
/// Provides extension methods for arrays.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class ArrayExtensions
{
    [Pure]
    public static TheoryData<int, int> ToArrayData(this int[] array!!)
    {
        var data = new TheoryData<int, int>();
        for (int i = 0; i < array.Length; i++) { data.Add(i, array[i]); }
        return data;
    }
}
