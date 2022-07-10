// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    public interface ISchemaBounded
    {
        /// <summary>
        /// Gets the underlying schema.
        /// </summary>
        public ICalendricalSchema Schema { get; }
    }
}
