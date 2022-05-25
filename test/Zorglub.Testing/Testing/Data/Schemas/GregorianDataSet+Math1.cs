// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

public partial class GregorianDataSet // IMathDataSet
{
    public DataGroup<YemodaPairAnd<int>> AddDaysData { get; } = new()
    {
        // A full month.
        new(new(3, 4, 5), new(3, 4, 30), 25),
        new(new(3, 4, 5), new(3, 4, 29), 24),
        new(new(3, 4, 5), new(3, 4, 28), 23),
        new(new(3, 4, 5), new(3, 4, 27), 22),
        new(new(3, 4, 5), new(3, 4, 26), 21),
        new(new(3, 4, 5), new(3, 4, 25), 20),
        new(new(3, 4, 5), new(3, 4, 24), 19),
        new(new(3, 4, 5), new(3, 4, 23), 18),
        new(new(3, 4, 5), new(3, 4, 22), 17),
        new(new(3, 4, 5), new(3, 4, 21), 16),
        new(new(3, 4, 5), new(3, 4, 20), 15),
        new(new(3, 4, 5), new(3, 4, 19), 14),
        new(new(3, 4, 5), new(3, 4, 18), 13),
        new(new(3, 4, 5), new(3, 4, 17), 12),
        new(new(3, 4, 5), new(3, 4, 16), 11),
        new(new(3, 4, 5), new(3, 4, 15), 10),
        new(new(3, 4, 5), new(3, 4, 14), 9),
        new(new(3, 4, 5), new(3, 4, 13), 8),
        new(new(3, 4, 5), new(3, 4, 12), 7),
        new(new(3, 4, 5), new(3, 4, 11), 6),
        new(new(3, 4, 5), new(3, 4, 10), 5),
        new(new(3, 4, 5), new(3, 4, 9), 4),
        new(new(3, 4, 5), new(3, 4, 8), 3),
        new(new(3, 4, 5), new(3, 4, 7), 2),
        new(new(3, 4, 5), new(3, 4, 6), 1),
        new(new(3, 4, 5), new(3, 4, 5), 0),
        new(new(3, 4, 5), new(3, 4, 4), -1),
        new(new(3, 4, 5), new(3, 4, 3), -2),
        new(new(3, 4, 5), new(3, 4, 2), -3),
        new(new(3, 4, 5), new(3, 4, 1), -4),

        // Change of year.
        new(new(3, 11, 30), new(4, 1, 1), 32),
        new(new(3, 2, 1), new(2, 12, 31), -32),

        // Change of month.
        new(new(3, 4, 5), new(3, 5, 1), 26),
        new(new(3, 4, 5), new(3, 3, 31), -5),
        // February, common year.
        new(new(3, 2, 28), new(3, 3, 1), 1),
        new(new(3, 3, 1), new(3, 2, 28), -1),
        // February, leap year.
        new(new(4, 2, 28), new(4, 2, 29), 1),
        new(new(4, 2, 28), new(4, 3, 1), 2),
        new(new(4, 2, 29), new(4, 3, 1), 1),
        new(new(4, 3, 1), new(4, 2, 29), -1),

        // years < -365, no leap year in the middle.
        new(new(3, 4, 5), new(2, 4, 5), -365),
        new(new(3, 4, 5), new(1, 4, 5), -2 * 365),

        // years > 365, one leap year in the middle.
        new(new(3, 4, 5), new(4, 4, 4), 365),
        new(new(3, 4, 5), new(5, 4, 4), 2 * 365),
    };

    public DataGroup<YemodaPair> ConsecutiveDaysData { get; } = new()
    {
        new(new(CommonYear, 4, 1), new(CommonYear, 4, 2)),
        new(new(CommonYear, 4, 2), new(CommonYear, 4, 3)),
        new(new(CommonYear, 4, 3), new(CommonYear, 4, 4)),
        new(new(CommonYear, 4, 4), new(CommonYear, 4, 5)),
        new(new(CommonYear, 4, 5), new(CommonYear, 4, 6)),
        new(new(CommonYear, 4, 6), new(CommonYear, 4, 7)),
        new(new(CommonYear, 4, 7), new(CommonYear, 4, 8)),
        new(new(CommonYear, 4, 8), new(CommonYear, 4, 9)),
        new(new(CommonYear, 4, 9), new(CommonYear, 4, 10)),
        new(new(CommonYear, 4, 10), new(CommonYear, 4, 11)),
        new(new(CommonYear, 4, 11), new(CommonYear, 4, 12)),
        new(new(CommonYear, 4, 12), new(CommonYear, 4, 13)),
        new(new(CommonYear, 4, 13), new(CommonYear, 4, 14)),
        new(new(CommonYear, 4, 14), new(CommonYear, 4, 15)),
        new(new(CommonYear, 4, 15), new(CommonYear, 4, 16)),
        new(new(CommonYear, 4, 16), new(CommonYear, 4, 17)),
        new(new(CommonYear, 4, 17), new(CommonYear, 4, 18)),
        new(new(CommonYear, 4, 18), new(CommonYear, 4, 19)),
        new(new(CommonYear, 4, 19), new(CommonYear, 4, 20)),
        new(new(CommonYear, 4, 20), new(CommonYear, 4, 21)),
        new(new(CommonYear, 4, 21), new(CommonYear, 4, 22)),
        new(new(CommonYear, 4, 22), new(CommonYear, 4, 23)),
        new(new(CommonYear, 4, 23), new(CommonYear, 4, 24)),
        new(new(CommonYear, 4, 24), new(CommonYear, 4, 25)),
        new(new(CommonYear, 4, 25), new(CommonYear, 4, 26)),
        new(new(CommonYear, 4, 26), new(CommonYear, 4, 27)),
        new(new(CommonYear, 4, 27), new(CommonYear, 4, 28)),
        new(new(CommonYear, 4, 28), new(CommonYear, 4, 29)),
        new(new(CommonYear, 4, 29), new(CommonYear, 4, 30)),
        new(new(CommonYear, 4, 30), new(CommonYear, 5, 1)),
        new(new(CommonYear, 5, 1), new(CommonYear, 5, 2)),

        // End of february.
        new(new(CommonYear, 2, 28), new(CommonYear, 3, 1)),
        new(new(LeapYear, 2, 28), new(LeapYear, 2, 29)),
        new(new(LeapYear, 2, 29), new(LeapYear, 3, 1)),

        // End of month.
        new(new(CommonYear, 1, 31), new(CommonYear, 2, 1)),
        new(new(CommonYear, 3, 31), new(CommonYear, 4, 1)),
        new(new(CommonYear, 5, 31), new(CommonYear, 6, 1)),
        new(new(CommonYear, 6, 30), new(CommonYear, 7, 1)),
        new(new(CommonYear, 7, 31), new(CommonYear, 8, 1)),
        new(new(CommonYear, 8, 31), new(CommonYear, 9, 1)),
        new(new(CommonYear, 9, 30), new(CommonYear, 10, 1)),
        new(new(CommonYear, 10, 31), new(CommonYear, 11, 1)),
        new(new(CommonYear, 11, 30), new(CommonYear, 12, 1)),
        new(new(3, 12, 31), new(4, 1, 1)),
    };

    public DataGroup<YedoyPairAnd<int>> AddDaysOrdinalData { get; } = new()
    {
        // A full month (January).
        new(new(3, 5), new(3, 30), 25),
        new(new(3, 5), new(3, 29), 24),
        new(new(3, 5), new(3, 28), 23),
        new(new(3, 5), new(3, 27), 22),
        new(new(3, 5), new(3, 26), 21),
        new(new(3, 5), new(3, 25), 20),
        new(new(3, 5), new(3, 24), 19),
        new(new(3, 5), new(3, 23), 18),
        new(new(3, 5), new(3, 22), 17),
        new(new(3, 5), new(3, 21), 16),
        new(new(3, 5), new(3, 20), 15),
        new(new(3, 5), new(3, 19), 14),
        new(new(3, 5), new(3, 18), 13),
        new(new(3, 5), new(3, 17), 12),
        new(new(3, 5), new(3, 16), 11),
        new(new(3, 5), new(3, 15), 10),
        new(new(3, 5), new(3, 14), 9),
        new(new(3, 5), new(3, 13), 8),
        new(new(3, 5), new(3, 12), 7),
        new(new(3, 5), new(3, 11), 6),
        new(new(3, 5), new(3, 10), 5),
        new(new(3, 5), new(3, 9), 4),
        new(new(3, 5), new(3, 8), 3),
        new(new(3, 5), new(3, 7), 2),
        new(new(3, 5), new(3, 6), 1),
        new(new(3, 5), new(3, 5), 0),
        new(new(3, 5), new(3, 4), -1),
        new(new(3, 5), new(3, 3), -2),
        new(new(3, 5), new(3, 2), -3),
        new(new(3, 5), new(3, 1), -4),
    };

    public DataGroup<YedoyPair> ConsecutiveDaysOrdinalData { get; } = new()
    {
        // January
        new(new(CommonYear, 1), new(CommonYear, 2)),
        new(new(CommonYear, 2), new(CommonYear, 3)),
        new(new(CommonYear, 3), new(CommonYear, 4)),
        new(new(CommonYear, 4), new(CommonYear, 5)),
        new(new(CommonYear, 5), new(CommonYear, 6)),
        new(new(CommonYear, 6), new(CommonYear, 7)),
        new(new(CommonYear, 7), new(CommonYear, 8)),
        new(new(CommonYear, 8), new(CommonYear, 9)),
        new(new(CommonYear, 9), new(CommonYear, 10)),
        new(new(CommonYear, 10), new(CommonYear, 11)),
        new(new(CommonYear, 11), new(CommonYear, 12)),
        new(new(CommonYear, 12), new(CommonYear, 13)),
        new(new(CommonYear, 13), new(CommonYear, 14)),
        new(new(CommonYear, 14), new(CommonYear, 15)),
        new(new(CommonYear, 15), new(CommonYear, 16)),
        new(new(CommonYear, 16), new(CommonYear, 17)),
        new(new(CommonYear, 17), new(CommonYear, 18)),
        new(new(CommonYear, 18), new(CommonYear, 19)),
        new(new(CommonYear, 19), new(CommonYear, 20)),
        new(new(CommonYear, 20), new(CommonYear, 21)),
        new(new(CommonYear, 21), new(CommonYear, 22)),
        new(new(CommonYear, 22), new(CommonYear, 23)),
        new(new(CommonYear, 23), new(CommonYear, 24)),
        new(new(CommonYear, 24), new(CommonYear, 25)),
        new(new(CommonYear, 25), new(CommonYear, 26)),
        new(new(CommonYear, 26), new(CommonYear, 27)),
        new(new(CommonYear, 27), new(CommonYear, 28)),
        new(new(CommonYear, 28), new(CommonYear, 29)),
        new(new(CommonYear, 29), new(CommonYear, 30)),
        new(new(CommonYear, 30), new(CommonYear, 31)),
        new(new(CommonYear, 31), new(CommonYear, 32)),

        // February
        new(new(CommonYear, 32), new(CommonYear, 33)),
        // End of february.
        new(new(CommonYear, 59), new(CommonYear, 60)), // February 28
        new(new(CommonYear, 60), new(CommonYear, 61)), // March 1st
        new(new(LeapYear, 60), new(LeapYear, 61)),   // February 29
        new(new(LeapYear, 61), new(LeapYear, 62)),   // March 1st

        // End of year.
        new(new(CommonYear, 365), new(CommonYear + 1, 1)),
        new(new(LeapYear, 365), new(LeapYear, 366)),
        new(new(LeapYear, 366), new(LeapYear + 1, 1)),
    };
}

// NB: we do not include data for which the result is ambiguous, see
// GregorianMathDataSet...Adjustment for that.
public partial class GregorianDataSet // IAdvancedMathDataSet
{
    public AddAdjustment AddAdjustment { get; } = AddAdjustment.EndOfMonth;

    public DataGroup<YemodaPairAnd<int>> AddYearsData { get; } = new()
    {
        new(new(3, 4, 5), new(9, 4, 5), 6),
        new(new(3, 4, 5), new(8, 4, 5), 5),
        new(new(3, 4, 5), new(7, 4, 5), 4),
        new(new(3, 4, 5), new(6, 4, 5), 3),
        new(new(3, 4, 5), new(5, 4, 5), 2),
        new(new(3, 4, 5), new(4, 4, 5), 1),
        new(new(3, 4, 5), new(3, 4, 5), 0),
        new(new(3, 4, 5), new(2, 4, 5), -1),
        new(new(3, 4, 5), new(1, 4, 5), -2),

        // End of february, common year -> common year.
        new(new(3, 2, 28), new(5, 2, 28), 2),
        new(new(3, 2, 28), new(1, 2, 28), -2),

        // End of february, common year -> leap year.
        new(new(3, 2, 28), new(4, 2, 28), 1),
        new(new(5, 2, 28), new(4, 2, 28), -1),

        // End of february, leap year -> leap year.
        new(new(4, 2, 29), new(8, 2, 29), 4),
        new(new(8, 2, 29), new(4, 2, 29), -4),

        // End of february, leap year -> common year.
        // See samples with cutoff.
    };

    public DataGroup<YemodaPairAnd<int>> AddMonthsData { get; } = new()
    {
        // Simple cases.
        new(new(3, 4, 5), new(5, 1, 5), 21),
        new(new(3, 4, 5), new(4, 12, 5), 20),
        new(new(3, 4, 5), new(4, 11, 5), 19),
        new(new(3, 4, 5), new(4, 10, 5), 18),
        new(new(3, 4, 5), new(4, 9, 5), 17),
        new(new(3, 4, 5), new(4, 8, 5), 16),
        new(new(3, 4, 5), new(4, 7, 5), 15),
        new(new(3, 4, 5), new(4, 6, 5), 14),
        new(new(3, 4, 5), new(4, 5, 5), 13),
        new(new(3, 4, 5), new(4, 4, 5), 12),
        new(new(3, 4, 5), new(4, 3, 5), 11),
        new(new(3, 4, 5), new(4, 2, 5), 10),
        new(new(3, 4, 5), new(4, 1, 5), 9),
        new(new(3, 4, 5), new(3, 12, 5), 8),
        new(new(3, 4, 5), new(3, 11, 5), 7),
        new(new(3, 4, 5), new(3, 10, 5), 6),
        new(new(3, 4, 5), new(3, 9, 5), 5),
        new(new(3, 4, 5), new(3, 8, 5), 4),
        new(new(3, 4, 5), new(3, 7, 5), 3),
        new(new(3, 4, 5), new(3, 6, 5), 2),
        new(new(3, 4, 5), new(3, 5, 5), 1),
        new(new(3, 4, 5), new(3, 4, 5), 0),
        new(new(3, 4, 5), new(3, 3, 5), -1),
        new(new(3, 4, 5), new(3, 2, 5), -2),
        new(new(3, 4, 5), new(3, 1, 5), -3),
        new(new(3, 4, 5), new(2, 12, 5), -4),
        new(new(3, 4, 5), new(2, 11, 5), -5),
        new(new(3, 4, 5), new(2, 10, 5), -6),
        new(new(3, 4, 5), new(2, 9, 5), -7),
        new(new(3, 4, 5), new(2, 8, 5), -8),
        new(new(3, 4, 5), new(2, 7, 5), -9),
        new(new(3, 4, 5), new(2, 6, 5), -10),
        new(new(3, 4, 5), new(2, 5, 5), -11),
        new(new(3, 4, 5), new(2, 4, 5), -12),
        new(new(3, 4, 5), new(2, 3, 5), -13),
        new(new(3, 4, 5), new(2, 2, 5), -14),
        new(new(3, 4, 5), new(2, 1, 5), -15),
        new(new(3, 4, 5), new(1, 12, 5), -16),

        // 30 days long month -> 31 days long month.
        new(new(1, 4, 30), new(1, 7, 30), 3),
        new(new(1, 4, 30), new(1, 1, 30), -3),

        // Target February (common year).
        new(new(1, 1, 27), new(1, 2, 27), 1),
        new(new(1, 1, 28), new(1, 2, 28), 1),
        // Backward.
        new(new(1, 3, 27), new(1, 2, 27), -1),
        new(new(1, 3, 28), new(1, 2, 28), -1),

        // Target February (leap year).
        new(new(4, 1, 27), new(4, 2, 27), 1),
        new(new(4, 1, 28), new(4, 2, 28), 1),
        new(new(4, 1, 29), new(4, 2, 29), 1),
        // Backward.
        new(new(4, 3, 27), new(4, 2, 27), -1),
        new(new(4, 3, 28), new(4, 2, 28), -1),
        new(new(4, 3, 29), new(4, 2, 29), -1),
    };

    public DataGroup<DateDiff> DateDiffData { get; } = DataGroup.Create(DateDiffs);

    // TODO(data): see XXX below. Something is wrong with the data or with Subtract().
    private static IEnumerable<DateDiff> DateDiffs
    {
        get
        {
            #region After >= 1 year

            // At least 2 years.
            yield return new(new(3, 4, 5), new(9, 4, 6), 6, 0, 1);
            yield return new(new(3, 4, 5), new(9, 4, 5), 6, 0, 0);
            yield return new(new(3, 4, 5), new(9, 4, 4), 5, 11, 30);

            yield return new(new(3, 4, 5), new(8, 4, 6), 5, 0, 1);
            yield return new(new(3, 4, 5), new(8, 4, 5), 5, 0, 0);
            yield return new(new(3, 4, 5), new(8, 4, 4), 4, 11, 30);

            yield return new(new(3, 4, 5), new(7, 4, 6), 4, 0, 1);
            yield return new(new(3, 4, 5), new(7, 4, 5), 4, 0, 0);
            yield return new(new(3, 4, 5), new(7, 4, 4), 3, 11, 30);

            yield return new(new(3, 4, 5), new(6, 4, 6), 3, 0, 1);
            yield return new(new(3, 4, 5), new(6, 4, 5), 3, 0, 0);
            yield return new(new(3, 4, 5), new(6, 4, 4), 2, 11, 30);

            yield return new(new(3, 4, 5), new(5, 4, 6), 2, 0, 1);
            yield return new(new(3, 4, 5), new(5, 4, 5), 2, 0, 0);
            yield return new(new(3, 4, 5), new(5, 4, 4), 1, 11, 30);

            // 1 year and 2 months.
            yield return new(new(3, 4, 5), new(4, 6, 29), 1, 2, 24);
            yield return new(new(3, 4, 5), new(4, 6, 6), 1, 2, 1);
            yield return new(new(3, 4, 5), new(4, 6, 5), 1, 2, 0);
            // 1 year and 1 month.
            yield return new(new(3, 4, 5), new(4, 5, 29), 1, 1, 24);
            yield return new(new(3, 4, 5), new(4, 5, 6), 1, 1, 1);
            yield return new(new(3, 4, 5), new(4, 5, 5), 1, 1, 0);

            // 1 year.
            yield return new(new(3, 4, 5), new(4, 4, 30), 1, 0, 25);
            yield return new(new(3, 4, 5), new(4, 4, 6), 1, 0, 1);
            yield return new(new(3, 4, 5), new(4, 4, 5), 1, 0, 0);

            #endregion
            #region After < 1 year

            yield return new(new(3, 4, 5), new(4, 4, 4), 0, 11, 30);
            yield return new(new(3, 4, 5), new(4, 4, 1), 0, 11, 27);
            yield return new(new(3, 4, 5), new(4, 3, 31), 0, 11, 26);
            yield return new(new(3, 4, 5), new(4, 3, 5), 0, 11, 0);
            yield return new(new(3, 4, 5), new(4, 2, 5), 0, 10, 0);
            yield return new(new(3, 4, 5), new(4, 1, 5), 0, 9, 0);

            yield return new(new(3, 4, 5), new(3, 12, 5), 0, 8, 0);
            yield return new(new(3, 4, 5), new(3, 11, 5), 0, 7, 0);
            yield return new(new(3, 4, 5), new(3, 10, 5), 0, 6, 0);
            yield return new(new(3, 4, 5), new(3, 9, 5), 0, 5, 0);
            yield return new(new(3, 4, 5), new(3, 8, 5), 0, 4, 0);
            yield return new(new(3, 4, 5), new(3, 7, 5), 0, 3, 0);

            // 2 months.
            yield return new(new(3, 4, 5), new(3, 7, 4), 0, 2, 29);
            yield return new(new(3, 4, 5), new(3, 7, 1), 0, 2, 26);
            yield return new(new(3, 4, 5), new(3, 6, 30), 0, 2, 25);
            yield return new(new(3, 4, 5), new(3, 6, 5), 0, 2, 0);
            // 1 month.
            yield return new(new(3, 4, 5), new(3, 6, 4), 0, 1, 30);
            yield return new(new(3, 4, 5), new(3, 6, 1), 0, 1, 27);
            yield return new(new(3, 4, 5), new(3, 5, 31), 0, 1, 26);
            yield return new(new(3, 4, 5), new(3, 5, 5), 0, 1, 0);
            // < 1 month.
            yield return new(new(3, 4, 5), new(3, 5, 4), 0, 0, 29);
            yield return new(new(3, 4, 5), new(3, 5, 1), 0, 0, 26);
            yield return new(new(3, 4, 5), new(3, 4, 30), 0, 0, 25);
            yield return new(new(3, 4, 5), new(3, 4, 6), 0, 0, 1);

            #endregion

            // Identical dates.
            yield return new(new(3, 4, 5), new(3, 4, 5), 0, 0, 0);

            #region Before < 1 year

            // < 1 month.
            yield return new(new(3, 4, 5), new(3, 4, 4), 0, 0, -1);
            yield return new(new(3, 4, 5), new(3, 4, 1), 0, 0, -4);
            yield return new(new(3, 4, 5), new(3, 3, 31), 0, 0, -5);
            yield return new(new(3, 4, 5), new(3, 3, 6), 0, 0, -30);
            // 1 month.
            yield return new(new(3, 4, 5), new(3, 3, 5), 0, -1, 0);
            yield return new(new(3, 4, 5), new(3, 3, 1), 0, -1, -4);
            yield return new(new(3, 4, 5), new(3, 2, 28), 0, -1, -5);
            yield return new(new(3, 4, 5), new(3, 2, 6), 0, -1, -27);
            // 2 months.
            yield return new(new(3, 4, 5), new(3, 2, 5), 0, -2, 0);
            yield return new(new(3, 4, 5), new(3, 2, 1), 0, -2, -4);
            yield return new(new(3, 4, 5), new(3, 1, 31), 0, -2, -5);
            yield return new(new(3, 4, 5), new(3, 1, 6), 0, -2, -30);
            // 3 months.
            yield return new(new(3, 4, 5), new(3, 1, 5), 0, -3, 0);
            yield return new(new(3, 4, 5), new(3, 1, 1), 0, -3, -4);

            yield return new(new(3, 4, 5), new(2, 4, 6), 0, -11, -29);

            #endregion
            #region Before >= 1 year

            yield return new(new(3, 4, 5), new(2, 4, 5), -1, 0, 0);
            yield return new(new(3, 4, 5), new(2, 4, 1), -1, 0, -4);
            yield return new(new(3, 4, 5), new(2, 3, 31), -1, 0, -5);
            yield return new(new(3, 4, 5), new(2, 3, 6), -1, 0, -30);
            // 1 year and 1 month.
            yield return new(new(3, 4, 5), new(2, 3, 5), -1, -1, 0);
            yield return new(new(3, 4, 5), new(2, 3, 4), -1, -1, -1);
            yield return new(new(3, 4, 5), new(2, 3, 1), -1, -1, -4);
            yield return new(new(3, 4, 5), new(2, 2, 28), -1, -1, -5);
            yield return new(new(3, 4, 5), new(2, 2, 6), -1, -1, -27);
            // 1 year and 2 months.
            yield return new(new(3, 4, 5), new(2, 2, 5), -1, -2, 0);
            yield return new(new(3, 4, 5), new(2, 2, 4), -1, -2, -1);
            yield return new(new(3, 4, 5), new(2, 2, 1), -1, -2, -4);

            yield return new(new(3, 4, 5), new(1, 4, 6), -1, -11, -29);

            // At least 2 years.
            yield return new(new(3, 4, 5), new(1, 4, 5), -2, 0, 0);
            yield return new(new(3, 4, 5), new(1, 4, 4), -2, 0, -1);
            yield return new(new(3, 4, 5), new(1, 1, 1), -2, -3, -4);

            #endregion
            #region END/m/y

            yield return new(new(3, 4, 30), new(4, 5, 1), 1, 0, 1);
            yield return new(new(3, 4, 30), new(4, 4, 30), 1, 0, 0);

            yield return new(new(3, 4, 30), new(4, 4, 29), 0, 11, 30);
            yield return new(new(3, 4, 30), new(4, 4, 2), 0, 11, 3);
            yield return new(new(3, 4, 30), new(4, 4, 1), 0, 11, 2);
            yield return new(new(3, 4, 30), new(4, 3, 31), 0, 11, 1);
            yield return new(new(3, 4, 30), new(4, 3, 30), 0, 11, 0);

            yield return new(new(3, 4, 30), new(4, 3, 29), 0, 10, 29);
            yield return new(new(3, 4, 30), new(4, 3, 1), 0, 10, 1);
            // XXX yield return new(new(3, 4, 30), new(4,  2, 29),  0,  10,   0);

            yield return new(new(3, 4, 30), new(4, 2, 28), 0, 9, 29);
            yield return new(new(3, 4, 30), new(4, 2, 1), 0, 9, 2);
            yield return new(new(3, 4, 30), new(4, 1, 31), 0, 9, 1);
            yield return new(new(3, 4, 30), new(4, 1, 30), 0, 9, 0);

            yield return new(new(3, 4, 30), new(2, 5, 1), 0, -11, -29);
            yield return new(new(3, 4, 30), new(2, 4, 30), -1, 0, 0);
            yield return new(new(3, 4, 30), new(2, 4, 29), -1, 0, -1);
            yield return new(new(3, 4, 30), new(2, 4, 2), -1, 0, -28);
            yield return new(new(3, 4, 30), new(2, 4, 1), -1, 0, -29);
            // XXX yield return new(new(3, 4, 30), new(2,  3, 31), -1,  -1,   1);
            // XXX yield return new(new(2, 3, 31), new(3,  4, 30),  1,   1,   0);

            yield return new(new(3, 4, 30), new(2, 3, 30), -1, -1, 0);

            #endregion
            #region Intercalary day

            // end is in a leap year and after start.
            // 29/02/0008 -> 29/02/0012 -> 01/03/0012.
            yield return new(new(8, 2, 29), new(12, 3, 1), 4, 0, 1);
            // 29/02/0008 -> 29/02/0012.
            yield return new(new(8, 2, 29), new(12, 2, 29), 4, 0, 0);
            // 29/02/0008 -> 28/02/0011 -> 28/02/0012.
            // NB: newStart for CountMonthsBetween() is 29/01/0012, not 28/02/0011.
            yield return new(new(8, 2, 29), new(12, 2, 28), 3, 11, 30);
            // 29/02/0008 -> 28/02/0011 -> 27/02/0012.
            // NB: newStart for CountMonthsBetween() is 29/01/0012, not 28/02/0011.
            yield return new(new(8, 2, 29), new(12, 2, 27), 3, 11, 29);

            // end is in a common year and after start.
            // 29/02/0008 -> 28/02/0011 -> 01/03/0011.
            yield return new(new(8, 2, 29), new(11, 3, 1), 3, 0, 1);
            // 29/02/0008 -> 28/02/0011.
            // XXX yield return new(new(8, 2, 29), new(11,  2, 28),  3,   0,   0);
            // 29/02/0008 -> 28/02/0010 -> 27/02/0011.
            // NB: newStart for CountMonthsBetween() is 29/01/0011, not 28/02/0010.
            yield return new(new(8, 2, 29), new(11, 2, 27), 2, 11, 29);

            // 29/02/0008 -> 28/02/0009 -> 01/03/0009.
            yield return new(new(8, 2, 29), new(9, 3, 1), 1, 0, 1);
            // 29/02/0008 -> 28/02/0009.
            // XXX yield return new(new(8, 2, 29), new( 9,  2, 28),  1,   0,   0);
            // 29/02/0008 -> 29/02/0008 -> 27/02/0009.
            yield return new(new(8, 2, 29), new(9, 2, 27), 0, 11, 29);

            // end same year.
            yield return new(new(8, 2, 29), new(8, 3, 1), 0, 0, 1);
            yield return new(new(8, 2, 29), new(8, 2, 28), 0, 0, -1);
            yield return new(new(8, 2, 29), new(8, 2, 27), 0, 0, -2);

            // end is in a common year and before start.
            yield return new(new(8, 2, 29), new(7, 3, 1), 0, -11, -28);
            yield return new(new(8, 2, 29), new(7, 2, 28), -1, 0, 0);
            yield return new(new(8, 2, 29), new(7, 2, 27), -1, 0, -1);

            // end is in a leap year and before start.
            yield return new(new(8, 2, 29), new(4, 3, 1), -3, -11, -28);
            yield return new(new(8, 2, 29), new(4, 2, 29), -4, 0, 0);
            yield return new(new(8, 2, 29), new(4, 2, 28), -4, 0, -1);

            #endregion
        }
    }
}
