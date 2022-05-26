// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

public sealed class FauxUserCalendar : Calendar
{
    public const string FauxKey = "FauxKey";
    internal const Cuid FauxCuid = Cuid.Invalid;

    public FauxUserCalendar()
        : base(FauxCuid, FauxKey, FauxSystemSchema.Default, default, default) { }

    internal FauxUserCalendar(Cuid id)
        : base(id, FauxKey, FauxSystemSchema.Default, default, default) { }

    public FauxUserCalendar(string key)
        : base(FauxCuid, key, FauxSystemSchema.Default, default, default) { }

    public FauxUserCalendar(SystemSchema schema)
        : base(FauxCuid, FauxKey, schema, default, default) { }

    public FauxUserCalendar(DayNumber epoch)
        : base(FauxCuid, FauxKey, FauxSystemSchema.Default, epoch, default) { }

    public FauxUserCalendar(bool proleptic)
        : base(FauxCuid, FauxKey, FauxSystemSchema.Default, default, proleptic) { }

    internal void ValidateCuidDisclosed(Cuid cuid, string paramName) =>
        ValidateCuid(cuid, paramName);
}
