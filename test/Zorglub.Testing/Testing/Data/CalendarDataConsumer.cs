// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

/// <summary>
/// Provides static access to calendar data.
/// </summary>
public abstract class CalendarDataConsumer<TDataSet> : CalendricalDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDataConsumer() { }

    private static DataSetAdapter Adapter { get; } = new(DataSet);

    protected static DayNumber Epoch { get; } = DataSet.Epoch;

    public static XunitData<DayNumberInfo> DayNumberInfoData => Adapter.DayNumberInfoData;

    public static XunitData<YearDayNumber> StartOfYearDayNumberData => Adapter.StartOfYearDayNumberData;
    public static XunitData<YearDayNumber> EndOfYearDayNumberData => Adapter.EndOfYearDayNumberData;

    private sealed class DataSetAdapter
    {
        private readonly TDataSet _dataSet;

        public DataSetAdapter(TDataSet dataSet)
        {
            _dataSet = dataSet ?? throw new ArgumentNullException(nameof(dataSet));
        }

        private XunitData<DayNumberInfo>? _dayNumberInfoData;
        public XunitData<DayNumberInfo> DayNumberInfoData =>
            _dayNumberInfoData ??= _dataSet.DayNumberInfoData.ToXunitData();

        private XunitData<YearDayNumber>? _startOfYearDayNumberData;
        public XunitData<YearDayNumber> StartOfYearDayNumberData =>
            _startOfYearDayNumberData ??= _dataSet.StartOfYearDayNumberData.ToXunitData();

        private XunitData<YearDayNumber>? _endOfYearDayNumberData;
        public XunitData<YearDayNumber> EndOfYearDayNumberData =>
            _endOfYearDayNumberData ??= _dataSet.EndOfYearDayNumberData.ToXunitData();
    }
}
