// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    /// <summary>
    /// Defines a calendrical schema or a calendar with a virtual month.
    /// </summary>
    public interface IVirtualMonthFeaturette : ICalendricalKernel
    {
        /// <summary>
        /// Gets the virtual month.
        /// </summary>
        int VirtualMonth { get; }
    }
}
