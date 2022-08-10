// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design) 👈 PreviewFeatures

namespace Zorglub.Time.Hemerology
{
    /// <summary>
    /// Defines a date.
    /// </summary>
    public interface IDate : IFixedDate, IDateable { }

    /// <summary>
    /// Defines a date type.
    /// </summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    public interface IDate<TSelf> : IDate, IFixedDate<TSelf>
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
    // seul calendrier, d'où le fait d'avoir choisi une propriété __statique__.
    // Pour des dates fonctionnant avec un calendrier "pluriel", voir
    // plutôt IInterconvertible.

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
    }
}
