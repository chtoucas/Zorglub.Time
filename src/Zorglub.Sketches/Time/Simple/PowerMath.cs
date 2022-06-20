// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core.Arithmetic;

    // WARNING: Math use a shorter range of years... (Yemoda vs Yemodax)

    public class PowerMath : CalendarMath
    {
        public PowerMath(Calendar calendar, AdditionRules additionRules) : base(calendar, additionRules)
        {
            Debug.Assert(calendar != null);

            DefaultMath = calendar.Math;
            CalendricalMath = CalendricalMath.Create(calendar.Schema);
        }

        protected CalendarMath DefaultMath { get; }

        protected CalendricalMath CalendricalMath { get; }

        // Version pour les calendriers proleptiques.
        [Pure]
        protected static CalendarDate AdjustResult(
            CalendarDate result, int roundoff, DateAdditionRule rule, Calendar calendar)
        {
            Debug.Assert(roundoff > 0);
            Debug.Assert(calendar != null);

            return rule switch
            {
                DateAdditionRule.StartOfNextMonth => result.PlusDays(1),
                DateAdditionRule.Exact => result.PlusDays(roundoff),
                DateAdditionRule.EndOfMonth => result,

                _ => Throw.ArgumentOutOfRange<CalendarDate>(nameof(rule)),
            };
        }

        [Pure]
        protected internal override CalendarDate AddYearsCore(CalendarDate date, int years) =>
            DefaultMath.AddYearsCore(date, years);

        [Pure]
        protected internal override CalendarDate AddMonthsCore(CalendarDate date, int months) =>
            DefaultMath.AddMonthsCore(date, months);

        [Pure]
        protected internal override OrdinalDate AddYearsCore(OrdinalDate date, int years) =>
            DefaultMath.AddYearsCore(date, years);

        [Pure]
        protected internal override CalendarMonth AddYearsCore(CalendarMonth month, int years) =>
            DefaultMath.AddYearsCore(month, years);
    }
}
