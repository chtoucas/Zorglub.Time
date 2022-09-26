// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

public sealed partial class ArmenianCalendar :
    SpecialCalendar<ArmenianDate>,
    IRegularFeaturette
{
    /// <summary>Initializes a new instance of the <see cref="ArmenianCalendar"/> class.</summary>
    public ArmenianCalendar() : this(new Egyptian12Schema()) { }

    internal ArmenianCalendar(Egyptian12Schema schema)
        : base("Armenian", StandardScope.Create(schema, DayZero.Armenian))
    {
        MonthsInYear = schema.MonthsInYear;
    }

    /// <inheritdoc />
    public int MonthsInYear { get; }
}

public readonly partial struct ArmenianDate :
    IDate<ArmenianDate, ArmenianCalendar>,
    IAdjustable<ArmenianDate>,
    IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public sealed partial class Armenian13Calendar :
    SpecialCalendar<Armenian13Date>,
    IRegularFeaturette,
    IVirtualMonthFeaturette
{
    /// <summary>Initializes a new instance of the <see cref="Armenian13Calendar"/> class.</summary>
    public Armenian13Calendar() : this(new Egyptian13Schema()) { }

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

public readonly partial struct Armenian13Date :
    IDate<Armenian13Date, Armenian13Calendar>,
    IAdjustable<Armenian13Date>,
    IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public sealed partial class CopticCalendar :
    SpecialCalendar<CopticDate>,
    IRegularFeaturette
{
    /// <summary>Initializes a new instance of the <see cref="CopticCalendar"/> class.</summary>
    public CopticCalendar() : this(new Coptic12Schema()) { }

    internal CopticCalendar(Coptic12Schema schema)
        : base("Coptic", StandardScope.Create(schema, DayZero.Coptic))
    {
        MonthsInYear = schema.MonthsInYear;
    }

    /// <inheritdoc/>
    public int MonthsInYear { get; }
}

public readonly partial struct CopticDate :
    IDate<CopticDate, CopticCalendar>,
    IAdjustable<CopticDate>,
    IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public sealed partial class Coptic13Calendar :
    SpecialCalendar<Coptic13Date>,
    IRegularFeaturette,
    IVirtualMonthFeaturette
{
    /// <summary>Initializes a new instance of the <see cref="Coptic13Calendar"/> class.</summary>
    public Coptic13Calendar() : this(new Coptic13Schema()) { }

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

public readonly partial struct Coptic13Date :
    IDate<Coptic13Date, Coptic13Calendar>,
    IAdjustable<Coptic13Date>,
    IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public sealed partial class EthiopicCalendar :
    SpecialCalendar<EthiopicDate>,
    IRegularFeaturette
{
    /// <summary>Initializes a new instance of the <see cref="EthiopicCalendar"/> class.</summary>
    public EthiopicCalendar() : this(new Coptic12Schema()) { }

    internal EthiopicCalendar(Coptic12Schema schema)
        : base("Ethiopic", StandardScope.Create(schema, DayZero.Ethiopic))
    {
        MonthsInYear = schema.MonthsInYear;
    }

    /// <inheritdoc/>
    public int MonthsInYear { get; }
}

public readonly partial struct EthiopicDate :
    IDate<EthiopicDate, EthiopicCalendar>,
    IAdjustable<EthiopicDate>,
    IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public sealed partial class Ethiopic13Calendar :
    SpecialCalendar<Ethiopic13Date>,
    IRegularFeaturette,
    IVirtualMonthFeaturette
{
    /// <summary>Initializes a new instance of the <see cref="Ethiopic13Calendar"/> class.</summary>
    public Ethiopic13Calendar() : this(new Coptic13Schema()) { }

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

public readonly partial struct Ethiopic13Date :
    IDate<Ethiopic13Date, Ethiopic13Calendar>,
    IAdjustable<Ethiopic13Date>,
    IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public sealed partial class TabularIslamicCalendar :
    SpecialCalendar<TabularIslamicDate>,
    IRegularFeaturette
{
    /// <summary>Initializes a new instance of the <see cref="TabularIslamicCalendar"/> class.
    /// </summary>
    public TabularIslamicCalendar() : this(new TabularIslamicSchema()) { }

    internal TabularIslamicCalendar(TabularIslamicSchema schema)
        : base("Tabular Islamic", StandardScope.Create(schema, DayZero.TabularIslamic))
    {
        MonthsInYear = schema.MonthsInYear;
    }

    /// <inheritdoc/>
    public int MonthsInYear { get; }
}

public readonly partial struct TabularIslamicDate :
    IDate<TabularIslamicDate, TabularIslamicCalendar>,
    IAdjustable<TabularIslamicDate>
{ }

public sealed partial class WorldCalendar :
    SpecialCalendar<WorldDate>,
    IRegularFeaturette
{
    /// <summary>Initializes a new instance of the <see cref="WorldCalendar"/> class.</summary>
    public WorldCalendar() : this(new WorldSchema()) { }

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

public readonly partial struct WorldDate :
    IDate<WorldDate, WorldCalendar>,
    IAdjustable<WorldDate>,
    IBlankDay
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

public sealed partial class ZoroastrianCalendar :
    SpecialCalendar<ZoroastrianDate>,
    IRegularFeaturette
{
    /// <summary>Initializes a new instance of the <see cref="ZoroastrianCalendar"/> class.</summary>
    public ZoroastrianCalendar() : this(new Egyptian12Schema()) { }

    internal ZoroastrianCalendar(Egyptian12Schema schema)
        : base("Zoroastrian", StandardScope.Create(schema, DayZero.Zoroastrian))
    {
        MonthsInYear = schema.MonthsInYear;
    }

    /// <inheritdoc/>
    public int MonthsInYear { get; }
}

public readonly partial struct ZoroastrianDate :
    IDate<ZoroastrianDate, ZoroastrianCalendar>,
    IAdjustable<ZoroastrianDate>,
    IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public sealed partial class Zoroastrian13Calendar :
    SpecialCalendar<Zoroastrian13Date>,
    IRegularFeaturette,
    IVirtualMonthFeaturette
{
    /// <summary>Initializes a new instance of the <see cref="Zoroastrian13Calendar"/> class.
    /// </summary>
    public Zoroastrian13Calendar() : this(new Egyptian13Schema()) { }

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

public readonly partial struct Zoroastrian13Date :
    IDate<Zoroastrian13Date, Zoroastrian13Calendar>,
    IAdjustable<Zoroastrian13Date>,
    IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return s_Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}
