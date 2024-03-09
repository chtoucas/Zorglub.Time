// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Core;

// We rely on the fact that a valid value for an enum is NOT limited by its
// members.
//
// C# 6.0 draft specification
// --------------------------
// Each enum type defines a distinct type; an explicit enumeration conversion
// (Explicit enumeration conversions) is required to convert between an enum
// type and an integral type, or between two enum types. The set of values
// that an enum type can take on is not limited by its enum members.
// In particular, any value of the underlying type of an enum can be cast to
// the enum type, and is a distinct valid value of that enum type.
//
// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/enums#186-enum-values-and-operations

/// <summary>Specifies the unique identifier of a calendar.
/// <para>An identifier may be transient.</para></summary>
internal enum Cuid : byte
{
    /// <summary>The identifier of the proleptic Gregorian calendar.</summary>
    Gregorian = CalendarId.Gregorian,

    /// <summary>The identifier of the proleptic Julian calendar.</summary>
    Julian = CalendarId.Julian,

    /// <summary>The identifier of the Civil calendar.</summary>
    Civil = CalendarId.Civil,

    /// <summary>The identifier of the Armenian calendar.</summary>
    Armenian = CalendarId.Armenian,

    /// <summary>The identifier of the Coptic calendar.</summary>
    Coptic = CalendarId.Coptic,

    /// <summary>The identifier of the Ethiopic calendar.</summary>
    Ethiopic = CalendarId.Ethiopic,

    /// <summary>The identifier of the Tabular Islamic calendar.</summary>
    TabularIslamic = CalendarId.TabularIslamic,

    /// <summary>The identifier of the Zoroastrian calendar.</summary>
    Zoroastrian = CalendarId.Zoroastrian,

    // WARNING: whenever we add an entry, we MUST update MaxSystem below.

    //
    // Sentinels and special values.
    //
    // Reserved IDs for system calendars: 0-63.
    // Available IDs for user-defined calendars: 64-127.
    // Unused or invalid IDs: 128-255.

    /// <summary>The maximum ID for a system calendar.</summary>
    MaxSystem = Zoroastrian,

    /// <summary>The minimum ID for a user-defined calendar.</summary>
    // NB: we could use MaxSystem + 1, but I prefer to be conservative.
    MinUser = 64,

    /// <summary>The absolute maximum ID for a calendar.</summary>
    Max = Yemodax.MaxExtra, // 127

    /// <summary>An invalid identifier.</summary>
    Invalid = Byte.MaxValue, // 255
}

/// <summary>Provides extension methods for <see cref="Cuid"/>.
/// <para>This class cannot be inherited.</para></summary>
internal static class CuidExtensions
{
    /// <summary>Returns true if the specified ID is fixed; otherwise returns false.
    /// <para>An ID is fixed iff it's the ID of a system calendar.</para></summary>
    [Pure]
    public static bool IsFixed(this Cuid @this) => @this <= Cuid.MaxSystem;
}
