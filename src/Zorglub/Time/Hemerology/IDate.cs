// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design) 👈 PreviewFeatures

namespace Zorglub.Time.Hemerology
{
    /// <summary>
    /// Defines a date.
    /// </summary>
    public interface IDate : IFixedDay, IDateable { }

    /// <summary>
    /// Defines a date type.
    /// </summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    public interface IDate<TSelf> : IDate, IFixedDay<TSelf>
        where TSelf : IDate<TSelf>
    { }

    /// <summary>
    /// Defines a date type with a companion calendar.
    /// </summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    /// <typeparam name="TCalendar">The companion calendar type.</typeparam>
    public interface IDate<TSelf, TCalendar> :
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
