// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

//public static class GregorianSchemaTests
//{
//    [Theory, MemberData(nameof(DayOfWeekByDate))]
//    public static void GetDayOfWeek(Yemoda xdate, DayOfWeek dayOfWeek)
//    {
//        // Arrange
//        var ymd = xdate.ToYemoda();
//        var (y, m, d) = ymd;
//        int doy = s_Schema.GetDayOfYear(y, m, d);
//        var ydoy = new Yedoy(y, doy);
//        // Act & Assert
//        Assert.Equal(dayOfWeek, s_Schema.GetDayOfWeek(ymd, (int)DayOfWeek.Monday));
//        Assert.Equal(dayOfWeek, s_Schema.GetDayOfWeek(ydoy, (int)DayOfWeek.Monday));
//    }
//}

//public sealed class JulianSchemaTests : SystemSchemaTesting<JulianSchema, JulianData>
//{
//    public JulianSchemaTests() : base(new JulianSchema()) { }

//    // Pour effectuer les tests, on ré-utilise les données collectées pour
//    // le calendrier grégorien.
//    public static TheoryData<Yemoda, DayOfWeek> JulianDayOfWeekByDate =>
//        new GregorianData().DayOfWeekByDate;

//    [Theory, MemberData(nameof(JulianDayOfWeekByDate))]
//    public static void GetDayOfWeek(Yemoda xdate, DayOfWeek dayOfWeek)
//    {
//        // Arrange
//        ICalendar chr = GregorianCalendar.Instance;
//        var dayNumber = chr.GetDayNumber(xdate.Year, xdate.Month, xdate.Day);
//        var date = JulianCalendar.Instance.GetCalendarDate(dayNumber);
//        var ydoy = new Yedoy(date.Year, date.DayOfYear);
//        // Act & Assert
//        Assert.Equal(dayOfWeek, SchemaUT.GetDayOfWeek(date.Parts, (int)DayOfWeek.Saturday));
//        Assert.Equal(dayOfWeek, SchemaUT.GetDayOfWeek(ydoy, (int)DayOfWeek.Saturday));
//    }
//}
