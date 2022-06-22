// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Arithmetic;

    // FIXME(code): move Subtract() elsewhere. Supprimer les Count...()
    // SupportedYears is wrong, see SystemArithmetic. Don't bother right now, as
    // we're going to merge CalendricalMath and CalendricalArithmetic.
    // On doit respecter les props schema.Min/MaxYear
    // mais aussi gérer le cas où MinYear < Yemoda.MinYear ou
    // MaxYear > Yemoda.MaxYear. Voir PlainArithmetic.
    // CountYearsBetween() overflow? Avec des années complètes, je ne pense pas.
    // Idem avec CountMonthsBetween(), etc.

    // Années complètes : on doit juste vérifier l'année.
    // Par contrat, à partir du moment où l'année est dans la plage
    // d'années supportée par un schéma, on sait que les méthodes ne
    // provoqueront pas de débordements arithmétiques.
    // Si les années n'étaient pas complètes, il faudrait prendre en
    // compte le cas des années limites (Min/MaxYear).

    // Un "calculateur" ne dépend que du "schéma" calendaire et du champs
    // d'application du calendrier.
    //
    // Pour des raisons techniques, la dépendance vis-à-vis du schéma n'est pas
    // visible ici ; voir p.ex. CustomCalculator. On aurait pu tout de même
    // rendre celle-ci explicite, mais comme ce n'est pas nécessaire, on s'en
    // passe très bien.
    //
    // Quant au champs d'application, celui étant fixe dans le cas présent
    // (calendriers proleptiques), on constate qu'on peut se contenter de
    // vérifier le champs année, ie on peut très bien se passer du "scope".
    //
    // Les méthods AddDays() et CountDaysBetween() ne sont pas incluses ici
    // mais dans Calendar. La raison en est qu'elles sont déjà largement
    // optimisées et qu'elles ne souffrent d'aucun choix arbitraire,
    // contrairement à AddMonths() p.ex. Notons aussi que AddDays() a besoin de
    // connaître Min/MaxDayNumber c-à-d le "scope" sous-jacent ; encore une
    // bonne raison de ne pas inclure cette méthode ici.

    public sealed class PowerMath : CalendarMath
    {
        private readonly ICalendricalArithmeticPlus _arithmetic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerMath"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        public PowerMath(Calendar calendar, AdditionRuleset additionRuleset)
            : base(calendar, additionRuleset)
        {
            Debug.Assert(calendar != null);

            _arithmetic = calendar.Arithmetic;
        }

        /// <inheritdoc/>
        [Pure]
        protected internal override CalendarDate AddYearsCore(CalendarDate date, int years)
        {
            Debug.Assert(date.Cuid == Cuid);

            var ymd = _arithmetic.AddYears(date.Parts, years, out int roundoff);
            if (roundoff > 0) { ymd = Adjust(ymd, roundoff); }

            YearOverflowChecker.Check(ymd.Year);

            return new CalendarDate(ymd, Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        protected internal override CalendarDate AddMonthsCore(CalendarDate date, int months)
        {
            Debug.Assert(date.Cuid == Cuid);

            var ymd = _arithmetic.AddMonths(date.Parts, months, out int roundoff);
            if (roundoff > 0) { ymd = Adjust(ymd, roundoff); }

            YearOverflowChecker.Check(ymd.Year);

            return new CalendarDate(ymd, Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        protected internal override OrdinalDate AddYearsCore(OrdinalDate date, int years)
        {
            Debug.Assert(date.Cuid == Cuid);

            var ydoy = _arithmetic.AddYears(date.Parts, years, out int roundoff);
            if (roundoff > 0) { ydoy = Adjust(ydoy, roundoff); }

            YearOverflowChecker.Check(ydoy.Year);

            return new OrdinalDate(ydoy, Cuid);
        }

        /// <inheritdoc/>
        [Pure]
        protected internal override CalendarMonth AddYearsCore(CalendarMonth month, int years)
        {
            Debug.Assert(month.Cuid == Cuid);

            var ym = _arithmetic.AddYears(month.Parts, years, out int roundoff);
            if (roundoff > 0) { ym = Adjust(ym, roundoff); }

            YearOverflowChecker.Check(ym.Year);

            return new CalendarMonth(ym, Cuid);
        }

        //
        // TODO(code): move elsewhere
        //

        /// <summary>
        /// Counts the number of years between the two specified dates.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        [Pure]
        public int CountYearsBetween(CalendarDate start, CalendarDate end, out CalendarDate newStart)
        {
            // The subtraction validates the Cuid.
            int years = end.CalendarYear - start.CalendarYear;
            newStart = AddYears(start, years);

            if (start < end)
            {
                if (newStart > end)
                {
                    years--;
                    newStart = AddYears(start, years);
                }
            }
            else
            {
                if (newStart < end)
                {
                    years++;
                    newStart = AddYears(start, years);
                }
            }

            return years;
        }

        /// <summary>
        /// Counts the number of months between the two specified dates.
        /// </summary>
        [Pure]
        public int CountMonthsBetween(CalendarDate start, CalendarDate end, out CalendarDate newStart)
        {
            // The subtraction validates the Cuid.
            int months = end.CalendarMonth - start.CalendarMonth;
            newStart = AddMonths(start, months);

            if (start < end)
            {
                if (newStart > end)
                {
                    months--;
                    newStart = AddMonths(start, months);
                }
            }
            else
            {
                if (newStart < end)
                {
                    months++;
                    newStart = AddMonths(start, months);
                }
            }

            return months;
        }

        /// <summary>
        /// Counts the number of years between the two specified ordinal dates.
        /// </summary>
        [Pure]
        public int CountYearsBetween(OrdinalDate start, OrdinalDate end, out OrdinalDate newStart)
        {
            // The subtraction validates the Cuid.
            int years = end.CalendarYear - start.CalendarYear;
            newStart = AddYears(start, years);

            if (start < end)
            {
                if (newStart > end)
                {
                    years--;
                    newStart = AddYears(start, years);
                }
            }
            else
            {
                if (newStart < end)
                {
                    years++;
                    newStart = AddYears(start, years);
                }
            }

            return years;
        }

        /// <summary>
        /// Counts the number of years between the two specified months.
        /// </summary>
        [Pure]
        public int CountYearsBetween(CalendarMonth start, CalendarMonth end, out CalendarMonth newStart)
        {
            // The subtraction validates the Cuid.
            int years = end.CalendarYear - start.CalendarYear;
            newStart = AddYears(start, years);

            if (start < end)
            {
                if (newStart > end)
                {
                    years--;
                    newStart = AddYears(start, years);
                }
            }
            else
            {
                if (newStart < end)
                {
                    years++;
                    newStart = AddYears(start, years);
                }
            }

            return years;
        }

        /// <summary>
        /// Calculates the exact difference between the two specified dates.
        /// </summary>
        [Pure]
        public (int Years, int Months, int Days) Subtract(CalendarDate left, CalendarDate right)
        {
            // Même si on utilise AddYears() et AddMonths() (qui retournent
            // parfois le jour la plus proche), le résultat final est exact car
            // on effectue les calculs de proche en proche.

            int years = CountYearsBetween(right, left, out CalendarDate newStart1);
            int months = CountMonthsBetween(right, newStart1, out CalendarDate newStart2);

            int days = left - newStart2;

            return (years, months, days);
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
