// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

public sealed class FauxUserCalendar : Calendar
{
    private const string DefaultKey = "FauxKey";
    // Fake ID.
    internal const Cuid DefaultId = Cuid.Invalid;

    private static readonly DayNumber s_DefaultEpoch = DayZero.NewStyle;

    public FauxUserCalendar()
        : base(DefaultId, DefaultKey, FauxSystemSchema.Default, s_DefaultEpoch, proleptic: false) { }

    internal FauxUserCalendar(Cuid cuid)
        : base(cuid, DefaultKey, FauxSystemSchema.Default, s_DefaultEpoch, proleptic: false) { }

    public FauxUserCalendar(string key)
        : base(DefaultId, key, FauxSystemSchema.Default, s_DefaultEpoch, proleptic: false) { }

    public FauxUserCalendar(DayNumber epoch)
        : base(DefaultId, DefaultKey, FauxSystemSchema.Default, epoch, proleptic: false) { }

    public FauxUserCalendar(FauxSystemSchema schema)
        : base(DefaultId, DefaultKey, schema, s_DefaultEpoch, proleptic: false) { }

    public FauxUserCalendar(bool proleptic)
        : base(DefaultId, DefaultKey, FauxSystemSchema.Default, s_DefaultEpoch, proleptic) { }

    internal void ValidateCuidDisclosed(Cuid cuid, string paramName) =>
        ValidateCuid(cuid, paramName);
}
