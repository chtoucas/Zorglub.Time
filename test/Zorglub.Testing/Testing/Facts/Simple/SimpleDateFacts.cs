// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;

// FIXME(fact): skip serialization if CalendarUT is not a system calendar; idem
// with CalendarYear/Month.

// NB: we know that all years within the range [1..9999] are valid.

/// <summary>
/// Provides facts about <see cref="ISimpleDate{TSelf}"/>.
/// </summary>
public abstract partial class SimpleDateFacts<TDate, TDataSet> :
    IDateFacts<TDate, TDataSet>
    where TDate : struct, ISimpleDate<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected SimpleDateFacts(Calendar calendar, Calendar otherCalendar)
        : this(calendar, otherCalendar, BaseCtorArgs.Create(calendar)) { }

    private SimpleDateFacts(Calendar calendar, Calendar otherCalendar, BaseCtorArgs args)
        : base(args.SupportedYears, args.Domain)
    {
        Debug.Assert(calendar != null);
        Requires.NotNull(otherCalendar);
        // NB: calendars of type Calendar are singletons.
        if (ReferenceEquals(otherCalendar, calendar))
        {
            throw new ArgumentException(
                "\"otherCalendar\" MUST NOT be equal to \"calendar\"", nameof(otherCalendar));
        }

        CalendarUT = calendar;
        OtherCalendar = otherCalendar;
    }

    protected Calendar CalendarUT { get; }
    protected Calendar OtherCalendar { get; }

    private sealed record BaseCtorArgs(Range<int> SupportedYears, Range<DayNumber> Domain)
    {
        public static BaseCtorArgs Create(Calendar calendar)
        {
            Requires.NotNull(calendar);
            return new BaseCtorArgs(calendar.SupportedYears, calendar.Domain);
        }
    }
}

public partial class SimpleDateFacts<TDate, TDataSet> // Prelude
{
    [Fact]
    public void Calendar_Prop()
    {
        var date = GetDate(1, 1, 1);
        // Act & Assert
        Assert.Equal(CalendarUT, date.Calendar);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CalendarYear_Prop(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        var exp = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(exp, date.CalendarYear);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CalendarMonth_Prop(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        var exp = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(exp, date.CalendarMonth);
    }
}

public partial class SimpleDateFacts<TDate, TDataSet> // Serialization
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void Serialization_Roundtrip1(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, TDate.FromBinary(date.ToBinary()));
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void Serialization_Roundtrip2(DayNumberInfo info)
    {
        var date = TDate.FromDayNumber(info.DayNumber);
        // Act & Assert
        Assert.Equal(date, TDate.FromBinary(date.ToBinary()));
    }
}
