// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

// TODO(data): DaysSinceEpochInfoData should include data before the epoch, then
// we should review the facts classes to ensure that we use it to test negative years.

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

    // Both years should be > 0. The reason is that we're going to filter the
    // data to build the calendar datasets, and most of them will filter out
    // negative years.
    // To test negative values, we shall use DaysSinceEpochInfoData, so it's
    // important to include data before the epoch.
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

    // TODO(data): derived classes should at least override ConsecutiveDaysData
    // and ConsecutiveDaysOrdinalData.
    // The Gregorian schema overrides all math-related props.
    // Other schemas overriding AddDaysData:
    // - Lunisolar
    // - TabularIslamic (model for lunar schema)
    // Other schemas overriding ConsecutiveDaysData & ConsecutiveDaysOrdinalData:
    // - Coptic12/13
    // - Lunisolar
    // - Pax
    // - Positivist (model for regular13 schema)
    // - TabularIslamic (model for lunar schema)
    public virtual DataGroup<YemodaPairAnd<int>> AddDaysData => new(AddDaysSamples);
    public virtual DataGroup<YemodaPair> ConsecutiveDaysData => new(ConsecutiveDaysSamples);
    public virtual DataGroup<YedoyPairAnd<int>> AddDaysOrdinalData => new(AddDaysOrdinalSamples);
    public virtual DataGroup<YedoyPair> ConsecutiveDaysOrdinalData => new(ConsecutiveDaysOrdinalSamples);

    public virtual DataGroup<YemodaPairAnd<int>> AddYearsData => new(AddYearsSamples);
    public virtual DataGroup<YemodaPairAnd<int>> AddMonthsData => new(AddMonthsSamples);

    /// <remarks>NB: First and Second belongs to the same month.</remarks>
    protected static IEnumerable<YemodaPairAnd<int>> AddDaysSamples { get; } = InitAddDaysSamples();
    /// <remarks>NB: First and Second belongs to the same month.</remarks>
    protected static IEnumerable<YemodaPair> ConsecutiveDaysSamples { get; } = InitConsecutiveDaysSamples();
    /// <remarks>NB: First and Second belongs to the same year.</remarks>
    protected static IEnumerable<YedoyPairAnd<int>> AddDaysOrdinalSamples { get; } = InitAddDaysOrdinalSamples();
    /// <remarks>NB: First and Second belongs to the same year.</remarks>
    protected static IEnumerable<YedoyPair> ConsecutiveDaysOrdinalSamples { get; } = InitConsecutiveDaysOrdinalSamples();

    /// <remarks>NB: The data is unambiguous.</remarks>
    protected static IEnumerable<YemodaPairAnd<int>> AddYearsSamples { get; } = InitAddYearsSamples();
    /// <remarks>NB: The data is unambiguous First and Second belongs to the same year.</remarks>
    protected static IEnumerable<YemodaPairAnd<int>> AddMonthsSamples { get; } = InitAddMonthsSamples();
}

public partial class SchemaDataSet // Helpers
{
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
}

public partial class SchemaDataSet // Math helpers
{
    private static List<YemodaPairAnd<int>> InitAddDaysSamples()
    {
        // Hypothesis: April is at least 28-days long.
        // new(new(3, 4, 14), new(3, 4, 1), -13)
        // new(new(3, 4, 14), new(3, 4, 2), -12)
        // ...
        // new(new(3, 4, 14), new(3, 4, 27), 13)
        // new(new(3, 4, 14), new(3, 4, 28), 14)
        const int
            SampleSize = 28,
            Year = 3,
            Month = 4,
            Day = SampleSize / 2;

        var start = new Yemoda(Year, Month, Day);

        var data = new List<YemodaPairAnd<int>>();
        for (int days = -Day + 1; days <= Day; days++)
        {
            var end = new Yemoda(Year, Month, Day + days);
            data.Add(new YemodaPairAnd<int>(start, end, days));
        }
        return data;
    }

    private static List<YemodaPair> InitConsecutiveDaysSamples()
    {
        // Hypothesis: April is at least 28-days long.
        // new(new(3, 4, 1), new(3, 4, 2))
        // new(new(3, 4, 2), new(3, 4, 3))
        // ...
        // new(new(3, 4, 27), new(3, 4, 28))
        const int
            SampleSize = 27,
            Year = 3,
            Month = 4;

        var data = new List<YemodaPair>();
        for (int d = 1; d <= SampleSize; d++)
        {
            var date = new Yemoda(Year, Month, d);
            var dateAfter = new Yemoda(Year, Month, d + 1);
            data.Add(new YemodaPair(date, dateAfter));
        }
        return data;
    }

    private static List<YedoyPairAnd<int>> InitAddDaysOrdinalSamples()
    {
        // Samples should cover the two first months.
        // new(new(3, 35), new(3, 1), -34)
        // new(new(3, 35), new(3, 2), -33)
        // ...
        // new(new(3, 35), new(3, 69), 34)
        // new(new(3, 35), new(3, 70), 35)
        const int
            SampleSize = 70,
            Year = 3,
            DayOfYear = SampleSize / 2;

        var start = new Yedoy(Year, DayOfYear);

        var data = new List<YedoyPairAnd<int>>();
        for (int days = -DayOfYear + 1; days <= DayOfYear; days++)
        {
            var end = new Yedoy(Year, DayOfYear + days);
            data.Add(new YedoyPairAnd<int>(start, end, days));
        }
        return data;
    }

    private static List<YedoyPair> InitConsecutiveDaysOrdinalSamples()
    {
        // Samples should cover the two first months.
        // new(new(3, 1), new(3, 2))
        // new(new(3, 2), new(3, 3))
        // ...
        // new(new(3, 69), new(3, 70))
        // new(new(3, 70), new(3, 71))
        const int
            SampleSize = 70,
            Year = 3;

        var data = new List<YedoyPair>();
        for (int doy = 1; doy <= SampleSize; doy++)
        {
            var date = new Yedoy(Year, doy);
            var dateAfter = new Yedoy(Year, doy + 1);
            data.Add(new YedoyPair(date, dateAfter));
        }
        return data;
    }

    private static List<YemodaPairAnd<int>> InitAddYearsSamples()
    {
        // new(new(5, 4, 5), new(1, 4, 5), -4)
        // new(new(5, 4, 5), new(2, 4, 5), -3)
        // ...
        // new(new(5, 4, 5), new(9, 4, 5), 4)
        // new(new(5, 4, 5), new(10, 4, 5), 5)
        const int
            SampleSize = 10,
            Year = SampleSize / 2,
            Month = 4,
            Day = 5;

        var start = new Yemoda(Year, Month, Day);

        var data = new List<YemodaPairAnd<int>>();
        for (int years = -Year + 1; years <= Year; years++)
        {
            var end = new Yemoda(Year + years, Month, Day);
            data.Add(new YemodaPairAnd<int>(start, end, years));
        }
        return data;
    }

    private static List<YemodaPairAnd<int>> InitAddMonthsSamples()
    {
        // Hypothesis: a year is at least 12-months long.
        // new(new(3, 6, 5), new(5, 1, 5), -5)
        // new(new(3, 6, 5), new(5, 2, 5), -4)
        // ...
        // new(new(3, 6, 5), new(5, 11, 5), 5)
        // new(new(3, 6, 5), new(5, 12, 5), 6)
        const int
            SampleSize = 12,
            Year = 3,
            Month = SampleSize / 2,
            Day = 5;

        var start = new Yemoda(Year, Month, Day);

        var data = new List<YemodaPairAnd<int>>();
        for (int months = -Month + 1; months <= Month; months++)
        {
            var end = new Yemoda(Year, Month + months, Day);
            data.Add(new YemodaPairAnd<int>(start, end, months));
        }
        return data;
    }
}
