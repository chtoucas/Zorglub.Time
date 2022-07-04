﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.PartsTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Data.Unbounded

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology.Scopes

open Xunit
open FsCheck
open FsCheck.Xunit

// TODO(code): test ToYemoda() & co, when the parts cannot be converted.

module TestCommon =
    //
    // Xunit
    //

    /// Data to be used when testing conversion to a core type parts using a
    /// short proleptic scope.
    [<Sealed>]
    type BadYearData() as self =
        inherit TheoryData<int>()
        do
            self.Add(Int32.MinValue)
            self.Add(ProlepticShortScope.MinYear - 1)
            self.Add(ProlepticShortScope.MaxYear + 1)
            self.Add(Int32.MaxValue)

module DateParts =
    open TestCommon

    module Prelude =
        [<Fact>]
        let ``Default value`` () =
            let parts = Unchecked.defaultof<DateParts>
            let y, m, d = parts.Deconstruct()

            (y, m, d) === (0, 0, 0)

        [<Property>]
        let Constructor y m d =
            let parts = new DateParts(y, m, d)
            let a = parts.Year
            let b = parts.Month
            let c = parts.Day

            (a, b, c) = (y, m, d)

        [<Property>]
        let ``Constructor(Yemoda)`` ymd  =
            let parts = new DateParts(ymd)
            let y, m, d = ymd.Deconstruct()
            let a = parts.Year
            let b = parts.Month
            let c = parts.Day

            (a, b, c) = (y, m, d)

        [<Property>]
        let Deconstructor y m d =
            let parts = new DateParts(y, m, d)
            let a, b, c = parts.Deconstruct()

            (a, b, c) = (y, m, d)

        [<Theory>]
        [<InlineData(0, 1, 1, "01/01/0000")>] // default
        [<InlineData(7, 5, 3, "03/05/0007")>]
        [<InlineData(-7, 5, 3, "03/05/-0007")>]
        [<InlineData(2019, 13, 47, "47/13/2019")>]
        [<InlineData(-2019, 13, 47, "47/13/-2019")>]
        [<InlineData(10_000, 20_000, 30_000, "30000/20000/10000")>]
        [<InlineData(-10_000, 20_000, 30_000, "30000/20000/-10000")>]
        let ``ToString()`` y m d str =
            let parts = new DateParts(y, m, d)

            parts.ToString() === str

    module Conversions =
        let private dataSet = GregorianDataSet.Instance
        let private calendarDataSet = UnboundedGregorianDataSet.Instance

        let dateInfoData = dataSet.DateInfoData
        let invalidMonthFieldData = dataSet.InvalidMonthFieldData
        let invalidDayFieldData = dataSet.InvalidDayFieldData

        let scope = new GregorianProlepticShortScope(new GregorianSchema(), calendarDataSet.Epoch)

        [<Property>]
        let ``ToYemoda()`` (ymd: Yemoda) =
            let y, m, d = ymd.Deconstruct()
            let parts = new DateParts(y, m, d)

            parts.ToYemoda() = ymd

        [<Fact>]
        let ``ToYemoda(scope) throws when "scope" is null`` () =
            let parts = new DateParts(2021, 12, 3)

            nullExn "scope" (fun () -> parts.ToYemoda(null))

        [<Theory; ClassData(typeof<BadYearData>)>]
        let ``ToYemoda(scope) throws when the property Year is out of range`` y =
            let parts = new DateParts(y, 1, 1)

            outOfRangeExn "year" (fun () -> parts.ToYemoda(scope))

        [<Theory; MemberData(nameof(invalidMonthFieldData))>]
        let ``ToYemoda(scope) throws when the property Month is out of range`` y m =
            if m >= 1 then
                let parts = new DateParts(y, m, 1)

                outOfRangeExn "month" (fun () -> parts.ToYemoda(scope))

        [<Theory; MemberData(nameof(invalidDayFieldData))>]
        let ``ToYemoda(scope) throws when the property Day is out of range`` y m d =
            if m >= 1 && d >= 1 then
                let parts = new DateParts(y, m, d)

                outOfRangeExn "day" (fun () -> parts.ToYemoda(scope))

        [<Theory; MemberData(nameof(dateInfoData))>]
        let ``ToYemoda(scope)`` (x: DateInfo) =
            let ymd = x.Yemoda
            let parts = new DateParts(ymd)

            parts.ToYemoda(scope) === ymd

    module Equality =
        open NonStructuralComparison

        /// Arbitrary for (x, y) where x and y are DateParts instances such that x <> y.
        let private xyArbitrary = Arb.fromGen <| gen {
            let! parts =
                Gen.elements [
                    // One different element.
                    (2, 1, 1); (1, 2, 1); (1, 1, 2);
                    // Two different elements.
                    (2, 2, 1); (1, 2, 2); (2, 1, 2);
                    // Three different elements.
                    (2, 2, 2) ]
                |> Gen.map (fun (y, m, d) -> new DateParts(y, m, d))
            return new DateParts(1, 1, 1), parts
        }

        // fsharplint:disable Hints
        [<Property>]
        let ``Equality when both operands are identical`` (x: DateParts) =
            x = x
            .&. not (x <> x)
            .&. x.Equals(x)
            .&. x.Equals(x :> obj)

        [<Property>]
        let ``Equality when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            not (x = y)
            .&. (x <> y)
            .&. not (x.Equals(y))
            .&. not (x.Equals(y :> obj))
            // Flipped
            .&. not (y = x)
            .&. (y <> x)
            .&. not (y.Equals(x))
            .&. not (y.Equals(x :> obj))
        // fsharplint:enable

        [<Property>]
        let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: DateParts) =
            not (x.Equals(null))
            .&. not (x.Equals(new obj()))

        [<Property>]
        let ``GetHashCode() is invariant`` (x: DateParts) =
            x.GetHashCode() = x.GetHashCode()

module MonthParts =
    open TestCommon

    module Prelude =
        [<Fact>]
        let ``Default value`` () =
            let parts = Unchecked.defaultof<MonthParts>
            let y, m = parts.Deconstruct()

            (y, m) === (0, 0)

        [<Property>]
        let Constructor y m =
            let parts = new MonthParts(y, m)
            let a = parts.Year
            let b = parts.Month

            (a, b) = (y, m)

        [<Property>]
        let ``Constructor(Yemo)`` ym  =
            let parts = new MonthParts(ym)
            let y, m = ym.Deconstruct()
            let a = parts.Year
            let b = parts.Month

            (a, b) = (y, m)

        [<Property>]
        let Deconstructor y m =
            let parts = new MonthParts(y, m)
            let a, b = parts.Deconstruct()

            (a, b) = (y, m)

        [<Theory>]
        [<InlineData(0, 1, "01/0000")>] // default
        [<InlineData(7, 5, "05/0007")>]
        [<InlineData(-7, 5, "05/-0007")>]
        [<InlineData(2019, 13, "13/2019")>]
        [<InlineData(-2019, 13, "13/-2019")>]
        [<InlineData(10_000, 20_000, "20000/10000")>]
        [<InlineData(-10_000, 20_000, "20000/-10000")>]
        let ``ToString()`` y m str =
            let parts = new MonthParts(y, m)

            parts.ToString() === str

    module Conversions =
        let private dataSet = GregorianDataSet.Instance
        let private calendarDataSet = UnboundedGregorianDataSet.Instance

        let monthInfoData = dataSet.MonthInfoData
        let invalidMonthFieldData = dataSet.InvalidMonthFieldData

        let scope = new GregorianProlepticShortScope(new GregorianSchema(), calendarDataSet.Epoch)

        [<Property>]
        let ``ToYemo()`` (ym: Yemo) =
            let y, m = ym.Deconstruct()
            let parts = new MonthParts(y, m)

            parts.ToYemo() = ym

        [<Fact>]
        let ``ToYemo(scope) throws when "scope" is null`` () =
            let parts = new MonthParts(2021, 12)

            nullExn "scope" (fun () -> parts.ToYemo(null))

        [<Theory; ClassData(typeof<BadYearData>)>]
        let ``ToYemo(scope) throws when the property Year is out of range`` y =
            let parts = new MonthParts(y, 1)

            outOfRangeExn "year" (fun () -> parts.ToYemo(scope))

        [<Theory; MemberData(nameof(invalidMonthFieldData))>]
        let ``ToYemo(scope) throws when the property Month is out of range`` y m =
            if m >= 1 then
                let parts = new MonthParts(y, m)

                outOfRangeExn "month" (fun () -> parts.ToYemo(scope))

        [<Theory; MemberData(nameof(monthInfoData))>]
        let ``ToYemo(scope)`` (x: MonthInfo) =
            let ym = x.Yemo
            let parts = new MonthParts(ym)

            parts.ToYemo(scope) === ym

    module Equality =
        open NonStructuralComparison

        /// Arbitrary for (x, y) where x and y are MonthParts instances such that x <> y.
        let private xyArbitrary = Arb.fromGen <| gen {
            let! parts =
                Gen.elements [ (2, 1); (1, 2); (2, 2) ]
                |> Gen.map (fun (y, m) -> new MonthParts(y, m))
            return new MonthParts(1, 1), parts
        }

        // fsharplint:disable Hints
        [<Property>]
        let ``Equality when both operands are identical`` (x: MonthParts) =
            x = x
            .&. not (x <> x)
            .&. x.Equals(x)
            .&. x.Equals(x :> obj)

        [<Property>]
        let ``Equality when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            not (x = y)
            .&. (x <> y)
            .&. not (x.Equals(y))
            .&. not (x.Equals(y :> obj))
            // Flipped
            .&. not (y = x)
            .&. (y <> x)
            .&. not (y.Equals(x))
            .&. not (y.Equals(x :> obj))
        // fsharplint:enable

        [<Property>]
        let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: MonthParts) =
            not (x.Equals(null))
            .&. not (x.Equals(new obj()))

        [<Property>]
        let ``GetHashCode() is invariant`` (x: MonthParts) =
            x.GetHashCode() = x.GetHashCode()

module OrdinalParts =
    open TestCommon

    module Prelude =
        [<Fact>]
        let ``Default value`` () =
            let parts = Unchecked.defaultof<OrdinalParts>
            let y, doy = parts.Deconstruct()

            (y, doy) === (0, 0)

        [<Property>]
        let Constructor y doy =
            let parts = new OrdinalParts(y, doy)
            let a = parts.Year
            let b = parts.DayOfYear

            (a, b) = (y, doy)

        [<Property>]
        let ``Constructor(Yedoy)`` ydoy  =
            let parts = new OrdinalParts(ydoy)
            let y, doy = ydoy.Deconstruct()
            let a = parts.Year
            let b = parts.DayOfYear

            (a, b) = (y, doy)

        [<Property>]
        let Deconstructor y doy =
            let parts = new OrdinalParts(y, doy)
            let a, b = parts.Deconstruct()

            (a, b) = (y, doy)

        [<Theory>]
        [<InlineData(0, 1, "001/0000")>] // default
        [<InlineData(7, 5, "005/0007")>]
        [<InlineData(-7, 5, "005/-0007")>]
        [<InlineData(2019, 133, "133/2019")>]
        [<InlineData(-2019, 133, "133/-2019")>]
        [<InlineData(10_000, 20_000, "20000/10000")>]
        [<InlineData(-10_000, 20_000, "20000/-10000")>]
        let ``ToString()`` y doy str =
            let parts = new OrdinalParts(y, doy)

            parts.ToString() === str

    module Conversions =
        let private dataSet = GregorianDataSet.Instance
        let private calendarDataSet = UnboundedGregorianDataSet.Instance

        let dateInfoData = dataSet.DateInfoData
        let invalidDayOfYearFieldData = dataSet.InvalidDayOfYearFieldData

        let scope = new GregorianProlepticShortScope(new GregorianSchema(), calendarDataSet.Epoch)

        [<Property>]
        let ``ToYedoy()`` (ydoy: Yedoy) =
            let y, doy = ydoy.Deconstruct()
            let parts = new OrdinalParts(y, doy)

            parts.ToYedoy() = ydoy

        [<Fact>]
        let ``ToYedoy(scope) throws when "scope" is null`` () =
            let parts = new OrdinalParts(2021, 255)

            nullExn "scope" (fun () -> parts.ToYedoy(null))

        [<Theory; ClassData(typeof<BadYearData>)>]
        let ``ToYedoy(scope) throws when the property Year is out of range`` y =
            let parts = new OrdinalParts(y, 1)

            outOfRangeExn "year" (fun () -> parts.ToYedoy(scope))

        [<Theory; MemberData(nameof(invalidDayOfYearFieldData))>]
        let ``ToYedoy(scope) throws when the property DayOfYear is out of range`` y doy =
            if doy >= 1 then
                let parts = new OrdinalParts(y, doy)

                outOfRangeExn "dayOfYear" (fun () -> parts.ToYedoy(scope))

        [<Theory; MemberData(nameof(dateInfoData))>]
        let ``ToYedoy(scope)`` (x: DateInfo) =
            let ydoy = x.Yedoy
            let parts = new OrdinalParts(ydoy)

            parts.ToYedoy(scope) === ydoy

    module Equality =
        open NonStructuralComparison

        /// Arbitrary for (x, y) where x and y are OrdinalParts instances such that x <> y.
        let private xyArbitrary = Arb.fromGen <| gen {
            let! parts =
                Gen.elements [ (2, 1); (1, 2); (2, 2) ]
                |> Gen.map (fun (y, doy) -> new OrdinalParts(y, doy))
            return new OrdinalParts(1, 1), parts
        }

        // fsharplint:disable Hints
        [<Property>]
        let ``Equality when both operands are identical`` (x: OrdinalParts) =
            x = x
            .&. not (x <> x)
            .&. x.Equals(x)
            .&. x.Equals(x :> obj)

        [<Property>]
        let ``Equality when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            not (x = y)
            .&. (x <> y)
            .&. not (x.Equals(y))
            .&. not (x.Equals(y :> obj))
            // Flipped
            .&. not (y = x)
            .&. (y <> x)
            .&. not (y.Equals(x))
            .&. not (y.Equals(x :> obj))
        // fsharplint:enable

        [<Property>]
        let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: OrdinalParts) =
            not (x.Equals(null))
            .&. not (x.Equals(new obj()))

        [<Property>]
        let ``GetHashCode() is invariant`` (x: OrdinalParts) =
            x.GetHashCode() = x.GetHashCode()
