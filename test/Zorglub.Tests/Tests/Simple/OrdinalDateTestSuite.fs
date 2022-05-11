// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.OrdinalDateTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

open Xunit

type [<Sealed>] GregorianTests() =
    inherit OrdinalDateFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance, JulianCalendar.Instance)

    [<Theory>]
    [<InlineData(-1, 1, "001/-0001 (Gregorian)")>]
    [<InlineData(0, 1, "001/0000 (Gregorian)")>]
    [<InlineData(1, 1, "001/0001 (Gregorian)")>]
    [<InlineData(1, 3, "003/0001 (Gregorian)")>]
    [<InlineData(11, 254, "254/0011 (Gregorian)")>]
    [<InlineData(111, 26, "026/0111 (Gregorian)")>]
    [<InlineData(2019, 3, "003/2019 (Gregorian)")>]
    [<InlineData(9999, 365, "365/9999 (Gregorian)")>]
    member x.ToString_InvariantCulture(y, doy, str) =
        let date = x.CalendarUT.GetOrdinalDate(y, doy);
        date.ToString() === str