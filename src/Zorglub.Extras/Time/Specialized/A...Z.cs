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
    internal ArmenianCalendar(Egyptian12Schema schema)
        : base("Armenian", StandardScope.Create(schema, DayZero.Armenian)) { }

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
    internal Armenian13Calendar(Egyptian13Schema schema)
        : base("Armenian", StandardScope.Create(schema, DayZero.Armenian))
    {
        VirtualMonth = schema.VirtualMonth;
    }

    /// <inheritdoc/>
    public int MonthsInYear => Egyptian13Schema.MonthsPerYear;

    /// <inheritdoc/>
    public int VirtualMonth { get; }
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
    internal CopticCalendar(Coptic12Schema schema)
        : base("Coptic", StandardScope.Create(schema, DayZero.Coptic)) { }

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
    internal Coptic13Calendar(Coptic13Schema schema)
        : base("Coptic", StandardScope.Create(schema, DayZero.Coptic))
    {
        VirtualMonth = schema.VirtualMonth;
    }

    /// <inheritdoc/>
    public int MonthsInYear => Coptic13Schema.MonthsPerYear;

    /// <inheritdoc/>
    public int VirtualMonth { get; }
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
    internal EthiopicCalendar(Coptic12Schema schema)
        : base("Ethiopic", StandardScope.Create(schema, DayZero.Ethiopic)) { }

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
    internal Ethiopic13Calendar(Coptic13Schema schema)
        : base("Ethiopic", StandardScope.Create(schema, DayZero.Ethiopic))
    {
        VirtualMonth = schema.VirtualMonth;
    }

    /// <inheritdoc/>
    public int MonthsInYear => Coptic13Schema.MonthsPerYear;

    /// <inheritdoc/>
    public int VirtualMonth { get; }
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
    internal TabularIslamicCalendar(TabularIslamicSchema schema)
        : base("Tabular Islamic", StandardScope.Create(schema, DayZero.TabularIslamic)) { }

    /// <inheritdoc/>
    public int MonthsInYear => TabularIslamicSchema.MonthsPerYear;
}

public partial class WorldCalendar : IRegularFeaturette
{
    internal WorldCalendar(WorldSchema schema)
        : base("World", StandardScope.Create(schema, DayZero.SundayBeforeGregorian)) { }

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
    internal ZoroastrianCalendar(Egyptian12Schema schema)
        : base("Zoroastrian", StandardScope.Create(schema, DayZero.Zoroastrian)) { }

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
    internal Zoroastrian13Calendar(Egyptian13Schema schema)
        : base("Zoroastrian", StandardScope.Create(schema, DayZero.Zoroastrian))
    {
        VirtualMonth = schema.VirtualMonth;
    }

    /// <inheritdoc/>
    public int MonthsInYear => Egyptian13Schema.MonthsPerYear;

    /// <inheritdoc/>
    public int VirtualMonth { get; }
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
