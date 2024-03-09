// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Data;

/// <summary>
/// Provides static access to calendrical data.
/// </summary>
public abstract class CalendricalDataConsumer<TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    // NB: the order in which the static fields are written is important.
    private static readonly TDataSet s_DataSet = TDataSet.Instance;
    private static readonly DataSetAdapter s_Adapter = new(s_DataSet);

    protected CalendricalDataConsumer() { }

    protected static TDataSet DataSet => s_DataSet;

    protected static int SampleCommonYear { get; } = s_DataSet.SampleCommonYear;
    protected static int SampleLeapYear { get; } = s_DataSet.SampleLeapYear;

    public static XunitData<MonthsSinceEpochInfo> MonthsSinceEpochInfoData => s_Adapter.MonthsSinceEpochInfoData;
    public static XunitData<DaysSinceEpochInfo> DaysSinceEpochInfoData => s_Adapter.DaysSinceEpochInfoData;

    public static XunitData<DateInfo> DateInfoData => s_Adapter.DateInfoData;
    public static XunitData<MonthInfo> MonthInfoData => s_Adapter.MonthInfoData;
    public static XunitData<YearInfo> YearInfoData => s_Adapter.YearInfoData;
    public static XunitData<CenturyInfo> CenturyInfoData => s_Adapter.CenturyInfoData;

    public static XunitData<YemodaAnd<int>> DaysInYearAfterDateData => s_Adapter.DaysInYearAfterDateData;
    public static XunitData<YemodaAnd<int>> DaysInMonthAfterDateData => s_Adapter.DaysInMonthAfterDateData;

    public static XunitData<Yemoda> StartOfYearPartsData => s_Adapter.StartOfYearPartsData;
    public static XunitData<Yemoda> EndOfYearPartsData => s_Adapter.EndOfYearPartsData;

    public static XunitData<YearMonthsSinceEpoch> StartOfYearMonthsSinceEpochData => s_Adapter.StartOfYearMonthsSinceEpochData;
    public static XunitData<YearMonthsSinceEpoch> EndOfYearMonthsSinceEpochData => s_Adapter.EndOfYearMonthsSinceEpochData;

    public static XunitData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData => s_Adapter.StartOfYearDaysSinceEpochData;
    public static XunitData<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData => s_Adapter.EndOfYearDaysSinceEpochData;

    public static TheoryData<int, int> InvalidMonthFieldData => s_DataSet.InvalidMonthFieldData;
    public static TheoryData<int, int, int> InvalidDayFieldData => s_DataSet.InvalidDayFieldData;
    public static TheoryData<int, int> InvalidDayOfYearFieldData => s_DataSet.InvalidDayOfYearFieldData;

    public static XunitData<YemodaPair> ConsecutiveDaysData => s_Adapter.ConsecutiveDaysData;
    public static XunitData<YedoyPair> ConsecutiveDaysOrdinalData => s_Adapter.ConsecutiveDaysOrdinalData;
    public static XunitData<YemoPair> ConsecutiveMonthsData => s_Adapter.ConsecutiveMonthsData;

    public static XunitData<YemodaPairAnd<int>> AddDaysData => s_Adapter.AddDaysData;
    public static XunitData<YemodaPairAnd<int>> AddMonthsData => s_Adapter.AddMonthsData;
    public static XunitData<YemodaPairAnd<int>> AddYearsData => s_Adapter.AddYearsData;
    public static XunitData<YedoyPairAnd<int>> AddDaysOrdinalData => s_Adapter.AddDaysOrdinalData;
    public static XunitData<YedoyPairAnd<int>> AddYearsOrdinalData => s_Adapter.AddYearsOrdinalData;
    public static XunitData<YemoPairAnd<int>> AddMonthsMonthData => s_Adapter.AddMonthsMonthData;
    public static XunitData<YemoPairAnd<int>> AddYearsMonthData => s_Adapter.AddYearsMonthData;

    public static XunitData<YemodaPairAnd<int>> CountMonthsBetweenData => s_Adapter.CountMonthsBetweenData;
    public static XunitData<YemodaPairAnd<int>> CountYearsBetweenData => s_Adapter.CountYearsBetweenData;
    public static XunitData<YedoyPairAnd<int>> CountYearsBetweenOrdinalData => s_Adapter.CountYearsBetweenOrdinalData;
    public static XunitData<YemoPairAnd<int>> CountYearsBetweenMonthData => s_Adapter.CountYearsBetweenMonthData;

    private sealed class DataSetAdapter
    {
        private readonly TDataSet _dataSet;

        public DataSetAdapter(TDataSet dataSet)
        {
            _dataSet = dataSet ?? throw new ArgumentNullException(nameof(dataSet));
        }

        private XunitData<MonthsSinceEpochInfo>? _monthsSinceEpochInfoData;
        public XunitData<MonthsSinceEpochInfo> MonthsSinceEpochInfoData =>
            _monthsSinceEpochInfoData ??= _dataSet.MonthsSinceEpochInfoData.ToXunitData();

        private XunitData<DaysSinceEpochInfo>? _daysSinceEpochInfoData;
        public XunitData<DaysSinceEpochInfo> DaysSinceEpochInfoData =>
            _daysSinceEpochInfoData ??= _dataSet.DaysSinceEpochInfoData.ToXunitData();

        private XunitData<DateInfo>? _dateInfoData;
        public XunitData<DateInfo> DateInfoData =>
            _dateInfoData ??= _dataSet.DateInfoData.ToXunitData();

        private XunitData<MonthInfo>? _monthInfoData;
        public XunitData<MonthInfo> MonthInfoData =>
            _monthInfoData ??= _dataSet.MonthInfoData.ToXunitData();

        private XunitData<YearInfo>? _yearInfoData;
        public XunitData<YearInfo> YearInfoData =>
            _yearInfoData ??= _dataSet.YearInfoData.ToXunitData();

        private XunitData<CenturyInfo>? _centuryInfoData;
        public XunitData<CenturyInfo> CenturyInfoData =>
            _centuryInfoData ??= _dataSet.CenturyInfoData.ToXunitData();

        private XunitData<YemodaAnd<int>>? _daysInYearAfterDateData;
        public XunitData<YemodaAnd<int>> DaysInYearAfterDateData =>
            _daysInYearAfterDateData ??= _dataSet.DaysInYearAfterDateData.ToXunitData();

        private XunitData<YemodaAnd<int>>? _daysInMonthAfterDateData;
        public XunitData<YemodaAnd<int>> DaysInMonthAfterDateData =>
            _daysInMonthAfterDateData ??= _dataSet.DaysInMonthAfterDateData.ToXunitData();

        private XunitData<Yemoda>? _startOfYearPartsData;
        public XunitData<Yemoda> StartOfYearPartsData =>
            _startOfYearPartsData ??= _dataSet.StartOfYearPartsData.ToXunitData();

        private XunitData<Yemoda>? _endOfYearPartsData;
        public XunitData<Yemoda> EndOfYearPartsData =>
            _endOfYearPartsData ??= _dataSet.EndOfYearPartsData.ToXunitData();

        private XunitData<YearMonthsSinceEpoch>? _startOfYearMonthsSinceEpochData;
        public XunitData<YearMonthsSinceEpoch> StartOfYearMonthsSinceEpochData =>
            _startOfYearMonthsSinceEpochData ??= _dataSet.StartOfYearMonthsSinceEpochData.ToXunitData();

        private XunitData<YearMonthsSinceEpoch>? _endOfYearMonthsSinceEpochData;
        public XunitData<YearMonthsSinceEpoch> EndOfYearMonthsSinceEpochData =>
            _endOfYearMonthsSinceEpochData ??= _dataSet.EndOfYearMonthsSinceEpochData.ToXunitData();

        private XunitData<YearDaysSinceEpoch>? _startOfYearDaysSinceEpochData;
        public XunitData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData =>
            _startOfYearDaysSinceEpochData ??= _dataSet.StartOfYearDaysSinceEpochData.ToXunitData();

        private XunitData<YearDaysSinceEpoch>? _endOfYearDaysSinceEpochData;
        public XunitData<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData =>
            _endOfYearDaysSinceEpochData ??= _dataSet.EndOfYearDaysSinceEpochData.ToXunitData();

        //
        // Data for Next() and Previous()
        //

        private XunitData<YemodaPair>? _consecutiveDaysData;
        public XunitData<YemodaPair> ConsecutiveDaysData =>
            _consecutiveDaysData ??= _dataSet.ConsecutiveDaysData.ToXunitData();

        private XunitData<YedoyPair>? _consecutiveDaysOrdinalData;
        public XunitData<YedoyPair> ConsecutiveDaysOrdinalData =>
            _consecutiveDaysOrdinalData ??= _dataSet.ConsecutiveDaysOrdinalData.ToXunitData();

        private XunitData<YemoPair>? _consecutiveMonthsData;
        public XunitData<YemoPair> ConsecutiveMonthsData =>
            _consecutiveMonthsData ??= _dataSet.ConsecutiveMonthsData.ToXunitData();

        //
        // Data for the additions
        //

        private XunitData<YemodaPairAnd<int>>? _addDaysData;
        public XunitData<YemodaPairAnd<int>> AddDaysData =>
            _addDaysData ??= _dataSet.AddDaysData.ToXunitData();

        private XunitData<YemodaPairAnd<int>>? _addMonthsData;
        public XunitData<YemodaPairAnd<int>> AddMonthsData =>
            _addMonthsData ??= _dataSet.AddMonthsData.ToXunitData();

        private XunitData<YemodaPairAnd<int>>? _addYearsData;
        public XunitData<YemodaPairAnd<int>> AddYearsData =>
            _addYearsData ??= _dataSet.AddYearsData.ToXunitData();

        private XunitData<YedoyPairAnd<int>>? _addDaysOrdinalData;
        public XunitData<YedoyPairAnd<int>> AddDaysOrdinalData =>
            _addDaysOrdinalData ??= _dataSet.AddDaysOrdinalData.ToXunitData();

        private XunitData<YedoyPairAnd<int>>? _addYearsOrdinalData;
        public XunitData<YedoyPairAnd<int>> AddYearsOrdinalData =>
            _addYearsOrdinalData ??= _dataSet.AddYearsOrdinalData.ToXunitData();

        private XunitData<YemoPairAnd<int>>? _addMonthsMonthData;
        public XunitData<YemoPairAnd<int>> AddMonthsMonthData =>
            _addMonthsMonthData ??= _dataSet.AddMonthsMonthData.ToXunitData();

        private XunitData<YemoPairAnd<int>>? _addYearsMonthData;
        public XunitData<YemoPairAnd<int>> AddYearsMonthData =>
            _addYearsMonthData ??= _dataSet.AddYearsMonthData.ToXunitData();

        //
        // Data for the subtractions
        //

        private XunitData<YemodaPairAnd<int>>? _countMonthsBetweenData;
        public XunitData<YemodaPairAnd<int>> CountMonthsBetweenData =>
            _countMonthsBetweenData ??= _dataSet.CountMonthsBetweenData.ToXunitData();

        private XunitData<YemodaPairAnd<int>>? _countYearsBetweenData;
        public XunitData<YemodaPairAnd<int>> CountYearsBetweenData =>
            _countYearsBetweenData ??= _dataSet.CountYearsBetweenData.ToXunitData();

        private XunitData<YedoyPairAnd<int>>? _countYearsBetweenOrdinalData;
        public XunitData<YedoyPairAnd<int>> CountYearsBetweenOrdinalData =>
            _countYearsBetweenOrdinalData ??= _dataSet.CountYearsBetweenOrdinalData.ToXunitData();

        private XunitData<YemoPairAnd<int>>? _countYearsBetweenMonthData;
        public XunitData<YemoPairAnd<int>> CountYearsBetweenMonthData =>
            _countYearsBetweenMonthData ??= _dataSet.CountYearsBetweenMonthData.ToXunitData();
    }
}
