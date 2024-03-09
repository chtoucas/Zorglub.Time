// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

[<AutoOpen>]
module Zorglub.Testing.TestHelpers

open Zorglub.Time.Core
open Zorglub.Time.Core.Utilities

/// Creates a new instance of the schema of type 'a.
let schemaOf<'a when 'a :> ICalendricalSchema and 'a :> IBoxable<'a>> () =
    SchemaActivator.CreateInstance<'a>()

/// Creates a new instance of the schema of type 'a.
let syschemaOf<'a when 'a :> SystemSchema and 'a :> IBoxable<'a>> () =
    SchemaActivator.CreateInstance<'a>()

/// Creates a new instance of the schema of type 'a.
let sysegmentOf<'a when 'a :> SystemSchema and 'a :> IBoxable<'a>> () =
    let sch = SchemaActivator.CreateInstance<'a>()
    SystemSegment.Create(sch, sch.SupportedYears)
