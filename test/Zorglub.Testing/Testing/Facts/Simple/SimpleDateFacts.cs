// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Testing.Facts.Hemerology;
using Zorglub.Time.Simple;

// TODO(fact): move math tests to IDateFacts. Prerequesite: ordinal factory.

// NB: we know that all years within the range [1..9999] are valid.

/// <summary>
/// Provides facts about <see cref="ISimpleDate{TSelf}"/>.
/// </summary>
public abstract partial class SimpleDateFacts<TDate, TDataSet> :
    IDateFacts<TDate, TDataSet>
    where TDate : struct, ISimpleDate<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected SimpleDateFacts(SimpleCalendar calendar, SimpleCalendar otherCalendar)
        : base(GetDomain(calendar))
    {
        Debug.Assert(calendar != null);
        Requires.NotNull(otherCalendar);
        // NB: instances of type Calendar are singletons.
        if (ReferenceEquals(otherCalendar, calendar))
        {
            throw new ArgumentException(
                "\"otherCalendar\" MUST NOT be equal to \"calendar\"", nameof(otherCalendar));
        }
        if (calendar.IsUserDefined)
        {
            // REVIEW(fact): serialization if CalendarUT is not a system calendar;
            // idem with CalendarYear/Month.
            throw new ArgumentException(
                "\"calendar\" MUST NOT be a user-defined calendar", nameof(calendar));
        }

        CalendarUT = calendar;
        OtherCalendar = otherCalendar;
    }

    protected SimpleCalendar CalendarUT { get; }
    protected SimpleCalendar OtherCalendar { get; }

    protected abstract TDate GetDate(int y, int doy);

    protected TDate GetDate(Yedoy ydoy)
    {
        var (y, doy) = ydoy;
        return GetDate(y, doy);
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

    //[Theory, MemberData(nameof(DayNumberInfoData))]
    //public void Serialization_Roundtrip2(DayNumberInfo info)
    //{
    //    var date = TDate.FromDayNumber(info.DayNumber);
    //    // Act & Assert
    //    Assert.Equal(date, TDate.FromBinary(date.ToBinary()));
    //}
}

public partial class SimpleDateFacts<TDate, TDataSet> // Math
{
    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void NextDay_Ordinal(YedoyPair pair)
    {
        var date = GetDate(pair.First);
        var copy = date;
        var dateAfter = GetDate(pair.Second);
        // Act & Assert
        Assert.Equal(dateAfter, ++copy);
        Assert.Equal(dateAfter, date.NextDay());
    }

    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void PreviousDay_Ordinal(YedoyPair pair)
    {
        var date = GetDate(pair.First);
        var dateAfter = GetDate(pair.Second);
        var copy = dateAfter;
        // Act & Assert
        Assert.Equal(date, --copy);
        Assert.Equal(date, dateAfter.PreviousDay());
    }

    [Theory, MemberData(nameof(AddDaysOrdinalData))]
    public void PlusDays_Ordinal(YedoyPairAnd<int> pair)
    {
        int days = pair.Value;
        var date = GetDate(pair.First);
        var other = GetDate(pair.Second);
        // Act & Assert
        Assert.Equal(other, date + days);
        Assert.Equal(other, date - (-days));
        Assert.Equal(date, other - days);
        Assert.Equal(date, other + (-days));

        Assert.Equal(other, date.PlusDays(days));
        Assert.Equal(date, other.PlusDays(-days));

        Assert.Equal(days, other - date);
        Assert.Equal(-days, date - other);

        Assert.Equal(days, other.CountDaysSince(date));
        Assert.Equal(-days, date.CountDaysSince(other));
    }

    [RedundantTest]
    [Theory, MemberData(nameof(ConsecutiveDaysOrdinalData))]
    public void PlusDays_Ordinal_ViaConsecutiveDays(YedoyPair pair)
    {
        var date = GetDate(pair.First);
        var dateAfter = GetDate(pair.Second);
        // Act & Assert
        Assert.Equal(dateAfter, date + 1);
        Assert.Equal(dateAfter, date - (-1));
        Assert.Equal(date, dateAfter - 1);
        Assert.Equal(date, dateAfter + (-1));

        Assert.Equal(dateAfter, date.PlusDays(1));
        Assert.Equal(date, dateAfter.PlusDays(-1));

        Assert.Equal(1, dateAfter - date);
        Assert.Equal(-1, date - dateAfter);

        Assert.Equal(1, dateAfter.CountDaysSince(date));
        Assert.Equal(-1, date.CountDaysSince(dateAfter));
    }
}
