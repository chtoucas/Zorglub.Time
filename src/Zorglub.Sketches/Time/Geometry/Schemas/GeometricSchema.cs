// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Geometry.Schemas;

using Zorglub.Time.Core;
using Zorglub.Time.Geometry.Forms;

// On n'inclut pas la forme triviale DayForm.

public abstract partial class GeometricSchema : IGeometricSchema
{
    protected GeometricSchema(YearForm yearForm, MonthForm monthForm)
    {
        YearForm = yearForm ?? throw new ArgumentNullException(nameof(yearForm));
        MonthForm = monthForm ?? throw new ArgumentNullException(nameof(monthForm));

        if (yearForm.Origin != monthForm.Origin)
        {
            // We pick up yearForm rather than monthForm since the latter is
            // usually the one that makes us choose a custom origin.
            Throw.Argument(nameof(yearForm));
        }
    }

    public YearForm YearForm { get; }

    public MonthForm MonthForm { get; }

    public Yemoda Origin => MonthForm.Origin;

    /// <inheritdoc />
    [Pure] public abstract int CountDaysSinceEpoch(int y, int m, int d);

    /// <inheritdoc />
    public abstract void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d);
}

// FIXME: GetMonth() & CountDaysInMonth().
// Conditions: ExceptionalMonth = MonthsInYear, Origin = (_, 1, 1),
// Ordinal Numbering, form for leap year, complete form.
// NB: les jours bissextils se trouvent en fin d'année.
// Un moyen de corriger ça serait d'appliquer le procédé de régularisation.
//
// Support complet: TabularIslamicSchema.
// Uniqt YearForm: Julian (ça pourrait marcher mais c'est un accident, de
// toute façon, la forme n'est pas normale).
// Uniqt MonthForm: Tropicalia3031 et Tropicalia3130.

public partial class GeometricSchema
{
    /// <summary>
    /// Valeur de la seconde forme quasi-affine (MonthForm).
    /// <para>Retourne le nombre de jours entre le début de l'année et le
    /// début du mois.</para>
    /// </summary>
    /// <remarks>
    /// Comme les jours bissextils sont placés en fin d'année, le résultat
    /// ne dépend pas de l'année, ni du fait que la forme des mois soit
    /// celle des années communes ou celle des années bissextiles !
    /// </remarks>
    [Pure]
    public int CountDaysInYearBeforeMonth(int m) => MonthForm.CountDaysInYearBeforeMonth(m);

    /// <summary>
    /// Code de la seconde forme quasi-affine (MonthForm).
    /// <para>ATTENTION : peut ne pas donner le résultat attendu pour le
    /// dernier mois, celui contenant les jours intercalaires.</para>
    /// </summary>
    [Pure]
    public int CountDaysInMonth(int m) => MonthForm.CountDaysInMonth(m);

    /// <summary>
    /// Valeur de l'inverse de la deuxième forme quasi-affine (MonthForm).
    /// <para>Peut ne pas marcher pour le dernier jour du mois, à moins
    /// que MonthForm soit la forme des mois des années bissextiles.</para>
    /// </summary>
    [Pure]
    public int GetMonth(int doy, out int d)
    {
        int m = MonthForm.GetMonth(doy - 1, out int d0);
        d = DayForm.GetDay(d0);
        return m;
    }
}
