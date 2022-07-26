// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.CalendricalSegmentBuilderTests

open System

open Zorglub.Testing

open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas

open Xunit

module Prelude =
    [<Fact>]
    let ``Properties`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        builder.HasMin |> nok
        builder.HasMax |> nok
        builder.IsBuildable |> nok

module Build =
    [<Fact>]
    let ``BuildSegment() throws when not buildable`` () =
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())

        throws<InvalidOperationException> (fun () -> builder.BuildSegment())
