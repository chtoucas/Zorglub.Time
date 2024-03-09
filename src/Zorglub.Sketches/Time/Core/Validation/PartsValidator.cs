// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Core.Validation;

/// <summary>
/// Provides a validator for calendrical parts.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class PartsValidator : ICalendricalValidator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PartsValidator"/> class.
    /// </summary>
    private PartsValidator() { }

    /// <summary>
    /// Gets a singleton instance of the <see cref="PartsValidator"/> class.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static PartsValidator Instance { get; } = new();

    /// <inheritdoc />
    public void ValidateYearMonth(int year, int month, string? paramName = null)
    {
        if (year < Yemo.MinYear || year > Yemo.MaxYear)
        {
            Throw.YearOutOfRange(year, paramName);
        }
        if (month < Yemo.MinMonth || month > Yemo.MaxMonth)
        {
            Throw.MonthOutOfRange(month, paramName);
        }
    }

    /// <inheritdoc />
    public void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
    {
        if (year < Yemoda.MinYear || year > Yemoda.MaxYear)
        {
            Throw.YearOutOfRange(year, paramName);
        }
        if (month < Yemoda.MinMonth || month > Yemoda.MaxMonth)
        {
            Throw.MonthOutOfRange(month, paramName);
        }
        if (day < Yemoda.MinDay || day > Yemoda.MaxDay)
        {
            Throw.DayOutOfRange(day, paramName);
        }
    }

    /// <inheritdoc />
    public void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
    {
        if (year < Yedoy.MinYear || year > Yedoy.MaxYear)
        {
            Throw.YearOutOfRange(year, paramName);
        }
        if (dayOfYear < Yedoy.MinDayOfYear || dayOfYear > Yedoy.MaxDayOfYear)
        {
            Throw.DayOfYearOutOfRange(dayOfYear, paramName);
        }
    }
}
