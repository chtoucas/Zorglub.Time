// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    // REVIEW(name): ICanInterconvert
    public interface IInterconvertible<out T, TCalendar>
    {
        /// <summary>
        /// Gets the calendar to which belongs the current instance.
        /// </summary>
        TCalendar Calendar { get; }

        /// <summary>
        /// Interconverts the current instance to a range within a different
        /// calendar.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/>
        /// is null.</exception>
        [Pure] T WithCalendar(TCalendar newCalendar);
    }
}
