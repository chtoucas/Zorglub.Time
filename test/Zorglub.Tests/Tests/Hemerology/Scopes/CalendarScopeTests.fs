// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.CalendarScopeTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology.Scopes

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when "segment" is null`` () =
        nullExn "segment" (fun () -> new FauxCalendarScope(null))

    [<Fact>]
    let ``Property Epoch`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let scope = new FauxCalendarScope(new FauxCalendricalSchema(), epoch, 1, 4)

        scope.Epoch === epoch

    [<Fact>]
    let ``Property Domain`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let sch = new GregorianSchema()
        let supportedYears = Range.Create(1, 4)
        let scope = new FauxCalendarScope(epoch, CalendricalSegment.Create(sch, supportedYears))

        let minDayNumber = epoch + sch.GetStartOfYear(supportedYears.Min)
        let maxDayNumber = epoch + sch.GetEndOfYear(supportedYears.Max)
        let range = Range.Create(minDayNumber, maxDayNumber)

        scope.Domain === range

    [<Fact>]
    let ``Property Segment and related properties`` () =
        let sch = new GregorianSchema()
        let seg = CalendricalSegment.Create(sch, Range.Create(1, 4))
        let scope = new FauxCalendarScope(seg)

        scope.Segment ==& seg
        scope.Segment.IsComplete === seg.IsComplete
        // It's enough to check the property Range.
        scope.DaysValidator.Range === seg.SupportedDays
        scope.MonthsValidator.Range === seg.SupportedMonths
        scope.YearsValidator.Range === seg.SupportedYears

    [<Fact>]
    let ``Property Schema`` () =
        let sch = new GregorianSchema()
        let seg = CalendricalSegment.Create(sch, Range.Create(1, 4))
        let scope = new FauxCalendarScope(seg) :> ISchemaBound

        scope.Schema ==& sch
