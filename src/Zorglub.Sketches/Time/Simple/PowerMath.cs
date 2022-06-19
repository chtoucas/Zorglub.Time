// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core.Arithmetic;

    // TODO: custom factory for CalendricalMath.
    // WARNING: Math use a shorter range of years... (Yemoda vs Yemodax)

    // Not sealed, extendable.
    public class PowerMath : CalendarMath
    {
        public PowerMath(Calendar calendar) : this(calendar, CreateCalendricalMath(calendar)) { }

        private PowerMath(Calendar calendar, CalendricalMath math) : base(calendar, math.AdditionRules)
        {
            Debug.Assert(calendar != null);
            Debug.Assert(math != null);

            DefaultMath = calendar.Math;
            Math = math;
        }

        protected CalendarMath DefaultMath { get; }

        protected CalendricalMath Math { get; }

        // REVIEW(code): move this method to CalendricalSchema?
        [Pure]
        private static CalendricalMath CreateCalendricalMath(Calendar calendar)
        {
            Requires.NotNull(calendar);

            var schema = calendar.Schema;

            return schema.IsRegular(out _) ? new RegularCalendricalMath(schema)
                : throw new NotImplementedException();
        }

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

        //
        // Operations on CalendarDate
        //

        [Pure]
        protected internal override CalendarDate AddYearsCore(CalendarDate date, int years) =>
            DefaultMath.AddYearsCore(date, years);

        [Pure]
        protected internal override CalendarDate AddMonthsCore(CalendarDate date, int months) =>
            DefaultMath.AddMonthsCore(date, months);

        [Pure]
        protected internal override int CountMonthsBetweenCore(CalendarDate start, CalendarDate end) =>
            DefaultMath.CountMonthsBetweenCore(start, end);

        //
        // Operations on OrdinalDate
        //

        [Pure]
        protected internal override OrdinalDate AddYearsCore(OrdinalDate date, int years) =>
            DefaultMath.AddYearsCore(date, years);

        //
        // Operations on CalendarMonth
        //

        [Pure]
        protected internal override CalendarMonth AddYearsCore(CalendarMonth month, int years) =>
            DefaultMath.AddYearsCore(month, years);
    }
}
