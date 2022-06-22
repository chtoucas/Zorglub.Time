// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Arithmetic;
    using Zorglub.Time.Core.Schemas;

    // TODO(api): merge with ICalendricalArithmetic?

    /// <summary>
    /// Defines the arithmetical operations on calendrical types.
    /// </summary>
    public interface ICalendricalArithmeticPlus : ICalendricalArithmetic
    {
        /// <summary>
        /// Adds a number of years to the year field of the specified date.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        /// <remarks>
        /// <para>When the result is not a valid date (roundoff > 0), returns the last day of the
        /// month or the last day of the year.</para>
        /// </remarks>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        [Pure] Yemoda AddYears(Yemoda ymd, int years, out int roundoff);

        /// <summary>
        /// Adds a number of months to the month field of the specified date.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        /// <remarks>
        /// <para>When the result is not a valid date (roundoff > 0), returns the last day of the
        /// month.</para>
        /// </remarks>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        [Pure] Yemoda AddMonths(Yemoda ymd, int months, out int roundoff);

        /// <summary>
        /// Adds a number of years to the year field of the specified ordinal date.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        /// <remarks>
        /// <para>When the result is not a valid date (roundoff > 0), returns the last day of the
        /// year.</para>
        /// </remarks>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        [Pure] Yedoy AddYears(Yedoy ydoy, int years, out int roundoff);

        /// <summary>
        /// Adds a number of years to the year field of the specified month.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        /// <remarks>
        /// <para>When the result is not a valid date (roundoff > 0), returns the last month of the
        /// year.</para>
        /// </remarks>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        [Pure] Yemo AddYears(Yemo ym, int years, out int roundoff);

        /// <summary>
        /// Creates the default <see cref="ICalendricalArithmeticPlus"/> for the specified schema.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        [Pure]
        public static ICalendricalArithmeticPlus CreateDefault(CalendricalSchema schema) =>
            SystemArithmetic.Create(schema);
    }
}