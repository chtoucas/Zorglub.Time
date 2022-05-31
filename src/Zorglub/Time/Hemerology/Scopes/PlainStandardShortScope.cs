// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Provides a default implementation for <see cref="ShortScope"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed class PlainStandardShortScope : StandardShortScope
    {
        /// <summary>
        /// Represents the minimum total number of days there is at least in a year.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _minDaysInYear;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainStandardShortScope"/> class with the
        /// specified schema and epoch.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> does not contain the interval [1..9999].</exception>
        public PlainStandardShortScope(CalendricalSchema schema, DayNumber epoch)
            : base(schema, epoch)
        {
            Debug.Assert(schema != null);

            // There are only two ways to create a DefaultShortScope:
            // - StandardShortScope.Create()
            // - ICalendricalScope.CreateStandardScope()
            // In the first case, we only end up here when
            // schema.Profile = SchemaProfile.Other. A direct consequence is
            // that using a validator will most certainly not help (unless
            // the schema provides a custom validator).
            _minDaysInYear = schema.MinDaysInYear;
        }

        /// <inheritdoc />
        public override void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            if (month < 1 || month > Schema.CountMonthsInYear(year))
            {
                Throw.MonthOutOfRange(month, paramName);
            }
        }

        /// <inheritdoc />
        public override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            if (month < 1 || month > Schema.CountMonthsInYear(year))
            {
                Throw.MonthOutOfRange(month, paramName);
            }
            if (day < 1 || day > Schema.CountDaysInMonth(year, month))
            {
                Throw.DayOutOfRange(day, paramName);
            }
        }

        /// <inheritdoc />
        public override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            if (dayOfYear < 1
                || (dayOfYear > _minDaysInYear && dayOfYear > Schema.CountDaysInYear(year)))
            {
                Throw.DayOfYearOutOfRange(dayOfYear, paramName);
            }
        }
    }
}
