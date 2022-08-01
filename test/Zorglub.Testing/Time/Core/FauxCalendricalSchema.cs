// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

// GetStartOfYear() must be implemented for calendars, arithmetic & scope to work.
// We must also implement IsRegular(), otherwise CalendricalSchema.Profile will fail.
// CountMonthsInYear() (must be > 0) and CountMonthsSinceEpoch() must be implemented
// too for arithmetic.

using Zorglub.Time.Core.Intervals;

using static Zorglub.Time.Core.CalendricalConstants;

public partial class FauxCalendricalSchema : CalendricalSchema
{
    private const int DefaultMinDaysInYear = 365;
    private const int DefaultMinDaysInMonth = 28;

    public FauxCalendricalSchema()
        : base(Yemoda.SupportedYears, DefaultMinDaysInYear, DefaultMinDaysInMonth) { }

    // Base constructor.
    protected FauxCalendricalSchema(Range<int> supportedYears, int minDaysInYear, int minDaysInMonth)
        : base(supportedYears, minDaysInYear, minDaysInMonth) { }

    // Constructor to be able to test the base constructors; see also WithMinDaysInXXX().
    public FauxCalendricalSchema(Range<int> supportedYears)
        : base(supportedYears, DefaultMinDaysInYear, DefaultMinDaysInMonth) { }
    private FauxCalendricalSchema(int minDaysInYear, int minDaysInMonth)
        : base(Yemoda.SupportedYears, minDaysInYear, minDaysInMonth) { }

    // Pre-defined instances.
    public static FauxCalendricalSchema Regular12 => new FauxRegularSchema(12);
    public static FauxCalendricalSchema Regular13 => new FauxRegularSchema(13);
    public static FauxCalendricalSchema Regular14 => new FauxRegularSchema(14);

    // Constructor to be able to test the setter for PreValidator.
    [Pure]
    public static FauxCalendricalSchema
        WithPreValidator(Func<CalendricalSchema, ICalendricalPreValidator> preValidator) =>
        new FauxRegularSchema(preValidator);

    [Pure]
    public static FauxCalendricalSchema WithMinDaysInYear(int minDaysInYear) =>
        new(minDaysInYear, DefaultMinDaysInMonth);

    [Pure]
    public static FauxCalendricalSchema WithMinDaysInMonth(int minDaysInMonth) =>
        new(DefaultMinDaysInYear, minDaysInMonth);

    private sealed class FauxRegularSchema : FauxCalendricalSchema, IRegularFeaturette
    {
        public FauxRegularSchema(Func<CalendricalSchema, ICalendricalPreValidator> preValidator)
            : this(12)
        {
            Requires.NotNull(preValidator);

            // NB: it will only works with Solar12PreValidator...
            PreValidator = preValidator.Invoke(this);
        }

        public FauxRegularSchema(int monthsInYear)
            : this(monthsInYear, DefaultMinDaysInYear, DefaultMinDaysInMonth) { }

        public FauxRegularSchema(int monthsInYear, int minDaysInYear, int minDaysInMonth)
            : base(minDaysInYear, minDaysInMonth)
        { MonthsInYear = monthsInYear; }

        public int MonthsInYear { get; }

        [Pure] public override int CountMonthsInYear(int y) => MonthsInYear;
    }
}

public partial class FauxCalendricalSchema // Props & methods
{
    public sealed override CalendricalFamily Family => throw new NotSupportedException();
    public sealed override CalendricalAdjustments PeriodicAdjustments => throw new NotSupportedException();

    [Pure]
    public override bool IsRegular(out int monthsInYear)
    {
        if (this is IRegularFeaturette sch)
        {
            monthsInYear = sch.MonthsInYear;
            return true;
        }
        else
        {
            monthsInYear = 0;
            return false;
        }
    }

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
}

public partial class FauxCalendricalSchema // Profiles
{
    public static readonly TheoryData<FauxCalendricalSchema> NotLunar = new()
    {
        new FauxRegularSchema(Lunar.MonthsInYear + 1, Lunar.MinDaysInYear, Lunar.MinDaysInMonth),
        new FauxRegularSchema(Lunar.MonthsInYear, Lunar.MinDaysInYear - 1, Lunar.MinDaysInMonth),
        new FauxRegularSchema(Lunar.MonthsInYear, Lunar.MinDaysInYear, Lunar.MinDaysInMonth - 1),
    };

    public static readonly TheoryData<FauxCalendricalSchema> NotLunisolar = new()
    {
        new FauxCalendricalSchema(Lunisolar.MinDaysInYear - 1, Lunisolar.MinDaysInMonth),
        new FauxCalendricalSchema(Lunisolar.MinDaysInYear, Lunisolar.MinDaysInMonth - 1),
    };

    public static readonly TheoryData<FauxCalendricalSchema> NotSolar12 = new()
    {
        new FauxRegularSchema(Solar12.MonthsInYear + 1, Solar.MinDaysInYear, Solar.MinDaysInMonth),
        new FauxRegularSchema(Solar12.MonthsInYear, Solar.MinDaysInYear - 1, Solar.MinDaysInMonth),
        new FauxRegularSchema(Solar12.MonthsInYear, Solar.MinDaysInYear, Solar.MinDaysInMonth - 1),
    };

    public static readonly TheoryData<FauxCalendricalSchema> NotSolar13 = new()
    {
        new FauxRegularSchema(Solar13.MonthsInYear + 1, Solar.MinDaysInYear, Solar.MinDaysInMonth),
        new FauxRegularSchema(Solar13.MonthsInYear, Solar.MinDaysInYear - 1, Solar.MinDaysInMonth),
        new FauxRegularSchema(Solar13.MonthsInYear, Solar.MinDaysInYear, Solar.MinDaysInMonth - 1),
    };
}
