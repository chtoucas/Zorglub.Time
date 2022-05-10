// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    /// <summary>
    /// Defines an extended calendrical schema.
    /// </summary>
    public interface ICalendricalSchemaPlus : ICalendricalSchema
    {
        /// <summary>
        /// Obtains the number of whole days remaining after the specified month and until the end
        /// of the year.
        /// </summary>
        [Pure] int CountDaysInYearAfterMonth(int y, int m);

        #region CountDaysInYearBefore()

        /// <summary>
        /// Obtains the number of whole days elapsed since the start of the year and before the
        /// specified day.
        /// </summary>
        // This result should match the value of (DayOfYear - 1).
        [Pure] int CountDaysInYearBefore(int y, int m, int d);

        /// <summary>
        /// Obtains the number of whole days elapsed since the start of the year and before the
        /// specified day.
        /// </summary>
        // Trivial (= doy - 1), only added for completeness.
        [Pure] int CountDaysInYearBefore(int y, int doy);

        /// <summary>
        /// Obtains the number of whole days elapsed since the start of the year and before the
        /// specified day.
        /// </summary>
        // This result should match the value of (DayOfYear - 1).
        [Pure] int CountDaysInYearBefore(int daysSinceEpoch);

        #endregion
        #region CountDaysInYearAfter()

        /// <summary>
        /// Obtains the number of whole days remaining after the specified date and until the end of
        /// the year.
        /// </summary>
        [Pure] int CountDaysInYearAfter(int y, int m, int d);

        /// <summary>
        /// Obtains the number of whole days remaining after the specified date and until the end of
        /// the year.
        /// </summary>
        [Pure] int CountDaysInYearAfter(int y, int doy);

        /// <summary>
        /// Obtains the number of whole days remaining after the specified date and until the end of
        /// the year.
        /// </summary>
        [Pure] int CountDaysInYearAfter(int daysSinceEpoch);

        #endregion
        #region CountDaysInMonthBefore()

        /// <summary>
        /// Obtains the number of whole days elapsed since the start of the month and before the
        /// specified day.
        /// </summary>
        // Trivial (= d - 1), only added for completeness.
        [Pure] int CountDaysInMonthBefore(int y, int m, int d);

        /// <summary>
        /// Obtains the number of whole days elapsed since the start of the month and before the
        /// specified day.
        /// </summary>
        // This result should match the value of (Day - 1).
        [Pure] int CountDaysInMonthBefore(int y, int doy);

        /// <summary>
        /// Obtains the number of whole days elapsed since the start of the month and before the
        /// specified day.
        /// </summary>
        // This result should match the value of (Day - 1).
        [Pure] int CountDaysInMonthBefore(int daysSinceEpoch);

        #endregion
        #region CountDaysInMonthAfter()

        /// <summary>
        /// Obtains the number of whole days remaining after the specified date and until the end of
        /// the month.
        /// </summary>
        [Pure] int CountDaysInMonthAfter(int y, int m, int d);

        /// <summary>
        /// Obtains the number of whole days remaining after the specified date and until the end of
        /// the month.
        /// </summary>
        [Pure] int CountDaysInMonthAfter(int y, int doy);

        /// <summary>
        /// Obtains the number of whole days remaining after the specified date and until the end of
        /// the month.
        /// </summary>
        [Pure] int CountDaysInMonthAfter(int daysSinceEpoch);

        #endregion
    }
}
