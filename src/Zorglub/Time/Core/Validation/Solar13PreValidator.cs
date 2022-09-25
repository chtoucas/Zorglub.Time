// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Validation;

using static Zorglub.Time.Core.CalendricalConstants;

/// <summary>
/// Provides methods to check the well-formedness of data according to a schema with profile
/// <see cref="CalendricalProfile.Solar13"/>.
/// <para>For such schemas, we can mostly avoid to compute the number of days in a year or in
/// a month.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class Solar13PreValidator : ICalendricalPreValidator
{
    /// <summary>
    /// Represents the schema.
    /// </summary>
    private readonly CalendricalSchema _schema;

    /// <summary>
    /// Initializes a new instance of the <see cref="Solar13PreValidator"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
    public Solar13PreValidator(CalendricalSchema schema)
    {
        _schema = schema ?? throw new ArgumentNullException(nameof(schema));

        Requires.Profile(schema, CalendricalProfile.Solar13);
    }

    /// <inheritdoc />
    public void ValidateMonth(int y, int month, string? paramName = null)
    {
        if (month < 1 || month > Solar13.MonthsInYear)
        {
            Throw.MonthOutOfRange(month, paramName);
        }
    }

    /// <inheritdoc />
    public void ValidateMonthDay(int y, int month, int day, string? paramName = null)
    {
        if (month < 1 || month > Solar13.MonthsInYear)
        {
            Throw.MonthOutOfRange(month, paramName);
        }
        if (day < 1
            || (day > Solar.MinDaysInMonth
                && day > _schema.CountDaysInMonth(y, month)))
        {
            Throw.DayOutOfRange(day, paramName);
        }
    }

    /// <inheritdoc />
    public void ValidateDayOfYear(int y, int dayOfYear, string? paramName = null)
    {
        if (dayOfYear < 1
            || (dayOfYear > Solar.MinDaysInYear
                && dayOfYear > _schema.CountDaysInYear(y)))
        {
            Throw.DayOfYearOutOfRange(dayOfYear, paramName);
        }
    }
}
