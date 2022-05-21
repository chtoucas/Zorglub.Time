// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

[<AutoOpen>]
module Zorglub.Testing.FsCheckModule

open System
open System.Runtime.CompilerServices

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Utilities

open FsCheck
open FsCheck.Xunit

// Custom operators.
// Adapted from https://github.com/fscheck/FsCheck/blob/master/src/FsCheck/Prop.fs

/// Conditional property combinator.
/// Resulting property holds if the property after &&& holds whenever the condition does.
/// Replacement for the FsCheck operator ==>.
let ( &&&& ) condition (assertion: 'Testable) = Prop.filter condition assertion

/// Quantified property combinator. Provide a custom test data generator to a property.
/// Moyen mnémotechnique : "four @all".
let ( @@@@ ) (arb: Arbitrary<'Value>) (body: 'Value -> 'Testable) = Prop.forAll arb body

/// Represents a pair of distinct values.
[<Struct; IsReadOnly>]
type Pair<'T when 'T: struct and 'T :> IComparable<'T>> = { Min: 'T; Max: 'T; Delta: 'T }

/// Generators for int.
module IntGenerators =
    /// Generator for integers > 0.
    let greaterThanZero = Arb.generate<int> |> Gen.filter (fun i -> i > 0)

    /// Generator for integers between -1_000_000 and 1_000_000, inclusive.
    let private ``[-10^6..10^6]`` = Gen.choose (-1_000_000, 1_000_000)

    /// Generator for integers between 1 and 1_000_000, inclusive.
    let private ``[1..10^6]`` = Gen.choose (1, 1_000_000)

    /// Generator for (i, j, j - i) where i and j are integers such that i < j.
    let orderedPair =
        gen {
            let! i = ``[-10^6..10^6]``
            let! n = ``[1..10^6]``
            return i, i + n, n
        }

/// Arbitraries for int.
module IntArbitraries =
    /// Arbitrary for an int <> 0.
    /// See also NonZeroInt from FsCheck.
    let nonZero =
        Arb.Default.Int32()
        |> Arb.filter ((<>) 0)

    /// Arbitrary for an int < 0.
    /// See also NegativeInt from FsCheck.
    let lessThanZero =
        Arb.Default.Int32()
        |> Arb.mapFilter (fun i -> -abs i) (fun i -> i < 0)

    /// Arbitrary for an int <= 0.
    let lessThanOrEqualToZero =
        Arb.Default.Int32()
        |> Arb.mapFilter (fun i -> -abs i) (fun i -> i <= 0)

    /// Arbitrary for an int > 0.
    /// See also PositiveInt from FsCheck.
    let greaterThanZero =
        Arb.Default.Int32()
        |> Arb.mapFilter abs (fun i -> i > 0)

    /// Arbitrary for an int >= 0.
    /// See also NonNegativeInt from FsCheck.
    let greaterThanOrEqualToZero =
        Arb.Default.Int32()
        |> Arb.mapFilter abs (fun i -> i >= 0)

/// Domain-specific arbitraries.
module DomainArbitraries =
    /// Arbitrary for the "daysSinceZero" of a DayNumber.
    let daysSinceZero =
        Arb.Default.Int32()
        |> Arb.filter (fun i -> DayNumber.MinDaysSinceZero <= i && i <= DayNumber.MaxDaysSinceZero)

    /// Arbitrary for the algebraic value of an Ord.
    let algebraicOrd =
        Arb.Default.Int32()
        // Formally, we we should also filter i with i <= Ord.MaxAlgebraicValue,
        // but it's useless since Ord.MaxAlgebraicValue is equal to Int32.MaxValue.
        |> Arb.filter (fun i -> i >= Ord.MinAlgebraicValue)

    /// Arbitrary for the "daysSinceZero" of a DayNumber64.
    let daysSinceZero64 =
        Arb.Default.Int64()
        |> Arb.filter (fun i -> DayNumber64.MinDaysSinceZero <= i && i <= DayNumber64.MaxDaysSinceZero)

    /// Arbitrary for the algebraic value of an Ord.
    let algebraicOrd64 =
        Arb.Default.Int64()
        |> Arb.filter (fun i -> i >= Ord64.MinAlgebraicValue)

/// Global arbitraries.
[<Sealed>]
type GlobalArbitraries =
    //
    // Zorglub.Testing
    //

    /// Gets an arbitrary for Pair<int>.
    static member GetPairOfIntArbitrary() =
        IntGenerators.orderedPair
        |> Gen.map (fun (i, j, n) -> { Pair.Min = i; Max = j; Delta = n })
        |> Arb.fromGen

    //
    // Zorglub.Time
    //

    /// Gets an arbitrary for DayNumber.
    static member GetDayNumberArbitrary() =
        DomainArbitraries.daysSinceZero
        |> Arb.convert (fun i -> DayNumber.Zero + i) (fun x -> x - DayNumber.Zero)

    /// Gets an arbitrary for DayNumber64.
    static member GetDayNumber64Arbitrary() =
        DomainArbitraries.daysSinceZero64
        |> Arb.convert (fun i -> DayNumber64.Zero + i) (fun x -> x - DayNumber64.Zero)

    /// Gets an arbitrary for Ord.
    static member GetOrdArbitrary() =
        DomainArbitraries.algebraicOrd
        |> Arb.convert Ord.FromInt32 int

    /// Gets an arbitrary for Ord64.
    static member GetOrd64Arbitrary() =
        DomainArbitraries.algebraicOrd64
        |> Arb.convert Ord64.FromInt64 int64

    /// Gets an arbitrary for DateParts.
    static member GetDatePartsArbitrary() =
        Arb.fromGen <| gen {
            let! y = Arb.generate<int>
            let! m = IntGenerators.greaterThanZero
            let! d = IntGenerators.greaterThanZero
            return new DateParts(y, m, d)
        }

    /// Gets an arbitrary for MonthParts.
    static member GetMonthPartsArbitrary() =
        Arb.fromGen <| gen {
            let! y = Arb.generate<int>
            let! m = IntGenerators.greaterThanZero
            return new MonthParts(y, m)
        }

    /// Gets an arbitrary for OrdinalParts.
    static member GetOrdinalPartsArbitrary() =
        Arb.fromGen <| gen {
            let! y = Arb.generate<int>
            let! doy = IntGenerators.greaterThanZero
            return new OrdinalParts(y, doy)
        }

    //
    // Zorglub.Time.Core
    //
    // For Yemoda(x) and Yedoy(x), we use de-serialization, which is a trivial
    // operation. Notice also that serialization is a one-to-one mapping between
    // Int32 and the type.
    // For Yemo(x), we have to do things manually as de-serialization is not
    // always valid.

    /// Gets an arbitrary for Yemoda.
    static member GetYemodaArbitrary() =
        Arb.generate<int>
        |> Gen.map Yemoda.FromBinary
        |> Arb.fromGen

    /// Gets an arbitrary for Yemodax.
    static member GetYemodaxArbitrary() =
        Arb.generate<int>
        |> Gen.map Yemodax.FromBinary
        |> Arb.fromGen

    /// Gets an arbitrary for Yemo.
    static member GetYemoArbitrary() =
        Arb.fromGen <| gen {
            let! y = Gen.choose (Yemo.MinYear, Yemo.MaxYear)
            let! m = Gen.choose (Yemo.MinMonth, Yemo.MaxMonth)
            return Yemo.Create(y, m)
        }

    /// Gets an arbitrary for Yemox.
    static member GetYemoxArbitrary() =
        Arb.fromGen <| gen {
            let! y = Gen.choose (Yemox.MinYear, Yemox.MaxYear)
            let! m = Gen.choose (Yemox.MinMonth, Yemox.MaxMonth)
            let! x = Gen.choose (Yemox.MinExtra, Yemox.MaxExtra)
            return Yemox.Create(y, m, x)
        }

    /// Gets an arbitrary for Yedoy.
    static member GetYedoyArbitrary() =
        Arb.generate<int>
        |> Gen.map Yedoy.FromBinary
        |> Arb.fromGen

    /// Gets an arbitrary for Yedoyx.
    static member GetYedoyxArbitrary() =
        Arb.generate<int>
        |> Gen.map Yedoyx.FromBinary
        |> Arb.fromGen

    //
    // Zorglub.Time.Core.Intervals
    //

    /// Gets an arbitrary for Range<int>.
    static member GetRangeArbitrary() =
        Arb.fromGen <| gen {
            let! i = Arb.generate<int>
            let! j = Arb.generate<int>
            return if i < j then new Range<int>(i, j) else new Range<int>(j, i)
        }

    /// Arbitrary for RangeSet<int>.
    static member GetRangeSetArbitrary() =
        Arb.fromGen <| gen {
            let! i = Arb.generate<int>
            let! j = Arb.generate<int>
            return if i < j then new RangeSet<int>(i, j) else new RangeSet<int>(j, i)
        }

    /// Gets an arbitrary for LowerRay<int>.
    static member GetLowerRayArbitrary() =
        Arb.generate<int>
        |> Gen.map (fun i -> new LowerRay<int>(i))
        |> Arb.fromGen

    /// Gets an arbitrary for UpperRay<int>.
    static member GetUpperRayAbitrary() =
        Arb.generate<int>
        |> Gen.map (fun i -> new UpperRay<int>(i))
        |> Arb.fromGen

    //
    // Zorglub.Time.Core.Utilities
    //

    /// Gets an arbitrary for OrderedPair<int>.
    static member GetOrderedPairArbitrary() =
        Arb.fromGen <| gen {
            let! i = Arb.generate<int>
            let! j = Arb.generate<int>
            return new OrderedPair<int>(i, j)
        }

[<assembly: Properties( Arbitrary = [| typeof<GlobalArbitraries> |] )>] do()
