// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

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
}
