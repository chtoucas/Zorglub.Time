// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Hemerology.YearNumberingTests

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time.Hemerology

open Xunit

// TODO(code): dépassements arithmétiques.

let decadeOfCenturyInfoData = YearNumberingDataSet.DecadeOfCenturyInfoData
let isoDecadeOfCenturyInfoData = YearNumberingDataSet.IsoDecadeOfCenturyInfoData
let decadeInfoData = YearNumberingDataSet.DecadeInfoData
let isoDecadeInfoData = YearNumberingDataSet.IsoDecadeInfoData
let centuryInfoData = YearNumberingDataSet.CenturyInfoData
let isoCenturyInfoData = YearNumberingDataSet.IsoCenturyInfoData
let millenniumInfoData = YearNumberingDataSet.MillenniumInfoData
let isoMillenniumInfoData = YearNumberingDataSet.IsoMillenniumInfoData

[<Theory; MemberData(nameof(centuryInfoData))>]
let ``GetCentury() plain`` (info: CenturyInfo) =
    YearNumbering.GetCentury(info.Year) === info.Century

[<Theory; MemberData(nameof(centuryInfoData))>]
let ``GetYearOfCentury()`` (info: CenturyInfo) =
    YearNumbering.GetYearOfCentury(info.Year) === int(info.YearOfCentury)

//
// Decades of the century
//

[<Theory; MemberData(nameof(decadeOfCenturyInfoData))>]
let ``GetDecadeOfCentury()`` (info: DecadeOfCenturyInfo) =
    let (decadeOfCentury, century, yearOfDecade) = YearNumbering.GetDecadeOfCentury(info.Year)

    century         === info.Century
    decadeOfCentury === int(info.DecadeOfCentury)
    yearOfDecade    === int(info.YearOfDecade)

[<Theory; MemberData(nameof(isoDecadeOfCenturyInfoData))>]
let ``GetIsoDecadeOfCentury()`` (info: DecadeOfCenturyInfo) =
    let (decadeOfCentury, century, yearOfDecade) = YearNumbering.GetIsoDecadeOfCentury(info.Year)

    century         === info.Century
    decadeOfCentury === int(info.DecadeOfCentury)
    yearOfDecade    === int(info.YearOfDecade)

//
// Decades
//

[<Theory; MemberData(nameof(decadeInfoData))>]
let ``GetDecade()`` (info: DecadeInfo) =
    let (decade, yearOfDecade) = YearNumbering.GetDecade(info.Year)

    decade       === info.Decade
    yearOfDecade === int(info.YearOfDecade)

[<Theory; MemberData(nameof(decadeInfoData))>]
let ``GetYearFromDecade()`` (info: DecadeInfo) =
    let (y, decade, yearOfDecade) = info.Deconstruct()

    YearNumbering.GetYearFromDecade(decade, int(yearOfDecade)) === y

[<Theory; MemberData(nameof(isoDecadeInfoData))>]
let ``GetIsoDecade()`` (info: DecadeInfo) =
    let (decade, yearOfDecade) = YearNumbering.GetIsoDecade(info.Year)

    decade       === info.Decade
    yearOfDecade === int(info.YearOfDecade)

[<Theory; MemberData(nameof(isoDecadeInfoData))>]
let ``GetYearFromIsoDecade()`` (info: DecadeInfo) =
    let (y, decade, yearOfDecade) = info.Deconstruct()

    YearNumbering.GetYearFromIsoDecade(decade, int(yearOfDecade)) === y

//
// Centuries
//

[<Theory; MemberData(nameof(centuryInfoData))>]
let ``GetCentury()`` (info: CenturyInfo) =
    let (century, yearOfCentury) = YearNumbering.GetCentury(info.Year)

    century       === info.Century
    yearOfCentury === int(info.YearOfCentury)

[<Theory; MemberData(nameof(centuryInfoData))>]
let ``GetYearFromCentury()`` (info: CenturyInfo) =
    let (y, century, yearOfCentury) = info.Deconstruct()

    YearNumbering.GetYearFromCentury(century, int(yearOfCentury)) === y

[<Theory; MemberData(nameof(isoCenturyInfoData))>]
let ``GetIsoCentury()`` (info: CenturyInfo) =
    let (century, yearOfCentury) = YearNumbering.GetIsoCentury(info.Year)

    century       === info.Century
    yearOfCentury === int(info.YearOfCentury)

[<Theory; MemberData(nameof(isoCenturyInfoData))>]
let ``GetYearFromIsoCentury()`` (info: CenturyInfo) =
    let (y, century, yearOfCentury) = info.Deconstruct()

    YearNumbering.GetYearFromIsoCentury(century, int(yearOfCentury)) === y

//
// Millennia
//

[<Theory; MemberData(nameof(millenniumInfoData))>]
let ``GetMillennium()`` (info: MillenniumInfo) =
    let (millennium, yearOfMillennium) = YearNumbering.GetMillennium(info.Year)

    millennium       === info.Millennium
    yearOfMillennium === int(info.YearOfMillennium)

[<Theory; MemberData(nameof(millenniumInfoData))>]
let ``GetYearFromMillennium()`` (info: MillenniumInfo) =
    let (y, millennium, yearOfMillennium) = info.Deconstruct()

    YearNumbering.GetYearFromMillennium(millennium, int(yearOfMillennium)) === y

[<Theory; MemberData(nameof(isoMillenniumInfoData))>]
let ``GetIsoMillennium()`` (info: MillenniumInfo) =
    let (millennium, yearOfMillennium) = YearNumbering.GetIsoMillennium(info.Year)

    millennium       === info.Millennium
    yearOfMillennium === int(info.YearOfMillennium)

[<Theory; MemberData(nameof(isoMillenniumInfoData))>]
let ``GetYearFromIsoMillennium()`` (info: MillenniumInfo) =
    let (y, millennium, yearOfMillennium) = info.Deconstruct()

    YearNumbering.GetYearFromIsoMillennium(millennium, int(yearOfMillennium)) === y
