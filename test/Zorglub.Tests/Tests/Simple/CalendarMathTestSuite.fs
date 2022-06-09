// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarMathTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

// TODO(code): WIP.

// We only need to test the following calendars
// - Gregorian (PlainMath, RegularMath, Regular12Math)          <- CalendricalProfile.Solar12
// - Positivist (PlainMath, RegularMath, Regular13Math)         <- CalendricalProfile.Solar13
// - Lunisolar (PlainMath), non-regular                         <- CalendricalProfile.Lunisolar
// but to be safe we also test
// - Coptic13 (PlainMath, RegularMath, Regular13Math)           <- CalendricalProfile.Other
// - TabularIslamic (PlainMath, RegularMath, Regular12Math)     <- CalendricalProfile.Lunar

module PlainMathCase =
    //[<Sealed>]
    //[<RedundantTestBundle>]
    //type Coptic13Tests() =
    //    inherit PlainMathFacts<StandardCoptic13DataSet>(new PlainMath(UserCalendars.Coptic13))

    [<Sealed>]
    type GregorianTests() =
        inherit PlainMathFacts<ProlepticGregorianDataSet>(new PlainMath(GregorianCalendar.Instance))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type LunisolarTests() =
        inherit PlainMathFacts<StandardLunisolarDataSet>(new PlainMath(UserCalendars.Lunisolar))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type PositivistTests() =
        inherit PlainMathFacts<StandardPositivistDataSet>(new PlainMath(UserCalendars.Positivist))

    [<Sealed>]
    [<RedundantTestBundle>]
    type TabularIslamicTests() =
        inherit PlainMathFacts<StandardTabularIslamicDataSet>(new PlainMath(TabularIslamicCalendar.Instance))

module RegularMathCase =
    //[<Sealed>]
    //[<RedundantTestBundle>]
    //type Coptic13Tests() =
    //    inherit CalendarMathFacts<StandardCoptic13DataSet>(new RegularMath(UserCalendars.Coptic13))

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

module Regular13MathCase =
    //[<Sealed>]
    //[<RedundantTestBundle>]
    //type Coptic13Tests() =
    //    inherit CalendarMathFacts<StandardCoptic13DataSet>(new Regular13Math(UserCalendars.Coptic13))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type PositivistTests() =
        inherit CalendarMathFacts<StandardPositivistDataSet>(new Regular13Math(UserCalendars.Positivist))

module MathAdvancedCase =
    //// PlainMath
    //[<Sealed>]
    //[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    //type GregorianRegularTests() =
    //    inherit CalendarMathAdvancedFacts<GregorianMathDataSetCutOff>(new PlainMath(GregorianCalendar.Instance))

    // RegularMath
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type GregorianRegularTests() =
        inherit CalendarMathAdvancedFacts<GregorianMathDataSetCutOff>(new RegularMath(GregorianCalendar.Instance))

    // Regular12Math
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type GregorianRegular12Tests() =
        inherit CalendarMathAdvancedFacts<GregorianMathDataSetCutOff>(new Regular12Math(GregorianCalendar.Instance))

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

// Regular12Math
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
