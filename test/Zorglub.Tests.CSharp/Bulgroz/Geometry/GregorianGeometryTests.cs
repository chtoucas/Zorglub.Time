// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

public sealed class AlgebraicGregorianGeometryTests
    : IGeometricSchemaFacts<GregorianDataSet>
{
    public AlgebraicGregorianGeometryTests() : base(GregorianGeometry.SchemaAlgebraicSystem) { }
}

public sealed class OrdinalGregorianGeometryTests
    : IGeometricSchemaFacts<GregorianDataSet>
{
    public OrdinalGregorianGeometryTests() : base(GregorianGeometry.SchemaOrdinalSystem) { }
}

public sealed class TroeschGregorianGeometryTests
    : IGeometricSchemaFacts<GregorianDataSet>
{
    public TroeschGregorianGeometryTests() : base(GregorianGeometry.SchemaTroeschSystem) { }
}

public sealed class AltAlgebraicGregorian1GeometryTests
    : IGeometricSchemaFacts<GregorianDataSet>
{
    public AltAlgebraicGregorian1GeometryTests() : base(GregorianGeometry.AltSchemaAlgebraicSystem1) { }
}

public sealed class AltAlgebraicGregorian2GeometryTests
    : IGeometricSchemaFacts<GregorianDataSet>
{
    public AltAlgebraicGregorian2GeometryTests() : base(GregorianGeometry.AltSchemaAlgebraicSystem2) { }
}
