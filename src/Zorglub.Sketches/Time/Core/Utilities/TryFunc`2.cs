// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities
{
    // On aurait pu écrire :
    //   public delegate bool TryFunc<in T, TResult>(T x, out TResult? y) where TResult : notnull;
    // mais ce n'est pas une bonne idée, un "delegate" définit une signature et
    // se doit rester le plus général possible.

    /// <summary>
    /// Encapsulates a method that has one parameter, one output parameter and returns a boolean
    /// value.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the method that this delegate encapsulates.
    /// </typeparam>
    /// <typeparam name="TResult">The type of the output parameter of the method that this delegate
    /// encapsulates.</typeparam>
    public delegate bool TryFunc<in T, TResult>(T x, out TResult y);
}
