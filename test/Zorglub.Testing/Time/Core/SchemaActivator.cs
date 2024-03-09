// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Core;

using static Zorglub.Time.Extensions.Unboxing;

public static class SchemaActivator
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
