// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;

/// <summary>
/// Provides methods to obtain new <see cref="OrdinalDate"/> instances in a given year or month.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class OrdinalDateProviders : IDateProviders<OrdinalDate>
{
    [ExcludeFromCodeCoverage(Justification = "Pseudo-static class implementing a static-only interface.")]
    private OrdinalDateProviders() { }
}

public partial class OrdinalDateProviders // CalendarYear
{
    /// <inheritdoc/>
    /// <remarks>
    /// See also <seealso cref="CalendarYear.ToRange()"/>.
    /// </remarks>
    [Pure]
    static Range<OrdinalDate> IDateProviders<OrdinalDate>.ConvertToRange(CalendarYear year) =>
        year.ToRange();

    /// <inheritdoc/>
    /// <remarks>
    /// See also <seealso cref="CalendarYear.GetAllDays()"/>.
    /// </remarks>
    [Pure]
    static IEnumerable<OrdinalDate> IDateProviders<OrdinalDate>.GetDaysInYear(CalendarYear year) =>
        year.GetAllDays();

    /// <inheritdoc/>
    /// <remarks>
    /// See also <seealso cref="CalendarYear.FirstDay"/>.
    /// </remarks>
    [Pure]
    static OrdinalDate IDateProviders<OrdinalDate>.GetStartOfYear(CalendarYear year) =>
        year.FirstDay;

    /// <inheritdoc/>
    /// <remarks>
    /// See also <seealso cref="CalendarYear.GetDayOfYear(int)"/>.
    /// </remarks>
    [Pure]
    static OrdinalDate IDateProviders<OrdinalDate>.GetDayOfYear(CalendarYear year, int dayOfYear) =>
        year.GetDayOfYear(dayOfYear);

    /// <inheritdoc/>
    /// <remarks>
    /// See also <seealso cref="CalendarYear.LastDay"/>.
    /// </remarks>
    [Pure]
    static OrdinalDate IDateProviders<OrdinalDate>.GetEndOfYear(CalendarYear year) =>
        year.LastDay;
}

public partial class OrdinalDateProviders // CalendarMonth
{
    /// <inheritdoc/>
    [Pure]
    public static OrdinalDate GetStartOfYear(CalendarMonth month) =>
        month.CalendarYear.FirstDay;

    /// <inheritdoc/>
    [Pure]
    public static OrdinalDate GetEndOfYear(CalendarMonth month) =>
        month.CalendarYear.LastDay;

    /// <inheritdoc/>
    [Pure]
    public static Range<OrdinalDate> ConvertToRange(CalendarMonth month) =>
        Range.Create(GetStartOfMonth(month), GetEndOfMonth(month));

    /// <inheritdoc/>
    [Pure]
    public static IEnumerable<OrdinalDate> GetDaysInMonth(CalendarMonth month)
    {
        var sch = month.Calendar.Schema;
        month.Parts.Unpack(out int y, out int m);
        int startOfMonth = sch.CountDaysInYearBeforeMonth(y, m);
        int daysInMonth = sch.CountDaysInMonth(y, m);

        for (int d = 1; d <= daysInMonth; d++)
        {
            yield return new OrdinalDate(y, startOfMonth + d, month.Cuid);
        }
    }

    /// <inheritdoc/>
    [Pure]
    public static OrdinalDate GetStartOfMonth(CalendarMonth month)
    {
        month.Parts.Unpack(out int y, out int m);
        ref readonly var chr = ref month.CalendarRef;
        var ydoy = chr.Schema.GetOrdinalPartsAtStartOfMonth(y, m);
        return new OrdinalDate(ydoy, month.Cuid);
    }

    /// <inheritdoc/>
    [Pure]
    public static OrdinalDate GetDayOfMonth(CalendarMonth month, int dayOfMonth)
    {
        month.Parts.Unpack(out int y, out int m);
        ref readonly var chr = ref month.CalendarRef;
        chr.ValidateDayOfMonth(y, m, dayOfMonth);
        int doy = chr.Schema.GetDayOfYear(y, m, dayOfMonth);
        return new OrdinalDate(new Yedoy(y, doy), month.Cuid);
    }

    /// <inheritdoc/>
    [Pure]
    public static OrdinalDate GetEndOfMonth(CalendarMonth month)
    {
        month.Parts.Unpack(out int y, out int m);
        ref readonly var chr = ref month.CalendarRef;
        var ydoy = chr.Schema.GetOrdinalPartsAtEndOfMonth(y, m);
        return new OrdinalDate(ydoy, month.Cuid);
    }
}
