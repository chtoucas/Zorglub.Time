// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Arithmetic;
    using Zorglub.Time.Core.Schemas;

    // TODO(api): add non-standard ops.
    // Later on, merge with ICalendricalArithmetic?

    /// <summary>
    /// Defines the arithmetical operations on calendrical types.
    /// </summary>
    public interface ICalendricalArithmeticPlus : ICalendricalArithmetic
    {
        /// <summary>
        /// Creates the default <see cref="ICalendricalArithmeticPlus"/> for the specified schema.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        [Pure]
        public static ICalendricalArithmeticPlus CreateDefault(CalendricalSchema schema) =>
            SystemArithmetic.Create(schema);
    }
}