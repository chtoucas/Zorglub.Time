// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Simple;

public sealed class FauxUserCalendar : SimpleCalendar
{
    public const string DefaultKey = "FauxKey";
    internal const Cuid DefaultCuid = Cuid.Invalid;

    public FauxUserCalendar()
        : base(DefaultCuid, DefaultKey, new FauxSystemSchema(), default, default) { }

    // Base constructors.
    internal FauxUserCalendar(Cuid id, string key, SystemSchema schema, DayNumber epoch, bool proleptic)
        : base(id, key, schema, epoch, proleptic) { }

    // Constructors in order to test the base constructors.
    internal FauxUserCalendar(Cuid id)
        : base(id, DefaultKey, new FauxSystemSchema(), default, default) { }
    public FauxUserCalendar(string key)
        : base(DefaultCuid, key, new FauxSystemSchema(), default, default) { }
    public FauxUserCalendar(SystemSchema schema)
        : base(DefaultCuid, DefaultKey, schema, default, default) { }
    public FauxUserCalendar(DayNumber epoch)
        : base(DefaultCuid, DefaultKey, new FauxSystemSchema(), epoch, default) { }
    public FauxUserCalendar(bool proleptic)
        : base(DefaultCuid, DefaultKey, new FauxSystemSchema(), default, proleptic) { }

    internal void ValidateCuidDisclosed(Cuid cuid, string paramName) => ValidateCuid(cuid, paramName);
}
