// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design) 👈 PreviewFeatures
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) 👈 PreviewFeatures

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Defines an affine date.
    /// </summary>
    public interface IAffineDate : IDateable
    {
        /// <summary>
        /// Counts the number of consecutive days from the epoch to the current instance.
        /// </summary>
        [Pure] int CountDaysSinceEpoch();
    }

    /// <summary>
    /// Defines an affine date type.
    /// <para>An affine date is a date type within a calendar system for which the epoch is not
    /// fixed, therefore the dates can not be linked to a timeline.</para>
    /// </summary>
    /// <remarks>
    /// <para>No epoch means no interconversion with other calendars and no day of the week. The
    /// weaker DaysSinceEpoch (the number of consecutive days since the epoch) is still available
    /// which allows for arithmetical operations.</para>
    /// </remarks>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    public interface IAffineDate<TSelf> :
        IAffineDate,
        // Comparison
        IMinMaxFunctions<TSelf>,
        IComparisonOperators<TSelf, TSelf>,
        // Arithmetic
        IStandardArithmetic<TSelf>,
        IAdditionOperators<TSelf, int, TSelf>,
        ISubtractionOperators<TSelf, int, TSelf>,
        IDifferenceOperators<TSelf, int>,
        IIncrementOperators<TSelf>,
        IDecrementOperators<TSelf>
        where TSelf : IAffineDate<TSelf>
    {

        /// <summary>
        /// Creates a new <typeparamref name="TSelf"/> instance in the default calendar from the
        /// specified number of consecutive days since the epoch.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="daysSinceEpoch"/> is outside the range
        /// of values supported by the default calendar.</exception>
        [Pure] static abstract TSelf FromDaysSinceEpoch(int daysSinceEpoch);

        /// <summary>
        /// Subtracts a number of days to the specified date, yielding a new date.
        /// </summary>
        static abstract TSelf operator -(TSelf left, int right);
    }
}
