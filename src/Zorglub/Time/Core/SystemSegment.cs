﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Validation;

    /// <summary>
    /// Provides informations on a range of years for a given schema.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class SystemSegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSegment"/> class.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        private SystemSegment(SystemSchema schema, Endpoint start, Endpoint end)
        {
            Debug.Assert(schema != null);
            Debug.Assert(start != null);
            Debug.Assert(start.CompareTo(end) <= 0);

            Schema = schema;

            SupportedDays = new SupportedDays(start.DaysSinceEpoch, end.DaysSinceEpoch);
            MinMaxDateParts = OrderedPair.FromOrderedValues(start.DateParts, end.DateParts);
            MinMaxOrdinalParts = OrderedPair.FromOrderedValues(start.OrdinalParts, end.OrdinalParts);

            SupportedMonths = new SupportedMonths(start.MonthsSinceEpoch, end.MonthsSinceEpoch);
            MinMaxMonthParts = OrderedPair.FromOrderedValues(start.MonthParts, end.MonthParts);

            SupportedYears = new SupportedYears(start.Year, end.Year);
        }

        /// <summary>
        /// Gets the range of supported days, that is the range of supported numbers of consecutive
        /// days from the epoch.
        /// </summary>
        /// <returns>The range from the first day of the first supported year to the last day of the
        /// last supported year.</returns>
        public SupportedDays SupportedDays { get; }

        /// <summary>
        /// Gets the range of supported months, that is the range of supported numbers of consecutive
        /// months from the epoch.
        /// </summary>
        /// <returns>The range from the first month of the first supported year to the last month of
        /// the last supported year.</returns>
        public SupportedMonths SupportedMonths { get; }

        /// <summary>
        /// Gets the range of supported years.
        /// </summary>
        public SupportedYears SupportedYears { get; }

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
        /// Gets the pair of earliest and latest supported month parts.
        /// </summary>
        /// <returns>The pair of the first month of the first supported year and the last month of
        /// the last supported year.</returns>
        public OrderedPair<Yemo> MinMaxMonthParts { get; }

        /// <summary>
        /// Gets the schema.
        /// </summary>
        internal SystemSchema Schema { get; }

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public sealed override string ToString() => SupportedYears.ToString();

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
            Requires.NotNull(schema);
            if (supportedYears.IsSubsetOf(schema.SupportedYears) == false)
            {
                Throw.ArgumentOutOfRange(nameof(supportedYears));
            }

            var (minYear, maxYear) = supportedYears.Endpoints;
            var start = Endpoint.FromMinYear(schema, minYear);
            var end = Endpoint.FromMaxYear(schema, maxYear);

            return new SystemSegment(schema, start, end);
        }

        private sealed class Endpoint
        {
            public int MonthsSinceEpoch { get; init; }
            public int DaysSinceEpoch { get; init; }
            public Yemoda DateParts { get; init; }
            public Yedoy OrdinalParts { get; init; }

            public Yemo MonthParts => DateParts.Yemo;
            public int Year => DateParts.Year;

            public int CompareTo(Endpoint other)
            {
                Requires.NotNull(other);

                return DaysSinceEpoch.CompareTo(other.DaysSinceEpoch);
            }

            public static Endpoint FromMinYear(SystemSchema schema, int minYear)
            {
                var parts = Yemoda.AtStartOfYear(minYear);

                return new Endpoint
                {
                    MonthsSinceEpoch = schema.CountMonthsSinceEpoch(parts.Year, parts.Month),
                    DaysSinceEpoch = schema.GetStartOfYear(minYear),
                    DateParts = parts,
                    OrdinalParts = Yedoy.AtStartOfYear(minYear),
                };
            }

            public static Endpoint FromMaxYear(SystemSchema schema, int maxYear)
            {
                var parts = schema.GetDatePartsAtEndOfYear(maxYear);

                return new Endpoint
                {
                    MonthsSinceEpoch = schema.CountMonthsSinceEpoch(parts.Year, parts.Month),
                    DaysSinceEpoch = schema.GetEndOfYear(maxYear),
                    DateParts = parts,
                    OrdinalParts = schema.GetOrdinalPartsAtEndOfYear(maxYear),
                };
            }
        }
    }
}
