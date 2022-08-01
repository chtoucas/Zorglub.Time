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
        inherit IBlankDayFeaturetteFacts<InternationalFixedSchema, InternationalFixedDataSet>(schemaOf<InternationalFixedSchema>())

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type PositivistTests() =
        inherit IBlankDayFeaturetteFacts<PositivistSchema, PositivistDataSet>(schemaOf<PositivistSchema>())

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type WorldTests() =
        inherit IBlankDayFeaturetteFacts<WorldSchema, WorldDataSet>(schemaOf<WorldSchema>())

module EpagomenalDay =
    [<Sealed>]
    type Coptic12Tests() =
        inherit IEpagomenalDayFeaturetteFacts<Coptic12Schema, Coptic12DataSet>(schemaOf<Coptic12Schema>())

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type Coptic13Tests() =
        inherit IEpagomenalDayFeaturetteFacts<Coptic13Schema, Coptic13DataSet>(schemaOf<Coptic13Schema>())

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type Egyptian12Tests() =
        inherit IEpagomenalDayFeaturetteFacts<Egyptian12Schema, Egyptian12DataSet>(schemaOf<Egyptian12Schema>())

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type Egyptian13Tests() =
        inherit IEpagomenalDayFeaturetteFacts<Egyptian13Schema, Egyptian13DataSet>(schemaOf<Egyptian13Schema>())

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type FrenchRepublican12Tests() =
        inherit IEpagomenalDayFeaturetteFacts<FrenchRepublican12Schema, FrenchRepublican12DataSet>(schemaOf<FrenchRepublican12Schema>())

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type FrenchRepublican13Tests() =
        inherit IEpagomenalDayFeaturetteFacts<FrenchRepublican13Schema, FrenchRepublican13DataSet>(schemaOf<FrenchRepublican13Schema>())
