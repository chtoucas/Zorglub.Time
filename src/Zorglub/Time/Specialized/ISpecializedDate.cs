// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1000 // Do not declare static members on generic types (Design) 👈 PreviewFeatures

namespace Zorglub.Time.Specialized
{
    using Zorglub.Time.Hemerology;

    /// <summary>
    /// Defines a "specialized" date.
    /// </summary>
    public interface ISpecializedDate : IDate
    {
        /// <summary>
        /// Gets the day number.
        /// </summary>
        DayNumber DayNumber { get; }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        void Deconstruct(out int year, out int month, out int day);
    }

    /// <summary>
    /// Defines a "specialized" date type.
    /// </summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    /// <typeparam name="TCalendar">The companion calendar type.</typeparam>
    public interface ISpecializedDate<TSelf, TCalendar> :
        ISpecializedDate,
        IDate<TSelf>,
        IMinMaxValue<TSelf>
        where TCalendar : ICalendar<TSelf>
        where TSelf : ISpecializedDate<TSelf, TCalendar>
    {
        /// <summary>
        /// Gets the calendar to which belongs the current instance.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        static abstract TCalendar Calendar { get; }
    }
}
