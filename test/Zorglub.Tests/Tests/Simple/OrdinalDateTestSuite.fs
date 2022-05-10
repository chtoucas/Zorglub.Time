// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.OrdinalDateTestSuite

open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple
open Zorglub.Time.Simple

type [<Sealed>] GregorianTests() =
    inherit OrdinalDateFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance, JulianCalendar.Instance)
