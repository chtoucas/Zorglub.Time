// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Extensions;

using System.Linq;

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static class TheoryDataExtensions
{
    // Two Yemoda's, one unit.
    [Pure]
    public static TheoryData<Yemoda, Yemoda, T> MapToTheoryDataOfTwoYemodas<T>(
        this IEnumerable<(int, int, int, int, int, int, T)> @this)
    {
        Requires.NotNull(@this);

        var data = new TheoryData<Yemoda, Yemoda, T>();
        foreach (var (y1, m1, d1, y2, m2, d2, t) in @this)
        {
            data.Add(new Yemoda(y1, m1, d1), new Yemoda(y2, m2, d2), t);
        }
        return data;
    }

    // Two Yemoda's, three units.
    [Pure]
    public static TheoryData<Yemoda, Yemoda, T1, T2, T3> MapToTheoryDataOfTwoYemodas<T1, T2, T3>(
        this IEnumerable<(int, int, int, int, int, int, T1, T2, T3)> @this)
    {
        Requires.NotNull(@this);

        var data = new TheoryData<Yemoda, Yemoda, T1, T2, T3>();
        foreach (var (y1, m1, d1, y2, m2, d2, t1, t2, t3) in @this)
        {
            data.Add(new Yemoda(y1, m1, d1), new Yemoda(y2, m2, d2), t1, t2, t3);
        }
        return data;
    }
}
