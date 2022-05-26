// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

public sealed class FauxSystemCalendar : Calendar
{
    // On ne peut pas utiliser un ID quelconque ici car la clé sera obtenue
    // en utilisant FauxCalendarId.ToCalendarKey().
    // Notons aussi que CalendarId doit avoir une valeur < 64.
    public const CalendarId DefaultIdent = CalendarId.Zoroastrian;

    public FauxSystemCalendar()
        : base(DefaultIdent, FauxSystemSchema.Default, default, default) { }

    public FauxSystemCalendar(CalendarId ident)
        : base(ident, FauxSystemSchema.Default, default, default) { }

    public FauxSystemCalendar(SystemSchema schema)
        : base(DefaultIdent, schema, default, default) { }

    public FauxSystemCalendar(DayNumber epoch)
        : base(DefaultIdent, FauxSystemSchema.Default, epoch, default) { }

    public FauxSystemCalendar(bool proleptic)
        : base(DefaultIdent, FauxSystemSchema.Default, default, proleptic) { }

    internal void ValidateCuidDisclosed(Cuid cuid, string paramName) =>
        ValidateCuid(cuid, paramName);
}
