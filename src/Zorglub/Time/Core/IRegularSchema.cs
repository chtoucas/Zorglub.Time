// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    /// <summary>
    /// Defines a calendrical schema with a fixed number of months in a year.
    /// <para>We say that the schema is <i>regular</i>.</para>
    /// </summary>
    /// <remarks>
    /// <para>Most system calendars implement this interface.</para>
    /// </remarks>
    public interface IRegularSchema : ICalendricalKernel
    {
        /// <summary>
        /// Gets the total number of months in a year.
        /// </summary>
        int MonthsInYear { get; }
    }
}
