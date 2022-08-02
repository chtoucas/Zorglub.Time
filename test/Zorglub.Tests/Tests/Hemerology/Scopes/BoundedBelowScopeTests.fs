// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.BoundedBelowScopeTests

open System

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology.Scopes

open Xunit

module Factories =
    [<Fact>]
    let ``Constructor throws for null schema`` () =
        nullExn "schema" (fun () -> new BoundedBelowScope(null, DayZero.NewStyle, new DateParts(1, 1, 1), 2))

    [<Fact>]
    let ``Constructor throws for an invalid date`` () =
        let sch = new GregorianSchema()

        outOfRangeExn "parts" (fun () -> new BoundedBelowScope(sch, DayZero.NewStyle, new DateParts(GregorianSchema.MinYear - 1, 12, 1), GregorianSchema.MaxYear))
        outOfRangeExn "parts" (fun () -> new BoundedBelowScope(sch, DayZero.NewStyle, new DateParts(GregorianSchema.MaxYear + 1, 12, 1), GregorianSchema.MaxYear))
        outOfRangeExn "parts" (fun () -> new BoundedBelowScope(sch, DayZero.NewStyle, new DateParts(1, 0, 1), 2))
        outOfRangeExn "parts" (fun () -> new BoundedBelowScope(sch, DayZero.NewStyle, new DateParts(1, 13, 1), 2))
        outOfRangeExn "parts" (fun () -> new BoundedBelowScope(sch, DayZero.NewStyle, new DateParts(1, 1, 0), 2))
        outOfRangeExn "parts" (fun () -> new BoundedBelowScope(sch, DayZero.NewStyle, new DateParts(1, 1, 32), 2))

    [<Fact>]
    let ``Constructor throws for an invalid max year`` () =
        outOfRangeExn "year" (fun () -> new BoundedBelowScope(new GregorianSchema(), DayZero.NewStyle, new DateParts(1, 12, 1), GregorianSchema.MaxYear + 1))

    [<Fact>]
    let ``Constructor when max year is null`` () =
        let scope = new BoundedBelowScope(new GregorianSchema(), DayZero.NewStyle, new DateParts(1, 12, 1), Nullable())
        let seg = scope.Segment

        scope.IsComplete |> nok
        seg.IsComplete |> nok

        seg.SupportedYears.Max === GregorianSchema.MaxYear

    [<Fact>]
    let ``Constructor`` () =
        let parts = new DateParts(5, 3, 13)
        let maxYear = 15
        let scope = new BoundedBelowScope(new GregorianSchema(), DayZero.NewStyle, parts, maxYear)
        let seg = scope.Segment

        scope.IsComplete |> nok
        seg.IsComplete |> nok

        seg.MinMaxDateParts.LowerValue === parts
        seg.MinMaxDateParts.UpperValue === new DateParts(maxYear, 12, 31)
        seg.SupportedYears.Max === maxYear

    [<Fact>]
    let ``Constructor when the min date is the start of a year`` () =
        let parts = new DateParts(5, 1, 1)
        let maxYear = 15
        let scope = new BoundedBelowScope(new GregorianSchema(), DayZero.NewStyle, parts, maxYear)
        let seg = scope.Segment

        scope.IsComplete |> ok
        seg.IsComplete |> ok

        seg.MinMaxDateParts.LowerValue === parts
        seg.MinMaxDateParts.UpperValue === new DateParts(maxYear, 12, 31)
        seg.SupportedYears.Max === maxYear
