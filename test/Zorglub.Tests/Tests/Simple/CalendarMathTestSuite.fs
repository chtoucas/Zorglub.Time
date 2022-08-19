// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarMathTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Simple

open Zorglub.Time
open Zorglub.Time.Simple

// TODO(code): WIP.

// We only need to test the following calendars
// - Gregorian (PlainMath, RegularMath)          <- CalendricalProfile.Solar12
// - Positivist (PlainMath, RegularMath)         <- CalendricalProfile.Solar13
// - Lunisolar (PlainMath), non-regular          <- CalendricalProfile.Lunisolar
// but to be safe we also test
// - Coptic13 (PlainMath, RegularMath)           <- CalendricalProfile.Other
// - TabularIslamic (PlainMath, RegularMath)     <- CalendricalProfile.Lunar

module PlainMathCase =
    [<Sealed>]
    [<RedundantTestBundle>]
    type Coptic13Tests() =
        inherit CalendarMathFacts<StandardCoptic13DataSet>(new PlainMath(UserCalendars.Coptic13))

    [<Sealed>]
    [<RedundantTestBundle>]
    type CivilTests() =
        inherit CalendarMathFacts<StandardGregorianDataSet>(new PlainMath(SimpleCalendar.Civil))

    [<Sealed>]
    type GregorianTests() =
        inherit CalendarMathFacts<ProlepticGregorianDataSet>(new PlainMath(SimpleCalendar.Gregorian))

    //[<Sealed>]
    //[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    //type LunisolarTests() =
    //    inherit CalendarMathFacts<StandardLunisolarDataSet>(new PlainMath(UserCalendars.Lunisolar))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type PositivistTests() =
        inherit CalendarMathFacts<StandardPositivistDataSet>(new PlainMath(UserCalendars.Positivist))

    [<Sealed>]
    [<RedundantTestBundle>]
    type TabularIslamicTests() =
        inherit CalendarMathFacts<StandardTabularIslamicDataSet>(new PlainMath(SimpleCalendar.TabularIslamic))

module RegularMathCase =
    [<Sealed>]
    [<RedundantTestBundle>]
    type Coptic13Tests() =
        inherit CalendarMathFacts<StandardCoptic13DataSet>(new RegularMath(UserCalendars.Coptic13))

    [<Sealed>]
    [<RedundantTestBundle>]
    type CivilTests() =
        inherit CalendarMathFacts<StandardGregorianDataSet>(new RegularMath(SimpleCalendar.Civil))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type GregorianTests() =
        inherit CalendarMathFacts<ProlepticGregorianDataSet>(new RegularMath(SimpleCalendar.Gregorian))

    [<Sealed>]
    [<RedundantTestBundle>]
    type PositivistTests() =
        inherit CalendarMathFacts<StandardPositivistDataSet>(new RegularMath(UserCalendars.Positivist))

    [<Sealed>]
    [<RedundantTestBundle>]
    type TabularIslamicTests() =
        inherit CalendarMathFacts<StandardTabularIslamicDataSet>(new RegularMath(SimpleCalendar.TabularIslamic))

module MathAdvancedCase =
    //// PlainMath (not yet fully implemented).
    //[<Sealed>]
    //[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    //type GregorianPlainTests() =
    //    inherit CalendarMathAdvancedFacts<GregorianMathDataSetCutOff>(new PlainMath(SimpleCalendar.Gregorian))

    // RegularMath
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type GregorianRegularTests() =
        inherit CalendarMathAdvancedFacts<GregorianMathDataSetCutOff>(new RegularMath(SimpleCalendar.Gregorian))

    // RegularMath
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type GregorianRegular12Tests() =
        inherit CalendarMathAdvancedFacts<GregorianMathDataSetCutOff>(new RegularMath(SimpleCalendar.Gregorian))

module PowerMathCase =
    let ruleset = new AdditionRuleset()

    [<Sealed>]
    [<RedundantTestBundle>]
    type Coptic13Tests() =
        inherit CalendarMathFacts<StandardCoptic13DataSet>(
            new PowerMath(UserCalendars.Coptic13, ruleset))

    [<Sealed>]
    [<RedundantTestBundle>]
    type CivilTests() =
        inherit CalendarMathFacts<StandardGregorianDataSet>(
            new PowerMath(SimpleCalendar.Civil, ruleset))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type GregorianTests() =
        inherit CalendarMathFacts<ProlepticGregorianDataSet>(
            new PowerMath(SimpleCalendar.Gregorian, ruleset))

    [<Sealed>]
    [<RedundantTestBundle>]
    type PositivistTests() =
        inherit CalendarMathFacts<StandardPositivistDataSet>(
            new PowerMath(UserCalendars.Positivist, ruleset))

    [<Sealed>]
    [<RedundantTestBundle>]
    type TabularIslamicTests() =
        inherit CalendarMathFacts<StandardTabularIslamicDataSet>(
            new PowerMath(SimpleCalendar.TabularIslamic, ruleset))

//
// Default Math
//

[<Sealed>]
[<RedundantTestBundle>]
type ArmenianTests() =
    inherit CalendarMathFacts<StandardArmenian12DataSet>(SimpleCalendar.Armenian)

[<Sealed>]
[<RedundantTestBundle>]
type CivilTests() =
    inherit CalendarMathFacts<StandardGregorianDataSet>(SimpleCalendar.Civil)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit CalendarMathFacts<StandardCoptic12DataSet>(SimpleCalendar.Coptic)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit CalendarMathFacts<StandardEthiopic12DataSet>(SimpleCalendar.Ethiopic)

// RegularMath
[<Sealed>]
type GregorianTests() =
    inherit CalendarMathFacts<ProlepticGregorianDataSet>(SimpleCalendar.Gregorian)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit CalendarMathFacts<ProlepticJulianDataSet>(SimpleCalendar.Julian)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit CalendarMathFacts<StandardTabularIslamicDataSet>(SimpleCalendar.TabularIslamic)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit CalendarMathFacts<StandardZoroastrian12DataSet>(SimpleCalendar.Zoroastrian)
