// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides facts about <see cref="IDate{TSelf}"/>.
/// </summary>
public abstract class MultiDateFacts<TDate, TCalendar, TDataSet> : IDateFacts<TDate, TDataSet>
    where TDate : struct, IDate<TDate>
    where TCalendar : class, ICalendar
    where TDataSet :
        ICalendarDataSet,
        IDaysAfterDataSet,
        IMathDataSet,
        IDayOfWeekDataSet,
        ISingleton<TDataSet>
{
    protected MultiDateFacts(TCalendar calendar!!, TCalendar otherCalendar!!)
        : base(calendar.SupportedYears, calendar.Domain)
    {
        if (otherCalendar == calendar)
        {
            throw new ArgumentException(
                "\"otherCalendar\" should not be equal to \"calendar\"", nameof(otherCalendar));
        }

        CalendarUT = calendar;
        OtherCalendar = otherCalendar;
    }

    protected TCalendar CalendarUT { get; }
    protected TCalendar OtherCalendar { get; }
}
