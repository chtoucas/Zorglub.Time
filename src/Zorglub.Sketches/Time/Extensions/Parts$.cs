// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extensions
{
    using System;

    using Zorglub.Time.Core;

    // !!! DO NOT USE !!!
    // It's not enough to validate y/m/d with a scope to be able create a Yemoda.
    // Idem w/ the other types.

    /// <summary>
    /// Provides extension methods for calendrical parts.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static partial class PartsExtensions { }

    public static partial class PartsExtensions // Parts
    {
        /// <summary>
        /// Validates the current instance with the specified scope then converts it to a
        /// <see cref="Yemoda"/> value.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="AoorException">The current instance is not valid according to the
        /// specified scope.</exception>
        [Pure]
        public static Yemoda ToYemoda(this DateParts @this, ICalendricalValidator scope)
        {
            Requires.NotNull(scope);

            var (y, m, d) = @this;
            scope.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }

        /// <summary>
        /// Validates the current instance with the specified scope then converts it to a
        /// <see cref="Yedoy"/> value.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="AoorException">The current instance is not valid according to the
        /// specified scope.</exception>
        [Pure]
        public static Yedoy ToYedoy(this OrdinalParts @this, ICalendricalValidator scope)
        {
            Requires.NotNull(scope);

            var (y, doy) = @this;
            scope.ValidateOrdinal(y, doy);
            return new Yedoy(y, doy);
        }

        /// <summary>
        /// Validates the current instance with the specified scope then converts it to a
        /// <see cref="Yemo"/> value.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="AoorException">The current instance is not valid according to the
        /// specified scope.</exception>
        [Pure]
        public static Yemo ToYemo(this MonthParts @this, ICalendricalValidator scope)
        {
            Requires.NotNull(scope);

            var (y, m) = @this;
            scope.ValidateYearMonth(y, m);
            return new Yemo(y, m);
        }
    }

    public static partial class PartsExtensions // Fields
    {
        /// <summary>
        /// Validates the current instance with the specified scope then converts it to a
        /// <see cref="Yemoda"/> value.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="AoorException">The current instance is not valid according to the
        /// specified scope.</exception>
        [Pure]
        public static Yemoda ToYemoda(this DateFields @this, ICalendricalValidator scope)
        {
            Requires.NotNull(scope);

            var (y, m, d) = @this;
            scope.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }

        /// <summary>
        /// Validates the current instance with the specified scope then converts it to a
        /// <see cref="Yedoy"/> value.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="AoorException">The current instance is not valid according to the
        /// specified scope.</exception>
        [Pure]
        public static Yedoy ToYedoy(this OrdinalFields @this, ICalendricalValidator scope)
        {
            Requires.NotNull(scope);

            var (y, doy) = @this;
            scope.ValidateOrdinal(y, doy);
            return new Yedoy(y, doy);
        }

        /// <summary>
        /// Validates the current instance with the specified scope then converts it to a
        /// <see cref="Yemo"/> value.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="AoorException">The current instance is not valid according to the
        /// specified scope.</exception>
        [Pure]
        public static Yemo ToYemo(this MonthFields @this, ICalendricalValidator scope)
        {
            Requires.NotNull(scope);

            var (y, m) = @this;
            scope.ValidateYearMonth(y, m);
            return new Yemo(y, m);
        }
    }
}
