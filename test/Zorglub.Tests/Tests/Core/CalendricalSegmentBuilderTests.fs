// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.CalendricalSegmentBuilderTests

open System

open Zorglub.Testing

open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas

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
    let ``SetMin/MaxDate() throws when the date is invalid`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        outOfRangeExn "year" (fun () -> builder.SetMinDate(GregorianSchema.MinYear - 1, 12, 1))
        outOfRangeExn "year" (fun () -> builder.SetMinDate(GregorianSchema.MaxYear + 1, 12, 1))
        outOfRangeExn "month" (fun () -> builder.SetMinDate(1, 0, 1))
        outOfRangeExn "month" (fun () -> builder.SetMinDate(1, 13, 1))
        outOfRangeExn "day" (fun () -> builder.SetMinDate(1, 1, 0))
        outOfRangeExn "day" (fun () -> builder.SetMinDate(1, 1, 32))

        outOfRangeExn "year" (fun () -> builder.SetMaxDate(GregorianSchema.MinYear - 1, 12, 1))
        outOfRangeExn "year" (fun () -> builder.SetMaxDate(GregorianSchema.MaxYear + 1, 12, 1))
        outOfRangeExn "month" (fun () -> builder.SetMaxDate(1, 0, 1))
        outOfRangeExn "month" (fun () -> builder.SetMaxDate(1, 13, 1))
        outOfRangeExn "day" (fun () -> builder.SetMaxDate(1, 1, 0))
        outOfRangeExn "day" (fun () -> builder.SetMaxDate(1, 1, 32))

    [<Fact>]
    let ``SetMin/MaxOrdinal() throws when the date is invalid`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        outOfRangeExn "year" (fun () -> builder.SetMinOrdinal(GregorianSchema.MinYear - 1, 1))
        outOfRangeExn "year" (fun () -> builder.SetMinOrdinal(GregorianSchema.MaxYear + 1, 1))
        outOfRangeExn "dayOfYear" (fun () -> builder.SetMinOrdinal(1, 0))
        outOfRangeExn "dayOfYear" (fun () -> builder.SetMinOrdinal(1, 367))

        outOfRangeExn "year" (fun () -> builder.SetMaxOrdinal(GregorianSchema.MinYear - 1, 1))
        outOfRangeExn "year" (fun () -> builder.SetMaxOrdinal(GregorianSchema.MaxYear + 1, 1))
        outOfRangeExn "dayOfYear" (fun () -> builder.SetMaxOrdinal(1, 0))
        outOfRangeExn "dayOfYear" (fun () -> builder.SetMaxOrdinal(1, 367))

    [<Fact>]
    let ``SetMin/MaxYear() throws when the year is out of range`` () =
        let sch = new GregorianSchema()
        let sys = SystemSegment.Create(sch, sch.SupportedYears)
        let builder = new CalendricalSegmentBuilder(sch)

        outOfRangeExn "year" (fun () -> builder.SetMinYear(sys.SupportedYears.Min - 1))
        outOfRangeExn "year" (fun () -> builder.SetMinYear(sys.SupportedYears.Max + 1))

        outOfRangeExn "year" (fun () -> builder.SetMaxYear(sys.SupportedYears.Min - 1))
        outOfRangeExn "year" (fun () -> builder.SetMaxYear(sys.SupportedYears.Max + 1))

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
