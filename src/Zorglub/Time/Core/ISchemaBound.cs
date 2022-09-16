// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    // REVIEW(api): which type needs to implement ISchemaBound? segments,
    // validators and scopes.

    // This interface is only meant to be implemented explicitely.

    public interface ISchemaBound
    {
        /// <summary>
        /// Gets the underlying schema.
        /// </summary>
        ICalendricalSchema Schema { get; }
    }
}
