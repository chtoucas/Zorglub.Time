// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    /// <summary>
    /// Defines a calendrical schema or a calendar with a fixed number of months in a year.
    /// <para>We say that the schema/calendar is <i>regular</i>.</para>
    /// </summary>
    /// <remarks>
    /// <para>Most calendars implement this interface.</para>
    /// </remarks>
    public interface IRegularFeaturette : ICalendricalKernel
    {
        /// <summary>
        /// Gets the total number of months in a year.
        /// </summary>
        int MonthsInYear { get; }
    }
}
