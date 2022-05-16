﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static class TheoryDataHelpers
{
    /// <summary>
    /// Converts a sequence of <typeparamref name="T"/> to a set of data of type
    /// <typeparamref name="T"/>.
    /// </summary>
    [Pure]
    public static TheoryData<T> MapToTheoryData<T>(this IEnumerable<T> @this)
    {
        Requires.NotNull(@this);

        var data = new TheoryData<T>();
        foreach (T a in @this) { data.Add(a); }
        return data;
    }

    /// <summary>
    /// Converts a sequence of <typeparamref name="TSource"/> to a set of data of type
    /// <typeparamref name="TResult"/>.
    /// </summary>
    [Pure]
    public static TheoryData<TResult> MapToTheoryData<TSource, TResult>(
        this IEnumerable<TSource> @this, Func<TSource, TResult> selector)
    {
        Requires.NotNull(@this);

        var data = new TheoryData<TResult>();
        foreach (var item in @this) { data.Add(selector(item)); }
        return data;
    }

    // Two Yemoda's.
    [Pure]
    public static TheoryData<Yemoda, Yemoda> MapToTheoryDataOfTwoYemodas(
        this IEnumerable<(int, int, int, int, int, int)> @this)
    {
        Requires.NotNull(@this);

        var data = new TheoryData<Yemoda, Yemoda>();
        foreach (var (y1, m1, d1, y2, m2, d2) in @this)
        {
            data.Add(new Yemoda(y1, m1, d1), new Yemoda(y2, m2, d2));
        }
        return data;
    }

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
