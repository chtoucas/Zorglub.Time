// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.FieldsTests

open System
open System.Runtime.CompilerServices

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

open type Zorglub.Time.Extensions.PartsExtensions

// TODO(code): test ToYemoda() & co, when the fields cannot be converted.

module TestCommon =
    //
    // Xunit
    //

    /// Data to be used when testing conversion to a core type fields using a
    /// short proleptic scope.
    [<Sealed>]
    type BadYearData() as self =
        inherit TheoryData<int>()
        do
            self.Add(Int32.MinValue)
            self.Add(ProlepticScope.MinYear - 1)
            self.Add(ProlepticScope.MaxYear + 1)
            self.Add(Int32.MaxValue)

    //
    // FsCheck
    //

    /// Represents an invalid field of type month, day or day of the month,
    /// its value is <= 0.
    [<Struct; IsReadOnly>]
    type BadField = { Value: int } with
        override x.ToString() = x.Value.ToString()
        static member op_Explicit (x: BadField) = x.Value

    /// Represents an valid field of type month, day or day of the month,
    /// its value is > 0.
    [<Struct; IsReadOnly>]
    type GoodField = { Value: int } with
        override x.ToString() = x.Value.ToString()
        static member op_Explicit (x: GoodField) = x.Value

    [<Sealed>]
    type Arbitraries =
        /// Gets an arbitrary for an invalid field, its value is <= 0.
        static member GetBadFieldArbitrary() =
            IntArbitraries.lessThanOrEqualToZero
            |> Arb.convert (fun i -> { BadField.Value = i }) int

        /// Gets an arbitrary for a valid field, its value is > 0.
        static member GetGoodFieldArbitrary() =
            IntArbitraries.greaterThanZero
            |> Arb.convert (fun i -> { GoodField.Value = i }) int

[<Properties(Arbitrary = [| typeof<TestCommon.Arbitraries> |] )>]
module DateFields =
    open TestCommon

    module Prelude =
        let centuryInfoData = YearNumberingDataSet.CenturyInfoData

        [<Fact>]
        let ``Default value`` () =
            let fields = Unchecked.defaultof<DateFields>
            let y, m, d = fields.Deconstruct()

            (y, m, d) === (0, 1, 1)

        [<Property>]
        let ``Constructor throws when "month" is out of range`` (m: BadField) =
            outOfRangeExn "month" (fun () -> new DateFields(1, m.Value, 1))

        [<Property>]
        let ``Constructor throws when "day" is out of range`` (d: BadField) =
            outOfRangeExn "day" (fun () -> new DateFields(1, 1, d.Value))

        [<Property>]
        let Constructor y (m: GoodField) (d: GoodField) =
            let fields = new DateFields(y, m.Value, d.Value)
            let a = fields.Year
            let b = fields.Month
            let c = fields.Day

            (a, b, c) = (y, m.Value, d.Value)

        [<Property>]
        let ``Constructor(Yemoda)`` ymd  =
            let fields = new DateFields(ymd)
            let y, m, d = ymd.Deconstruct()
            let a = fields.Year
            let b = fields.Month
            let c = fields.Day

            (a, b, c) = (y, m, d)

        [<Property>]
        let Deconstructor y (m: GoodField) (d: GoodField) =
            let fields = new DateFields(y, m.Value, d.Value)
            let a, b, c = fields.Deconstruct()

            (a, b, c) = (y, m.Value, d.Value)

        [<Theory>]
        [<InlineData(0, 1, 1, "01/01/0000")>] // default
        [<InlineData(7, 5, 3, "03/05/0007")>]
        [<InlineData(-7, 5, 3, "03/05/-0007")>]
        [<InlineData(2019, 13, 47, "47/13/2019")>]
        [<InlineData(-2019, 13, 47, "47/13/-2019")>]
        [<InlineData(10_000, 20_000, 30_000, "30000/20000/10000")>]
        [<InlineData(-10_000, 20_000, 30_000, "30000/20000/-10000")>]
        let ``ToString()`` y m d str =
            let fields = new DateFields(y, m, d)

            fields.ToString() === str

        //
        // Properties
        //

        [<Theory; MemberData(nameof(centuryInfoData))>]
        let ``Property CenturyOfEra`` (info: CenturyInfo) =
            let y, century, _ = info.Deconstruct()
            let fields = new DateFields(y, 1, 1)
            let centuryOfEra = Ord.Zeroth + century

            fields.CenturyOfEra === centuryOfEra

        [<Theory; MemberData(nameof(centuryInfoData))>]
        let ``Property Century`` (info: CenturyInfo) =
            let y, century, _ = info.Deconstruct()
            let fields = new DateFields(y, 1, 1)

            fields.Century === century

        [<Theory; MemberData(nameof(centuryInfoData))>]
        let ``Property YearOfEra`` (info: CenturyInfo) =
            let y = info.Year
            let fields = new DateFields(y, 1, 1)
            let yearOfEra = Ord.Zeroth + y

            fields.YearOfEra === yearOfEra

        [<Theory; MemberData(nameof(centuryInfoData))>]
        let ``Property YearOfCentury`` (info: CenturyInfo) =
            let y, _, yearOfCentury = info.Deconstruct()
            let fields = new DateFields(y, 1, 1)

            fields.YearOfCentury === int(yearOfCentury)

    module Conversions =
        let private dataSet = GregorianDataSet.Instance
        let private calendarDataSet = UnboundedGregorianDataSet.Instance

        let dateInfoData = dataSet.DateInfoData
        let invalidMonthFieldData = dataSet.InvalidMonthFieldData
        let invalidDayFieldData = dataSet.InvalidDayFieldData

        let scope = new ProlepticScope(new GregorianSchema(), calendarDataSet.Epoch)

        [<Property>]
        let ``ToYemoda()`` (ymd: Yemoda) =
            let y, m, d = ymd.Deconstruct()
            let fields = new DateFields(y, m, d)

            fields.ToYemoda() = ymd

        [<Fact>]
        let ``ToYemoda(scope) throws when "scope" is null`` () =
            let fields = new DateFields(2021, 12, 3)

            nullExn "scope" (fun () -> fields.ToYemoda(null))

        [<Theory; ClassData(typeof<BadYearData>)>]
        let ``ToYemoda(scope) throws when the property Year is out of range`` y =
            let fields = new DateFields(y, 1, 1)

            outOfRangeExn "year" (fun () -> fields.ToYemoda(scope))

        [<Theory; MemberData(nameof(invalidMonthFieldData))>]
        let ``ToYemoda(scope) throws when the property Month is out of range`` y m =
            if m >= 1 then
                let fields = new DateFields(y, m, 1)

                outOfRangeExn "month" (fun () -> fields.ToYemoda(scope))

        [<Theory; MemberData(nameof(invalidDayFieldData))>]
        let ``ToYemoda(scope) throws when the property Day is out of range`` y m d =
            if m >= 1 && d >= 1 then
                let fields = new DateFields(y, m, d)

                outOfRangeExn "day" (fun () -> fields.ToYemoda(scope))

        [<Theory; MemberData(nameof(dateInfoData))>]
        let ``ToYemoda(scope)`` (x: DateInfo) =
            let ymd = x.Yemoda
            let fields = new DateFields(ymd)

            fields.ToYemoda(scope) === ymd

    module Equality =
        open NonStructuralComparison

        /// Arbitrary for (x, y) where x and y are DateFields instances such that x <> y.
        let private xyArbitrary = Arb.fromGen <| gen {
            let! fields =
                Gen.elements [
                    // One different element.
                    (2, 1, 1); (1, 2, 1); (1, 1, 2);
                    // Two different elements.
                    (2, 2, 1); (1, 2, 2); (2, 1, 2);
                    // Three different elements.
                    (2, 2, 2) ]
                |> Gen.map (fun (y, m, d) -> new DateFields(y, m, d))
            return new DateFields(1, 1, 1), fields
        }

        // fsharplint:disable Hints
        [<Property>]
        let ``Equality when both operands are identical`` (x: DateFields) =
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
        let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: DateFields) =
            not (x.Equals(null))
            .&. not (x.Equals(new obj()))

        [<Property>]
        let ``GetHashCode() is invariant`` (x: DateFields) =
            x.GetHashCode() = x.GetHashCode()

[<Properties(Arbitrary = [| typeof<TestCommon.Arbitraries> |] )>]
module MonthFields =
    open TestCommon

    module Prelude =
        let centuryInfoData = YearNumberingDataSet.CenturyInfoData

        [<Fact>]
        let ``Default value`` () =
            let fields = Unchecked.defaultof<MonthFields>
            let y, m = fields.Deconstruct()

            (y, m) === (0, 1)

        [<Property>]
        let ``Constructor throws when "month" is out of range`` (m: BadField) =
            outOfRangeExn "month" (fun () -> new MonthFields(1, m.Value))

        [<Property>]
        let Constructor y (m: GoodField) =
            let fields = new MonthFields(y, m.Value)
            let a = fields.Year
            let b = fields.Month

            (a, b) = (y, m.Value)

        [<Property>]
        let ``Constructor(Yemo)`` ym  =
            let fields = new MonthFields(ym)
            let y, m = ym.Deconstruct()
            let a = fields.Year
            let b = fields.Month

            (a, b) = (y, m)

        [<Property>]
        let Deconstructor y (m: GoodField) =
            let fields = new MonthFields(y, m.Value)
            let a, b = fields.Deconstruct()

            (a, b) = (y, m.Value)

        [<Theory>]
        [<InlineData(0, 1, "01/0000")>] // default
        [<InlineData(7, 5, "05/0007")>]
        [<InlineData(-7, 5, "05/-0007")>]
        [<InlineData(2019, 13, "13/2019")>]
        [<InlineData(-2019, 13, "13/-2019")>]
        [<InlineData(10_000, 20_000, "20000/10000")>]
        [<InlineData(-10_000, 20_000, "20000/-10000")>]
        let ``ToString()`` y m str =
            let fields = new MonthFields(y, m)

            fields.ToString() === str

        //
        // Properties
        //

        [<Theory; MemberData(nameof(centuryInfoData))>]
        let ``Property CenturyOfEra`` (info: CenturyInfo) =
            let y, century, _ = info.Deconstruct()
            let fields = new MonthFields(y, 1)
            let centuryOfEra = Ord.Zeroth + century

            fields.CenturyOfEra === centuryOfEra

        [<Theory; MemberData(nameof(centuryInfoData))>]
        let ``Property Century`` (info: CenturyInfo) =
            let y, century, _ = info.Deconstruct()
            let fields = new MonthFields(y, 1)

            fields.Century === century

        [<Theory; MemberData(nameof(centuryInfoData))>]
        let ``Property YearOfEra`` (info: CenturyInfo) =
            let y = info.Year
            let fields = new MonthFields(y, 1)
            let yearOfEra = Ord.Zeroth + y

            fields.YearOfEra === yearOfEra

        [<Theory; MemberData(nameof(centuryInfoData))>]
        let ``Property YearOfCentury`` (info: CenturyInfo) =
            let y, _, yearOfCentury = info.Deconstruct()
            let fields = new MonthFields(y, 1)

            fields.YearOfCentury === int(yearOfCentury)

    module Conversions =
        let private dataSet = GregorianDataSet.Instance
        let private calendarDataSet = UnboundedGregorianDataSet.Instance

        let monthInfoData = dataSet.MonthInfoData
        let invalidMonthFieldData = dataSet.InvalidMonthFieldData

        let scope = new ProlepticScope(new GregorianSchema(), calendarDataSet.Epoch)

        [<Property>]
        let ``ToYemo()`` (ym: Yemo) =
            let y, m = ym.Deconstruct()
            let fields = new MonthFields(y, m)

            fields.ToYemo() = ym

        [<Fact>]
        let ``ToYemo(scope) throws when "scope" is null`` () =
            let fields = new MonthFields(2021, 12)

            nullExn "scope" (fun () -> fields.ToYemo(null))

        [<Theory; ClassData(typeof<BadYearData>)>]
        let ``ToYemo(scope) throws when the property Year is out of range`` y =
            let fields = new MonthFields(y, 1)

            outOfRangeExn "year" (fun () -> fields.ToYemo(scope))

        [<Theory; MemberData(nameof(invalidMonthFieldData))>]
        let ``ToYemo(scope) throws when the property Month is out of range`` y m =
            if m >= 1 then
                let fields = new MonthFields(y, m)

                outOfRangeExn "month" (fun () -> fields.ToYemo(scope))

        [<Theory; MemberData(nameof(monthInfoData))>]
        let ``ToYemo(scope)`` (x: MonthInfo) =
            let ym = x.Yemo
            let fields = new MonthFields(ym)

            fields.ToYemo(scope) === ym

    module Equality =
        open NonStructuralComparison

        /// Arbitrary for (x, y) where x and y are MonthFields instances such that x <> y.
        let private xyArbitrary = Arb.fromGen <| gen {
            let! fields =
                Gen.elements [ (2, 1); (1, 2); (2, 2) ]
                |> Gen.map (fun (y, m) -> new MonthFields(y, m))
            return new MonthFields(1, 1), fields
        }

        // fsharplint:disable Hints
        [<Property>]
        let ``Equality when both operands are identical`` (x: MonthFields) =
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
        let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: MonthFields) =
            not (x.Equals(null))
            .&. not (x.Equals(new obj()))

        [<Property>]
        let ``GetHashCode() is invariant`` (x: MonthFields) =
            x.GetHashCode() = x.GetHashCode()

[<Properties(Arbitrary = [| typeof<TestCommon.Arbitraries> |] )>]
module OrdinalFields =
    open TestCommon

    module Prelude =
        let centuryInfoData = YearNumberingDataSet.CenturyInfoData

        [<Fact>]
        let ``Default value`` () =
            let fields = Unchecked.defaultof<OrdinalFields>
            let y, doy = fields.Deconstruct()

            (y, doy) === (0, 1)

        [<Property>]
        let ``Constructor throws when "dayOfYear" is out of range`` (doy: BadField) =
            outOfRangeExn "dayOfYear" (fun () -> new OrdinalFields(1, doy.Value))

        [<Property>]
        let Constructor y (doy: GoodField) =
            let fields = new OrdinalFields(y, doy.Value)
            let a = fields.Year
            let b = fields.DayOfYear

            (a, b) = (y, doy.Value)

        [<Property>]
        let ``Constructor(Yedoy)`` ydoy  =
            let fields = new OrdinalFields(ydoy)
            let y, doy = ydoy.Deconstruct()
            let a = fields.Year
            let b = fields.DayOfYear

            (a, b) = (y, doy)

        [<Property>]
        let Deconstructor y (doy: GoodField) =
            let fields = new OrdinalFields(y, doy.Value)
            let a, b = fields.Deconstruct()

            (a, b) = (y, doy.Value)

        [<Theory>]
        [<InlineData(0, 1, "001/0000")>] // default
        [<InlineData(7, 5, "005/0007")>]
        [<InlineData(-7, 5, "005/-0007")>]
        [<InlineData(2019, 133, "133/2019")>]
        [<InlineData(-2019, 133, "133/-2019")>]
        [<InlineData(10_000, 20_000, "20000/10000")>]
        [<InlineData(-10_000, 20_000, "20000/-10000")>]
        let ``ToString()`` y doy str =
            let fields = new OrdinalFields(y, doy)

            fields.ToString() === str

        //
        // Properties
        //

        [<Theory; MemberData(nameof(centuryInfoData))>]
        let ``Property CenturyOfEra`` (info: CenturyInfo) =
            let y, century, _ = info.Deconstruct()
            let fields = new OrdinalFields(y, 1)
            let centuryOfEra = Ord.Zeroth + century

            fields.CenturyOfEra === centuryOfEra

        [<Theory; MemberData(nameof(centuryInfoData))>]
        let ``Property Century`` (info: CenturyInfo) =
            let y, century, _ = info.Deconstruct()
            let fields = new OrdinalFields(y, 1)

            fields.Century === century

        [<Theory; MemberData(nameof(centuryInfoData))>]
        let ``Property YearOfEra`` (info: CenturyInfo) =
            let y = info.Year
            let fields = new OrdinalFields(y, 1)
            let yearOfEra = Ord.Zeroth + y

            fields.YearOfEra === yearOfEra

        [<Theory; MemberData(nameof(centuryInfoData))>]
        let ``Property YearOfCentury`` (info: CenturyInfo) =
            let y, _, yearOfCentury = info.Deconstruct()
            let fields = new OrdinalFields(y, 1)

            fields.YearOfCentury === int(yearOfCentury)

    module Conversions =
        let private dataSet = GregorianDataSet.Instance
        let private calendarDataSet = UnboundedGregorianDataSet.Instance

        let dateInfoData = dataSet.DateInfoData
        let invalidDayOfYearFieldData = dataSet.InvalidDayOfYearFieldData

        let scope = new ProlepticScope(new GregorianSchema(), calendarDataSet.Epoch)

        [<Property>]
        let ``ToYedoy()`` (ydoy: Yedoy) =
            let y, doy = ydoy.Deconstruct()
            let fields = new OrdinalFields(y, doy)

            fields.ToYedoy() = ydoy

        [<Fact>]
        let ``ToYedoy(scope) throws when "scope" is null`` () =
            let fields = new OrdinalFields(2021, 255)

            nullExn "scope" (fun () -> fields.ToYedoy(null))

        [<Theory; ClassData(typeof<BadYearData>)>]
        let ``ToYedoy(scope) throws when the property Year is out of range`` y =
            let fields = new OrdinalFields(y, 1)

            outOfRangeExn "year" (fun () -> fields.ToYedoy(scope))

        [<Theory; MemberData(nameof(invalidDayOfYearFieldData))>]
        let ``ToYedoy(scope) throws when the property DayOfYear is out of range`` y doy =
            if doy >= 1 then
                let fields = new OrdinalFields(y, doy)

                outOfRangeExn "dayOfYear" (fun () -> fields.ToYedoy(scope))

        [<Theory; MemberData(nameof(dateInfoData))>]
        let ``ToYedoy(scope)`` (x: DateInfo) =
            let ydoy = x.Yedoy
            let fields = new OrdinalFields(ydoy)

            fields.ToYedoy(scope) === ydoy

    module Equality =
        open NonStructuralComparison

        /// Arbitrary for (x, y) where x and y are OrdinalFields instances such that x <> y.
        let private xyArbitrary = Arb.fromGen <| gen {
            let! fields =
                Gen.elements [ (2, 1); (1, 2); (2, 2) ]
                |> Gen.map (fun (y, doy) -> new OrdinalFields(y, doy))
            return new OrdinalFields(1, 1), fields
        }

        // fsharplint:disable Hints
        [<Property>]
        let ``Equality when both operands are identical`` (x: OrdinalFields) =
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
        let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: OrdinalFields) =
            not (x.Equals(null))
            .&. not (x.Equals(new obj()))

        [<Property>]
        let ``GetHashCode() is invariant`` (x: OrdinalFields) =
            x.GetHashCode() = x.GetHashCode()
