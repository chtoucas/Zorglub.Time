// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using System.Collections;

public sealed class XunitData<T> : IReadOnlyCollection<object?[]>
{
    private readonly List<object?[]> _values;

    internal XunitData(List<object?[]> values)
    {
        Requires.NotNull(values);

        _values = values;
    }

    public int Count => _values.Count;

    public IEnumerator<object?[]> GetEnumerator() => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
