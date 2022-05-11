// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.ShortScopeTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology.Scopes

open Xunit

let private epoch = DayZero.OldStyle

module Prelude =
    [<Fact>]
    let ``Constructor throws when "schema" is null`` () =
        nullExn "schema" (fun () -> new FauxShortScope(null, epoch, 1))

    [<Fact>]
    let ``Constructor throws when schema.MinYear > minYear`` () =
        let range = Range.Create(1, ShortScope.MaxYear)
        let sch = new FauxCalendricalSchema(range)

        // Scope range [0..9999] is not a subset of the schema range [1..9999].
        argExn "schema" (fun () -> new FauxShortScope(sch, epoch, range.Min - 1))

    [<Fact>]
    let ``Constructor throws when schema.MaxYear < 9999`` () =
        let range = Range.Create(1, ShortScope.MaxYear - 1)
        let sch = new FauxCalendricalSchema(range)

        // Scope range [1..9999] is not a subset of the schema range [1..9998].
        argExn "schema" (fun () -> new FauxShortScope(sch, epoch, range.Min))

    //
    // Properties
    //

    [<Fact>]
    let ``Property Epoch`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let scope = new FauxShortScope(FauxCalendricalSchema.Default, epoch, 1)

        scope.Epoch === epoch

    [<Fact>]
    let ``Property Domain`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let minYear = 0
        let sch = new GregorianSchema()
        let scope = new FauxShortScope(sch, epoch, minYear)
        let minDayNumber = epoch + sch.GetStartOfYear(minYear)
        let maxDayNumber = epoch + sch.GetEndOfYear(ShortScope.MaxYear)
        let range = Range.Create(minDayNumber, maxDayNumber)

        scope.Domain === range

    [<Fact>]
    let ``Property SupportedYears`` () =
        let scope = new FauxShortScope(FauxCalendricalSchema.Default, epoch, 123)
        let range = Range.Create(123, ShortScope.MaxYear)

        scope.SupportedYears === range


