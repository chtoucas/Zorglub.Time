// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

#define USE_GREGORIAN_KERNEL_DATA

namespace Zorglub.Bulgroz.Prototypes;

using Zorglub.Time.Core.Prototypes;

public sealed partial class GregorianPrototype : ArchetypalSchema
{
    // Index 0 is fake in order for the span index to match the month index.
    // Another option for DaysInMonthOfXXX is to use "span index = month - 1",
    // in which case the initialization would work unchanged, only the method
    // CountDaysInYearBeforeMonth() would have to be updated.

#if USE_GREGORIAN_KERNEL_DATA
    private static readonly int[] s_DaysInYearBeforeMonthOfCommonYear =
        ReadOnlySpanHelpers.ConvertToCumulativeArray(GregorianKernel.DaysInMonthOfCommonYear);
    private static readonly int[] s_DaysInYearBeforeMonthOfLeapYear =
        ReadOnlySpanHelpers.ConvertToCumulativeArray(GregorianKernel.DaysInMonthOfLeapYear);

    [Pure]
    public static ReadOnlySpan<byte> CountDaysInMonths(bool leapYear) =>
        leapYear ? GregorianKernel.DaysInMonthOfLeapYear[1..]
        : GregorianKernel.DaysInMonthOfCommonYear[1..];
#else
    private static readonly int[] s_DaysInYearBeforeMonthOfCommonYear =
        { 0, 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
    private static readonly int[] s_DaysInYearBeforeMonthOfLeapYear =
        { 0, 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335 };

    // Index 0 is fake in order for the span index to match the month index.
    private static ReadOnlySpan<byte> DaysInMonthOfCommonYear =>
        new byte[] { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
    private static ReadOnlySpan<byte> DaysInMonthOfLeapYear =>
        new byte[] { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

    [Pure]
    public static ReadOnlySpan<byte> CountDaysInMonths(bool leapYear) =>
        leapYear ? DaysInMonthOfLeapYear[1..]
        : DaysInMonthOfCommonYear[1..];
#endif

    #region Initialization

    private GregorianPrototype()
        : base(
            GregorianKernel.Instance,
            minDaysInYear: 365,
            minDaysInMonth: 28)
    { }

    internal static GregorianPrototype Instance { get; } = new();

    public static Box<GregorianPrototype> GetInstance() => Box.Create(new GregorianPrototype());

    #endregion
}

public partial class GregorianPrototype // Overriden methods
{
    /// <inheritdoc />
    [Pure]
    public override int CountDaysInYearBeforeMonth(int y, int m)
    {
        // This method throws an IndexOutOfRangeException if m < 0 or m > 12.
        // Index 0 is fake.
        Debug.Assert(m != 0);

        return (IsLeapYear(y)
            ? s_DaysInYearBeforeMonthOfLeapYear
            : s_DaysInYearBeforeMonthOfCommonYear
        )[m];
    }
}
