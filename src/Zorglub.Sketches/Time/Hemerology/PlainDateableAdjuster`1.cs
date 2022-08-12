// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Hemerology.Scopes;

    // WARNING: this code does NOT work with date types linked to a poly-calendar
    // system; see TDate.FromDayNumber().
    //
    // This class is only interesting when the scope is not complete.
    // The code works because TDate.FromDayNumber() is validating, but it also
    // makes it unefficient when the calendar is complete --- in that case, the
    // validation is simply unnecessary.

    [Obsolete("Does not work with dates linked to a poly-calendar system.")]
    public class PlainDateableAdjuster<TDate> : DateableAdjuster<TDate>
        where TDate : IDate<TDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlainDateableAdjuster{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public PlainDateableAdjuster(CalendarScope scope) : base(scope) { }

        /// <inheritdoc />
        [Pure]
        protected sealed override TDate GetDate(int daysSinceEpoch) =>
            TDate.FromDayNumber(Epoch + daysSinceEpoch);
    }
}
