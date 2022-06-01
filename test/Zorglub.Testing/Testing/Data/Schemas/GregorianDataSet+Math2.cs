// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

public static class GregorianMathDataSetUnambiguous
{
    /// <summary>Start date, end date, exact diff between.</summary>
    public static DataGroup<DateDiff> DateDiffData { get; } = DataGroup.Create(DateDiffs);

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

/// <summary>
/// Provides test data for <see cref="GregorianSchema"/> for math operations when the result is
/// ambiguous.
/// <para>This class provides data using the default <see cref="AdditionRules"/>.</para>
/// <para>See also <see cref="GregorianDataSet"/> for test data for math operations when the result
/// is unambiguous.</para>
/// </summary>
public class GregorianMathDataSetCutOff : IAdvancedMathDataSet, ISingleton<GregorianMathDataSetCutOff>
{
    private GregorianMathDataSetCutOff() { }

    public static GregorianMathDataSetCutOff Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly GregorianMathDataSetCutOff Instance = new();
        static Singleton() { }
    }

    // Default rules.
    public AdditionRules AdditionRules { get; }

    /// <inheritdoc/>
    /// <remarks>Intercalary day, expected result in a common year, years to be added.</remarks>
    public DataGroup<YemodaPairAnd<int>> AddYearsData { get; } = new()
    {
        // End of february, leap year -> common year.

        new(new(4, 2, 29), new(11, 2, 28), 7),
        new(new(4, 2, 29), new(10, 2, 28), 6),
        new(new(4, 2, 29), new(9, 2, 28), 5),
        new(new(4, 2, 29), new(7, 2, 28), 3),
        new(new(4, 2, 29), new(6, 2, 28), 2),
        new(new(4, 2, 29), new(5, 2, 28), 1),

        new(new(8, 2, 29), new(7, 2, 28), -1),
        new(new(8, 2, 29), new(6, 2, 28), -2),
        new(new(8, 2, 29), new(5, 2, 28), -3),
        new(new(8, 2, 29), new(3, 2, 28), -5),
        new(new(8, 2, 29), new(2, 2, 28), -6),
        new(new(8, 2, 29), new(1, 2, 28), -7),
    };

    public DataGroup<YemodaPairAnd<int>> AddMonthsData { get; } = new()
    {
        // 31 days long month -> 30 days long month.
        new(new(1, 3, 31), new(1, 6, 30), 3),
        new(new(1, 5, 31), new(1, 4, 30), -1),

        // Target February (common year).
        new(new(1, 1, 29), new(1, 2, 28), 1),
        new(new(1, 1, 30), new(1, 2, 28), 1),
        new(new(1, 1, 31), new(1, 2, 28), 1),
        // Backward.
        new(new(1, 3, 29), new(1, 2, 28), -1),
        new(new(1, 3, 30), new(1, 2, 28), -1),
        new(new(1, 3, 31), new(1, 2, 28), -1),

        // Target February (leap year).
        new(new(4, 1, 30), new(4, 2, 29), 1),
        new(new(4, 1, 31), new(4, 2, 29), 1),
        // Backward.
        new(new(4, 3, 30), new(4, 2, 29), -1),
        new(new(4, 3, 31), new(4, 2, 29), -1),

        // Change of year.
        // Target shorter month.
        new(new(3, 1, 31), new(4, 4, 30), 15),
        new(new(4, 1, 31), new(3, 11, 30), -2),
        // Target end of February.
        new(new(3, 1, 31), new(4, 2, 29), 13),
        new(new(3, 4, 30), new(4, 2, 29), 10),
        new(new(4, 1, 31), new(3, 2, 28), -11),
        new(new(4, 4, 30), new(3, 2, 28), -14),
    };

    public DataGroup<YedoyPairAnd<int>> AddYearsOrdinalData { get; } = new()
    {
        // End of year, leap year -> common year.
        new(new(4, 366), new(3, 365), -1),
        new(new(4, 366), new(5, 365), 1),
    };

    public DataGroup<DateDiff> DateDiffData { get; } = DataGroup.Create(DateDiffs);

    private static IEnumerable<DateDiff> DateDiffs
    {
        get
        {
            yield return new(new(3, 4, 30), new(4, 2, 29), 0, 9, 30);
            yield return new(new(3, 4, 30), new(2, 3, 31), -1, 0, -30);
            yield return new(new(3, 4, 30), new(3, 3, 31), 0, 0, -30);

            yield return new(new(8, 2, 29), new(11, 2, 28), 2, 11, 30);
            yield return new(new(8, 2, 29), new(9, 2, 28), 0, 11, 30);
        }
    }
}

/// <summary>
/// Provides test data for <see cref="GregorianSchema"/> for math operations when the result is
/// ambiguous.
/// <para>See also <see cref="GregorianDataSet"/> for test data for math operations when the result
/// is unambiguous.</para>
/// </summary>
public class GregorianMathDataSetNext : IAdvancedMathDataSet, ISingleton<GregorianMathDataSetNext>
{
    private GregorianMathDataSetNext() { }

    public static GregorianMathDataSetNext Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly GregorianMathDataSetNext Instance = new();
        static Singleton() { }
    }

    public AdditionRules AdditionRules { get; } =
        new(DateAdditionRule.StartOfNextMonth, OrdinalAdditionRule.StartOfNextYear, MonthAdditionRule.StartOfNextYear);

    /// <inheritdoc/>
    /// <remarks>Intercalary day, expected result in a common year, years to be added.</remarks>
    public DataGroup<YemodaPairAnd<int>> AddYearsData { get; } = new()
    {
        new(new(4, 2, 29), new(11, 3, 1), 7),
        new(new(4, 2, 29), new(10, 3, 1), 6),
        new(new(4, 2, 29), new(9, 3, 1), 5),
        new(new(4, 2, 29), new(7, 3, 1), 3),
        new(new(4, 2, 29), new(6, 3, 1), 2),
        new(new(4, 2, 29), new(5, 3, 1), 1),

        new(new(8, 2, 29), new(7, 3, 1), -1),
        new(new(8, 2, 29), new(6, 3, 1), -2),
        new(new(8, 2, 29), new(5, 3, 1), -3),
        new(new(8, 2, 29), new(3, 3, 1), -5),
        new(new(8, 2, 29), new(2, 3, 1), -6),
        new(new(8, 2, 29), new(1, 3, 1), -7),
    };

#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations (Design)

    public DataGroup<YemodaPairAnd<int>> AddMonthsData => throw new NotImplementedException();
    public DataGroup<YedoyPairAnd<int>> AddYearsOrdinalData => throw new NotImplementedException();

#pragma warning restore CA1065
}
