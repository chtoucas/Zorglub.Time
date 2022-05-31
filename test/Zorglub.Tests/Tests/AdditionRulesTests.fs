// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.AdditionRulesTests

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time

open Xunit

module Prelude =
    let dateAdditionRuleData = EnumDataSet.DateAdditionRuleData
    let ordinalAdditionRuleData = EnumDataSet.OrdinalAdditionRuleData
    let monthAdditionRuleData = EnumDataSet.MonthAdditionRuleData

    let invalidDateAdditionRuleData = EnumDataSet.InvalidDateAdditionRuleData
    let invalidOrdinalAdditionRuleData = EnumDataSet.InvalidOrdinalAdditionRuleData
    let invalidMonthAdditionRuleData = EnumDataSet.InvalidMonthAdditionRuleData

    [<Theory; MemberData(nameof(invalidDateAdditionRuleData))>]
    let ``Constructor throws for invalid DateAdditionRule`` (rule: DateAdditionRule) =
        outOfRangeExn "dateRule" (fun () -> new AdditionRules(rule, OrdinalAdditionRule.EndOfYear, MonthAdditionRule.EndOfYear))

    [<Theory; MemberData(nameof(invalidOrdinalAdditionRuleData))>]
    let ``Constructor throws for invalid OrdinalAdditionRule`` (rule: OrdinalAdditionRule) =
        outOfRangeExn "ordinalRule" (fun () -> new AdditionRules(DateAdditionRule.EndOfMonth, rule, MonthAdditionRule.EndOfYear))

    [<Theory; MemberData(nameof(invalidMonthAdditionRuleData))>]
    let ``Constructor throws for invalid MonthAdditionRule`` (rule: MonthAdditionRule) =
        outOfRangeExn "monthRule" (fun () -> new AdditionRules(DateAdditionRule.EndOfMonth, OrdinalAdditionRule.EndOfYear, rule))

    //
    // Properties
    //

    [<Theory; MemberData(nameof(dateAdditionRuleData))>]
    let ``Property DateRule`` (rule: DateAdditionRule) =
        let rules = new AdditionRules(rule, OrdinalAdditionRule.EndOfYear, MonthAdditionRule.EndOfYear)

        rules.DateRule === rule

    [<Theory; MemberData(nameof(dateAdditionRuleData))>]
    let ``Property OrdinalRule`` (rule: OrdinalAdditionRule) =
        let rules = new AdditionRules(DateAdditionRule.EndOfMonth, rule, MonthAdditionRule.EndOfYear)

        rules.OrdinalRule === rule

    [<Theory; MemberData(nameof(dateAdditionRuleData))>]
    let ``Property MonthRule`` (rule: MonthAdditionRule) =
        let rules = new AdditionRules(DateAdditionRule.EndOfMonth, OrdinalAdditionRule.EndOfYear, rule)

        rules.MonthRule === rule
