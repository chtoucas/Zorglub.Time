// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Represents the default proleptic short scope of a schema.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed class DefaultProlepticShortScope : ProlepticShortScope
    {
        /// <summary>
        /// Represents the pre-validator.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalPreValidator _preValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultProlepticShortScope"/>
        /// class with the specified schema and epoch.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
        /// null.</exception>
        public DefaultProlepticShortScope(ICalendricalSchema schema, DayNumber epoch)
            : base(schema, epoch)
        {
            Debug.Assert(schema != null);

            // There are only two ways to create a DefaultProlepticShortScope:
            // - ProlepticShortScope.Create()
            // - ICalendricalScope.CreateProlepticScope()
            // In the first case, we end up here unless
            //   schema.Profile = SchemaProfile.Solar
            // Contrary to DefaultStandardShortScope, a validator can help here;
            // see the comments in DefaultStandardShortScope.
            _preValidator = schema.PreValidator;
        }

        /// <inheritdoc />
        public override void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            _preValidator.ValidateMonth(year, month, paramName);
        }

        /// <inheritdoc />
        public override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            _preValidator.ValidateMonthDay(year, month, day, paramName);
        }

        /// <inheritdoc />
        public override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            _preValidator.ValidateDayOfYear(year, dayOfYear, paramName);
        }
    }
}
