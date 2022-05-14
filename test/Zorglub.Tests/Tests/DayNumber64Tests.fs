// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.DayNumber64Tests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Hemerology

open Xunit

module Prelude =
    [<Fact>]
    let ``Age of the universe`` () =
        // ~14 billion Julian years.
        let aof = DayNumber64.MinSupportedYear
        let dayNumber = DayNumber64.FromJulianParts(aof, 1, 1)

        dayNumber.DaysSinceZero === -5_113_500_000_002L

    //
    // Properties of DayZero64
    //

    [<Fact>]
    let ``Static property DayZero64.NewStyle`` () =
        DayZero64.NewStyle === DayNumber64.Zero
        DayZero64.NewStyle.Ordinal === Ord64.First

        DayZero64.NewStyle === DayNumber64.FromDayNumber DayZero.NewStyle

    [<Fact>]
    let ``Static property DayZero64.OldStyle`` () =
        DayZero64.OldStyle === DayNumber64.Zero - 2L
        DayZero64.OldStyle.Ordinal === Ord64.First - 2L

        DayZero64.OldStyle === DayNumber64.FromDayNumber DayZero.OldStyle

    [<Fact>]
    let ``Static property DayZero64.RataDie`` () =
        DayZero64.RataDie === DayNumber64.Zero - 1L
        DayZero64.RataDie.Ordinal === Ord64.First - 1L

        DayZero64.RataDie === DayNumber64.FromDayNumber DayZero.RataDie

module Postlude =
    /// Compare the core properties.
    let rec private compareTypes (dayNumber: DayNumber64) (date: CivilDate) =
        let y, m, d = dayNumber.GetGregorianParts()
        let passed =
            int y = date.Year
            && m = date.Month
            && d = date.Day
            && dayNumber.DayOfWeek = date.DayOfWeek
            && dayNumber.IsoDayOfWeek = date.IsoDayOfWeek

        if passed then
            if dayNumber.DaysSinceZero = CivilDate.MaxDaysSinceEpoch then
                (true, "OK")
            else
                compareTypes (dayNumber.NextDay()) (date.NextDay())
        else
            (false, sprintf "First failure: %O." dayNumber)

    [<Fact>]
    [<TestPerformance(TestPerformance.SlowUnit)>]
    [<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
    let ``Deep comparison between DayNumber64 and CivilDate`` () =
        // NB: both start on Monday January 1, 1 (CE).
        compareTypes DayNumber64.Zero CivilDate.MinValue |> Assert.True
