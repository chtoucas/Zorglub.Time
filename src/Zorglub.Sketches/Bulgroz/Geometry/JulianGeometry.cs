// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

using Zorglub.Time.Geometry.Forms;
using Zorglub.Time.Geometry.Schemas;

using static Zorglub.Bulgroz.GJConstants;

/// <summary>
/// Provides geometric schemas for the Julian calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class JulianGeometry : GJGeometry
{
    /// <summary>
    /// Represents a geometric schema for the Julian calendar using
    /// the algebraic numbering system underneath.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <remarks>
    /// <para>With this schema, we recover the formulae used within
    /// <see cref="Zorglub.Time.Core.Schemas.JulianSchema"/>.</para>
    /// </remarks>
    public static IGeometricSchema SchemaAlgebraicSystem =>
        CreateSchema(MonthForm);

    /// <summary>
    /// Represents a geometric schema for the Julian calendar using
    /// the ordinal numbering system underneath.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static IGeometricSchema SchemaOrdinalSystem =>
        CreateSchema(MonthForm.WithOrdinalNumbering());

    /// <summary>
    /// Represents a geometric schema for the Julian calendar using
    /// the Troesch numbering system underneath.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static IGeometricSchema SchemaTroeschSystem =>
        CreateSchema(MonthForm.WithTroeschNumbering(ExceptionalMonth));

    private static IGeometricSchema CreateSchema(MonthForm monthForm) =>
        new SecondOrderSchema(YearForm, monthForm)
            .Transpose(MonthsInYear, ExceptionalMonth)
            .Rebase(DaysFromEndOfFebruaryYear0ToEpoch);

    //
    // Geometric schemas without Rebase().
    //

    // On incorpore DaysFromFirstMarchYear0ToEpoch à YearForm.
    public static IGeometricSchema AltSchemaAlgebraicSystem =>
        new SecondOrderSchema(AltYearForm, MonthForm)
            .Transpose(MonthsInYear, ExceptionalMonth);
}
