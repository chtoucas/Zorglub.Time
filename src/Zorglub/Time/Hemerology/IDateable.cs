// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Hemerology;

/// <summary>Defines a date within a calendar system using the year/month/day subdivision of time.
/// </summary>
public interface IDateable
{
    /// <summary>Gets the century of the era.</summary>
    /// <remarks>
    /// <para>A default implementation should look like:
    /// <code><![CDATA[
    ///   Ord CenturyOfEra => Ord.FromInt32(Century);
    /// ]]></code>
    /// </para>
    /// </remarks>
    Ord CenturyOfEra { get; }

    /// <summary>Gets the century number.</summary>
    /// <remarks>
    /// <para>A default implementation looks like:
    /// <code><![CDATA[
    ///   int Century => YearNumbering.GetCentury(Year);
    /// ]]></code>
    /// </para>
    /// </remarks>
    int Century { get; }

    /// <summary>Gets the year of the era.</summary>
    /// <remarks>
    /// <para>A default implementation looks like:
    /// <code><![CDATA[
    ///   Ord YearOfEra => Ord.FromInt32(Year);
    /// ]]></code>
    /// </para>
    /// </remarks>
    Ord YearOfEra { get; }

    /// <summary>Gets the year of the century.</summary>
    /// <remarks>
    /// <para>The result is in the range from 1 to 100.</para>
    /// <para>A default implementation looks like:
    /// <code><![CDATA[
    ///   int YearOfCentury => YearNumbering.GetYearOfCentury(Year);
    /// ]]></code>
    /// </para>
    /// </remarks>
    int YearOfCentury { get; }

    /// <summary>Gets the (algebraic) year number.</summary>
    int Year { get; }

    /// <summary>Gets the month of the year.</summary>
    int Month { get; }

    /// <summary>Gets the day of the month.</summary>
    int Day { get; }

    /// <summary>Gets the day of the year.</summary>
    int DayOfYear { get; }

    /// <summary>Returns true if the current instance is an intercalary day; otherwise returns false.
    /// </summary>
    bool IsIntercalary { get; }

    /// <summary>Returns true if the current instance is a supplementary day; otherwise returns
    /// false.</summary>
    bool IsSupplementary { get; }

    /// <summary>Deconstructs the current instance into its components.</summary>
    void Deconstruct(out int year, out int month, out int day);

    /// <summary>Deconstructs the current instance into its ordinal components.</summary>
    void Deconstruct(out int year, out int dayOfYear);

    /// <summary>Obtains the number of whole days in the year elapsed since the start of the year
    /// and before this date instance.</summary>
    // Trivial (= DayOfYear - 1), only added for completeness.
    [Pure] int CountElapsedDaysInYear();

    /// <summary>Obtains the number of whole days remaining until the end of the year.</summary>
    [Pure] int CountRemainingDaysInYear();

    /// <summary>Obtains the number of whole days in the year elapsed since the start of the month
    /// and before this date instance.</summary>
    // Trivial (= Day - 1), only added for completeness.
    [Pure] int CountElapsedDaysInMonth();

    /// <summary>Obtains the number of whole days remaining until the end of the month.</summary>
    [Pure] int CountRemainingDaysInMonth();
}
