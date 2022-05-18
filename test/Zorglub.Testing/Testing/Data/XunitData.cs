// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using System.Collections;
using System.Linq;

public sealed class XunitData<T> : IReadOnlyCollection<object?[]>
{
    private readonly IEnumerable<object?[]> _values;

    public XunitData(IEnumerable<object?[]> values)
    {
        _values = values ?? throw new ArgumentNullException(nameof(values));
    }

    public int Count => _values.Count();

    public IEnumerator<object?[]> GetEnumerator() => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
