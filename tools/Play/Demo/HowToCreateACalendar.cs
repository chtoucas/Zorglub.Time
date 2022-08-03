// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Play.Demo;

using Samples;

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
            .CreateCalendar("CreateCalendar", DayZero.NewStyle, proleptic: true);

    // Hand-written version.
    public static SimpleCalendar CreateSimpleCalendar_Plain() =>
        (from x in GregorianSchema.GetInstance()
         select SimpleCatalog.Add("CreateCalendar_HWV", x, DayZero.NewStyle, proleptic: true)
         ).Unbox();

    #endregion
    #region Try create a simple calendar

    public static bool TryCreateSimpleCalendar(out SimpleCalendar? calendar) =>
        GregorianSchema.GetInstance()
            .TryCreateCalendar("TryCreateCalendar", DayZero.NewStyle, out calendar, proleptic: true);

    // Hand-written version.
    public static SimpleCalendar TryCreateSimpleCalendar_Plain()
    {
        return GregorianSchema.GetInstance().Select(TryAdd).Unbox();

        static SimpleCalendar? TryAdd(GregorianSchema schema)
        {
            _ = SimpleCatalog.TryAdd(
                "TryCreateCalendar_HWV", schema, DayZero.NewStyle, proleptic: true, out var chr);

            return chr;
        }
    }

    #endregion

    // Pre-defined calendar.
    public static MinMaxYearNakedCalendar CreateMinMaxYearNakedCalendar() =>
         (from x in GregorianSchema.GetInstance()
          select new MinMaxYearNakedCalendar(
              "CreateMinMaxYearNakedCalendar",
              MinMaxYearScope.Create(x, DayZero.NewStyle, Range.Create(1, 9999)))
          ).Unbox();

    // User-defined calendar.
    public static MyNakedCalendar CreateNakedCalendar() =>
        (from x in GregorianSchema.GetInstance()
         select new MyNakedCalendar(
             "CreateNakedCalendar",
             MinMaxYearScope.Create(x, DayZero.NewStyle, Range.Create(1, 9999)))
         ).Unbox();

    #region Scopes

    public static Box<MinMaxYearScope> GetScope() =>
        GregorianSchema.GetInstance()
            .Select(x => MinMaxYearScope.Create(x, DayZero.NewStyle, Range.Create(1, 9999)));

    public static Box<MinMaxYearScope> GetScope_QEP() =>
        from x in GregorianSchema.GetInstance()
        select MinMaxYearScope.Create(x, DayZero.NewStyle, Range.Create(1, 9999));

    private static Box<MinMaxYearScope> GetScope(Box<GregorianSchema> schema) =>
        schema.Select(x => MinMaxYearScope.Create(x, DayZero.NewStyle, Range.Create(1, 9999)));

    #endregion

    // Ways to construct the same calendar.

    public static MyNakedCalendar Select_QEP() =>
        (from x in GregorianSchema.GetInstance()
         let y = MinMaxYearScope.Create(x, DayZero.NewStyle, Range.Create(1, 9999))
         select new MyNakedCalendar("Select_QEP", y)
         ).Unbox();

    // Versions below are here to demonstrate ZipWith() and SelectMany(),
    // but Select_QEP() is a better and simpler solution; the intermediate
    // Box<MinMaxYearScope> is unnecessary.
    // Prerequisite: BoxExtensions from Zorglub.Sketches.

    public static MyNakedCalendar ZipWith()
    {
        Box<GregorianSchema> schema = GregorianSchema.GetInstance();
        Box<MinMaxYearScope> scope = GetScope(schema);

        return schema.ZipWith(scope, (x, y) => new MyNakedCalendar("ZipWith", y)).Unbox();
    }

    public static MyNakedCalendar SelectMany_QEP()
    {
        Box<GregorianSchema> schema = GregorianSchema.GetInstance();
        Box<MinMaxYearScope> scope = GetScope(schema);

        return (from x in scope
                select new MyNakedCalendar("SelectMany_QEP", x)
                ).Unbox();
    }

    // Abscons.
    public static MyNakedCalendar SelectMany()
    {
        Box<GregorianSchema> schema = GregorianSchema.GetInstance();
        Box<MinMaxYearScope> scope = GetScope(schema);

        return schema
            .SelectMany(
                _ => scope,
                (x, y) => new MyNakedCalendar("SelectMany", y))
            .Unbox();
    }
}
