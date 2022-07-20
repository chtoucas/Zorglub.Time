// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System.Diagnostics.Contracts;

using Zorglub.Time.Core;

using static Zorglub.Time.Extensions.Unboxing;

internal static class SchemaActivator
{
    /// <summary>
    /// Creates a new instance of the <typeparamref name="TSchema"/> class.
    /// </summary>
    [Pure]
    public static TSchema CreateInstance<TSchema>()
        where TSchema : class, ICalendricalSchema, IBoxable<TSchema>
    {
        return TSchema.GetInstance().Unbox();
    }
}
