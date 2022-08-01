// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    // FIXME(api): remove IEpagomenalCalendar<>, interface and in ZoroastrianSimpleCalendar.
    // Replace IEpagomenalCalendar by IEpagomenalDate
    // Replace IEpagomenalCalendarFacts by IEpagomenalDateFacts
    // Test the specialized date types
    // Remove SimpleDateExtensions?

    // Calendars could "implement" IEpagomenalFeaturette, but they don't.
    // The rationale is that the method sould be on the date type, not on the
    // calendar. We already encounters this in the base class with
    // ICalendricalKernel.
    // IEpagomenalFeaturette.IsEpagomenalDay() is also static and the scope is
    // not, so too "much" work to do for little benefit.

    // IEpagomenalCalendar does not inherit ICalendar<T> on purpose.
    // See for instance Simple.Calendar.
    // If a calendar does not have a companion date type, one should use
    // IEpagomenalFeaturette instead.

    /// <summary>
    /// Defines methods specific to calendars featuring epagomenal days.
    /// <para>The epagomenal days are usually found in descendants of the Egyptian calendar.</para>
    /// </summary>
    public interface IEpagomenalCalendar<TDate> : ICalendar where TDate : IDate
    {
        /// <summary>
        /// Determines whether the specified date is an epagomenal day or not, and also returns the
        /// epagomenal number of the day in an output parameter, zero if the date is not an
        /// epagomenal day.
        /// </summary>
        [Pure]
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "VB.NET Date")]
        bool IsEpagomenalDay(TDate date, out int epagomenalNumber);
    }
}
