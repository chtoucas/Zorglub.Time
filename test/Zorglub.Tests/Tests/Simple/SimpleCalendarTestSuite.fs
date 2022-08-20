// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.SimpleCalendarTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Core.Intervals
open Zorglub.Time.Simple

// Since the Gregorian calendar has the richest dataset, we use it as a default
// model for testing.

[<Sealed>]
[<RedundantTestBundle>]
type ArmenianTests() =
    inherit SimpleCalendarFacts<StandardArmenian12DataSet>(SimpleCalendar.Armenian)

    override __.GetSingleton() = SimpleCalendar.Armenian
    override x.Id() = x.CalendarUT.Id === Cuid.Armenian
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope.Segment.SupportedYears === Range.Create(1, 9999)

[<Sealed>]
type CivilTests() =
    inherit SimpleCalendarFacts<StandardGregorianDataSet>(SimpleCalendar.Civil)

    override __.GetSingleton() = SimpleCalendar.Civil
    override x.Id() = x.CalendarUT.Id === Cuid.Civil
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope.Segment.SupportedYears === Range.Create(1, 9999)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit SimpleCalendarFacts<StandardCoptic12DataSet>(SimpleCalendar.Coptic)

    override __.GetSingleton() = SimpleCalendar.Coptic
    override x.Id() = x.CalendarUT.Id === Cuid.Coptic
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope.Segment.SupportedYears === Range.Create(1, 9999)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit SimpleCalendarFacts<StandardEthiopic12DataSet>(SimpleCalendar.Ethiopic)

    override __.GetSingleton() = SimpleCalendar.Ethiopic
    override x.Id() = x.CalendarUT.Id === Cuid.Ethiopic
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope.Segment.SupportedYears === Range.Create(1, 9999)

[<Sealed>]
type GregorianTests() =
    inherit SimpleCalendarFacts<ProlepticGregorianDataSet>(SimpleCalendar.Gregorian)

    override __.GetSingleton() = SimpleCalendar.Gregorian
    override x.Id() = x.CalendarUT.Id === Cuid.Gregorian
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope.Segment.SupportedYears === Range.Create(-9998, 9999)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit SimpleCalendarFacts<ProlepticJulianDataSet>(SimpleCalendar.Julian)

    override __.GetSingleton() = SimpleCalendar.Julian
    override x.Id() = x.CalendarUT.Id === Cuid.Julian
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope.Segment.SupportedYears === Range.Create(-9998, 9999)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit SimpleCalendarFacts<StandardTabularIslamicDataSet>(SimpleCalendar.TabularIslamic)

    override __.GetSingleton() = SimpleCalendar.TabularIslamic
    override x.Id() = x.CalendarUT.Id === Cuid.TabularIslamic
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope.Segment.SupportedYears === Range.Create(1, 9999)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit SimpleCalendarFacts<StandardZoroastrian12DataSet>(SimpleCalendar.Zoroastrian)

    override __.GetSingleton() = SimpleCalendar.Zoroastrian
    override x.Id() = x.CalendarUT.Id === Cuid.Zoroastrian
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope.Segment.SupportedYears === Range.Create(1, 9999)

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
    override x.Scope() = x.CalendarUT.Scope.Segment.SupportedYears === Range.Create(1, 9999)

[<Sealed>]
[<RedundantTestBundle>]
type UserJulianTests() =
    inherit SimpleCalendarFacts<ProlepticJulianDataSet>(UserCalendars.Julian)

    override __.GetSingleton() = UserCalendars.Julian
    override x.Id() = x.CalendarUT.Id === UserCalendars.Julian.Id
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope.Segment.SupportedYears === Range.Create(-9998, 9999)

[<Sealed>]
[<RedundantTestBundle>]
type UserLunisolarTests() =
    inherit SimpleCalendarFacts<StandardLunisolarDataSet>(UserCalendars.Lunisolar)

    override __.GetSingleton() = UserCalendars.Lunisolar
    override x.Id() = x.CalendarUT.Id === UserCalendars.Lunisolar.Id
    override x.Math() = x.CalendarUT.Math |> is<PlainMath>
    override x.Scope() = x.CalendarUT.Scope.Segment.SupportedYears === Range.Create(1, 9999)

[<Sealed>]
[<RedundantTestBundle>]
type UserPositivistTests() =
    inherit SimpleCalendarFacts<StandardPositivistDataSet>(UserCalendars.Positivist)

    override __.GetSingleton() = UserCalendars.Positivist
    override x.Id() = x.CalendarUT.Id === UserCalendars.Positivist.Id
    override x.Math() = x.CalendarUT.Math |> is<RegularMath>
    override x.Scope() = x.CalendarUT.Scope.Segment.SupportedYears === Range.Create(1, 9999)
