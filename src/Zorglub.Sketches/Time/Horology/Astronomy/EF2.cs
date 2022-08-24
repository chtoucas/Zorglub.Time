// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Astronomy
{
    internal static class EF
    {
        [Pure]
        public static InvalidOperationException ControlFlow =>
            new(
                "The flow of execution just reached a section of the code that should have been unreachable."
                + $"{Environment.NewLine}Most certainly signals a coding error. Please report.");

        [Pure]
        [DebuggerStepThrough]
        public static ArgumentException InvalidType(string paramName, Type expected, object obj) =>
            new(
                $"The parameter should be of type {expected} but it is of type {obj.GetType()}.",
                paramName);

    }
    internal static class EF2
    {
        // Échelles de temps.
        public static class Timescales
        {
            [Pure]
            [DebuggerStepThrough]
            public static ArgumentException UnexpectedTimescale(
                string paramName, Timescale expected, Timescale actual)
            {
                return new(
                    $"The object should be in the timescale \"{expected}\" but it is in \"{actual}\".",
                    paramName);
            }
        }
    }
}
