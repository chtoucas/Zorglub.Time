// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

// Why?????
#pragma warning disable CA1027 // Mark enums with FlagsAttribute (Design)

// REVIEW: add this and NewYearStyle to Calendar? use a record to hold both?
// Only informational? since a date object does not contain a time part...

namespace Zorglub.Time;

// Suivant le calendrier, le jour peut commencer à un instant différent :
// minuit, midi, lever ou coucher du soleil (Sunrise ou Sunset).
public enum NewDayStyle
{
    Default = 0,

    Sunrise,

    Noon,

    Sunset,

    Midnight = Default,
}
