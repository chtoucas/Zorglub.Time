// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Play.Demo;

using Zorglub.Time;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Utilities;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;
using Zorglub.Time.Simple;

using static Zorglub.Time.Extensions.BoxExtensions;
using static Zorglub.Time.Extensions.Unboxing;

public static class HowToCreateACalendar
{
    #region Create a simple calendar

    public static SimpleCalendar CreateSimpleCalendar() =>
        GregorianSchema.GetInstance()
            .CreateCalendar("CreateSimpleCalendar", DayZero.NewStyle, proleptic: true);

    // Hand-written version.
    public static SimpleCalendar CreateSimpleCalendarPlain() =>
        (from x in GregorianSchema.GetInstance()
         select SimpleCatalog.Add("CreateSimpleCalendarPlain", x, DayZero.NewStyle, proleptic: true)
         ).Unbox();

    #endregion
    #region Try create a simple calendar

    public static bool TryCreateSimpleCalendar(out SimpleCalendar? calendar) =>
        GregorianSchema.GetInstance()
            .TryCreateCalendar("TryCreateSimpleCalendar", DayZero.NewStyle, out calendar, proleptic: true);

    // Hand-written version.
    public static SimpleCalendar TryCreateSimpleCalendarPlain()
    {
        return GregorianSchema.GetInstance().Select(tryAdd).Unbox();

        static SimpleCalendar? tryAdd(GregorianSchema schema)
        {
            _ = SimpleCatalog.TryAdd(
                "TryCreateSimpleCalendarPlain", schema, DayZero.NewStyle, proleptic: true, out var chr);

            return chr;
        }
    }

    #endregion
    #region Scopes

    public static Box<MinMaxYearScope> GetScope() =>
        GregorianSchema.GetInstance()
            .Select(x => MinMaxYearScope.Create(x, DayZero.NewStyle, Range.Create(1, 9999)));

    public static Box<MinMaxYearScope> GetScopeQEP() =>
        from x in GregorianSchema.GetInstance()
        select MinMaxYearScope.Create(x, DayZero.NewStyle, Range.Create(1, 9999));

    private static Box<MinMaxYearScope> GetScope(Box<GregorianSchema> schema) =>
        schema.Select(x => MinMaxYearScope.Create(x, DayZero.NewStyle, Range.Create(1, 9999)));

    #endregion

    // Ways to construct the same calendar.

    public static MinMaxYearCalendar SelectQEP() =>
        (from x in GregorianSchema.GetInstance()
         let y = MinMaxYearScope.Create(x, DayZero.NewStyle, Range.Create(1, 9999))
         select new MinMaxYearCalendar("SelectQEP", y)
         ).Unbox();

    // Versions below are here to demonstrate ZipWith() and SelectMany(),
    // but SelectQEP() is a better and simpler solution; the intermediate
    // Box<MinMaxYearScope> is unnecessary.
    // Prerequisite: BoxExtensions from Zorglub.Sketches.

    public static MinMaxYearCalendar ZipWith()
    {
        Box<GregorianSchema> schema = GregorianSchema.GetInstance();
        Box<MinMaxYearScope> scope = GetScope(schema);

        return schema.ZipWith(scope, (x, y) => new MinMaxYearCalendar("ZipWith", y)).Unbox();
    }

    public static MinMaxYearCalendar SelectManyQEP()
    {
        Box<GregorianSchema> schema = GregorianSchema.GetInstance();
        Box<MinMaxYearScope> scope = GetScope(schema);

        return (from x in scope
                select new MinMaxYearCalendar("SelectManyQEP", x)
                ).Unbox();
    }

    // Abscons.
    public static MinMaxYearCalendar SelectMany()
    {
        Box<GregorianSchema> schema = GregorianSchema.GetInstance();
        Box<MinMaxYearScope> scope = GetScope(schema);

        return schema
            .SelectMany(
                _ => scope,
                (x, y) => new MinMaxYearCalendar("SelectMany", y))
            .Unbox();
    }
}
