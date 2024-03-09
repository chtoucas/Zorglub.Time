// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Core.CalendricalSegmentBuilderTests

open System

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Core.Utilities

open Xunit

module Prelude =
    [<Fact>]
    let ``Properties (pristine)`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        builder.HasMin |> nok
        builder.HasMax |> nok
        builder.IsBuildable |> nok

        throws<InvalidOperationException> (fun () -> builder.MinDaysSinceEpoch)
        throws<InvalidOperationException> (fun () -> builder.MinDateParts)
        throws<InvalidOperationException> (fun () -> builder.MinOrdinalParts)

        throws<InvalidOperationException> (fun () -> builder.MaxDaysSinceEpoch)
        throws<InvalidOperationException> (fun () -> builder.MaxDateParts)
        throws<InvalidOperationException> (fun () -> builder.MaxOrdinalParts)

    [<Fact>]
    let ``Properties (half-buildable, only the minimum is set)`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())
        builder.SetMinToStartOfYear(1)

        builder.HasMin |> ok
        builder.HasMax |> nok
        builder.IsBuildable |> nok

        builder.MinDaysSinceEpoch |> ignore
        builder.MinDateParts |> ignore
        builder.MinOrdinalParts |> ignore

        throws<InvalidOperationException> (fun () -> builder.MaxDaysSinceEpoch)
        throws<InvalidOperationException> (fun () -> builder.MaxDateParts)
        throws<InvalidOperationException> (fun () -> builder.MaxOrdinalParts)

    [<Fact>]
    let ``Properties (half-buildable, only the maximum is set)`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())
        builder.SetMaxToEndOfYear(2)

        builder.HasMin |> nok
        builder.HasMax |> ok
        builder.IsBuildable |> nok

        throws<InvalidOperationException> (fun () -> builder.MinDaysSinceEpoch)
        throws<InvalidOperationException> (fun () -> builder.MinDateParts)
        throws<InvalidOperationException> (fun () -> builder.MinOrdinalParts)

        builder.MaxDaysSinceEpoch |> ignore
        builder.MaxDateParts |> ignore
        builder.MaxOrdinalParts |> ignore

    [<Fact>]
    let ``Properties (buildable)`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())
        builder.SetMinToStartOfYear(1)
        builder.SetMaxToEndOfYear(2)

        builder.HasMin |> ok
        builder.HasMax |> ok
        builder.IsBuildable |> ok

        builder.MinDaysSinceEpoch |> ignore
        builder.MinDateParts |> ignore
        builder.MinOrdinalParts |> ignore

        builder.MaxDaysSinceEpoch |> ignore
        builder.MaxDateParts |> ignore
        builder.MaxOrdinalParts |> ignore

    [<Fact>]
    let ``BuildSegment() throws when not buildable`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        throws<InvalidOperationException> (fun () -> builder.BuildSegment())

        builder.SetMinToStartOfYear(1)
        throws<InvalidOperationException> (fun () -> builder.BuildSegment())

        builder.SetMaxToEndOfYear(2)
        builder.BuildSegment() |> ignore

module Setters =
    [<Fact>]
    let ``MinDaysSinceEpoch throws when daysSinceEpoch is out of range`` () =
        let sch = new GregorianSchema()
        let sys = SystemSegment.Create(sch, sch.SupportedYears)
        let builder = new CalendricalSegmentBuilder(sch)

        outOfRangeExn "daysSinceEpoch" (fun () -> builder.MinDaysSinceEpoch <- sys.SupportedDays.Min - 1; builder)
        builder.MinDaysSinceEpoch <- sys.SupportedDays.Min
        builder.MinDaysSinceEpoch <- sys.SupportedDays.Max
        outOfRangeExn "daysSinceEpoch" (fun () -> builder.MinDaysSinceEpoch <- sys.SupportedDays.Max + 1; builder)

    [<Fact>]
    let ``MaxDaysSinceEpoch throws when daysSinceEpoch is out of range`` () =
        let sch = new GregorianSchema()
        let sys = SystemSegment.Create(sch, sch.SupportedYears)
        let builder = new CalendricalSegmentBuilder(sch)

        outOfRangeExn "daysSinceEpoch" (fun () -> builder.MaxDaysSinceEpoch <- sys.SupportedDays.Min - 1; builder)
        builder.MaxDaysSinceEpoch <- sys.SupportedDays.Min
        builder.MaxDaysSinceEpoch <- sys.SupportedDays.Max
        outOfRangeExn "daysSinceEpoch" (fun () -> builder.MaxDaysSinceEpoch <- sys.SupportedDays.Max + 1; builder)

    [<Fact>]
    let ``SetMinToStartOfYear() throws when the year is out of range`` () =
        let sch = new GregorianSchema()
        let sys = SystemSegment.Create(sch, sch.SupportedYears)
        let builder = new CalendricalSegmentBuilder(sch)

        outOfRangeExn "year" (fun () -> builder.SetMinToStartOfYear(sys.SupportedYears.Min - 1))
        builder.SetMinToStartOfYear(sys.SupportedYears.Min)
        builder.SetMinToStartOfYear(sys.SupportedYears.Max)
        outOfRangeExn "year" (fun () -> builder.SetMinToStartOfYear(sys.SupportedYears.Max + 1))

    [<Fact>]
    let ``SetMaxToEndOfYear() throws when the year is out of range`` () =
        let sch = new GregorianSchema()
        let sys = SystemSegment.Create(sch, sch.SupportedYears)
        let builder = new CalendricalSegmentBuilder(sch)

        outOfRangeExn "year" (fun () -> builder.SetMaxToEndOfYear(sys.SupportedYears.Min - 1))
        builder.SetMaxToEndOfYear(sys.SupportedYears.Min)
        builder.SetMaxToEndOfYear(sys.SupportedYears.Max)
        outOfRangeExn "year" (fun () -> builder.SetMaxToEndOfYear(sys.SupportedYears.Max + 1))

    [<Fact>]
    let ``MinDateParts throws when the date is invalid`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        outOfRangeExn "parts" (fun () -> builder.MinDateParts <- new DateParts(GregorianSchema.MinYear - 1, 12, 1); builder)
        outOfRangeExn "parts" (fun () -> builder.MinDateParts <- new DateParts(GregorianSchema.MaxYear + 1, 12, 1); builder)
        outOfRangeExn "parts" (fun () -> builder.MinDateParts <- new DateParts(1, 0, 1); builder)
        outOfRangeExn "parts" (fun () -> builder.MinDateParts <- new DateParts(1, 13, 1); builder)
        outOfRangeExn "parts" (fun () -> builder.MinDateParts <- new DateParts(1, 1, 0); builder)
        outOfRangeExn "parts" (fun () -> builder.MinDateParts <- new DateParts(1, 1, 32); builder)

    [<Fact>]
    let ``MaxDateParts throws when the date is invalid`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        outOfRangeExn "parts" (fun () -> builder.MaxDateParts <- new DateParts(GregorianSchema.MinYear - 1, 12, 1); builder)
        outOfRangeExn "parts" (fun () -> builder.MaxDateParts <- new DateParts(GregorianSchema.MaxYear + 1, 12, 1); builder)
        outOfRangeExn "parts" (fun () -> builder.MaxDateParts <- new DateParts(1, 0, 1); builder)
        outOfRangeExn "parts" (fun () -> builder.MaxDateParts <- new DateParts(1, 13, 1); builder)
        outOfRangeExn "parts" (fun () -> builder.MaxDateParts <- new DateParts(1, 1, 0); builder)
        outOfRangeExn "parts" (fun () -> builder.MaxDateParts <- new DateParts(1, 1, 32); builder)

    [<Fact>]
    let ``MinOrdinalParts throws when the date is invalid`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        outOfRangeExn "parts" (fun () -> builder.MinOrdinalParts <- new OrdinalParts(GregorianSchema.MinYear - 1, 1); builder)
        outOfRangeExn "parts" (fun () -> builder.MinOrdinalParts <- new OrdinalParts(GregorianSchema.MaxYear + 1, 1); builder)
        outOfRangeExn "parts" (fun () -> builder.MinOrdinalParts <- new OrdinalParts(1, 0); builder)
        outOfRangeExn "parts" (fun () -> builder.MinOrdinalParts <- new OrdinalParts(1, 367); builder)

    [<Fact>]
    let ``MaxOrdinalParts throws when the date is invalid`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        outOfRangeExn "parts" (fun () -> builder.MaxOrdinalParts <- new OrdinalParts(GregorianSchema.MinYear - 1, 1); builder)
        outOfRangeExn "parts" (fun () -> builder.MaxOrdinalParts <- new OrdinalParts(GregorianSchema.MaxYear + 1, 1); builder)
        outOfRangeExn "parts" (fun () -> builder.MaxOrdinalParts <- new OrdinalParts(1, 0); builder)
        outOfRangeExn "parts" (fun () -> builder.MaxOrdinalParts <- new OrdinalParts(1, 367); builder)

    [<Fact>]
    let ``Min setter throws when Min > Max`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        builder.MaxDaysSinceEpoch <- 0
        outOfRangeExn "value" (fun () -> builder.MinDaysSinceEpoch <- 1; builder)

    [<Fact>]
    let ``Min setter does not throw when Min = Max`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        builder.MaxDaysSinceEpoch <- 0
        builder.MinDaysSinceEpoch <- 0

    [<Fact>]
    let ``Max setter throws when Max < Max`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        builder.MinDaysSinceEpoch <- 1
        outOfRangeExn "value" (fun () -> builder.MaxDaysSinceEpoch <- 0; builder)

    [<Fact>]
    let ``Max setter does not throw when Max = Min`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        builder.MinDaysSinceEpoch <- 0
        builder.MaxDaysSinceEpoch <- 0

    [<Fact>]
    let ``Build a segment using Min/MaxDaysSinceEpoch`` () =
        let sch = new GregorianSchema()
        let builder = new CalendricalSegmentBuilder(sch)

        builder.MinDaysSinceEpoch <- 0
        builder.MinDaysSinceEpoch === 0

        builder.MaxDaysSinceEpoch <- 31
        builder.MaxDaysSinceEpoch === 31

        let seg = builder.BuildSegment()

        let minMaxDateParts = OrderedPair.Create(
            new DateParts(1, 1, 1),
            new DateParts(1, 2, 1))

        let minMaxOrdinalParts = OrderedPair.Create(
            new OrdinalParts(1, 1),
            new OrdinalParts(1, 32))

        let minMaxMonthParts = OrderedPair.Create(
            new MonthParts(1, 1),
            new MonthParts(1, 2))

        seg.IsComplete |> nok

        seg.SupportedDays   === Range.Create(0, 31)
        seg.SupportedMonths === Range.Create(0, 1)
        seg.SupportedYears  === Range.Singleton(1)

        seg.MinMaxDateParts    === minMaxDateParts
        seg.MinMaxOrdinalParts === minMaxOrdinalParts
        seg.MinMaxMonthParts   === minMaxMonthParts

    [<Fact>]
    let ``Build a segment using MinDateParts and MaxOrdinalParts`` () =
        let sch = new GregorianSchema()
        let range = Range.Create(1, 2)
        let min = new DateParts(range.Min, 2, 1)
        let max = new OrdinalParts(range.Max, 364)
        let builder = new CalendricalSegmentBuilder(sch)

        builder.MinDateParts <- min
        builder.MinDateParts === min

        builder.MaxOrdinalParts <- max
        builder.MaxOrdinalParts === max

        let seg = builder.BuildSegment()

        let minMaxDateParts = OrderedPair.Create(
            min,
            new DateParts(range.Max, 12, 30))

        let minMaxOrdinalParts = OrderedPair.Create(
            new OrdinalParts(range.Min, 32),
            max)

        let minMaxMonthParts = OrderedPair.Create(
            new MonthParts(range.Min, 2),
            new MonthParts(range.Max, 12))

        seg.IsComplete |> nok

        seg.SupportedDays   === Range.Create(31, 728)
        seg.SupportedMonths === Range.Create(1, 23)
        seg.SupportedYears  === range

        seg.MinMaxDateParts    === minMaxDateParts
        seg.MinMaxOrdinalParts === minMaxOrdinalParts
        seg.MinMaxMonthParts   === minMaxMonthParts

    [<Fact>]
    let ``Build a segment using MinOrdinalParts and MaxDateParts`` () =
        let sch = new GregorianSchema()
        let range = Range.Create(1, 2)
        let min = new OrdinalParts(range.Min, 32)
        let max = new DateParts(range.Max, 12, 30)
        let builder = new CalendricalSegmentBuilder(sch)

        builder.MinOrdinalParts <- min
        builder.MinOrdinalParts === min

        builder.MaxDateParts <- max
        builder.MaxDateParts === max

        let seg = builder.BuildSegment()

        let minMaxDateParts = OrderedPair.Create(
            new DateParts(range.Min, 2, 1),
            max)

        let minMaxOrdinalParts = OrderedPair.Create(
            min,
            new OrdinalParts(range.Max, 364))

        let minMaxMonthParts = OrderedPair.Create(
            new MonthParts(range.Min, 2),
            new MonthParts(range.Max, 12))

        seg.IsComplete |> nok

        seg.SupportedDays   === Range.Create(31, 728)
        seg.SupportedMonths === Range.Create(1, 23)
        seg.SupportedYears  === range

        seg.MinMaxDateParts    === minMaxDateParts
        seg.MinMaxOrdinalParts === minMaxOrdinalParts
        seg.MinMaxMonthParts   === minMaxMonthParts
