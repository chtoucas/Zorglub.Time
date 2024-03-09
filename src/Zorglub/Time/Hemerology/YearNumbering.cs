// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Hemerology;

// REVIEW(api): cycles of arbitrary fixed length and with a known starting
// point, for instance
//   GetCycle(int year, int len, int startYear, out int yearOfCycle)
// We could also not restrict this to years (cycles of days for instance).

/// <summary>Provides static methods for common operations related to decades, centuries, millennia
/// and eras.</summary>
/// <remarks>This class cannot be inherited.</remarks>
public static partial class YearNumbering { }

public partial class YearNumbering
{
    /// <summary>Obtains the century from the specified year.</summary>
    /// <remarks>The first century starts at the onset of year 1.</remarks>
    [Pure]
    // CIL code size = 9 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetCentury(int year) => MathZ.AdjustedDivide(year, 100);

    /// <summary>Obtains the year of the century from the specified year.</summary>
    /// <remarks>The first century starts at the onset of year 1.</remarks>
    [Pure]
    // CIL code size = 9 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetYearOfCentury(int year) => MathZ.AdjustedModulo(year, 100);
}

public partial class YearNumbering // Decades, centuries, millennia; default (chronological) numbering.
{
    // For a 0-based cycle, we use the adjusted division:
    //   GetCycle(int year, int len, out int yearOfCycle) =
    //     AdjustedDivide(year, len, yearOfCycle)

    /// <summary>Obtains the decade of the century from the specified year and also returns the
    /// century and the year of the decade in output parameters.</summary>
    [Pure]
    public static int GetDecadeOfCentury(int year, out int century, out int yearOfDecade)
    {
        century = MathZ.AdjustedDivide(year, 100);
        return MathZ.AdjustedDivide(year - 100 * (century - 1), 10, out yearOfDecade);
    }

    /// <summary>Obtains the decade from the specified year and also returns the year of the decade
    /// in an output parameter.</summary>
    /// <remarks>The first decade starts at the onset of year 0.</remarks>
    [Pure]
    public static int GetDecade(int year, out int yearOfDecade) =>
        MathZ.AdjustedDivide(year, 10, out yearOfDecade);

    /// <summary>Obtains the year from the specified decade and year of the decade.</summary>
    [Pure]
    public static int GetYearFromDecade(int decade, int yearOfDecade) =>
        GetYearFromCycle(10, decade, yearOfDecade);

    /// <summary>Obtains the century from the specified year and also returns the year of the
    /// century in an output parameter.</summary>
    /// <remarks>The first century starts at the onset of year 0.</remarks>
    [Pure]
    public static int GetCentury(int year, out int yearOfCentury) =>
        MathZ.AdjustedDivide(year, 100, out yearOfCentury);

    /// <summary>Obtains the year from the specified century and year of the century.</summary>
    [Pure]
    public static int GetYearFromCentury(int century, int yearOfCentury) =>
        GetYearFromCycle(100, century, yearOfCentury);

    /// <summary>Obtains the millennium from the specified year and also returns the year of the
    /// millennium in an output parameter.</summary>
    /// <remarks>The first millennium starts at the onset of year 0.</remarks>
    [Pure]
    public static int GetMillennium(int year, out int yearOfMillennium) =>
        MathZ.AdjustedDivide(year, 1000, out yearOfMillennium);

    /// <summary>Obtains the year from the specified millennium and year of the millennium.</summary>
    [Pure]
    public static int GetYearFromMillennium(int millennium, int yearOfMillennium) =>
        GetYearFromCycle(1000, millennium, yearOfMillennium);

    [Pure]
    private static int GetYearFromCycle(int len, int cycle, int yearOfCycle) =>
        len * (cycle - 1) + yearOfCycle;
}

public partial class YearNumbering // Decades, centuries, millennia; ISO numbering.
{
    // For a 1-based cycle, we use the augmented division:
    //   GetIsoCycle(int year, int len, out int yearOfCycle) =
    //     AugmentedDivide(year, len, yearOfCycle)

    // Decades, centuries, millennia; ISO numbering which, for once, matches the
    // common usage... except (maybe) near the beginning of the era:
    // - the year 1 is the second year of the first decade.
    // - the year 1 is the second year of the first century.
    // - the year 1 is the second year of the first millennium.
    // To be coherent, we could treat the first decade, century and millennium
    // separately. Two options:
    // - the first decade contains only 9 years, negative years are shift
    //   backward by 1. Caveats: the first decade is no longer a decade.
    // - the first decade contains 10 years but numbered from 0 to 9, negative
    //   years are unchanged. Caveats: the year zero is not ordinally numbered.
    // I guess that neither one is good. In fact, "common usage" being dependent
    // of the cultural habits, this is better left aside for now.

    /// <summary>Obtains the ISO decade of the century from the specified year and also returns the
    /// century and the year of the decade in output parameters.</summary>
    [Pure]
    public static int GetIsoDecadeOfCentury(int year, out int century, out int yearOfDecade)
    {
        century = MathZ.AugmentedDivide(year, 100);
        return MathZ.AugmentedDivide(year - 100 * (century - 1), 10, out yearOfDecade);
    }

    /// <summary>Obtains the ISO decade from the specified year and also returns the year of the
    /// decade in an output parameter.</summary>
    /// <remarks>The first ISO decade starts at the onset of year 0.</remarks>
    [Pure]
    public static int GetIsoDecade(int year, out int yearOfDecade) =>
        MathZ.AugmentedDivide(year, 10, out yearOfDecade);

    /// <summary>Obtains the year from the specified ISO decade and year of the decade.</summary>
    [Pure]
    public static int GetYearFromIsoDecade(int decade, int yearOfDecade) =>
        GetYearFromIsoCycle(10, decade, yearOfDecade);

    /// <summary>Obtains the ISO century from the specified year and also returns the year of the
    /// century in an output parameter.</summary>
    /// <remarks>The first ISO century starts at the onset of year 0.</remarks>
    [Pure]
    public static int GetIsoCentury(int year, out int yearOfCentury) =>
        MathZ.AugmentedDivide(year, 100, out yearOfCentury);

    /// <summary>Obtains the year from the specified ISO century and year of the century.</summary>
    [Pure]
    public static int GetYearFromIsoCentury(int century, int yearOfCentury) =>
        GetYearFromIsoCycle(100, century, yearOfCentury);

    /// <summary>Obtains the ISO millennium from the specified year and also returns the year of the
    /// millennium in an output parameter.</summary>
    /// <remarks>The first ISO millennium starts at the onset of year 0.</remarks>
    [Pure]
    public static int GetIsoMillennium(int year, out int yearOfMillennium) =>
        MathZ.AugmentedDivide(year, 1000, out yearOfMillennium);

    /// <summary>Obtains the year from the specified ISO millennium and year of the millennium.
    /// </summary>
    [Pure]
    public static int GetYearFromIsoMillennium(int millennium, int yearOfMillennium) =>
        GetYearFromIsoCycle(1000, millennium, yearOfMillennium);

    [Pure]
    private static int GetYearFromIsoCycle(int len, int cycle, int yearOfCycle) =>
        len * (cycle - 1) + yearOfCycle - 1;
}
