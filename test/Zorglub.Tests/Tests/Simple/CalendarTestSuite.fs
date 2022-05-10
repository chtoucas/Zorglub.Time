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
    inherit CalendarFacts<ArmenianCalendar, StandardArmenian12DataSet>(ArmenianCalendar.Instance)

    override __.GetSingleton() = ArmenianCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Armenian
    // We don't test this property here but in CalendarTests.Arithmetic...
    override __.Math() = ()
    override x.Scope() = x.CalendarUT.Scope |> is<Solar12StandardShortScope>

[<Sealed>]
type CopticTests() =
    inherit CalendarFacts<CopticCalendar, StandardCoptic12DataSet>(CopticCalendar.Instance)

    override __.GetSingleton() = CopticCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Coptic
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<Solar12StandardShortScope>

[<Sealed>]
type EthiopicTests() =
    inherit CalendarFacts<EthiopicCalendar, StandardEthiopic12DataSet>(EthiopicCalendar.Instance)

    override __.GetSingleton() = EthiopicCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Ethiopic
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<Solar12StandardShortScope>

[<Sealed>]
type GregorianTests() =
    inherit CalendarFacts<GregorianCalendar, ProlepticGregorianDataSet>(GregorianCalendar.Instance)

    override __.GetSingleton() = GregorianCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Gregorian
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<GregorianProlepticShortScope>

[<Sealed>]
type JulianTests() =
    inherit CalendarFacts<JulianCalendar, ProlepticJulianDataSet>(JulianCalendar.Instance)

    override __.GetSingleton() = JulianCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Julian
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<Solar12ProlepticShortScope>

[<Sealed>]
type TabularIslamicTests() =
    inherit CalendarFacts<TabularIslamicCalendar, StandardTabularIslamicDataSet>(TabularIslamicCalendar.Instance)

    override __.GetSingleton() = TabularIslamicCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.TabularIslamic
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<LunarStandardShortScope>

[<Sealed>]
type ZoroastrianTests() =
    inherit CalendarFacts<ZoroastrianCalendar, StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance)

    override __.GetSingleton() = ZoroastrianCalendar.Instance
    override x.Id() = x.CalendarUT.Id === Cuid.Zoroastrian
    override x.Math() = x.CalendarUT.Math |> is<Regular12Math>
    override x.Scope() = x.CalendarUT.Scope |> is<Solar12StandardShortScope>
