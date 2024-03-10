// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Specialized;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// TODO(api): clocks, non-standard math.
// Date.CountDaysSince(other) checked context or not? ensure that we test that?

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999] of years.</remarks>
public partial class CivilCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(CivilSchema schema) =>
        StandardScope.Create(schema, DayZero.NewStyle);

    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;
}

/// <remarks>This calendar is <i>proleptic</i>. It supports <i>all</i> dates within the range
/// [-999_998..999_999] of years.</remarks>
public partial class GregorianCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(GregorianSchema schema) =>
        MinMaxYearScope.CreateMaximal(schema, DayZero.NewStyle);

    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;
}

/// <remarks>This calendar is <i>proleptic</i>. It supports <i>all</i> dates within the range
/// [-999_998..999_999] of years.</remarks>
public partial class JulianCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(JulianSchema schema) =>
        MinMaxYearScope.CreateMaximal(schema, DayZero.OldStyle);

    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999] of years.</remarks>
public partial class ArmenianCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(Egyptian12Schema schema) =>
        StandardScope.Create(schema, DayZero.Armenian);

    /// <inheritdoc />
    public int MonthsInYear => Egyptian12Schema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999] of years.</remarks>
public partial class Armenian13Calendar : IRegularFeaturette, IVirtualMonthFeaturette
{
    private static partial MinMaxYearScope GetScope(Egyptian13Schema schema) =>
        StandardScope.Create(schema, DayZero.Armenian);

    partial void OnInitializing(Egyptian13Schema schema) => VirtualMonth = schema.VirtualMonth;

    /// <inheritdoc/>
    public int MonthsInYear => Egyptian13Schema.MonthsPerYear;

    /// <inheritdoc/>
    public int VirtualMonth { get; private set; }
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999] of years.</remarks>
public partial class CopticCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(Coptic12Schema schema) =>
        StandardScope.Create(schema, DayZero.Coptic);

    /// <inheritdoc/>
    public int MonthsInYear => Coptic12Schema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999] of years.</remarks>
public partial class Coptic13Calendar : IRegularFeaturette, IVirtualMonthFeaturette
{
    private static partial MinMaxYearScope GetScope(Coptic13Schema schema) =>
        StandardScope.Create(schema, DayZero.Coptic);

    partial void OnInitializing(Coptic13Schema schema) => VirtualMonth = schema.VirtualMonth;

    /// <inheritdoc/>
    public int MonthsInYear => Coptic13Schema.MonthsPerYear;

    /// <inheritdoc/>
    public int VirtualMonth { get; private set; }
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999] of years.</remarks>
public partial class EthiopicCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(Coptic12Schema schema) =>
        StandardScope.Create(schema, DayZero.Ethiopic);

    /// <inheritdoc/>
    public int MonthsInYear => Coptic12Schema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999] of years.</remarks>
public partial class Ethiopic13Calendar : IRegularFeaturette, IVirtualMonthFeaturette
{
    private static partial MinMaxYearScope GetScope(Coptic13Schema schema) =>
        StandardScope.Create(schema, DayZero.Ethiopic);

    partial void OnInitializing(Coptic13Schema schema) => VirtualMonth = schema.VirtualMonth;

    /// <inheritdoc/>
    public int MonthsInYear => Coptic13Schema.MonthsPerYear;

    /// <inheritdoc/>
    public int VirtualMonth { get; private set; }
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999] of years.</remarks>
public partial class TabularIslamicCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(TabularIslamicSchema schema) =>
        StandardScope.Create(schema, DayZero.TabularIslamic);

    /// <inheritdoc/>
    public int MonthsInYear => TabularIslamicSchema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999] of years.</remarks>
public partial class WorldCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(WorldSchema schema) =>
        StandardScope.Create(schema, DayZero.SundayBeforeGregorian);

    /// <inheritdoc/>
    public int MonthsInYear => WorldSchema.MonthsPerYear;

    /// <summary>Obtains the genuine number of days in a month (excluding the blank days that are
    /// formally outside any month).
    /// <para>See also <seealso cref="MinMaxYearBasicCalendar.CountDaysInMonth(int, int)"/>.</para>
    /// </summary>
    [Pure]
    public int CountDaysInWorldMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return WorldSchema.CountDaysInWorldMonth(month);
    }
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999] of years.</remarks>
public partial class ZoroastrianCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(Egyptian12Schema schema) =>
        StandardScope.Create(schema, DayZero.Zoroastrian);

    /// <inheritdoc/>
    public int MonthsInYear => Egyptian12Schema.MonthsPerYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999] of years.</remarks>
public partial class Zoroastrian13Calendar : IRegularFeaturette, IVirtualMonthFeaturette
{
    private static partial MinMaxYearScope GetScope(Egyptian13Schema schema) =>
        StandardScope.Create(schema, DayZero.Zoroastrian);

    partial void OnInitializing(Egyptian13Schema schema) => VirtualMonth = schema.VirtualMonth;

    /// <inheritdoc/>
    public int MonthsInYear => Egyptian13Schema.MonthsPerYear;

    /// <inheritdoc/>
    public int VirtualMonth { get; private set; }
}
