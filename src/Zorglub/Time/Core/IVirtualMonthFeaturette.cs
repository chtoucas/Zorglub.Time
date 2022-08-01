// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    // REVIEW(api): static property or not?

    /// <summary>
    /// Defines a schema with a virtual month.
    /// </summary>
    public interface IVirtualMonthFeaturette
    {
        /// <summary>
        /// Gets the virtual month.
        /// </summary>
        static abstract int VirtualMonth { get; }
    }
}
