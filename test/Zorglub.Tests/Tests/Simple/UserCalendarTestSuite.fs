// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.UserCalendarTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Hemerology.Scopes
open Zorglub.Time.Simple

// NB: don't remove the proleptic calendar, it's useful to check that everything
// continues to work with negative years.

[<Sealed>]
[<RedundantTestGroup>]
type UserGregorianTests() =
    inherit CalendarFacts<StandardGregorianDataSet>(UserCalendars.Gregorian)

    override __.GetSingleton() = UserCalendars.Gregorian
    override x.Id() = x.CalendarUT.Id === UserCalendars.Gregorian.Id
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<GregorianStandardShortScope>

[<Sealed>]
type UserJulianTests() =
    inherit CalendarFacts<ProlepticJulianDataSet>(UserCalendars.Julian)

    override __.GetSingleton() = UserCalendars.Julian
    override x.Id() = x.CalendarUT.Id === UserCalendars.Julian.Id
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<Solar12ProlepticShortScope>
