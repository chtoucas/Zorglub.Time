﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time;

// Calendars with the same ID SHOULD be functionally equivalent, i.e. they
// should use functionally equivalent schema and scope.

/// <summary>Specifies the permanent unique identifier of a calendar.
/// <para>This feature is only available to system calendars.</para></summary>
public enum CalendarId
{
    /// <summary>The permanent identifier of the proleptic Gregorian calendar.</summary>
    Gregorian = 0,

    /// <summary>The permanent identifier of the proleptic Julian calendar.</summary>
    Julian,

    /// <summary>The permanent identifier of the Civil calendar.</summary>
    Civil,

    /// <summary>The permanent identifier of the Armenian calendar.</summary>
    Armenian,

    /// <summary>The permanent identifier of the Coptic calendar.</summary>
    Coptic,

    /// <summary>The permanent identifier of the Ethiopic calendar.</summary>
    Ethiopic,

    /// <summary>The permanent identifier of the Tabular Islamic calendar.</summary>
    TabularIslamic,

    /// <summary>The permanent identifier of the Zoroastrian calendar.</summary>
    Zoroastrian,

    // WARNING: whenever we add an entry, we MUST update
    // - CalendarIdExtensions
    // - Cuid
    // - add XXXSimpleCalendar in SimpleCalendar.A..Z.cs
    // - add SimpleCalendar.XXX
    // - SimpleCatalog.InitializeSystemCalendars()
    // - add XXXZCalendar in ZCalendar.A..Z.cs
    // - add ZCalendar.XXX
    // - ZCatalog.InitCalendarsByKey()
    // and the following test classes:
    // - Zorglub.Testing.Data.EnumDataSet.CalendarIdData
    // - Zorglub.Testing.Data.EnumDataSet.FixedCuidData
    // - Zorglub.Tests.CalendarIdTests.IdToStringData
    // - tests for SimpleCalendar and ZCalendar
}

/// <summary>Provides extension methods for <see cref="CalendarId"/>.
/// <para>This class cannot be inherited.</para></summary>
internal static class CalendarIdExtensions
{
    /// <summary>Returns true if the specified ID is defined; otherwise returns false.</summary>
    public static bool IsDefined(this CalendarId @this) =>
        CalendarId.Gregorian <= @this && @this <= CalendarId.Zoroastrian;

    /// <summary>Converts the ID to a calendar key.</summary>
    /// <exception cref="AoorException" />
    public static string ToCalendarKey(this CalendarId @this) =>
        @this switch
        {
            CalendarId.Gregorian => "Gregorian",
            CalendarId.Julian => "Julian",
            CalendarId.Civil => "Civil",
            CalendarId.Armenian => "Armenian",
            CalendarId.Coptic => "Coptic",
            CalendarId.Ethiopic => "Ethiopic",
            CalendarId.TabularIslamic => "Tabular Islamic",
            CalendarId.Zoroastrian => "Zoroastrian",
            _ => Throw.ArgumentOutOfRange<string>(nameof(@this)),
        };
}
