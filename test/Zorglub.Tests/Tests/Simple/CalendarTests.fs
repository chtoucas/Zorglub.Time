// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Bounded

open Zorglub.Time
open Zorglub.Time.Simple

open Xunit

// NB: here, we test calendar-specific methods.
// For methods related to IEpagomenalCalendar, this is done in FeaturetteTestSuite.

module GregorianCase =
    let private chr = GregorianCalendar.Instance
    let private domain = chr.Domain
    let private calendarDataSet = ProlepticGregorianDataSet.Instance

    let dayOfWeekData = calendarDataSet.DayOfWeekData
    // TODO(code): pre-filter data. Idem in JulianCase.
    let dayNumberToDayOfWeekData = CalCalDataSet.DayNumberToDayOfWeekData

    [<Theory; MemberData(nameof(dayOfWeekData))>]
    let ``GetDayOfWeek(CalendarDate)`` (info: YemodaAnd<DayOfWeek>) =
        let (y, m, d, dayOfWeek) = info.Deconstruct()
        let date = chr.GetCalendarDate(y, m, d)

        chr.GetDayOfWeek(date) === dayOfWeek

    [<Theory; MemberData(nameof(dayOfWeekData))>]
    let ``GetDayOfWeek(OrdinalDate)`` (info: YemodaAnd<DayOfWeek>) =
        let (y, m, d, dayOfWeek) = info.Deconstruct()
        let date = chr.GetCalendarDate(y, m, d).ToOrdinalDate()

        chr.GetDayOfWeek(date) === dayOfWeek

    [<RedundantTestUnit>]
    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    let ``GetDayOfWeek(CalendarDate) via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        if domain.Contains(dayNumber) then
            let date = chr.GetCalendarDateOn(dayNumber)

            chr.GetDayOfWeek(date) === dayOfWeek

    [<RedundantTestUnit>]
    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    let ``GetDayOfWeek(OrdinalDate) via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        if domain.Contains(dayNumber) then
            let date = chr.GetOrdinalDateOn(dayNumber)

            chr.GetDayOfWeek(date) === dayOfWeek

module JulianCase =
    let private chr = JulianCalendar.Instance
    let private domain = chr.Domain

    let dayNumberToDayOfWeekData = CalCalDataSet.DayNumberToDayOfWeekData

    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    let ``GetDayOfWeek(CalendarDate) via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        if domain.Contains(dayNumber) then
            let date = chr.GetCalendarDateOn(dayNumber)

            chr.GetDayOfWeek(date) === dayOfWeek

    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    let ``GetDayOfWeek(OrdinalDate) via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        if domain.Contains(dayNumber) then
            let date = chr.GetOrdinalDateOn(dayNumber)

            chr.GetDayOfWeek(date) === dayOfWeek
