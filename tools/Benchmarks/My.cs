// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks;

using Zorglub.Time;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;
using Zorglub.Time.Simple;

using static Zorglub.Time.Extensions.Unboxing;

internal static class My
{
    public static readonly SimpleCalendar Tropicalia = TropicaliaSchema.GetInstance()
        .CreateCalendar("Tropicália", DayZero.NewStyle);

    public static readonly SimpleCalendar Tropicalia3031 = Tropicalia3031Schema.GetInstance()
        .CreateCalendar("Tropicália 30-31", DayZero.NewStyle);

    public static readonly SimpleCalendar Tropicalia3130 = Tropicalia3130Schema.GetInstance()
        .CreateCalendar("Tropicália 31-30", DayZero.NewStyle);

    //
    // MinMaxYear calendars.
    //

    public static readonly MinMaxYearNakedCalendar NakedCivil =
        (from x in CivilSchema.GetInstance()
         select new MinMaxYearNakedCalendar("Gregorian", MinMaxYearScope.StartingAt(x, DayZero.NewStyle, 1))
         ).Unbox();

    public static readonly MinMaxYearNakedCalendar NakedJulian =
        (from x in JulianSchema.GetInstance()
         select new MinMaxYearNakedCalendar("Julian", MinMaxYearScope.StartingAt(x, DayZero.OldStyle, 1))
         ).Unbox();
}
