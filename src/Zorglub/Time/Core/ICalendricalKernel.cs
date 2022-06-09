// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;

    // La substantifique moelle d'un calendrier.

    /// <summary>
    /// Defines a calendrical schema.
    /// </summary>
    public interface ICalendricalKernel
    {
        //
        // Global characteristics
        //

        /// <summary>
        /// Gets the calendrical algorithm: arithmetical, astronomical or observational.
        /// </summary>
        CalendricalAlgorithm Algorithm { get; }

        /// <summary>
        /// Gets the calendrical family, determined by the astronomical cycle: solar, lunar,
        /// lunisolar...
        /// </summary>
        CalendricalFamily Family { get; }

        /// <summary>
        /// Gets the method employed at regular intervals in order to synchronise the two main
        /// cycles, lunar and solar.
        /// </summary>
        CalendricalAdjustments PeriodicAdjustments { get; }

        /// <summary>
        /// Gets the range of years for which the methods are known not to overflow.
        /// </summary>
        Range<int> SupportedYears { get; }

        /// <summary>
        /// Returns true if this schema is regular; otherwise returns false.
        /// <para>The number of months is given in an output parameter; if this schema is not
        /// regular <paramref name="monthsInYear"/> is set to 0.</para>
        /// </summary>
        [Pure] bool IsRegular(out int monthsInYear);

        //
        // Characteristics
        //

        /// <summary>
        /// Determines whether the specified year is leap or not.
        /// <para>A leap year is a year with at least one intercalary day, week or month.</para>
        /// </summary>
        [Pure] bool IsLeapYear(int y);

        /// <summary>
        /// Determines whether the specified month is intercalary or not.
        /// </summary>
        [Pure] bool IsIntercalaryMonth(int y, int m);

        /// <summary>
        /// Determines whether the specified date is an intercalary day or not.
        /// </summary>
        [Pure] bool IsIntercalaryDay(int y, int m, int d);

        /// <summary>
        /// Determines whether the specified date is a supplementary day or not.
        /// <para>A supplementary day is a day kept outside the intermediary cycles, those shorter
        /// than a year.</para>
        /// </summary>
        /// <remarks>
        /// <para>For technical reasons, we usually attach a supplementary day to the month before.
        /// </para>
        /// <para>A supplementary day may be intercalary too.</para>
        /// <para>An example of such days is given by the epagomenal days which are kept outside any
        /// regular month or decade.</para>
        /// </remarks>
        // By attaching a supplementary day to the preceding month, we differ
        // from NodaTime & others which seem to prefer the creation of a virtual
        // month for holding the supplementary days. Advantages/disadvantages:
        // - CountMonthsInYear() returns the number of months as defined by the
        //   calendar.
        // - CountDaysInMonth() does not always return the actual number of days
        //   in a month.
        // - more importantly, arithmetical ops work without any modification.
        //   For instance, with the simple Egyptian calendar, it always bothered
        //   me that with Nodatime 30/12/1970 + 13 months = 05/13/1970, it seems
        //   to me more sensical to get 30/01/1971.
        [Pure] bool IsSupplementaryDay(int y, int m, int d);

        //
        // Counting
        //

        /// <summary>
        /// Obtains the number of months in the specified year.
        /// </summary>
        [Pure] int CountMonthsInYear(int y);

        /// <summary>
        /// Obtains the number of days in the specified year.
        /// </summary>
        [Pure] int CountDaysInYear(int y);

        /// <summary>
        /// Obtains the number of days in the specified month.
        /// </summary>
        [Pure] int CountDaysInMonth(int y, int m);
    }
}
