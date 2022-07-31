// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized
{
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Hemerology;

    /// <summary>
    /// Provides common adjusters for <see cref="JulianDate"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class JulianAdjusters : IDateAdjusters<JulianDate>
    {
        /// <summary>
        /// Represents the Julian schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly JulianSchema _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="JulianAdjusters"/> class.
        /// </summary>
        public JulianAdjusters(JulianSchema schema)
        {
            Requires.NotNull(schema);

            _schema = schema;
        }

        /// <inheritdoc />
        [Pure]
        public JulianDate GetStartOfYear(JulianDate date)
        {
            int daysSinceEpoch = JulianFormulae.GetStartOfYear(date.Year);
            return new JulianDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public JulianDate GetEndOfYear(JulianDate date)
        {
            int daysSinceEpoch = _schema.GetEndOfYear(date.Year);
            return new JulianDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public JulianDate GetStartOfMonth(JulianDate date)
        {
            _schema.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out _);
            int daysSinceEpoch = _schema.GetStartOfMonth(y, m);
            return new JulianDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public JulianDate GetEndOfMonth(JulianDate date)
        {
            _schema.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out _);
            int daysSinceEpoch = _schema.GetEndOfMonth(y, m);
            return new JulianDate(daysSinceEpoch);
        }
    }
}
