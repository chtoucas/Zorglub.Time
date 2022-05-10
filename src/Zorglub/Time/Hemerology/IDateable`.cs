// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design) 👈 PreviewFeatures

namespace Zorglub.Time.Hemerology
{
    /// <summary>
    /// Defines a date within a calendar system using the year/month/day subdivision of time.
    /// </summary>
    public interface IDateable
    {
        /// <summary>
        /// Gets the century of the era.
        /// </summary>
        /// <remarks>
        /// <para>A default implementation should look like:
        /// <code><![CDATA[
        ///   Ord CenturyOfEra => Ord.FromInt32(Century);
        /// ]]></code>
        /// </para>
        /// </remarks>
        Ord CenturyOfEra { get; }

        /// <summary>
        /// Gets the century number.
        /// </summary>
        /// <remarks>
        /// <para>A default implementation looks like:
        /// <code><![CDATA[
        ///   int Century => YearNumbering.GetCentury(Year);
        /// ]]></code>
        /// </para>
        /// </remarks>
        int Century { get; }

        /// <summary>
        /// Gets the year of the era.
        /// </summary>
        /// <remarks>
        /// <para>A default implementation looks like:
        /// <code><![CDATA[
        ///   Ord YearOfEra => Ord.FromInt32(Year);
        /// ]]></code>
        /// </para>
        /// </remarks>
        Ord YearOfEra { get; }

        /// <summary>
        /// Gets the year of the century.
        /// <para>The result is in the range from 1 to 100.</para>
        /// </summary>
        /// <remarks>
        /// <para>A default implementation looks like:
        /// <code><![CDATA[
        ///   int YearOfCentury => YearNumbering.GetYearOfCentury(Year);
        /// ]]></code>
        /// </para>
        /// </remarks>
        int YearOfCentury { get; }

        /// <summary>
        /// Gets the (algebraic) year number.
        /// </summary>
        int Year { get; }

        /// <summary>
        /// Gets the month of the year.
        /// </summary>
        int Month { get; }

        /// <summary>
        /// Gets the day of the month.
        /// </summary>
        int Day { get; }

        /// <summary>
        /// Gets the day of the year.
        /// </summary>
        int DayOfYear { get; }

        /// <summary>
        /// Returns true if the current instance is an intercalary day; otherwise returns false.
        /// </summary>
        bool IsIntercalary { get; }

        /// <summary>
        /// Returns true if the current instance is a supplementary day; otherwise returns false.
        /// </summary>
        bool IsSupplementary { get; }

        //
        // Counting
        //

        /// <summary>
        /// Obtains the number of whole days in the year elapsed since the start of the year and
        /// before this date instance.
        /// </summary>
        // Trivial (= DayOfYear - 1), only added for completeness.
        [Pure] int CountElapsedDaysInYear();

        /// <summary>
        /// Obtains the number of whole days remaining until the end of the year.
        /// </summary>
        [Pure] int CountRemainingDaysInYear();

        /// <summary>
        /// Obtains the number of whole days in the year elapsed since the start of the month and
        /// before this date instance.
        /// </summary>
        // Trivial (= Day - 1), only added for completeness.
        [Pure] int CountElapsedDaysInMonth();

        /// <summary>
        /// Obtains the number of whole days remaining until the end of the month.
        /// </summary>
        [Pure] int CountRemainingDaysInMonth();
    }

    /// <summary>
    /// Defines a dateable object type.
    /// </summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    public interface IDateable<TSelf> : IDateable
        where TSelf : IDateable<TSelf>
    {
        //
        // Year and month boundaries
        //
        // Static or not? If not static, property or not?
        // On utilise non pas des propriétés mais des méthodes car en général on
        // ne peut pas dire si le résultat est dans les limites du calendrier
        // sous-jacent, on peut donc être amené à lever une exception.
        // De plus, GetEndOfYear() n'est pas une opération totalement
        // élémentaire. Quant à GetStartOfYear(), pour des questions de symétrie
        // on va aussi opter pour une méthode, même si utiliser une propriété
        // aurait été plus appropriée.

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="day"/> is null.</exception>
        [Pure] static abstract TSelf GetStartOfYear(TSelf day);

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="day"/> is null.</exception>
        [Pure] static abstract TSelf GetEndOfYear(TSelf day);

        /// <summary>
        /// Obtains the first day of the month to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="day"/> is null.</exception>
        [Pure] static abstract TSelf GetStartOfMonth(TSelf day);

        /// <summary>
        /// Obtains the last day of the month to which belongs the specified day.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="day"/> is null.</exception>
        [Pure] static abstract TSelf GetEndOfMonth(TSelf day);
    }
}
