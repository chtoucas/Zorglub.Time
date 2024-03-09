// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

using Zorglub.Time.Core;
using Zorglub.Time.Geometry.Forms;

using static Zorglub.Bulgroz.GJConstants;
using static Zorglub.Time.Core.CalendricalConstants;

// TODO(doc): expliquer dans quel contexte on peut utiliser les formes
// YearForm et AltYearForm.

// Les formes de type année sont de deux types.
// D'une part, YearForm0-4 que l'on prévoit d'utiliser pour elles-mêmes.
// D'autre part, YearForm et AltYearForm qui n'ont de sens qu'en tant que
// composantes d'un schéma géométrique.
//
// YearForm0-4
// -----------
// Ces quatre formes représentent les quatres manière d'encoder la suite des
// longueurs des années grégoriennes ou juliennes dans un cycle de 4 ans,
// c-à-d 3 années de 365 jours et une année de 366 jours.
// Avant de pouvoir les utiliser, il faut encore les normaliser.
// Le procédé de normalisation permet simplement de recentrer l'origine
// d'une forme sur l'epoch 01/01/0001 (on adopte ci le point de vue
// géométrique). On peut ainsi utiliser en paramètre une année sans
// modification (translation horizontale y -> y - y0 où y0 est l'année de
// l'origine de la forme), et de s'assurer que la valeur de la forme pour
// l'epoch est bien égale à 0 (translation verticale
// v -> v + daysFromEpochToOrigin).
//
// ### Normalisation.
// Examinons en détails la forme YearForm2. Cette dernière encode la
// séquence 365, 365, 366, 365 ayant pour origine le 01/01/0002. Avant
// normalisation, on a :
//   form.CountDaysInYear(0) = 365 // Valeur réelle pour y = 0 : 366
//   form.CountDaysInYear(1) = 365 // Valeur réelle pour y = 1 : 365
//   form.CountDaysInYear(2) = 366 // Valeur réelle pour y = 2 : 365
//   form.CountDaysInYear(3) = 365 // Valeur réelle pour y = 3 : 365
// ce qui est de toute évidence incorrect mais attendu car une forme de base
// utilise une séquence indexée à partir de 0, et pas 2 comme souhaité dans
// le cas présent. La translation horizontale permet de corriger cela :
//   form.CountDaysInYear(2) = 365
//   form.CountDaysInYear(3) = 365
//   form.CountDaysInYear(4) = 366
//   form.CountDaysInYear(5) = 365
// Quant à la translation verticale, elle modifie les valeurs de la forme
// c-à-d GetStartOfYear(). Sous la forme de base, on a :
//   form.GetStartOfYear(0) = 0     // Valeur réelle pour y = 0 : -366
//   form.GetStartOfYear(1) = 365   // Valeur réelle pour y = 1 : 0
//   form.GetStartOfYear(2) = 730   // Valeur réelle pour y = 2 : 365
//   form.GetStartOfYear(3) = 1096  // Valeur réelle pour y = 3 : 730
// On applique la translation verticale v -> v + 365 :
//   form.GetStartOfYear(0) = 365
//   form.GetStartOfYear(1) = 730
//   form.GetStartOfYear(2) = 1096
//   form.GetStartOfYear(3) = 1461
// puis la translation horizontale, pour enfn obtenir les bons résultats :
//   form.GetStartOfYear(2) = 365
//   form.GetStartOfYear(3) = 730
//   form.GetStartOfYear(4) = 1096
//   form.GetStartOfYear(5) = 1461
// IMPORTANT: YearForm0-4 sont définies ici sous leur forme de base,
// autrement dit AVANT normalisation.
//
// ### Domaine de validité.
// Quand on s'intéresse au calendrier julien, on peut utiliser les formes
// YearForm0-4 sont aucune restriction, autrement dit elles sont valables
// quelques soient les années. Ce n'est pas le cas avec le calendrier
// grégorien. En effet, ces formes ne prennent pas en compte la règle
// qui décrète que parmi les années séculaires seules les années multiples
// de 400 sont bissextiles.
//
// Précisément, en considérant les formes YearForm0-4 normalisées,
// - CountDaysInYear(y) donne le bon résultat à condition d'exclure les
//   années exceptionnelles par rapport au calendrer julien, c-à-d les
//   années séculaires communes.
// - GetStartOfYear(y) et GetYear(...) ne sont valables que lorsque "y"
//   (paramètre ou résultat) est dans l'intervalle [-99, 100]. On peut
//   garder l'année 100 car les calculs ne dépendent que du début d'une
//   année.
//
// YearForm & AltYearForm
// ----------------------
// Ces formes sont particulières, elles prennent pour origine le 1er mars de
// l'an zéro (1 BC), elles encodent la séquence 365, 365, 365, 366.
// Comme leurs origines ne correspondent pas au premier jour d'une année,
// telles quelles on ne peut pas facilemment les utiliser ; de toute façon
// comme on va le voir, ce n'est pas l'idée.
//
// Comme son nom l'indique, une forme année s'occupe (presque) exclusivement
// des années, mais on aimerait bien entendu pouvoir aussi utiliser les
// formes quasi-affines pour pouvoir calculer le DayNumber d'une date.
// Rappelons la formule :
//   GetStartOfYear(y) + CountDaysInYearBeforeMonth(y, m) + (d - 1)
// Le deuxième terme est relié au nombre de jours dans un mois.
//
//
//
// Le 1er mars de l'an 0 est un bon choix car le mois de février est ainsi
// repoussé en fin d'année, mais aussi parce que l'an 0 est le début d'un
// cycle de 4 ans où l'année bissextile est positionnée en fin de cycle.

// Attention, les formes de base prennent pour origine le 1er mars de
// l'an 0. Les formules non-transposées utilisent donc une origine décalée
// de DaysInYearAfterFebruary dans le passé par rapport à l'origine réelle
// du calendrier (grégorien ou julien).

/// <summary>
/// Provides a base for the Julian and Gregorian geometries.
/// </summary>
public abstract partial class GJGeometry
{
    protected GJGeometry() { }

    /// <summary>
    /// Represents the year form (1461, 4, 0).
    /// <para>This field is read-only.</para>
    /// </summary>
    /// <remarks>
    /// <para>This form encodes the sequence: 365, 365, 365, 366.</para>
    /// <para>This form origin is 01/03/0000.</para>
    /// </remarks>
    //
    // Normalement l'année zéro est bissextile et l'an 3 ne l'est pas mais,
    // comme pour MonthForm, on prend le 1er mars de l'an 0 pour origine.
    public static readonly YearForm YearForm =
        new(DaysPer4JulianYearCycle, 4, 0) { Origin = new Yemoda(0, 3, 1) };

    /// <summary>
    /// Gets the year form (1461, 4, -1224).
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <remarks>
    /// <para>This form encodes the sequence: 365, 365, 365, 366.</para>
    /// <para>This form origin is 01/03/0000.</para>
    /// <para>The value of the form at y = 0 is equal to -306.</para>
    /// </remarks>
    public static YearForm AltYearForm =>
        YearForm.PatchValue(-DaysFromEndOfFebruaryYear0ToEpoch);

    /// <summary>
    /// Represents the month form (153, 5, 2).
    /// <para>This field is read-only.</para>
    /// </summary>
    /// <remarks>
    /// <para>This form encodes the sequence: 31-30-31-30-31 repeated 2
    /// times followed by 31-30.</para>
    /// <para>It only matches the sequence of lengths of the <i>first eleven
    /// months</i> of a Gregorian or Julian year when March is considered to
    /// be the first month of the year.</para>
    /// </remarks>
    public static readonly MonthForm MonthForm = new(153, 5, 2) { Origin = new Yemoda(0, 3, 1) };
}

// TODO(doc): the comments below are wrong.

// Alternative year forms obtained by rotating the sequences.
//
// Remember that "YearForm" starts at 01/03/0000.
// If we could use the "real" start of year, that is 01/01, instead of
// "form" we would use "form0" and "form1" which start respectively
// at 01/01/0000 and 01/01/0001.
//   var form0 = new YearForm(1461, 4, 3);
//   var form1 = new YearForm(1461, 4, 0);
// and we would check that:
//   form0.ShiftEpochForwards(DaysInYearZero)
//   form1.WithYearOrigin(1)
// give the form:
//   new YearForm(1461, 4, -1461)
// This is the equivalent of AltYearForm.
// Summary: if the intercalary was not in the middle of the year
// we would have set GJGeometry.YearForm to (1461, 4, -1461).
// See TabularIslamicYearFormTests for a simpler situation, and a
// more interesting one (here "form0" and "form1" are rather useless).
public partial class GJGeometry
{
    /// <summary>Represents the year form (1461, 4, 3).</summary>
    /// <remarks>
    /// <para>This form encodes the sequence: 366, 365, 365, 365.</para>
    /// <para>This form origin is 01/01/0000.</para>
    /// </remarks>
    public static readonly YearForm YearForm0 =
        new(DaysPer4JulianYearCycle, 4, 3) { Origin = new Yemoda(0, 1, 1) };

    /// <summary>Represents the year form (1461, 4, 0).</summary>
    /// <remarks>
    /// <para>This form encodes the sequence: 365, 365, 365, 366.</para>
    /// <para>This form origin is 01/01/0001.</para>
    /// <para>It is <see cref="YearForm"/> BUT with a different origin.</para>
    /// </remarks>
    public static readonly YearForm YearForm1 = new(DaysPer4JulianYearCycle, 4, 0);

    /// <summary>Represents the year form (1461, 4, 1).</summary>
    /// <remarks>
    /// <para>This form encodes the sequence 365, 365, 366, 365 repeated twice.</para>
    /// <para>This form origin is 01/01/0002.</para>
    /// </remarks>
    public static readonly YearForm YearForm2 =
        new(DaysPer4JulianYearCycle, 4, 1) { Origin = new Yemoda(2, 1, 1) };

    /// <summary>Represents the year form (1461, 4, 2).</summary>
    /// <remarks>
    /// <para>This form encodes the sequence 365, 366, 365, 365 repeated twice.</para>
    /// <para>This form origin is 01/01/0003.</para>
    /// </remarks>
    public static readonly YearForm YearForm3 =
        new(DaysPer4JulianYearCycle, 4, 2) { Origin = new Yemoda(3, 1, 1) };
}

// Alternative month forms obtained by rotating the sequences.
//
// - MonthForm0 = form when the origin is January 1st and February removed.
// - MonthForm2 = MonthForm = form when the origin is March 1st and February removed.
// - Strictly speaking MonthForm2 should have been called MonthForm1,
//   but here we refer to the genuine sequence of (lengths of) months,
//   which we truncated because it isn't the code of a segment of a
//   digital straight line.
// - No other sequence (obtained after rotating the truncated genuine
//   seq) is a segment.
public partial class GJGeometry
{
    /// <summary>Represents the month form (153, 5, 4).</summary>
    /// <remarks><para>This form encodes the sequence:
    /// 31, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31.</para></remarks>
    public static readonly MonthForm MonthForm0 = new(153, 5, 4);

    /// <summary>Represents the month form (153, 5, 2).</summary>
    /// <remarks><para>This form encodes the sequence:
    /// 31, 30, 31, 30, 31, 31, 30, 31, 30, 31, 31.</para></remarks>
    public static readonly MonthForm MonthForm2 = new(153, 5, 2);
}
