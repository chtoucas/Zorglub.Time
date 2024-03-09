// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz.Obsolete;

using Zorglub.Time.Core;

/// <summary>
/// Provides methods you can use to create new calendrical parts.
/// </summary>
internal sealed partial class SchemaAdapterSlim : ISchemaAdapter
{
    /// <summary>
    /// Represents the schema.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly ICalendricalSchema _schema;

    /// <summary>
    /// Initializes a new instance of the <see cref="SchemaAdapterSlim"/> class.
    /// </summary>
    public SchemaAdapterSlim(ICalendricalSchema schema)
    {
        Debug.Assert(schema != null);
        Debug.Assert(schema.SupportedYears.IsSubsetOf(Yemoda.SupportedYears));

        _schema = schema;
    }
}

internal partial class SchemaAdapterSlim // Conversions
{
    /// <inheritdoc />
    [Pure]
    public Yemo GetMonthParts(int monthsSinceEpoch)
    {
        _schema.GetMonthParts(monthsSinceEpoch, out int y, out int m);
        if (m < Yemo.MinMonth || m > Yemo.MaxMonth) Throw.MonthOutOfRange(m);
        return new Yemo(y, m);
    }

    /// <inheritdoc />
    [Pure]
    public Yemoda GetDateParts(int daysSinceEpoch)
    {
        _schema.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        if (m < Yemoda.MinMonth || m > Yemoda.MaxMonth) Throw.MonthOutOfRange(m);
        if (d < Yemoda.MinDay || d > Yemoda.MaxDay) Throw.DayOutOfRange(d);
        return new Yemoda(y, m, d);
    }

    /// <inheritdoc />
    [Pure]
    public Yedoy GetOrdinalParts(int daysSinceEpoch)
    {
        int y = _schema.GetYear(daysSinceEpoch, out int doy);
        if (doy < Yedoy.MinDayOfYear || doy > Yedoy.MaxDayOfYear) Throw.DayOfYearOutOfRange(doy);
        return new Yedoy(y, doy);
    }

    /// <inheritdoc />
    [Pure]
    public Yedoy GetOrdinalParts(int y, int m, int d)
    {
        int doy = _schema.GetDayOfYear(y, m, d);
        if (doy < Yedoy.MinDayOfYear || doy > Yedoy.MaxDayOfYear) Throw.DayOfYearOutOfRange(doy);
        return new Yedoy(y, doy);
    }

    /// <inheritdoc />
    [Pure]
    public Yemoda GetDateParts(int y, int doy)
    {
        int m = _schema.GetMonth(y, doy, out int d);
        if (m < Yemoda.MinMonth || m > Yemoda.MaxMonth) Throw.MonthOutOfRange(m);
        if (d < Yemoda.MinDay || d > Yemoda.MaxDay) Throw.DayOutOfRange(d);
        return new Yemoda(y, m, d);
    }
}

internal partial class SchemaAdapterSlim // Dates in a given year or month
{
    /// <inheritdoc />
    [Pure]
    public Yemo GetMonthPartsAtStartOfYear(int y) => Yemo.AtStartOfYear(y);

    /// <inheritdoc />
    [Pure]
    public Yemoda GetDatePartsAtStartOfYear(int y) => Yemoda.AtStartOfYear(y);

    /// <inheritdoc />
    [Pure]
    public Yedoy GetOrdinalPartsAtStartOfYear(int y) => Yedoy.AtStartOfYear(y);

    /// <inheritdoc />
    [Pure]
    public Yemo GetMonthPartsAtEndOfYear(int y)
    {
        int m = _schema.CountMonthsInYear(y);
        if (m < Yemo.MinMonth || m > Yemo.MaxMonth) Throw.MonthOutOfRange(m);
        return new Yemo(y, m);
    }

    /// <inheritdoc />
    [Pure]
    public Yemoda GetDatePartsAtEndOfYear(int y)
    {
        int m = _schema.CountMonthsInYear(y);
        if (m < Yemoda.MinMonth || m > Yemoda.MaxMonth) Throw.MonthOutOfRange(m);
        int d = _schema.CountDaysInMonth(y, m);
        if (d < Yemoda.MinDay || d > Yemoda.MaxDay) Throw.DayOutOfRange(d);
        return new Yemoda(y, m, d);
    }

    /// <inheritdoc />
    [Pure]
    public Yedoy GetOrdinalPartsAtEndOfYear(int y)
    {
        int doy = _schema.CountDaysInYear(y);
        if (doy < Yedoy.MinDayOfYear || doy > Yedoy.MaxDayOfYear) Throw.DayOfYearOutOfRange(doy);
        return new Yedoy(y, doy);
    }

    /// <inheritdoc />
    [Pure]
    public Yemoda GetDatePartsAtStartOfMonth(int y, int m)
    {
        if (y < Yemoda.MinMonth || m > Yemoda.MaxMonth) Throw.MonthOutOfRange(m);
        return Yemoda.AtStartOfMonth(y, m);
    }

    /// <inheritdoc />
    [Pure]
    public Yedoy GetOrdinalPartsAtStartOfMonth(int y, int m)
    {
        int doy = _schema.GetDayOfYear(y, m, 1);
        if (doy < Yedoy.MinDayOfYear || doy > Yedoy.MaxDayOfYear) Throw.DayOfYearOutOfRange(doy);
        return new Yedoy(y, doy);
    }

    /// <inheritdoc />
    [Pure]
    public Yemoda GetDatePartsAtEndOfMonth(int y, int m)
    {
        int d = _schema.CountDaysInMonth(y, m);
        if (d < Yemoda.MinDay || d > Yemoda.MaxDay) Throw.DayOutOfRange(d);
        return new Yemoda(y, m, d);
    }

    /// <inheritdoc />
    [Pure]
    public Yedoy GetOrdinalPartsAtEndOfMonth(int y, int m)
    {
        int d = _schema.CountDaysInMonth(y, m);
        int doy = _schema.GetDayOfYear(y, m, d);
        if (doy < Yedoy.MinDayOfYear || doy > Yedoy.MaxDayOfYear) Throw.DayOfYearOutOfRange(doy);
        return new Yedoy(y, doy);
    }
}
