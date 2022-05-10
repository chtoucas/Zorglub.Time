// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

public sealed class FauxSystemCalendar : Calendar
{
    private static readonly DayNumber s_DefaultEpoch = DayZero.NewStyle;

    // On ne peut pas utiliser un ID quelconque ici car la clé sera obtenue
    // en utilisant FauxCalendarId.ToCalendarKey().
    // De plus, ici, CalendarId doit avoir une valeur < 64.
    public const CalendarId DefaultId = CalendarId.Zoroastrian;

    public FauxSystemCalendar()
        : base(DefaultId, FauxSystemSchema.Default, s_DefaultEpoch, proleptic: false) { }

    public FauxSystemCalendar(CalendarId id)
        : base(id, FauxSystemSchema.Default, s_DefaultEpoch, proleptic: false) { }

    public FauxSystemCalendar(DayNumber epoch)
        : base(DefaultId, FauxSystemSchema.Default, epoch, proleptic: false) { }

    public FauxSystemCalendar(FauxSystemSchema schema)
        : base(DefaultId, schema, s_DefaultEpoch, proleptic: false) { }

    public FauxSystemCalendar(bool proleptic)
        : base(DefaultId, FauxSystemSchema.Default, s_DefaultEpoch, proleptic) { }

    internal void ValidateCuidDisclosed(Cuid cuid, string paramName) =>
        ValidateCuid(cuid, paramName);
}
