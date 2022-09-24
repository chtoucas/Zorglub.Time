// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Validation;

using Endpoint = CalendricalSegment.Endpoint;

// We validate before and after calling a method from the method:
// - before, to respect the schema layout (_supportedYears)
// - after, to stay within the limits of Yemoda/Yedoy (_partsFactory)

/// <summary>Represents a builder for <see cref="CalendricalSegment"/>.
/// <para>This class cannot be inherited.</para></summary>
public sealed partial class CalendricalSegmentBuilder
{
    /// <summary>Represents the schema.</summary>
    private readonly ICalendricalSchema _schema;

    /// <summary>Represents the adapter for calendrical parts.</summary>
    private readonly PartsAdapter _partsAdapter;

    /// <summary>Represents the validator for the range of supported years.</summary>
    private readonly YearsValidator _yearsValidator;

    /// <summary>Represents the validator for the range of supported values for the number of
    /// consecutive days from the epoch.</summary>
    private readonly DaysValidator _daysValidator;

    /// <summary>Initializes a new instance of the <see cref="CalendricalSegmentBuilder"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
    public CalendricalSegmentBuilder(ICalendricalSchema schema)
    {
        _schema = schema ?? throw new ArgumentNullException(nameof(schema));

        _partsAdapter = new PartsAdapter(schema);
        _yearsValidator = new YearsValidator(schema.SupportedYears);
        _daysValidator = new DaysValidator(schema.SupportedDays);
    }

    /// <summary>Returns true if the minimum has been set; otherwise returns false.</summary>
    public bool HasMin => _min != null;

    /// <summary>Returns true if the maximum has been set; otherwise returns false.</summary>
    public bool HasMax => _max != null;

    /// <summary>Returns true if both minimum and maximum have been set; otherwise returns false.
    /// </summary>
    public bool IsBuildable => HasMin && HasMax;

    private Endpoint? _min;
    /// <summary>Gets the minimum of the segment.</summary>
    /// <exception cref="InvalidOperationException">The minimum was not set.</exception>
    private Endpoint Min
    {
        get => _min ?? Throw.InvalidOperation<Endpoint>();
        set
        {
            if (Endpoint.IsGreaterThan(value, _max))
            {
                Throw.ArgumentOutOfRange(nameof(value));
            }
            _min = value;
        }
    }

    private Endpoint? _max;
    /// <summary>Gets the maximum of the segment.</summary>
    /// <exception cref="InvalidOperationException">The maximum was not set.</exception>
    private Endpoint Max
    {
        get => _max ?? Throw.InvalidOperation<Endpoint>();
        set
        {
            if (Endpoint.IsGreaterThan(_min, value))
            {
                Throw.ArgumentOutOfRange(nameof(value));
            }
            _max = value;
        }
    }

    /// <summary>Gets the pre-validator for this schema.</summary>
    private ICalendricalPreValidator PreValidator => _schema.PreValidator;

    /// <summary>Builds the segment.</summary>
    /// <exception cref="InvalidOperationException">The segment was not buildable.</exception>
    [Pure]
    public CalendricalSegment BuildSegment()
    {
        var min = fixEndpoint(Min);
        var max = fixEndpoint(Max);

        return new CalendricalSegment(_schema, min, max)
        {
            MinIsStartOfYear = isStartOfYear(min),
            MaxIsEndOfYear = isEndOfYear(max)
        };

        [Pure]
        bool isStartOfYear(Endpoint ep) =>
            ep.OrdinalParts == OrdinalParts.AtStartOfYear(ep.Year);

        [Pure]
        bool isEndOfYear(Endpoint ep) =>
            ep.OrdinalParts == _partsAdapter.GetOrdinalPartsAtEndOfYear(ep.Year);

        [Pure]
        Endpoint fixEndpoint(Endpoint ep)
        {
            var (y, m) = ep.MonthParts;
            ep.MonthsSinceEpoch = _schema.CountMonthsSinceEpoch(y, m);
            return ep;
        }
    }
}

public partial class CalendricalSegmentBuilder // Builder methods
{
    /// <summary>Gets or sets the minimum.
    /// <para>The setter automatically update <see cref="MinDateParts"/> and
    /// <see cref="MinOrdinalParts"/>.</para></summary>
    /// <value>The number of consecutive days from the epoch.</value>
    /// <exception cref="InvalidOperationException">(Getter) The minimum is not set.</exception>
    /// <exception cref="AoorException">(Setter) The specified number of consecutive days from the
    /// epoch is outside the range of supported values by the schema.</exception>
    public int MinDaysSinceEpoch
    {
        get => Min.DaysSinceEpoch;
        set => Min = GetEndpointFromDaysSinceEpoch(value);
    }

    /// <summary>Gets or sets the maximum.
    /// <para>The setter automatically update <see cref="MaxDateParts"/> and
    /// <see cref="MaxOrdinalParts"/>.</para></summary>
    /// <value>The number of consecutive days from the epoch.</value>
    /// <exception cref="InvalidOperationException">(Getter) The maximum is not set.</exception>
    /// <exception cref="AoorException">(Setter) The specified number of consecutive days from the
    /// epoch is outside the range of supported values by the schema.</exception>
    public int MaxDaysSinceEpoch
    {
        get => Max.DaysSinceEpoch;
        set => Max = GetEndpointFromDaysSinceEpoch(value);
    }

    /// <summary>Gets or sets the minimum.
    /// <para>The setter automatically update <see cref="MinDaysSinceEpoch"/> and
    /// <see cref="MinOrdinalParts"/>.</para></summary>
    /// <value>The date parts.</value>
    /// <exception cref="InvalidOperationException">(Getter) The minimum is not set.</exception>
    /// <exception cref="AoorException">(Setter) The specified date parts are invalid or outside the
    /// range of supported values by the schema.</exception>
    public DateParts MinDateParts
    {
        get => Min.DateParts;
        set => Min = GetEndpoint(value);
    }

    /// <summary>Gets or sets the maximum.
    /// <para>The setter automatically update <see cref="MaxDaysSinceEpoch"/> and
    /// <see cref="MaxOrdinalParts"/>.</para></summary>
    /// <value>The date parts.</value>
    /// <exception cref="InvalidOperationException">(Getter) The maximum is not set.</exception>
    /// <exception cref="AoorException">(Setter) The specified date parts are invalid or outside the
    /// range of supported values by the schema.</exception>
    public DateParts MaxDateParts
    {
        get => Max.DateParts;
        set => Max = GetEndpoint(value);
    }

    /// <summary>Gets or sets the minimum.
    /// <para>The setter automatically update <see cref="MinDaysSinceEpoch"/> and
    /// <see cref="MinDateParts"/>.</para></summary>
    /// <value>The ordinal date parts.</value>
    /// <exception cref="InvalidOperationException">(Getter) The minimum is not set.</exception>
    /// <exception cref="AoorException">(Setter) The specified ordinal date parts are invalid or
    /// outside the range of supported values by the schema.</exception>
    public OrdinalParts MinOrdinalParts
    {
        get => Min.OrdinalParts;
        set => Min = GetEndpoint(value);
    }

    /// <summary>Gets or sets the maximum.
    /// <para>The setter automatically update <see cref="MaxDaysSinceEpoch"/> and
    /// <see cref="MaxDateParts"/>.</para></summary>
    /// <value>The ordinal date parts.</value>
    /// <exception cref="InvalidOperationException">(Getter) The maximum is not set.</exception>
    /// <exception cref="AoorException">(Setter) The specified ordinal date parts are invalid or
    /// outside the range of supported values by the schema.</exception>
    public OrdinalParts MaxOrdinalParts
    {
        get => Max.OrdinalParts;
        set => Max = GetEndpoint(value);
    }

    /// <summary>Sets the minimum to the start of the specified year.</summary>
    /// <exception cref="AoorException"><paramref name="year"/> is outside the range of supported
    /// values by the schema.</exception>
    public void SetMinToStartOfYear(int year)
    {
        _yearsValidator.Validate(year);

        Min = GetEndpointAtStartOfYear(year);
    }

    /// <summary>Sets the minimum to the start of the earliest supported year.</summary>
    public void SetMinToStartOfMinSupportedYear() =>
        Min = GetEndpointAtStartOfYear(_yearsValidator.MinYear);

    /// <summary>Attempts to set the minimum to the start of the earliest supported year &gt;= 1.
    /// </summary>
    [Pure]
    public bool TrySetMinToStartOfMinSupportedYearOnOrAfterYear1()
    {
        var set = Interval.Intersect(_schema.SupportedYears, Range.StartingAt(1));
        if (set.IsEmpty) return false;

        Min = GetEndpointAtStartOfYear(set.Range.Min);
        return true;
    }

    /// <summary>Sets the maximum to the end of the specified year.</summary>
    /// <exception cref="AoorException"><paramref name="year"/> is outside the range of supported
    /// values by the schema.</exception>
    public void SetMaxToEndOfYear(int year)
    {
        _yearsValidator.Validate(year);

        Max = GetEndpointAtEndOfYear(year);
    }

    /// <summary>Sets the maximum to the end of the latest supported year.</summary>
    public void SetMaxToEndOfMaxSupportedYear() =>
        Max = GetEndpointAtEndOfYear(_yearsValidator.MaxYear);

    // This method throw an ArgumentException not an AoorException, therefore
    // it's not equivalent to set Min and Max separately.
    internal void SetSupportedYears(Range<int> supportedYears)
    {
        if (supportedYears.IsSubsetOf(_schema.SupportedYears) == false)
        {
            Throw.Argument(nameof(supportedYears));
        }

        Min = GetEndpointAtStartOfYear(supportedYears.Min);
        Max = GetEndpointAtEndOfYear(supportedYears.Max);
    }

    //
    // Private helpers
    //

    [Conditional("DEBUG")]
    private void __ValidateYear(int year) => _yearsValidator.Validate(year);

    [Pure]
    private Endpoint GetEndpointAtStartOfYear(int year)
    {
        __ValidateYear(year);

        return new Endpoint
        {
            DaysSinceEpoch = _schema.GetStartOfYear(year),
            DateParts = DateParts.AtStartOfYear(year),
            OrdinalParts = OrdinalParts.AtStartOfYear(year),
        };
    }

    [Pure]
    private Endpoint GetEndpointAtEndOfYear(int year)
    {
        __ValidateYear(year);

        return new Endpoint
        {
            DaysSinceEpoch = _schema.GetEndOfYear(year),
            DateParts = _partsAdapter.GetDatePartsAtEndOfYear(year),
            OrdinalParts = _partsAdapter.GetOrdinalPartsAtEndOfYear(year),
        };
    }

    [Pure]
    private Endpoint GetEndpointFromDaysSinceEpoch(int daysSinceEpoch)
    {
        _daysValidator.Validate(daysSinceEpoch);

        return new Endpoint
        {
            DaysSinceEpoch = daysSinceEpoch,
            DateParts = _partsAdapter.GetDateParts(daysSinceEpoch),
            OrdinalParts = _partsAdapter.GetOrdinalParts(daysSinceEpoch),
        };
    }

    [Pure]
    private Endpoint GetEndpoint(DateParts parts)
    {
        var (y, m, d) = parts;
        _yearsValidator.Validate(y, nameof(parts));
        PreValidator.ValidateMonthDay(y, m, d, nameof(parts));

        return new Endpoint
        {
            DaysSinceEpoch = _schema.CountDaysSinceEpoch(y, m, d),
            DateParts = parts,
            OrdinalParts = _partsAdapter.GetOrdinalParts(y, m, d),
        };
    }

    [Pure]
    private Endpoint GetEndpoint(OrdinalParts parts)
    {
        var (y, doy) = parts;
        _yearsValidator.Validate(y, nameof(parts));
        PreValidator.ValidateDayOfYear(y, doy, nameof(parts));

        return new Endpoint
        {
            DaysSinceEpoch = _schema.CountDaysSinceEpoch(y, doy),
            DateParts = _partsAdapter.GetDateParts(y, doy),
            OrdinalParts = parts,
        };
    }
}
