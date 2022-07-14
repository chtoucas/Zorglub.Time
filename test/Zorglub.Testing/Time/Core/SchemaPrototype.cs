// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using Zorglub.Time.Core.Prototypes;

// We added this type to be able to test PrototypalSchema, particularly its
// virtual methods, otherwise this one is not very interesting. Indeed, if we
// already have an ICalendricalSchema, then why use an archetype?

public sealed class SchemaPrototype : PrototypalSchema
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SchemaPrototype"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
    public SchemaPrototype(ICalendricalSchema schema) : base(schema)
    {
        Schema = schema;
    }

    /// <summary>
    /// Gets the inner schema.
    /// </summary>
    internal ICalendricalSchema Schema { get; }

    /// <inheritdoc />
    public override int MinDaysInYear => Schema.MinDaysInYear;

    /// <inheritdoc />
    public override int MinDaysInMonth => Schema.MinDaysInMonth;

    // We do NOT override with Schema.SupportedYears.
    // See comments in PrototypalSchema.SupportedYears.

    /// <inheritdoc />
    public override ICalendricalPreValidator PreValidator => Schema.PreValidator;
}
