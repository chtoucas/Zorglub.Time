// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    /// <summary>
    /// Defines a calendrical validator.
    /// </summary>
    public interface ICalendricalValidator
    {
        /// <summary>
        /// Validates the specified year.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        void ValidateYear(int year, string? paramName = null);

        /// <summary>
        /// Validates the specified month.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        void ValidateYearMonth(int year, int month, string? paramName = null);

        /// <summary>
        /// Validates the specified date.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        void ValidateYearMonthDay(int year, int month, int day, string? paramName = null);

        /// <summary>
        /// Validates the specified ordinal date.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        void ValidateOrdinal(int year, int dayOfYear, string? paramName = null);
    }
}