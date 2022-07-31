// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using System.Linq;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Simple;

// NB: we know that all years within the range [1..9999] are valid.

/// <summary>
/// Provides facts about <see cref="CalendarYear"/>.
/// </summary>
public abstract partial class CalendarYearFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarYearFacts(SimpleCalendar calendar, SimpleCalendar otherCalendar)
    {
        Requires.NotNull(calendar);
        Requires.NotNull(otherCalendar);
        // NB: instances of type Calendar are singletons.
        if (ReferenceEquals(otherCalendar, calendar))
        {
            throw new ArgumentException(
                "\"otherCalendar\" MUST NOT be equal to \"calendar\"", nameof(otherCalendar));
        }
        if (calendar.IsUserDefined)
        {
            throw new ArgumentException(
                "\"calendar\" MUST NOT be a user-defined calendar", nameof(calendar));
        }

        CalendarUT = calendar;
        OtherCalendar = otherCalendar;

        SupportedYearsTester = new SupportedYearsTester(calendar.YearsValidator.Range);

        (MinYear, MaxYear) = calendar.MinMaxYear;
    }

    protected SimpleCalendar CalendarUT { get; }
    protected SimpleCalendar OtherCalendar { get; }

    protected SupportedYearsTester SupportedYearsTester { get; }

    protected CalendarYear MinYear { get; }
    protected CalendarYear MaxYear { get; }

    /// <summary>
    /// We only use this sample year when its value matters (mathops); otherwise
    /// just use the year 1.
    /// </summary>
    private CalendarYear GetSampleYear() => CalendarUT.GetCalendarYear(1234);
}

public partial class CalendarYearFacts<TDataSet> // Prelude
{
    //
    // Properties
    //

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void CenturyOfEra_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var year = CalendarUT.GetCalendarYear(y);
        var centuryOfEra = Ord.Zeroth + century;
        // Act & Assert
        Assert.Equal(centuryOfEra, year.CenturyOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void Century_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var year = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(century, year.Century);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfEra_Prop(CenturyInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var yearOfEra = Ord.Zeroth + y;
        // Act & Assert
        Assert.Equal(yearOfEra, year.YearOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfCentury_Prop(CenturyInfo info)
    {
        var (y, _, yearOfCentury) = info;
        var year = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(yearOfCentury, year.YearOfCentury);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void Year_Prop(CenturyInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(y, year.Year);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void IsLeap_Prop(YearInfo info)
    {
        // Act
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Assert
        Assert.Equal(info.IsLeap, year.IsLeap);
    }

    [Fact]
    public void Calendar_Prop()
    {
        var year = CalendarUT.GetCalendarYear(1);
        // Act & Assert
        Assert.Equal(CalendarUT, year.Calendar);
        // We also test the internal prop Cuid.
        Assert.Equal(CalendarUT.Id, year.Cuid);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void FirstMonth_Prop(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var month = CalendarUT.GetCalendarMonth(y, 1);
        // Act & Assert
        Assert.Equal(month, year.FirstMonth);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void LastMonth_Prop(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var month = CalendarUT.GetCalendarMonth(y, info.MonthsInYear);
        // Act & Assert
        Assert.Equal(month, year.LastMonth);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void FirstDay_Prop(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var startOfYear = CalendarUT.GetOrdinalDate(y, 1);
        // Act & Assert
        Assert.Equal(startOfYear, year.FirstDay);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void LastDay_Prop(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var endOfYear = CalendarUT.GetOrdinalDate(y, info.DaysInYear);
        // Act & Assert
        Assert.Equal(endOfYear, year.LastDay);
    }
}

public partial class CalendarYearFacts<TDataSet> // Calendar mismatch
{
    [Fact]
    public void Equality_OtherCalendar()
    {
        var year = CalendarUT.GetCalendarYear(1);
        var other = OtherCalendar.GetCalendarYear(1);
        // Act & Assert
        Assert.False(year == other);
        Assert.True(year != other);

        Assert.False(year.Equals(other));
        Assert.False(year.Equals((object)other));
    }

    [Fact]
    public void Comparison_OtherCalendar()
    {
        var year = CalendarUT.GetCalendarYear(1);
        var other = OtherCalendar.GetCalendarYear(1);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => year > other);
        Assert.Throws<ArgumentException>("other", () => year >= other);
        Assert.Throws<ArgumentException>("other", () => year < other);
        Assert.Throws<ArgumentException>("other", () => year <= other);

        Assert.Throws<ArgumentException>("other", () => year.CompareTo(other));
    }

    [Fact]
    public void CountYearsSince_OtherCalendar()
    {
        var year = CalendarUT.GetCalendarYear(1);
        var other = OtherCalendar.GetCalendarYear(1);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => year.CountYearsSince(other));
        Assert.Throws<ArgumentException>("other", () => year - other);
    }
}

public partial class CalendarYearFacts<TDataSet> // Serialization
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void Serialization_Roundtrip(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Act & Assert
        Assert.Equal(year, CalendarYear.FromBinary(year.ToBinary()));
    }
}

public partial class CalendarYearFacts<TDataSet> // Conversions
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void ToRange(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var min = CalendarUT.GetOrdinalDate(y, 1);
        var max = CalendarUT.GetOrdinalDate(y, info.DaysInYear);
        // Act
        var range = year.ToRange();
        // Assert
        Assert.Equal(min, range.Min);
        Assert.Equal(max, range.Max);
    }

    [Fact]
    public void WithCalendar_InvalidCalendar()
    {
        var year = CalendarUT.GetCalendarYear(1);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => year.WithCalendar(null!));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void WithCalendar_Invariance(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        var range = year.ToRange();
        // Act & Assert
        Assert.Equal(range, year.WithCalendar(CalendarUT));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void WithCalendar(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        var range = year.ToRange().WithCalendar(OtherCalendar);
        // Act & Assert
        Assert.Equal(range, year.WithCalendar(OtherCalendar));
    }
}

public partial class CalendarYearFacts<TDataSet> // Counting
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void CountMonthsInYear(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Act & Assert
        Assert.Equal(info.MonthsInYear, year.CountMonthsInYear());
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYear(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Act & Assert
        Assert.Equal(info.DaysInYear, year.CountDaysInYear());
    }
}

public partial class CalendarYearFacts<TDataSet> // Months and days
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetMonthOfYear(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var year = CalendarUT.GetCalendarYear(y);
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(month, year.GetMonthOfYear(m));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetAllMonths(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var exp = from m in Enumerable.Range(1, info.MonthsInYear)
                  select CalendarUT.GetCalendarMonth(y, m);
        // Act
        var actual = year.GetAllMonths();
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDayOfYear(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var year = CalendarUT.GetCalendarYear(y);
        var date = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(date, year.GetDayOfYear(doy));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetAllDays(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var exp = from doy in Enumerable.Range(1, info.DaysInYear)
                  select CalendarUT.GetOrdinalDate(y, doy);
        // Act
        var actual = year.GetAllDays();
        // Assert
        Assert.Equal(exp, actual);
    }
}

public partial class CalendarYearFacts<TDataSet> // Adjustments
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void WithYear_InvalidYears(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Act & Assert
        SupportedYearsTester.TestInvalidYear(year.WithYear, "newYear");
    }

    [Fact]
    public void WithYear_ValidYears()
    {
        foreach (int y in SupportedYearsTester.ValidYears)
        {
            var year = CalendarUT.GetCalendarYear(1);
            var exp = CalendarUT.GetCalendarYear(y);
            // Act & Assert
            Assert.Equal(exp, year.WithYear(y));
        }
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void WithYear_Invariance(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(year, year.WithYear(y));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void WithYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(1);
        var exp = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(exp, year.WithYear(y));
    }
}

public partial class CalendarYearFacts<TDataSet> // IEquatable
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void Equals_WhenSame(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var same = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.True(year == same);
        Assert.False(year != same);

        Assert.True(year.Equals(same));
        Assert.True(year.Equals((object)same));
    }

    [Fact]
    public void Equals_WhenNotSame()
    {
        var year = CalendarUT.GetCalendarYear(1);
        var notSame = CalendarUT.GetCalendarYear(2);
        // Act & Assert
        Assert.False(year == notSame);
        Assert.True(year != notSame);

        Assert.False(year.Equals(notSame));
        Assert.False(year.Equals((object)notSame));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void Equals_NullOrPlainObject(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Act & Assert
        Assert.False(year.Equals(1));
        Assert.False(year.Equals(null));
        Assert.False(year.Equals(new object()));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetHashCode_Repeated(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        var obj = (object)year;
        // Act & Assert
        Assert.Equal(year.GetHashCode(), year.GetHashCode());
        Assert.Equal(year.GetHashCode(), obj.GetHashCode());
    }
}

public partial class CalendarYearFacts<TDataSet> // IComparable
{
    [Fact]
    public void CompareTo_Null()
    {
        var year = CalendarUT.GetCalendarYear(1);
        var comparable = (IComparable)year;
        // Act & Assert
        Assert.Equal(1, comparable.CompareTo(null));
    }

    [Fact]
    public void CompareTo_PlainObject()
    {
        var year = CalendarUT.GetCalendarYear(1);
        var comparable = (IComparable)year;
        object other = new();
        // Act & Assert
        Assert.Throws<ArgumentException>("obj", () => comparable.CompareTo(other));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CompareTo_WhenEqual(YearInfo info)
    {
        int y = info.Year;
        var left = CalendarUT.GetCalendarYear(y);
        var right = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.False(left > right);
        Assert.True(left >= right);
        Assert.True(left <= right);
        Assert.False(left < right);

        Assert.Equal(0, left.CompareTo(right));
        Assert.Equal(0, left.CompareTo((object)right));
    }

    [Fact]
    public void CompareTo_WhenNotEqual()
    {
        var left = CalendarUT.GetCalendarYear(1);
        var right = CalendarUT.GetCalendarYear(2);
        // Act & Assert
        Assert.False(left > right);
        Assert.False(left >= right);
        Assert.True(left <= right);
        Assert.True(left < right);

        Assert.True(left.CompareTo(right) < 0);
        Assert.True(left.CompareTo((object)right) < 0);
    }

    [Fact]
    public void Min()
    {
        var min = CalendarUT.GetCalendarYear(1);
        var max = CalendarUT.GetCalendarYear(2);
        // Act & Assert
        Assert.Equal(min, CalendarYear.Min(min, max));
        Assert.Equal(min, CalendarYear.Min(max, min));
    }

    [Fact]
    public void Max()
    {
        var min = CalendarUT.GetCalendarYear(1);
        var max = CalendarUT.GetCalendarYear(2);
        // Act & Assert
        Assert.Equal(max, CalendarYear.Max(min, max));
        Assert.Equal(max, CalendarYear.Max(max, min));
    }
}

public partial class CalendarYearFacts<TDataSet> // Math
{
    #region NextYear()

    [Fact]
    public void NextYear_Overflows_AtMaxValue()
    {
        var copy = MaxYear;
        // Act & Assert
        Assert.Overflows(() => copy++);
        Assert.Overflows(() => MaxYear.NextYear());
    }

    [Fact]
    public void NextYear()
    {
        var year = GetSampleYear();
        var copy = year;
        var yearAfter = CalendarUT.GetCalendarYear(year.Year + 1);
        // Act & Assert
        Assert.Equal(yearAfter, ++copy);
        Assert.Equal(yearAfter, year.NextYear());
    }

    #endregion
    #region PreviousYear()

    [Fact]
    public void PreviousYear_Overflows_AtMinValue()
    {
        var copy = MinYear;
        // Act & Assert
        Assert.Overflows(() => copy--);
        Assert.Overflows(() => MinYear.PreviousYear());
    }

    [Fact]
    public void PreviousYear()
    {
        var year = GetSampleYear();
        var copy = year;
        var yearBefore = CalendarUT.GetCalendarYear(year.Year - 1);
        // Act & Assert
        Assert.Equal(yearBefore, --copy);
        Assert.Equal(yearBefore, year.PreviousYear());
    }

    #endregion
    #region PlusYears() & CountYearsSince()

    [Fact]
    public void PlusYears_Overflows_WithMaxYears()
    {
        var year = CalendarUT.GetCalendarYear(1);
        // Act & Assert
        Assert.Overflows(() => year + Int32.MinValue);
        Assert.Overflows(() => year + Int32.MaxValue);

        Assert.Overflows(() => year.PlusYears(Int32.MinValue));
        Assert.Overflows(() => year.PlusYears(Int32.MaxValue));
    }

    [Fact]
    public void PlusYears_WithLimitYears()
    {
        var year = GetSampleYear();
        int minYs = MinYear - year;
        int maxYs = MaxYear - year;
        // Act & Assert
        Assert.Overflows(() => year + (minYs - 1));
        Assert.Equal(MinYear, year + minYs);
        Assert.Equal(MaxYear, year + maxYs);
        Assert.Overflows(() => year + (maxYs + 1));

        Assert.Overflows(() => year.PlusYears(minYs - 1));
        Assert.Equal(MinYear, year.PlusYears(minYs));
        Assert.Equal(MaxYear, year.PlusYears(maxYs));
        Assert.Overflows(() => year.PlusYears(maxYs + 1));
    }

    [Fact]
    public void CountYearsSince_DoesNotOverflow()
    {
        int ys = MaxYear.Year - MinYear.Year;
        // Act & Assert
        Assert.Equal(ys, MaxYear - MinYear);
        Assert.Equal(-ys, MinYear - MaxYear);

        Assert.Equal(ys, MaxYear.CountYearsSince(MinYear));
        Assert.Equal(-ys, MinYear.CountYearsSince(MaxYear));
    }

    [Fact]
    public void PlusYears_AtMinYear()
    {
        // We could have written:
        // > int ys = MaxYear - MinYear;
        // but this is CountYearsSince() in disguise and I prefer to stick to
        // basic maths.
        int ys = CalendarUT.YearsValidator.Range.Count() - 1;
        // Act & Assert
        Assert.Overflows(() => MinYear - 1);
        Assert.Equal(MinYear, MinYear - 0);
        Assert.Equal(MinYear, MinYear + 0);
        Assert.Equal(MaxYear, MinYear + ys);
        Assert.Overflows(() => MinYear + (ys + 1));

        Assert.Overflows(() => MinYear.PlusYears(-1));
        Assert.Equal(MinYear, MinYear.PlusYears(0));
        Assert.Equal(MaxYear, MinYear.PlusYears(ys));
        Assert.Overflows(() => MinYear.PlusYears(ys + 1));
    }

    [Fact]
    public void PlusYears_AtMaxYear()
    {
        // We could have written:
        // > int ys = MaxYear - MinYear;
        // but this is CountYearsSince() in disguise and I prefer to stick to
        // basic maths.
        int ys = CalendarUT.YearsValidator.Range.Count() - 1;
        // Act & Assert
        Assert.Overflows(() => MaxYear - (ys + 1));
        Assert.Equal(MinYear, MaxYear - ys);
        Assert.Equal(MaxYear, MaxYear - 0);
        Assert.Equal(MaxYear, MaxYear + 0);
        Assert.Overflows(() => MaxYear + 1);

        Assert.Overflows(() => MaxYear.PlusYears(-ys - 1));
        Assert.Equal(MinYear, MaxYear.PlusYears(-ys));
        Assert.Equal(MaxYear, MaxYear.PlusYears(0));
        Assert.Overflows(() => MaxYear.PlusYears(1));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void PlusYears_Zero_IsNeutral(YearInfo info)
    {
        var year = CalendarUT.GetCalendarYear(info.Year);
        // Act & Assert
        Assert.Equal(year, year + 0);
        Assert.Equal(year, year - 0);
        Assert.Equal(year, year.PlusYears(0));

        Assert.Equal(0, year - year);
        Assert.Equal(0, year.CountYearsSince(year));
    }

    [Fact]
    public void PlusYears()
    {
        // NB: ys is such that "other" is a valid year for both standard and
        // proleptic calendars.
        int ys = 876;
        var year = GetSampleYear();
        var other = CalendarUT.GetCalendarYear(year.Year + ys);
        // Act & Assert
        Assert.Equal(other, year + ys);
        Assert.Equal(other, year - (-ys));
        Assert.Equal(year, other - ys);
        Assert.Equal(year, other + (-ys));

        Assert.Equal(other, year.PlusYears(ys));
        Assert.Equal(year, other.PlusYears(-ys));

        Assert.Equal(ys, other - year);
        Assert.Equal(-ys, year - other);

        Assert.Equal(ys, other.CountYearsSince(year));
        Assert.Equal(-ys, year.CountYearsSince(other));
    }

    #endregion
}