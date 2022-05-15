// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;

    // ShortScope is internal -> no need to go generics.
    // Keeping this class internal ensures that we have complete control on its
    // instances. In particular, we make sure that none of them is used in
    // a wrong context, meaning in a place where a different schema is expected.
    //
    // We could have re-used the validators, but let's keep things simple.
    // Anyway, the code is pretty straightforward.

    /// <summary>
    /// Represents the short scope of a schema and provides a base for derived classes.
    /// </summary>
    internal abstract class ShortScope : ICalendarScope
    {
        /// <summary>
        /// Represents the latest supported year.
        /// <para>This field is a constant equal to 9999.</para>
        /// </summary>
        public const int MaxYear = 9999;

        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="ShortScope"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">[minYear..9999] is not a subrange of the range of
        /// years supported by <paramref name="schema"/>.</exception>
        protected ShortScope(ICalendricalSchema schema, DayNumber epoch, int minYear)
        {
            Requires.NotNull(schema);

            // NB: don't write
            // > if (minYear < schema.SupportedYears.Min) Throw.ArgumentOutOfRange(nameof(minYear));
            // > if (schema.SupportedYears.Max < MaxYear) Throw.Argument(nameof(schema));
            // This class is internal and the derived classes have a fixed min year,
            // which means that the culprit will be the schema not the specified minYear.
            var range = Range.Create(minYear, MaxYear);
            if (range.IsSubsetOf(schema.SupportedYears) == false) Throw.Argument(nameof(schema));

            Schema = schema;
            Epoch = epoch;

            SupportedYears = Range.CreateLeniently(minYear, MaxYear);

            var minDaysSinceEpoch = schema.GetStartOfYear(minYear);
            var maxDaysSinceEpoch = schema.GetEndOfYear(MaxYear);

            Domain = Range.CreateLeniently(epoch + minDaysSinceEpoch, epoch + maxDaysSinceEpoch);
        }

        /// <inheritdoc />
        public DayNumber Epoch { get; }

        /// <inheritdoc />
        public Range<DayNumber> Domain { get; }

        /// <inheritdoc />
        public Range<int> SupportedYears { get; }

        /// <summary>
        /// Gets the underlying schema.
        /// </summary>
        protected ICalendricalSchema Schema { get; }

        /// <inheritdoc />
        public abstract void ValidateYear(int year, string? paramName = null);

        /// <inheritdoc />
        public abstract void ValidateYearMonth(int year, int month, string? paramName = null);

        /// <inheritdoc />
        public abstract void ValidateYearMonthDay(int year, int month, int day, string? paramName = null);

        /// <inheritdoc />
        public abstract void ValidateOrdinal(int year, int dayOfYear, string? paramName = null);
    }
}
