// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Obsolete
{
    using System;

    using Zorglub.Time;
    using Zorglub.Time.Core;

    // !!! DO NOT USE !!!
    // It's not enough to validate y/m/d with a scope to be able create a Yemoda.
    // Idem w/ the other types.

    /// <summary>
    /// Provides extension methods for calendrical parts.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class FieldsExtensions
    {
        /// <summary>
        /// Validates the current instance with the specified scope then converts it to a
        /// <see cref="Yemoda"/> value.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="validator"/> is null.</exception>
        /// <exception cref="AoorException">The current instance is not valid according to the
        /// specified scope.</exception>
        [Pure]
        public static Yemoda ToYemoda(this DateFields @this, ICalendricalValidator validator)
        {
            Requires.NotNull(validator);

            var (y, m, d) = @this;
            validator.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }

        /// <summary>
        /// Validates the current instance with the specified scope then converts it to a
        /// <see cref="Yedoy"/> value.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="validator"/> is null.</exception>
        /// <exception cref="AoorException">The current instance is not valid according to the
        /// specified scope.</exception>
        [Pure]
        public static Yedoy ToYedoy(this OrdinalFields @this, ICalendricalValidator validator)
        {
            Requires.NotNull(validator);

            var (y, doy) = @this;
            validator.ValidateOrdinal(y, doy);
            return new Yedoy(y, doy);
        }

        /// <summary>
        /// Validates the current instance with the specified scope then converts it to a
        /// <see cref="Yemo"/> value.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="validator"/> is null.</exception>
        /// <exception cref="AoorException">The current instance is not valid according to the
        /// specified scope.</exception>
        [Pure]
        public static Yemo ToYemo(this MonthFields @this, ICalendricalValidator validator)
        {
            Requires.NotNull(validator);

            var (y, m) = @this;
            validator.ValidateYearMonth(y, m);
            return new Yemo(y, m);
        }
    }

    /// <summary>
    /// Provides extension methods for calendrical parts.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class PartsExtensions
    {
        /// <summary>
        /// Validates the current instance with the specified scope then converts it to a
        /// <see cref="Yemoda"/> value.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="validator"/> is null.</exception>
        /// <exception cref="AoorException">The current instance is not valid according to the
        /// specified scope.</exception>
        [Pure]
        public static Yemoda ToYemoda(this DateParts @this, ICalendricalValidator validator)
        {
            Requires.NotNull(validator);

            var (y, m, d) = @this;
            validator.ValidateYearMonthDay(y, m, d);
            return new Yemoda(y, m, d);
        }

        /// <summary>
        /// Validates the current instance with the specified scope then converts it to a
        /// <see cref="Yedoy"/> value.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="validator"/> is null.</exception>
        /// <exception cref="AoorException">The current instance is not valid according to the
        /// specified scope.</exception>
        [Pure]
        public static Yedoy ToYedoy(this OrdinalParts @this, ICalendricalValidator validator)
        {
            Requires.NotNull(validator);

            var (y, doy) = @this;
            validator.ValidateOrdinal(y, doy);
            return new Yedoy(y, doy);
        }

        /// <summary>
        /// Validates the current instance with the specified scope then converts it to a
        /// <see cref="Yemo"/> value.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="validator"/> is null.</exception>
        /// <exception cref="AoorException">The current instance is not valid according to the
        /// specified scope.</exception>
        [Pure]
        public static Yemo ToYemo(this MonthParts @this, ICalendricalValidator validator)
        {
            Requires.NotNull(validator);

            var (y, m) = @this;
            validator.ValidateYearMonth(y, m);
            return new Yemo(y, m);
        }
    }
}
