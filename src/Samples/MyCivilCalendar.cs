// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

using static Zorglub.Time.Extensions.Unboxing;

public sealed class MyCivilCalendar : MinMaxYearCalendar<MyCivilDate>
{
    public MyCivilCalendar() : this(GetCivilSchema()) { }

    public MyCivilCalendar(CivilSchema schema)
        : base("Gregorian", new StandardScope(schema, DayZero.NewStyle)) { }

    [Pure]
    private static CivilSchema GetCivilSchema() => CivilSchema.GetInstance().Unbox();

    [Pure]
    protected sealed override MyCivilDate GetDateOn(int daysSinceEpoch) => new(daysSinceEpoch);
}
