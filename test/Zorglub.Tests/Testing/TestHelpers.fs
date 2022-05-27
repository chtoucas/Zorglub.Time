// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

[<AutoOpen>]
module Zorglub.Testing.TestHelpers

open Zorglub.Time.Core

/// Creates a new instance of the schema of type 'a.
let schemaOf<'a when 'a :> ICalendricalSchema and 'a :> IBoxable<'a>> () =
    SchemaActivator.CreateInstance<'a>()
