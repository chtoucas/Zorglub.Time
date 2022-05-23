// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design) 👈 PreviewFeatures

namespace Zorglub.Time.Hemerology
{
    using System;

    public interface IMonthEndpointsProvider<TDate>
    {
        /// <summary>
        /// Obtains the first day of the month to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="day"/> is null.</exception>
        [Pure] static abstract TDate GetStartOfMonth(TDate day);

        /// <summary>
        /// Obtains the last day of the month to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="day"/> is null.</exception>
        [Pure] static abstract TDate GetEndOfMonth(TDate day);
    }
}
