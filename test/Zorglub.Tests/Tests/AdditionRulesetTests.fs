// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.AdditionRulesetTests

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time

open Xunit

module Prelude =
    let additionRuleData = EnumDataSet.AdditionRuleData
    let invalidAdditionRuleData = EnumDataSet.InvalidAdditionRuleData

    [<Theory; MemberData(nameof(invalidAdditionRuleData))>]
    let ``Constructor throws for an invalid rule for dates`` (rule: AdditionRule) =
        outOfRangeExn "value" (fun () -> AdditionRuleset(DateRule = rule))

    [<Theory; MemberData(nameof(invalidAdditionRuleData))>]
    let ``Constructor throws for an invalid rule for ordinal dates`` (rule: AdditionRule) =
        outOfRangeExn "value" (fun () -> AdditionRuleset(OrdinalRule = rule))

    [<Theory; MemberData(nameof(invalidAdditionRuleData))>]
    let ``Constructor throws for an invalid rule for months`` (rule: AdditionRule) =
        outOfRangeExn "value" (fun () -> AdditionRuleset(MonthRule = rule))

    //
    // Properties
    //

    [<Theory; MemberData(nameof(additionRuleData))>]
    let ``Property DateRule`` (rule: AdditionRule) =
        let rules = AdditionRuleset(DateRule = rule)

        rules.DateRule === rule

    [<Theory; MemberData(nameof(additionRuleData))>]
    let ``Property OrdinalRule`` (rule: AdditionRule) =
        let rules = AdditionRuleset(OrdinalRule = rule)

        rules.OrdinalRule === rule

    [<Theory; MemberData(nameof(additionRuleData))>]
    let ``Property MonthRule`` (rule: AdditionRule) =
        let rules = AdditionRuleset(MonthRule = rule)

        rules.MonthRule === rule
