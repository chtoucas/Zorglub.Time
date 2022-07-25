// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Core.Validation;

    using static Zorglub.Time.Core.CalendricalConstants;

    /// <summary>
    /// Represents the standard short scope of the Gregorian schema.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal static class GregorianStandardScope
    {
        /// <summary>
        /// Represents the earliest supported year.
        /// <para>This field is a constant equal to -9998.</para>
        /// </summary>
        public const int MinYear = StandardScope.MinYear;

        /// <summary>
        /// Represents the latest supported year.
        /// <para>This field is a constant equal to 9999.</para>
        /// </summary>
        public const int MaxYear = StandardScope.MaxYear;

        /// <summary>
        /// Represents the range of supported years.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly Range<int> s_SupportedYears = Range.Create(MinYear, MaxYear);

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
        /// Gets the range of supported <see cref="DayNumber"/> values by the <i>Gregorian</i>
        /// calendar, the one using the default epoch i.e. <see cref="DayZero.NewStyle"/> .
        /// <para>This static propery is thread-safe.</para>
        /// </summary>
        public static Range<DayNumber> DefaultDomain { get; } =
            Range.Create(
                DayZero.NewStyle + s_MinDaysSinceEpoch,
                DayZero.NewStyle + s_MaxDaysSinceEpoch);

        /// <summary>
        /// Gets the validator for the range of supported days.
        /// <para>This static propery is thread-safe.</para>
        /// </summary>
        public static DaysValidator DaysValidator { get; } =
            new(Range.Create(s_MinDaysSinceEpoch, s_MaxDaysSinceEpoch));

        /// <summary>
        /// Gets the validator for the range of supported years.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static IRangeValidator<int> YearsValidator { get; } = new YearsValidator_();

        /// <summary>
        /// Validates the specified month.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        public static void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear) Throw.YearOutOfRange(year, paramName);
            if (month < 1 || month > Solar12.MonthsInYear) Throw.MonthOutOfRange(month, paramName);
        }

        /// <summary>
        /// Validates the specified date.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        public static void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear) Throw.YearOutOfRange(year, paramName);
            if (month < 1 || month > Solar12.MonthsInYear) Throw.MonthOutOfRange(month, paramName);
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
        public static void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear) Throw.YearOutOfRange(year, paramName);
            if (dayOfYear < 1
                || (dayOfYear > Solar.MinDaysInYear
                    && dayOfYear > GregorianFormulae.CountDaysInYear(year)))
            {
                Throw.DayOfYearOutOfRange(dayOfYear, paramName);
            }
        }

        private sealed class YearsValidator_ : IRangeValidator<int>
        {
            public Range<int> Range => s_SupportedYears;

            public void Validate(int year, string? paramName = null)
            {
                if (year < MinYear || year > MaxYear) Throw.YearOutOfRange(year, paramName);
            }

            public void CheckOverflow(int year)
            {
                if (year < MinYear || year > MaxYear) Throw.DateOverflow();
            }

            public void CheckUpperBound(int year)
            {
                if (year > MaxYear) Throw.DateOverflow();
            }

            public void CheckLowerBound(int year)
            {
                if (year < MinYear) Throw.DateOverflow();
            }
        }
    }
}
