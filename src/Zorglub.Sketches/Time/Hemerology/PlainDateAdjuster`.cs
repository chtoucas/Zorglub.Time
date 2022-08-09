// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Hemerology.Scopes;

    // The code works because we use TDate.FromDayNumber() which validates
    // the dayNumber, but it also makes the code unefficient when the
    // calendar is complete (the validation is unnecessary).
    //
    // This class is only interesting when the scope is not complete.

    /// <summary>
    /// Provides a plain implementation for <see cref="IDateAdjuster{TDate}"/>.
    /// <para>This class works best when <typeparamref name="TDate"/> is based on the count of
    /// consecutive days since the epoch.</para>
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public class PlainDateAdjuster<TDate> : DateAdjuster<TDate>
        where TDate : IDate<TDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlainDateAdjuster{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public PlainDateAdjuster(CalendarScope scope) : base(scope) { }

        /// <inheritdoc />
        [Pure]
        protected sealed override TDate GetDate(int daysSinceEpoch) =>
            TDate.FromDayNumber(Epoch + daysSinceEpoch);
    }
}
