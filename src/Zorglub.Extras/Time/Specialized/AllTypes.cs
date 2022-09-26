// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

#region Armenian

/// <summary>Represents the Armenian calendar.
/// <para>This class cannot be inherited.</para></summary>
public sealed partial class ArmenianCalendar :
    SpecialCalendar<ArmenianDate>,
    IRegularFeaturette
{
    /// <summary>Initializes a new instance of the <see cref="ArmenianCalendar"/> class.</summary>
    public ArmenianCalendar() : this(new Egyptian12Schema()) { }

    /// <summary>Initializes a new instance of the <see cref="ArmenianCalendar"/> class.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
    internal ArmenianCalendar(Egyptian12Schema schema)
        : base("Armenian", StandardScope.Create(schema, DayZero.Armenian))
    {
        MonthsInYear = schema.MonthsInYear;
    }

    /// <inheritdoc />
    public int MonthsInYear { get; }
}

/// <summary>Represents the Armenian date.
/// <para><see cref="ArmenianDate"/> is an immutable struct.</para></summary>
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

#endregion
#region Armenian13

/// <summary>Represents the Armenian calendar.
/// <para>This class cannot be inherited.</para></summary>
public sealed partial class Armenian13Calendar :
    SpecialCalendar<Armenian13Date>,
    IRegularFeaturette,
    IVirtualMonthFeaturette
{
    /// <summary>Initializes a new instance of the <see cref="Armenian13Calendar"/> class.</summary>
    public Armenian13Calendar() : this(new Egyptian13Schema()) { }

    /// <summary>Initializes a new instance of the <see cref="Armenian13Calendar"/> class.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
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

/// <summary>Represents the Armenian date.
/// <para><see cref="Armenian13Date"/> is an immutable struct.</para></summary>
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

#endregion
#region Coptic

/// <summary>Represents the Coptic calendar.
/// <para>This class cannot be inherited.</para></summary>
public sealed partial class CopticCalendar :
    SpecialCalendar<CopticDate>,
    IRegularFeaturette
{
    /// <summary>Initializes a new instance of the <see cref="CopticCalendar"/> class.</summary>
    public CopticCalendar() : this(new Coptic12Schema()) { }

    /// <summary>Initializes a new instance of the <see cref="CopticCalendar"/> class.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
    internal CopticCalendar(Coptic12Schema schema)
        : base("Coptic", StandardScope.Create(schema, DayZero.Coptic))
    {
        MonthsInYear = schema.MonthsInYear;
    }

    /// <inheritdoc/>
    public int MonthsInYear { get; }
}

/// <summary>Represents the Coptic date.
/// <para><see cref="CopticDate"/> is an immutable struct.</para></summary>
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

#endregion
#region Coptic13

/// <summary>Represents the Coptic calendar.
/// <para>This class cannot be inherited.</para></summary>
public sealed partial class Coptic13Calendar :
    SpecialCalendar<Coptic13Date>,
    IRegularFeaturette,
    IVirtualMonthFeaturette
{
    /// <summary>Initializes a new instance of the <see cref="Coptic13Calendar"/> class.</summary>
    public Coptic13Calendar() : this(new Coptic13Schema()) { }

    /// <summary>Initializes a new instance of the <see cref="Coptic13Calendar"/> class.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
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

/// <summary>Represents the Coptic date.
/// <para><see cref="Coptic13Date"/> is an immutable struct.</para></summary>
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

#endregion
