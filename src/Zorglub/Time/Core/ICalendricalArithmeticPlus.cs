// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    /// <summary>
    /// Defines an extended calendrical arithmetic.
    /// </summary>
    public interface ICalendricalArithmeticPlus : ICalendricalArithmetic
    {
        /// <summary>
        /// Adds a number of years to the year field of the specified date.
        /// </summary>
        /// <returns>The end of the target month (resp. year) when the naive result is not a valid
        /// day (resp. month).</returns>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        [Pure] Yemoda AddYears(Yemoda ymd, int years, out int roundoff);

        /// <summary>
        /// Adds a number of months to the specified date.
        /// </summary>
        /// <returns>The last day of the month when the naive result is not a valid day
        /// (roundoff > 0).</returns>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        [Pure] Yemoda AddMonths(Yemoda ymd, int months, out int roundoff);

        /// <summary>
        /// Adds a number of years to the year field of the specified ordinal date.
        /// </summary>
        /// <returns>The last day of the year when the naive result is not a valid day
        /// (roundoff > 0).</returns>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        [Pure] Yedoy AddYears(Yedoy ydoy, int years, out int roundoff);

        /// <summary>
        /// Adds a number of years to the year field of the specified month.
        /// </summary>
        /// <returns>The last month of the year when the naive result is not a valid month
        /// (roundoff > 0).</returns>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        [Pure] Yemo AddYears(Yemo ym, int years, out int roundoff);
    }
}
