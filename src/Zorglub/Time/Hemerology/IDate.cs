// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design)

namespace Zorglub.Time.Hemerology;

using System.Numerics;

// TODO(api): add FromDayNumber() to IDate<TSelf, out TCalendar>?
// Almost done, we still have to decide whether to keep it or not in simple
// date types.
// I'm not sure yet. We can achieve the same thing using one of the factory
// methods on SimpleCalendar. Two reasons to keep it: constructors are also
// specialized and optimised for the Gregorian case.
// We will definitely keep it for date types not linked to a calendar type.

// A date type is expected to provide a constructor or factory for the
// following parameters:
// - (y, m, d)
// - (y, doy)
// - (dayNumber)

/// <summary>Defines a date.</summary>
public interface IDate : IFixedDate, IDateable { }

/// <summary>Defines a date type.</summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IDate<TSelf> :
    IDate,
    IFixedDate<TSelf>,
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
    where TSelf : IDate<TSelf>
{ }

// L'interface suivante est prévue pour les dates ne fonctionnant qu'avec un
// seul calendrier, d'où le fait d'avoir choisi des propriétés et méthodes
// __statiques__.
// Pour des dates fonctionnant avec un calendrier "pluriel", on utilisera
// plutôt une propriété non-statique Calendar et on ajoutera une méthode pour
// WithCalendar(newCalendar) pour l'interconversion; voir p.ex. ZDate et
// ISimpleDate.

// FIXME(api): remove IDate`2...
// UtcToday() and Today(ITimepiece).
// XXXClock in Specialized.
// Debug.Assert(daysSinceEpoch).

/// <summary>Defines a date type with a companion calendar.</summary>
/// <remarks>This interface SHOULD NOT be implemented by date types participating in a poly-calendar
/// system.</remarks>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
/// <typeparam name="TCalendar">The companion calendar type.</typeparam>
public interface IDate<TSelf, out TCalendar> :
    IDate<TSelf>,
    IMinMaxValue<TSelf>
    where TCalendar : ICalendar<TSelf>
    where TSelf : IDate<TSelf, TCalendar>
{
    /// <summary>Gets the calendar to which belongs the current instance.
    /// <para>This static property is thread-safe.</para></summary>
    static abstract TCalendar Calendar { get; }
}
