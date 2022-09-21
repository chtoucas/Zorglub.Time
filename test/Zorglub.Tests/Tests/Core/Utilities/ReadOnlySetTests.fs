// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Utilities.ReadOnlySetTests

open System.Collections
open System.Collections.Generic
open System.Linq

open Zorglub.Testing

open Zorglub.Time.Core.Utilities

open Xunit
open FsCheck.Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when "set" is null`` () =
        nullExn "set" (fun () -> new ReadOnlySet<string>(null))

    [<Fact>]
    let ``Constructor throws when "collection" is null`` () =
        nullExn "collection" (fun () -> new ReadOnlySet<string>(null :> IEnumerable<string>))

    [<Property>]
    let ``Constructor using a seq`` (xs: int list) =
        let set = new ReadOnlySet<int>(xs)

        set.SetEquals(xs) |> ok

    [<Property>]
    let ``Constructor using an hashset`` (xs: int list) =
        let hashset = new HashSet<int>(xs)
        let set = new ReadOnlySet<int>(hashset)

        set.SetEquals(hashset) |> ok

    [<Property>]
    let ``Property Count`` (xs: int list) =
        let hashset = new HashSet<int>(xs)
        let set = new ReadOnlySet<int>(hashset)
        let count = hashset.Count

        set.Count = count

module Enumeration =
    [<Fact>]
    let ``GetEnumerator()`` () =
        let values = [| 1; 3; 2 |]
        let set = new ReadOnlySet<int>(values) :> IEnumerable<int>

        let mutable i = 0
        for n in set do
            n === values[i]
            i <- i + 1

    [<Property>]
    let ``GetEnumerator() generic`` (xs: int list) =
        let hashset = new HashSet<int>(xs)
        let set = new ReadOnlySet<int>(hashset) :> IEnumerable<int>
        let seq = hashset :> IEnumerable<int>

        // We use the Xunit assertion to verify the equality of sequences.
        set === seq

    [<Property>]
    let ``GetEnumerator() non-generic`` (xs: int list) =
        let hashset = new HashSet<int>(xs)
        let set = new ReadOnlySet<int>(hashset) :> IEnumerable
        let seq = hashset :> IEnumerable

        // We use the Xunit assertion to verify the equality of sequences.
        set === seq

module SetOps =
    let private seq0 = SingletonSet.Create(0)
    let private seq1 = SingletonSet.Create(1)
    let private seq01 = seq { yield 0; yield 1 }

    [<Property>]
    let ``Contains() returns true when "item" is an element of the set`` (xs: int list) =
        let hashset = new HashSet<int>(xs)
        let set = new ReadOnlySet<int>(hashset)

        for v in hashset do
            set.Contains(v) |> ok

    [<Fact>]
    let ``Contains() returns false when "item" is not an element of the set`` () =
        let values = seq {
            yield "un"
            yield "deux"
            yield "trois"
        }
        let set = new ReadOnlySet<string>(values)

        set.Contains(null)    |> nok
        set.Contains("")      |> nok
        set.Contains("one")   |> nok
        set.Contains("two")   |> nok
        set.Contains("three") |> nok

    [<Fact>]
    let ``IsProperSubsetOf()`` () =
        let set0 = new ReadOnlySet<int>(seq0)
        let set01 = new ReadOnlySet<int>(seq01)

        set0.IsProperSubsetOf(Enumerable.Empty<int>()) |> nok
        set0.IsProperSubsetOf(seq0)  |> nok
        set0.IsProperSubsetOf(seq1)  |> nok
        set0.IsProperSubsetOf(seq01) |> ok

        set01.IsProperSubsetOf(Enumerable.Empty<int>()) |> nok
        set01.IsProperSubsetOf(seq0)  |> nok
        set01.IsProperSubsetOf(seq1)  |> nok
        set01.IsProperSubsetOf(seq01) |> nok

    [<Fact>]
    let ``IsProperSupersetOf()`` () =
        let set0 = new ReadOnlySet<int>(seq0)
        let set01 = new ReadOnlySet<int>(seq01)

        set0.IsProperSupersetOf(Enumerable.Empty<int>()) |> ok
        set0.IsProperSupersetOf(seq0)  |> nok
        set0.IsProperSupersetOf(seq1)  |> nok
        set0.IsProperSupersetOf(seq01) |> nok

        set01.IsProperSupersetOf(Enumerable.Empty<int>()) |> ok
        set01.IsProperSupersetOf(seq0)  |> ok
        set01.IsProperSupersetOf(seq1)  |> ok
        set01.IsProperSupersetOf(seq01) |> nok

    [<Fact>]
    let ``IsSubsetOf()`` () =
        let set0 = new ReadOnlySet<int>(seq0)
        let set01 = new ReadOnlySet<int>(seq01)

        set0.IsSubsetOf(Enumerable.Empty<int>()) |> nok
        set0.IsSubsetOf(seq0)  |> ok
        set0.IsSubsetOf(seq1)  |> nok
        set0.IsSubsetOf(seq01) |> ok

        set01.IsSubsetOf(Enumerable.Empty<int>()) |> nok
        set01.IsSubsetOf(seq0)  |> nok
        set01.IsSubsetOf(seq1)  |> nok
        set01.IsSubsetOf(seq01) |> ok

    [<Fact>]
    let ``IsSupersetOf()`` () =
        let set0 = new ReadOnlySet<int>(seq0)
        let set01 = new ReadOnlySet<int>(seq01)

        set0.IsSupersetOf(Enumerable.Empty<int>()) |> ok
        set0.IsSupersetOf(seq0)  |> ok
        set0.IsSupersetOf(seq1)  |> nok
        set0.IsSupersetOf(seq01) |> nok

        set01.IsSupersetOf(Enumerable.Empty<int>()) |> ok
        set01.IsSupersetOf(seq0)  |> ok
        set01.IsSupersetOf(seq1)  |> ok
        set01.IsSupersetOf(seq01) |> ok

    [<Fact>]
    let ``Overlaps()`` () =
        let set0 = new ReadOnlySet<int>(seq0)

        set0.Overlaps(Enumerable.Empty<int>()) |> nok
        set0.Overlaps(seq0)  |> ok
        set0.Overlaps(seq1)  |> nok
        set0.Overlaps(seq01) |> ok

    [<Fact>]
    let ``SetEquals()`` () =
        let set0 = new ReadOnlySet<int>(seq0)

        set0.SetEquals(Enumerable.Empty<int>()) |> nok
        set0.SetEquals(seq0)  |> ok
        set0.SetEquals(seq1)  |> nok
        set0.SetEquals(seq01) |> nok
