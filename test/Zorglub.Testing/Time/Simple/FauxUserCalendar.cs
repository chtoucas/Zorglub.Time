// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

public sealed class FauxUserCalendar : Calendar
{
    public const string FauxKey = "FauxKey";
    internal const Cuid FauxId = Cuid.Invalid;

    public FauxUserCalendar()
        : base(FauxId, FauxKey, FauxSystemSchema.Default, default, default) { }

    internal FauxUserCalendar(Cuid id)
        : base(id, FauxKey, FauxSystemSchema.Default, default, default) { }

    public FauxUserCalendar(string key)
        : base(FauxId, key, FauxSystemSchema.Default, default, default) { }

    public FauxUserCalendar(SystemSchema schema)
        : base(FauxId, FauxKey, schema, default, default) { }

    public FauxUserCalendar(DayNumber epoch)
        : base(FauxId, FauxKey, FauxSystemSchema.Default, epoch, default) { }

    public FauxUserCalendar(bool proleptic)
        : base(FauxId, FauxKey, FauxSystemSchema.Default, default, proleptic) { }

    internal void ValidateCuidDisclosed(Cuid cuid, string paramName) =>
        ValidateCuid(cuid, paramName);
}
