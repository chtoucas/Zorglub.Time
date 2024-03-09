// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz.Obsolete;

using Zorglub.Time.Core;

/// <summary>
/// Provides methods you can use to create new calendrical parts.
/// </summary>
internal sealed partial class SchemaAdapterChecked : ISchemaAdapter
{
    /// <summary>
    /// Represents the schema.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly ICalendricalSchema _schema;

    /// <summary>
    /// Initializes a new instance of the <see cref="SchemaAdapterChecked"/> class.
    /// </summary>
    public SchemaAdapterChecked(ICalendricalSchema schema)
    {
        Debug.Assert(schema != null);

        _schema = schema;
    }
}

internal partial class SchemaAdapterChecked // Conversions
{
    /// <inheritdoc />
    [Pure]
    public Yemo GetMonthParts(int monthsSinceEpoch)
    {
        _schema.GetMonthParts(monthsSinceEpoch, out int y, out int m);
        return Yemo.Create(y, m);
    }

    /// <inheritdoc />
    [Pure]
    public Yemoda GetDateParts(int daysSinceEpoch)
    {
        _schema.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        return Yemoda.Create(y, m, d);
    }

    /// <inheritdoc />
    [Pure]
    public Yedoy GetOrdinalParts(int daysSinceEpoch)
    {
        int y = _schema.GetYear(daysSinceEpoch, out int doy);
        return Yedoy.Create(y, doy);
    }

    /// <inheritdoc />
    [Pure]
    public Yedoy GetOrdinalParts(int y, int m, int d)
    {
        int doy = _schema.GetDayOfYear(y, m, d);
        return Yedoy.Create(y, doy);
    }

    /// <inheritdoc />
    [Pure]
    public Yemoda GetDateParts(int y, int doy)
    {
        int m = _schema.GetMonth(y, doy, out int d);
        return Yemoda.Create(y, m, d);
    }
}

internal partial class SchemaAdapterChecked // Dates in a given year or month
{
    /// <inheritdoc />
    [Pure]
    public Yemo GetMonthPartsAtStartOfYear(int y)
    {
        if (y < Yemoda.MinYear || y > Yemoda.MaxYear) Throw.YearOutOfRange(y);
        return Yemo.AtStartOfYear(y);
    }

    /// <inheritdoc />
    [Pure]
    public Yemoda GetDatePartsAtStartOfYear(int y)
    {
        if (y < Yemoda.MinYear || y > Yemoda.MaxYear) Throw.YearOutOfRange(y);
        return Yemoda.AtStartOfYear(y);
    }

    /// <inheritdoc />
    [Pure]
    public Yedoy GetOrdinalPartsAtStartOfYear(int y)
    {
        if (y < Yemoda.MinYear || y > Yemoda.MaxYear) Throw.YearOutOfRange(y);
        return Yedoy.AtStartOfYear(y);
    }

    /// <inheritdoc />
    [Pure]
    public Yemo GetMonthPartsAtEndOfYear(int y)
    {
        int m = _schema.CountMonthsInYear(y);
        return Yemo.Create(y, m);
    }

    /// <inheritdoc />
    [Pure]
    public Yemoda GetDatePartsAtEndOfYear(int y)
    {
        int m = _schema.CountMonthsInYear(y);
        int d = _schema.CountDaysInMonth(y, m);
        return Yemoda.Create(y, m, d);
    }

    /// <inheritdoc />
    [Pure]
    public Yedoy GetOrdinalPartsAtEndOfYear(int y)
    {
        int doy = _schema.CountDaysInYear(y);
        return Yedoy.Create(y, doy);
    }

    /// <inheritdoc />
    [Pure]
    public Yemoda GetDatePartsAtStartOfMonth(int y, int m)
    {
        if (y < Yemoda.MinYear || y > Yemoda.MaxYear) Throw.YearOutOfRange(y);
        if (y < Yemoda.MinMonth || m > Yemoda.MaxMonth) Throw.MonthOutOfRange(m);
        return Yemoda.AtStartOfMonth(y, m);
    }

    /// <inheritdoc />
    [Pure]
    public Yedoy GetOrdinalPartsAtStartOfMonth(int y, int m)
    {
        int doy = _schema.GetDayOfYear(y, m, 1);
        return Yedoy.Create(y, doy);
    }

    /// <inheritdoc />
    [Pure]
    public Yemoda GetDatePartsAtEndOfMonth(int y, int m)
    {
        int d = _schema.CountDaysInMonth(y, m);
        return Yemoda.Create(y, m, d);
    }

    /// <inheritdoc />
    [Pure]
    public Yedoy GetOrdinalPartsAtEndOfMonth(int y, int m)
    {
        int d = _schema.CountDaysInMonth(y, m);
        int doy = _schema.GetDayOfYear(y, m, d);
        return Yedoy.Create(y, doy);
    }
}
