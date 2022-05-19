// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.DayNumberTests

open System
open System.Runtime.CompilerServices

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Data.Unbounded

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology
open Zorglub.Time.Simple

open Xunit
open FsCheck
open FsCheck.Xunit

open type Zorglub.Time.Extensions.DayNumberExtensions

/// Convert one (Gregorian) Yemoda to a DayNumber.
/// This function does NOT verify that x represents a valid Gregorian triple.
let private toDayNumber (x: Yemoda) =
    // CalendarDate or CivilDay? The latter is faster and more natural here
    // (day number) BUT we may need support for negative years. Moreover, the
    // speed argument is not really valid here as we construct a new CalendarDate
    // instance using the internal ctor which does not perform any validation,
    // something we cannot do with CivilDay.
    let v = new CalendarDate(x, Cuid.Gregorian)
    v.ToDayNumber()

/// Convert two (Gregorian) Yemoda's to a 2-uple of DayNumber's.
/// This function does NOT verify that x and y represent valid Gregorian triples.
let inline private toDayNumber2 x y = (toDayNumber x), (toDayNumber y)

module TestCommon =
    /// Arbitrary for (x, y, y - x) where x and y are DayNumber instances such that x < y.
    let xynArbitrary =
        Arb.fromGen <| gen {
            let! i, j, n = IntGenerators.orderedPair
            let v = DayNumber.Zero + i
            let w = DayNumber.Zero + j
            return v, w, n
        }

    /// Represents the absolute value of the Rank of an Ord, its position.
    [<Struct; IsReadOnly>]
    type DaysSinceZero = { Value: int } with
        override x.ToString() = x.Value.ToString()
        static member op_Explicit (x: DaysSinceZero) = x.Value

    [<Sealed>]
    type Arbitraries =
        /// Gets an arbitrary for the absolute value of the Rank of an Ord, its position.
        static member GetDaysSinceZeroArbitrary() =
            DomainArbitraries.daysSinceZero
            |> Arb.convert (fun i -> { DaysSinceZero.Value = i }) int

module Prelude =
    open TestCommon

    let dayNumberToDayOfWeekData = CalCalDataSet.DayNumberToDayOfWeekData

    [<Fact>]
    let ``Default value`` () =
        Unchecked.defaultof<DayNumber> === DayNumber.Zero

    // NB: the constructor is private.

    [<Property>]
    let ``ToString() returns the string representation of DaysSinceZero using the current culture`` (i: DaysSinceZero) =
            let dayNumber = DayNumber.Zero + i.Value

            dayNumber.ToString() = i.Value.ToString(System.Globalization.CultureInfo.CurrentCulture)

    //
    // Properties
    //

    [<Fact>]
    let ``Static property Zero`` () =
        DayNumber.Zero.DaysSinceZero === 0
        DayNumber.Zero.Ordinal === Ord.First

    [<Fact>]
    let ``Static property MinValue`` () =
        DayNumber.MinValue.DaysSinceZero === DayNumber.MinDaysSinceZero
        // Along the way, we also check that the foll. props do not overflow.
        DayNumber.MinValue.Ordinal === Ord.MinValue
        DayNumber.MinValue.DayOfWeek === DayOfWeek.Sunday

    [<Fact>]
    let ``Static property MaxValue`` () =
        DayNumber.MaxValue.DaysSinceZero === DayNumber.MaxDaysSinceZero
        // Along the way, we also check that the foll. props do not overflow.
        DayNumber.MaxValue.Ordinal === Ord.MaxValue
        DayNumber.MaxValue.DayOfWeek === DayOfWeek.Monday

    // This is also the standard way of constructing a new DayNumber instance.
    [<Property>]
    let ``Property DaysSinceZero`` (i: DaysSinceZero) =
        let dayNumber = DayNumber.Zero + i.Value

        dayNumber.DaysSinceZero = i.Value

    [<Property>]
    let ``Property Ordinal`` (i: DaysSinceZero) =
        let dayNumber = DayNumber.Zero + i.Value

        dayNumber.Ordinal = Ord.First + i.Value

    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    let ``Property DayOfWeek`` (dayNumber: DayNumber) dayOfWeek =
        dayNumber.DayOfWeek === dayOfWeek

    //
    // Properties of DayZero
    //

    [<Fact>]
    let ``Static property DayZero.NewStyle`` () =
        let date = GregorianCalendar.Instance.GetCalendarDateOn(DayZero.NewStyle)
        let y, m, d = date.Deconstruct()

        DayZero.NewStyle === DayNumber.Zero
        DayZero.NewStyle.Ordinal === Ord.First
        DayZero.NewStyle.DayOfWeek === DayOfWeek.Monday
        (y, m, d) === (1, 1, 1)
        date.YearOfEra === Ord.First

    [<Fact>]
    let ``Static property DayZero.OldStyle`` () =
        let date = JulianCalendar.Instance.GetCalendarDateOn(DayZero.OldStyle)
        let y, m, d = date.Deconstruct()

        DayZero.OldStyle === DayNumber.Zero - 2
        DayZero.OldStyle.Ordinal === Ord.First - 2
        DayZero.OldStyle.DayOfWeek === DayOfWeek.Saturday
        (y, m, d) === (1, 1, 1)
        date.YearOfEra === Ord.First

    [<Fact>]
    let ``Static property DayZero.RataDie`` () =
        DayZero.RataDie === DayNumber.Zero - 1
        DayZero.RataDie.Ordinal === Ord.First - 1

module Factories =
    [<Fact>]
    let ``Today()`` () =
        let today = CivilDate.Today()

        DayNumber.Today() === today.ToDayNumber()

    [<Fact>]
    let ``UtcToday()`` () =
        let today = CivilDate.UtcToday()

        DayNumber.UtcToday() === today.ToDayNumber()

module Conversions =
    [<Property>]
    let ``ToDayNumber()`` (x: DayNumber) =
        (x :> IFixedDay).ToDayNumber() = x

module GregorianConversion =
    let private dataSet = GregorianDataSet.Instance
    let private calendarDataSet = GregorianCalendarDataSet.Instance

    let dayNumberInfoData = calendarDataSet.DayNumberInfoData
    let dateInfoData = dataSet.DateInfoData
    let invalidMonthFieldData = dataSet.InvalidMonthFieldData
    let invalidDayOfYearFieldData = dataSet.InvalidDayOfYearFieldData
    let invalidDayFieldData = dataSet.InvalidDayFieldData

    //
    // Arg check
    //

    [<Fact>]
    let ``Conversion from Gregorian throws when "year" is out of range`` () =
        outOfRangeExn "year" (fun () -> DayNumber.FromGregorianParts(DayNumber.MinSupportedYear - 1, 1, 1))
        outOfRangeExn "year" (fun () -> DayNumber.FromGregorianOrdinalParts(DayNumber.MinSupportedYear - 1, 1))

        outOfRangeExn "year" (fun () -> DayNumber.FromGregorianParts(DayNumber.MaxSupportedYear + 1, 1, 1))
        outOfRangeExn "year" (fun () -> DayNumber.FromGregorianOrdinalParts(DayNumber.MaxSupportedYear + 1, 1))

    [<Theory; MemberData(nameof(invalidMonthFieldData))>]
    let ``FromGregorianParts() throws when "month" is out of range`` y m =
        outOfRangeExn "month" (fun () -> DayNumber.FromGregorianParts(y, m, 1))

    [<Theory; MemberData(nameof(invalidDayFieldData))>]
    let ``FromGregorianParts() throws when "day" is out of range`` y m d =
        outOfRangeExn "day" (fun () -> DayNumber.FromGregorianParts(y, m, d))

    [<Theory; MemberData(nameof(invalidDayOfYearFieldData))>]
    let ``FromGregorianOrdinalParts() throws when "dayOfYear" is out of range`` y doy =
        outOfRangeExn "dayOfYear" (fun () -> DayNumber.FromGregorianOrdinalParts(y, doy))

    //
    // Behaviour at the limits
    //

    [<Fact>]
    let ``Conversion to Gregorian throws when outside the Gregorian domain`` () =
        let v = DayNumber.GregorianDomain.Min - 1
        (fun () -> v.GetGregorianParts())        |> overflows
        (fun () -> v.GetGregorianOrdinalParts()) |> overflows
        (fun () -> v.GetGregorianYear())         |> overflows

        let w = DayNumber.GregorianDomain.Max + 1
        (fun () -> w.GetGregorianParts())        |> overflows
        (fun () -> w.GetGregorianOrdinalParts()) |> overflows
        (fun () -> w.GetGregorianYear())         |> overflows

    [<Fact>]
    let ``Date parts for DayNumber.MinSupportedYear`` () =
        let dayNumber = DayNumber.FromGregorianParts(DayNumber.MinSupportedYear, 1, 1)
        dayNumber === DayNumber.GregorianDomain.Min

        let ymd = dayNumber.GetGregorianParts()
        ymd.Deconstruct() === (DayNumber.MinSupportedYear, 1, 1)

        dayNumber.GetGregorianYear() === DayNumber.MinSupportedYear

    [<Fact>]
    let ``Ordinal parts for DayNumber.MinSupportedYear`` () =
        let dayNumber = DayNumber.FromGregorianOrdinalParts(DayNumber.MinSupportedYear, 1)
        dayNumber === DayNumber.GregorianDomain.Min

        let ydoy = dayNumber.GetGregorianOrdinalParts()
        ydoy.Deconstruct() === (DayNumber.MinSupportedYear, 1)

        dayNumber.GetGregorianYear() === DayNumber.MinSupportedYear

    [<Fact>]
    let ``Date parts for DayNumber.MaxSupportedYear`` () =
        let dayNumber = DayNumber.FromGregorianParts(DayNumber.MaxSupportedYear, 12, 31)
        dayNumber === DayNumber.GregorianDomain.Max

        let ymd = dayNumber.GetGregorianParts()
        ymd.Deconstruct() === (DayNumber.MaxSupportedYear, 12, 31)

        dayNumber.GetGregorianYear() === DayNumber.MaxSupportedYear

    [<Fact>]
    let ``Ordinal parts for DayNumber.MaxSupportedYear`` () =
        GregorianFormulae.IsLeapYear(DayNumber.MaxSupportedYear) |> ok

        let dayNumber = DayNumber.FromGregorianOrdinalParts(DayNumber.MaxSupportedYear, GJSchema.DaysInLeapYear)
        dayNumber === DayNumber.GregorianDomain.Max

        let ydoy = dayNumber.GetGregorianOrdinalParts()
        ydoy.Deconstruct() === (DayNumber.MaxSupportedYear, GJSchema.DaysInLeapYear)

        dayNumber.GetGregorianYear() === DayNumber.MaxSupportedYear

    //
    // DDT
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``FromGregorianParts()`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()

        DayNumber.FromGregorianParts(y, m, d) === dayNumber

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``GetGregorianParts()`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let parts = new DateParts(y, m, d)

        dayNumber.GetGregorianParts() === parts

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``FromGregorianOrdinalParts()`` (x: DateInfo) =
        let y, m, d, doy = x.Deconstruct()
        let dayNumber = DayNumber.FromGregorianParts(y, m, d)

        DayNumber.FromGregorianOrdinalParts(y, doy) === dayNumber

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``GetGregorianOrdinalParts()`` (x: DateInfo) =
        let y, m, d, doy = x.Deconstruct()
        let dayNumber = DayNumber.FromGregorianParts(y, m, d)
        let parts = new OrdinalParts(y, doy)

        dayNumber.GetGregorianOrdinalParts() === parts

module JulianConversion =
    let private dataSet = JulianDataSet.Instance
    let private calendarDataSet = JulianCalendarDataSet.Instance

    let dayNumberInfoData = calendarDataSet.DayNumberInfoData
    let dateInfoData = dataSet.DateInfoData

    let invalidMonthFieldData = dataSet.InvalidMonthFieldData
    let invalidDayOfYearFieldData = dataSet.InvalidDayOfYearFieldData
    let invalidDayFieldData = dataSet.InvalidDayFieldData

    //
    // Arg check
    //

    [<Fact>]
    let ``Conversion from Julian throws when "year" is out of range`` () =
        outOfRangeExn "year" (fun () -> DayNumber.FromJulianParts(DayNumber.MinSupportedYear - 1, 1, 1))
        outOfRangeExn "year" (fun () -> DayNumber.FromJulianOrdinalParts(DayNumber.MinSupportedYear - 1, 1))

        outOfRangeExn "year" (fun () -> DayNumber.FromJulianParts(DayNumber.MaxSupportedYear + 1, 1, 1))
        outOfRangeExn "year" (fun () -> DayNumber.FromJulianOrdinalParts(DayNumber.MaxSupportedYear + 1, 1))

    [<Theory; MemberData(nameof(invalidMonthFieldData))>]
    let ``FromJulianParts() throws when "month" is out of range`` y m =
        outOfRangeExn "month" (fun () -> DayNumber.FromJulianParts(y, m, 1))

    [<Theory; MemberData(nameof(invalidDayFieldData))>]
    let ``FromJulianParts() throws when "day" is out of range`` y m d =
        outOfRangeExn "day" (fun () -> DayNumber.FromJulianParts(y, m, d))

    [<Theory; MemberData(nameof(invalidDayOfYearFieldData))>]
    let ``FromJulianOrdinalParts() throws when "dayOfYear" is out of range`` y doy =
        outOfRangeExn "dayOfYear" (fun () -> DayNumber.FromJulianOrdinalParts(y, doy))

    //
    // Behaviour at the limits
    //

    [<Fact>]
    let ``Conversion to Julian throws when outside the Julian domain`` () =
        let v = DayNumber.JulianDomain.Min - 1
        (fun () -> v.GetJulianParts())        |> overflows
        (fun () -> v.GetJulianOrdinalParts()) |> overflows
        (fun () -> v.GetJulianYear())         |> overflows

        let w = DayNumber.JulianDomain.Max + 1
        (fun () -> w.GetJulianParts())        |> overflows
        (fun () -> w.GetJulianOrdinalParts()) |> overflows
        (fun () -> w.GetJulianYear())         |> overflows

    [<Fact>]
    let ``Date parts for DayNumber.MinSupportedYear`` () =
        let dayNumber = DayNumber.FromJulianParts(DayNumber.MinSupportedYear, 1, 1)
        dayNumber === DayNumber.JulianDomain.Min

        let ymd = dayNumber.GetJulianParts()
        ymd.Deconstruct() === (DayNumber.MinSupportedYear, 1, 1)

        dayNumber.GetJulianYear() === DayNumber.MinSupportedYear

    [<Fact>]
    let ``Ordinal parts for DayNumber.MinSupportedYear`` () =
        let dayNumber = DayNumber.FromJulianOrdinalParts(DayNumber.MinSupportedYear, 1)
        dayNumber === DayNumber.JulianDomain.Min

        let ydoy = dayNumber.GetJulianOrdinalParts()
        ydoy.Deconstruct() === (DayNumber.MinSupportedYear, 1)

        dayNumber.GetJulianYear() === DayNumber.MinSupportedYear

    [<Fact>]
    let ``Date parts for DayNumber.MaxSupportedYear`` () =
        let dayNumber = DayNumber.FromJulianParts(DayNumber.MaxSupportedYear, 12, 31)
        dayNumber === DayNumber.JulianDomain.Max

        let ymd = dayNumber.GetJulianParts()
        ymd.Deconstruct() === (DayNumber.MaxSupportedYear, 12, 31)

        dayNumber.GetJulianYear() === DayNumber.MaxSupportedYear

    [<Fact>]
    let ``Ordinal parts for DayNumber.MaxSupportedYear`` () =
        JulianFormulae.IsLeapYear(DayNumber.MaxSupportedYear) |> ok

        let dayNumber = DayNumber.FromJulianOrdinalParts(DayNumber.MaxSupportedYear, GJSchema.DaysInLeapYear)
        dayNumber === DayNumber.JulianDomain.Max

        let ydoy = dayNumber.GetJulianOrdinalParts()
        ydoy.Deconstruct() === (DayNumber.MaxSupportedYear, GJSchema.DaysInLeapYear)

        dayNumber.GetJulianYear() === DayNumber.MaxSupportedYear

    //
    // DDT
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``FromJulianParts()`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()

        DayNumber.FromJulianParts(y, m, d) === dayNumber

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``GetJulianParts()`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let parts = new DateParts(y, m, d)

        dayNumber.GetJulianParts() === parts

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``FromJulianOrdinalParts()`` (x: DateInfo) =
        let y, m, d, doy = x.Deconstruct()
        let dayNumber = DayNumber.FromJulianParts(y, m, d)

        DayNumber.FromJulianOrdinalParts(y, doy) === dayNumber

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``GetJulianOrdinalParts()`` (x: DateInfo) =
        let y, m, d, doy = x.Deconstruct()
        let dayNumber = DayNumber.FromJulianParts(y, m, d)
        let parts = new OrdinalParts(y, doy)

        dayNumber.GetJulianOrdinalParts() === parts

module DayOfWeekAdjustment =
    let private dataSet = GregorianCalendarDataSet.Instance

    let invalidDayOfWeekData = EnumDataSet.InvalidDayOfWeekData

    let beforeData = dataSet.DayOfWeek_Before_Data
    let onOrBeforeData = dataSet.DayOfWeek_OnOrBefore_Data
    let nearestData = dataSet.DayOfWeek_Nearest_Data
    let onOrAfterData = dataSet.DayOfWeek_OnOrAfter_Data
    let afterData = dataSet.DayOfWeek_After_Data

    //
    // Arg check
    //

    [<Theory; MemberData(nameof(invalidDayOfWeekData))>]
    let ``Previous() throws when "dayOfWeek" is out of range`` dayOfWeek =
        outOfRangeExn "dayOfWeek" (fun () -> DayNumber.Zero.Previous(dayOfWeek))

    [<Theory; MemberData(nameof(invalidDayOfWeekData))>]
    let ``PreviousOrSame() throws when "dayOfWeek" is out of range`` dayOfWeek =
        outOfRangeExn "dayOfWeek" (fun () -> DayNumber.Zero.PreviousOrSame(dayOfWeek))

    [<Theory; MemberData(nameof(invalidDayOfWeekData))>]
    let ``Nearest() throws when "dayOfWeek" is out of range`` dayOfWeek =
        outOfRangeExn "dayOfWeek" (fun () -> DayNumber.Zero.Nearest(dayOfWeek))

    [<Theory; MemberData(nameof(invalidDayOfWeekData))>]
    let ``NextOrSame() throws when "dayOfWeek" is out of range`` dayOfWeek =
        outOfRangeExn "dayOfWeek" (fun () -> DayNumber.Zero.NextOrSame(dayOfWeek))

    [<Theory; MemberData(nameof(invalidDayOfWeekData))>]
    let ``Next() throws when "dayOfWeek" is out of range`` dayOfWeek =
        outOfRangeExn "dayOfWeek" (fun () -> DayNumber.Zero.Next(dayOfWeek))

    //
    // Behaviour near DayNumber.Min/MaxValue
    //

    [<Fact>]
    let ``Previous() near DayNumber.MinValue`` () =
        DayOfWeekAdjusterTester.NearMinValue(DayNumber.MinValue).TestPrevious()

    [<Fact>]
    let ``PreviousOrSame() near DayNumber.MinValue`` () =
        DayOfWeekAdjusterTester.NearMinValue(DayNumber.MinValue).TestPreviousOrSame()

    [<Fact>]
    let ``NextOrSame() near DayNumber.MaxValue`` () =
        DayOfWeekAdjusterTester.NearMaxValue(DayNumber.MaxValue).TestNextOrSame()

    [<Fact>]
    let ``Next() near DayNumber.MaxValue`` () =
        DayOfWeekAdjusterTester.NearMaxValue(DayNumber.MaxValue).TestNext()

    //
    // Ajustments
    //

    [<Theory; MemberData(nameof(beforeData))>]
    let ``Previous()`` (info: YemodaPairAnd<DayOfWeek>) =
        let x, y, dayOfWeek = info.Deconstruct()
        let v, w = toDayNumber2 x y

        v.Previous(dayOfWeek) === w

    [<Theory; MemberData(nameof(onOrBeforeData))>]
    let ``PreviousOrSame()`` (info: YemodaPairAnd<DayOfWeek>) =
        let x, y, dayOfWeek = info.Deconstruct()
        let v, w = toDayNumber2 x y

        v.PreviousOrSame(dayOfWeek) === w

    [<Theory; MemberData(nameof(nearestData))>]
    let ``Nearest()`` (info: YemodaPairAnd<DayOfWeek>) =
        let x, y, dayOfWeek = info.Deconstruct()
        let v, w = toDayNumber2 x y

        v.Nearest(dayOfWeek) === w

    [<Theory; MemberData(nameof(onOrAfterData))>]
    let ``NextOrSame()`` (info: YemodaPairAnd<DayOfWeek>) =
        let x, y, dayOfWeek = info.Deconstruct()
        let v, w = toDayNumber2 x y

        v.NextOrSame(dayOfWeek) === w

    [<Theory; MemberData(nameof(afterData))>]
    let ``Next()`` (info: YemodaPairAnd<DayOfWeek>) =
        let x, y, dayOfWeek = info.Deconstruct()
        let v, w = toDayNumber2 x y

        v.Next(dayOfWeek) === w

module DayOfWeekAdjustment2 =
    // Here we test the extension methods from DayNumberExtensions.

    let private dataSet = GregorianCalendarDataSet.Instance

    let invalidDayOfWeekData = EnumDataSet.InvalidDayOfWeekData

    let beforeData = dataSet.DayOfWeek_Before_Data
    let onOrBeforeData = dataSet.DayOfWeek_OnOrBefore_Data
    let nearestData = dataSet.DayOfWeek_Nearest_Data
    let onOrAfterData = dataSet.DayOfWeek_OnOrAfter_Data
    let afterData = dataSet.DayOfWeek_After_Data

    //
    // Arg check
    //

    [<Theory; MemberData(nameof(invalidDayOfWeekData))>]
    let ``Before() throws when "dayOfWeek" is out of range`` dayOfWeek =
        outOfRangeExn "dayOfWeek" (fun () -> DayNumber.Zero.Before(dayOfWeek))

    [<Theory; MemberData(nameof(invalidDayOfWeekData))>]
    let ``OnOrBefore() throws when "dayOfWeek" is out of range`` dayOfWeek =
        outOfRangeExn "dayOfWeek" (fun () -> DayNumber.Zero.OnOrBefore(dayOfWeek))

    [<Theory; MemberData(nameof(invalidDayOfWeekData))>]
    let ``OnOrAfter() throws when "dayOfWeek" is out of range`` dayOfWeek =
        outOfRangeExn "dayOfWeek" (fun () -> DayNumber.Zero.OnOrAfter(dayOfWeek))

    [<Theory; MemberData(nameof(invalidDayOfWeekData))>]
    let ``After() throws when "dayOfWeek" is out of range`` dayOfWeek =
        outOfRangeExn "dayOfWeek" (fun () -> DayNumber.Zero.After(dayOfWeek))

    //
    // Ajustments
    //

    [<Theory; MemberData(nameof(beforeData))>]
    let ``Before()`` (info: YemodaPairAnd<DayOfWeek>) =
        let x, y, dayOfWeek = info.Deconstruct()
        let v, w = toDayNumber2 x y

        v.Before(dayOfWeek) === w

    [<Theory; MemberData(nameof(onOrBeforeData))>]
    let ``OnOrBefore()`` (info: YemodaPairAnd<DayOfWeek>) =
        let x, y, dayOfWeek = info.Deconstruct()
        let v, w = toDayNumber2 x y

        v.OnOrBefore(dayOfWeek) === w

    [<Theory; MemberData(nameof(onOrAfterData))>]
    let ``OnOrAfter()`` (info: YemodaPairAnd<DayOfWeek>) =
        let x, y, dayOfWeek = info.Deconstruct()
        let v, w = toDayNumber2 x y

        v.OnOrAfter(dayOfWeek) === w

    [<Theory; MemberData(nameof(afterData))>]
    let ``After()`` (info: YemodaPairAnd<DayOfWeek>) =
        let x, y, dayOfWeek = info.Deconstruct()
        let v, w = toDayNumber2 x y

        v.After(dayOfWeek) === w

module Equality =
    open NonStructuralComparison
    open TestCommon

    // fsharplint:disable Hints
    [<Property>]
    let ``Equality when both operands are identical`` (x: DayNumber) =
        x = x
        .&. not (x <> x)
        .&. x.Equals(x)
        .&. x.Equals(x :> obj)

    [<Property>]
    let ``Equality when both operands are distinct`` () = xynArbitrary @@@@ fun (x, y, _) ->
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
    let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: DayNumber) =
        not (x.Equals(null))
        .&. not (x.Equals(new obj()))

    [<Property>]
    let ``Equals(obj) returns false when "obj" is an integer whose value is equal to DaysSinceZero`` (i: DaysSinceZero) =
            let dayNumber = DayNumber.Zero + i.Value
            not (dayNumber.Equals(i.Value))

    [<Property>]
    let ``GetHashCode() is invariant`` (x: DayNumber) =
        x.GetHashCode() = x.GetHashCode()

    [<Property>]
    let ``GetHashCode() returns the hashcode of DaysSinceZero`` (x: DayNumber) =
        let hash = x.DaysSinceZero.GetHashCode()

        x.GetHashCode() = hash

module Comparison =
    open NonStructuralComparison
    open TestCommon

    // fsharplint:disable Hints
    [<Property>]
    let ``Comparisons when both operands are identical`` (x: DayNumber) =
        not (x > x)
        .&. not (x < x)
        .&. (x >= x)
        .&. (x <= x)

    [<Property>]
    let ``Comparisons when both operands are distinct`` () = xynArbitrary @@@@ fun (x, y, _) ->
        not (x > y)
        .&. not (x >= y)
        .&. (x < y)
        .&. (x <= y)
        // Flipped
        .&. (y > x)
        .&. (y >= x)
        .&. not (y < x)
        .&. not (y <= x)
    // fsharplint:enable

    //
    // CompareTo()
    //

    [<Property>]
    let ``CompareTo() returns 0 when both objects are identical`` (x: DayNumber) =
        (x.CompareTo(x) = 0)
        .&. (x.CompareTo(x :> obj) = 0)

    [<Property>]
    let ``CompareTo() when both objects are distinct`` () = xynArbitrary @@@@ fun (x, y, _) ->
        (x.CompareTo(y) <= 0)
        .&. (x.CompareTo(y :> obj) <= 0)
        // Flipped
        .&. (y.CompareTo(x) >= 0)
        .&. (y.CompareTo(x :> obj) >= 0)

    [<Property>]
    let ``CompareTo(obj) returns 1 when "obj" is null`` (x: DayNumber) =
         x.CompareTo(null) = 1

    [<Property>]
    let ``CompareTo(obj) throws when "obj" is a plain object`` (x: DayNumber) =
        argExn "obj" (fun () -> x.CompareTo(new obj()))

    //
    // Min() and Max()
    //

    [<Property>]
    let ``Min() when both values are identical`` (x: DayNumber) =
        DayNumber.Min(x, x) = x

    [<Property>]
    let ``Max() when both values are identical`` (x: DayNumber) =
        DayNumber.Max(x, x) = x

    [<Property>]
    let ``Min() when both values are distinct`` () = xynArbitrary @@@@ fun (x, y, _) ->
        DayNumber.Min(x, y) === x
        DayNumber.Min(y, x) === x

    [<Property>]
    let ``Max() when both values are distinct`` () = xynArbitrary @@@@ fun (x, y, _) ->
        DayNumber.Max(x, y) === y
        DayNumber.Max(y, x) === y

module Math =
    open TestCommon

    //
    // DayNumber.Zero
    //

    // NB: DayNumber.MaxDaysSinceZero = Int32.MaxValue - 1
    [<Fact>]
    let ``DayNumber.Zero + Int32.MaxValue overflows`` () =
        (fun () -> DayNumber.Zero + Int32.MaxValue)         |> overflows
        (fun () -> DayNumber.Zero.PlusDays(Int32.MaxValue)) |> overflows

    [<Fact>]
    let ``DayNumber.Zero + (Int32.MaxValue - 1) = DayNumber.MaxValue`` () =
        DayNumber.Zero + (Int32.MaxValue - 1)       === DayNumber.MaxValue
        DayNumber.Zero.PlusDays(Int32.MaxValue - 1) === DayNumber.MaxValue

    [<Fact>]
    let ``DayNumber.MaxValue - (Int32.MaxValue - 1) = DayNumber.Zero`` () =
        DayNumber.MaxValue - (Int32.MaxValue - 1)          === DayNumber.Zero
        DayNumber.MaxValue.PlusDays(-(Int32.MaxValue - 1)) === DayNumber.Zero

    // NB: DayNumber.MinDaysSinceZero = Int32.MinValue + 1
    [<Fact>]
    let ``DayNumber.Zero + Int32.MinValue overflows`` () =
        (fun () -> DayNumber.Zero + Int32.MinValue)         |> overflows
        (fun () -> DayNumber.Zero.PlusDays(Int32.MinValue)) |> overflows

    [<Fact>]
    let ``DayNumber.Zero + (Int32.MinValue + 1) = DayNumber.MinValue`` () =
        DayNumber.Zero + (Int32.MinValue + 1)       === DayNumber.MinValue
        DayNumber.Zero.PlusDays(Int32.MinValue + 1) === DayNumber.MinValue

    [<Fact>]
    let ``DayNumber.MinValue - (Int32.MinValue + 1) = DayNumber.MinValue`` () =
        DayNumber.MinValue - (Int32.MinValue + 1)          === DayNumber.Zero
        DayNumber.MinValue.PlusDays(-(Int32.MinValue + 1)) === DayNumber.Zero

    //
    // DayNumber.MinValue
    //

    [<Fact>]
    let ``DayNumber.MinValue - 1 overflows`` () =
        (fun () -> DayNumber.MinValue - 1)           |> overflows
        (fun () -> DayNumber.MinValue + (-1))        |> overflows
        (fun () -> DayNumber.MinValue.PlusDays(-1))  |> overflows
        (fun () -> DayNumber.MinValue.PreviousDay()) |> overflows

    [<Fact>]
    let ``DayNumber.MinValue + Int32.MaxValue does not overflow`` () =
        DayNumber.MinValue + Int32.MaxValue         === DayNumber.Zero
        DayNumber.MinValue.PlusDays(Int32.MaxValue) === DayNumber.Zero

    //
    // DayNumber.MaxValue
    //

    [<Fact>]
    let ``DayNumber.MaxValue + 1 overflows`` () =
        (fun () -> DayNumber.MaxValue + 1)         |> overflows
        (fun () -> DayNumber.MaxValue - (-1))      |> overflows
        (fun () -> DayNumber.MaxValue.PlusDays(1)) |> overflows
        (fun () -> DayNumber.MaxValue.NextDay())   |> overflows

    [<Fact>]
    let ``DayNumber.MaxValue - Int32.MaxValue does not overflow`` () =
        DayNumber.MaxValue - Int32.MaxValue          === DayNumber.Zero - 1
        DayNumber.MaxValue.PlusDays(-Int32.MaxValue) === DayNumber.Zero - 1

    //
    // Difference
    //

    [<Fact>]
    let ``DayNumber.MaxValue - DayNumber.MinValue overflows`` () =
        (fun () -> DayNumber.MaxValue - DayNumber.MinValue) |> overflows

    //
    // Operations
    //

    // fsharplint:disable Hints
    [<Property>]
    let ``0 is a neutral element (operators)`` (x: DayNumber) =
        (x + 0 = x)
        .&. (x - 0 = x)
        .&. (x - x = 0)
    // fsharplint:enable

    [<Property>]
    let ``0 is a neutral element (methods)`` (x: DayNumber) =
        (x.PlusDays(0) = x)
        .&. (x.CountDaysSince(x) = 0)

    [<Property>]
    let ``Addition and subtraction operators`` () = xynArbitrary @@@@ fun (x, y, n) ->
        (x + n = y)
        .&. (y - n = x)

    [<Property>]
    let ``Difference operator`` () = xynArbitrary @@@@ fun (x, y, n) ->
        (y - x = n)
        .&. (x - y = -n)

    [<Property>]
    let ``Increment operator`` (x: DayNumber) = x <> DayNumber.MaxValue &&&& lazy (
        DayNumber.op_Increment(x) = x + 1
    )

    [<Property>]
    let ``Decrement operator`` (x: DayNumber) = x <> DayNumber.MaxValue &&&& lazy (
        DayNumber.op_Decrement(x) = x - 1
    )

    [<Property>]
    let ``NextDay()`` (x: DayNumber) = x <> DayNumber.MaxValue &&&& lazy (
        x.NextDay() = x + 1
    )

    [<Property>]
    let ``PreviousDay()`` (x: DayNumber) = x <> DayNumber.MinValue &&&& lazy (
        x.PreviousDay() = x - 1
    )

    [<Property>]
    let ``PlusDays()`` () = xynArbitrary @@@@ fun (x, y, n) ->
        (x.PlusDays(n) = y)
        .&. (y.PlusDays(-n) = x)

    [<Property>]
    let ``CountDaysSince()`` () = xynArbitrary @@@@ fun (x, y, n) ->
        (y.CountDaysSince(x) = n)
        .&. (x.CountDaysSince(y) = -n)

module Postlude =
    /// Compare the core properties.
    let rec private compareTypes (dayNumber: DayNumber) (date: CivilDate) =
        let ymd = dayNumber.GetGregorianParts()
        let y, m, d = ymd.Deconstruct()
        let passed =
            y = date.Year
            && m = date.Month
            && d = date.Day
            && dayNumber.DayOfWeek = date.DayOfWeek

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
    let ``Deep comparison between DayNumber and CivilDate`` () =
        // NB: both start on Monday January 1, 1 (CE).
        compareTypes DayNumber.Zero CivilDate.MinValue |> Assert.True
