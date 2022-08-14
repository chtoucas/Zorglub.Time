// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design) 👈 PreviewFeatures

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;

    // TODO(api): remove FromDayNumber() from IDate<TSelf, out TCalendar>?
    // Almost done, we still have to decide whether to keep it or not in simple
    // date types.
    // I'm not sure yet. We can achieve the same thing using one of the factory
    // methods on SimpleCalendar. Two reasons to keep it: constructors are also
    // specialized and optimised for the Gregorian case.
    // We will definitely keep it for date types not linked to a calendar type.

    #region Developer Notes

    // A date type is expected to provide the following constructors or factories:
    // - new(y, m, d)
    // - new(y, doy)
    // - new(dayNumber)

    #endregion

    /// <summary>
    /// Defines a date.
    /// </summary>
    public interface IDate : IFixedDay, IDateable { }

    /// <summary>
    /// Defines a date type.
    /// </summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    public interface IDate<TSelf> :
        IDate,
        IFixedDay<TSelf>,
        // Comparison
        IComparisonOperators<TSelf, TSelf>,
        IMinMaxFunctions<TSelf>,
        // Arithmetic
        IStandardArithmetic<TSelf>,
        IAdditionOperators<TSelf, int, TSelf>,
        ISubtractionOperators<TSelf, int, TSelf>,
        IDifferenceOperators<TSelf, int>,
        IIncrementOperators<TSelf>,
        IDecrementOperators<TSelf>
        where TSelf : IDate<TSelf>
    { }

    // REVIEW(api): "covariant return type" ne marche pas pour une propriété
    // provenant d'une interface ou ayant un "setter". Si c'était possible
    // j'aurais bien aimé rajouter la propriété suivante:
    // > static abstract IDateAdjuster<TDate> Adjuters { get; }
    // Dans l'état actuel des choses, il faudrait donc rajouter encore un
    // paramètre générique (TAdjuster : IDateAdjuster<TDate>), ce qui ne me
    // plaît guère. Bien entendu, tout ceci n'est pas nécessaire si un "adjuster"
    // ne fournit pas plus de fonctionnalités que l'interface, ce qui est le cas
    // pour le moment, mais cela devrait changer et puis même je ne souhaite pas
    // être coincer dans le future avec une interface bancale.
    // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/covariant-returns

    // L'interface suivante est prévue pour les dates ne fonctionnant qu'avec un
    // seul calendrier, d'où le fait d'avoir choisi des propriétés et méthodes
    // __statiques__.
    // Pour des dates fonctionnant avec un calendrier "pluriel", on utilisera
    // plutôt une propriété non-statique Calendar et on ajoutera une méthode pour
    // WithCalendar(newCalendar) pour l'interconversion; voir p.ex. ZDate et
    // ISimpleDate.

    /// <summary>
    /// Defines a date type with a companion calendar.
    /// <para>This interface SHOULD NOT be implemented by date types participating in a
    /// poly-calendar system.</para>
    /// </summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    /// <typeparam name="TCalendar">The companion calendar type.</typeparam>
    public interface IDate<TSelf, out TCalendar> :
        IDate<TSelf>,
        IMinMaxValue<TSelf>
        where TCalendar : ICalendar<TSelf>
        where TSelf : IDate<TSelf, TCalendar>
    {
        /// <summary>
        /// Gets the calendar to which belongs the current instance.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        static abstract TCalendar Calendar { get; }

        // FIXME(api): this is not the right way of doing it -> use a provider;
        // see also NodaTime.
        // We don't add the UTC version UtcToday(). I don't think that we need
        // such a level of precision, furthermore one can still do it manually
        // using a constructor new(DayNumber.UtcToday()) --- or a static factory
        // for dates linked to a poly-calendar system.

        /// <summary>
        /// Obtains the current day on this machine, expressed in local time, not UTC.
        /// </summary>
        [Pure] static abstract TSelf Today();
    }
}
