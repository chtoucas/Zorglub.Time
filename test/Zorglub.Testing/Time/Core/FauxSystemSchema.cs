// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using Zorglub.Time.Core.Intervals;

using static Zorglub.Time.Core.CalendricalConstants;

public partial class FauxSystemSchema : SystemSchema
{
    private const int DefaultMinDaysInYear = 365;
    private const int DefaultMinDaysInMonth = 28;

    // Base constructors.

    public FauxSystemSchema(int minDaysInYear, int minDaysInMonth)
        : base(minDaysInYear, minDaysInMonth) { }

    public FauxSystemSchema(Range<int> supportedYears, int minDaysInYear, int minDaysInMonth)
        : base(supportedYears, minDaysInYear, minDaysInMonth) { }

    // Constructors in order to test the base constructors.

    public FauxSystemSchema(Range<int> supportedYears)
        : base(supportedYears, DefaultMinDaysInYear, DefaultMinDaysInMonth) { }

    public FauxSystemSchema(CalendricalFamily family)
        : base(DefaultMinDaysInYear, DefaultMinDaysInMonth)
    {
        Family = family;
    }

    public FauxSystemSchema(CalendricalAdjustments adjustments)
        : base(DefaultMinDaysInYear, DefaultMinDaysInMonth)
    {
        PeriodicAdjustments = adjustments;
    }

    // Constructor to test the setter for SupportedYearsCore.
    public FauxSystemSchema(Range<int> supportedYears, Range<int> supportedYearsCore)
        : base(supportedYears, DefaultMinDaysInYear, DefaultMinDaysInMonth)
    {
        SupportedYearsCore = supportedYearsCore;
    }

    public static FauxSystemSchema Default { get; } = new(DefaultMinDaysInYear, DefaultMinDaysInMonth);

    public sealed override CalendricalFamily Family { get; } = CalendricalFamily.Other;
    public sealed override CalendricalAdjustments PeriodicAdjustments { get; } = CalendricalAdjustments.None;

    [Pure] public sealed override bool IsLeapYear(int y) => throw new NotSupportedException();
    [Pure] public sealed override bool IsIntercalaryMonth(int y, int m) => throw new NotSupportedException();
    [Pure] public sealed override bool IsIntercalaryDay(int y, int m, int d) => throw new NotSupportedException();
    [Pure] public sealed override bool IsSupplementaryDay(int y, int m, int d) => throw new NotSupportedException();

    [Pure] public override int CountMonthsInYear(int y) => throw new NotSupportedException();
    [Pure] public sealed override int CountDaysInYear(int y) => MinDaysInYear;
    [Pure] public sealed override int CountDaysInYearBeforeMonth(int y, int m) => throw new NotSupportedException();
    [Pure] public sealed override int CountDaysInMonth(int y, int m) => MinDaysInMonth;

    [Pure] public sealed override int GetMonth(int y, int doy, out int d) => throw new NotSupportedException();
    [Pure] public sealed override int GetYear(int daysSinceEpoch) => throw new NotSupportedException();

    [Pure] public sealed override int GetStartOfYear(int y) => 0;
    public sealed override void GetEndOfYearParts(int y, out int m, out int d) => throw new NotSupportedException();
}

public partial class FauxSystemSchema // Profiles
{
    public static readonly TheoryData<FauxSystemSchema> NotLunar = new()
    {
        new FauxRegularSchema(Lunar.MonthsInYear + 1, Lunar.MinDaysInYear, Lunar.MinDaysInMonth),
        new FauxRegularSchema(Lunar.MonthsInYear, Lunar.MinDaysInYear - 1, Lunar.MinDaysInMonth),
        new FauxRegularSchema(Lunar.MonthsInYear, Lunar.MinDaysInYear, Lunar.MinDaysInMonth - 1),
    };

    public static readonly TheoryData<FauxSystemSchema> NotLunisolar = new()
    {
        new FauxSystemSchema(Lunisolar.MinDaysInYear - 1, Lunisolar.MinDaysInMonth),
        new FauxSystemSchema(Lunisolar.MinDaysInYear, Lunisolar.MinDaysInMonth - 1),
    };

    public static readonly TheoryData<FauxSystemSchema> NotSolar12 = new()
    {
        new FauxRegularSchema(Solar12.MonthsInYear + 1, Solar.MinDaysInYear, Solar.MinDaysInMonth),
        new FauxRegularSchema(Solar12.MonthsInYear, Solar.MinDaysInYear - 1, Solar.MinDaysInMonth),
        new FauxRegularSchema(Solar12.MonthsInYear, Solar.MinDaysInYear, Solar.MinDaysInMonth - 1),
    };

    public static readonly TheoryData<FauxSystemSchema> NotSolar13 = new()
    {
        new FauxRegularSchema(Solar13.MonthsInYear + 1, Solar.MinDaysInYear, Solar.MinDaysInMonth),
        new FauxRegularSchema(Solar13.MonthsInYear, Solar.MinDaysInYear - 1, Solar.MinDaysInMonth),
        new FauxRegularSchema(Solar13.MonthsInYear, Solar.MinDaysInYear, Solar.MinDaysInMonth - 1),
    };

    private sealed class FauxRegularSchema : FauxSystemSchema, IRegularSchema
    {
        public FauxRegularSchema(int monthsInYear, int minDaysInYear, int minDaysInMonth)
            : base(minDaysInYear, minDaysInMonth)
        {
            MonthsInYear = monthsInYear;
        }

        public int MonthsInYear { get; }

        [Pure] public sealed override int CountMonthsInYear(int y) => MonthsInYear;
    }
}
