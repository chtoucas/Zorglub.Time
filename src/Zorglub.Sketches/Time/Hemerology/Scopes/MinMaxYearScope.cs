// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time;
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
        /// is outside the range of supported years by <paramref name="schema"/>.</exception>
        public MinMaxYearScope(ICalendricalSchema schema, DayNumber epoch, int minYear, int maxYear)
            : base(
                  schema,
                  epoch,
                  CalendricalSegment.Create(schema, Range.Create(minYear, maxYear)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearScope"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        private MinMaxYearScope(ICalendricalSchema schema, DayNumber epoch, CalendricalSegment segment)
            : base(schema, epoch, segment) { }

        #region Factories

        /// <summary>
        /// Creates the default maximal scope for the specified schema and epoch.
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

            var segment = CalendricalSegment.CreateMaximal(schema, onOrAfterEpoch);

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
            var builder = new CalendricalSegmentBuilder(schema);
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
            var builder = new CalendricalSegmentBuilder(schema);
            builder.UseMinSupportedYear(onOrAfterEpoch: false);
            builder.SetMaxYear(maxYear);
            var segment = builder.BuildSegment();

            return new MinMaxYearScope(schema, epoch, segment);
        }

        #endregion

        /// <inheritdoc />
        public sealed override void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year, paramName);
            PreValidator.ValidateMonth(year, month, paramName);
        }

        /// <inheritdoc />
        public sealed override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year, paramName);
            PreValidator.ValidateMonthDay(year, month, day, paramName);
        }

        /// <inheritdoc />
        public sealed override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year, paramName);
            PreValidator.ValidateDayOfYear(year, dayOfYear, paramName);
        }
    }
}
