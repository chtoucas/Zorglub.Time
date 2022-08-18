// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Validation;

    /// <summary>
    /// Represents a scope for a calendar supporting <i>all</i> dates within the range [1..9999] of
    /// years.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    /// <remarks>
    /// <para>This is the scope used by <see cref="Simple.SimpleCalendar"/>, except in the Gregorian and
    /// Julian cases.</para>
    /// </remarks>
    public sealed class StandardScope : CalendarScope
    {
        /// <summary>
        /// Represents the earliest supported year.
        /// <para>This field is a constant equal to 1.</para>
        /// </summary>
        public const int MinYear = 1;

        /// <summary>
        /// Represents the latest supported year.
        /// <para>This field is a constant equal to 9999.</para>
        /// </summary>
        public const int MaxYear = 9999;

        /// <summary>
        /// Represents the range of supported years.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly Range<int> s_SupportedYears = Range.Create(MinYear, MaxYear);

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardScope"/> class with the
        /// specified schema and epoch.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="AoorException">The range of supported years by
        /// <paramref name="schema"/> does not contain the interval [1..9999].</exception>
        public StandardScope(ICalendricalSchema schema, DayNumber epoch)
            : base(epoch, CalendricalSegment.Create(schema, s_SupportedYears)) { }

        /// <summary>
        /// Gets the validator for the range of supported years.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        internal static IRangeValidator<int> YearsValidatorImpl { get; } = new YearsValidator_();

        public static implicit operator MinMaxYearScope(StandardScope scope) =>
            MinMaxYearScope.Create(scope);

        /// <inheritdoc />
        public sealed override void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear) Throw.YearOutOfRange(year, paramName);
            PreValidator.ValidateMonth(year, month, paramName);
        }

        /// <inheritdoc />
        public sealed override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear) Throw.YearOutOfRange(year, paramName);
            PreValidator.ValidateMonthDay(year, month, day, paramName);
        }

        /// <inheritdoc />
        public sealed override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear) Throw.YearOutOfRange(year, paramName);
            PreValidator.ValidateDayOfYear(year, dayOfYear, paramName);
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
