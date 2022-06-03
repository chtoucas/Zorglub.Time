// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

// Hypothesis:
// - TDate is a value type.
// - See also IDateableFacts.

// TODO(fact): for simple date objects the constructor is tested in
// CalendarFacts (Factories). For the others, copy this code: CivilDate
// and WideDate. See also CalendarDateTests.
// DayOfWeek via DayNumber. Pre-filter CalCalDataSet.DayOfWeekData.

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides facts about <see cref="IDate{TSelf}"/>.
/// </summary>
public abstract partial class IDateFacts<TDate, TDataSet> :
    IDateableFacts<TDate, TDataSet>
    where TDate : struct, IDate<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected IDateFacts(Range<int> supportedYears, Range<DayNumber> domain)
    {
        Domain = domain;

        SupportedYearsTester = new SupportedYearsTester(supportedYears);
    }

    protected Range<DayNumber> Domain { get; }

    protected SupportedYearsTester SupportedYearsTester { get; }

    protected abstract TDate MinDate { get; }
    protected abstract TDate MaxDate { get; }

    protected TDate GetDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return GetDate(y, m, d);
    }

    // ICalendarDataSet
    public static DataGroup<DayNumberInfo> DayNumberInfoData => DataSet.DayNumberInfoData;
}

public partial class IDateFacts<TDate, TDataSet> // Prelude
{
    //[Theory, MemberData(nameof(CalCalDataSet.DayOfWeekData), MemberType = typeof(CalCalDataSet))]
    //public void DayOfWeek_Prop_ViaDayNumber(DayNumber dayNumber, DayOfWeek dayOfWeek)
    //{
    //    if (Domain.Contains(dayNumber) == false) { return; }
    //    var date = TDate.FromDayNumber(dayNumber);
    //    // Act & Assert
    //    Assert.Equal(dayOfWeek, date.DayOfWeek);
    //}
}

public partial class IDateFacts<TDate, TDataSet> // Conversions
{
    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void ToDayNumber(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(dayNumber, date.ToDayNumber());
    }
}

public partial class IDateFacts<TDate, TDataSet> // Adjust the day of the week
{
    #region Arg check

    [Theory, MemberData(nameof(EnumDataSet.InvalidDayOfWeekData), MemberType = typeof(EnumDataSet))]
    public void Previous_InvalidDayOfWeek(DayOfWeek dayOfWeek)
    {
        var date = GetDate(1, 1, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("dayOfWeek", () => date.Previous(dayOfWeek));
    }

    [Theory, MemberData(nameof(EnumDataSet.InvalidDayOfWeekData), MemberType = typeof(EnumDataSet))]
    public void PreviousOrSame_InvalidDayOfWeek(DayOfWeek dayOfWeek)
    {
        var date = GetDate(1, 1, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("dayOfWeek", () => date.PreviousOrSame(dayOfWeek));
    }

    [Theory, MemberData(nameof(EnumDataSet.InvalidDayOfWeekData), MemberType = typeof(EnumDataSet))]
    public void Nearest_InvalidDayOfWeek(DayOfWeek dayOfWeek)
    {
        var date = GetDate(1, 1, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("dayOfWeek", () => date.Nearest(dayOfWeek));
    }

    [Theory, MemberData(nameof(EnumDataSet.InvalidDayOfWeekData), MemberType = typeof(EnumDataSet))]
    public void NextOrSame_InvalidDayOfWeek(DayOfWeek dayOfWeek)
    {
        var date = GetDate(1, 1, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("dayOfWeek", () => date.NextOrSame(dayOfWeek));
    }

    [Theory, MemberData(nameof(EnumDataSet.InvalidDayOfWeekData), MemberType = typeof(EnumDataSet))]
    public void Next_InvalidDayOfWeek(DayOfWeek dayOfWeek)
    {
        var date = GetDate(1, 1, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("dayOfWeek", () => date.Next(dayOfWeek));
    }

    #endregion

    [Fact]
    public void Previous_NearMinValue() =>
        DayOfWeekAdjusterTester.NearMinValue(MinDate).TestPrevious();

    [Fact]
    public void PreviousOrSame_NearMinValue() =>
        DayOfWeekAdjusterTester.NearMinValue(MinDate).TestPreviousOrSame();

    [Fact]
    public void NextOrSame_NearMaxValue() =>
        DayOfWeekAdjusterTester.NearMaxValue(MaxDate).TestNextOrSame();

    [Fact]
    public void Next_NearMaxValue() =>
        DayOfWeekAdjusterTester.NearMaxValue(MaxDate).TestNext();
}

public partial class IDateFacts<TDate, TDataSet> // Math
{
    #region NextDay()

    [Fact]
    public void NextDay_Overflows_AtMaxValue()
    {
        var copy = MaxDate;
        // Act & Assert
        Assert.Overflows(() => copy++);
        Assert.Overflows(() => MaxDate.NextDay());
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void NextDay(YemodaPair pair)
    {
        var date = GetDate(pair.First);
        var copy = date;
        var dateAfter = GetDate(pair.Second);
        // Act & Assert
        Assert.Equal(dateAfter, ++copy);
        Assert.Equal(dateAfter, date.NextDay());
    }

    #endregion
    #region PreviousDay()

    [Fact]
    public void PreviousDay_Overflows_AtMinValue()
    {
        var copy = MinDate;
        // Act & Assert
        Assert.Overflows(() => copy--);
        Assert.Overflows(() => MinDate.PreviousDay());
    }

    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void PreviousDay(YemodaPair pair)
    {
        var date = GetDate(pair.First);
        var dateAfter = GetDate(pair.Second);
        var copy = dateAfter;
        // Act & Assert
        Assert.Equal(date, --copy);
        Assert.Equal(date, dateAfter.PreviousDay());
    }

    #endregion
    #region PlusDays() & CountDaysSince()

    [Fact]
    public void PlusDays_Overflows()
    {
        var date = GetDate(1, 1, 1);
        // Act & Assert
        Assert.Overflows(() => date + Int32.MinValue);
        Assert.Overflows(() => date + Int32.MaxValue);

        Assert.Overflows(() => date.PlusDays(Int32.MinValue));
        Assert.Overflows(() => date.PlusDays(Int32.MaxValue));
    }

    [Fact]
    public void PlusDays_WithLimitValues()
    {
        var (minDayNumber, maxDayNumber) = Domain.Endpoints;
        // We do not use the epoch 1/1/1, most of the time it's like testing
        // MinValue which we already do in PlusDays_WithLimitValues_AtMinValue().
        var date = GetDate(4, 3, 2);
        var dayNumber = date.ToDayNumber();
        int minDays = minDayNumber - dayNumber;
        int maxDays = maxDayNumber - dayNumber;
        // Act & Assert
        Assert.Overflows(() => date + (minDays - 1));
        Assert.Equal(MinDate, date + minDays);
        Assert.Equal(MaxDate, date + maxDays);
        Assert.Overflows(() => date + (maxDays + 1));

        Assert.Overflows(() => date.PlusDays(minDays - 1));
        Assert.Equal(MinDate, date.PlusDays(minDays));
        Assert.Equal(MaxDate, date.PlusDays(maxDays));
        Assert.Overflows(() => date.PlusDays(maxDays + 1));

        Assert.Equal(minDays, MinDate - date);
        Assert.Equal(-minDays, date - MinDate);
        Assert.Equal(maxDays, MaxDate - date);
        Assert.Equal(-maxDays, date - MaxDate);

        Assert.Equal(minDays, MinDate.CountDaysSince(date));
        Assert.Equal(-minDays, date.CountDaysSince(MinDate));
        Assert.Equal(maxDays, MaxDate.CountDaysSince(date));
        Assert.Equal(-maxDays, date.CountDaysSince(MaxDate));
    }

    [Fact]
    public void CountDaysSince_DoesNotOverflow()
    {
        int days = Domain.Count() - 1;
        // Act & Assert
        Assert.Equal(days, MaxDate - MinDate);
        Assert.Equal(-days, MinDate - MaxDate);

        Assert.Equal(days, MaxDate.CountDaysSince(MinDate));
        Assert.Equal(-days, MinDate.CountDaysSince(MaxDate));
    }

    [Fact]
    public void PlusDays_AtMinValue()
    {
        int days = Domain.Count() - 1;
        // Act & Assert
        Assert.Overflows(() => MinDate - 1);
        Assert.Equal(MinDate, MinDate - 0);
        Assert.Equal(MinDate, MinDate + 0);
        Assert.Equal(MaxDate, MinDate + days);
        Assert.Overflows(() => MinDate + (days + 1));

        Assert.Overflows(() => MinDate.PlusDays(-1));
        Assert.Equal(MinDate, MinDate.PlusDays(0));
        Assert.Equal(MaxDate, MinDate.PlusDays(days));
        Assert.Overflows(() => MinDate.PlusDays(days + 1));
    }

    [Fact]
    public void PlusDays_AtMaxValue()
    {
        int days = Domain.Count() - 1;
        // Act & Assert
        Assert.Overflows(() => MaxDate - (days + 1));
        Assert.Equal(MinDate, MaxDate - days);
        Assert.Equal(MaxDate, MaxDate - 0);
        Assert.Equal(MaxDate, MaxDate + 0);
        Assert.Overflows(() => MaxDate + 1);

        Assert.Overflows(() => MaxDate.PlusDays(-days - 1));
        Assert.Equal(MinDate, MaxDate.PlusDays(-days));
        Assert.Equal(MaxDate, MaxDate.PlusDays(0));
        Assert.Overflows(() => MaxDate.PlusDays(1));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void PlusDays_Zero_IsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, date + 0);
        Assert.Equal(date, date - 0);
        Assert.Equal(date, date.PlusDays(0));

        Assert.Equal(0, date - date);
        Assert.Equal(0, date.CountDaysSince(date));
    }

    [Theory, MemberData(nameof(AddDaysData))]
    public void PlusDays(YemodaPairAnd<int> pair)
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
    [Theory, MemberData(nameof(ConsecutiveDaysData))]
    public void PlusDays_ViaConsecutiveDays(YemodaPair pair)
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

    #endregion
}

public partial class IDateFacts<TDate, TDataSet> // IEquatable
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void Equals_WhenSame(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        var same = GetDate(y, m, d);
        // Act & Assert
        Assert.True(date == same);
        Assert.False(date != same);

        Assert.True(date.Equals(same));
        Assert.True(date.Equals((object)same));
    }

    [Theory]
    [InlineData(2, 1, 1)]
    [InlineData(1, 2, 1)]
    [InlineData(1, 1, 2)]
    public void Equals_WhenNotSame(int y, int m, int d)
    {
        var date = GetDate(1, 1, 1);
        var notSame = GetDate(y, m, d);
        // Act & Assert
        Assert.False(date == notSame);
        Assert.True(date != notSame);

        Assert.False(date.Equals(notSame));
        Assert.False(date.Equals((object)notSame));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Equals_NullOrPlainObject(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.False(date.Equals(1));
        Assert.False(date.Equals(null));
        Assert.False(date.Equals(new object()));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetHashCode_Repeated(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        var obj = (object)date;
        // Act & Assert
        Assert.Equal(date.GetHashCode(), date.GetHashCode());
        Assert.Equal(date.GetHashCode(), obj.GetHashCode());
    }
}

public partial class IDateFacts<TDate, TDataSet> // IComparable
{
    [Fact]
    public void CompareTo_Null()
    {
        var date = GetDate(1, 1, 1);
        var comparable = (IComparable)date;
        // Act & Assert
        Assert.Equal(1, comparable.CompareTo(null));
    }

    [Fact]
    public void CompareTo_PlainObject()
    {
        var date = GetDate(1, 1, 1);
        var comparable = (IComparable)date;
        object other = new();
        // Act & Assert
        Assert.Throws<ArgumentException>("obj", () => comparable.CompareTo(other));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CompareTo_WhenEqual(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var left = GetDate(y, m, d);
        var right = GetDate(y, m, d);
        // Act & Assert
        Assert.False(left > right);
        Assert.True(left >= right);
        Assert.True(left <= right);
        Assert.False(left < right);

        Assert.Equal(0, left.CompareTo(right));
        Assert.Equal(0, left.CompareTo((object)right));
    }

    [Theory]
    [InlineData(2, 1, 1)]
    [InlineData(1, 2, 1)]
    [InlineData(1, 1, 2)]
    public void CompareTo_WhenNotEqual(int y, int m, int d)
    {
        var left = GetDate(1, 1, 1);
        var right = GetDate(y, m, d);
        // Act & Assert
        Assert.False(left > right);
        Assert.False(left >= right);
        Assert.True(left <= right);
        Assert.True(left < right);

        Assert.True(left.CompareTo(right) < 0);
        Assert.True(left.CompareTo((object)right) < 0);
    }

    [Theory]
    [InlineData(2, 1, 1)]
    [InlineData(1, 2, 1)]
    [InlineData(1, 1, 2)]
    public void Min(int y, int m, int d)
    {
        var min = GetDate(1, 1, 1);
        var max = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(min, TDate.Min(min, max));
        Assert.Equal(min, TDate.Min(max, min));
    }

    [Theory]
    [InlineData(2, 1, 1)]
    [InlineData(1, 2, 1)]
    [InlineData(1, 1, 2)]
    public void Max(int y, int m, int d)
    {
        var min = GetDate(1, 1, 1);
        var max = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(max, TDate.Max(min, max));
        Assert.Equal(max, TDate.Max(max, min));
    }
}
