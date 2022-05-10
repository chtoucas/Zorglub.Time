// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extensions
{
    using Zorglub.Time.Simple;

    /// <summary>
    /// Provides static helpers for <see cref="CalendarYear"/> or
    /// <see cref="CalendarMonth"/>.
    /// </summary>
    [Obsolete("Use the methods from OrdinalDate.")]
    public static partial class OrdinalResult { }

    public partial class OrdinalResult // CalendarYear.
    {
        /// <summary>
        /// Obtains the first day of the specified year.
        /// </summary>
        [Pure]
        public static OrdinalDate GetStartOfYear(this CalendarYear @this) =>
            OrdinalDate.AtStartOfYear(@this);

        /// <summary>
        /// Obtains the ordinal date corresponding to the specified day of this
        /// year instance.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfYear"/> is
        /// outside the range of valid values.</exception>
        [Pure]
        public static OrdinalDate GetDayOfYear(this CalendarYear @this, int dayOfYear) =>
            OrdinalDate.AtDayOfYear(@this, dayOfYear);

        /// <summary>
        /// Obtains the last day of the specified year.
        /// </summary>
        [Pure]
        public static OrdinalDate GetEndOfYear(this CalendarYear @this) =>
            OrdinalDate.AtEndOfYear(@this);
    }

    public partial class OrdinalResult // CalendarMonth.
    {
#if false

        /// <summary>
        /// Obtains the first day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public static OrdinalDate GetStartOfYear(this CalendarMonth @this)
        {
            var ydoy = @this.Calendar.Schema.GetStartOfYearOrdinalParts(@this.Year);
            return new OrdinalDate(ydoy, @this.Cuid);
        }

        /// <summary>
        /// Obtains the last day of the year to which belongs the specified month.
        /// </summary>
        [Pure]
        public static OrdinalDate GetEndOfYear(this CalendarMonth @this)
        {
            var ydoy = @this.Calendar.Schema.GetEndOfYearOrdinalParts(@this.Year);
            return new OrdinalDate(ydoy, @this.Cuid);
        }

#endif

        /// <summary>
        /// Obtains the first day of the specified month.
        /// </summary>
        [Pure]
        public static OrdinalDate GetStartOfMonth(this CalendarMonth @this) =>
            OrdinalDate.AtStartOfMonth(@this);

        /// <summary>
        /// Obtains the date corresponding to the specified day of the specified
        /// month.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfMonth"/> is
        /// outside the range of valid values.</exception>
        [Pure]
        public static OrdinalDate GetDayOfMonth(this CalendarMonth @this, int dayOfMonth) =>
            OrdinalDate.AtDayOfMonth(@this, dayOfMonth);

        /// <summary>
        /// Obtains the last day of the specified month.
        /// </summary>
        [Pure]
        public static OrdinalDate GetEndOfMonth(this CalendarMonth @this) =>
            OrdinalDate.AtEndOfMonth(@this);
    }
}
