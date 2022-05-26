// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Time.FSharpExtensions

open Zorglub.Time.Simple

let inline toCalendarKey id = CalendarIdExtensions.ToCalendarKey(id)

let inline isFixed id = CuidExtensions.IsFixed(id)
