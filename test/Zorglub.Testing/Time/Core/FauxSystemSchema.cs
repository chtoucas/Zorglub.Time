// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using Zorglub.Time.Core.Intervals;

using static Zorglub.Time.Core.CalendricalConstants;

public partial class FauxSystemSchema : SystemSchema
{
    private const int DefaultMinDaysInYear = 365;
    private const int DefaultMinDaysInMonth = 28;

    public FauxSystemSchema()
        : base(DefaultMinDaysInYear, DefaultMinDaysInMonth) { }

    // Base constructors.
    public FauxSystemSchema(int minDaysInYear, int minDaysInMonth)
        : base(minDaysInYear, minDaysInMonth) { }
    public FauxSystemSchema(Range<int> supportedYears, int minDaysInYear, int minDaysInMonth)
        : base(supportedYears, minDaysInYear, minDaysInMonth) { }

    // Constructor in order to test the base constructors.
    public FauxSystemSchema(Range<int> supportedYears)
        : base(supportedYears, DefaultMinDaysInYear, DefaultMinDaysInMonth) { }

    // Constructor to test the properties.
    public FauxSystemSchema(CalendricalFamily family)
        : this() { Family = family; }
    public FauxSystemSchema(CalendricalAdjustments adjustments)
        : this() { PeriodicAdjustments = adjustments; }
    public FauxSystemSchema(Range<int> supportedYears, Range<int> supportedYearsCore)
        : this(supportedYears) { SupportedYearsCore = supportedYearsCore; }

    // Pre-defined instances.
    public static FauxSystemSchema Regular12 => new FauxRegularSchema(12);
    public static FauxSystemSchema Regular13 => new FauxRegularSchema(13);
    public static FauxSystemSchema Regular14 => new FauxRegularSchema(14);

    [Pure]
    public static FauxSystemSchema WithMinDaysInYear(int minDaysInYear) =>
        new(minDaysInYear, DefaultMinDaysInMonth);

    [Pure]
    public static FauxSystemSchema WithMinDaysInMonth(int minDaysInMonth) =>
        new(DefaultMinDaysInYear, minDaysInMonth);

    private sealed class FauxRegularSchema : FauxSystemSchema, IRegularSchema
    {
        public FauxRegularSchema(int monthsInYear)
            : this(monthsInYear, DefaultMinDaysInYear, DefaultMinDaysInMonth) { }

        public FauxRegularSchema(int monthsInYear, int minDaysInYear, int minDaysInMonth)
            : base(minDaysInYear, minDaysInMonth)
        { MonthsInYear = monthsInYear; }

        public int MonthsInYear { get; }

        [Pure] public override int CountMonthsInYear(int y) => MonthsInYear;
    }
}

public partial class FauxSystemSchema // Props & methods
{
    public sealed override CalendricalFamily Family { get; } = CalendricalFamily.Other;
    public sealed override CalendricalAdjustments PeriodicAdjustments { get; } = CalendricalAdjustments.None;

    [Pure] public sealed override bool IsLeapYear(int y) => throw new NotSupportedException();
    [Pure] public sealed override bool IsIntercalaryMonth(int y, int m) => throw new NotSupportedException();
    [Pure] public sealed override bool IsIntercalaryDay(int y, int m, int d) => throw new NotSupportedException();
    [Pure] public sealed override bool IsSupplementaryDay(int y, int m, int d) => throw new NotSupportedException();

    [Pure] public override int CountMonthsInYear(int y) => 1;
    [Pure] public sealed override int CountDaysInYear(int y) => MinDaysInYear;
    [Pure] public sealed override int CountDaysInYearBeforeMonth(int y, int m) => throw new NotSupportedException();
    [Pure] public sealed override int CountDaysInMonth(int y, int m) => MinDaysInMonth;

    [Pure] public sealed override int CountMonthsSinceEpoch(int y, int m) => 0;
    public sealed override void GetMonthParts(int monthsSinceEpoch, out int y, out int m) => throw new NotSupportedException();
    [Pure] public sealed override int GetMonth(int y, int doy, out int d) => throw new NotSupportedException();
    [Pure] public sealed override int GetYear(int daysSinceEpoch) => throw new NotSupportedException();

    [Pure] public sealed override int GetStartOfYear(int y) => 0;
    public sealed override void GetDatePartsAtEndOfYear(int y, out int m, out int d) => throw new NotSupportedException();
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
}
