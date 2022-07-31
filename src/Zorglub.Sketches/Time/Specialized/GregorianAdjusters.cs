// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized
{
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Hemerology;

    /// <summary>
    /// Provides common adjusters for <see cref="GregorianDate"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class GregorianAdjusters : IDateAdjusters<GregorianDate>
    {
        /// <summary>
        /// Represents the Gregorian schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly GregorianSchema _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianAdjusters"/> class.
        /// </summary>
        public GregorianAdjusters(GregorianSchema schema)
        {
            Requires.NotNull(schema);

            _schema = schema;
        }

        /// <inheritdoc />
        [Pure]
        public GregorianDate GetStartOfYear(GregorianDate date)
        {
            int daysSinceZero = GregorianFormulae.GetStartOfYear(date.Year);
            return new GregorianDate(daysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public GregorianDate GetEndOfYear(GregorianDate date)
        {
            int daysSinceZero = _schema.GetEndOfYear(date.Year);
            return new GregorianDate(daysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public GregorianDate GetStartOfMonth(GregorianDate date)
        {
            GregorianFormulae.GetDateParts(date.DaysSinceZero, out int y, out int m, out _);
            int daysSinceZero = _schema.GetStartOfMonth(y, m);
            return new GregorianDate(daysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public GregorianDate GetEndOfMonth(GregorianDate date)
        {
            GregorianFormulae.GetDateParts(date.DaysSinceZero, out int y, out int m, out _);
            int daysSinceZero = _schema.GetEndOfMonth(y, m);
            return new GregorianDate(daysSinceZero);
        }
    }
}
