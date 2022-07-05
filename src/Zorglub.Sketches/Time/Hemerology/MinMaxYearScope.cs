// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Represents the scope of a schema with dates within a given range of years.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class MinMaxYearScope : CalendarScope
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearScope"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="AoorException"><paramref name="minYear"/> or <paramref name="maxYear"/>
        /// is outside the range of supported years by <paramref name="schema"/> or
        /// <see cref="Yemoda"/>.</exception>
        public MinMaxYearScope(ICalendricalSchema schema, DayNumber epoch, int minYear, int maxYear)
            : base(schema, epoch, GetSegment(schema, minYear, maxYear)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearScope"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        private MinMaxYearScope(ICalendricalSchema schema, DayNumber epoch, CalendricalSection segment)
            : base(schema, epoch, segment) { }

        /// <summary>
        /// Creates a new instance of the <see cref="CalendricalSection"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of years supported by
        /// <paramref name="schema"/> is a strict superset of
        /// <see cref="Yemoda.SupportedYears"/>.</exception>
        // Internal for testing.
        internal static CalendricalSection GetSegment(
            ICalendricalSchema schema, int minYear, int maxYear)
        {
            var builder = new CalendricalSectionBuilder(schema);
            builder.SetMinYear(minYear);
            builder.SetMaxYear(maxYear);
            return builder.BuildSegment();
        }

        /// <summary>
        /// Gets the checker for overflows of the range of years.
        /// </summary>
        internal IOverflowChecker<int> YearOverflowChecker =>
            new YearOverflowChecker_(SupportedYears);

        #region Factories

        /// <summary>
        /// Creates the default maximal scope for the specified schema and epoch.
        /// <para>A maximal scope only supports years within <see cref="Yemoda.SupportedYears"/>,
        /// even if the schema accepts a wider range of years.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        [Pure]
        internal static MinMaxYearScope WithMaximalRange(
            ICalendricalSchema schema, DayNumber epoch, bool onOrAfterEpoch)
        {
            // NB: onOrAfterEpoch = false does not necessary mean "proleptic".
            // int minYear = !onOrAfterEpoch || schema.MinYear > 1
            //    ? Math.Max(schema.MinYear, Yemoda.MinYear)
            //    : 1;
            // int maxYear = Math.Min(Yemoda.MaxYear, schema.MaxYear);

            var segment = CalendricalSection.CreateMaximal(schema, onOrAfterEpoch);

            return new MinMaxYearScope(schema, epoch, segment);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MinMaxYearScope"/> class with dates on or after
        /// the specified year.
        /// </summary>
        [Pure]
        public static MinMaxYearScope WithMinYear(
            ICalendricalSchema schema, DayNumber epoch, int minYear)
        {
            var builder = new CalendricalSectionBuilder(schema);
            builder.SetMinYear(minYear);
            builder.UseMaxSupportedYear();
            var segment = builder.BuildSegment();

            return new MinMaxYearScope(schema, epoch, segment);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MinMaxYearScope"/> class with dates on or
        /// before the specified year.
        /// </summary>
        [Pure]
        public static MinMaxYearScope WithMaxYear(
            ICalendricalSchema schema, DayNumber epoch, int maxYear)
        {
            var builder = new CalendricalSectionBuilder(schema);
            builder.UseMinSupportedYear(onOrAfterEpoch: false);
            builder.SetMaxYear(maxYear);
            var segment = builder.BuildSegment();

            return new MinMaxYearScope(schema, epoch, segment);
        }

        #endregion

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

        private sealed class YearOverflowChecker_ : IOverflowChecker<int>
        {
            private readonly Range<int> _range;

            public YearOverflowChecker_(Range<int> range) { _range = range; }

            public void Check(int value)
            {
                if (value < _range.Min || value > _range.Max) Throw.DateOverflow();
            }

            public void CheckUpperBound(int value)
            {
                if (value > _range.Max) Throw.DateOverflow();
            }

            public void CheckLowerBound(int value)
            {
                if (value < _range.Min) Throw.DateOverflow();
            }
        }
    }
}
