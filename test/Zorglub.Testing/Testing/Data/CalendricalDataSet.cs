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

    public virtual DataGroup<YemodaAnd<int>> DaysInYearAfterDateData =>
        MapToDaysInYearAfterDateData(DateInfoData);
    public virtual DataGroup<YemodaAnd<int>> DaysInMonthAfterDateData =>
        MapToDaysInMonthAfterDateData(DateInfoData);

    public virtual DataGroup<Yemoda> StartOfYearPartsData =>
        MapToStartOfYearParts(EndOfYearPartsData);
    public abstract DataGroup<Yemoda> EndOfYearPartsData { get; }

    public abstract DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; }
    public virtual DataGroup<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData =>
        MapToEndOfYearDaysSinceEpochData(StartOfYearDaysSinceEpochData);

    public abstract TheoryData<int, int> InvalidMonthFieldData { get; }
    public abstract TheoryData<int, int, int> InvalidDayFieldData { get; }
    public abstract TheoryData<int, int> InvalidDayOfYearFieldData { get; }

    #region Helpers
    // We could have removed the parameter "source" from InitStartOfYearParts
    // and use the property EndOfYearPartsData instead, but this is not such a
    // good idea. Indeed, I prefer to make it clear that, for the method to
    // work properly, "source" must not be null.
    // This remark applies to the other helpers.

    [Pure]
    private static DataGroup<Yemoda> MapToStartOfYearParts(DataGroup<Yemoda> source)
    {
        Requires.NotNull(source);

        return source.SelectT(Selector);

        static Yemoda Selector(Yemoda x) => new(x.Year, 1, 1);
    }

    [Pure]
    private static DataGroup<YearDaysSinceEpoch> MapToEndOfYearDaysSinceEpochData(DataGroup<YearDaysSinceEpoch> source)
    {
        Requires.NotNull(source);

        return source.SelectT(Selector);

        static YearDaysSinceEpoch Selector(YearDaysSinceEpoch x)
        {
            var (y, daysSinceEpoch) = x;
            return new YearDaysSinceEpoch(y - 1, daysSinceEpoch - 1);
        }
    }

    [Pure]
    private DataGroup<YemodaAnd<int>> MapToDaysInYearAfterDateData(DataGroup<DateInfo> source)
    {
        Requires.NotNull(source);

        var sch = Schema;
        return source.SelectT(Selector);

        YemodaAnd<int> Selector(DateInfo x)
        {
            var (y, m, d, doy) = x;
            int daysInYearAfter = sch.CountDaysInYear(y) - doy;
            return new YemodaAnd<int>(y, m, d, daysInYearAfter);
        }
    }

    [Pure]
    private DataGroup<YemodaAnd<int>> MapToDaysInMonthAfterDateData(DataGroup<DateInfo> source)
    {
        Requires.NotNull(source);

        var sch = Schema;
        return source.SelectT(Selector);

        YemodaAnd<int> Selector(DateInfo x)
        {
            var (y, m, d) = x.Yemoda;
            int daysInMonthAfter = sch.CountDaysInMonth(y, m) - d;
            return new YemodaAnd<int>(y, m, d, daysInMonthAfter);
        }
    }

    #endregion
}
