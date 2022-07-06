// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarTestSuite

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
    inherit CalendarFacts<StandardArmenian12DataSet>(ArmenianCalendar.Instance)

    override __.GetSingleton() = ArmenianCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Armenian
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<PlainStandardShortScope>

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit CalendarFacts<StandardCoptic12DataSet>(CopticCalendar.Instance)

    override __.GetSingleton() = CopticCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Coptic
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<PlainStandardShortScope>

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit CalendarFacts<StandardEthiopic12DataSet>(EthiopicCalendar.Instance)

    override __.GetSingleton() = EthiopicCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Ethiopic
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<PlainStandardShortScope>

[<Sealed>]
type GregorianTests() =
    inherit CalendarFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance)

    override __.GetSingleton() = GregorianCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Gregorian
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<GregorianProlepticShortScope>

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit CalendarFacts<ProlepticJulianDataSet>(JulianCalendar.Instance)

    override __.GetSingleton() = JulianCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Julian
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<PlainProlepticShortScope>

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit CalendarFacts<StandardTabularIslamicDataSet>(TabularIslamicCalendar.Instance)

    override __.GetSingleton() = TabularIslamicCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.TabularIslamic
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<PlainStandardShortScope>

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit CalendarFacts<StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance)

    override __.GetSingleton() = ZoroastrianCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Zoroastrian
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<PlainStandardShortScope>

//
// User-defined calendars
//

[<Sealed>]
[<RedundantTestBundle>]
type UserGregorianTests() =
    inherit CalendarFacts<StandardGregorianDataSet>(UserCalendars.Gregorian)

    override __.GetSingleton() = UserCalendars.Gregorian
    override x.Id() = x.CalendarUT.Id === UserCalendars.Gregorian.Id
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<GregorianStandardShortScope>

[<Sealed>]
[<RedundantTestBundle>]
type UserJulianTests() =
    inherit CalendarFacts<ProlepticJulianDataSet>(UserCalendars.Julian)

    override __.GetSingleton() = UserCalendars.Julian
    override x.Id() = x.CalendarUT.Id === UserCalendars.Julian.Id
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<PlainProlepticShortScope>

[<Sealed>]
[<RedundantTestBundle>]
type UserLunisolarTests() =
    inherit CalendarFacts<StandardLunisolarDataSet>(UserCalendars.Lunisolar)

    override __.GetSingleton() = UserCalendars.Lunisolar
    override x.Id() = x.CalendarUT.Id === UserCalendars.Lunisolar.Id
    override x.Math() = x.CalendarUT.Math |> is<PlainMath>
    override x.Scope() = x.CalendarUT.Scope |> is<PlainStandardShortScope>

[<Sealed>]
[<RedundantTestBundle>]
type UserPositivistTests() =
    inherit CalendarFacts<StandardPositivistDataSet>(UserCalendars.Positivist)

    override __.GetSingleton() = UserCalendars.Positivist
    override x.Id() = x.CalendarUT.Id === UserCalendars.Positivist.Id
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope |> is<PlainStandardShortScope>
