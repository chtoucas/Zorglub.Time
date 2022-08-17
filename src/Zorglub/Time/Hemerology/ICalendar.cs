// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Hemerology.Scopes;

    #region Developer Notes

    // Standard calendar: year/month/day subdivision of time, a single era,
    // that is an interval of days.
    //
    // Types implementing ICalendar or ICalendar<T>
    // --------------------------------------------
    //
    //   SimpleCalendar                         CalendarDate, OrdinalDate, etc.
    //     SimpleGregorian & co
    //   BasicCalendar
    //     BoundedBelowCalendar
    //       BoundedBelowCalendar<TDate>        TDate
    //         BoundedBelowDayCalendar          DayNumber
    //     MinMaxYearCalendar
    //       MinMaxYearDayCalendar            DayNumber
    //       (MyCivilCalendar)                MyCivilDate
    //       SpecialCalendar
    //         # Specialized calendars
    //         CivilCalendar                    CivilDate
    //         GregorianCalendar                GregorianDate
    //         JulianCalendar                   JulianDate
    //         etc.
    //       ZCalendar                        ZDate
    //         GregorianZCalendar & co
    // A   NakedCalendar
    //       BoundedBelowNakedCalendar
    //       MinMaxYearNakedCalendar
    //       (MyNakedCalendar)
    //
    // Annotation: A = abstract
    // Between parentheses: sample calendars
    //
    // Calendar vs Date
    // ----------------
    //
    // Calendar pros
    // - Pluggable calendars means that we define only once all kind of
    //   calendrical objects (CalendarDate, CalendarDay, OrdinalDate,
    //   CalendarYear, CalendarMonth, etc.).
    // Date pros
    // - We can be very specific. With calendars, we have to take the "plus
    //   petit dénominateur commun".
    //
    // See https://blog.joda.org/2009/11/why-jsr-310-isn-joda-time_4941.html
    //
    // Remarks
    // -------
    //
    // Interval of days: this is the simplest and most natural choice. Beware,
    // without this assumption, arithmetic becomes much more complicated.
    //
    // Do NOT use DayNumber in parameters. Since the calendars we are dealing
    // with are modeled around a year, month, day partition, most of the time a
    // DayNumber in input will be converted before doing anything, so better
    // left this (conversion) outside the method.
    //
    // Even for conversion purposes, we do not add methods to convert from a
    // DayNumber to a y/m/d or y/doy representation. See comments in ICalendar<>.
    //
    // To be complete, we should have methods GetStart(End)OfYear(Month) with a
    // DayNumber return type. We do not include them here but in ICalendar<>,
    // which means that we expect that calendar developpers to implement
    // ICalendar<DayNumber> (unless they have a custom day type like CalendarDay,
    // in which they surely implement ICalendar<the date type>).
    //
    // Since week numbering has no universal definition, this interface has
    // nothing to say about weeks.
    //
    // We have three different ways to model a date:
    //  1. year/month/day repr. (human)
    //  2. year/dayOfYear repr. (ordinal)
    //  3. DaysSinceEpoch repr. (universal)
    // Notes:
    // - Common calendar types come w/ a companion date of type 1 and use
    //   DayNumber for type 3.
    // - DayNumber being always available, we offer conversions from
    //   representation of type 1 and 2 to DayNumber.

    // We have three different ways to model a date:
    //  1. Year/Month/Day repr. (human)
    //  2. Year/DayOfYear repr. (ordinal)
    //  3. DayNumber repr.      (universal)
    // Classes implementing this interface should also provide:
    // - A type constructor, a factory,
    //     GetXXX(repr) -> TDate
    // - Conversion ops from the other date repr. to TDate
    //     GetXXX(other repr) -> TDate
    // where XXX is the name of the date type.
    // With conversion ops, not only do we have to validate the input but we
    // must also transform it before we can create the target object.
    // We did not include them in the interface because GetDate() feel too
    // generic to me. For instance,
    // ZCalendar has
    // - GetDate(y, m, d)   -> ZDate
    // - GetDate(y, doy)    -> ZDate
    // - GetDate(dayNumber) -> ZDate
    // See also Calendar which offers several methods of this kind.
    //
    // Regarding min/max values, for exactly the same reason, we do not include
    // them here but end calendars should have them, e.g. Min/MaxDate or a
    // single MinMaxDate. For instance, I prefer Min/MaxDateParts to Min/MaxDate
    // with NakedCalendar, but it could be also Min/MaxDay or Min/MaxOrdinalDate.
    // Furthermore, for mono-system of calendars, we expect TDate to implement
    // IMinMaxValue<TDate>.
    //
    // API
    // ---
    //
    // Missing methods/props?
    // - factories, today, conversions, interconversion
    // - adjusters, providers
    // - min/max values
    //
    // ## Mono-calendar system with a single companion date type.
    // Ex. CivilCalendar+CivilDate
    // * Calendar:
    //   - impl ICalendar<CivilDate>, ie date providers
    // * Date:
    //   - impl IDate<TSelf, TCalendar>
    //     - prop MinMaxValue <- IMinMaxValue<TSelf>
    //     - static prop Calendar
    //   - static prop Adjuster
    //   - Today() <- ITodayProvider
    // * Supporting types:
    //   - date adjuster
    //
    // Special case of a mono-calendar system with TDate = DayNumber.
    // - min/max values are provided by Calendar.Domain
    // - today is the only method missing
    // - factories are provided by BasicCalendar
    // - conversions, none
    // - adjusters, none
    //
    // ## Poly-calendar system with a single companion date type
    // Ex. ZCalendar+ZDate
    // * Calendar:
    //   - impl ICalendar<ZDate>, ie date providers
    //   - prop MinMaxDate
    //   - impl IDateFactory<ZDate>
    //      - GetDate(dayNumber)
    //      - GetDate(y, m, d)
    //      - GetDate(y, doy)
    //   - Today()
    // * Date:
    //   - impl IDate<TSelf>
    //   - inst prop Calendar
    //   - WithCalendar(newCalendar) <- interconversion
    //   - Today() is using the __default calendar__
    //   - FromDayNumber() is using the __default calendar__
    // * Supporting types:
    //   - date adjuster
    //
    // ## Poly-calendar system with more than one companion date type
    // Ex. SimpleCalendar+CalendarDate/CalendarDay/OrdinalDate.
    // * Calendar:
    //   - impl ICalendar
    //   - prop MinMaxXXX
    //   - GetXXXDate(dayNumber)
    //   - GetXXXDate(y, m, d)
    //   - GetXXXDate(y, doy)
    //   - GetCurrentXXX()
    // * Date:
    //   - impl IDate<TSelf>
    //   - inst prop Calendar
    //   - WithCalendar(newCalendar) <- interconversion
    //   - Today() is using the __default calendar__
    //   - FromDayNumber() is using the __default calendar__
    // * Supporting types:
    //   - date provider
    //   - date adjuster
    //
    // ## The date type is the calendar type too
    // Ex. XCivilDate or MyDate.
    //
    // ## Mono-calendar system without a companion date type.
    // Ex. NakedCalendar, MinMaxYearCalendar<TDate>?

    #endregion

    /// <summary>
    /// Defines a calendar.
    /// </summary>
    /// <remarks>
    /// <para>We do NOT assume the existence of a custom type for representing dates.</para>
    /// </remarks>
    public interface ICalendar : ICalendricalKernel
    {
        /// <summary>
        /// Gets the epoch of the calendar.
        /// </summary>
        DayNumber Epoch { get; }

        /// <summary>
        /// Gets the range of supported values for a <see cref="DayNumber"/>.
        /// </summary>
        Range<DayNumber> Domain { get; }

        /// <summary>
        /// Gets the calendar scope.
        /// </summary>
        CalendarScope Scope { get; }

        //
        // Conversions
        //

        /// <summary>
        /// Obtains the day number on the specified date.
        /// </summary>
        /// <exception cref="AoorException">The date is not within the calendar boundaries.
        /// </exception>
        [Pure] DayNumber GetDayNumber(int year, int month, int day);

        /// <summary>
        /// Obtains the day number on the specified ordinal date.
        /// </summary>
        /// <exception cref="AoorException">The ordinal date is not within the calendar boundaries.
        /// </exception>
        [Pure] DayNumber GetDayNumber(int year, int dayOfYear);
    }

    // TODO(api): add Today(), UtcToday().

    /// <summary>
    /// Defines a calendar with a companion date type.
    /// </summary>
    /// <typeparam name="TDate">The type of date object to return.</typeparam>
    public interface ICalendar<out TDate> : ICalendar, IDateProvider<TDate> { }
}
