// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

public partial class GregorianDataSet // Math data
{
    public override DataGroup<YemodaPairAnd<int>> AddDaysData { get; } = new DataGroup<YemodaPairAnd<int>>()
    {
        // Branch AddDaysViaDayOfMonth()
        //
        // |days| <= MinDaysInMonth = 28
        // We only need to provide data when there is a change of month, the
        // other case is already covered by AddDaysSamples.

        // Change of year.
        new(new(2, 12, 4), new(3, 1, 1), 28),
        // Change of month.
        new(new(3, 4, 5), new(3, 5, 3), 28),
        // February, common year.
        new(new(CommonYear, 2, 28), new(CommonYear, 3, 28), 28),
        // February, leap year.
        new(new(LeapYear, 2, 28), new(LeapYear, 3, 27), 28),
        new(new(LeapYear, 2, 29), new(LeapYear, 3, 28), 28),

        //
        // Branch AddDaysViaDayOfYear()
        //
        // MinDaysInMonth = 28 < |days| <= MinDaysInYear = 365

        // We use 32 to ensure that the operation skips one month.
        // Change of year.
        new(new(3, 11, 30), new(4, 1, 1), 32),
        // Change of month.
        new(new(3, 4, 1), new(3, 5, 3), 32),

        // February, common year.
        new(new(CommonYear, 2, 28), new(CommonYear - 1, 2, 28), -365),
        new(new(CommonYear, 2, 28), new(CommonYear + 1, 2, 28), 365),
        // February, leap year.
        new(new(LeapYear, 2, 28), new(LeapYear - 1, 2, 28), -365),
        new(new(LeapYear, 2, 28), new(LeapYear + 1, 2, 27), 365),
        new(new(LeapYear, 2, 29), new(LeapYear - 1, 3, 1), -365),
        new(new(LeapYear, 2, 29), new(LeapYear + 1, 2, 28), 365),

        //
        // Slowtrack
        //
        // |days| > MinDaysInYear = 365
        // We start with a day after february.
        // In fact we have two cases for which |days| = 365.

        new(new(3, 4, 5), new(-2, 4, 6), -5 * 365),
        new(new(3, 4, 5), new(-1, 4, 6), -4 * 365),
        new(new(3, 4, 5), new(0, 4, 5), -3 * 365),  // Result is leap
        new(new(3, 4, 5), new(1, 4, 5), -2 * 365),
        new(new(3, 4, 5), new(2, 4, 5), -365),
        //new(new(3, 4, 5), new(3, 4, 5), 0),
        new(new(3, 4, 5), new(4, 4, 4), 365),       // Result is leap
        new(new(3, 4, 5), new(5, 4, 4), 2 * 365),
        new(new(3, 4, 5), new(6, 4, 4), 3 * 365),
        new(new(3, 4, 5), new(7, 4, 4), 4 * 365),
        new(new(3, 4, 5), new(8, 4, 3), 5 * 365),   // Result is leap
        new(new(3, 4, 5), new(9, 4, 3), 6 * 365),
        new(new(3, 4, 5), new(10, 4, 3), 7 * 365),
    }.ConcatT(AddDaysSamples);

    public override DataGroup<YemodaPair> ConsecutiveDaysData { get; } = new DataGroup<YemodaPair>()
    {
        // End of month.
        new(new(CommonYear, 1, 31), new(CommonYear, 2, 1)),
        new(new(CommonYear, 2, 28), new(CommonYear, 3, 1)),
        new(new(LeapYear, 2, 28), new(LeapYear, 2, 29)),
        new(new(LeapYear, 2, 29), new(LeapYear, 3, 1)),
        new(new(CommonYear, 3, 31), new(CommonYear, 4, 1)),
        new(new(CommonYear, 5, 31), new(CommonYear, 6, 1)),
        new(new(CommonYear, 6, 30), new(CommonYear, 7, 1)),
        new(new(CommonYear, 7, 31), new(CommonYear, 8, 1)),
        new(new(CommonYear, 8, 31), new(CommonYear, 9, 1)),
        new(new(CommonYear, 9, 30), new(CommonYear, 10, 1)),
        new(new(CommonYear, 10, 31), new(CommonYear, 11, 1)),
        new(new(CommonYear, 11, 30), new(CommonYear, 12, 1)),
        new(new(CommonYear, 12, 31), new(CommonYear + 1, 1, 1)),
    }.ConcatT(ConsecutiveDaysSamples);

    public override DataGroup<YedoyPairAnd<int>> AddDaysOrdinalData { get; } = new DataGroup<YedoyPairAnd<int>>()
    {
        //
        // Branch AddDaysViaDayOfYear()
        //
        // |days| <= MinDaysInYear = 365

        // February, common year.
        new(new(CommonYear, 59), new(CommonYear - 1, 59), -365),
        new(new(CommonYear, 59), new(CommonYear + 1, 59), 365),
        // February, leap year.
        new(new(LeapYear, 59), new(LeapYear - 1, 59), -365),
        new(new(LeapYear, 59), new(LeapYear + 1, 58), 365),
        new(new(LeapYear, 60), new(LeapYear - 1, 60), -365),
        new(new(LeapYear, 60), new(LeapYear + 1, 59), 365),

        //
        // Slowtrack
        //
        // |days| > MinDaysInYear = 365
        // We start with a day after february.
        // In fact we have two cases for which |days| = 365.

        new(new(3, 64), new(-2, 65), -5 * 365),
        new(new(3, 64), new(-1, 65), -4 * 365),
        new(new(3, 64), new(0, 65), -3 * 365),  // Result is leap -> next step loose one day
        new(new(3, 64), new(1, 64), -2 * 365),
        new(new(3, 64), new(2, 64), -365),
        //new(new(3, 64), new(3, 64), 0),
        new(new(3, 64), new(4, 64), 365),       // Result is leap -> next step loose one day
        new(new(3, 64), new(5, 63), 2 * 365),
        new(new(3, 64), new(6, 63), 3 * 365),
        new(new(3, 64), new(7, 63), 4 * 365),
        new(new(3, 64), new(8, 63), 5 * 365),   // Result is leap -> next step loose one day
        new(new(3, 64), new(9, 62), 6 * 365),
        new(new(3, 64), new(10, 62), 7 * 365),
    }.ConcatT(AddDaysOrdinalSamples);

    public override DataGroup<YedoyPair> ConsecutiveDaysOrdinalData { get; } = new DataGroup<YedoyPair>()
    {
        // End of year.
        new(new(CommonYear, 365), new(CommonYear + 1, 1)),
        new(new(LeapYear, 365), new(LeapYear, 366)),
        new(new(LeapYear, 366), new(LeapYear + 1, 1)),
    }.ConcatT(ConsecutiveDaysOrdinalSamples);

    // NB: we do not include data for which the result is ambiguous, see
    // GregorianMathDataSet...Adjustment for that.

    public override DataGroup<YemodaPairAnd<int>> AddYearsData { get; } = new DataGroup<YemodaPairAnd<int>>()
    {
        // End of february, common year -> common year.
        new(new(3, 2, 28), new(5, 2, 28), 2),
        // End of february, common year -> leap year.
        new(new(3, 2, 28), new(4, 2, 28), 1),
        // End of february, leap year -> leap year.
        new(new(4, 2, 29), new(8, 2, 29), 4),
        // End of february, leap year -> common year.
        // Ambiguous, see GregorianMathDataSet...Adjustment.
    }.ConcatT(AddYearsSamples);

    public override DataGroup<YemodaPairAnd<int>> AddMonthsData { get; } = new DataGroup<YemodaPairAnd<int>>()
    {
        new(new(3, 6, 5), new(1, 12, 5), -18),
        new(new(3, 6, 5), new(2, 1, 5), -17),
        new(new(3, 6, 5), new(2, 12, 5), -6),
        // For months = -5 to 6, data is already provided by AddMonthsSamples
        new(new(3, 6, 5), new(4, 1, 5), 7),
        new(new(3, 6, 5), new(4, 12, 5), 18),
        new(new(3, 6, 5), new(5, 1, 5), 19),

        // 30 days long month -> 31 days long month.
        new(new(1, 4, 30), new(1, 7, 30), 3),
        new(new(1, 4, 30), new(1, 1, 30), -3),

        // Target February (common year).
        new(new(1, 1, 27), new(1, 2, 27), 1),
        new(new(1, 1, 28), new(1, 2, 28), 1),
        // Target February (leap year).
        new(new(4, 1, 27), new(4, 2, 27), 1),
        new(new(4, 1, 28), new(4, 2, 28), 1),
        new(new(4, 1, 29), new(4, 2, 29), 1),
    }.ConcatT(AddMonthsSamples);
}
