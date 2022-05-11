﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.AdjustableDateTestSuite

open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time.Simple

type [<Sealed>] GregorianTests() =
    inherit IAdjustableDateFacts<CalendarDate, ProlepticGregorianDataSet>(GregorianCalendar.Instance.SupportedYears)

    override __.CreateDate(y, m, d) = GregorianCalendar.Instance.GetCalendarDate(y, m, d)
