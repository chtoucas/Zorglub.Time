// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

// TODO(fact): CivilDateTests. Idem with IDateMathFacts.

/// <summary>
/// Provides more facts about <see cref="IDate{TSelf}"/>.
/// <para>See also <seealso cref="IDateFacts{TDate, TDataSet}"/>.</para>
/// </summary>
public abstract partial class IDateDayOfWeekFacts<TDate, TDataSet>
    where TDate : IDate<TDate>
    where TDataSet : IDayOfWeekDataSet, ISingleton<TDataSet>
{
    protected IDateDayOfWeekFacts() { }

    protected static TDataSet DataSet { get; } = TDataSet.Instance;

    protected abstract TDate GetDate(int y, int m, int d);

    protected TDate GetDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return GetDate(y, m, d);
    }

    public static TheoryData<YemodaAnd<DayOfWeek>> DayOfWeekData => DataSet.DayOfWeekData;
    public static TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_Before_Data => DataSet.DayOfWeek_Before_Data;
    public static TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_OnOrBefore_Data => DataSet.DayOfWeek_OnOrBefore_Data;
    public static TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_Nearest_Data => DataSet.DayOfWeek_Nearest_Data;
    public static TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_OnOrAfter_Data => DataSet.DayOfWeek_OnOrAfter_Data;
    public static TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_After_Data => DataSet.DayOfWeek_After_Data;
}

public partial class IDateDayOfWeekFacts<TDate, TDataSet> // Prelude
{
    [Theory, MemberData(nameof(DayOfWeekData))]
    public void DayOfWeek_Prop(YemodaAnd<DayOfWeek> info)
    {
        var (y, m, d, dayOfWeek) = info;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(dayOfWeek, date.DayOfWeek);
    }
}

public partial class IDateDayOfWeekFacts<TDate, TDataSet> // Adjust the day of the week
{
    [Theory, MemberData(nameof(DayOfWeek_Before_Data))]
    public void Previous(Yemoda ymd, Yemoda ymdExp, DayOfWeek dayOfWeek)
    {
        var date = GetDate(ymd);
        var exp = GetDate(ymdExp);
        // Act
        var actual = date.Previous(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DayOfWeek_OnOrBefore_Data))]
    public void PreviousOrSame(Yemoda ymd, Yemoda ymdExp, DayOfWeek dayOfWeek)
    {
        var date = GetDate(ymd);
        var exp = GetDate(ymdExp);
        // Act
        var actual = date.PreviousOrSame(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DayOfWeek_Nearest_Data))]
    public void Nearest(Yemoda ymd, Yemoda ymdExp, DayOfWeek dayOfWeek)
    {
        var date = GetDate(ymd);
        var exp = GetDate(ymdExp);
        // Act
        var actual = date.Nearest(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DayOfWeek_OnOrAfter_Data))]
    public void NextOrSame(Yemoda ymd, Yemoda ymdExp, DayOfWeek dayOfWeek)
    {
        var date = GetDate(ymd);
        var exp = GetDate(ymdExp);
        // Act
        var actual = date.NextOrSame(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DayOfWeek_After_Data))]
    public void Next(Yemoda ymd, Yemoda ymdExp, DayOfWeek dayOfWeek)
    {
        var date = GetDate(ymd);
        var exp = GetDate(ymdExp);
        // Act
        var actual = date.Next(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }
}
