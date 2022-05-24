// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using System.Text;

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

            MinMaxDaysSinceEpoch =
                OrderedPair.FromOrderedValues(start.DaysSinceEpoch, end.DaysSinceEpoch);
            MinMaxDateParts =
                OrderedPair.FromOrderedValues(start.DateParts, end.DateParts);
            MinMaxOrdinalParts =
                OrderedPair.FromOrderedValues(start.OrdinalParts, end.OrdinalParts);
        }

        /// <summary>
        /// Gets the pair of earliest and latest supported "day number"s.
        /// </summary>
        public OrderedPair<int> MinMaxDaysSinceEpoch { get; }

        /// <summary>
        /// Gets the pair of earliest and latest supported date parts.
        /// </summary>
        public OrderedPair<Yemoda> MinMaxDateParts { get; }

        /// <summary>
        /// Gets the pair of earliest and latest supported ordinal date parts.
        /// </summary>
        public OrderedPair<Yedoy> MinMaxOrdinalParts { get; }

        /// <summary>
        /// Gets the schema.
        /// </summary>
        internal ICalendricalSchema Schema { get; }

        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "PrintMembers() for records.")]
        private bool PrintMembers(StringBuilder builder)
        {
            builder.Append("Start = { ");
            builder.Append(
                FormattableString.Invariant(
                    $"DaysSinceEpoch = {MinMaxDaysSinceEpoch.LowerValue}, DateParts = {MinMaxDateParts.LowerValue}, OrdinalParts = {MinMaxOrdinalParts.LowerValue}"));
            builder.Append(" }, End = { ");
            builder.Append(
                FormattableString.Invariant(
                    $"DaysSinceEpoch = {MinMaxDaysSinceEpoch.UpperValue}, DateParts = {MinMaxDateParts.UpperValue}, OrdinalParts = {MinMaxOrdinalParts.UpperValue}"));
            builder.Append(" }");
            return true;
        }
    }
}
