// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Core;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;

/// <summary>
/// Provides facts about <see cref="CalendricalSchema"/>.
/// </summary>
public abstract class CalendricalSchemaFacts<TSchema, TDataSet> :
    ICalendricalSchemaPlusFacts<TSchema, TDataSet>
    where TSchema : CalendricalSchema
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected CalendricalSchemaFacts(TSchema schema) : base(schema) { }

    [Fact]
    public sealed override void Algorithm_Prop() =>
        Assert.Equal(CalendricalAlgorithm.Arithmetical, SchemaUT.Algorithm);

    [Fact]
    public void Domain_Prop()
    {
        int startOfYear = SchemaUT.GetStartOfYear(MinYear);
        int endOfYear = SchemaUT.GetEndOfYear(MaxYear);
        var domain = new Range<int>(OrderedPair.Create(startOfYear, endOfYear));
        // Act & Assert
        Assert.Equal(domain, SchemaUT.Domain);
        // Lazy prop: we duplicate the test to ensure full test coverage.
        Assert.Equal(domain, SchemaUT.Domain);
    }
}
