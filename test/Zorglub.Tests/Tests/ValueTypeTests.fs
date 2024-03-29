﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.ValueTypeTests

open System
open System.Runtime.InteropServices

open Zorglub.Bulgroz
open Zorglub.Bulgroz.Obsolete
open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Utilities
open Zorglub.Time.Hemerology
open Zorglub.Time.Horology
open Zorglub.Time.Horology.Ntp
open Zorglub.Time.Simple
open Zorglub.Time.Specialized

open Xunit

module RuntimeSizes =
    // The runtime size of a struct should stay <= 16 bytes.

    /// Returns the unmanaged size of a (generic) object in bytes.
    let private sizeof<'a when 'a : struct> () = Marshal.SizeOf(Unchecked.defaultof<'a>)

    [<Fact>]
    let ``Generic types`` () =
        // Reminder:
        // byte     1 byte (!)
        // int16    2 bytes
        // int32    4 bytes
        // int64    8 bytes
        // double   8 bytes

        // OrderedPair<T>, two T's.
        sizeof<OrderedPair<byte>>() === 2
        sizeof<OrderedPair<int16>>() === 4
        sizeof<OrderedPair<int32>>() === 8
        sizeof<OrderedPair<int64>>() === 16
        sizeof<OrderedPair<double>>() === 16
        // Range<T>, two T's.
        sizeof<Range<byte>>() === 2
        sizeof<Range<int16>>() === 4
        sizeof<Range<int32>>() === 8
        sizeof<Range<int64>>() === 16
        sizeof<Range<double>>() === 16
        // RangeSet<T>, two T's and one int32.
        sizeof<RangeSet<byte>>() === 8
        sizeof<RangeSet<int16>>() === 8
        sizeof<RangeSet<int32>>() === 12
        sizeof<RangeSet<int64>>() === 24   // BIG struct
        sizeof<RangeSet<double>>() === 24  // BIG struct
        // UpperRay<T>, one T.
        sizeof<UpperRay<byte>>() === 1
        sizeof<UpperRay<int16>>() === 2
        sizeof<UpperRay<int32>>() === 4
        sizeof<UpperRay<int64>>() === 8
        sizeof<UpperRay<double>>() === 8
        // LowerRay<T>, one T.
        sizeof<LowerRay<byte>>() === 1
        sizeof<LowerRay<int16>>() === 2
        sizeof<LowerRay<int32>>() === 4
        sizeof<LowerRay<int64>>() === 8
        sizeof<LowerRay<double>>() === 8
        // IntervalBoundary<T>, two T's and one int32.
        sizeof<IntervalBoundary<byte>>() === 8
        sizeof<IntervalBoundary<int16>>() === 8
        sizeof<IntervalBoundary<int32>>() === 12
        sizeof<IntervalBoundary<int64>>() === 24   // BIG struct
        sizeof<IntervalBoundary<double>>() === 24  // BIG struct

    [<Fact>]
    let ``Types in Zorglub.Time`` () =
        Marshal.SizeOf(typedefof<AdditionRuleset>) === 12
        Marshal.SizeOf(typedefof<DateParts>) === 12
        Marshal.SizeOf(typedefof<DayNumber>) === 4
        Marshal.SizeOf(typedefof<Moment>) === 8
        Marshal.SizeOf(typedefof<MonthParts>) === 8
        Marshal.SizeOf(typedefof<Ord>) === 4
        Marshal.SizeOf(typedefof<OrdinalParts>) === 8
        Marshal.SizeOf(typedefof<TimeOfDay>) === 4
        // Zorglub.Sketches
        Marshal.SizeOf(typedefof<DayNumber64>) === 8
        Marshal.SizeOf(typedefof<Ord64>) === 8

    [<Fact>]
    let ``Types in Zorglub.Time.Core`` () =
        Marshal.SizeOf(typedefof<Yedoy>) === 4
        Marshal.SizeOf(typedefof<Yedoyx>) === 4
        Marshal.SizeOf(typedefof<Yemo>) === 4
        Marshal.SizeOf(typedefof<Yemoda>) === 4
        Marshal.SizeOf(typedefof<Yemodax>) === 4
        Marshal.SizeOf(typedefof<Yemox>) === 4
        // Zorglub.Sketches
        Marshal.SizeOf(typedefof<Unit>) === 1
        Marshal.SizeOf(typedefof<Yewe>) === 4
        Marshal.SizeOf(typedefof<Yewex>) === 4

    [<Fact>]
    let ``Types in Zorglub.Time.Hemerology`` () =
        // Zorglub.Sketches
        Marshal.SizeOf(typedefof<ZDate>) === 8

    [<Fact>]
    let ``Types in Zorglub.Time.Horology`` () =
        // Zorglub.Extras
        Marshal.SizeOf(typedefof<Duration32>) === 4
        Marshal.SizeOf(typedefof<Duration64>) === 8
        Marshal.SizeOf(typedefof<GregorianInstant>) === 16
        Marshal.SizeOf(typedefof<NtpPacket>) === 52 // HUGE struct
        Marshal.SizeOf(typedefof<ReferenceId>) === 4
        Marshal.SizeOf(typedefof<Timestamp64>) === 8
        // Zorglub.Sketches
        Marshal.SizeOf(typedefof<InstantOfDay>) === 8

    [<Fact>]
    let ``Types in Zorglub.Time.Simple`` () =
        Marshal.SizeOf(typedefof<CalendarDate>) === 4
        Marshal.SizeOf(typedefof<CalendarDay>) === 4
        Marshal.SizeOf(typedefof<CalendarMonth>) === 4
        Marshal.SizeOf(typedefof<CalendarYear>) === 4
        Marshal.SizeOf(typedefof<OrdinalDate>) === 4
        // Zorglub.Sketches
        Marshal.SizeOf(typedefof<CalendarWeek>) === 4

    [<Fact>]
    let ``Types in Zorglub.Time.Specialized`` () =
        Marshal.SizeOf(typedefof<CivilDate>) === 4
        Marshal.SizeOf(typedefof<GregorianDate>) === 4
        Marshal.SizeOf(typedefof<JulianDate>) === 4
        // Zorglub.Extras
        Marshal.SizeOf(typedefof<Armenian13Date>) === 4
        Marshal.SizeOf(typedefof<ArmenianDate>) === 4
        Marshal.SizeOf(typedefof<Coptic13Date>) === 4
        Marshal.SizeOf(typedefof<CopticDate>) === 4
        Marshal.SizeOf(typedefof<Ethiopic13Date>) === 4
        Marshal.SizeOf(typedefof<EthiopicDate>) === 4
        Marshal.SizeOf(typedefof<TabularIslamicDate>) === 4
        Marshal.SizeOf(typedefof<WorldDate>) === 4
        Marshal.SizeOf(typedefof<Zoroastrian13Date>) === 4
        Marshal.SizeOf(typedefof<ZoroastrianDate>) === 4

    [<Fact>]
    let ``Types in Zorglub.Bulgroz`` () =
        // Zorglub.Sketches
        Marshal.SizeOf(typedefof<DateFields>) === 12
        Marshal.SizeOf(typedefof<MonthFields>) === 8
        Marshal.SizeOf(typedefof<OrdinalFields>) === 8
        Marshal.SizeOf(typedefof<XCivilDate>) === 4

    // TODO(code): add tests for the data types defined within THIS project.
    [<Fact>]
    let ``Types in Zorglub.Testing`` () =
        // Zorglub.Testing.Data
        Marshal.SizeOf(typedefof<YearDaysSinceEpoch>) === 8
        Marshal.SizeOf(typedefof<YearDayNumber>) === 8
        //
        Marshal.SizeOf(typedefof<MonthsSinceEpochInfo>) === 8
        Marshal.SizeOf(typedefof<DaysSinceEpochInfo>) === 8
        Marshal.SizeOf(typedefof<DaysSinceZeroInfo>) === 8
        Marshal.SizeOf(typedefof<DaysSinceRataDieInfo>) === 8
        Marshal.SizeOf(typedefof<DayNumberInfo>) === 8
        Marshal.SizeOf(typedefof<DaysSinceEpochYewedaInfo>) === 12
        Marshal.SizeOf(typedefof<DayNumberYewedaInfo>) === 12
        //
        Marshal.SizeOf(typedefof<DateInfo>) === 16
        Marshal.SizeOf(typedefof<MonthInfo>) === 16
        Marshal.SizeOf(typedefof<YearInfo>) === 12
        Marshal.SizeOf(typedefof<DecadeInfo>) === 12
        Marshal.SizeOf(typedefof<CenturyInfo>) === 12
        Marshal.SizeOf(typedefof<MillenniumInfo>) === 12
        Marshal.SizeOf(typedefof<DecadeOfCenturyInfo>) === 12
        //
        sizeof<YemodaAnd<int>>() === 8
        sizeof<YemodaAnd<DayOfWeek>>() === 8
        sizeof<YemoAnd<int>>() === 8
        sizeof<YemoAnd<bool, bool>>() === 12
        sizeof<YearAnd<int>>() === 8
        Marshal.SizeOf(typedefof<YemodaPair>) === 8
        sizeof<YemodaPairAnd<int>>() === 12
        Marshal.SizeOf(typedefof<YedoyPair>) === 8
        sizeof<YedoyPairAnd<int>>() === 12
        Marshal.SizeOf(typedefof<YemoPair>) === 8
        sizeof<YemoPairAnd<int>>() === 12
        //
        Marshal.SizeOf(typedefof<OrdinalPartsPair>) === 16
        Marshal.SizeOf(typedefof<MonthPartsPair>) === 16

module DefaultValues =
    // Date types built upon DayNumber or Yemoda: 01/01/0001 (year 1)
    // For types not attached to a specific calendar, we always default to Gregorian.

    //
    // Types found in Zorglub.Time
    //

    [<Fact>]
    let ``Default value of DayNumber is 01/01/0001 (Gregorian)`` () =
        let dayNumber = Unchecked.defaultof<DayNumber>
        let parts = dayNumber.GetGregorianParts()
        let y, m, d = parts.Deconstruct()

        (y, m, d) === (1, 1, 1)

    [<Fact>]
    let ``Default value of DayNumber64 is 01/01/0001 (Gregorian)`` () =
        let dayNumber64 = Unchecked.defaultof<DayNumber64>
        let y, m, d = dayNumber64.GetGregorianParts()

        (y, m, d) === (1L, 1, 1)

    [<Fact>]
    let ``Default value of TimeOfDay is Midnight (00:00:00.000)`` () =
        let time = Unchecked.defaultof<TimeOfDay>
        let h, m, s, ms = time.Deconstruct()

        time === TimeOfDay.Midnight
        (h, m, s, ms) === (0, 0, 0, 0)

    [<Fact>]
    let ``Default value of DateParts is 00/00/0000`` () =
        let parts = Unchecked.defaultof<DateParts>
        let y, m, d = parts.Deconstruct()

        (y, m, d) === (0, 0, 0)

    [<Fact>]
    let ``Default value of MonthParts is 00/0000`` () =
        let parts = Unchecked.defaultof<MonthParts>
        let y, m = parts.Deconstruct()

        (y, m) === (0, 0)

    [<Fact>]
    let ``Default value of OrdinalParts is 00/0000`` () =
        let parts = Unchecked.defaultof<OrdinalParts>
        let y, doy = parts.Deconstruct()

        (y, doy) === (0, 0)

    //
    // Calendrical parts found in Zorglub.Time.Core
    //

    [<Fact>]
    let ``Default value of Yemoda is 01/01/0001`` () =
        let parts = Unchecked.defaultof<Yemoda>
        let y, m, d = parts.Deconstruct()

        (y, m, d) === (1, 1, 1)

    [<Fact>]
    let ``Default value of Yemodax is 01/01/0001 (0)`` () =
        let parts = Unchecked.defaultof<Yemodax>
        let y, m, d = parts.Deconstruct()

        (y, m, d) === (1, 1, 1)
        parts.Extra === 0

    [<Fact>]
    let ``Default value of Yemo is 01/0001`` () =
        let parts = Unchecked.defaultof<Yemo>
        let y, m = parts.Deconstruct()

        (y, m) === (1, 1)

    [<Fact>]
    let ``Default value of Yemox is 01/0001 (0)`` () =
        let parts = Unchecked.defaultof<Yemox>
        let y, m = parts.Deconstruct()

        (y, m) === (1, 1)
        parts.Extra === 0

    [<Fact>]
    let ``Default value of Yedoy is 001/0001`` () =
        let parts = Unchecked.defaultof<Yedoy>
        let y, doy = parts.Deconstruct()

        (y, doy) === (1, 1)

    [<Fact>]
    let ``Default value of Yedoyx is 001/0001 (0)`` () =
        let parts = Unchecked.defaultof<Yedoyx>
        let y, doy = parts.Deconstruct()

        (y, doy) === (1, 1)
        parts.Extra === 0

    [<Fact>]
    let ``Default value of Yewe is 001/0001`` () =
        let parts = Unchecked.defaultof<Yewe>
        let y, woy = parts.Deconstruct()

        (y, woy) === (1, 1)

    [<Fact>]
    let ``Default value of Yewex is 001/0001 (0)`` () =
        let parts = Unchecked.defaultof<Yewex>
        let y, woy = parts.Deconstruct()

        (y, woy) === (1, 1)
        parts.Extra === 0

    //
    // Date types found in Zorglub.Time.Hemerology
    //

    [<Fact>]
    let ``Default value of ZDate is 01/01/0001 (Gregorian)`` () =
        let date = Unchecked.defaultof<ZDate>
        let y, m, d = date.Deconstruct()

        (y, m, d) === (1, 1, 1)
        date.Calendar.Key === "Gregorian"

    //
    // Date types found in Zorglub.Time.Simple
    //

    [<Fact>]
    let ``Default value of CalendarDay is 01/01/0001 (Gregorian)`` () =
        let date = Unchecked.defaultof<CalendarDay>
        let y, m, d = date.Deconstruct()

        (y, m, d) === (1, 1, 1)
        date.Calendar.PermanentId === CalendarId.Gregorian

    [<Fact>]
    let ``Default value of CalendarDate is 01/01/0001 (Gregorian)`` () =
        let date = Unchecked.defaultof<CalendarDate>
        let y, m, d = date.Deconstruct()

        (y, m, d) === (1, 1, 1)
        date.Calendar.PermanentId === CalendarId.Gregorian

    [<Fact>]
    let ``Default value of OrdinalDate is 001/0001 (Gregorian)`` () =
        let date = Unchecked.defaultof<OrdinalDate>
        let y, doy = date.Deconstruct()

        (y, doy) === (1, 1)
        date.Calendar.PermanentId === CalendarId.Gregorian

    [<Fact>]
    let ``Default value of CalendarWeek is 001/0001 (Gregorian)`` () =
        let week = Unchecked.defaultof<CalendarWeek>
        let y, woy = week.Deconstruct()

        (y, woy) === (1, 1)
        week.Calendar.PermanentId === CalendarId.Gregorian

    [<Fact>]
    let ``Default value of CalendarMonth is 01/0001 (Gregorian)`` () =
        let month = Unchecked.defaultof<CalendarMonth>
        let y, m = month.Deconstruct()

        (y, m) === (1, 1)
        month.Calendar.PermanentId === CalendarId.Gregorian

    [<Fact>]
    let ``Default value of CalendarYear is 0001 (Gregorian)`` () =
        let year = Unchecked.defaultof<CalendarYear>

        year.Year === 1
        year.Calendar.PermanentId === CalendarId.Gregorian

    //
    // Time parts found in Zorglub.Time.Horology
    //

    [<Fact>]
    let ``Default value of InstantOfDay is Midnight (00:00:00.000_000_000)`` () =
        let instant = Unchecked.defaultof<InstantOfDay>
        let h, m, s, ms = instant.Deconstruct()

        instant === InstantOfDay.Midnight
        (h, m, s, ms) === (0, 0, 0, 0)
        instant.Nanosecond === 0

    //
    // Date types found in Zorglub.Time.Specialized
    //

    [<Fact>]
    let ``Default value of CivilDate is 01/01/0001 (Gregorian-only)`` () =
        let date = Unchecked.defaultof<CivilDate>
        let y, m, d = date.Deconstruct()

        (y, m, d) === (1, 1, 1)

    [<Fact>]
    let ``Default value of GregorianDate is 01/01/0001 (Gregorian-only)`` () =
        let date = Unchecked.defaultof<GregorianDate>
        let y, m, d = date.Deconstruct()

        (y, m, d) === (1, 1, 1)

    [<Fact>]
    let ``Default value of JulianDate is 01/01/0001 (Julian-only)`` () =
        let date = Unchecked.defaultof<JulianDate>
        let y, m, d = date.Deconstruct()

        (y, m, d) === (1, 1, 1)

    //
    // Types found in Zorglub.Bulgroz
    //

    [<Fact>]
    let ``Default value of DateFields is 01/01/0000`` () =
        let fields = Unchecked.defaultof<DateFields>
        let y, m, d = fields.Deconstruct()

        (y, m, d) === (0, 1, 1)

    [<Fact>]
    let ``Default value of MonthFields is 01/0000`` () =
        let fields = Unchecked.defaultof<MonthFields>
        let y, m = fields.Deconstruct()

        (y, m) === (0, 1)

    [<Fact>]
    let ``Default value of OrdinalFields is 01/0000`` () =
        let fields = Unchecked.defaultof<OrdinalFields>
        let y, doy = fields.Deconstruct()

        (y, doy) === (0, 1)

    [<Fact>]
    let ``Default value of XCivilDate is 01/01/0001 (Gregorian-only)`` () =
        let date = Unchecked.defaultof<XCivilDate>
        let y, m, d = date.Deconstruct()

        (y, m, d) === (1, 1, 1)
