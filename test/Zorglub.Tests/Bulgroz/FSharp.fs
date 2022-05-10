// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time

open System

// TODO(code): computational expression for Box<T>?

module EqualityModule =
    let inline eq<'a when 'a :> IEquatable<'a>> (x: 'a) (y: 'a) = x.Equals y

    let inline ( == ) x y = eq x y

    let inline ( != ) x y = not (eq x y)
