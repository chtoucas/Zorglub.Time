// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

// REVIEW(code): can be removed as soon as CivilDay becomes part of the main assembly.
#pragma warning disable CA1812 // Avoid uninstantiated internal classes (Performance)

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;

    using static Zorglub.Time.Core.CalendricalConstants;

    /// <summary>
    /// Represents the standard short scope of the Gregorian schema.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed partial class GregorianStandardShortScope : StandardShortScope
    {
        /// <summary>
        /// Represents the minimum possible value for the number of consecutive days from the epoch.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly int s_MinDaysSinceEpoch = GregorianFormulae.GetStartOfYear(MinYear);

        /// <summary>
        /// Represents the maximum possible value for the number of consecutive days from the epoch.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly int s_MaxDaysSinceEpoch = GregorianFormulae.GetEndOfYear(MaxYear);

        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianStandardShortScope"/> class with
        /// the specified schema and epoch.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        public GregorianStandardShortScope(GregorianSchema schema, DayNumber epoch)
            : base(schema, epoch)
        {
            Debug.Assert(schema != null);
            // We check the profile because we use the constant attached to it.
            Debug.Assert(schema.Profile == CalendricalProfile.Solar12);
        }

        /// <summary>
        /// Gets the range of supported <see cref="DayNumber"/> values (<i>Gregorian</i> calendar).
        /// <para>This static propery is thread-safe.</para>
        /// </summary>
        // WARNING: only for the Gregorian calendar, epoch = DayZero.NewStyle
        public static Range<DayNumber> DefaultDomain { get; } =
            Range.CreateLeniently(
                DayZero.NewStyle + s_MinDaysSinceEpoch,
                DayZero.NewStyle + s_MaxDaysSinceEpoch);

        /// <summary>
        /// Checks that the specified <paramref name="daysSinceEpoch"/> does not overflow the range
        /// of supported values.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// dates.</exception>
        public static void CheckOverflow(int daysSinceEpoch)
        {
            if (daysSinceEpoch < s_MinDaysSinceEpoch || daysSinceEpoch > s_MaxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }
        }
    }

    internal partial class GregorianStandardShortScope // ICalendricalScope
    {
        /// <inheritdoc />
        public override void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            if (month < 1 || month > Solar12.MonthsInYear)
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
            if (month < 1 || month > Solar12.MonthsInYear)
            {
                Throw.MonthOutOfRange(month, paramName);
            }
            if (day < 1
                || (day > Solar.MinDaysInMonth
                    && day > GregorianFormulae.CountDaysInMonth(year, month)))
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
                || (dayOfYear > Solar.MinDaysInYear
                    && dayOfYear > GregorianFormulae.CountDaysInYear(year)))
            {
                Throw.DayOfYearOutOfRange(dayOfYear, paramName);
            }
        }
    }

    internal partial class GregorianStandardShortScope // Static versions
    {
        /// <summary>
        /// Validates the specified year.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        public static void ValidateYearImpl(int year, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
        }

        /// <summary>
        /// Validates the specified month.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        public static void ValidateYearMonthImpl(int year, int month, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            if (month < 1 || month > Solar12.MonthsInYear)
            {
                Throw.MonthOutOfRange(month, paramName);
            }
        }

        /// <summary>
        /// Validates the specified date.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        public static void ValidateYearMonthDayImpl(int year, int month, int day, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            if (month < 1 || month > Solar12.MonthsInYear)
            {
                Throw.MonthOutOfRange(month, paramName);
            }
            if (day < 1
                || (day > Solar.MinDaysInMonth
                    && day > GregorianFormulae.CountDaysInMonth(year, month)))
            {
                Throw.DayOutOfRange(day, paramName);
            }
        }

        /// <summary>
        /// Validates the specified ordinal date.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        public static void ValidateOrdinalImpl(int year, int dayOfYear, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            if (dayOfYear < 1
                || (dayOfYear > Solar.MinDaysInYear
                    && dayOfYear > GregorianFormulae.CountDaysInYear(year)))
            {
                Throw.DayOfYearOutOfRange(dayOfYear, paramName);
            }
        }
    }
}
