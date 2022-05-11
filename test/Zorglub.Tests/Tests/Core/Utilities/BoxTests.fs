// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Utilities.BoxTests

open System

open Zorglub.Testing

open Zorglub.Time.Core.Utilities

open Xunit
open FsCheck
open FsCheck.Xunit

open Zorglub.Testing.XunitModule.BoxAssertions

module Prelude =
    [<Property>]
    let Constructor (x: NonNull<string>) =
        let box = new Box<string>(x.Get)

        box |> issome <| x.Get

    [<Fact>]
    let ``Static property Empty`` () =
        Box<Uri>.Empty |> isempty

module Factories =
    [<Fact>]
    let ``Empty()`` () =
        Box.Empty<string>() |> isempty

    [<Fact>]
    let ``Create(obj) when "obj" is empty`` () =
        Box.Create(null) |> isempty

    [<Property>]
    let ``Create(obj) when "obj" is not empty`` (x: NonNull<string>) =
        let box = Box.Create(x.Get)

        box |> issome <| x.Get

module MonadOps =
    let private uri = new Uri("http://www.narvalo.org")

    let private returnNull _ = null
    let private returnAbsoluteUri (x: Uri) = x.AbsoluteUri

    module EmptyCase =
        [<Fact>]
        let ``Select() throws when "selector" is null`` () =
            nullExn "selector" (fun () -> Box<Uri>.Empty.Select(null))

        [<Fact>]
        let ``Select() returns an empty box when "selector" always returns null`` () =
            Box<Uri>.Empty.Select(returnNull) |> isempty

        [<Fact>]
        let ``Select() returns an empty box when "selector" returns something`` () =
            Box<Uri>.Empty.Select(returnAbsoluteUri) |> isempty

        [<Fact>]
        let ``Flatten() returns an empty box when the box is empty`` () =
            Box<Box<Uri>>.Empty.Flatten() |> isempty

    module NonEmptyCase =
        [<Fact>]
        let ``Select() throws when "selector" is null`` () =
            nullExn "selector" (fun () -> Box.Create(uri).Select(null))

        [<Fact>]
        let ``Select() returns an empty box when "selector" always returns null`` () =
            Box.Create(uri).Select(returnNull) |> isempty

        [<Fact>]
        let ``Select() returns a non-empty box when "selector" returns something`` () =
            Box.Create(uri).Select(returnAbsoluteUri) |> issome <| uri.AbsoluteUri

        [<Fact>]
        let ``Flatten() returns an empty box when the box content is empty`` () =
            let box2 = Box.Create(Box<Uri>.Empty)
            box2.Flatten() |> isempty

        [<Fact>]
        let ``Flatten() returns a non-empty box when the box content is not empty`` () =
            let box2 = Box.Create(Box.Create(uri))
            box2.Flatten() |> issome <| uri
