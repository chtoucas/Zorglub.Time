// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.AdditionRulesTests

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time

open Xunit

module Prelude =
    let private defaultDateRule    = Unchecked.defaultof<DateAdditionRule>
    let private defaultOrdinalRule = Unchecked.defaultof<OrdinalAdditionRule>
    let private defaultMonthRule   = Unchecked.defaultof<MonthAdditionRule>

    let dateAdditionRuleData = EnumDataSet.DateAdditionRuleData
    let ordinalAdditionRuleData = EnumDataSet.OrdinalAdditionRuleData
    let monthAdditionRuleData = EnumDataSet.MonthAdditionRuleData

    let invalidDateAdditionRuleData = EnumDataSet.InvalidDateAdditionRuleData
    let invalidOrdinalAdditionRuleData = EnumDataSet.InvalidOrdinalAdditionRuleData
    let invalidMonthAdditionRuleData = EnumDataSet.InvalidMonthAdditionRuleData

    [<Theory; MemberData(nameof(invalidDateAdditionRuleData))>]
    let ``Constructor throws for invalid DateAdditionRule`` (rule: DateAdditionRule) =
        outOfRangeExn "dateRule" (fun () -> new AdditionRules(rule, defaultOrdinalRule, defaultMonthRule))

    [<Theory; MemberData(nameof(invalidOrdinalAdditionRuleData))>]
    let ``Constructor throws for invalid OrdinalAdditionRule`` (rule: OrdinalAdditionRule) =
        outOfRangeExn "ordinalRule" (fun () -> new AdditionRules(defaultDateRule, rule, defaultMonthRule))

    [<Theory; MemberData(nameof(invalidMonthAdditionRuleData))>]
    let ``Constructor throws for invalid MonthAdditionRule`` (rule: MonthAdditionRule) =
        outOfRangeExn "monthRule" (fun () -> new AdditionRules(defaultDateRule, defaultOrdinalRule, rule))

    //
    // Properties
    //

    [<Theory; MemberData(nameof(dateAdditionRuleData))>]
    let ``Property DateRule`` (rule: DateAdditionRule) =
        let rules = new AdditionRules(rule, defaultOrdinalRule, defaultMonthRule)

        rules.DateRule === rule

    [<Theory; MemberData(nameof(dateAdditionRuleData))>]
    let ``Property OrdinalRule`` (rule: OrdinalAdditionRule) =
        let rules = new AdditionRules(defaultDateRule, rule, defaultMonthRule)

        rules.OrdinalRule === rule

    [<Theory; MemberData(nameof(dateAdditionRuleData))>]
    let ``Property MonthRule`` (rule: MonthAdditionRule) =
        let rules = new AdditionRules(defaultDateRule, defaultOrdinalRule, rule)

        rules.MonthRule === rule
