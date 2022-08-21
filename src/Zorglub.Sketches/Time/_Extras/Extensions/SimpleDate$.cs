// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extensions
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Simple;

    /// <summary>
    /// Provides extension methods for <see cref="CalendarDate"/>, <see cref="CalendarDay"/> and
    /// <see cref="OrdinalDate"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static partial class SimpleDateExtensions { }

    public partial class SimpleDateExtensions // Interconversion
    {
        /// <summary>
        /// Interconverts the specified date to a date within a different calendar.
        /// </summary>
        /// <remarks>
        /// <para>This method always performs the conversion whether it's necessary or not. To avoid
        /// an expensive operation, it's better to check before that <paramref name="newCalendar"/>
        /// is actually different from the calendar of the current instance.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
        /// </exception>
        /// <exception cref="AoorException">The specified date cannot be converted into the new
        /// calendar, the resulting date would be outside its range of years.</exception>
        [Pure]
        public static CalendarDate WithCalendar(this CalendarDate date, SimpleCalendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            return newCalendar.GetDate(date.DayNumber).ToCalendarDate();
        }

        /// <summary>
        /// Interconverts the specified date to a date within a different calendar.
        /// </summary>
        /// <remarks>
        /// <para>This method always performs the conversion whether it's necessary or not. To avoid
        /// an expensive operation, it's better to check before that <paramref name="newCalendar"/>
        /// is actually different from the calendar of the current instance.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
        /// </exception>
        /// <exception cref="AoorException">The specified date cannot be converted into the new
        /// calendar, the resulting date would be outside its range of years.</exception>
        [Pure]
        public static CalendarDay WithCalendar(this CalendarDay date, SimpleCalendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            return newCalendar.GetDate(date.DayNumber);
        }

        /// <summary>
        /// Interconverts the specified date to a date within a different calendar.
        /// </summary>
        /// <remarks>
        /// <para>This method always performs the conversion whether it's necessary or not. To avoid
        /// an expensive operation, it's better to check before that <paramref name="newCalendar"/>
        /// is actually different from the calendar of the current instance.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
        /// </exception>
        /// <exception cref="AoorException">The specified date cannot be converted into the new
        /// calendar, the resulting date would be outside its range of years.</exception>
        [Pure]
        public static OrdinalDate WithCalendar(this OrdinalDate date, SimpleCalendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            return newCalendar.GetDate(date.DayNumber).ToOrdinalDate();
        }
    }

    public partial class SimpleDateExtensions // IEpagomenalFeaturette
    {
        /// <summary>
        /// Determines whether the specified date is an epagomenal day or not.
        /// </summary>
        [Pure]
        public static bool IsEpagomenal(this CalendarDate date, out int epagomenalNumber)
        {
            if (date.Calendar.Schema is IEpagomenalDayFeaturette sch)
            {
                date.Parts.Unpack(out int y, out int m, out int d);
                return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
            }
            else
            {
                epagomenalNumber = 0;
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified date is an epagomenal day or not.
        /// </summary>
        [Pure]
        public static bool IsEpagomenal(this CalendarDay date, out int epagomenalNumber) =>
            IsEpagomenal(date.ToCalendarDate(), out epagomenalNumber);

        /// <summary>
        /// Determines whether the specified date is an epagomenal day or not.
        /// </summary>
        [Pure]
        public static bool IsEpagomenal(this OrdinalDate date, out int epagomenalNumber) =>
            IsEpagomenal(date.ToCalendarDate(), out epagomenalNumber);
    }

    public partial class SimpleDateExtensions // IBlankDayFeaturette
    {
        /// <summary>
        /// Determines whether the specified date is a blank day or not.
        /// </summary>
        [Pure]
        public static bool IsBlank(this CalendarDate date)
        {
            if (date.Calendar.Schema is IBlankDayFeaturette sch)
            {
                date.Parts.Unpack(out int y, out int m, out int d);
                return sch.IsBlankDay(y, m, d);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified date is a blank day or not.
        /// </summary>
        [Pure]
        public static bool IsBlank(this CalendarDay date) => IsBlank(date.ToCalendarDate());

        /// <summary>
        /// Determines whether the specified date is a blank day or not.
        /// </summary>
        [Pure]
        public static bool IsBlank(this OrdinalDate date) => IsBlank(date.ToCalendarDate());
    }
}
