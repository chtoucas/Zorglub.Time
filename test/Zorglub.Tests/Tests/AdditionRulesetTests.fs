// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.AdditionRulesetTests

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time

open Xunit

module Prelude =
    let private defaultDateRule = Unchecked.defaultof<AdditionRule>

    let additionRuleData = EnumDataSet.AdditionRuleData
    let invalidAdditionRuleData = EnumDataSet.InvalidAdditionRuleData

    [<Theory; MemberData(nameof(invalidAdditionRuleData))>]
    let ``Constructor throws for an invalid rule for dates`` (rule: AdditionRule) =
        outOfRangeExn "dateRule" (fun () -> new AdditionRuleset(rule, defaultDateRule, defaultDateRule))

    [<Theory; MemberData(nameof(invalidAdditionRuleData))>]
    let ``Constructor throws for an invalid rule for ordinal dates`` (rule: AdditionRule) =
        outOfRangeExn "ordinalRule" (fun () -> new AdditionRuleset(defaultDateRule, rule, defaultDateRule))

    [<Theory; MemberData(nameof(invalidAdditionRuleData))>]
    let ``Constructor throws for an invalid rule for months`` (rule: AdditionRule) =
        outOfRangeExn "monthRule" (fun () -> new AdditionRuleset(defaultDateRule, defaultDateRule, rule))

    //
    // Properties
    //

    [<Theory; MemberData(nameof(additionRuleData))>]
    let ``Property DateRule`` (rule: AdditionRule) =
        let rules = new AdditionRuleset(rule, defaultDateRule, defaultDateRule)

        rules.DateRule === rule

    [<Theory; MemberData(nameof(additionRuleData))>]
    let ``Property OrdinalRule`` (rule: AdditionRule) =
        let rules = new AdditionRuleset(defaultDateRule, rule, defaultDateRule)

        rules.OrdinalRule === rule

    [<Theory; MemberData(nameof(additionRuleData))>]
    let ``Property MonthRule`` (rule: AdditionRule) =
        let rules = new AdditionRuleset(defaultDateRule, defaultDateRule, rule)

        rules.MonthRule === rule
