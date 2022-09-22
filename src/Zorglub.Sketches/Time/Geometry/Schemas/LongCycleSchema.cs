// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Schemas;

// TODO: this should work too with the Gregorian calendar (400 years).
// - month forms catalog + tests.
// - TransformedSchema.
// - MonthForm validity.
// - schema origin.
// - SecondOrderSchemaExt + the other ones.

public sealed partial class LongCycleSchema : IGeometricSchema
{
    public LongCycleSchema(
        IGeometricSchema shortCycleShema,
        int daysPerLongCycle,
        int yearsPerLongCycle)
    {
        ShortCycleShema = shortCycleShema ?? throw new ArgumentNullException(nameof(shortCycleShema));
        DaysPerLongCycle = daysPerLongCycle;
        YearsPerLongCycle = yearsPerLongCycle;
    }

    public IGeometricSchema ShortCycleShema { get; }

    public int DaysPerLongCycle { get; }

    public int YearsPerLongCycle { get; }

    /// <inheritdoc />
    [Pure]
    public int CountDaysSinceEpoch(int y, int m, int d)
    {
        // TODO: expliquer y--, en fait faire plus général.
        y--;
        int C = MathZ.Divide(y, YearsPerLongCycle, out int Y);

        return DaysPerLongCycle * C
            // Formules d'ordre 2 avec Y à la place de y.
            + ShortCycleShema.CountDaysSinceEpoch(Y, m, d);
    }

    /// <inheritdoc />
    public void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        int C = MathZ.Divide(daysSinceEpoch, DaysPerLongCycle, out int D);

        // Formules d'ordre 2 avec (D, Y) à la place de (daysSinceEpoch, y).
        ShortCycleShema.GetDateParts(D, out int Y, out m, out d);

        y = YearsPerLongCycle * C + Y + 1;
    }
}
