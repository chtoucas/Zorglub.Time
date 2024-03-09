// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Time.FSharpExtensions

open Zorglub.Time.Simple

/// Converts the specified permanent ID, a CalendarId value, to a calendar key.
let inline toCalendarKey ident = CalendarIdExtensions.ToCalendarKey(ident)

/// Converts the specified ident, a CalendarId value, to a Cuid value.
let inline internal toCuid (ident: CalendarId) =
    let cuid: Cuid = LanguagePrimitives.EnumOfValue <| byte(ident)
    cuid

/// Checks whether the specified Cuid is fixed or not.
let inline internal isFixed id = CuidExtensions.IsFixed(id)
