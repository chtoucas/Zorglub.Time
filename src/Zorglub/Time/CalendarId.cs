// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    /// <summary>
    /// Specifies the permanent unique identifier of a calendar.
    /// <para>This is a feature only available to system calendars.</para>
    /// </summary>
    public enum CalendarId
    {
        /// <summary>
        /// The permanent identifier of the Gregorian calendar.
        /// </summary>
        Gregorian = 0,

        /// <summary>
        /// The permanent identifier of the Julian calendar.
        /// </summary>
        Julian,

        /// <summary>
        /// The permanent identifier of the Armenian calendar.
        /// </summary>
        Armenian,

        /// <summary>
        /// The permanent identifier of the Coptic calendar.
        /// </summary>
        Coptic,

        /// <summary>
        /// The permanent identifier of the Ethiopic calendar.
        /// </summary>
        Ethiopic,

        /// <summary>
        /// The permanent identifier of the Tabular Islamic calendar.
        /// </summary>
        TabularIslamic,

        /// <summary>
        /// The permanent identifier of the Zoroastrian calendar.
        /// </summary>
        Zoroastrian,

        // WARNING: whenever we add an entry, we MUST update CalendarIdExtensions
        // and Cuid.
    }

    /// <summary>
    /// Provides extension methods for <see cref="CalendarId"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal static class CalendarIdExtensions
    {
        /// <summary>
        /// Returns true if the specified permanent ID is invalid; otherwise
        /// returns false.
        /// </summary>
        public static bool IsInvalid(this CalendarId id) =>
            id < CalendarId.Gregorian || id > CalendarId.Zoroastrian;

        /// <summary>
        /// Converts the permanent ID to a calendar key.
        /// </summary>
        /// <exception cref="AoorException">The specified ID is not valid.
        /// </exception>
        public static string ToCalendarKey(this CalendarId @this) =>
            @this switch
            {
                CalendarId.Gregorian => "Gregorian",
                CalendarId.Julian => "Julian",
                CalendarId.Armenian => "Armenian",
                CalendarId.Coptic => "Coptic",
                CalendarId.Ethiopic => "Ethiopic",
                CalendarId.TabularIslamic => "Tabular Islamic",
                CalendarId.Zoroastrian => "Zoroastrian",
                _ => Throw.ArgumentOutOfRange<string>(nameof(@this)),
            };
    }
}
