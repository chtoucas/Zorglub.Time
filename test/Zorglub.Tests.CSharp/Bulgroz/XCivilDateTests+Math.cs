﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz;

using Zorglub.Time;

public partial class XCivilDateTests // CountDaysSince()
{
    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void CountDaysSince_Epoch(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        var date = new XCivilDate(y, m, d);
        // Act
        // NB: Bulgroz.MinValue.DayNumber = GregorianCalendar.Epoch.
        var actual = DayZero.NewStyle + date.CountDaysSince(XCivilDate.MinValue);
        // Act & Assert
        Assert.Equal(dayNumber, actual);
    }
}

public partial class XCivilDateTests // PlusYears(), CountYearsSince()
{
    [Fact]
    public static void PlusYears_OverflowOrUnderflow()
    {
        var date = new XCivilDate(3, 4, 5);
        // Act & Assert
        Assert.Overflows(() => date.PlusYears(Int32.MinValue));
        Assert.Overflows(() => date.PlusYears(Int32.MaxValue));
    }

    [Fact]
    public static void PlusYears_WithLimitValues_AtMinValue()
    {
        var min = XCivilDate.MinValue;
        int years = XCivilDate.MaxYear - XCivilDate.MinYear;
        var exp = new XCivilDate(XCivilDate.MaxYear, 1, 1);
        // Act & Assert
        Assert.Overflows(() => min.PlusYears(-1));
        Assert.Equal(min, min.PlusYears(0));
        Assert.Equal(exp, min.PlusYears(years));
        Assert.Overflows(() => min.PlusYears(years + 1));
    }

    [Fact]
    public static void PlusYears_WithLimitValues_AtMaxValue()
    {
        var max = XCivilDate.MaxValue;
        int years = XCivilDate.MaxYear - XCivilDate.MinYear;
        var exp = new XCivilDate(XCivilDate.MinYear, 12, 31);
        // Act & Assert
        Assert.Overflows(() => max.PlusYears(-years - 1));
        Assert.Equal(exp, max.PlusYears(-years));
        Assert.Equal(max, max.PlusYears(0));
        Assert.Overflows(() => max.PlusYears(1));
    }

    [Fact]
    public static void PlusYears_WithLimitValues()
    {
        var date = new XCivilDate(3, 4, 5);
        int minYears = XCivilDate.MinYear - date.Year;
        int maxYears = XCivilDate.MaxYear - date.Year;
        var earliest = new XCivilDate(XCivilDate.MinYear, 4, 5);
        var latest = new XCivilDate(XCivilDate.MaxYear, 4, 5);
        // Act & Assert
        Assert.Overflows(() => date.PlusYears(minYears - 1));
        Assert.Equal(earliest, date.PlusYears(minYears));
        Assert.Equal(latest, date.PlusYears(maxYears));
        Assert.Overflows(() => date.PlusYears(maxYears + 1));
    }

    [Fact]
    public static void CountYearsSince_DoesNotOverflow()
    {
        var min = XCivilDate.MinValue;
        var max = XCivilDate.MaxValue;
        int years = XCivilDate.MaxYear - XCivilDate.MinYear;
        // Act & Assert
        Assert.Equal(years, max.CountYearsSince(min));
        Assert.Equal(-years, min.CountYearsSince(max));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public static void PlusYears(YemodaPairAnd<int> info)
    {
        var (xdate, xother, years) = info;
        var date = CreateDate(xdate);
        var other = CreateDate(xother);

        // Act & Assert
        // 1) date + years -> other.
        Assert.Equal(other, date.PlusYears(years));

        // 2) other - years -> date.
        // The test passes because date is not an intercalary day which,
        // by the way, "implies" that other is not intercalary too.
        Assert.Equal(date, other.PlusYears(-years));

        // 3) other - date -> years.
        Assert.Equal(years, other.CountYearsSince(date));

        // 4) date - other -> -years.
        Assert.Equal(-years, date.CountYearsSince(other));
    }

    [Theory, MemberData(nameof(AddYearsCutOffData))]
    public static void PlusYears_CutOff(YemodaPairAnd<int> info)
    {
        var (xdate, xother, years) = info;
        var date = CreateDate(xdate);
        var other = CreateDate(xother);
        var dateBefore = date - 1;

        // Act & Assert
        // 1) date + years -> other.
        Assert.Equal(other, date.PlusYears(years));
        Assert.Equal(other, dateBefore.PlusYears(years));

        // 2) other - years does NOT return date but dateBefore.
        Assert.Equal(dateBefore, other.PlusYears(-years));

        // 3) other - date -> years.
        Assert.Equal(years, other.CountYearsSince(date));
        Assert.Equal(years, other.CountYearsSince(dateBefore));

        // 4) date - other -> -years.
        Assert.Equal(-years, date.CountYearsSince(other));
        Assert.Equal(-years, dateBefore.CountYearsSince(other));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public static void PlusYears_ZeroIsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = new XCivilDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, date.PlusYears(0));
        Assert.Equal(0, date.CountYearsSince(date));
    }

    [Theory, MemberData(nameof(DateDiffData))]
    public static void CountYearsSince(DateDiff info)
    {
        var (xstart, xend, years, _, _) = info;
        var start = CreateDate(xstart);
        var end = CreateDate(xend);
        // Act & Assert
        // 1) end - start -> years.
        Assert.Equal(years, end.CountYearsSince(start));

        // 2) start - end -> -years.
        Assert.Equal(-years, start.CountYearsSince(end));

        // Now, we verify the contract of the method.
        // We filter out test data that could underflow or underflow.
        if (years > 0 && start.Year + years + 1 <= XCivilDate.MaxYear)
        {
            Assert.True(start.PlusYears(years + 1) > end);
        }
        else if (years < 0 && start.Year + years - 1 >= XCivilDate.MinYear)
        {
            Assert.True(start.PlusYears(years - 1) < end);
        }
    }

    [Fact]
    public static void AddYears_IntegerOverflow()
    {
        var date = new XCivilDate(3, 4, 5);
        // Act & Assert
        // There is no integer underflow, only integer overflow.
        Assert.Overflows(() => XCivilDate.AddYears(date, Int32.MaxValue, out _));
    }

    [Fact]
    public static void AddYears_WithLimitValues_AtMinValue()
    {
        var min = XCivilDate.MinValue;
        int years = XCivilDate.MaxYear - XCivilDate.MinYear;
        var exp = new XCivilDate(XCivilDate.MaxYear, 1, 1);
        // Act & Assert
        Assert.Overflows(() => XCivilDate.AddYears(min, -1, out _));
        Assert.Equal(min, XCivilDate.AddYears(min, 0, out _));
        Assert.Equal(exp, XCivilDate.AddYears(min, years, out _));
        Assert.Overflows(() => XCivilDate.AddYears(min, years + 1, out _));
    }

    [Fact]
    public static void AddYears_WithLimitValues_AtMaxValue()
    {
        var max = XCivilDate.MaxValue;
        int years = XCivilDate.MaxYear - XCivilDate.MinYear;
        var exp = new XCivilDate(XCivilDate.MinYear, 12, 31);
        // Act & Assert
        Assert.Overflows(() => XCivilDate.AddYears(max, -years - 1, out _));
        Assert.Equal(exp, XCivilDate.AddYears(max, -years, out _));
        Assert.Equal(max, XCivilDate.AddYears(max, 0, out _));
        Assert.Overflows(() => XCivilDate.AddYears(max, 1, out _));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public static void AddYears(YemodaPairAnd<int> info)
    {
        var (xdate, xother, years) = info;
        var date = CreateDate(xdate);
        var other = CreateDate(xother);
        // Act & Assert
        // 1) date + years -> other.
        Assert.Equal(other, XCivilDate.AddYears(date, years, out int cutoff));
        Assert.Equal(0, cutoff);

        // 2) other - years -> date.
        Assert.Equal(date, XCivilDate.AddYears(other, -years, out cutoff));
        Assert.Equal(0, cutoff);
    }

    [Theory, MemberData(nameof(AddYearsCutOffData))]
    public static void AddYears_CutOff(YemodaPairAnd<int> info)
    {
        var (xdate, xother, years) = info;
        var date = CreateDate(xdate);
        var other = CreateDate(xother);
        var dateBefore = date - 1;

        // Act & Assert
        // 1) date + years -> other.
        Assert.Equal(other, XCivilDate.AddYears(date, years, out int cutoff));
        Assert.Equal(1, cutoff);

        // 2) other - years does NOT return date but dateBefore.
        Assert.Equal(dateBefore, XCivilDate.AddYears(other, -years, out cutoff));
        Assert.Equal(0, cutoff);
    }
}

public partial class XCivilDateTests // PlusMonths(), CountMonthsSince()
{
    [Fact]
    public static void PlusMonths_OverflowOrUnderflow()
    {
        var date = new XCivilDate(3, 4, 5);
        // Act & Assert
        Assert.Overflows(() => date.PlusMonths(Int32.MinValue));
        Assert.Overflows(() => date.PlusMonths(Int32.MaxValue));
    }

    [Fact]
    public static void PlusMonths_WithLimitValues_AtMinValue()
    {
        var min = XCivilDate.MinValue;
        int months = 11 + 12 * (XCivilDate.MaxYear - XCivilDate.MinYear);
        var exp = new XCivilDate(XCivilDate.MaxYear, 12, 1);
        // Act & Assert
        Assert.Overflows(() => min.PlusMonths(-1));
        Assert.Equal(min, min.PlusMonths(0));
        Assert.Equal(exp, min.PlusMonths(months));
        Assert.Overflows(() => min.PlusMonths(months + 1));
    }

    [Fact]
    public static void PlusMonths_WithLimitValues_AtMaxValue()
    {
        var max = XCivilDate.MaxValue;
        int months = 11 + 12 * (XCivilDate.MaxYear - XCivilDate.MinYear);
        var exp = new XCivilDate(XCivilDate.MinYear, 1, 31);
        // Act & Assert
        Assert.Overflows(() => max.PlusMonths(-months - 1));
        Assert.Equal(exp, max.PlusMonths(-months));
        Assert.Equal(max, max.PlusMonths(0));
        Assert.Overflows(() => max.PlusMonths(1));
    }

    [Fact]
    public static void PlusMonths_WithLimitValues()
    {
        var date = new XCivilDate(3, 4, 5);
        int minMonths = XCivilDate.MinValue.CountMonthsSince(date);
        int maxMonths = XCivilDate.MaxValue.CountMonthsSince(date);
        var earliest = new XCivilDate(XCivilDate.MinYear, 1, 5);
        var latest = new XCivilDate(XCivilDate.MaxYear, 12, 5);
        // Act & Assert
        Assert.Overflows(() => date.PlusMonths(minMonths - 1));
        Assert.Equal(earliest, date.PlusMonths(minMonths));
        Assert.Equal(latest, date.PlusMonths(maxMonths));
        Assert.Overflows(() => date.PlusMonths(maxMonths + 1));
    }

    [Fact]
    public static void CountMonthsSince_DoesNotOverflow()
    {
        var min = XCivilDate.MinValue;
        var max = XCivilDate.MaxValue;
        int months = 11 + 12 * (XCivilDate.MaxYear - XCivilDate.MinYear);
        // Act & Assert
        Assert.Equal(months, max.CountMonthsSince(min));
        Assert.Equal(-months, min.CountMonthsSince(max));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public static void PlusMonths(YemodaPairAnd<int> info)
    {
        var (xdate, xother, months) = info;
        var date = CreateDate(xdate);
        var other = CreateDate(xother);
        // Act & Assert
        // 1) date + months -> other.
        Assert.Equal(other, date.PlusMonths(months));

        // 2) other - months -> date.
        Assert.Equal(date, other.PlusMonths(-months));

        // 3) other - date -> months.
        Assert.Equal(months, other.CountMonthsSince(date));

        // 4) date - other -> -months.
        Assert.Equal(-months, date.CountMonthsSince(other));
    }

    [Theory, MemberData(nameof(AddMonthsCutOffData))]
    public static void PlusMonths_CutOff(YemodaPairAnd<int> info)
    {
        var (xdate, xother, months) = info;
        var date = CreateDate(xdate);
        var other = CreateDate(xother);
        var rev = date.WithDay(other.Day);

        // Act & Assert
        // 1) date + months -> other.
        Assert.Equal(other, date.PlusMonths(months));

        // 2) other - months does NOT return date but a cut-off date.
        Assert.Equal(rev, other.PlusMonths(-months));

        // 3) other - date -> months.
        Assert.Equal(months, other.CountMonthsSince(date));

        // 4) date - other -> -months.
        Assert.Equal(-months, date.CountMonthsSince(other));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public static void PlusMonths_ZeroIsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = new XCivilDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, date.PlusMonths(0));
        Assert.Equal(0, date.CountMonthsSince(date));
    }

    [Theory, MemberData(nameof(DateDiffData))]
    public static void CountMonthsSince(DateDiff info)
    {
        var (xstart, xend, years, months, _) = info;
        var start = CreateDate(xstart);
        var end = CreateDate(xend);
        months += 12 * years;
        // Act & Assert
        // 1) end - start -> months.
        Assert.Equal(months, end.CountMonthsSince(start));

        // 2) start - end -> -months.
        Assert.Equal(-months, start.CountMonthsSince(end));

        // Now, we verify the contract of the method.
        // We do not bother with strict limits.
        if (months > 0 && end.Year < XCivilDate.MaxYear)
        {
            Assert.True(start.PlusMonths(months + 1) > end);
        }
        else if (months < 0 && end.Year > XCivilDate.MinYear)
        {
            Assert.True(start.PlusMonths(months - 1) < end);
        }
    }

    [Fact]
    public static void AddMonths_IntegerOverflow()
    {
        var date = new XCivilDate(3, 4, 5);
        // Act & Assert
        // There is no integer underflow, only integer overflow.
        Assert.Overflows(() => XCivilDate.AddMonths(date, Int32.MaxValue, out _));
    }

    [Fact]
    public static void AddMonths_WithLimitValues_AtMinValue()
    {
        var min = XCivilDate.MinValue;
        int months = 11 + 12 * (XCivilDate.MaxYear - XCivilDate.MinYear);
        var exp = new XCivilDate(XCivilDate.MaxYear, 12, 1);
        // Act & Assert
        Assert.Overflows(() => XCivilDate.AddMonths(min, -1, out _));
        Assert.Equal(min, XCivilDate.AddMonths(min, 0, out _));
        Assert.Equal(exp, XCivilDate.AddMonths(min, months, out _));
        Assert.Overflows(() => XCivilDate.AddMonths(min, months + 1, out _));
    }

    [Fact]
    public static void AddMonths_WithLimitValues_AtMaxValue()
    {
        var max = XCivilDate.MaxValue;
        int months = 11 + 12 * (XCivilDate.MaxYear - XCivilDate.MinYear);
        var exp = new XCivilDate(XCivilDate.MinYear, 1, 31);
        // Act & Assert
        Assert.Overflows(() => XCivilDate.AddMonths(max, -months - 1, out _));
        Assert.Equal(exp, XCivilDate.AddMonths(max, -months, out _));
        Assert.Equal(max, XCivilDate.AddMonths(max, 0, out _));
        Assert.Overflows(() => XCivilDate.AddMonths(max, 1, out _));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public static void AddMonths(YemodaPairAnd<int> info)
    {
        var (xdate, xother, months) = info;
        var date = CreateDate(xdate);
        var other = CreateDate(xother);
        // Act & Assert
        // 1) date + months -> other.
        Assert.Equal(other, XCivilDate.AddMonths(date, months, out int cutoff));
        Assert.Equal(0, cutoff);

        // 2) other - months -> date.
        Assert.Equal(date, XCivilDate.AddMonths(other, -months, out cutoff));
        Assert.Equal(0, cutoff);
    }

    [Theory, MemberData(nameof(AddMonthsCutOffData))]
    public static void AddMonths_CutOff(YemodaPairAnd<int> info)
    {
        var (xdate, xother, months) = info;
        var date = CreateDate(xdate);
        var other = CreateDate(xother);
        var cutoffDate = date.WithDay(other.Day);
        int err = date - cutoffDate;

        // Act & Assert
        // 1) date + months -> other.
        Assert.Equal(other, XCivilDate.AddMonths(date, months, out int cutoff));
        Assert.Equal(err, cutoff);

        // 2) other - months does NOT return date but a cut-off date.
        Assert.Equal(cutoffDate, XCivilDate.AddMonths(other, -months, out cutoff));
        Assert.Equal(0, cutoff);
    }
}

public partial class XCivilDateTests // Subtract()
{
    [Theory, MemberData(nameof(DateDiffData))]
    public static void Subtract(DateDiff info)
    {
        var (xstart, xend, years, months, days) = info;
        var start = CreateDate(xstart);
        var end = CreateDate(xend);
        // Act & Assert
        var (ys, ms, ds) = XCivilDate.Subtract(end, start);
        Assert.Equal(years, ys);
        Assert.Equal(months, ms);
        Assert.Equal(days, ds);

        var target = start.PlusMonths(12 * ys + ms).PlusDays(ds);
        Assert.Equal(end, target);
    }

    [Theory, MemberData(nameof(DateDiffCutOffData))]
    public static void Subtract_CutOff(DateDiff info)
    {
        var (xstart, xend, years, months, days) = info;
        var start = CreateDate(xstart);
        var end = CreateDate(xend);
        // Act & Assert
        var (ys, ms, ds) = XCivilDate.Subtract(end, start);
        Assert.Equal(years, ys);
        Assert.Equal(months, ms);
        Assert.Equal(days, ds);

        var target = start.PlusMonths(12 * ys + ms).PlusDays(ds);
        Assert.Equal(end, target);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public static void Subtract_WithIdenticalDates(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = new XCivilDate(y, m, d);
        // Act
        var (y0, m0, d0) = XCivilDate.Subtract(date, date);
        // Assert
        Assert.Equal(0, y0);
        Assert.Equal(0, m0);
        Assert.Equal(0, d0);
    }
}
