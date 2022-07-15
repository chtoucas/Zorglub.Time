// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    // These interfaces are only meant to be implemented explicitely.

    public interface ISchemaBound
    {
        /// <summary>
        /// Gets the underlying schema.
        /// </summary>
        public ICalendricalSchema Schema { get; }
    }

    public interface ISchemaBound<TSchema>
        where TSchema : ICalendricalSchema
    {
        /// <summary>
        /// Gets the underlying schema.
        /// </summary>
        public TSchema Schema { get; }
    }
}
