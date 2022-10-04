// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// TODO(api): clocks, non-standard math.
// XML doc, explain the scope for all calendars.
// CountDaysSince(other) checked context or not? ensure that we test that?

public partial class ArmenianCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(Egyptian12Schema schema) =>
        StandardScope.Create(schema, DayZero.Armenian);

    /// <inheritdoc />
    public int MonthsInYear => Egyptian12Schema.MonthsPerYear;
}

public partial struct ArmenianDate : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

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

public partial struct Armenian13Date : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public partial class CopticCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(Coptic12Schema schema) =>
        StandardScope.Create(schema, DayZero.Coptic);

    /// <inheritdoc/>
    public int MonthsInYear => Coptic12Schema.MonthsPerYear;
}

public partial struct CopticDate : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

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

public partial struct Coptic13Date : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public partial class EthiopicCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(Coptic12Schema schema) =>
        StandardScope.Create(schema, DayZero.Ethiopic);

    /// <inheritdoc/>
    public int MonthsInYear => Coptic12Schema.MonthsPerYear;
}

public partial struct EthiopicDate : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

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

public partial struct Ethiopic13Date : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public partial class TabularIslamicCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(TabularIslamicSchema schema) =>
        StandardScope.Create(schema, DayZero.TabularIslamic);

    /// <inheritdoc/>
    public int MonthsInYear => TabularIslamicSchema.MonthsPerYear;
}

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

public partial struct WorldDate : IBlankDay
{
    /// <inheritdoc />
    public bool IsBlank
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return s_Schema.IsBlankDay(y, m, d);
        }
    }
}

public partial class ZoroastrianCalendar : IRegularFeaturette
{
    private static partial MinMaxYearScope GetScope(Egyptian12Schema schema) =>
        StandardScope.Create(schema, DayZero.Zoroastrian);

    /// <inheritdoc/>
    public int MonthsInYear => Egyptian12Schema.MonthsPerYear;
}

public partial struct ZoroastrianDate : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

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

public partial struct Zoroastrian13Date : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}
