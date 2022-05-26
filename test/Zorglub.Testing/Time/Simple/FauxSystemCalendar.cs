// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

public sealed class FauxSystemCalendar : Calendar
{
    // On ne peut pas utiliser un ID quelconque ici car la clé sera obtenue
    // en utilisant FauxCalendarId.ToCalendarKey().
    // De plus, ici, CalendarId doit avoir une valeur < 64.
    public const CalendarId FauxIdent = CalendarId.Zoroastrian;

    public FauxSystemCalendar()
        : base(FauxIdent, FauxSystemSchema.Default, default, default) { }

    public FauxSystemCalendar(CalendarId ident)
        : base(ident, FauxSystemSchema.Default, default, default) { }

    public FauxSystemCalendar(SystemSchema schema)
        : base(FauxIdent, schema, default, default) { }

    public FauxSystemCalendar(DayNumber epoch)
        : base(FauxIdent, FauxSystemSchema.Default, epoch, default) { }

    public FauxSystemCalendar(bool proleptic)
        : base(FauxIdent, FauxSystemSchema.Default, default, proleptic) { }

    internal void ValidateCuidDisclosed(Cuid cuid, string paramName) =>
        ValidateCuid(cuid, paramName);
}
