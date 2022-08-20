// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Intervals.RangeTests

open System
open System.Linq

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Utilities

open Xunit
open FsCheck
open FsCheck.Xunit

module Prelude =
    [<Fact>]
    let ``Default value`` () =
        let v = new Range<int>(0, 0)

        Unchecked.defaultof<Range<int>> === v

    [<Fact>]
    let ``Static property Range.Maximal32`` () =
        let v = Range.Maximal32
        let endpoints = OrderedPair.Create(Int32.MinValue, Int32.MaxValue)

        v.Endpoints === endpoints
        v.Min === Int32.MinValue
        v.Max === Int32.MaxValue
        v.IsLeftOpen        |> nok
        v.IsRightOpen       |> nok
        v.IsLeftBounded     |> ok
        v.IsRightBounded    |> ok
        v.IsSingleton       |> nok
        v.ToString() === "[-2147483648..2147483647]"

        // ISegment
        let segment = v :> ISegment<int>
        segment.LowerEnd === Int32.MinValue
        segment.UpperEnd === Int32.MaxValue

        // Constructor
        let other = new Range<int>(Int32.MinValue, Int32.MaxValue)
        v === other

module Factories =
    [<Property>]
    let ``Range.Create() throws when max < min`` (x: Pair<int>) =
        outOfRangeExn "max" (fun () -> Range.Create(x.Max, x.Min))

    [<Property>]
    let ``Range.Create()`` (x: Pair<int>) =
        let v = Range.Create(x.Min, x.Max)
        let endpoints = OrderedPair.Create(x.Min, x.Max)

        v.Endpoints === endpoints
        v.Min === x.Min
        v.Max === x.Max
        v.IsLeftOpen        |> nok
        v.IsRightOpen       |> nok
        v.IsLeftBounded     |> ok
        v.IsRightBounded    |> ok
        v.IsSingleton       |> nok
        v.ToString() === sprintf "[%i..%i]" x.Min x.Max

        // ISegment
        let segment = v :> ISegment<int>
        segment.LowerEnd === x.Min
        segment.UpperEnd === x.Max

        // Constructor
        let other = new Range<int>(x.Min, x.Max)
        v === other

    [<Property>]
    let ``Range.Create() when singleton`` (i: int) =
        let v = Range.Create(i, i)
        let endpoints = OrderedPair.Create(i, i)

        v.Endpoints === endpoints
        v.Min === i
        v.Max === i
        v.IsLeftOpen        |> nok
        v.IsRightOpen       |> nok
        v.IsLeftBounded     |> ok
        v.IsRightBounded    |> ok
        v.IsSingleton       |> ok
        v.ToString() === sprintf "[%i]" i

        // ISegment
        let segment = v :> ISegment<int>
        segment.LowerEnd === i
        segment.UpperEnd === i

        // Constructor
        let other = new Range<int>(i, i)
        v === other

    [<Property>]
    let ``Range.Singleton()`` (i: int) =
        let v = Range.Singleton(i)
        let endpoints = OrderedPair.Create(i, i)

        v.Endpoints === endpoints
        v.Min === i
        v.Max === i
        v.IsLeftOpen        |> nok
        v.IsRightOpen       |> nok
        v.IsLeftBounded     |> ok
        v.IsRightBounded    |> ok
        v.IsSingleton       |> ok
        v.ToString() === sprintf "[%i]" i

        // ISegment
        let segment = v :> ISegment<int>
        segment.LowerEnd === i
        segment.UpperEnd === i

        // Constructor
        let other = new Range<int>(i, i)
        v === other

    [<Property>]
    let ``Range.Maximal()`` () =
        // We test this method w/ int16. For int, see Maximal32 below.
        let v = Range.Maximal<int16>()
        let endpoints = OrderedPair.Create(Int16.MinValue, Int16.MaxValue)

        v.Endpoints === endpoints
        v.Min === Int16.MinValue
        v.Max === Int16.MaxValue
        v.IsLeftOpen        |> nok
        v.IsRightOpen       |> nok
        v.IsLeftBounded     |> ok
        v.IsRightBounded    |> ok
        v.IsSingleton       |> nok
        v.ToString() === "[-32768..32767]"

        // ISegment
        let segment = v :> ISegment<int16>
        segment.LowerEnd === Int16.MinValue
        segment.UpperEnd === Int16.MaxValue

        // Constructor
        let other = new Range<int16>(Int16.MinValue, Int16.MaxValue)
        v === other

    [<Property>]
    let ``Range.StartingAt()`` (i: int) =
        let v = Range.StartingAt(i)
        let endpoints = OrderedPair.Create(i, Int32.MaxValue)

        v.Endpoints === endpoints
        v.Min === i
        v.Max === Int32.MaxValue
        v.IsLeftOpen        |> nok
        v.IsRightOpen       |> nok
        v.IsLeftBounded     |> ok
        v.IsRightBounded    |> ok
        v.IsSingleton       |> nok
        v.ToString() === sprintf "[%i..2147483647]" i

        // ISegment
        let segment = v :> ISegment<int>
        segment.LowerEnd === i
        segment.UpperEnd === Int32.MaxValue

        // Constructor
        let other = new Range<int>(i, Int32.MaxValue)
        v === other

    [<Property>]
    let ``Range.StartingAt(length)`` (i: int) =
        let len = 10
        let v = Range.StartingAt(i, len)
        let j = i + len - 1
        let endpoints = OrderedPair.Create(i, j)

        v.Endpoints === endpoints
        v.Min === i
        v.Max === j
        v.IsLeftOpen        |> nok
        v.IsRightOpen       |> nok
        v.IsLeftBounded     |> ok
        v.IsRightBounded    |> ok
        v.IsSingleton       |> nok
        v.ToString() === sprintf "[%i..%i]" i j

        // ISegment
        let segment = v :> ISegment<int>
        segment.LowerEnd === i
        segment.UpperEnd === j

        // Constructor
        let other = new Range<int>(i, j)
        v === other

    [<Property>]
    let ``Range.EndingAt()`` (i: int) =
        let v = Range.EndingAt(i)
        let endpoints = OrderedPair.Create(Int32.MinValue, i)

        v.Endpoints === endpoints
        v.Min === Int32.MinValue
        v.Max === i
        v.IsLeftOpen        |> nok
        v.IsRightOpen       |> nok
        v.IsLeftBounded     |> ok
        v.IsRightBounded    |> ok
        v.IsSingleton       |> nok
        v.ToString() === sprintf "[-2147483648..%i]" i

        // ISegment
        let segment = v :> ISegment<int>
        segment.LowerEnd === Int32.MinValue
        segment.UpperEnd === i

        // Constructor
        let other = new Range<int>(Int32.MinValue, i)
        v === other

    [<Property>]
    let ``Range.EndingAt(length)`` (i: int) =
        let len = 10
        let v = Range.EndingAt(i, len)
        let j = i - (len - 1)
        let endpoints = OrderedPair.Create(j, i)

        v.Endpoints === endpoints
        v.Min === j
        v.Max === i
        v.IsLeftOpen        |> nok
        v.IsRightOpen       |> nok
        v.IsLeftBounded     |> ok
        v.IsRightBounded    |> ok
        v.IsSingleton       |> nok
        v.ToString() === sprintf "[%i..%i]" j i

        // ISegment
        let segment = v :> ISegment<int>
        segment.LowerEnd === j
        segment.UpperEnd === i

        // Constructor
        let other = new Range<int>(j, i)
        v === other

    [<Property>]
    let ``Range.FromEndpoints()`` (x: OrderedPair<int>) =
        let v = Range.FromEndpoints(x)
        let isSingleton = x.LowerValue = x.UpperValue

        v.Endpoints === x
        v.Min === x.LowerValue
        v.Max === x.UpperValue
        v.IsLeftOpen        |> nok
        v.IsRightOpen       |> nok
        v.IsLeftBounded     |> ok
        v.IsRightBounded    |> ok
        v.IsSingleton === isSingleton
        v.ToString() ===
            if isSingleton then
                sprintf "[%i]" x.LowerValue
            else
                sprintf "[%i..%i]" x.LowerValue x.UpperValue

        // ISegment
        let segment = v :> ISegment<int>
        segment.LowerEnd === x.LowerValue
        segment.UpperEnd === x.UpperValue

        // Constructor
        let other = new Range<int>(x.LowerValue, x.UpperValue)
        v === other

module Adjustments =
    [<Fact>]
    let ``WithMin() throws when min > range.Max`` () =
        let v = Range.Create(1, 4)

        outOfRangeExn "min" (fun () -> v.WithMin(5))

    [<Theory>]
    [<InlineData 1>]
    [<InlineData 0>]
    [<InlineData -1>]
    [<InlineData -2>]
    let ``WithMin()`` (i: int) =
        let v = Range.Create(1, 4)
        let range = Range.Create(i, 4)

        v.WithMin(i) === range

    [<Fact>]
    let ``WithMax() throws when max < range.Min`` () =
        let v = Range.Create(1, 4)

        outOfRangeExn "max" (fun () -> v.WithMax(0))

    [<Theory>]
    [<InlineData 4>]
    [<InlineData 5>]
    [<InlineData 6>]
    [<InlineData 7>]
    let ``WithMax()`` (i: int) =
        let v = Range.Create(1, 4)
        let range = Range.Create(1, i)

        v.WithMax(i) === range

module SetOperations =
    //
    // Membership
    //

    [<Fact>]
    let ``Range.Maximal32.Contains(Int32.Min/MaxValue)`` () =
        let v = Range.Maximal32

        v.Contains(Int32.MinValue) |> ok
        v.Contains(Int32.MaxValue) |> ok

    [<Property>]
    let ``Range.Maximal32.Contains() always returns true`` (i: int) =
        let v = Range.Maximal32

        v.Contains(i)

    [<Property>]
    let ``Contains()`` (x: Range<int>) = x <> Range.Maximal32 &&&& (
        x.Contains(Int32.MinValue)  |> nok
        x.Contains(x.Min - 1)       |> nok
        x.Contains(x.Min)           |> ok
        x.Contains(x.Max)           |> ok
        x.Contains(x.Max + 1)       |> nok
        x.Contains(Int32.MaxValue)  |> nok
    )

    [<Fact>]
    let ``Contains() when not a singleton`` () =
        let v = Range.Create(1, 4)

        not (v.Contains(Int32.MinValue)) |> ok
        not (v.Contains(0))              |> ok
        v.Contains(1)                    |> ok
        v.Contains(2)                    |> ok
        v.Contains(3)                    |> ok
        v.Contains(4)                    |> ok
        not (v.Contains(5))              |> ok
        not (v.Contains(Int32.MaxValue)) |> ok

    //
    // Set inclusion
    //

    [<Fact>]
    let ``Set inclusion of a singleton with itself`` () =
        let v = Range.Singleton(1)

        v.IsSubsetOf(v)   |> ok
        v.IsSupersetOf(v) |> ok

        v.IsProperSubsetOf(v)   |> nok
        v.IsProperSupersetOf(v) |> nok

    [<Fact>]
    let ``Set inclusion of a singleton with another singleton`` () =
        let v = Range.Singleton(1)
        let w = Range.Singleton(2)

        v.IsSubsetOf(w)   |> nok
        v.IsSupersetOf(w) |> nok

        v.IsProperSubsetOf(w)   |> nok
        v.IsProperSupersetOf(w) |> nok

    [<Fact>]
    let ``Set inclusion of a singleton with an overlapping (proper) range`` () =
        let v = Range.Singleton(1)
        let w1 = Range.Create(1, 2) // v is the left endpoint
        let w2 = Range.Create(0, 1) // v is the right endpoint
        let w3 = Range.Create(0, 2) // v is an inner point

        v.IsSubsetOf(w1) |> ok
        v.IsSubsetOf(w2) |> ok
        v.IsSubsetOf(w3) |> ok

        v.IsSupersetOf(w1) |> nok
        v.IsSupersetOf(w2) |> nok
        v.IsSupersetOf(w3) |> nok

        v.IsProperSubsetOf(w1) |> ok
        v.IsProperSubsetOf(w2) |> ok
        v.IsProperSubsetOf(w3) |> ok

        v.IsProperSupersetOf(w1) |> nok
        v.IsProperSupersetOf(w2) |> nok
        v.IsProperSupersetOf(w3) |> nok

    [<Fact>]
    let ``Set inclusion of a singleton with a disjoint (proper) range`` () =
        let v = Range.Singleton(1)
        let w1 = Range.Create(2, 3)  // v is on the left side
        let w2 = Range.Create(-1, 0) // v is on the right side

        v.IsSubsetOf(w1) |> nok
        v.IsSubsetOf(w2) |> nok

        v.IsSupersetOf(w1) |> nok
        v.IsSupersetOf(w2) |> nok

        v.IsProperSubsetOf(w1) |> nok
        v.IsProperSubsetOf(w2) |> nok

        v.IsProperSupersetOf(w1) |> nok
        v.IsProperSupersetOf(w2) |> nok

    [<Property>]
    let ``Set inclusion of a range with itself`` (x: Range<int>) =
        x.IsSubsetOf(x)         |> ok
        x.IsSupersetOf(x)       |> ok
        x.IsProperSubsetOf(x)   |> nok
        x.IsProperSupersetOf(x) |> nok

    [<Fact>]
    let ``Set inclusion of a range with an overlapping (proper) range and no inclusion`` () =
        let v = Range.Create(1, 4)
        let w1 = Range.Create(4, 7) // v.Max = w.Min
        let w2 = Range.Create(0, 1) // v.Min = w.Max
        let w3 = Range.Create(0, 2) // non-degenerate intersection

        v.IsSubsetOf(w1) |> nok
        v.IsSubsetOf(w2) |> nok
        v.IsSubsetOf(w3) |> nok

        v.IsSupersetOf(w1) |> nok
        v.IsSupersetOf(w2) |> nok
        v.IsSupersetOf(w3) |> nok

        v.IsProperSubsetOf(w1) |> nok
        v.IsProperSubsetOf(w2) |> nok
        v.IsProperSubsetOf(w3) |> nok

        v.IsProperSupersetOf(w1) |> nok
        v.IsProperSupersetOf(w2) |> nok
        v.IsProperSupersetOf(w3) |> nok

    [<Fact>]
    let ``Set inclusion of a range with a proper superset`` () =
        let v = Range.Create(1, 4)
        let w1 = Range.Create(0, 4) // right endpoint in common
        let w2 = Range.Create(1, 5) // left endpoint in common
        let w3 = Range.Create(0, 5) // no endpoint in common

        v.IsSubsetOf(w1) |> ok
        v.IsSubsetOf(w2) |> ok
        v.IsSubsetOf(w3) |> ok

        v.IsSupersetOf(w1) |> nok
        v.IsSupersetOf(w2) |> nok
        v.IsSupersetOf(w3) |> nok

        v.IsProperSubsetOf(w1) |> ok
        v.IsProperSubsetOf(w2) |> ok
        v.IsProperSubsetOf(w3) |> ok

        v.IsProperSupersetOf(w1) |> nok
        v.IsProperSupersetOf(w2) |> nok
        v.IsProperSupersetOf(w3) |> nok

    [<Fact>]
    let ``Set inclusion of a range with a proper subset`` () =
        let v = Range.Create(1, 4)
        let w1 = Range.Create(1, 3) // left endpoint in common
        let w2 = Range.Create(2, 4) // right endpoint in common
        let w3 = Range.Create(2, 3) // no endpoint in common

        v.IsSubsetOf(w1) |> nok
        v.IsSubsetOf(w2) |> nok
        v.IsSubsetOf(w3) |> nok

        v.IsSupersetOf(w1) |> ok
        v.IsSupersetOf(w2) |> ok
        v.IsSupersetOf(w3) |> ok

        v.IsProperSubsetOf(w1) |> nok
        v.IsProperSubsetOf(w2) |> nok
        v.IsProperSubsetOf(w3) |> nok

        v.IsProperSupersetOf(w1) |> ok
        v.IsProperSupersetOf(w2) |> ok
        v.IsProperSupersetOf(w3) |> ok

    [<Fact>]
    let ``Set inclusion of a range with a disjoint range`` () =
        let v = Range.Create(1, 4)
        let w1 = Range.Create(5, 7)  // v is on the left side
        let w2 = Range.Create(-1, 0) // v is on the right side

        v.IsSubsetOf(w1) |> nok
        v.IsSubsetOf(w2) |> nok

        v.IsSupersetOf(w1) |> nok
        v.IsSupersetOf(w2) |> nok

        v.IsProperSubsetOf(w1) |> nok
        v.IsProperSubsetOf(w2) |> nok

        v.IsProperSupersetOf(w1) |> nok
        v.IsProperSupersetOf(w2) |> nok

    //
    // Set equality
    //

    [<Property>]
    let ``SetEquals() when both ranges are identical`` (x: Range<int>) =
        x.SetEquals(x)

    [<Property>]
    let ``SetEquals() when both ranges are distinct`` (x: Range<int>) (y: Range<int>) = x <> y &&&& (
        not (x.SetEquals(y))
    )

module Extensions =
    //
    // Range<int>
    //

    [<Fact>]
    let ``Range<int>.Count() and LongCount()`` () =
        let v = Range.Create(1, 4)

        v.Count() === 4
        v.LongCount() === 4

    [<Fact>]
    let ``Range<int>.Count() and LongCount() for a singleton`` () =
        let v = Range.Singleton(1)

        v.Count() === 1
        v.LongCount() === 1

    [<Fact>]
    let ``Range.Maximal32.Count() overflows`` () =
        let v = Range.Maximal32

        (fun () -> v.Count()) |> overflows

    [<Fact>]
    let ``Range.Maximal32.LongCount()`` () =
        let v = Range.Maximal32
        let count = (int64)Int32.MaxValue - (int64)Int32.MinValue + 1L

        v.LongCount() === count

    [<Fact>]
    let ``Range.Maximal32.ToEnumerable() overflows`` () =
        let v = Range.Maximal32

        (fun () -> v.ToEnumerable()) |> overflows

    [<Fact>]
    let ``Range<int>.ToEnumerable() singleton case`` () =
        let v = Range.Singleton(4)
        let q = Enumerable.Range(4, 1)

        v.ToEnumerable() === q

    [<Fact>]
    let ``Range<int>.ToEnumerable()`` () =
        let v = Range.Create(1, 4)
        let q = Enumerable.Range(1, 4)

        v.ToEnumerable() === q

    //
    // Range<DayNumber>
    //

    [<Fact>]
    let ``Range<DayNumber>.Count() and LongCount()`` () =
        let v = Range.Create(DayZero.OldStyle, DayZero.OldStyle + 3)

        v.Count() === 4
        v.LongCount() === 4

    [<Fact>]
    let ``Range<DayNumber>.Count() and LongCount() for a singleton`` () =
        let v = Range.Create(DayZero.OldStyle, DayZero.OldStyle)

        v.Count() === 1
        v.LongCount() === 1

    [<Fact>]
    let ``Range.Maximum<DayNumber>().Count() overflows`` () =
        let v = Range.Maximal<DayNumber>()

        (fun () -> v.Count()) |> overflows

    [<Fact>]
    let ``Range.Maximum<DayNumber>().LongCount()`` () =
        let v = Range.Maximal<DayNumber>()
        let count = (int64)DayNumber.MaxDaysSinceZero - (int64)DayNumber.MinDaysSinceZero + 1L

        v.LongCount() === count

    [<Fact>]
    let ``Range<DayNumber>.ToEnumerable() singleton case`` () =
        let v = Range.Singleton(DayZero.OldStyle)
        let q = seq { yield DayZero.OldStyle }

        v.ToEnumerable() === q

    [<Fact>]
    let ``Range<DayNumber>.ToEnumerable()`` () =
        let v = Range.Create(DayZero.OldStyle, DayZero.OldStyle + 3)
        let q = seq {
            yield DayZero.OldStyle
            yield DayZero.OldStyle + 1
            yield DayZero.OldStyle + 2
            yield DayZero.OldStyle + 3
        }

        v.ToEnumerable() === q

module Equality =
    open NonStructuralComparison

    /// Arbitrary for (x, y) where x and y are RangeSet<int> instances such that x <> y.
    let private xyArbitrary = Arb.fromGen <| gen {
        let! range =
            Gen.elements [ (0, 1); (1, 2); (2, 3) ]
            |> Gen.map (fun (i, j) -> new Range<int>(i, j))
        return new Range<int>(1, 1), range
    }

    // fsharplint:disable Hints
    [<Property>]
    let ``Equality when both operands are identical`` (x: Range<int>) =
        x = x
        .&. not (x <> x)
        .&. x.Equals(x)
        .&. x.Equals(x :> obj)

    [<Property>]
    let ``Equality when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
        not (x = y)
        .&. (x <> y)
        .&. not (x.Equals(y))
        .&. not (x.Equals(y :> obj))
        // Flipped
        .&. not (y = x)
        .&. (y <> x)
        .&. not (y.Equals(x))
        .&. not (y.Equals(x :> obj))
    // fsharplint:enable

    [<Property>]
    let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: Range<int>) =
        not (x.Equals(null))
        .&. not (x.Equals(new obj()))

    [<Property>]
    let ``GetHashCode() is invariant`` (x: Range<int>) =
        x.GetHashCode() = x.GetHashCode()
