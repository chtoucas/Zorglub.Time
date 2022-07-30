// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extras.Extensions
{
    using System.ComponentModel;
    using System.Text;

    /// <summary>
    /// Provides extension methods for <see cref="XCivilDate"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class XCivilDateExtensions
    {
        /// <summary>
        /// Gets the ISO week of the year.
        /// <para>Returns zero if the day belongs to the last week of the previous ISO year.</para>
        /// </summary>
        [Pure]
        public static int GetIsoWeekOfYear(this XCivilDate @this)
        {
            // TODO: dates near the end of the year. Which number to return
            // when we are near the boundary of a year and the week does not
            // belong to the current year? We should certainly return a pair
            // (week-of-year, year). We keep this property internal until we
            // reach a final decision.
            // When done, make it a property then make the method
            // GetIsoDayOfWeekAtStartOfYear() private.

            uint dow = XCivilDate.GetIsoDayOfWeekAtStartOfYear(@this.Year);
            uint weekOfYear = ((uint)@this.DayOfYear + 5 + dow) / 7;
            // The first week must have at least 4 days.
            return (int)(dow > 4 ? weekOfYear - 1 : weekOfYear);
        }

        /// <summary>
        /// Obtains the nearest date that falls on the specified day of the week.
        /// <para>Near the calendar boundaries, we do NOT throw an overflow exception, we return the
        /// nearest date within the calendar boundaries.</para>
        /// <para>See also <seealso cref="XCivilDate.Nearest(DayOfWeek)"/>.</para>
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfWeek"/> is not a valid day of the
        /// week.</exception>
        [Pure]
        public static XCivilDate NearestSafe(this XCivilDate @this, DayOfWeek dayOfWeek)
        {
            // Quand on aura décidé quoi faire de cette méthode, repasser en
            // "private" les éléments  suivants : DaysSinceEpoch,
            // Min/MaxDaysSinceEpoch, FromDaysSinceEpoch().
            // Ne pas publier cette méthode...

            Requires.Defined(dayOfWeek);

            // REVIEW: voir si les tests correspondent à ce que j'avance...
            int daysSinceEpoch = @this.DaysSinceEpoch + 3;
            daysSinceEpoch -= MathZ.Modulo(daysSinceEpoch + (DayOfWeek.Monday - dayOfWeek), 7);
            if (daysSinceEpoch > XCivilDate.MaxDaysSinceEpoch)
            {
                daysSinceEpoch -= 7;
            }
            else if (daysSinceEpoch < XCivilDate.MinDaysSinceEpoch)
            {
                daysSinceEpoch += 7;
            }

            return XCivilDate.FromDaysSinceEpoch(daysSinceEpoch);
        }

        /// <summary>
        /// Gets a nicely formatted string representation of the binary data stored in the specified
        /// Gregorian date.
        /// </summary>
        [Pure]
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string FormatBinary(this XCivilDate @this)
        {
            int bin = @this.ToBinary();
            char[] c = Convert.ToString(bin, 2).ToCharArray();
            return new StringBuilder("0...")
                .Append(c[0..^9]).Append('|')
                .Append(c[^9..^5]).Append('|')
                .Append(c[^5..])
                .ToString();
        }
    }
}
