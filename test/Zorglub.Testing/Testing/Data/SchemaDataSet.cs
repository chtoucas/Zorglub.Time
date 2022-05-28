// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

/// <summary>
/// Defines test data for a schema and provides a base for derived classes.
/// </summary>
public abstract partial class SchemaDataSet : ICalendricalDataSet
{
    // ICalendricalKernel, not ICalendricalSchema, to prevent us from using any
    // fancy method. We keep ICalendricalSchema in the ctor to ensure that we
    // construct an CalendricalDataSet from a schema not a calendar.
    private readonly ICalendricalKernel _schema;

    protected SchemaDataSet(ICalendricalSchema schema, int commonYear, int leapYear)
    {
        _schema = schema ?? throw new ArgumentNullException(nameof(schema));

        SampleCommonYear = commonYear;
        SampleLeapYear = leapYear;
    }

    public int SampleCommonYear { get; }
    public int SampleLeapYear { get; }

    public abstract DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData { get; }

    public abstract DataGroup<DateInfo> DateInfoData { get; }
    public abstract DataGroup<MonthInfo> MonthInfoData { get; }
    public abstract DataGroup<YearInfo> YearInfoData { get; }
    /// <inheritdoc/>
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
    // We could have removed the parameter "source" from MapToStartOfYearParts()
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

        var sch = _schema;
        return source.SelectT(Selector);

        YemodaAnd<int> Selector(DateInfo x)
        {
            var (y, m, d, doy) = x;
            // Assumption: CountDaysInYear() is correct.
            int daysInYearAfter = sch.CountDaysInYear(y) - doy;
            return new YemodaAnd<int>(y, m, d, daysInYearAfter);
        }
    }

    [Pure]
    private DataGroup<YemodaAnd<int>> MapToDaysInMonthAfterDateData(DataGroup<DateInfo> source)
    {
        Requires.NotNull(source);

        var sch = _schema;
        return source.SelectT(Selector);

        YemodaAnd<int> Selector(DateInfo x)
        {
            var (y, m, d) = x.Yemoda;
            // Assumption: CountDaysInMonth() is correct.
            int daysInMonthAfter = sch.CountDaysInMonth(y, m) - d;
            return new YemodaAnd<int>(y, m, d, daysInMonthAfter);
        }
    }

    #endregion
}

public partial class SchemaDataSet // Math data
{
    // Samples for Yedoy should cover the two first months of a year.
    private const int SampleSize = 70; // Even number for InitDefaultAddDaysOrdinalData()
    private const int SampleYear = 3;

    public virtual DataGroup<YedoyPairAnd<int>> AddDaysOrdinalData => new(AddDaysOrdinalSamples);

    public virtual DataGroup<YedoyPair> ConsecutiveDaysOrdinalData => new(ConsecutiveDaysOrdinalSamples);

    protected static IEnumerable<YedoyPairAnd<int>> AddDaysOrdinalSamples { get; } =
        InitDefaultAddDaysOrdinalData();

    protected static IEnumerable<YedoyPair> ConsecutiveDaysOrdinalSamples { get; } =
        InitDefaultConsecutiveDaysOrdinalData();

    private static IEnumerable<YedoyPairAnd<int>> InitDefaultAddDaysOrdinalData()
    {
        const int Middle = SampleSize / 2;

        var data = new List<YedoyPairAnd<int>>();
        for (int i = -Middle + 1; i <= Middle; i++)
        {
            var first = new Yedoy(SampleYear, Middle);
            var second = new Yedoy(SampleYear, Middle + i);
            data.Add(new YedoyPairAnd<int>(first, second, i));
        }
        return data;
    }

    private static IEnumerable<YedoyPair> InitDefaultConsecutiveDaysOrdinalData()
    {
        var data = new List<YedoyPair>();
        for (int doy = 1; doy <= SampleSize; doy++)
        {
            var first = new Yedoy(SampleYear, doy);
            var second = new Yedoy(SampleYear, doy + 1);
            data.Add(new YedoyPair(first, second));
        }
        return data;
    }
}