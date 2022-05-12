// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarDateTestSuite

open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

[<Sealed>]
type GregorianTests() =
    inherit CalendarDateFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance, JulianCalendar.Instance)
