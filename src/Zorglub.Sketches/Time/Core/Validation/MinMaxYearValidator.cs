// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Core.Validation;

using Zorglub.Time.Core.Intervals;

/// <summary>
/// Represents a validator for a calendar supporting <i>all</i> dates within a range of years.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class MinMaxYearValidator : ICalendricalValidator, ISchemaBound
{
    /// <summary>
    /// Represents the underlying schema.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly ICalendricalSchema _schema;

    /// <summary>
    /// Represents the pre-validator.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly ICalendricalPreValidator _preValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="MinMaxYearValidator"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
    /// <exception cref="AoorException"><paramref name="supportedYears"/> is NOT a subinterval
    /// of the range of supported years by <paramref name="schema"/>.</exception>
    public MinMaxYearValidator(ICalendricalSchema schema, Range<int> supportedYears)
    {
        _schema = schema ?? throw new ArgumentNullException(nameof(schema));

        _preValidator = schema.PreValidator;

        var seg = CalendricalSegment.Create(schema, supportedYears);

        Segment = seg;
        DaysValidator = new DaysValidator(seg.SupportedDays);
        MonthsValidator = new MonthsValidator(seg.SupportedMonths);
        YearsValidator = new YearsValidator(seg.SupportedYears);
    }

    /// <summary>
    /// Gets the segment of supported days.
    /// </summary>
    public CalendricalSegment Segment { get; }

    /// <summary>
    /// Gets the validator for the range of supported days.
    /// </summary>
    public DaysValidator DaysValidator { get; }

    /// <summary>
    /// Gets the validator for the range of supported months.
    /// </summary>
    public MonthsValidator MonthsValidator { get; }

    /// <summary>
    /// Gets the validator for the range of supported years.
    /// </summary>
    public YearsValidator YearsValidator { get; }

    ICalendricalSchema ISchemaBound.Schema => _schema;

    /// <inheritdoc />
    public void ValidateYearMonth(int year, int month, string? paramName = null)
    {
        YearsValidator.Validate(year, paramName);
        _preValidator.ValidateMonth(year, month, paramName);
    }

    /// <inheritdoc />
    public void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
    {
        YearsValidator.Validate(year, paramName);
        _preValidator.ValidateMonthDay(year, month, day, paramName);
    }

    /// <inheritdoc />
    public void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
    {
        YearsValidator.Validate(year, paramName);
        _preValidator.ValidateDayOfYear(year, dayOfYear, paramName);
    }
}
