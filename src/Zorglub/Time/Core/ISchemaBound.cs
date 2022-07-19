﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    // REVIEW(api): do we really need ISchemaBound<TSchema>?
    // Which type needs to implement ISchemaBound? scopes, validators, segments.

    // These interfaces are only meant to be implemented explicitely.

    public interface ISchemaBound
    {
        /// <summary>
        /// Gets the underlying schema.
        /// </summary>
        public ICalendricalSchema Schema { get; }
    }

    public interface ISchemaBound<out TSchema>
        where TSchema : ICalendricalSchema
    {
        /// <summary>
        /// Gets the underlying schema.
        /// </summary>
        public TSchema Schema { get; }
    }
}
