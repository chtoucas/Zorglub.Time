// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.AdditionRulesetTests

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Time

open Xunit

let private defaultRule = Unchecked.defaultof<AdditionRule>

let additionRuleData = EnumDataSet.AdditionRuleData
let badAdditionRuleData = EnumDataSet.InvalidAdditionRuleData

[<Fact>]
let ``Default value of AdditionRule is AdditionRule.Truncate`` () =
    defaultRule === AdditionRule.Truncate

[<Theory; MemberData(nameof(badAdditionRuleData))>]
let ``Constructor throws for an invalid rule for dates`` rule =
    outOfRangeExn "value" (fun () -> AdditionRuleset(DateRule = rule))

[<Theory; MemberData(nameof(badAdditionRuleData))>]
let ``Constructor throws for an invalid rule for ordinal dates`` rule =
    outOfRangeExn "value" (fun () -> AdditionRuleset(OrdinalRule = rule))

[<Theory; MemberData(nameof(badAdditionRuleData))>]
let ``Constructor throws for an invalid rule for months`` rule =
    outOfRangeExn "value" (fun () -> AdditionRuleset(MonthRule = rule))

[<Theory; MemberData(nameof(additionRuleData))>]
let ``Property DateRule`` rule =
    let ruleset = AdditionRuleset(DateRule = rule)

    ruleset.DateRule    === rule
    ruleset.OrdinalRule === defaultRule
    ruleset.MonthRule   === defaultRule

[<Theory; MemberData(nameof(additionRuleData))>]
let ``Property OrdinalRule`` rule =
    let ruleset = AdditionRuleset(OrdinalRule = rule)

    ruleset.DateRule    === defaultRule
    ruleset.OrdinalRule === rule
    ruleset.MonthRule   === defaultRule

[<Theory; MemberData(nameof(additionRuleData))>]
let ``Property MonthRule`` rule =
    let ruleset = AdditionRuleset(MonthRule = rule)

    ruleset.DateRule    === defaultRule
    ruleset.OrdinalRule === defaultRule
    ruleset.MonthRule   === rule
