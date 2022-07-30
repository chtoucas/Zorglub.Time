// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extras.Extensions
{
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Simple;

    // REVIEW(api): IsEpagomenalDay() for the other date types.
    // We use conversion but Calendar could inherit IEpagomenalCalendar<CalendarDay>,
    // not the case right now, but why not?

    /// <summary>
    /// Provides extension methods for <see cref="CalendarDate"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class SimpleDateExtensions
    {
        [Pure]
        public static bool IsEpagomenalDay(this CalendarDate @this, out int epagomenalNumber)
        {
            if (@this.Calendar is IEpagomenalCalendar<CalendarDate> chr)
            {
                return chr.IsEpagomenalDay(@this, out epagomenalNumber);
            }
            else
            {
                epagomenalNumber = 0;
                return false;
            }
        }

        [Pure]
        public static bool IsEpagomenalDay(this CalendarDay @this, out int epagomenalNumber)
        {
            var date = @this.ToCalendarDate();
            return IsEpagomenalDay(date, out epagomenalNumber);
        }

        [Pure]
        public static bool IsEpagomenalDay(this OrdinalDate @this, out int epagomenalNumber)
        {
            var date = @this.ToCalendarDate();
            return IsEpagomenalDay(date, out epagomenalNumber);
        }

        ///// <summary>
        ///// Determines whether the specified date is an epagomenal day or not.
        ///// </summary>
        //[Pure]
        //public static bool IsEpagomenalDay(this CalendarDate @this, out int epagomenalNumber)
        //{
        //    if (@this.Calendar.Schema is IEpagomenalFeaturette sch)
        //    {
        //        @this.Parts.Unpack(out int y, out int m, out int d);
        //        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
        //    }
        //    else
        //    {
        //        epagomenalNumber = 0;
        //        return false;
        //    }
        //}
    }
}
