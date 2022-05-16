// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using System.Linq;

public static class TheoryDataFactories
{
    public static TheoryData<T> Create<T>(IEnumerable<T> source)
    {
        Requires.NotNull(source);

        return new TheoryData_<T>(source);
    }

    public static TheoryData<TResult> Create<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        Requires.NotNull(source);

        var q = source.Select(selector);
        return new TheoryData_<TResult>(q);
    }

    private sealed class TheoryData_<T> : TheoryData<T>
    {
        public TheoryData_(IEnumerable<T> seq)
        {
            Requires.NotNull(seq);

            foreach (T item in seq) { AddRow(item); }
        }
    }
}
