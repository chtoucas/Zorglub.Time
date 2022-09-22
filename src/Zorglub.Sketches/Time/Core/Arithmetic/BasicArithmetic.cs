// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic;

using Zorglub.Time.Core.Validation;

/// <summary>
/// Provides a reference implementation for <see cref="PartsArithmetic"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class BasicArithmetic : PartsArithmetic
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BasicArithmetic"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="segment"/> is null.</exception>
    public BasicArithmetic(CalendricalSegment segment) : base(segment) { }
}

public partial class BasicArithmetic // Operations on DateParts
{
    /// <inheritdoc />
    [Pure]
    public sealed override DateParts AddDays(DateParts parts, int days)
    {
        var (y, m, d) = parts;
        int daysSinceEpoch = checked(Schema.CountDaysSinceEpoch(y, m, d) + days);
        DaysValidator.CheckOverflow(daysSinceEpoch);

        return PartsAdapter.GetDateParts(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override DateParts NextDay(DateParts parts) => AddDays(parts, 1);

    /// <inheritdoc />
    [Pure]
    public sealed override DateParts PreviousDay(DateParts parts) => AddDays(parts, -1);
}

public partial class BasicArithmetic // Operations on OrdinalParts
{
    /// <inheritdoc />
    [Pure]
    public sealed override OrdinalParts AddDays(OrdinalParts parts, int days)
    {
        var (y, doy) = parts;
        int daysSinceEpoch = checked(Schema.CountDaysSinceEpoch(y, doy) + days);
        DaysValidator.CheckOverflow(daysSinceEpoch);

        return PartsAdapter.GetOrdinalParts(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override OrdinalParts NextDay(OrdinalParts parts) => AddDays(parts, 1);

    /// <inheritdoc />
    [Pure]
    public sealed override OrdinalParts PreviousDay(OrdinalParts parts) => AddDays(parts, -1);
}

public partial class BasicArithmetic // Operations on MonthParts
{
    /// <inheritdoc />
    [Pure]
    public sealed override MonthParts AddMonths(MonthParts parts, int months)
    {
        var (y, m) = parts;
        int monthsSinceEpoch = checked(Schema.CountMonthsSinceEpoch(y, m) + months);
        MonthsValidator.CheckOverflow(monthsSinceEpoch);

        return PartsAdapter.GetMonthParts(monthsSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override MonthParts NextMonth(MonthParts parts) => AddMonths(parts, 1);

    /// <inheritdoc />
    [Pure]
    public sealed override MonthParts PreviousMonth(MonthParts parts) => AddMonths(parts, -1);
}
