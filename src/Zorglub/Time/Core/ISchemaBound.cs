﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    // Only meant to be implemented explicitely.

    public interface ISchemaBound
    {
        /// <summary>
        /// Gets the underlying schema.
        /// </summary>
        public ICalendricalSchema Schema { get; }
    }
}