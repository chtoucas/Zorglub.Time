// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

[<AutoOpen>]
module Zorglub.Testing.TestHelpers

open System

open Zorglub.Time.Core

// TODO(code): use EnumDataSet.InvalidDayOfWeek.

/// Represents an invalid value for DayOfWeek.
let dayOfWeekBeforeSunday: DayOfWeek  = enum <| int(DayOfWeek.Sunday) - 1   // (DayOfWeek)(-1)

/// Represents an invalid value for DayOfWeek.
let dayOfWeekAfterSaturday: DayOfWeek = enum <| int(DayOfWeek.Saturday) + 1 // (DayOfWeek)7

/// Creates a new instance of the schema of type 'a.
let schemaOf<'a when 'a :> ICalendricalSchema and 'a :> IBoxable<'a>> () =
    SchemaActivator.CreateInstance<'a>()
