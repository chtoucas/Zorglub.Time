// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Core.Validation;

using Zorglub.Time.Core.Schemas;

using static Zorglub.Time.Core.CalendricalConstants;

/// <summary>
/// Provides methods to check the well-formedness of data in the Gregorian case.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class GregorianPreValidator : ICalendricalPreValidator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianPreValidator"/> class.
    /// </summary>
    private GregorianPreValidator() { }

    /// <summary>
    /// Gets a singleton instance of the <see cref="GregorianPreValidator"/> class.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static GregorianPreValidator Instance { get; } = new();

    #region 64-bit versions

    /// <summary>
    /// Validates the well-formedness of the specified month of the year and day of the month.
    /// <para>This method does NOT validate <paramref name="y"/>.</para>
    /// </summary>
    /// <exception cref="AoorException">The validation failed.</exception>
    public static void ValidateMonthDay(long y, int month, int day, string? paramName = null)
    {
        if (month < 1 || month > Solar12.MonthsInYear)
        {
            Throw.MonthOutOfRange(month, paramName);
        }
        if (day < 1
            || (day > Solar.MinDaysInMonth
                && day > GregorianFormulae.CountDaysInMonth(y, month)))
        {
            Throw.DayOutOfRange(day, paramName);
        }
    }

    /// <summary>
    /// Validates the well-formedness of the specified day of the year.
    /// <para>This method does NOT validate <paramref name="y"/>.</para>
    /// </summary>
    /// <exception cref="AoorException">The validation failed.</exception>
    public static void ValidateDayOfYear(long y, int dayOfYear, string? paramName = null)
    {
        if (dayOfYear < 1
            || (dayOfYear > Solar.MinDaysInYear
                && dayOfYear > GregorianFormulae.CountDaysInYear(y)))
        {
            Throw.DayOfYearOutOfRange(dayOfYear, paramName);
        }
    }

    #endregion

    /// <inheritdoc />
    public void ValidateMonth(int y, int month, string? paramName = null)
    {
        if (month < 1 || month > Solar12.MonthsInYear)
        {
            Throw.MonthOutOfRange(month, paramName);
        }
    }

    /// <inheritdoc />
    public void ValidateMonthDay(int y, int month, int day, string? paramName = null)
    {
        if (month < 1 || month > Solar12.MonthsInYear)
        {
            Throw.MonthOutOfRange(month, paramName);
        }
        if (day < 1
            || (day > Solar.MinDaysInMonth
                && day > GregorianFormulae.CountDaysInMonth(y, month)))
        {
            Throw.DayOutOfRange(day, paramName);
        }
    }

    /// <inheritdoc />
    public void ValidateDayOfYear(int y, int dayOfYear, string? paramName = null)
    {
        if (dayOfYear < 1
            || (dayOfYear > Solar.MinDaysInYear
                && dayOfYear > GregorianFormulae.CountDaysInYear(y)))
        {
            Throw.DayOfYearOutOfRange(dayOfYear, paramName);
        }
    }
}
