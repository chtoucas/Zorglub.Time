// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using System.Text;

    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Provides informations on an interval of days for a given schema.
    /// <para>This class can only represent subintervals of <see cref="Yemoda.SupportedYears"/>.
    /// </para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed record CalendricalSegment
    {
        // No public ctor, see CalendricalSegmentBuilder.

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendricalSegment"/> class.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal CalendricalSegment(
            ICalendricalSchema schema,
            CalendricalEndpoint start,
            CalendricalEndpoint end)
        {
            Debug.Assert(schema != null);
            Debug.Assert(start <= end);

            Schema = schema;

            Domain = Range.Create(start.DaysSinceEpoch, end.DaysSinceEpoch);
            MinMaxDateParts = OrderedPair.FromOrderedValues(start.DateParts, end.DateParts);
            MinMaxOrdinalParts = OrderedPair.FromOrderedValues(start.OrdinalParts, end.OrdinalParts);

            MonthDomain = Range.Create(start.MonthsSinceEpoch, end.MonthsSinceEpoch);
            MinMaxMonthParts = OrderedPair.FromOrderedValues(start.MonthParts, end.MonthParts);

            SupportedYears = Range.Create(start.Year, end.Year);
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
        public OrderedPair<Yemoda> MinMaxDateParts { get; }

        /// <summary>
        /// Gets the pair of earliest and latest supported ordinal date parts.
        /// </summary>
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
        public OrderedPair<Yemo> MinMaxMonthParts { get; }

        /// <summary>
        /// Gets the range of supported years.
        /// </summary>
        public Range<int> SupportedYears { get; }

        /// <summary>
        /// Gets the schema.
        /// </summary>
        internal ICalendricalSchema Schema { get; }

        /// <summary>
        /// Creates the maximal segment for <paramref name="schema"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> and <see cref="Yemoda"/> are disjoint.
        /// </exception>
        [Pure]
        public static CalendricalSegment CreateMaximal(ICalendricalSchema schema, bool onOrAfterEpoch)
        {
            var builder = new CalendricalSegmentBuilder(schema);
            builder.UseMinSupportedYear(onOrAfterEpoch);
            builder.UseMaxSupportedYear();
            return builder.GetSegment();
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
    }
}
