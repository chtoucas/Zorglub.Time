// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

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

    [<Fact>]
    let ``Properties (half-buildable)`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())
        builder.SetMinYear(1)

        builder.HasMin |> ok
        builder.HasMax |> nok
        builder.IsBuildable |> nok

    [<Fact>]
    let ``Properties (buildable)`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())
        builder.SetMinYear(1)
        builder.SetMaxYear(2)

        builder.HasMin |> ok
        builder.HasMax |> ok
        builder.IsBuildable |> ok

    [<Fact>]
    let ``BuildSegment() throws when not buildable`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        throws<InvalidOperationException> (fun () -> builder.BuildSegment())

        builder.SetMinYear(1)
        throws<InvalidOperationException> (fun () -> builder.BuildSegment())

        builder.SetMaxYear(2)
        builder.BuildSegment() |> ignore

module Setters =
    [<Fact>]
    let ``SetMinDaysSinceEpoch() throws when daysSinceEpoch is out of range`` () =
        let sch = new GregorianSchema()
        let sys = SystemSegment.Create(sch, sch.SupportedYears)
        let builder = new CalendricalSegmentBuilder(sch)

        outOfRangeExn "daysSinceEpoch" (fun () -> builder.SetMinDaysSinceEpoch(sys.SupportedDays.Min - 1))
        builder.SetMinDaysSinceEpoch(sys.SupportedDays.Min)
        builder.SetMinDaysSinceEpoch(sys.SupportedDays.Max)
        outOfRangeExn "daysSinceEpoch" (fun () -> builder.SetMinDaysSinceEpoch(sys.SupportedDays.Max + 1))

    [<Fact>]
    let ``SetMaxDaysSinceEpoch() throws when daysSinceEpoch is out of range`` () =
        let sch = new GregorianSchema()
        let sys = SystemSegment.Create(sch, sch.SupportedYears)
        let builder = new CalendricalSegmentBuilder(sch)

        outOfRangeExn "daysSinceEpoch" (fun () -> builder.SetMaxDaysSinceEpoch(sys.SupportedDays.Min - 1))
        builder.SetMaxDaysSinceEpoch(sys.SupportedDays.Min)
        builder.SetMaxDaysSinceEpoch(sys.SupportedDays.Max)
        outOfRangeExn "daysSinceEpoch" (fun () -> builder.SetMaxDaysSinceEpoch(sys.SupportedDays.Max + 1))

    [<Fact>]
    let ``SetMinYear() throws when the year is out of range`` () =
        let sch = new GregorianSchema()
        let sys = SystemSegment.Create(sch, sch.SupportedYears)
        let builder = new CalendricalSegmentBuilder(sch)

        outOfRangeExn "year" (fun () -> builder.SetMinYear(sys.SupportedYears.Min - 1))
        builder.SetMinYear(sys.SupportedYears.Min)
        builder.SetMinYear(sys.SupportedYears.Max)
        outOfRangeExn "year" (fun () -> builder.SetMinYear(sys.SupportedYears.Max + 1))

    [<Fact>]
    let ``SetMaxYear() throws when the year is out of range`` () =
        let sch = new GregorianSchema()
        let sys = SystemSegment.Create(sch, sch.SupportedYears)
        let builder = new CalendricalSegmentBuilder(sch)

        outOfRangeExn "year" (fun () -> builder.SetMaxYear(sys.SupportedYears.Min - 1))
        builder.SetMaxYear(sys.SupportedYears.Min)
        builder.SetMaxYear(sys.SupportedYears.Max)
        outOfRangeExn "year" (fun () -> builder.SetMaxYear(sys.SupportedYears.Max + 1))

    [<Fact>]
    let ``SetMin/MaxDate() throws when the date is invalid`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        outOfRangeExn "parts" (fun () -> builder.SetMinDateParts(new DateParts(GregorianSchema.MinYear - 1, 12, 1)))
        outOfRangeExn "parts" (fun () -> builder.SetMinDateParts(new DateParts(GregorianSchema.MaxYear + 1, 12, 1)))
        outOfRangeExn "parts" (fun () -> builder.SetMinDateParts(new DateParts(1, 0, 1)))
        outOfRangeExn "parts" (fun () -> builder.SetMinDateParts(new DateParts(1, 13, 1)))
        outOfRangeExn "parts" (fun () -> builder.SetMinDateParts(new DateParts(1, 1, 0)))
        outOfRangeExn "parts" (fun () -> builder.SetMinDateParts(new DateParts(1, 1, 32)))

        outOfRangeExn "parts" (fun () -> builder.SetMaxDateParts(new DateParts(GregorianSchema.MinYear - 1, 12, 1)))
        outOfRangeExn "parts" (fun () -> builder.SetMaxDateParts(new DateParts(GregorianSchema.MaxYear + 1, 12, 1)))
        outOfRangeExn "parts" (fun () -> builder.SetMaxDateParts(new DateParts(1, 0, 1)))
        outOfRangeExn "parts" (fun () -> builder.SetMaxDateParts(new DateParts(1, 13, 1)))
        outOfRangeExn "parts" (fun () -> builder.SetMaxDateParts(new DateParts(1, 1, 0)))
        outOfRangeExn "parts" (fun () -> builder.SetMaxDateParts(new DateParts(1, 1, 32)))

    [<Fact>]
    let ``SetMin/MaxOrdinal() throws when the date is invalid`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        outOfRangeExn "parts" (fun () -> builder.SetMinOrdinalParts(new OrdinalParts(GregorianSchema.MinYear - 1, 1)))
        outOfRangeExn "parts" (fun () -> builder.SetMinOrdinalParts(new OrdinalParts(GregorianSchema.MaxYear + 1, 1)))
        outOfRangeExn "parts" (fun () -> builder.SetMinOrdinalParts(new OrdinalParts(1, 0)))
        outOfRangeExn "parts" (fun () -> builder.SetMinOrdinalParts(new OrdinalParts(1, 367)))

        outOfRangeExn "parts" (fun () -> builder.SetMaxOrdinalParts(new OrdinalParts(GregorianSchema.MinYear - 1, 1)))
        outOfRangeExn "parts" (fun () -> builder.SetMaxOrdinalParts(new OrdinalParts(GregorianSchema.MaxYear + 1, 1)))
        outOfRangeExn "parts" (fun () -> builder.SetMaxOrdinalParts(new OrdinalParts(1, 0)))
        outOfRangeExn "parts" (fun () -> builder.SetMaxOrdinalParts(new OrdinalParts(1, 367)))

    [<Fact>]
    let ``Min setter throws when Min > Max`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        builder.SetMaxDaysSinceEpoch(0)
        outOfRangeExn "value" (fun () -> builder.SetMinDaysSinceEpoch(1))

    [<Fact>]
    let ``Min setter does not throw when Min = Max`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        builder.SetMaxDaysSinceEpoch(0)
        builder.SetMinDaysSinceEpoch(0)

    [<Fact>]
    let ``Max setter throws when Max < Max`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        builder.SetMinDaysSinceEpoch(1)
        outOfRangeExn "value" (fun () -> builder.SetMaxDaysSinceEpoch(0))

    [<Fact>]
    let ``Max setter does not throw when Max = Min`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        builder.SetMinDaysSinceEpoch(0)
        builder.SetMaxDaysSinceEpoch(0)

    [<Fact>]
    let ``Build a segment using Min/MaxDaysSinceEpoch`` () =
        let sch = new GregorianSchema()
        let builder = new CalendricalSegmentBuilder(sch)
        builder.SetMinDaysSinceEpoch(0)
        builder.SetMaxDaysSinceEpoch(31)
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
        builder.SetMinDateParts(min)
        builder.SetMaxOrdinalParts(max)
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
        builder.SetMinOrdinalParts(min)
        builder.SetMaxDateParts(max)
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
