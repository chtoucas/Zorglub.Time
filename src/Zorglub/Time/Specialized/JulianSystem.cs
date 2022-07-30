// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized
{
    using System.Diagnostics.Contracts;

    using Zorglub.Time;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Represents the Julian calendar system.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class JulianSystem : MinMaxYearCalendar<JulianDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JulianSystem"/> class.
        /// </summary>
        public JulianSystem() : this(new JulianSchema()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="JulianSystem"/> class.
        /// </summary>
        // Constructor for JulianDate.
        internal JulianSystem(JulianSchema schema)
            : base("Julian", MinMaxYearScope.WithMaximalRange(schema, DayZero.OldStyle)) { }

        /// <inheritdoc/>
        [Pure]
        protected sealed override JulianDate GetDateOn(int daysSinceEpoch) => new(daysSinceEpoch);
    }
}
