// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized
{
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Core;

    /// <summary>
    /// Represents the common adjusters for <typeparamref name="TDate"/>.
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public class SpecializedAdjusters<TDate> : IDateAdjusters<TDate>
        where TDate : IDate<TDate>, ISpecializedDate
    {
        /// <summary>
        /// Represents the schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalSchema _schema;

        /// <summary>
        /// Represents the epoch.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly DayNumber _epoch;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecializedAdjusters{TDate}"/> class.
        /// </summary>
        public SpecializedAdjusters(DayNumber epoch, ICalendricalSchema schema)
        {
            Requires.NotNull(schema);

            _epoch = epoch;
            _schema = schema;
        }

        /// <summary>
        /// Creates a new instance of <typeparamref name="TDate"/> from the specified count of
        /// consecutive days since the epoch.
        /// </summary>
        [Pure]
        protected virtual TDate GetDate(int daysSinceEpoch) =>
            TDate.FromDayNumber(_epoch + daysSinceEpoch);

        /// <inheritdoc />
        [Pure]
        public TDate GetStartOfYear(TDate date)
        {
            int daysSinceEpoch = _schema.GetStartOfYear(date.Year);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetEndOfYear(TDate date)
        {
            int daysSinceEpoch = _schema.GetEndOfYear(date.Year);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetStartOfMonth(TDate date)
        {
            _schema.GetDateParts(date.DayNumber - _epoch, out int y, out int m, out _);
            int daysSinceEpoch = _schema.GetStartOfMonth(y, m);
            return GetDate(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public TDate GetEndOfMonth(TDate date)
        {
            _schema.GetDateParts(date.DayNumber - _epoch, out int y, out int m, out _);
            int daysSinceEpoch = _schema.GetEndOfMonth(y, m);
            return GetDate(daysSinceEpoch);
        }
    }
}
