// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extras.Extensions
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Simple;

    /// <summary>
    /// Provides extension methods for <see cref="CalendarDate"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static partial class SimpleDateExtensions { }

    public partial class SimpleDateExtensions // IEpagomenalFeaturette
    {
        /// <summary>
        /// Determines whether the specified date is an epagomenal day or not.
        /// </summary>
        [Pure]
        public static bool IsEpagomenal(this CalendarDate @this, out int epagomenalNumber)
        {
            if (@this.Calendar.Schema is IEpagomenalDayFeaturette sch)
            {
                @this.Parts.Unpack(out int y, out int m, out int d);
                return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
            }
            else
            {
                epagomenalNumber = 0;
                return false;
            }
        }

        [Pure]
        public static bool IsEpagomenal(this CalendarDay @this, out int epagomenalNumber) =>
            IsEpagomenal(@this.ToCalendarDate(), out epagomenalNumber);

        [Pure]
        public static bool IsEpagomenal(this OrdinalDate @this, out int epagomenalNumber) =>
            IsEpagomenal(@this.ToCalendarDate(), out epagomenalNumber);
    }

    public partial class SimpleDateExtensions // IBlankDayFeaturette
    {
        /// <summary>
        /// Determines whether the specified date is an epagomenal day or not.
        /// </summary>
        [Pure]
        public static bool IsBlank(this CalendarDate @this)
        {
            if (@this.Calendar.Schema is IBlankDayFeaturette sch)
            {
                @this.Parts.Unpack(out int y, out int m, out int d);
                return sch.IsBlankDay(y, m, d);
            }
            else
            {
                return false;
            }
        }

        [Pure]
        public static bool IsBlank(this CalendarDay @this) => IsBlank(@this.ToCalendarDate());

        [Pure]
        public static bool IsBlank(this OrdinalDate @this) => IsBlank(@this.ToCalendarDate());
    }
}
