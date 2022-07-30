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
    /// Represents the Gregorian calendar system.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class GregorianSystem : MinMaxYearCalendar<GregorianDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianSystem"/> class.
        /// </summary>
        public GregorianSystem() : this(new GregorianSchema()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianSystem"/> class.
        /// </summary>
        // Constructor for GregorianDate.
        internal GregorianSystem(GregorianSchema schema)
            : base("Gregorian", MinMaxYearScope.WithMaximalRange(schema, DayZero.NewStyle)) { }

        /// <inheritdoc/>
        [Pure]
        protected sealed override GregorianDate GetDateOn(int daysSinceEpoch) => new(daysSinceEpoch);
    }
}
