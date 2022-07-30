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
    /// Represents the Civil calendar system.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class CivilSystem : MinMaxYearCalendar<CivilDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CivilSystem"/> class.
        /// </summary>
        public CivilSystem() : this(new CivilSchema()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CivilSystem"/> class.
        /// </summary>
        // Constructor for CivilDate.
        internal CivilSystem(CivilSchema schema)
            : base("Gregorian", new StandardScope(schema, DayZero.NewStyle)) { }

        /// <inheritdoc/>
        [Pure]
        protected sealed override CivilDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }
}
