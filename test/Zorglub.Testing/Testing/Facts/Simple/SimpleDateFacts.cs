// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;

// TODO(fact): skip serialization if CalendarUT is not a system calendar; idem
// with CalendarYear/Month.
// ISimpleDate is internal, create and use a SimpleDateProxy?

/// <summary>
/// Provides facts about <see cref="ISimpleDate{TSelf}"/>.
/// </summary>
public abstract partial class SimpleDateFacts<TDate, TDataSet> :
    IDateFacts<TDate, TDataSet>
    // ISimpleDate being internal, we cannot use in a type constraint.
    where TDate : struct, IDate<TDate>, ISerializable<TDate, int>
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
