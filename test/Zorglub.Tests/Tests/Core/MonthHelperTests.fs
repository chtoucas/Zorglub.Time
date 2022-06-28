﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.MonthHelperTests

open Zorglub.Testing

open Zorglub.Time.Core

open Xunit

module Factories =
    [<Fact>]
    let ``Create() throws for null schema`` () =
        nullExn "schema" (fun () -> MonthHelper.Create(null))

