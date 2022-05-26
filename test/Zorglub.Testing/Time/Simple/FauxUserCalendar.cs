// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

public sealed class FauxUserCalendar : Calendar
{
    private const string DefaultKey = "FauxKey";
    internal const Cuid DefaultCuid = Cuid.Invalid;

    public FauxUserCalendar()
        : base(DefaultCuid, DefaultKey, FauxSystemSchema.Default, default, default) { }

    internal FauxUserCalendar(Cuid id)
        : base(id, DefaultKey, FauxSystemSchema.Default, default, default) { }

    public FauxUserCalendar(string key)
        : base(DefaultCuid, key, FauxSystemSchema.Default, default, default) { }

    public FauxUserCalendar(SystemSchema schema)
        : base(DefaultCuid, DefaultKey, schema, default, default) { }

    public FauxUserCalendar(DayNumber epoch)
        : base(DefaultCuid, DefaultKey, FauxSystemSchema.Default, epoch, default) { }

    public FauxUserCalendar(bool proleptic)
        : base(DefaultCuid, DefaultKey, FauxSystemSchema.Default, default, proleptic) { }

    internal void ValidateCuidDisclosed(Cuid cuid, string paramName) =>
        ValidateCuid(cuid, paramName);
}
