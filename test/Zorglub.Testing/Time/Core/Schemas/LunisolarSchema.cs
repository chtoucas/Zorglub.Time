// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas;

using static Zorglub.Time.Core.CalendricalConstants;

// NB: waiting for HebrewSchema.

/// <summary>
/// Represents a <b>fake</b> lunisolar schema.
/// </summary>
public sealed class LunisolarSchema :
    SystemSchema,
    IDaysInMonthDistribution,
    IBoxable<LunisolarSchema>
{
    public LunisolarSchema() : base(Lunisolar.MinDaysInYear, Lunisolar.MinDaysInMonth) { }

    [Pure]
    public static Box<LunisolarSchema> GetInstance() => Box.Create(new LunisolarSchema());

    [Pure]
    static ReadOnlySpan<byte> IDaysInMonthDistribution.GetDaysInMonthDistribution(bool leap) =>
        leap
        ? new byte[13] { 30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30 }
        : new byte[12] { 30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 29 };

    public override CalendricalFamily Family => CalendricalFamily.Lunisolar;
    public override CalendricalAdjustments PeriodicAdjustments => CalendricalAdjustments.Months;

    [Pure] public override bool IsLeapYear(int y) => (y & 3) == 0;
    [Pure] public override bool IsIntercalaryMonth(int y, int m) => m == 13;
    [Pure] public override bool IsIntercalaryDay(int y, int m, int d) => false;
    [Pure] public override bool IsSupplementaryDay(int y, int m, int d) => false;

    [Pure] public override int CountMonthsInYear(int y) => IsLeapYear(y) ? 13 : 12;
    [Pure] public override int CountDaysInYear(int y) => IsLeapYear(y) ? 384 : 354;
    [Pure] public override int CountDaysInYearBeforeMonth(int y, int m) => 29 * (m - 1) + (m >> 1);
    [Pure] public override int CountDaysInMonth(int y, int m) => 29 + (m & 1);

    [Pure]
    public override int CountMonthsSinceEpoch(int y, int m) =>
        throw new NotImplementedException();

    public override void GetMonthParts(int monthsSinceEpoch, out int y, out int m) =>
        throw new NotImplementedException();

    [Pure]
    public override int GetMonth(int y, int doy, out int d)
    {
        int d0y = doy - 1;
        int m = ((d0y << 1) + 59) / 59;
        d = 1 + d0y - 29 * (m - 1) - (m >> 1);
        return m;
    }

    [Pure]
    public override int GetYear(int daysSinceEpoch)
    {
        int C = MathZ.Divide(daysSinceEpoch, 1446, out int D);
        return (C << 2) + (D >= 1416 ? 4 : 1 + D / 354);
    }

    [Pure]
    public override int GetStartOfYear(int y) => 354 * (--y) + 30 * (y >> 2);

    public override void GetEndOfYearParts(int y, out int m, out int d)
    {
        if (IsLeapYear(y))
        {
            m = 13;
            d = 30;
        }
        else
        {
            m = 12;
            d = 29;
        }
    }
}
