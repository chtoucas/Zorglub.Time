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
    type GregorianTests() =
        inherit CalendarMathFacts<ProlepticGregorianDataSet>(new PlainMath(GregorianCalendar.Instance))

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
        inherit CalendarMathFacts<StandardTabularIslamicDataSet>(new PlainMath(TabularIslamicCalendar.Instance))

module RegularMathCase =
    [<Sealed>]
    [<RedundantTestBundle>]
    type Coptic13Tests() =
        inherit CalendarMathFacts<StandardCoptic13DataSet>(new RegularMath(UserCalendars.Coptic13))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type GregorianTests() =
        inherit CalendarMathFacts<ProlepticGregorianDataSet>(new RegularMath(GregorianCalendar.Instance))

    [<Sealed>]
    [<RedundantTestBundle>]
    type PositivistTests() =
        inherit CalendarMathFacts<StandardPositivistDataSet>(new RegularMath(UserCalendars.Positivist))

    [<Sealed>]
    [<RedundantTestBundle>]
    type TabularIslamicTests() =
        inherit CalendarMathFacts<StandardTabularIslamicDataSet>(new RegularMath(TabularIslamicCalendar.Instance))

module MathAdvancedCase =
    //// PlainMath (not yet fully implemented).
    //[<Sealed>]
    //[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    //type GregorianPlainTests() =
    //    inherit CalendarMathAdvancedFacts<GregorianMathDataSetCutOff>(new PlainMath(GregorianCalendar.Instance))

    // RegularMath
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type GregorianRegularTests() =
        inherit CalendarMathAdvancedFacts<GregorianMathDataSetCutOff>(new RegularMath(GregorianCalendar.Instance))

    // RegularMath
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type GregorianRegular12Tests() =
        inherit CalendarMathAdvancedFacts<GregorianMathDataSetCutOff>(new RegularMath(GregorianCalendar.Instance))

module PowerMathCase =
    let ruleset = new AdditionRuleset()

    [<Sealed>]
    [<RedundantTestBundle>]
    type Coptic13Tests() =
        inherit CalendarMathFacts<StandardCoptic13DataSet>(
            new PowerMath(UserCalendars.Coptic13, ruleset))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type GregorianTests() =
        inherit CalendarMathFacts<ProlepticGregorianDataSet>(
            new PowerMath(GregorianCalendar.Instance, ruleset))

    [<Sealed>]
    [<RedundantTestBundle>]
    type PositivistTests() =
        inherit CalendarMathFacts<StandardPositivistDataSet>(
            new PowerMath(UserCalendars.Positivist, ruleset))

    [<Sealed>]
    [<RedundantTestBundle>]
    type TabularIslamicTests() =
        inherit CalendarMathFacts<StandardTabularIslamicDataSet>(
            new PowerMath(TabularIslamicCalendar.Instance, ruleset))

//
// Default Math
//

[<Sealed>]
[<RedundantTestBundle>]
type ArmenianTests() =
    inherit CalendarMathFacts<StandardArmenian12DataSet>(ArmenianCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit CalendarMathFacts<StandardCoptic12DataSet>(CopticCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit CalendarMathFacts<StandardEthiopic12DataSet>(EthiopicCalendar.Instance)

// RegularMath
[<Sealed>]
type GregorianTests() =
    inherit CalendarMathFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit CalendarMathFacts<ProlepticJulianDataSet>(JulianCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit CalendarMathFacts<StandardTabularIslamicDataSet>(TabularIslamicCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit CalendarMathFacts<StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance)
