// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extensions
{
    using Zorglub.Time.Core;

    // Fluent interface

    /// <summary>
    /// Provides extension methods for <see cref="CalendricalSectionBuilder"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class CalendricalSectionBuilderExtensions
    {
        /// <summary>
        /// Sets the start of the segment to the specified number of consecutive days from the epoch.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure]
        public static CalendricalSectionBuilder WithMinDaysSinceEpoch(
            this CalendricalSectionBuilder @this, int daysSinceEpoch)
        {
            Requires.NotNull(@this);

            @this.SetMinDaysSinceEpoch(daysSinceEpoch);
            return @this;
        }

        /// <summary>
        /// Sets the end of the segment to the specified number of consecutive days from the epoch.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure]
        public static CalendricalSectionBuilder WithMaxDaysSinceEpoch(
            this CalendricalSectionBuilder @this, int daysSinceEpoch)
        {
            Requires.NotNull(@this);

            @this.SetMaxDaysSinceEpoch(daysSinceEpoch);
            return @this;
        }

        /// <summary>
        /// Sets the start of the segment to the specified date.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure]
        public static CalendricalSectionBuilder WithMinDate(
            this CalendricalSectionBuilder @this, DateParts parts)
        {
            Requires.NotNull(@this);

            @this.SetMinDate(parts);
            return @this;
        }

        /// <summary>
        /// Sets the end of the segment to the specified date.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure]
        public static CalendricalSectionBuilder WithMaxDate(
            this CalendricalSectionBuilder @this, DateParts parts)
        {
            Requires.NotNull(@this);

            @this.SetMaxDate(parts);
            return @this;
        }

        /// <summary>
        /// Sets the start of the segment to the specified ordinal date.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure]
        public static CalendricalSectionBuilder WithMinOrdinal(
            this CalendricalSectionBuilder @this, OrdinalParts parts)
        {
            Requires.NotNull(@this);

            @this.SetMinOrdinal(parts);
            return @this;
        }

        /// <summary>
        /// Sets the end of the segment to the specified ordinal date.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure]
        public static CalendricalSectionBuilder WithMaxOrdinal(
            this CalendricalSectionBuilder @this, OrdinalParts parts)
        {
            Requires.NotNull(@this);

            @this.SetMaxOrdinal(parts);
            return @this;
        }

        /// <summary>
        /// Sets the start of the segment to the start of the specified year.
        /// <para>If <paramref name="year"/> is null, this method fallbacks to the earliest
        /// supported year.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure]
        public static CalendricalSectionBuilder WithMinYear(
            this CalendricalSectionBuilder @this, int year)
        {
            Requires.NotNull(@this);

            @this.SetMinYear(year);
            return @this;
        }

        /// <summary>
        /// Sets the end of the segment to the end of the specified year.
        /// <para>If <paramref name="year"/> is null, this method fallbacks to the latest supported
        /// year.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure]
        public static CalendricalSectionBuilder WithMaxYear(
            this CalendricalSectionBuilder @this, int year)
        {
            Requires.NotNull(@this);

            @this.SetMaxYear(year);
            return @this;
        }

        /// <summary>
        /// Sets the start of the segment to the start of the earliest supported year.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by the schema
        /// does not contain the year 1.</exception>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure]
        public static CalendricalSectionBuilder WithMinSupportedYear(
            this CalendricalSectionBuilder @this, bool onOrAfterEpoch)
        {
            Requires.NotNull(@this);

            @this.UseMinSupportedYear(onOrAfterEpoch);
            return @this;
        }

        /// <summary>
        /// Sets the end of the segment to the end of the latest supported year.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure]
        public static CalendricalSectionBuilder WithMaxSupportedYear(
            this CalendricalSectionBuilder @this)
        {
            Requires.NotNull(@this);

            @this.UseMaxSupportedYear();
            return @this;
        }
    }
}
