// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design)

namespace Zorglub.Time.Hemerology;

using System.Numerics;

// NB: an affine date can only be linked to a single calendar system,
// therefore we can provide unambigiously a factory method FromDaysSinceEpoch().

/// <summary>Defines an affine date.</summary>
public interface IAffineDate : IDateable
{
    /// <summary>Gets the number of consecutive days from the epoch to the current instance.
    /// </summary>
    int DaysSinceEpoch { get; }
}

/// <summary>Defines an affine date type.
/// <para>An affine date is a date type within a calendar system for which the epoch has not been
/// fixed, therefore the dates can not be linked to a timeline.</para>
/// <para>No epoch means no interconversion with other calendars and no day of the week. The weaker
/// DaysSinceEpoch (the number of consecutive days since the epoch) is still available which allows
/// for arithmetical operations.</para></summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IAffineDate<TSelf> :
    IAffineDate,
    // Comparison
    IComparisonOperators<TSelf, TSelf>,
    IMinMaxFunction<TSelf>,
    // Arithmetic
    IStandardArithmetic<TSelf>,
    IAdditionOperators<TSelf, int, TSelf>,
    ISubtractionOperators<TSelf, int, TSelf>,
    IDifferenceOperators<TSelf, int>,
    IIncrementOperators<TSelf>,
    IDecrementOperators<TSelf>
    where TSelf : IAffineDate<TSelf>
{
    /// <summary>Creates a new <typeparamref name="TSelf"/> instance from the specified number of
    /// consecutive days since the epoch.</summary>
    /// <exception cref="AoorException"><paramref name="daysSinceEpoch"/> is outside the range of
    /// values supported by the default calendar.</exception>
    [Pure] static abstract TSelf FromDaysSinceEpoch(int daysSinceEpoch);
}
