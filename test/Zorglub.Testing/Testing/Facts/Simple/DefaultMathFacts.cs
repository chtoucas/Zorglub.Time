// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

// REVIEW(fact): temporary class to be able to test DefaultMath.

/// <summary>
/// Provides facts about <see cref="DefaultMath"/>.
/// </summary>
public abstract partial class DefaultMathFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    internal DefaultMathFacts(DefaultMath math)
    {
        MathUT = math ?? throw new ArgumentNullException(nameof(math));
        Calendar = math.Calendar;
    }

    private DefaultMath MathUT { get; }
    protected Calendar Calendar { get; }

    protected CalendarDate GetDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return Calendar.GetCalendarDate(y, m, d);
    }

    private CalendarMonth GetMonth(Yemoda ymd)
    {
        var (y, m, _) = ymd;
        return Calendar.GetCalendarMonth(y, m);
    }
}

public partial class DefaultMathFacts<TDataSet> // CalendarDate
{
}

public partial class DefaultMathFacts<TDataSet> // CalendarMonth
{
    [Theory, MemberData(nameof(AddYearsData))]
    public void AddYears﹍CalendarMonth(YemodaPairAnd<int> info)
    {
        int ys = info.Value;
        var month = GetMonth(info.First);
        var other = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddYears(month, ys));
        Assert.Equal(month, MathUT.AddYears(other, -ys));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void CountMonthsBetween﹍CalendarMonth(YemodaPairAnd<int> info)
    {
        int ms = info.Value;
        var start = GetMonth(info.First);
        var end = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(ms, MathUT.CountMonthsBetween(start, end));
        Assert.Equal(-ms, MathUT.CountMonthsBetween(end, start));
    }
}
