// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized
{
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Hemerology;

    /// <summary>
    /// Provides common adjusters for <see cref="CivilDate"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class CivilAdjusters : IDateAdjusters<CivilDate>
    {
        /// <summary>
        /// Represents the Civil schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly CivilSchema _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="CivilAdjusters"/> class.
        /// </summary>
        public CivilAdjusters(CivilSchema schema)
        {
            Requires.NotNull(schema);

            _schema = schema;
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate GetStartOfYear(CivilDate date)
        {
            int daysSinceZero = CivilFormulae.GetStartOfYear(date.Year);
            return new CivilDate(daysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate GetEndOfYear(CivilDate date)
        {
            int daysSinceZero = _schema.GetEndOfYear(date.Year);
            return new CivilDate(daysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate GetStartOfMonth(CivilDate date)
        {
            CivilFormulae.GetDateParts(date.DaysSinceZero, out int y, out int m, out _);
            int daysSinceZero = _schema.GetStartOfMonth(y, m);
            return new CivilDate(daysSinceZero);
        }

        /// <inheritdoc />
        [Pure]
        public CivilDate GetEndOfMonth(CivilDate date)
        {
            CivilFormulae.GetDateParts(date.DaysSinceZero, out int y, out int m, out _);
            int daysSinceZero = _schema.GetEndOfMonth(y, m);
            return new CivilDate(daysSinceZero);
        }
    }
}
