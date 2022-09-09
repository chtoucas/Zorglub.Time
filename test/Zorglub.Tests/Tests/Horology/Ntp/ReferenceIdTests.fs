// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Horology.Ntp.ReferenceIdTests

open Zorglub.Time.Horology.Ntp

open FsCheck
open FsCheck.Xunit

module Equality =
    open NonStructuralComparison

    // fsharplint:disable Hints
    [<Property>]
    let ``Equality when both operands are identical`` (x: ReferenceId) =
        x = x
        .&. not (x <> x)
        .&. x.Equals(x)
        .&. x.Equals(x :> obj)

    [<Property>]
    let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: ReferenceId) =
        not (x.Equals(null))
        .&. not (x.Equals(new obj()))

    [<Property>]
    let ``GetHashCode() is invariant`` (x: ReferenceId) =
        x.GetHashCode() = x.GetHashCode()
