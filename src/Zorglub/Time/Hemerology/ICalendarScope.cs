// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Hemerology.Scopes;

    #region Developer Notes

    // Types Implementing ICalendarScope
    // ---------------------------------
    //
    // ### Public Hierarchy
    //
    // ICalendarScope
    // └─ CalendarScope [A]
    //    ├─ BoundedBelowScope
    //    └─ MinMaxYearScope
    //
    // Annotation: [A] = abstract
    //
    // ### Full Hierarchy
    //
    // ICalendarScope
    // ├─ CalendarScope [A]
    // │  ├─ BoundedBelowScope
    // │  └─ MinMaxYearScope
    // └─ ShortScope [A]
    //    ├─ ProlepticShortScope [A]
    //    │  ├─ GregorianProlepticShortScope    (Gregorian-only)
    //    │  ├─ PlainProlepticShortScope
    //    │  └─ Solar12ProlepticShortScope      (CalendricalSchema)
    //    └─ StandardShortScope [A]
    //       ├─ GregorianStandardShortScope     (Gregorian-only)
    //       ├─ LunarStandardShortScope         (CalendricalSchema)
    //       ├─ LunisolarStandardShortScope     (CalendricalSchema)
    //       ├─ PlainStandardShortScope         (CalendricalSchema)
    //       ├─ Solar12StandardShortScope       (CalendricalSchema)
    //       └─ Solar13StandardShortScope       (CalendricalSchema)
    //
    // Construction
    // ------------
    // Public ctors:
    //   BoundedBelowScope.ctor(ICalendricalSchema)
    //   MinMaxYearScope.ctor(ICalendricalSchema)

    // Public factory methods:
    //   MinMaxYearScope.WithMaxYear(ICalendricalSchema)
    //   MinMaxYearScope.WithMinYear(ICalendricalSchema)
    //   ICalendarScope.CreateStandardScope(ICalendricalSchema)
    //   ICalendarScope.CreateProlepticScope(ICalendricalSchema)
    //
    // Internal factory methods:
    //   MinMaxYearScope.WithMaximalRange(ICalendricalSchema)
    //   ProlepticShortScope.Create(CalendricalSchema)
    //   StandardShortScope.Create(CalendricalSchema)
    //
    // Range of Supported Years
    // ------------------------
    // Only BoundedBelowScope supports ranges that do not map to complete years.
    //
    // - ProlepticShortScope    [-9998..9999]
    // - StandardShortScope     [1..9999]
    // - CalendarScope          Subintervals of [Yemoda.MinYear..Yemoda.MaxYear]
    //   - MinMaxYearScope      [minYear..maxYear] ⋂ [Yemoda.MinYear..Yemoda.MaxYear]
    //   - BoundedBelowScope    [minDate..maxYear] ⋂ [Yemoda.MinYear..Yemoda.MaxYear]

    #endregion

    /// <summary>
    /// Defines the scope of application of a calendar, an interval of days.
    /// </summary>
    public partial interface ICalendarScope : ICalendricalValidator
    {
        /// <summary>
        /// Gets the epoch.
        /// </summary>
        DayNumber Epoch { get; }

        /// <summary>
        /// Gets the range of supported <see cref="DayNumber"/> values.
        /// </summary>
        Range<DayNumber> Domain { get; }
    }

    public partial interface ICalendarScope // Factories
    {
        // Public versions of internal factory methods.

        /// <summary>
        /// Creates a standard scope for the specified schema and epoch.
        /// <para>A standard scope supports dates within the interval [1..9999] of years.</para>
        /// </summary>
        /// <remarks>
        /// <para>This is the scope used by <see cref="Simple.Calendar"/>, except in the Gregorian
        /// and Julian cases.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> does not contain the interval [1..9999].</exception>
        [Pure]
        public static ICalendarScope CreateStandardScope(CalendricalSchema schema, DayNumber epoch) =>
            StandardShortScope.Create(schema, epoch);

        /// <summary>
        /// Creates a proleptic scope for the specified schema and epoch.
        /// <para>A proleptic scope supports dates within the interval [-9998..9999] of years.</para>
        /// </summary>
        /// <remarks>
        /// <para>This is the scope used by <see cref="Simple.Calendar"/> in the Gregorian and
        /// Julian cases.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> does not contain the interval [-9998..9999].</exception>
        [Pure]
        public static ICalendarScope CreateProlepticScope(CalendricalSchema schema, DayNumber epoch) =>
            ProlepticShortScope.Create(schema, epoch);
    }
}