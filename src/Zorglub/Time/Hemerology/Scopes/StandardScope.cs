// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Represents the standard short scope of a schema.
    /// <para>A standard scope supports dates within the interval [1..9999] of years.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    /// <remarks>
    /// <para>This is the scope used by <see cref="Simple.Calendar"/>, except in the Gregorian and
    /// Julian cases.</para>
    /// </remarks>
    public sealed class StandardScope : ICalendarScope
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
        private static readonly Range<int> s_SupportedYears = Range.CreateLeniently(MinYear, MaxYear);

        /// <summary>
        /// Represents the pre-validator.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalPreValidator _preValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardScope"/> class with the
        /// specified schema and epoch.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> does not contain the interval [1..9999].</exception>
        public StandardScope(ICalendricalSchema schema, DayNumber epoch)
        {
            Requires.NotNull(schema);
            if (s_SupportedYears.IsSubsetOf(schema.SupportedYears) == false) Throw.Argument(nameof(schema));

            Schema = schema;
            Epoch = epoch;
            _preValidator = schema.PreValidator;

            var seg = CalendricalSegment.Create(schema, s_SupportedYears);
            Domain = seg.GetFixedDomain(epoch);
        }

        /// <inheritdoc />
        public DayNumber Epoch { get; }

        /// <inheritdoc />
        public Range<DayNumber> Domain { get; }

        /// <inheritdoc />
        public Range<int> SupportedYears => s_SupportedYears;

        /// <summary>
        /// Gets the underlying schema.
        /// </summary>
        internal ICalendricalSchema Schema { get; }

        /// <summary>
        /// Gets the checker for overflows of the range of years.
        /// </summary>
        internal static IOverflowChecker<int> YearOverflowChecker { get; } = new YearOverflowChecker_();

        /// <inheritdoc />
        public void ValidateYear(int year, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear) Throw.YearOutOfRange(year, paramName);
        }

        /// <inheritdoc />
        public void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear) Throw.YearOutOfRange(year, paramName);
            _preValidator.ValidateMonth(year, month, paramName);
        }

        /// <inheritdoc />
        public void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear) Throw.YearOutOfRange(year, paramName);
            _preValidator.ValidateMonthDay(year, month, day, paramName);
        }

        /// <inheritdoc />
        public void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear) Throw.YearOutOfRange(year, paramName);
            _preValidator.ValidateDayOfYear(year, dayOfYear, paramName);
        }

        private sealed class YearOverflowChecker_ : IOverflowChecker<int>
        {
            public void Check(int year)
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
