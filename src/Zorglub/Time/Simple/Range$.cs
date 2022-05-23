// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Collections.Generic;

    using Zorglub.Time.Core.Intervals;

    // TODO(code): WithCalendar(), optimize enumeration (see DateRange), LongCount().

    /// <summary>
    /// Provides extension methods for <see cref="Range{T}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static partial class RangeExtensions { }

    public partial class RangeExtensions // Range<CalendarDate>
    {
        #region Finite

        /// <summary>
        /// Obtains the number of elements in the specified range.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        [Pure]
        public static int Count(this Range<CalendarDate> @this) => @this.Max - @this.Min + 1;

        #endregion
        #region Enumerable

        /// <summary>
        /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
        /// </summary>
        [Pure]
        public static IEnumerable<CalendarDate> ToEnumerable(this Range<CalendarDate> @this)
        {
            var min = @this.Min;
            var max = @this.Max;

            for (var i = min; i <= max; i++)
            {
                yield return i;
            }
        }

        #endregion
        #region Interconversion

        /// <summary>
        /// Interconverts the specified range to a range within a different calendar.
        /// </summary>
        /// <remarks>
        /// <para>This method always performs the conversion whether it's necessary or not. To avoid
        /// an expensive operation, it's better to check before that <paramref name="newCalendar"/>
        /// is actually different from the calendar of the current instance.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
        /// </exception>
        [Pure]
        public static Range<CalendarDate> WithCalendar(this Range<CalendarDate> @this, Calendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            var (min, max) = @this.Endpoints;

            var start = min.WithCalendar(newCalendar);
            var end = max.WithCalendar(newCalendar);

            return Range.Create(start, end);
        }

        #endregion
    }

    public partial class RangeExtensions // Range<CalendarDay>
    {
        #region Finite

        /// <summary>
        /// Obtains the number of elements in the specified range.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        [Pure]
        public static int Count(this Range<CalendarDay> @this) => @this.Max - @this.Min + 1;

        #endregion
        #region Enumerable

        /// <summary>
        /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
        /// </summary>
        [Pure]
        public static IEnumerable<CalendarDay> ToEnumerable(this Range<CalendarDay> @this)
        {
            var min = @this.Min;
            var max = @this.Max;

            for (var i = min; i <= max; i++)
            {
                yield return i;
            }
        }

        #endregion
    }

    public partial class RangeExtensions // Range<OrdinalDate>
    {
        #region Finite

        /// <summary>
        /// Obtains the number of elements in the specified range.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        [Pure]
        public static int Count(this Range<OrdinalDate> @this) => @this.Max - @this.Min + 1;

        #endregion
        #region Enumerable

        /// <summary>
        /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
        /// </summary>
        [Pure]
        public static IEnumerable<OrdinalDate> ToEnumerable(this Range<OrdinalDate> @this)
        {
            var min = @this.Min;
            var max = @this.Max;

            for (var i = min; i <= max; i++)
            {
                yield return i;
            }
        }

        #endregion
    }

    public partial class RangeExtensions // Range<CalendarMonth>
    {
        #region Finite

        /// <summary>
        /// Obtains the number of elements in the specified range.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        [Pure]
        public static int Count(this Range<CalendarMonth> @this) => @this.Max - @this.Min + 1;

        #endregion
        #region Enumerable

        /// <summary>
        /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
        /// </summary>
        [Pure]
        public static IEnumerable<CalendarMonth> ToEnumerable(this Range<CalendarMonth> @this)
        {
            var min = @this.Min;
            var max = @this.Max;

            for (var i = min; i <= max; i++)
            {
                yield return i;
            }
        }

        #endregion
    }
}
