// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.PartsTests

open Zorglub.Testing

open Zorglub.Time

open Xunit
open FsCheck
open FsCheck.Xunit

module DateParts =
    module Prelude =
        [<Fact>]
        let ``Default value`` () =
            let parts = Unchecked.defaultof<DateParts>
            let y, m, d = parts.Deconstruct()

            (y, m, d) === (0, 0, 0)

        [<Property>]
        let Constructor y m d =
            let parts = new DateParts(y, m, d)
            let a = parts.Year
            let b = parts.Month
            let c = parts.Day

            (a, b, c) = (y, m, d)

        [<Property>]
        let Deconstructor y m d =
            let parts = new DateParts(y, m, d)
            let a, b, c = parts.Deconstruct()

            (a, b, c) = (y, m, d)

        [<Theory>]
        [<InlineData(0, 1, 1, "01/01/0000")>] // default
        [<InlineData(7, 5, 3, "03/05/0007")>]
        [<InlineData(-7, 5, 3, "03/05/-0007")>]
        [<InlineData(2019, 13, 47, "47/13/2019")>]
        [<InlineData(-2019, 13, 47, "47/13/-2019")>]
        [<InlineData(10_000, 20_000, 30_000, "30000/20000/10000")>]
        [<InlineData(-10_000, 20_000, 30_000, "30000/20000/-10000")>]
        let ``ToString()`` y m d str =
            let parts = new DateParts(y, m, d)

            parts.ToString() === str

    module Equality =
        open NonStructuralComparison

        /// Arbitrary for (x, y) where x and y are DateParts instances such that x <> y.
        let private xyArbitrary = Arb.fromGen <| gen {
            let! parts =
                Gen.elements [
                    // One different element.
                    (2, 1, 1); (1, 2, 1); (1, 1, 2);
                    // Two different elements.
                    (2, 2, 1); (1, 2, 2); (2, 1, 2);
                    // Three different elements.
                    (2, 2, 2) ]
                |> Gen.map (fun (y, m, d) -> new DateParts(y, m, d))
            return new DateParts(1, 1, 1), parts
        }

        // fsharplint:disable Hints
        [<Property>]
        let ``Equality when both operands are identical`` (x: DateParts) =
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
        let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: DateParts) =
            not (x.Equals(null))
            .&. not (x.Equals(new obj()))

        [<Property>]
        let ``GetHashCode() is invariant`` (x: DateParts) =
            x.GetHashCode() = x.GetHashCode()

module MonthParts =
    module Prelude =
        [<Fact>]
        let ``Default value`` () =
            let parts = Unchecked.defaultof<MonthParts>
            let y, m = parts.Deconstruct()

            (y, m) === (0, 0)

        [<Property>]
        let Constructor y m =
            let parts = new MonthParts(y, m)
            let a = parts.Year
            let b = parts.Month

            (a, b) = (y, m)

        [<Property>]
        let Deconstructor y m =
            let parts = new MonthParts(y, m)
            let a, b = parts.Deconstruct()

            (a, b) = (y, m)

        [<Theory>]
        [<InlineData(0, 1, "01/0000")>] // default
        [<InlineData(7, 5, "05/0007")>]
        [<InlineData(-7, 5, "05/-0007")>]
        [<InlineData(2019, 13, "13/2019")>]
        [<InlineData(-2019, 13, "13/-2019")>]
        [<InlineData(10_000, 20_000, "20000/10000")>]
        [<InlineData(-10_000, 20_000, "20000/-10000")>]
        let ``ToString()`` y m str =
            let parts = new MonthParts(y, m)

            parts.ToString() === str

    module Equality =
        open NonStructuralComparison

        /// Arbitrary for (x, y) where x and y are MonthParts instances such that x <> y.
        let private xyArbitrary = Arb.fromGen <| gen {
            let! parts =
                Gen.elements [ (2, 1); (1, 2); (2, 2) ]
                |> Gen.map (fun (y, m) -> new MonthParts(y, m))
            return new MonthParts(1, 1), parts
        }

        // fsharplint:disable Hints
        [<Property>]
        let ``Equality when both operands are identical`` (x: MonthParts) =
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
        let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: MonthParts) =
            not (x.Equals(null))
            .&. not (x.Equals(new obj()))

        [<Property>]
        let ``GetHashCode() is invariant`` (x: MonthParts) =
            x.GetHashCode() = x.GetHashCode()

module OrdinalParts =
    module Prelude =
        [<Fact>]
        let ``Default value`` () =
            let parts = Unchecked.defaultof<OrdinalParts>
            let y, doy = parts.Deconstruct()

            (y, doy) === (0, 0)

        [<Property>]
        let Constructor y doy =
            let parts = new OrdinalParts(y, doy)
            let a = parts.Year
            let b = parts.DayOfYear

            (a, b) = (y, doy)

        [<Property>]
        let Deconstructor y doy =
            let parts = new OrdinalParts(y, doy)
            let a, b = parts.Deconstruct()

            (a, b) = (y, doy)

        [<Theory>]
        [<InlineData(0, 1, "001/0000")>] // default
        [<InlineData(7, 5, "005/0007")>]
        [<InlineData(-7, 5, "005/-0007")>]
        [<InlineData(2019, 133, "133/2019")>]
        [<InlineData(-2019, 133, "133/-2019")>]
        [<InlineData(10_000, 20_000, "20000/10000")>]
        [<InlineData(-10_000, 20_000, "20000/-10000")>]
        let ``ToString()`` y doy str =
            let parts = new OrdinalParts(y, doy)

            parts.ToString() === str

    module Equality =
        open NonStructuralComparison

        /// Arbitrary for (x, y) where x and y are OrdinalParts instances such that x <> y.
        let private xyArbitrary = Arb.fromGen <| gen {
            let! parts =
                Gen.elements [ (2, 1); (1, 2); (2, 2) ]
                |> Gen.map (fun (y, doy) -> new OrdinalParts(y, doy))
            return new OrdinalParts(1, 1), parts
        }

        // fsharplint:disable Hints
        [<Property>]
        let ``Equality when both operands are identical`` (x: OrdinalParts) =
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
        let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: OrdinalParts) =
            not (x.Equals(null))
            .&. not (x.Equals(new obj()))

        [<Property>]
        let ``GetHashCode() is invariant`` (x: OrdinalParts) =
            x.GetHashCode() = x.GetHashCode()
