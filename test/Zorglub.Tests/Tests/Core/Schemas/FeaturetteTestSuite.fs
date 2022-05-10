// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.FeaturetteTestSuite

open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core
open Zorglub.Time.Core.Schemas

module BlankDay =
    type [<Sealed>] InternationalFixedTests() =
        inherit IBlankDayFeaturetteFacts<InternationalFixedSchema, InternationalFixedDataSet>()

    type [<Sealed>] PositivistTests() =
        inherit IBlankDayFeaturetteFacts<PositivistSchema, PositivistDataSet>()

    type [<Sealed>] WorldTests() =
        inherit IBlankDayFeaturetteFacts<WorldSchema, WorldDataSet>()

module EpagomenalDay =
    type [<Sealed>] Coptic12Tests() =
        inherit IEpagomenalFeaturetteFacts<Coptic12Schema, Coptic12DataSet>()

    type [<Sealed>] Coptic13Tests() =
        inherit IEpagomenalFeaturetteFacts<Coptic13Schema, Coptic13DataSet>()

    type [<Sealed>] Egyptian12Tests() =
        inherit IEpagomenalFeaturetteFacts<Egyptian12Schema, Egyptian12DataSet>()

    type [<Sealed>] Egyptian13Tests() =
        inherit IEpagomenalFeaturetteFacts<Egyptian13Schema, Egyptian13DataSet>()

    type [<Sealed>] FrenchRepublican12Tests() =
        inherit IEpagomenalFeaturetteFacts<FrenchRepublican12Schema, FrenchRepublican12DataSet>()

    type [<Sealed>] FrenchRepublican13Tests() =
        inherit IEpagomenalFeaturetteFacts<FrenchRepublican13Schema, FrenchRepublican13DataSet>()
