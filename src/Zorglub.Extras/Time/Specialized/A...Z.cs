// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// REVIEW(perf): custom methods on calendars and dates.
// - IRegularFeaturette       -> use MonthsPerYear
// - IVirtualMonthFeaturette  -> use a constant

public sealed partial class ArmenianCalendar : IRegularFeaturette
{
    internal ArmenianCalendar(Egyptian12Schema schema)
        : base("Armenian", StandardScope.Create(schema, DayZero.Armenian))
    {
        MonthsInYear = schema.MonthsInYear;
    }

    /// <inheritdoc />
    public int MonthsInYear { get; }
}

public readonly partial struct ArmenianDate : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public sealed partial class Armenian13Calendar : IRegularFeaturette, IVirtualMonthFeaturette
{
    internal Armenian13Calendar(Egyptian13Schema schema)
        : base("Armenian", StandardScope.Create(schema, DayZero.Armenian))
    {
        MonthsInYear = schema.MonthsInYear;
        VirtualMonth = schema.VirtualMonth;
    }

    /// <inheritdoc/>
    public int MonthsInYear { get; }
    /// <inheritdoc/>
    public int VirtualMonth { get; }
}

public readonly partial struct Armenian13Date : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public sealed partial class CopticCalendar : IRegularFeaturette
{
    internal CopticCalendar(Coptic12Schema schema)
        : base("Coptic", StandardScope.Create(schema, DayZero.Coptic))
    {
        MonthsInYear = schema.MonthsInYear;
    }

    /// <inheritdoc/>
    public int MonthsInYear { get; }
}

public readonly partial struct CopticDate : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public sealed partial class Coptic13Calendar : IRegularFeaturette, IVirtualMonthFeaturette
{
    internal Coptic13Calendar(Coptic13Schema schema)
        : base("Coptic", StandardScope.Create(schema, DayZero.Coptic))
    {
        MonthsInYear = schema.MonthsInYear;
        VirtualMonth = schema.VirtualMonth;
    }

    /// <inheritdoc/>
    public int MonthsInYear { get; }
    /// <inheritdoc/>
    public int VirtualMonth { get; }
}

public readonly partial struct Coptic13Date : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public sealed partial class EthiopicCalendar : IRegularFeaturette
{
    internal EthiopicCalendar(Coptic12Schema schema)
        : base("Ethiopic", StandardScope.Create(schema, DayZero.Ethiopic))
    {
        MonthsInYear = schema.MonthsInYear;
    }

    /// <inheritdoc/>
    public int MonthsInYear { get; }
}

public readonly partial struct EthiopicDate : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public sealed partial class Ethiopic13Calendar : IRegularFeaturette, IVirtualMonthFeaturette
{
    internal Ethiopic13Calendar(Coptic13Schema schema)
        : base("Ethiopic", StandardScope.Create(schema, DayZero.Ethiopic))
    {
        MonthsInYear = schema.MonthsInYear;
        VirtualMonth = schema.VirtualMonth;
    }

    /// <inheritdoc/>
    public int MonthsInYear { get; }
    /// <inheritdoc/>
    public int VirtualMonth { get; }
}

public readonly partial struct Ethiopic13Date : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public sealed partial class TabularIslamicCalendar : IRegularFeaturette
{
    internal TabularIslamicCalendar(TabularIslamicSchema schema)
        : base("Tabular Islamic", StandardScope.Create(schema, DayZero.TabularIslamic))
    {
        MonthsInYear = schema.MonthsInYear;
    }

    /// <inheritdoc/>
    public int MonthsInYear { get; }
}

public sealed partial class WorldCalendar : IRegularFeaturette
{
    internal WorldCalendar(WorldSchema schema)
        : base("World", StandardScope.Create(schema, DayZero.SundayBeforeGregorian))
    {
        MonthsInYear = schema.MonthsInYear;
    }

    /// <inheritdoc/>
    public int MonthsInYear { get; }

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

public readonly partial struct WorldDate : IBlankDay
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

public sealed partial class ZoroastrianCalendar : IRegularFeaturette
{
    internal ZoroastrianCalendar(Egyptian12Schema schema)
        : base("Zoroastrian", StandardScope.Create(schema, DayZero.Zoroastrian))
    {
        MonthsInYear = schema.MonthsInYear;
    }

    /// <inheritdoc/>
    public int MonthsInYear { get; }
}

public readonly partial struct ZoroastrianDate : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public sealed partial class Zoroastrian13Calendar : IRegularFeaturette,
    IVirtualMonthFeaturette
{
    internal Zoroastrian13Calendar(Egyptian13Schema schema)
        : base("Zoroastrian", StandardScope.Create(schema, DayZero.Zoroastrian))
    {
        MonthsInYear = schema.MonthsInYear;
        VirtualMonth = schema.VirtualMonth;
    }

    /// <inheritdoc/>
    public int MonthsInYear { get; }
    /// <inheritdoc/>
    public int VirtualMonth { get; }
}

public readonly partial struct Zoroastrian13Date : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}
