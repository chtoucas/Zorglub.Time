// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Testing.Data.Bounded;
using Zorglub.Time.Hemerology;

public sealed class JulianCalendarTests
{
    public static readonly JulianCalendar CalendarUT = JulianCalendar.Instance;

    // Pour effectuer les tests, on ré-utilise les données collectées pour
    // le calendrier grégorien.
    public static TheoryData<YemodaAnd<DayOfWeek>> GregorianDayOfWeekData =>
        ProlepticGregorianDataSet.Instance.DayOfWeekData;

    [Theory, MemberData(nameof(GregorianDayOfWeekData))]
    public void GetDayOfWeek_UsingDates(YemodaAnd<DayOfWeek> info)
    {
        var (y, m, d, dayOfWeek) = info;
        // Arrange
        ICalendar chr = GregorianCalendar.Instance;
        var dayNumber = chr.GetDayNumberOn(y, m, d);
        var date = CalendarUT.GetCalendarDateOn(dayNumber);
        // Act & Assert
        Assert.Equal(dayOfWeek, date.DayOfWeek);
    }
}
