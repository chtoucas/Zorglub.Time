// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

using Zorglub.Time.Core;
using Zorglub.Time.Geometry.Forms;
using Zorglub.Time.Geometry.Schemas;

using static Zorglub.Bulgroz.TabularIslamicConstants;

/// <summary>
/// Provides geometric schemas for the Tabular Islamic calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class TabularIslamicGeometry
{
    /// <summary>
    /// Represents the quasi-affine form (10_631, 30, 3).
    /// <para>This field is read-only.</para>
    /// </summary>
    public static readonly YearForm YearForm0 =
        new(DaysPer30YearCycle, 30, 3) { Origin = new Yemoda(0, 1, 1) };

    /// <summary>
    /// Represents the quasi-affine form (10_631, 30, -10_617).
    /// <para>This field is read-only.</para>
    /// </summary>
    public static YearForm YearForm => YearForm0.Normalize();

    /// <summary>
    /// Represents the quasi-affine form (325, 11, 5).
    /// <para>This field is read-only.</para>
    /// </summary>
    /// <remarks>
    /// <para>This form encodes the sequence: 30-29 repeated 5 times,
    /// followed by 30-30.</para>
    /// <para>It matches the sequence of lengths of months in a <i>leap</i>
    /// year.</para>
    /// </remarks>
    public static readonly MonthForm MonthFormForLeapYear = new(325, 11, 5);

    //
    // Geometric schemas.
    //

    /// <summary>
    /// Represents a geometric schema for the Tabular Islamic calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <remarks>
    /// <para>With this schema, we recover the formulae used within
    /// <see cref="Zorglub.Time.Core.Schemas.TabularIslamicSchema"/>.</para>
    /// </remarks>
    //
    // La constante 10_631 est trop grande pour envisager de pouvoir remplacer
    // CountDaysSinceEpoch() et GetStartOfYear() dans TabularIslamicSchema
    // par les versions construites ici. Aussi, CountDaysInYearBeforeMonth()
    // peut être obtenue de manière plus simple.
    // Par contre, on s'en inspire pour obtenir les formules inverses de
    // GetMonth() et GetYear().
    public static SecondOrderSchema SchemaOrdinalSystem =>
        new(YearForm, MonthFormForLeapYear.WithOrdinalNumbering());

    /// <summary>
    /// Represents a geometric schema for the Tabular Islamic calendar using
    /// the algebraic numbering system underneath.
    /// </summary>
    public static IGeometricSchema SchemaAlgebraicSystem =>
        new SecondOrderSchema(YearForm, MonthFormForLeapYear)
            .Transpose(MonthsInYear, ExceptionalMonth);
}
