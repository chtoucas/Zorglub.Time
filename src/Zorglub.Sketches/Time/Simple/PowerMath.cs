// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Arithmetic;

    // TODO: custom factory for CalendricalMath.
    // WARNING: Math use a shorter range of years... (Yemoda vs Yemodax)

    // Not sealed, extendable.
    public class PowerMath : CalendarMath
    {
        public PowerMath(Calendar calendar) : base(calendar)
        {
            Debug.Assert(calendar != null);

            DefaultRules = calendar.Math;
            Math = Create(calendar.Schema);
        }

        protected CalendarMath DefaultRules { get; }

        protected CalendricalMath Math { get; }

        public override AddAdjustment AddAdjustment => Math.AddAdjustment;

        // REVIEW: move this method to CalendricalSchema?
        [Pure]
        private static CalendricalMath Create(SystemSchema schema)
        {
            Debug.Assert(schema != null);

            int monthsInYear = schema.IsRegular(out int v) ? v : 0;

            return monthsInYear switch
            {
                12 => new Core.Arithmetic.Regular12Math(schema),
                _ => throw new NotImplementedException()
            };
        }

        // Version pour les calendriers proleptiques.
        [Pure]
        protected static CalendarDate AdjustResult(
            CalendarDate result, int roundoff, AddAdjustment adjustment, Calendar calendar)
        {
            Debug.Assert(roundoff > 0);
            Debug.Assert(calendar != null);

            return adjustment switch
            {
                AddAdjustment.StartOfNextMonth => result.PlusDays(1),
                AddAdjustment.Exact => result.PlusDays(roundoff),
                AddAdjustment.EndOfMonth => result,

                _ => Throw.ArgumentOutOfRange<CalendarDate>(nameof(adjustment)),
            };
        }

        //
        // Operations on CalendarDate
        //

        [Pure]
        protected internal override CalendarDate AddYearsCore(CalendarDate date, int years) =>
            DefaultRules.AddYearsCore(date, years);

        [Pure]
        protected internal override CalendarDate AddMonthsCore(CalendarDate date, int months) =>
            DefaultRules.AddMonthsCore(date, months);

        [Pure]
        protected internal override int CountYearsBetweenCore(CalendarDate start, CalendarDate end) =>
            DefaultRules.CountYearsBetweenCore(start, end);

        [Pure]
        protected internal override int CountMonthsBetweenCore(CalendarDate start, CalendarDate end) =>
            DefaultRules.CountMonthsBetweenCore(start, end);

        //
        // Operations on OrdinalDate
        //

        /// <inheritdoc />
        [Pure]
        protected internal override OrdinalDate AddYearsCore(OrdinalDate date, int years) =>
            DefaultRules.AddYearsCore(date, years);

        /// <inheritdoc />
        [Pure]
        protected internal override int CountYearsBetweenCore(OrdinalDate start, OrdinalDate end) =>
            DefaultRules.CountYearsBetweenCore(start, end);

        //
        // Operations on CalendarMonth
        //

        [Pure]
        protected internal override CalendarMonth AddYearsCore(CalendarMonth month, int years) =>
            DefaultRules.AddYearsCore(month, years);

        [Pure]
        protected internal override CalendarMonth AddMonthsCore(CalendarMonth month, int months) =>
            DefaultRules.AddMonthsCore(month, months);

        [Pure]
        protected internal override int CountYearsBetweenCore(CalendarMonth start, CalendarMonth end) =>
            DefaultRules.CountYearsBetweenCore(start, end);

        [Pure]
        protected internal override int CountMonthsBetweenCore(CalendarMonth start, CalendarMonth end) =>
            DefaultRules.CountMonthsBetweenCore(start, end);
    }
}
