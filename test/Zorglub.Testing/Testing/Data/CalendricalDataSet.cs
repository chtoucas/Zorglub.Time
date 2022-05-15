﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

// TODO(data): there is a "one-to-one" correspondance between schemas and
// datasets, therefore one should be able to verify that a dataset is used in
// the right context.

public abstract class CalendricalDataSet : ICalendricalDataSet
{
    protected CalendricalDataSet(ICalendricalSchema schema, int commonYear, int leapYear)
    {
        Schema = schema ?? throw new ArgumentNullException(nameof(schema));

        SampleCommonYear = commonYear;
        SampleLeapYear = leapYear;
    }

    public ICalendricalSchema Schema { get; }

    public int SampleCommonYear { get; }
    public int SampleLeapYear { get; }

    public abstract TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData { get; }

    public abstract TheoryData<DateInfo> DateInfoData { get; }
    public abstract TheoryData<MonthInfo> MonthInfoData { get; }
    public abstract TheoryData<YearInfo> YearInfoData { get; }
    // Override this property if the schema does not support all years in the default list.
    public virtual TheoryData<CenturyInfo> CenturyInfoData => YearNumberingDataSet.CenturyInfoData;

    private TheoryData<YemodaAnd<int>>? _daysInYearAfterDateData;
    public virtual TheoryData<YemodaAnd<int>> DaysInYearAfterDateData =>
        _daysInYearAfterDateData ??= GetDaysInYearAfterDateData(DateInfoData, Schema);

    private TheoryData<YemodaAnd<int>>? _daysInMonthAfterDateData;
    public virtual TheoryData<YemodaAnd<int>> DaysInMonthAfterDateData =>
        _daysInMonthAfterDateData ??= GetDaysInMonthAfterDateData(DateInfoData, Schema);

    private TheoryData<Yemoda>? _startOfYearPartsData;
    public virtual TheoryData<Yemoda> StartOfYearPartsData =>
        _startOfYearPartsData ??= GetStartOfYearFromEndOfYear(EndOfYearPartsData);

    public abstract TheoryData<Yemoda> EndOfYearPartsData { get; }

    public abstract TheoryData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; }

    private TheoryData<YearDaysSinceEpoch>? _endOfYearDaysSinceEpochData;
    public virtual TheoryData<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData =>
        _endOfYearDaysSinceEpochData ??= GetEndOfYearFromStartOfYear(StartOfYearDaysSinceEpochData);

    public abstract TheoryData<int, int> InvalidMonthFieldData { get; }
    public abstract TheoryData<int, int, int> InvalidDayFieldData { get; }
    public abstract TheoryData<int, int> InvalidDayOfYearFieldData { get; }

    #region Helpers

    /// <summary>
    /// Converts a collection of <see cref="DaysSinceOriginInfo"/> to a set of data of type
    /// <see cref="DaysSinceEpochInfo"/>.
    /// </summary>
    [Pure]
    protected static TheoryData<DaysSinceEpochInfo> MapToDaysSinceEpochInfoData(
        IEnumerable<DaysSinceOriginInfo> source, DayNumber origin, DayNumber epoch)
    {
        Requires.NotNull(source);

        int shift = origin - epoch;
        var data = new TheoryData<DaysSinceEpochInfo>();
        foreach (var (daysSinceOrigin, y, m, d) in source)
        {
            data.Add(new DaysSinceEpochInfo(daysSinceOrigin + shift, y, m, d));
        }
        return data;
    }

    [Pure]
    private static TheoryData<YemodaAnd<int>> GetDaysInYearAfterDateData(
        TheoryData<DateInfo> source, ICalendricalSchema schema)
    {
        Requires.NotNull(source);
        Requires.NotNull(schema);

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
    private static TheoryData<YemodaAnd<int>> GetDaysInMonthAfterDateData(
        TheoryData<DateInfo> source, ICalendricalSchema schema)
    {
        Requires.NotNull(source);
        Requires.NotNull(schema);

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
    private static TheoryData<Yemoda> GetStartOfYearFromEndOfYear(TheoryData<Yemoda> source)
    {
        Requires.NotNull(source);

        var data = new TheoryData<Yemoda>();
        foreach (var item in source)
        {
            var y = ((Yemoda)item[0]).Year;
            data.Add(new Yemoda(y, 1, 1));
        }
        return data;
    }

    [Pure]
    private static TheoryData<YearDaysSinceEpoch> GetEndOfYearFromStartOfYear(TheoryData<YearDaysSinceEpoch> source)
    {
        Requires.NotNull(source);

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
