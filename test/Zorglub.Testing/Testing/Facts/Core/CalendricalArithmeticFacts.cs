// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Core;

using Zorglub.Testing.Data;

public abstract class CalendricalArithmeticFacts<TDataSet> : ICalendricalArithmeticFacts<TDataSet>
    where TDataSet : ICalendricalDataSet, IMathDataSet, ISingleton<TDataSet>
{
    protected CalendricalArithmeticFacts(ICalendricalSchema schema)
        : base(schema?.Arithmetic ?? throw new ArgumentNullException(nameof(schema)))
    { }
}
