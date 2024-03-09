// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

public sealed class AlgebraicJulianGeometryTests
    : IGeometricSchemaFacts<JulianDataSet>
{
    public AlgebraicJulianGeometryTests() : base(JulianGeometry.SchemaAlgebraicSystem) { }
}

public sealed class OrdinalJulianGeometryTests
    : IGeometricSchemaFacts<JulianDataSet>
{
    public OrdinalJulianGeometryTests() : base(JulianGeometry.SchemaOrdinalSystem) { }
}

public sealed class TroeschJulianGeometryTests
    : IGeometricSchemaFacts<JulianDataSet>
{
    public TroeschJulianGeometryTests() : base(JulianGeometry.SchemaTroeschSystem) { }
}

public sealed class AltAlgebraicJulianGeometryTests
    : IGeometricSchemaFacts<JulianDataSet>
{
    public AltAlgebraicJulianGeometryTests() : base(JulianGeometry.AltSchemaAlgebraicSystem) { }
}
