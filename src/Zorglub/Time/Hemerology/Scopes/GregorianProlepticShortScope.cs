// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;

    using static Zorglub.Time.Core.CalendricalConstants;

    /// <summary>
    /// Represents the proleptic short scope of the Gregorian schema.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal static class GregorianProlepticShortScope
    {
        /// <summary>
        /// Represents the earliest supported year.
        /// <para>This field is a constant equal to -9998.</para>
        /// </summary>
        public const int MinYear = ProlepticShortScope.MinYear;

        /// <summary>
        /// Represents the latest supported year.
        /// <para>This field is a constant equal to 9999.</para>
        /// </summary>
        public const int MaxYear = ProlepticShortScope.MaxYear;

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
        /// values.</exception>
        public static void CheckOverflow(int daysSinceEpoch)
        {
            if (daysSinceEpoch < s_MinDaysSinceEpoch || daysSinceEpoch > s_MaxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }
        }

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
