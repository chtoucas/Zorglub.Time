// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.UserCalendarTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Hemerology.Scopes
open Zorglub.Time.Simple

// NB: for smoke testing we keep the proleptic calendar to verify that everything
// continues to work with negative years.

[<Sealed>]
type UserJulianTests() =
    inherit CalendarFacts<Calendar, ProlepticJulianDataSet>(UserCalendars.Julian)

    override __.GetSingleton() = UserCalendars.Julian
    override x.Id() = x.CalendarUT.Id === UserCalendars.Julian.Id
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<Solar12ProlepticShortScope>

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type UserGregorianTests() =
    inherit CalendarFacts<Calendar, StandardGregorianDataSet>(UserCalendars.Gregorian)

    override __.GetSingleton() = UserCalendars.Gregorian
    override x.Id() = x.CalendarUT.Id === UserCalendars.Gregorian.Id
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<GregorianStandardShortScope>
