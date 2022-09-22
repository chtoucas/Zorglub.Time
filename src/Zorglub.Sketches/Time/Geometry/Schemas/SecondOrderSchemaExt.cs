// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Schemas;

using Zorglub.Time.Geometry.Forms;

// TODO: à supprimer?
public class SecondOrderSchemaExt
{
    private readonly SecondOrderSchema _schema;

    public SecondOrderSchemaExt(SecondOrderSchema schema)
    {
        _schema = schema ?? throw new ArgumentNullException(nameof(schema));

        MonthForm0 = schema.MonthForm.WithAlgebraicNumbering();
    }

    public MonthForm MonthForm0 { get; }

    /// <summary>
    /// Seconde forme quasi-affine, version initiale.
    /// <para>Retourne le nombre de jours entre le début d'une année
    /// bissextile et la fin du mois.</para>
    /// </summary>
    /// <remarks>
    /// <para>Parfois, par commodité, on obtient plutôt la formule pour les
    /// années communes.</para>
    /// <para>Si le calendrier n'est pas suffisamment régulier on peut être
    /// amené à exclure le dernier mois du calcul.</para>
    /// <para>Il s'agit de la forme quasi-affine étudiée par A.Troesch. Dans
    /// les faits, on n'a besoin que de la version "faible"
    /// <see cref="GeometricSchema.CountDaysInYearBeforeMonth(int)"/>.
    /// Par exemple, les calendriers "ptolémaïques" ne sont pas suffisamment
    /// réguliers (le dernier mois est trop long) pour pouvoir construire
    /// <see cref="CountDaysInLeapYearAtEndOfMonth(int)"/>, néanmoins ça
    /// ne pose aucun problème à partir du moment où on se contente de
    /// <see cref="GeometricSchema.CountDaysInYearBeforeMonth(int)"/>.
    /// </para>
    /// </remarks>
    [Pure]
    public int CountDaysInLeapYearAtEndOfMonth(int m) => MonthForm0.CountDaysInYearBeforeMonth(m);

    /// <summary>
    /// Retourne le nombre de jours dans un mois d'une année bissextile.
    /// </summary>
    [Pure]
    public int CountDaysInMonthOfLeapYear(int m) =>
        CountDaysInLeapYearAtEndOfMonth(m) - _schema.CountDaysInYearBeforeMonth(m);
}
