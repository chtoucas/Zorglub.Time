// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using System.Text;

    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Provides informations on a range of days for a given schema.
    /// <para>Only <i>complete</i> ranges of years are supported by this type.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed record SystemSegment
    {
        // No public ctor, see SystemSegmentBuilder.

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSegment"/> class.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal SystemSegment(SystemSchema schema, Endpoint start, Endpoint end)
        {
            Debug.Assert(schema != null);
            Debug.Assert(start <= end);

            Schema = schema;

            Domain = Range.Create(start.DaysSinceEpoch, end.DaysSinceEpoch);
            MinMaxDateParts = OrderedPair.FromOrderedValues(start.DateParts, end.DateParts);
            MinMaxOrdinalParts = OrderedPair.FromOrderedValues(start.OrdinalParts, end.OrdinalParts);

            MonthDomain = Range.FromEndpoints(MinMaxDateParts.Select(CountMonthsSinceEpoch, CountMonthsSinceEpoch));
            MinMaxMonthParts = OrderedPair.FromOrderedValues(start.MonthParts, end.MonthParts);

            SupportedYears = Range.Create(start.Year, end.Year);

            [Pure]
            int CountMonthsSinceEpoch(Yemoda ymd)
            {
                var (y, m, _) = ymd;
                return schema.CountMonthsSinceEpoch(y, m);
            }
        }

        /// <summary>
        /// Gets the range of supported days, or more precisely the range of supported numbers of
        /// consecutive days from the epoch.
        /// </summary>
        /// <returns>The range from the first day of the first supported year to the last day of the
        /// last supported year.</returns>
        public Range<int> Domain { get; }

        /// <summary>
        /// Gets the pair of earliest and latest supported date parts.
        /// </summary>
        /// <returns>The pair of the first day of the first supported year and the last day of the
        /// last supported year.</returns>
        public OrderedPair<Yemoda> MinMaxDateParts { get; }

        /// <summary>
        /// Gets the pair of earliest and latest supported ordinal date parts.
        /// </summary>
        /// <returns>The pair of the first day of the first supported year and the last day of the
        /// last supported year.</returns>
        public OrderedPair<Yedoy> MinMaxOrdinalParts { get; }

        /// <summary>
        /// Gets the range of supported months, or more precisely the range of supported numbers of
        /// consecutive months from the epoch.
        /// </summary>
        /// <returns>The range from the first month of the first supported year to the last month of
        /// the last supported year.</returns>
        public Range<int> MonthDomain { get; }

        /// <summary>
        /// Gets the pair of earliest and latest supported month parts.
        /// </summary>
        /// <returns>The pair of the first month of the first supported year and the last month of
        /// the last supported year.</returns>
        public OrderedPair<Yemo> MinMaxMonthParts { get; }

        /// <summary>
        /// Gets the range of supported years.
        /// </summary>
        public Range<int> SupportedYears { get; }

        /// <summary>
        /// Gets the schema.
        /// </summary>
        internal SystemSchema Schema { get; }

        /// <summary>
        /// Creates the maximal segment for <paramref name="schema"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        [Pure]
        public static SystemSegment CreateMaximal(SystemSchema schema)
        {
            var builder = new SystemSegmentBuilder(schema);
            builder.SetSupportedYears(schema.SupportedYears);
            return builder.BuildSegment();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SystemSegment"/> class from the specified range
        /// of supported years.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="AoorException"><paramref name="supportedYears"/> is NOT a subinterval
        /// of the range of supported years by <paramref name="schema"/>.</exception>
        [Pure]
        public static SystemSegment Create(SystemSchema schema, Range<int> supportedYears)
        {
            var builder = new SystemSegmentBuilder(schema);
            builder.SetSupportedYears(supportedYears);
            return builder.BuildSegment();
        }

        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "PrintMembers() for records.")]
        private bool PrintMembers(StringBuilder builder)
        {
            builder.Append("Start = { ");

            builder.Append(
                FormattableString.Invariant(
                    $"DaysSinceEpoch = {Domain.Min}, DateParts = {MinMaxDateParts.LowerValue}, OrdinalParts = {MinMaxOrdinalParts.LowerValue}"));
            builder.Append(
                FormattableString.Invariant(
                    $"MonthsSinceEpoch = {MonthDomain.Min}, MonthParts = {MinMaxMonthParts.LowerValue}"));

            builder.Append(" }, End = { ");
            builder.Append(
                FormattableString.Invariant(
                    $"DaysSinceEpoch = {Domain.Max}, DateParts = {MinMaxDateParts.UpperValue}, OrdinalParts = {MinMaxOrdinalParts.UpperValue}"));
            builder.Append(
                FormattableString.Invariant(
                    $"MonthsSinceEpoch = {MonthDomain.Max}, MonthParts = {MinMaxMonthParts.UpperValue}"));

            builder.Append(" }");
            return true;
        }

        // By keeping this record internal, we can ensure that the properties are
        // coherent, ie that they represent the same day. Furthemore, an endpoint
        // does not keep track of the schema, which makes it incomplete.
        internal sealed record Endpoint
        {
            public int DaysSinceEpoch { get; init; }
            public Yemoda DateParts { get; init; }
            public Yedoy OrdinalParts { get; init; }

            public Yemo MonthParts => DateParts.Yemo;
            public int Year => DateParts.Year;

            // Comparison w/ null always returns false, even null >= null and null <= null.

            public static bool operator <(Endpoint? left, Endpoint? right) =>
                left is not null && right is not null && left.CompareTo(right) < 0;
            public static bool operator <=(Endpoint? left, Endpoint? right) =>
                left is not null && right is not null && left.CompareTo(right) <= 0;
            public static bool operator >(Endpoint? left, Endpoint? right) =>
                left is not null && right is not null && left.CompareTo(right) > 0;
            public static bool operator >=(Endpoint? left, Endpoint? right) =>
                left is not null && right is not null && left.CompareTo(right) >= 0;

            public int CompareTo(Endpoint other)
            {
                Requires.NotNull(other);

                return DaysSinceEpoch.CompareTo(other.DaysSinceEpoch);
            }
        }
    }
}
