// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Geometry.Forms;

/// <summary>
/// This form encodes the sequence of lengths of the years in a calendrical cycle.
/// </summary>
/// <remarks>
/// <para>To build such a form, we must in general change the origin of years (the epoch) such
/// that the exceptionnal year becomes the last one.</para>
/// </remarks>
public record YearForm(int DaysPerCycle, int YearsPerCycle, int Remainder)
    : CalendricalForm(DaysPerCycle, YearsPerCycle, Remainder)
{
    // FIXME: ce n'est pas suffisant, il faut aussi que le param y soit
    // "normal", dans le sens où on n'a pas besoin de manip du genre y -> y - 1.
    public bool IsNormal => Origin == Epoch;

    // TODO: naming PatchValue, add to the other forms?
    // Il faut que GetStartOfYear(1 - y0) soit valide.

    // Normalisation d'une forme "presque" normale.
    // - changement de coordonnée : y -> y - y0.
    // - translation verticale.
    //
    // InvalidOperationException
    [Pure]
    public YearForm Normalize()
    {
        var (y0, m0, d0) = Origin;

        if ((m0, d0) != (1, 1)) Throw.InvalidOperation();

        // On compte le nombre de jour entre l'epoch et l'origine de la forme.
        int daysFromEpochToOrigin = CountDaysFromEpochToStartOfYear(y0);

        // Forme d'origine "form" :
        //   form.GetStartOfYear(y)
        //     = (DaysPerCycle * y + Remainder) / YearsPerCycle
        // Forme corrigée :
        //   form.GetStartOfYear(y - y0) + daysFromEpochToOrigin
        //     = (DaysPerCycle * (y - y0) + Remainder) / YearsPerCycle + daysFromEpochToOrigin
        return this with
        {
            Remainder = Remainder - DaysPerCycle * y0 + YearsPerCycle * daysFromEpochToOrigin,
            Origin = Epoch
        };
    }

    // Translation verticale.
    // - Altère GetStartOfYear(y) c-à-d ValueAt(y).
    // - Ne change pas CountDaysInYear(y) c-à-d CodeAt(y)
    // If we want to only "normalize" ValueAt(), we can use
    //   NormalizeValue(daysFromEpochToOrigin)
    // where
    //   daysFromEpochToOrigin < 0: shift forwards, that is when Origin < Epoch.
    //   daysFromEpochToOrigin > 0: shift backwards, that is when Origin > Epoch.
    // This is mostly useful when the origin is not set at the start of a year.
    [Pure]
    public YearForm PatchValue(int days) =>
        // Forme de base "form0" :
        //   daysSinceEpoch0
        //     = form0.GetStartOfYear(y) + doy - 1
        //     = form0.ValueAt(y) + doy - 1
        //     = (DaysPerCycle * y + Remainder) / YearsPerCycle + doy - 1
        // Forme modifiée :
        //   daysSinceEpoch
        //     = daysSinceEpoch0 + days
        //     = (DaysPerCycle * y + Remainder + YearsPerCycle * days) / YearsPerCycle + doy - 1
        this with { Remainder = Remainder + YearsPerCycle * days };

    [Pure]
    public int CountDaysFromEpochToStartOfYear(int y)
    {
        int y0 = Origin.Year;

        // On est tenté d'utiliser
        //   GetStartOfYear(y) - GetStartOfYear(1)
        // mais ça n'est pas bon. En effet, il faut ne pas oublier
        // d'appliquer la translation y -> y - y0.
        return GetStartOfYear(y - y0) - GetStartOfYear(1 - y0);
    }

    /// <summary>
    /// Counts the number of consecutive days from <see cref="CalendricalForm.Origin"/> to the
    /// first day of the specified year.
    /// </summary>
    [Pure]
    public int GetStartOfYear(int y) => ValueAt(y);

    /// <summary>
    /// Obtains the year of the specified count of consecutive days since
    /// <see cref="CalendricalForm.Origin"/>.
    /// </summary>
    //
    // Il s'agit de la formule standard donnant le jour de l'année :
    //   int d0y = daysSinceEpoch - GetStartOfYear(y)
    [Pure]
    public int GetYear(int daysSinceEpoch, out int d0y) => Divide(daysSinceEpoch, out d0y);

    [Pure]
    public int GetYear(int daysSinceEpoch) => Divide(daysSinceEpoch);

    /// <summary>
    /// Obtains the number of days in the specified year.
    /// </summary>
    [Pure]
    public int CountDaysInYear(int y) => CodeAt(y);
}
