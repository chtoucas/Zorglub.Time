// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Hemerology.Scopes
open Zorglub.Time.Simple

[<Sealed>]
type ArmenianTests() =
    inherit CalendarFacts<StandardArmenian12DataSet>(ArmenianCalendar.Instance)

    override __.GetSingleton() = ArmenianCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Armenian
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<Solar12StandardShortScope>

[<Sealed>]
[<RedundantTestGroup>]
type CopticTests() =
    inherit CalendarFacts<StandardCoptic12DataSet>(CopticCalendar.Instance)

    override __.GetSingleton() = CopticCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Coptic
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<Solar12StandardShortScope>

[<Sealed>]
[<RedundantTestGroup>]
type EthiopicTests() =
    inherit CalendarFacts<StandardEthiopic12DataSet>(EthiopicCalendar.Instance)

    override __.GetSingleton() = EthiopicCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Ethiopic
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<Solar12StandardShortScope>

[<Sealed>]
[<RedundantTestGroup>]
type GregorianTests() =
    inherit CalendarFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance)

    override __.GetSingleton() = GregorianCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Gregorian
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<GregorianProlepticShortScope>

[<Sealed>]
[<RedundantTestGroup>]
type JulianTests() =
    inherit CalendarFacts<ProlepticJulianDataSet>(JulianCalendar.Instance)

    override __.GetSingleton() = JulianCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Julian
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<Solar12ProlepticShortScope>

[<Sealed>]
[<RedundantTestGroup>]
type TabularIslamicTests() =
    inherit CalendarFacts<StandardTabularIslamicDataSet>(TabularIslamicCalendar.Instance)

    override __.GetSingleton() = TabularIslamicCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.TabularIslamic
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<LunarStandardShortScope>

[<Sealed>]
[<RedundantTestGroup>]
type ZoroastrianTests() =
    inherit CalendarFacts<StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance)

    override __.GetSingleton() = ZoroastrianCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Zoroastrian
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<Solar12StandardShortScope>
