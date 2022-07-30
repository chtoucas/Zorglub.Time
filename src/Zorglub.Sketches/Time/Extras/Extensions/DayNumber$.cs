// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extras.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="DayNumber"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static partial class DayNumberExtensions { }

    // Alternative impl to Previous() & co.
    public partial class DayNumberExtensions
    {
        /// <summary>
        /// Obtains the day number before the current instance that falls on the
        /// specified day of the week.
        /// <para>See also <seealso cref="DayNumber.Previous(DayOfWeek)"/>.</para>
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfWeek"/> is not
        /// a valid day of the week.</exception>
        /// <exception cref="OverflowException">The operation would overflow the
        /// capacity of <see cref="Int32"/>.</exception>
        //
        // Plus proche jour de la semaine avant dayNumber et qui n'est pas égal
        // à ce dernier.
        [Pure]
        public static DayNumber Before(this DayNumber @this, DayOfWeek dayOfWeek) =>
            // We don't use
            // > PreviousOrSameCore(@this, dayOfWeek, -1, 0);
            // to avoid problems near MinValue.
            DayNumber.NextOrSameCore(@this, dayOfWeek, -7, 0);

        /// <summary>
        /// Obtains the day number before the current instance that falls on the
        /// specified day of the week.
        /// <para>See also <seealso cref="DayNumber.Previous(DayOfWeek)"/>.</para>
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfWeek"/> is not
        /// a valid day of the week.</exception>
        /// <exception cref="OverflowException">The operation would overflow the
        /// capacity of <see cref="Int32"/>.</exception>
        [Pure]
        public static DayNumber Before(this DayNumber @this, DayOfWeek dayOfWeek, int num)
        {
            if (num < 1) Throw.ArgumentOutOfRange(nameof(num));

            // We don't use
            // > PreviousOrSameCore(@this, dayOfWeek, -1, 1 - num);
            // to avoid problems near MinValue.
            return DayNumber.NextOrSameCore(@this, dayOfWeek, -7, num - 1);
        }

        /// <summary>
        /// Obtains the day number on or before the current instance that falls
        /// on the specified day of the week.
        /// <para>If the day number already falls on the given day of the week,
        /// returns the current instance.</para>
        /// <para>See also <seealso cref="DayNumber.PreviousOrSame(DayOfWeek)"/>.</para>
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfWeek"/> is not
        /// a valid day of the week.</exception>
        /// <exception cref="OverflowException">The operation would overflow the
        /// capacity of <see cref="Int32"/>.</exception>
        [Pure]
        public static DayNumber OnOrBefore(this DayNumber @this, DayOfWeek dayOfWeek) =>
            // We don't use
            // > PreviousOrSameCore(@this, dayOfWeek, 0, 0);
            // to avoid problems near MinValue.
            DayNumber.NextOrSameCore(@this, dayOfWeek, -6, 0);

        /// <summary>
        /// Obtains the day number on or before the current instance that falls
        /// on the specified day of the week.
        /// <para>If the day number already falls on the given day of the week,
        /// returns the current instance.</para>
        /// <para>See also <seealso cref="DayNumber.PreviousOrSame(DayOfWeek)"/>.</para>
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfWeek"/> is not
        /// a valid day of the week.</exception>
        /// <exception cref="OverflowException">The operation would overflow the
        /// capacity of <see cref="Int32"/>.</exception>
        //
        // n-ième jour (n >= 1) avant @this (inclue) portant le libellé dayOfWeek.
        [Pure]
        public static DayNumber OnOrBefore(this DayNumber @this, DayOfWeek dayOfWeek, int num)
        {
            if (num < 1) Throw.ArgumentOutOfRange(nameof(num));

            // We don't use
            // > PreviousOrSameCore(@this, dayOfWeek, 0, 1 - num);
            // to avoid problems near MinValue.
            return DayNumber.NextOrSameCore(@this, dayOfWeek, -6, num - 1);
        }

        /// <summary>
        /// Obtains the day number on or after the current instance that falls
        /// on the specified day of the week.
        /// <para>If the day number already falls on the given day of the week,
        /// returns the current instance.</para>
        /// <para>See also <seealso cref="DayNumber.NextOrSame(DayOfWeek)"/>.</para>
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfWeek"/> is not
        /// a valid day of the week.</exception>
        /// <exception cref="OverflowException">The operation would overflow the
        /// capacity of <see cref="Int32"/>.</exception>
        [Pure]
        public static DayNumber OnOrAfter(this DayNumber @this, DayOfWeek dayOfWeek) =>
            // We don't use
            // > NextOrSameCore(@this, dayOfWeek, 0, 0);
            // to avoid problems near MaxValue.
            DayNumber.PreviousOrSameCore(@this, dayOfWeek, 6, 0);

        /// <summary>
        /// Obtains the day number on or after the current instance that falls
        /// on the specified day of the week.
        /// <para>If the day number already falls on the given day of the week,
        /// returns the current instance.</para>
        /// <para>See also <seealso cref="DayNumber.NextOrSame(DayOfWeek)"/>.</para>
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfWeek"/> is not
        /// a valid day of the week.</exception>
        /// <exception cref="OverflowException">The operation would overflow the
        /// capacity of <see cref="Int32"/>.</exception>
        //
        // n-ième jour (n >= 1) après @this (inclue) portant le libellé dayOfWeek.
        [Pure]
        public static DayNumber OnOrAfter(this DayNumber @this, DayOfWeek dayOfWeek, int num)
        {
            if (num < 1) Throw.ArgumentOutOfRange(nameof(num));

            // We don't use
            // > NextOrSameCore(@this, dayOfWeek, 0, 1 - num);
            // to avoid problems near MaxValue.
            return DayNumber.PreviousOrSameCore(@this, dayOfWeek, 6, num - 1);
        }

        /// <summary>
        /// Obtains the day number after the current instance  that falls on the
        /// specified day of the week.
        /// <para>See also <seealso cref="DayNumber.Next(DayOfWeek)"/>.</para>
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfWeek"/> is not
        /// a valid day of the week.</exception>
        /// <exception cref="OverflowException">The operation would overflow the
        /// capacity of <see cref="Int32"/>.</exception>
        //
        // Plus proche jour de la semaine après dayNumber et qui n'est pas égal
        // à ce dernier.
        [Pure]
        public static DayNumber After(this DayNumber @this, DayOfWeek dayOfWeek) =>
            // We don't use
            // > NextOrSameCore(@this, dayOfWeek, 1, 0);
            // to avoid problems near MaxValue.
            DayNumber.PreviousOrSameCore(@this, dayOfWeek, 7, 0);

        /// <summary>
        /// Obtains the day number after the current instance  that falls on the
        /// specified day of the week.
        /// <para>See also <seealso cref="DayNumber.Next(DayOfWeek)"/>.</para>
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfWeek"/> is not
        /// a valid day of the week.</exception>
        /// <exception cref="OverflowException">The operation would overflow the
        /// capacity of <see cref="Int32"/>.</exception>
        [Pure]
        public static DayNumber After(this DayNumber @this, DayOfWeek dayOfWeek, int num)
        {
            if (num < 1) Throw.ArgumentOutOfRange(nameof(num));

            // We don't use
            // > NextOrSameCore(@this, dayOfWeek, 1, 1 - num);
            // to avoid problems near MaxValue.
            return DayNumber.PreviousOrSameCore(@this, dayOfWeek, 7, num - 1);
        }
    }
}
