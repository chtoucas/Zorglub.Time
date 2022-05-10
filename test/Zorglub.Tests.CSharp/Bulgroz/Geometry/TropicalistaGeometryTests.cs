// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

public sealed class Tropicalia3031ShortGeometryTests
    : IGeometricSchemaFacts<Tropicalia3031DataSet>
{
    public Tropicalia3031ShortGeometryTests() : base(TropicalistaGeometry.Schema3031Short) { }
}

public sealed class Tropicalia3031LongGeometryTests
    : IGeometricSchemaFacts<Tropicalia3031DataSet>
{
    public Tropicalia3031LongGeometryTests() : base(TropicalistaGeometry.Schema3031Long) { }
}

public sealed class Tropicalia3130GeometryTests
    : IGeometricSchemaFacts<Tropicalia3130DataSet>
{
    public Tropicalia3130GeometryTests() : base(TropicalistaGeometry.Schema3130) { }
}
