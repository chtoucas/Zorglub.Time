// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Extensions.UnboxingTests

open System

open Zorglub.Testing

open Zorglub.Time.Core.Utilities

open Xunit
open FsCheck
open FsCheck.Xunit

open type Zorglub.Time.Extensions.Unboxing

[<Fact>]
let ``Unbox() throws when the box is empty`` () =
    let v = Box<string>.Empty

    throws<InvalidOperationException> (fun () -> v.Unbox())

[<Property>]
let ``Unbox() when the box is not empty`` (x: NonNull<string>) =
    let v = Box.Create(x.Get)

    v.Unbox() ==& x.Get

[<Fact>]
let ``TryUnbox() when the box is empty`` () =
    let v = Box<Uri>.Empty
    let succeed, w = v.TryUnbox()

    succeed |> nok
    w |> isnull

[<Fact>]
let ``TryUnbox() when the box is not empty`` () =
    let uri = new Uri("http://www.narvalo.org")
    let v = Box.Create(uri)
    let succeed, w = v.TryUnbox()

    succeed |> ok
    w ==& uri
