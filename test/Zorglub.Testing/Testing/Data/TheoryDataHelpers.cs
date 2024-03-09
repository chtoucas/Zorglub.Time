// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Data;

public static class TheoryDataHelpers
{
    [Pure]
    public static TheoryData<int, int> ConvertToArrayData(int[] array)
    {
        Requires.NotNull(array);

        var data = new TheoryData<int, int>();
        for (int i = 0; i < array.Length; i++) { data.Add(i, array[i]); }
        return data;
    }
}
