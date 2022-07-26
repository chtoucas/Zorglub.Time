// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Validation;

    /// <summary>
    /// Provides informations on a range of days for a given schema.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class CalendricalSegment : ISchemaBound
    {
        /// <summary>
        /// Represents the underlying schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalSchema _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendricalSegment"/> class.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal CalendricalSegment(ICalendricalSchema schema, Endpoint min, Endpoint max, bool complete)
        {
            _schema = schema;

            IsComplete = complete;

            SupportedDays = Range.Create(min.DaysSinceEpoch, max.DaysSinceEpoch);
            SupportedMonths = Range.Create(min.MonthsSinceEpoch, max.MonthsSinceEpoch);
            SupportedYears = Range.Create(min.Year, max.Year);

            MinMaxDateParts = OrderedPair.FromOrderedValues(min.DateParts, max.DateParts);
            MinMaxOrdinalParts = OrderedPair.FromOrderedValues(min.OrdinalParts, max.OrdinalParts);
            MinMaxMonthParts = OrderedPair.FromOrderedValues(min.MonthParts, max.MonthParts);
        }

        /// <summary>
        /// Returns true if this segment is complete; otherwise returns false.
        /// <para>A segment is said to be <i>complete</i> if it spans all days of a range of years.
        /// </para>
        /// </summary>
        public bool IsComplete { get; }

        /// <summary>
        /// Gets the range of supported days, that is the range of supported numbers of consecutive
        /// days from the epoch.
        /// </summary>
        /// <returns>The range from the first day of the first supported year to the last day of the
        /// last supported year.</returns>
        public Range<int> SupportedDays { get; }

        /// <summary>
        /// Gets the range of supported months, that is the range of supported numbers of consecutive
        /// months from the epoch.
        /// </summary>
        /// <returns>The range from the first month of the first supported year to the last month of
        /// the last supported year.</returns>
        public Range<int> SupportedMonths { get; }

        /// <summary>
        /// Gets the range of supported years.
        /// </summary>
        public Range<int> SupportedYears { get; }

        /// <summary>
        /// Gets the pair of earliest and latest supported date parts.
        /// </summary>
        /// <returns>The pair of the first day of the first supported year and the last day of the
        /// last supported year.</returns>
        public OrderedPair<DateParts> MinMaxDateParts { get; }

        /// <summary>
        /// Gets the pair of earliest and latest supported ordinal date parts.
        /// </summary>
        /// <returns>The pair of the first day of the first supported year and the last day of the
        /// last supported year.</returns>
        public OrderedPair<OrdinalParts> MinMaxOrdinalParts { get; }

        /// <summary>
        /// Gets the pair of earliest and latest supported month parts.
        /// </summary>
        /// <returns>The pair of the first month of the first supported year and the last month of
        /// the last supported year.</returns>
        public OrderedPair<MonthParts> MinMaxMonthParts { get; }

        /// <summary>
        /// Gets the underlying schema.
        /// </summary>
        internal ICalendricalSchema Schema => _schema;

        ICalendricalSchema ISchemaBound.Schema => _schema;

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public sealed override string ToString() => MinMaxDateParts.ToString();

        /// <summary>
        /// Creates the maximal segment for <paramref name="schema"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by the schema
        /// does not contain the year 1.</exception>
        [Pure]
        public static CalendricalSegment CreateMaximal(ICalendricalSchema schema, bool onOrAfterEpoch = false)
        {
            var builder = new CalendricalSegmentBuilder(schema);
            builder.UseMinSupportedYear(onOrAfterEpoch);
            builder.UseMaxSupportedYear();
            return builder.BuildSegment();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CalendricalSegment"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="supportedYears"/> is NOT a
        /// subinterval of the range of supported years by <paramref name="schema"/>.</exception>
        [Pure]
        public static CalendricalSegment Create(ICalendricalSchema schema, Range<int> supportedYears)
        {
            var builder = new CalendricalSegmentBuilder(schema);
            builder.SetSupportedYears(supportedYears);
            return builder.BuildSegment();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CalendricalSegment"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="AoorException">The min date is invalid or outside the range of dates
        /// supported by <paramref name="schema"/>.</exception>
        /// <exception cref="AoorException"><paramref name="maxYear"/> is outside range of supported
        /// years by <paramref name="schema"/>.</exception>
        [Pure]
        internal static CalendricalSegment Create(
            ICalendricalSchema schema,
            int year,
            int month,
            int day,
            int? maxYear)
        {
            var builder = new CalendricalSegmentBuilder(schema);
            builder.SetMinDate(year, month, day);
            if (maxYear.HasValue)
            {
                builder.SetMaxYear(maxYear.Value);
            }
            else
            {
                builder.UseMaxSupportedYear();
            }
            return builder.BuildSegment();
        }

        internal sealed class Endpoint
        {
            public int MonthsSinceEpoch { get; internal set; }
            public int DaysSinceEpoch { get; init; }

            public DateParts DateParts { get; init; }
            public OrdinalParts OrdinalParts { get; init; }

            public MonthParts MonthParts => DateParts.MonthParts;
            public int Year => DateParts.Year;

            /// <summary>
            /// <c>left &gt; right</c>
            /// <para><c>(null &gt; null)</c> evaluates to false</para>
            /// </summary>
            public static bool IsGreaterThan(Endpoint? left, Endpoint? right) =>
                left is not null
                && right is not null
                && left.DaysSinceEpoch.CompareTo(right.DaysSinceEpoch) > 0;

            // Using .NET comparison ops would have been nicer but, since this
            // type is internal, it would have made full code coverage almost
            // impossible to achieve.
            //
            // Comparison w/ null always returns false, even null >= null and
            // null <= null.

            //public static bool operator <(Endpoint? left, Endpoint? right) =>
            //    left is not null && right is not null && left.CompareTo(right) < 0;
            //public static bool operator <=(Endpoint? left, Endpoint? right) =>
            //    left is not null && right is not null && left.CompareTo(right) <= 0;
            //public static bool operator >(Endpoint? left, Endpoint? right) =>
            //    left is not null && right is not null && left.CompareTo(right) > 0;
            //public static bool operator >=(Endpoint? left, Endpoint? right) =>
            //    left is not null && right is not null && left.CompareTo(right) >= 0;

            //public int CompareTo(Endpoint other)
            //{
            //    Requires.NotNull(other);

            //    return DaysSinceEpoch.CompareTo(other.DaysSinceEpoch);
            //}
        }
    }
}
