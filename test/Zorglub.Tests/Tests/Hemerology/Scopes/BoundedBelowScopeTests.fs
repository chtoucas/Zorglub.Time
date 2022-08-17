// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.BoundedBelowScopeTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology.Scopes

open Xunit

module Factories =
    [<Fact>]
    let ``Create() throws for null schema`` () =
        nullExn "schema" (fun () -> BoundedBelowScope.Create(null, DayZero.NewStyle, new DateParts(1, 1, 1), 2))

    [<Fact>]
    let ``Create() throws for an invalid date`` () =
        let sch = new GregorianSchema()

        outOfRangeExn "parts" (fun () -> BoundedBelowScope.Create(sch, DayZero.NewStyle, new DateParts(GregorianSchema.MinYear - 1, 12, 1), GregorianSchema.MaxYear))
        outOfRangeExn "parts" (fun () -> BoundedBelowScope.Create(sch, DayZero.NewStyle, new DateParts(GregorianSchema.MaxYear + 1, 12, 1), GregorianSchema.MaxYear))
        outOfRangeExn "parts" (fun () -> BoundedBelowScope.Create(sch, DayZero.NewStyle, new DateParts(1, 0, 1), 2))
        outOfRangeExn "parts" (fun () -> BoundedBelowScope.Create(sch, DayZero.NewStyle, new DateParts(1, 13, 1), 2))
        outOfRangeExn "parts" (fun () -> BoundedBelowScope.Create(sch, DayZero.NewStyle, new DateParts(1, 1, 0), 2))
        outOfRangeExn "parts" (fun () -> BoundedBelowScope.Create(sch, DayZero.NewStyle, new DateParts(1, 1, 32), 2))

    [<Fact>]
    let ``Create() throws for an invalid max year`` () =
        outOfRangeExn "year" (fun () -> BoundedBelowScope.Create(new GregorianSchema(), DayZero.NewStyle, new DateParts(1, 12, 1), GregorianSchema.MaxYear + 1))

    [<Fact>]
    let ``Create()`` () =
        let parts = new DateParts(5, 3, 13)
        let maxYear = 15
        let scope = BoundedBelowScope.Create(new GregorianSchema(), DayZero.NewStyle, parts, maxYear)
        let seg = scope.Segment

        scope.IsComplete |> nok
        seg.IsComplete |> nok

        seg.MinMaxDateParts.LowerValue === parts
        seg.MinMaxDateParts.UpperValue === new DateParts(maxYear, 12, 31)
        seg.SupportedYears.Max === maxYear

    [<Fact>]
    let ``Create() throws when the min date is the start of a year`` () =
        let parts = new DateParts(5, 1, 1)
        let maxYear = 15

        argExn "segment" (fun () -> BoundedBelowScope.Create(new GregorianSchema(), DayZero.NewStyle, parts, maxYear))

    [<Fact>]
    let ``StartingAt()`` () =
        let scope = BoundedBelowScope.StartingAt(new GregorianSchema(), DayZero.NewStyle, new DateParts(1, 12, 1))
        let seg = scope.Segment

        scope.IsComplete |> nok
        seg.IsComplete |> nok

        seg.SupportedYears.Max === GregorianSchema.MaxYear
