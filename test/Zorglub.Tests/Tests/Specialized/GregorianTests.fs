﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Specialized.GregorianTests

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Unbounded
open Zorglub.Testing.Facts.Hemerology
open Zorglub.Testing.Facts.Specialized

open Zorglub.Time
open Zorglub.Time.Specialized

open Xunit

// NB: notice the use of UnboundedGregorianDataSet.

module Prelude =
    let private calendarDataSet = UnboundedGregorianDataSet.Instance

    let daysSinceEpochInfoData = calendarDataSet.DaysSinceEpochInfoData

    [<Theory; MemberData(nameof(daysSinceEpochInfoData))>]
    let ``Property DaysSinceZero`` (info: DaysSinceEpochInfo) =
        let (daysSinceEpoch, y, m, d) = info.Deconstruct()
        let date = new GregorianDate(y, m, d)

        date.DaysSinceZero === daysSinceEpoch

module Bundles =
    let private chr = new GregorianCalendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<GregorianDate, GregorianCalendar, UnboundedGregorianDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d);
        override __.GetDate(y, doy) = new GregorianDate(y, doy);
        override __.GetDate(dayNumber) = GregorianDate.FromDayNumber(dayNumber);

        [<Fact>]
        member x.MonthsInYear_Prop() = x.CalendarUT.MonthsInYear === 12

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DateFacts() =
        inherit IDateFacts<GregorianDate, GregorianCalendar, UnboundedGregorianDataSet>(chr)

        override __.MinDate = GregorianDate.MinValue
        override __.MaxDate = GregorianDate.MaxValue

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = GregorianDate.Calendar |> isnotnull

        [<Fact>]
        static member Adjuster_Prop() = GregorianDate.Adjuster |> isnotnull

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DateAdjusterFacts() =
        inherit SpecialAdjusterFacts<GregorianDate, UnboundedGregorianDataSet>(new GregorianAdjuster())

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d)
        override __.GetDate(y, doy) = new GregorianDate(y, doy)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<GregorianDate, UnboundedGregorianDataSet>()

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d)
