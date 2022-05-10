// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

// We added this type to be able to test ArchetypalSchema, particularly its
// virtual methods, otherwise this one is not very interesting. Indeed, if we
// already have an ICalendricalSchema, then why use an archetype?

public sealed class SchemaArchetype : ArchetypalSchema
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SchemaArchetype"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.
    /// </exception>
    public SchemaArchetype(ICalendricalSchema schema) : base(schema)
    {
        Schema = schema;
    }

    /// <summary>
    /// Obtains the schema.
    /// </summary>
    internal ICalendricalSchema Schema { get; }

    /// <inheritdoc />
    public override int MinDaysInYear => Schema.MinDaysInYear;
    /// <inheritdoc />
    public override int MinDaysInMonth => Schema.MinDaysInMonth;

    // We do NOT override with Schema.SupportedYears.
    // See comments in ArchetypalSchema.SupportedYears.

    /// <inheritdoc />
    public override ICalendricalPreValidator PreValidator => Schema.PreValidator;
    /// <inheritdoc />
    public override ICalendricalArithmetic Arithmetic => Schema.Arithmetic;
}
