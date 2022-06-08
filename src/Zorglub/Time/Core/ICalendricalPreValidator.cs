// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    #region Developer Notes

    // Types Implementing ICalendricalPreValidator
    // -------------------------------------------
    //
    // ICalendricalPreValidator                         Public
    // ├─ CalendricalPreValidator   (ICalendrical)      Public
    // ├─ GregorianPreValidator     (Gregorian-only)
    // ├─ JulianPreValidator        (Julian-only)
    // ├─ LunarPreValidator         (CalendricalSchema)
    // ├─ LunisolarPreValidator     (CalendricalSchema)
    // ├─ PlainPreValidator         (CalendricalSchema)
    // ├─ Solar12PreValidator       (CalendricalSchema)
    // └─ Solar13PreValidator       (CalendricalSchema)
    //
    // Comments
    // --------
    // ICalendricalPreValidator is more naturally part of ICalendricalSchema but
    // the code being the same for very different types of schemas, adding the
    // members of this interface to ICalendricalSchema would lead to a lot of
    // duplications. Therefore this is just an implementation detail and one
    // should really use the public property ICalendricalSchema.PreValidator.

    #endregion

    /// <summary>
    /// Provides methods to check the well-formedness of calendrical data.
    /// </summary>
    public interface ICalendricalPreValidator
    {
        /// <summary>
        /// Validates the well-formedness of the specified month of the year.
        /// <para>This method does NOT validate <paramref name="y"/>.</para>
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        /// <exception cref="AoorException">The validation failed.</exception>
        void ValidateMonth(int y, int month, string? paramName = null);

        /// <summary>
        /// Validates the well-formedness of the specified month of the year and day of the month.
        /// <para>This method does NOT validate <paramref name="y"/>.</para>
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        /// <exception cref="AoorException">The validation failed.</exception>
        void ValidateMonthDay(int y, int month, int day, string? paramName = null);

        /// <summary>
        /// Validates the well-formedness of the specified day of the year.
        /// <para>This method does NOT validate <paramref name="y"/>.</para>
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        /// <exception cref="AoorException">The validation failed.</exception>
        void ValidateDayOfYear(int y, int dayOfYear, string? paramName = null);
    }
}