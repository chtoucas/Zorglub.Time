// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

// REVIEW(data): there is a "one-to-one" correspondance between schemas and
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

    #region ICalendricalDataSet

    public int SampleCommonYear { get; }
    public int SampleLeapYear { get; }

    public abstract TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData { get; }

    public abstract TheoryData<DateInfo> DateInfoData { get; }
    public abstract TheoryData<MonthInfo> MonthInfoData { get; }
    public abstract TheoryData<YearInfo> YearInfoData { get; }
    /// <remarks>
    /// Override this property if the schema does not support all years in the default list.
    /// </remarks>
    public virtual TheoryData<CenturyInfo> CenturyInfoData => YearNumberingDataSet.CenturyInfoData;

    private TheoryData<YemodaAnd<int>>? _daysInYearAfterDateData;
    public virtual TheoryData<YemodaAnd<int>> DaysInYearAfterDateData =>
        _daysInYearAfterDateData ??= InitDaysInYearAfterDateData(DateInfoData);

    private TheoryData<YemodaAnd<int>>? _daysInMonthAfterDateData;
    public virtual TheoryData<YemodaAnd<int>> DaysInMonthAfterDateData =>
        _daysInMonthAfterDateData ??= InitDaysInMonthAfterDateData(DateInfoData);

    private TheoryData<Yemoda>? _startOfYearPartsData;
    public virtual TheoryData<Yemoda> StartOfYearPartsData =>
        _startOfYearPartsData ??= InitStartOfYearParts(EndOfYearPartsData);

    public abstract TheoryData<Yemoda> EndOfYearPartsData { get; }

    public abstract TheoryData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; }

    private TheoryData<YearDaysSinceEpoch>? _endOfYearDaysSinceEpochData;
    public virtual TheoryData<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData =>
        _endOfYearDaysSinceEpochData ??= InitEndOfYearDaysSinceEpochData(StartOfYearDaysSinceEpochData);

    public abstract TheoryData<int, int> InvalidMonthFieldData { get; }
    public abstract TheoryData<int, int, int> InvalidDayFieldData { get; }
    public abstract TheoryData<int, int> InvalidDayOfYearFieldData { get; }

    #endregion
    #region Helpers
    // We could have removed the parameter "source" from InitStartOfYearParts
    // and use the property EndOfYearPartsData instead, but this is not such a
    // good idea. Indeed, I prefer to make it clear that, for the method to
    // work properly, "source" must not be null.
    // This remark applies to the other helpers.

    [Pure]
    private static TheoryData<Yemoda> InitStartOfYearParts(TheoryData<Yemoda> source)
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
    private static TheoryData<YearDaysSinceEpoch> InitEndOfYearDaysSinceEpochData(TheoryData<YearDaysSinceEpoch> source)
    {
        Requires.NotNull(source);

        var data = new TheoryData<YearDaysSinceEpoch>();
        foreach (var item in source)
        {
            var (y, daysSinceEpoch) = (YearDaysSinceEpoch)item[0];
            // endOfYear = startOfNextYear - 1.
            data.Add(new YearDaysSinceEpoch(y - 1, daysSinceEpoch - 1));
        }
        return data;
    }

    [Pure]
    private TheoryData<YemodaAnd<int>> InitDaysInYearAfterDateData(TheoryData<DateInfo> source)
    {
        Requires.NotNull(source);

        var sch = Schema;
        var data = new TheoryData<YemodaAnd<int>>();
        foreach (var info in source)
        {
            var (y, m, d, doy) = (DateInfo)info[0];
            int daysInYearAfter = sch.CountDaysInYear(y) - doy;
            data.Add(new(y, m, d, daysInYearAfter));
        }
        return data;
    }

    [Pure]
    private TheoryData<YemodaAnd<int>> InitDaysInMonthAfterDateData(TheoryData<DateInfo> source)
    {
        Requires.NotNull(source);

        var sch = Schema;
        var data = new TheoryData<YemodaAnd<int>>();
        foreach (var info in source)
        {
            var (y, m, d) = ((DateInfo)info[0]).Yemoda;
            int daysInMonthAfter = sch.CountDaysInMonth(y, m) - d;
            data.Add(new(y, m, d, daysInMonthAfter));
        }
        return data;
    }

    #endregion
}
