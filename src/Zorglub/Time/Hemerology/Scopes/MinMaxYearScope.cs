// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time;
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Represents a scope for a calendar supporting <i>all</i> dates within a range of years.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class MinMaxYearScope : CalendarScope
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearScope"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="segment"/> is null.</exception>
        public MinMaxYearScope(DayNumber epoch, CalendricalSegment segment)
            : base(epoch, segment)
        {
            Debug.Assert(segment != null);
            Debug.Assert(segment.IsComplete);
        }

        #region Factories

        /// <summary>
        /// Creates the default maximal scope for the specified schema and epoch.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="supportedYears"/> is NOT a
        /// subinterval of the range of supported years by <paramref name="schema"/>.</exception>
        [Pure]
        public static MinMaxYearScope Create(ICalendricalSchema schema, DayNumber epoch, Range<int> supportedYears)
        {
            var seg = CalendricalSegment.Create(schema, supportedYears);

            return new MinMaxYearScope(epoch, seg);
        }

        /// <summary>
        /// Creates the default maximal scope for the specified schema and epoch.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        [Pure]
        public static MinMaxYearScope CreateMaximal(ICalendricalSchema schema, DayNumber epoch)
        {
            var seg = CalendricalSegment.CreateMaximal(schema);

            return new MinMaxYearScope(epoch, seg);
        }

        /// <summary>
        /// Creates the default maximal scope for the specified schema and epoch.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> does not contain the year 1.</exception>
        [Pure]
        public static MinMaxYearScope CreateMaximalOnOrAfterYear1(ICalendricalSchema schema, DayNumber epoch)
        {
            var seg = CalendricalSegment.CreateMaximalOnOrAfterYear1(schema);

            return new MinMaxYearScope(epoch, seg);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MinMaxYearScope"/> class with dates on or after
        /// the specified year.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="year"/> is outside the range of
        /// supported values by the schema.</exception>
        [Pure]
        public static MinMaxYearScope StartingAt(ICalendricalSchema schema, DayNumber epoch, int year)
        {
            var builder = new CalendricalSegmentBuilder(schema);
            builder.SetMinToStartOfYear(year);
            builder.SetMaxToEndOfMaxSupportedYear();
            var segment = builder.BuildSegment();

            return new MinMaxYearScope(epoch, segment);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MinMaxYearScope"/> class with dates on or
        /// before the specified year.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="year"/> is outside the range of
        /// supported values by the schema.</exception>
        [Pure]
        public static MinMaxYearScope EndingAt(ICalendricalSchema schema, DayNumber epoch, int year)
        {
            var builder = new CalendricalSegmentBuilder(schema);
            builder.SetMinToStartOfMinSupportedYear();
            builder.SetMaxToEndOfYear(year);
            var segment = builder.BuildSegment();

            return new MinMaxYearScope(epoch, segment);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MinMaxYearScope"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="scope"/> is NOT complete.</exception>
        [Pure]
        public static MinMaxYearScope Create(CalendarScope scope)
        {
            Requires.NotNull(scope);

            return scope is MinMaxYearScope scope_ ? scope_
                : scope.Segment.IsComplete == false ? Throw.Argument<MinMaxYearScope>(nameof(scope))
                : new MinMaxYearScope(scope.Epoch, scope.Segment);
        }

        #endregion

        /// <inheritdoc />
        public sealed override void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            YearsValidator.Validate(year, paramName);
            PreValidator.ValidateMonth(year, month, paramName);
        }

        /// <inheritdoc />
        public sealed override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
        {
            YearsValidator.Validate(year, paramName);
            PreValidator.ValidateMonthDay(year, month, day, paramName);
        }

        /// <inheritdoc />
        public sealed override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
        {
            YearsValidator.Validate(year, paramName);
            PreValidator.ValidateDayOfYear(year, dayOfYear, paramName);
        }
    }
}
