// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.SimpleCalendarTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Hemerology.Scopes
open Zorglub.Time.Simple

// Since the Gregorian calendar has the richest dataset, we use it as a default
// model for testing.

[<Sealed>]
[<RedundantTestBundle>]
type ArmenianTests() =
    inherit SimpleCalendarFacts<StandardArmenian12DataSet>(ArmenianCalendar.Instance)

    override __.GetSingleton() = ArmenianCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Armenian
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<StandardScope>

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit SimpleCalendarFacts<StandardCoptic12DataSet>(CopticCalendar.Instance)

    override __.GetSingleton() = CopticCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Coptic
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<StandardScope>

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit SimpleCalendarFacts<StandardEthiopic12DataSet>(EthiopicCalendar.Instance)

    override __.GetSingleton() = EthiopicCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Ethiopic
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<StandardScope>

[<Sealed>]
type GregorianTests() =
    inherit SimpleCalendarFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance)

    override __.GetSingleton() = GregorianCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Gregorian
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<ProlepticScope>

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit SimpleCalendarFacts<ProlepticJulianDataSet>(JulianCalendar.Instance)

    override __.GetSingleton() = JulianCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Julian
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<ProlepticScope>

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit SimpleCalendarFacts<StandardTabularIslamicDataSet>(TabularIslamicCalendar.Instance)

    override __.GetSingleton() = TabularIslamicCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.TabularIslamic
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<StandardScope>

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit SimpleCalendarFacts<StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance)

    override __.GetSingleton() = ZoroastrianCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Zoroastrian
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<StandardScope>

//
// User-defined calendars
//

[<Sealed>]
[<RedundantTestBundle>]
type UserGregorianTests() =
    inherit SimpleCalendarFacts<StandardGregorianDataSet>(UserCalendars.Gregorian)

    override __.GetSingleton() = UserCalendars.Gregorian
    override x.Id() = x.CalendarUT.Id === UserCalendars.Gregorian.Id
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<StandardScope>

[<Sealed>]
[<RedundantTestBundle>]
type UserJulianTests() =
    inherit SimpleCalendarFacts<ProlepticJulianDataSet>(UserCalendars.Julian)

    override __.GetSingleton() = UserCalendars.Julian
    override x.Id() = x.CalendarUT.Id === UserCalendars.Julian.Id
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<ProlepticScope>

[<Sealed>]
[<RedundantTestBundle>]
type UserLunisolarTests() =
    inherit SimpleCalendarFacts<StandardLunisolarDataSet>(UserCalendars.Lunisolar)

    override __.GetSingleton() = UserCalendars.Lunisolar
    override x.Id() = x.CalendarUT.Id === UserCalendars.Lunisolar.Id
    override x.Math() = x.CalendarUT.Math |> is<PlainMath>
    override x.Scope() = x.CalendarUT.Scope |> is<StandardScope>

[<Sealed>]
[<RedundantTestBundle>]
type UserPositivistTests() =
    inherit SimpleCalendarFacts<StandardPositivistDataSet>(UserCalendars.Positivist)

    override __.GetSingleton() = UserCalendars.Positivist
    override x.Id() = x.CalendarUT.Id === UserCalendars.Positivist.Id
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<StandardScope>
