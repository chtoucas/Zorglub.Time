// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Arithmetic;

    public sealed class PowerMath : CalendarMath
    {
        private readonly ICalendricalArithmetic _arithmetic;
        private readonly CalendricalMath _math;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerMath"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        public PowerMath(Calendar calendar, AdditionRuleset additionRuleset)
            : base(calendar, additionRuleset)
        {
            Debug.Assert(calendar != null);

            _arithmetic = calendar.Arithmetic;
            _math = CalendricalMath.Create(calendar.Schema);
        }

        /// <inheritdoc/>
        [Pure]
        protected internal override CalendarDate AddYearsCore(CalendarDate date, int years)
        {
            Debug.Assert(date.Cuid == Cuid);

            var ymd = _math.AddYears(date.Parts, years, out int roundoff);
            if (roundoff > 0) { ymd = Adjust(ymd, roundoff); }

            YearOverflowChecker.Check(ymd.Year);

            return new CalendarDate(ymd, Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        protected internal override CalendarDate AddMonthsCore(CalendarDate date, int months)
        {
            Debug.Assert(date.Cuid == Cuid);

            var ymd = _math.AddMonths(date.Parts, months, out int roundoff);
            if (roundoff > 0) { ymd = Adjust(ymd, roundoff); }

            YearOverflowChecker.Check(ymd.Year);

            return new CalendarDate(ymd, Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        protected internal override OrdinalDate AddYearsCore(OrdinalDate date, int years)
        {
            Debug.Assert(date.Cuid == Cuid);

            var ydoy = _math.AddYears(date.Parts, years, out int roundoff);
            if (roundoff > 0) { ydoy = Adjust(ydoy, roundoff); }

            YearOverflowChecker.Check(ydoy.Year);

            return new OrdinalDate(ydoy, Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        protected internal override CalendarMonth AddYearsCore(CalendarMonth month, int years)
        {
            Debug.Assert(month.Cuid == Cuid);

            var ym = _math.AddYears(month.Parts, years, out int roundoff);
            if (roundoff > 0) { ym = Adjust(ym, roundoff); }

            YearOverflowChecker.Check(ym.Year);

            return new CalendarMonth(ym, Cuid);
        }

        //
        // Adjustments
        //
        // Résultat rectifié en fonction de la règle sélectionnée.

        [Pure]
        private Yemoda Adjust(Yemoda ymd, int roundoff)
        {
            // Si on ne filtrait pas roundoff > 0, il faudrait prendre en compte
            // le cas roundoff = 0 et retourner ymd (résultat exact).
            Debug.Assert(roundoff > 0);

            // NB: according to CalendricalMath, ymd is the last day of the month.
            return AdditionRuleset.DateRule switch
            {
                AdditionRule.Overspill => _arithmetic.AddDays(ymd, 1),
                AdditionRule.Exact => _arithmetic.AddDays(ymd, roundoff),
                AdditionRule.Truncate => ymd,

                _ => Throw.InvalidOperation<Yemoda>(),
            };
        }

        [Pure]
        private Yedoy Adjust(Yedoy ydoy, int roundoff)
        {
            // Si on ne filtrait pas roundoff > 0, il faudrait prendre en compte
            // le cas roundoff = 0 et retourner ydoy (résultat exact).
            Debug.Assert(roundoff > 0);

            // NB: according to CalendricalMath, ydoy is the last day of the year.
            return AdditionRuleset.OrdinalRule switch
            {
                AdditionRule.Overspill => _arithmetic.AddDays(ydoy, 1),
                AdditionRule.Exact => _arithmetic.AddDays(ydoy, roundoff),
                AdditionRule.Truncate => ydoy,

                _ => Throw.InvalidOperation<Yedoy>(),
            };
        }

        [Pure]
        private Yemo Adjust(Yemo ym, int roundoff)
        {
            // Si on ne filtrait pas roundoff > 0, il faudrait prendre en compte
            // le cas roundoff = 0 et retourner ym (résultat exact).
            Debug.Assert(roundoff > 0);

            // NB: according to CalendricalMath, ym is the last month of the year.
            return AdditionRuleset.MonthRule switch
            {
                AdditionRule.Overspill => _arithmetic.AddMonths(ym, 1),
                AdditionRule.Exact => _arithmetic.AddMonths(ym, roundoff),
                AdditionRule.Truncate => ym,

                _ => Throw.InvalidOperation<Yemo>(),
            };
        }
    }
}
