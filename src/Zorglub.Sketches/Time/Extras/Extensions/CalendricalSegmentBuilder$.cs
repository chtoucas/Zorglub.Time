// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extras.Extensions
{
    using Zorglub.Time.Core;

    // Fluent interface

    /// <summary>
    /// Provides extension methods for <see cref="CalendricalSegmentBuilder"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class CalendricalSegmentBuilderExtensions
    {
        /// <summary>
        /// Sets the start of the segment to the specified number of consecutive days from the epoch.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure]
        public static CalendricalSegmentBuilder WithMinDaysSinceEpoch(
            this CalendricalSegmentBuilder @this, int daysSinceEpoch)
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
        public static CalendricalSegmentBuilder WithMaxDaysSinceEpoch(
            this CalendricalSegmentBuilder @this, int daysSinceEpoch)
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
        public static CalendricalSegmentBuilder WithMinDateParts(
            this CalendricalSegmentBuilder @this, DateParts parts)
        {
            Requires.NotNull(@this);

            @this.SetMinDateParts(parts);
            return @this;
        }

        /// <summary>
        /// Sets the end of the segment to the specified date.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure]
        public static CalendricalSegmentBuilder WithMaxDateParts(
            this CalendricalSegmentBuilder @this, DateParts parts)
        {
            Requires.NotNull(@this);

            @this.SetMaxDateParts(parts);
            return @this;
        }

        /// <summary>
        /// Sets the start of the segment to the specified ordinal date.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure]
        public static CalendricalSegmentBuilder WithMinOrdinalParts(
            this CalendricalSegmentBuilder @this, OrdinalParts parts)
        {
            Requires.NotNull(@this);

            @this.SetMinOrdinalParts(parts);
            return @this;
        }

        /// <summary>
        /// Sets the end of the segment to the specified ordinal date.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure]
        public static CalendricalSegmentBuilder WithMaxOrdinalParts(
            this CalendricalSegmentBuilder @this, OrdinalParts parts)
        {
            Requires.NotNull(@this);

            @this.SetMaxOrdinalParts(parts);
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
        public static CalendricalSegmentBuilder WithMinYear(
            this CalendricalSegmentBuilder @this, int year)
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
        public static CalendricalSegmentBuilder WithMaxYear(
            this CalendricalSegmentBuilder @this, int year)
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
        public static CalendricalSegmentBuilder WithMinSupportedYear(
            this CalendricalSegmentBuilder @this, bool onOrAfterEpoch)
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
        public static CalendricalSegmentBuilder WithMaxSupportedYear(
            this CalendricalSegmentBuilder @this)
        {
            Requires.NotNull(@this);

            @this.UseMaxSupportedYear();
            return @this;
        }
    }
}
