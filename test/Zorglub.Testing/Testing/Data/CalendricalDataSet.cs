// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

// TODO(data): there is a "one-to-one" correspondance between schemas and
// datasets, therefore one should be able to verify that a dataset is used in
// the right context.

public abstract class CalendricalDataSet : ICalendricalDataSet
{
    protected CalendricalDataSet(int commonYear, int leapYear)
    {
        SampleCommonYear = commonYear;
        SampleLeapYear = leapYear;
    }

    public int SampleCommonYear { get; }
    public int SampleLeapYear { get; }

    public abstract TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData { get; }

    public abstract TheoryData<DateInfo> DateInfoData { get; }
    public abstract TheoryData<MonthInfo> MonthInfoData { get; }
    public abstract TheoryData<YearInfo> YearInfoData { get; }
    // Override this property if the schema does not support all years in the default list.
    public virtual TheoryData<CenturyInfo> CenturyInfoData => YearNumberingDataSet.CenturyInfoData;

    public abstract TheoryData<YemodaAnd<int>> DaysInYearAfterDateData { get; }
    public abstract TheoryData<YemodaAnd<int>> DaysInMonthAfterDateData { get; }

    private TheoryData<Yemoda>? _startOfYearPartsData;
    public TheoryData<Yemoda> StartOfYearPartsData =>
        _startOfYearPartsData ??= GetStartOfYearFromEndOfYear(EndOfYearPartsData);

    public abstract TheoryData<Yemoda> EndOfYearPartsData { get; }

    public abstract TheoryData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; }

    private TheoryData<YearDaysSinceEpoch>? _endOfYearDaysSinceEpochData;
    public TheoryData<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData =>
        _endOfYearDaysSinceEpochData ??= GetEndOfYearFromStartOfYear(StartOfYearDaysSinceEpochData);

    public abstract TheoryData<int, int> InvalidMonthFieldData { get; }
    public abstract TheoryData<int, int, int> InvalidDayFieldData { get; }
    public abstract TheoryData<int, int> InvalidDayOfYearFieldData { get; }

    #region Helpers

    /// <summary>
    /// Converts a collection of (DaysSinceEpoch, Year, Month, Day) to a set of data of type
    /// <see cref="DaysSinceEpochInfo"/>.
    /// </summary>
    [Pure]
    protected static TheoryData<DaysSinceEpochInfo> ConvertToDaysSinceEpochInfoData(
        IEnumerable<(int DaysSinceEpoch, int Year, int Month, int Day)> source!!)
    {
        var data = new TheoryData<DaysSinceEpochInfo>();
        foreach (var (daysSinceEpoch, y, m, d) in source)
        {
            data.Add(new DaysSinceEpochInfo(daysSinceEpoch, y, m, d));
        }
        return data;
    }

    /// <summary>
    /// Converts a collection of (DaysSinceOrigin, Year, Month, Day) to a set of data of type
    /// <see cref="DaysSinceEpochInfo"/>.
    /// </summary>
    [Pure]
    protected static TheoryData<DaysSinceEpochInfo> ConvertToDaysSinceEpochInfoData(
        IEnumerable<(int DaysSinceOrigin, int Year, int Month, int Day)> source!!, DayNumber origin, DayNumber epoch)
    {
        int shift = origin - epoch;
        var data = new TheoryData<DaysSinceEpochInfo>();
        foreach (var (daysSinceOrigin, y, m, d) in source)
        {
            data.Add(new DaysSinceEpochInfo(daysSinceOrigin + shift, y, m, d));
        }
        return data;
    }

    // REVIEW(data): at some point, we should no longer use the methods
    // - GetDaysInYearAfterDateData()
    // - GetDaysInMonthAfterDateData()
    // to initialize the props.
    // Other option: always initialize the props here, for this to work, we need
    // the schema in the constructor.
    [Pure]
    protected static TheoryData<YemodaAnd<int>> GetDaysInYearAfterDateData(
        TheoryData<DateInfo> source!!, ICalendricalSchema schema!!)
    {
        var data = new TheoryData<YemodaAnd<int>>();
        foreach (var info in source)
        {
            var (y, m, d, doy) = (DateInfo)info[0];
            int daysInYear = schema.CountDaysInYear(y);
            // CountDaysInYear(y) - doy
            data.Add(new(y, m, d, daysInYear - doy));
        }
        return data;
    }

    [Pure]
    protected static TheoryData<YemodaAnd<int>> GetDaysInMonthAfterDateData(
        TheoryData<DateInfo> source!!, ICalendricalSchema schema!!)
    {
        var data = new TheoryData<YemodaAnd<int>>();
        foreach (var info in source)
        {
            var (y, m, d) = ((DateInfo)info[0]).Yemoda;
            int daysInMonth = schema.CountDaysInMonth(y, m);
            // CountDaysInMonth(y, m) - d
            data.Add(new(y, m, d, daysInMonth - d));
        }
        return data;
    }

    [Pure]
    private static TheoryData<Yemoda> GetStartOfYearFromEndOfYear(TheoryData<Yemoda> source!!)
    {
        var data = new TheoryData<Yemoda>();
        foreach (var item in source)
        {
            var y = ((Yemoda)item[0]).Year;
            data.Add(new Yemoda(y, 1, 1));
        }
        return data;
    }

    [Pure]
    private static TheoryData<YearDaysSinceEpoch> GetEndOfYearFromStartOfYear(TheoryData<YearDaysSinceEpoch> source!!)
    {
        var data = new TheoryData<YearDaysSinceEpoch>();
        foreach (var item in source)
        {
            var (y, daysSinceEpoch) = (YearDaysSinceEpoch)item[0];
            data.Add(new YearDaysSinceEpoch(y - 1, daysSinceEpoch - 1));
        }
        return data;
    }

    #endregion
}
