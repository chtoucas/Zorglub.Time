// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

using Zorglub.Time.Core;
using Zorglub.Time.Geometry.Forms;
using Zorglub.Time.Geometry.Schemas;

using static Zorglub.Bulgroz.GJConstants;
using static Zorglub.Bulgroz.GregorianConstants;

/// <summary>
/// Provides geometric schemas for the Gregorian calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class GregorianGeometry : GJGeometry
{
    /// <summary>
    /// Represents the quasi-affine form (146_097, 4, 0).
    /// <para>This field is read-only.</para>
    /// </summary>
    /// <remarks>
    /// <para>This form encodes the sequence: 36_524, 36_524, 36_524, 36_525.
    /// </para>
    /// </remarks>
    public static readonly CenturyForm CenturyForm =
        new(DaysPer400YearCycle, 4, 0) { Origin = new Yemoda(0, 3, 1) };

    /// <summary>
    /// Gets the quasi-affine form (146_097, 4, -1224).
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <remarks>
    /// <para>This form encodes the sequence: 36_524, 36_524, 36_524, 36_525.
    /// </para>
    /// <para>It is <see cref="CenturyForm"/> but with a different origin:
    /// the value of the form at 0 is equal to -306.</para>
    /// </remarks>
    public static CenturyForm AltCenturyForm =>
        CenturyForm.PatchValue(-DaysFromEndOfFebruaryYear0ToEpoch);

    //
    // Geometric schemas.
    //

    /// <summary>
    /// Represents a geometric schema for the Gregorian calendar using
    /// the algebraic numbering system underneath.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <remarks>
    /// <para>With this schema, we recover the formulae used within
    /// <see cref="Zorglub.Time.Core.Schemas.GregorianSchema"/>.</para>
    /// </remarks>
    public static IGeometricSchema SchemaAlgebraicSystem => CreateSchema(MonthForm);

    /// <summary>
    /// Represents a geometric schema for the Gregorian calendar using
    /// the ordinal numbering system underneath.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static IGeometricSchema SchemaOrdinalSystem =>
        CreateSchema(MonthForm.WithOrdinalNumbering());

    /// <summary>
    /// Represents a geometric schema for the Gregorian calendar using
    /// the Troesch numbering system underneath.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static IGeometricSchema SchemaTroeschSystem =>
        CreateSchema(MonthForm.WithTroeschNumbering(ExceptionalMonth));

    // It doesn't matter which numbering system we are using since
    // Transpose() will rectify it automatically.
    private static IGeometricSchema CreateSchema(MonthForm monthForm) =>
        new ThirdOrderSchema(CenturyForm, YearForm, monthForm)
            .Transpose(MonthsInYear, ExceptionalMonth)
            .Rebase(DaysFromEndOfFebruaryYear0ToEpoch);

    //
    // Geometric schemas without Rebase().
    //

    // Dans les formes de base, l'origine est placée au 1er mars de l'an 0.
    // En déplacant l'origine de DaysFromFirstMarchYear0ToEpoch (dans le
    // futur) on se recale sur le 1er janvier de l'an 1 (voir
    // TransposedGeometricSchema).
    //
    // On doit quand même encore transposé les formules afin de pouvoir
    // rétablir la bonne numérotation des mois et par conséquent aussi des
    // années (voir les classes de régularisation).
    //
    // Si les deux formules suivantes sont équivalentes d'un point de vue
    // calculatoire, la version utilisant AltYearForm est moins naturelle.
    // En effet, AltYearForm décale l'origine de chaque cycle court (celui
    // de 4 ans) dans le cycle long (celui de 400 ans).
    //
    // NB : bien entendu ça marcherait aussi si on utilisait MonthForm
    // sous forme "ordinaire" ou "Troesch".

    // On incorpore DaysFromFirstMarchYear0ToEpoch à CenturyForm.
    public static IGeometricSchema AltSchemaAlgebraicSystem1 =>
        new ThirdOrderSchema(AltCenturyForm, YearForm, MonthForm)
            .Transpose(MonthsInYear, ExceptionalMonth);

    // On incorpore DaysFromFirstMarchYear0ToEpoch à YearForm.
    public static IGeometricSchema AltSchemaAlgebraicSystem2 =>
        new ThirdOrderSchema(CenturyForm, AltYearForm, MonthForm)
            .Transpose(MonthsInYear, ExceptionalMonth);
}

// Alternative forms obtained by rotating the sequences.
public partial class GregorianGeometry
{
    // Remarks:
    // - CenturyForm1 matches CenturyForm, the form with origin March 1st, 0 (aka 1 BC).
    // - CenturyForm0 is the right form when using the std origin 1/1/1.
    // - 109_573 = 300 * 365 + 73 = 2 * 36_524 + 36_525.

    /// <summary>Represents the quasi-affine form (146_097, 4, 3).</summary>
    /// <remarks><para>This form encodes the sequence: 36_525, 36_524, 36_524, 36_524.</para></remarks>
    public static readonly CenturyForm CenturyForm0 = new(DaysPer400YearCycle, 4, 3);

    /// <summary>Represents the quasi-affine form (146_097, 4, 0).</summary>
    /// <remarks><para>This form encodes the sequence: 36_524, 36_524, 36_524, 36_525.</para></remarks>
    public static readonly CenturyForm CenturyForm1 = new(DaysPer400YearCycle, 4, 0);

    /// <summary>Represents the quasi-affine form (146_097, 4, 1).</summary>
    /// <remarks><para>This form encodes the sequence 36_524, 36_524, 36_525, 36_524 repeated twice.</para></remarks>
    public static readonly CenturyForm CenturyForm2 = new(DaysPer400YearCycle, 3, 1);

    /// <summary>Represents the quasi-affine form (146_097, 4, 2).</summary>
    /// <remarks><para>This form encodes the sequence 36_524, 36_525, 36_524, 36_524 repeated twice.</para></remarks>
    public static readonly CenturyForm CenturyForm3 = new(DaysPer400YearCycle, 3, 2);
}
