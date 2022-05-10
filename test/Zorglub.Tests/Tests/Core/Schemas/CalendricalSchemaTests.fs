// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.CalendricalSchemaTests

open Zorglub.Testing
open Zorglub.Time.Core
open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Validation

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor expects minDaysInYear to be > 0`` () =
        outOfRangeExn "minDaysInYear" (fun () -> FauxCalendricalSchema.WithMinDaysInYear(0))
        outOfRangeExn "minDaysInYear" (fun () -> FauxCalendricalSchema.WithMinDaysInYear(-1))

    [<Fact>]
    let ``Constructor expects minDaysInMonth to be > 0`` () =
        outOfRangeExn "minDaysInMonth" (fun () -> FauxCalendricalSchema.WithMinDaysInMonth(0))
        outOfRangeExn "minDaysInMonth" (fun () -> FauxCalendricalSchema.WithMinDaysInMonth(-1))

    [<Fact>]
    let ``Constructor succeeds with minDaysInYear > 0`` () =
        FauxCalendricalSchema.WithMinDaysInYear(1) |> ignore

    [<Fact>]
    let ``Constructor succeeds with minDaysInMonth > 0`` () =
        FauxCalendricalSchema.WithMinDaysInMonth(1) |> ignore

    [<Fact>]
    let ``Property PreValidator: default value, repeated`` () =
        let sch = FauxCalendricalSchema.Default

        let validator1 = sch.PreValidator
        validator1 |> is<DefaultPreValidator>

        let validator2 = sch.PreValidator
        validator2 |> is<DefaultPreValidator>

        validator2 === validator1

    [<Fact>]
    let ``Property Arithmetic: default value, repeated`` () =
        let sch = FauxCalendricalSchema.Default

        let arith1 = sch.Arithmetic
        arith1 |> is<DefaultArithmetic>

        let arith2 = sch.Arithmetic
        arith2 |> is<DefaultArithmetic>

        arith2 === arith1

    [<Fact>]
    let ``Property Profile: default value, repeated`` () =
        let sch = FauxCalendricalSchema.Default

        let profile1 = sch.Profile
        let profile2 = sch.Profile

        // The faux schema is not regular.
        profile1 === CalendricalProfile.Other
        profile2 === profile1

module SystemSchemaPrelude =
    [<Fact>]
    let ``Constructor expects supportedYears.Min to be >= MaxSupportedYears.Min`` () =
        let maxrange = SystemSchema.MaxSupportedYears
        let range = maxrange.WithMin(maxrange.Min - 1)
        outOfRangeExn "supportedYears" (fun () -> new FauxSystemSchema(range))

    [<Fact>]
    let ``Constructor expects supportedYears.Max to be <= MaxSupportedYears.Max`` () =
        let maxrange = SystemSchema.MaxSupportedYears
        let range = maxrange.WithMax(maxrange.Max + 1)
        outOfRangeExn "supportedYears" (fun () -> new FauxSystemSchema(range))

    [<Fact>]
    let ``Constructor succeeds with supportedYears = DefaultSupportedYears`` () =
        new FauxSystemSchema(SystemSchema.DefaultSupportedYears) |> ignore

    [<Fact>]
    let ``Constructor succeeds with supportedYears = MaxSupportedYears`` () =
        new FauxSystemSchema(SystemSchema.MaxSupportedYears) |> ignore

    [<Fact>]
    let ``Constructor throws when supportedYearsCore is not a superset of supportedYears`` () =
        let range = Range.Create(1, 100)
        let rangeCore = Range.Create(2, 99)
        argExn "value" (fun () -> new FauxSystemSchema(range, rangeCore))

    [<Fact>]
    let ``Constructor succeeds when supportedYearsCore = supportedYears`` () =
        let range = Range.Create(1, 100)
        new FauxSystemSchema(range, range) |> ignore

    [<Fact>]
    let ``Constructor succeeds when supportedYearsCore is a superset of supportedYears`` () =
        let range = Range.Create(1, 100)
        let rangeCore = Range.Create(0, 101)
        new FauxSystemSchema(range, rangeCore) |> ignore

    [<Fact>]
    let ``Default value for SupportedYearsCore is any int`` () =
        FauxSystemSchema.Default.SupportedYearsCore === Range.Maximal32

    [<Fact>]
    let ``Default value for SupportedYears is DefaultSupportedYears`` () =
        FauxSystemSchema.Default.SupportedYears === SystemSchema.DefaultSupportedYears
