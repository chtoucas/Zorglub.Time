// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time.Core;

    using static Zorglub.Time.Core.CalendricalConstants;

    /// <summary>
    /// Represents the short scope of a schema with profile <see cref="CalendricalProfile.Lunar"/>.
    /// <para>For such calendars, we can mostly avoid to compute the number of days in a year or in
    /// a month.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed class LunarStandardShortScope : StandardShortScope
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LunarStandardShortScope"/> class with the specified
        /// schema and epoch.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="schema"/> does not have the expected
        /// profile <see cref="CalendricalProfile.Lunar"/>.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> does not contain the interval [1..9999].</exception>
        public LunarStandardShortScope(CalendricalSchema schema, DayNumber epoch)
            : base(schema, epoch)
        {
            Debug.Assert(schema != null);

            if (schema.Profile != CalendricalProfile.Lunar)
            {
                Throw.Argument(nameof(schema));
            }
        }

        /// <inheritdoc />
        public override void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            if (month < 1 || month > Lunar.MonthsInYear)
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
            if (month < 1 || month > Lunar.MonthsInYear)
            {
                Throw.MonthOutOfRange(month, paramName);
            }
            if (day < 1
                || (day > Lunar.MinDaysInMonth
                    && day > Schema.CountDaysInMonth(year, month)))
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
                || (dayOfYear > Lunar.MinDaysInYear
                    && dayOfYear > Schema.CountDaysInYear(year)))
            {
                Throw.DayOfYearOutOfRange(dayOfYear, paramName);
            }
        }
    }
}
