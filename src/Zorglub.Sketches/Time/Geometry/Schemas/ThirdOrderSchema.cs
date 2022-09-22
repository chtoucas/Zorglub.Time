// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Schemas;

using Zorglub.Time.Geometry.Forms;

/// <summary>
/// Represents a geometric schema of order 3.
/// <para>Distribution of leap years fully encoded by the forms CenturyForm
/// and YearForm.</para>
/// <para><i>Intercalary days positioned at the end of a leap year.</i></para>
/// </summary>
/// <remarks>
/// <para>Convient au calendrier grégorien.</para>
/// <para>Il s'agit presque des formules pour une base de numération
/// quasi-affine d'ordre 3.</para>
/// </remarks>
public sealed partial class ThirdOrderSchema : GeometricSchema
{
    public ThirdOrderSchema(
        CenturyForm centuryForm,
        YearForm yearForm,
        MonthForm monthForm)
        : base(yearForm, monthForm)
    {
        CenturyForm = centuryForm ?? throw new ArgumentNullException(nameof(centuryForm));

        Debug.Assert(monthForm != null);
        if (centuryForm.Origin != monthForm.Origin)
        {
            Throw.Argument(nameof(centuryForm));
        }
    }

    public CenturyForm CenturyForm { get; }

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysSinceEpoch(int y, int m, int d)
    {
        // Détermination du siècle (c) et de l'année dans le siècle (Y).
        int c = MathZ.Divide(y, 100, out int Y0);

        // Ça marche parce que le jour bissextil d'une année séculaire
        // (bissextile) est placé en toute fin d'année et est donc
        // entièrement pris en charge par le dernier terme (d - 1).
        return CenturyForm.GetStartOfCentury(c)
            // Formules d'ordre 2 avec (Y, m, d) à la place de (y, m, d).
            + YearForm.GetStartOfYear(Y0)
            + MonthForm.CountDaysInYearBeforeMonth(m)
            + DayForm.CountDaysInMonthBeforeDay(d);
    }

    /// <inheritdoc />
    public sealed override void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        // Détermination du siècle (c) et du rang dans le siècle (Y).
        int c = CenturyForm.GetCentury(daysSinceEpoch, out int D);
        // Formules d'ordre 2 avec D à la place de daysSinceEpoch.
        int Y0 = YearForm.GetYear(D, out int d0y);
        m = MonthForm.GetMonth(d0y, out int d0);
        d = DayForm.GetDay(d0);

        // Passage de l'année dans le siècle (Y) à l'année tout court (y).
        y = 100 * c + Y0;
    }
}

public partial class ThirdOrderSchema : GeometricSchema
{
    // Cette méthode marche même si le jour bissextil n'est pas en fin
    // d'année.
    [Pure]
    public int GetStartOfYear(int y)
    {
        // REVIEW: pourquoi y-- ? pourquoi on n'utilise pas YearNumbering ?
        y--;
        int c = MathZ.Divide(y, 100, out int Y);
        return CenturyForm.GetStartOfCentury(c)
            // Formule d'ordre 2 avec Y à la place de y.
            + YearForm.GetStartOfYear(Y);
    }

    // TODO: more methods.
}
