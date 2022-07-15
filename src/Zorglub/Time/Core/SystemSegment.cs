﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Validation;

    /// <summary>
    /// Provides informations on a range of years for a given system schema.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class SystemSegment : ISchemaBound<SystemSchema>
    {
        /// <summary>
        /// Represents the underlying schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly SystemSchema _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSegment"/> class.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        private SystemSegment(SystemSchema schema, Endpoint min, Endpoint max)
        {
            _schema = schema;

            SupportedDays = new SupportedDays(min.DaysSinceEpoch, max.DaysSinceEpoch);
            MinMaxDateParts = OrderedPair.FromOrderedValues(min.DateParts, max.DateParts);
            MinMaxOrdinalParts = OrderedPair.FromOrderedValues(min.OrdinalParts, max.OrdinalParts);

            SupportedMonths = new SupportedMonths(min.MonthsSinceEpoch, max.MonthsSinceEpoch);
            MinMaxMonthParts = OrderedPair.FromOrderedValues(min.MonthParts, max.MonthParts);

            SupportedYears = new SupportedYears(min.Year, max.Year);
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
        /// Gets the underlying system schema.
        /// </summary>
        internal SystemSchema Schema => _schema;

        SystemSchema ISchemaBound<SystemSchema>.Schema => _schema;

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
            var min = FixEndpoint(new Endpoint
            {
                DaysSinceEpoch = schema.GetStartOfYear(minYear),
                DateParts = Yemoda.AtStartOfYear(minYear),
                OrdinalParts = Yedoy.AtStartOfYear(minYear),
            });
            var max = FixEndpoint(new Endpoint
            {
                DaysSinceEpoch = schema.GetEndOfYear(maxYear),
                DateParts = schema.GetDatePartsAtEndOfYear(maxYear),
                OrdinalParts = schema.GetOrdinalPartsAtEndOfYear(maxYear),
            });

            return new SystemSegment(schema, min, max);

            [Pure]
            Endpoint FixEndpoint(Endpoint ep)
            {
                ep.DateParts.Unpack(out int y, out int m);
                ep.MonthsSinceEpoch = schema.CountMonthsSinceEpoch(y, m);
                return ep;
            }
        }

        private sealed class Endpoint
        {
            public int MonthsSinceEpoch { get; internal set; }
            public int DaysSinceEpoch { get; init; }

            public Yemoda DateParts { get; init; }
            public Yedoy OrdinalParts { get; init; }

            public Yemo MonthParts => DateParts.Yemo;
            public int Year => DateParts.Year;
        }
    }
}
