// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.FeaturetteTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core.Schemas

module BlankDay =
    [<Sealed>]
    type InternationalFixedTests() =
        inherit IBlankDayFeaturetteFacts<InternationalFixedSchema, InternationalFixedDataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type PositivistTests() =
        inherit IBlankDayFeaturetteFacts<PositivistSchema, PositivistDataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type WorldTests() =
        inherit IBlankDayFeaturetteFacts<WorldSchema, WorldDataSet>()

module EpagomenalDay =
    [<Sealed>]
    type Coptic12Tests() =
        inherit IEpagomenalFeaturetteFacts<Coptic12Schema, Coptic12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type Coptic13Tests() =
        inherit IEpagomenalFeaturetteFacts<Coptic13Schema, Coptic13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type Egyptian12Tests() =
        inherit IEpagomenalFeaturetteFacts<Egyptian12Schema, Egyptian12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type Egyptian13Tests() =
        inherit IEpagomenalFeaturetteFacts<Egyptian13Schema, Egyptian13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type FrenchRepublican12Tests() =
        inherit IEpagomenalFeaturetteFacts<FrenchRepublican12Schema, FrenchRepublican12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type FrenchRepublican13Tests() =
        inherit IEpagomenalFeaturetteFacts<FrenchRepublican13Schema, FrenchRepublican13DataSet>()
