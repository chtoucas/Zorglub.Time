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

    public abstract DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData { get; }

    public abstract DataGroup<DateInfo> DateInfoData { get; }
    public abstract DataGroup<MonthInfo> MonthInfoData { get; }
    public abstract DataGroup<YearInfo> YearInfoData { get; }
    /// <remarks>
    /// Override this property if the schema does not support all years in the default list.
    /// </remarks>
    public virtual DataGroup<CenturyInfo> CenturyInfoData => YearNumberingDataSet.CenturyInfoData;

    private DataGroup<YemodaAnd<int>>? _daysInYearAfterDateData;
    public virtual DataGroup<YemodaAnd<int>> DaysInYearAfterDateData =>
        _daysInYearAfterDateData ??= InitDaysInYearAfterDateData(DateInfoData);

    private DataGroup<YemodaAnd<int>>? _daysInMonthAfterDateData;
    public virtual DataGroup<YemodaAnd<int>> DaysInMonthAfterDateData =>
        _daysInMonthAfterDateData ??= InitDaysInMonthAfterDateData(DateInfoData);

    private DataGroup<Yemoda>? _startOfYearPartsData;
    public virtual DataGroup<Yemoda> StartOfYearPartsData =>
        _startOfYearPartsData ??= InitStartOfYearParts(EndOfYearPartsData);

    public abstract DataGroup<Yemoda> EndOfYearPartsData { get; }

    public abstract DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; }

    private DataGroup<YearDaysSinceEpoch>? _endOfYearDaysSinceEpochData;
    public virtual DataGroup<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData =>
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
    private static DataGroup<Yemoda> InitStartOfYearParts(DataGroup<Yemoda> source)
    {
        Requires.NotNull(source);

        var data = new DataGroup<Yemoda>();
        foreach (var item in source.AsEnumerableT())
        {
            data.Add(new Yemoda(item.Year, 1, 1));
        }
        return data;
    }

    [Pure]
    private static DataGroup<YearDaysSinceEpoch> InitEndOfYearDaysSinceEpochData(DataGroup<YearDaysSinceEpoch> source)
    {
        Requires.NotNull(source);

        var data = new DataGroup<YearDaysSinceEpoch>();
        foreach (var (y, daysSinceEpoch) in source.AsEnumerableT())
        {
            // endOfYear = startOfNextYear - 1.
            data.Add(new YearDaysSinceEpoch(y - 1, daysSinceEpoch - 1));
        }
        return data;
    }

    [Pure]
    private DataGroup<YemodaAnd<int>> InitDaysInYearAfterDateData(DataGroup<DateInfo> source)
    {
        Requires.NotNull(source);

        var sch = Schema;
        var data = new DataGroup<YemodaAnd<int>>();
        foreach (var (y, m, d, doy) in source.AsEnumerableT())
        {
            int daysInYearAfter = sch.CountDaysInYear(y) - doy;
            data.Add(new(y, m, d, daysInYearAfter));
        }
        return data;
    }

    [Pure]
    private DataGroup<YemodaAnd<int>> InitDaysInMonthAfterDateData(DataGroup<DateInfo> source)
    {
        Requires.NotNull(source);

        var sch = Schema;
        var data = new DataGroup<YemodaAnd<int>>();
        foreach (var item in source.AsEnumerableT())
        {
            var (y, m, d) = item.Yemoda;
            int daysInMonthAfter = sch.CountDaysInMonth(y, m) - d;
            data.Add(new(y, m, d, daysInMonthAfter));
        }
        return data;
    }

    #endregion
}
