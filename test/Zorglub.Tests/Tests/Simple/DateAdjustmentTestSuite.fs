﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.DateAdjustmentTestSuite

open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time.Simple

[<Sealed>]
type GregorianTests() =
    inherit IAdjustableDateFacts<CalendarDate, ProlepticGregorianDataSet>(GregorianCalendar.Instance.SupportedYears)

    override __.GetDate(y, m, d) = GregorianCalendar.Instance.GetCalendarDate(y, m, d)

