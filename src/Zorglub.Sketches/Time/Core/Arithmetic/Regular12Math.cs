// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    public sealed partial class Regular12Math : CalendricalMath
    {
        /// <summary>
        /// Represents the total number of months in a year.
        /// <para>This field is a constant equal to 12.</para>
        /// </summary>
        private const int MonthsInYear = 12;

        public Regular12Math(ICalendricalSchema schema) : base(schema, default)
        {
            Debug.Assert(schema != null);

            if (schema.IsRegular(out int monthsInYear) == false)
            {
                Throw.Argument(nameof(schema));
            }
            if (monthsInYear != MonthsInYear)
            {
                Throw.Argument(nameof(schema));
            }
        }
    }

    // Operations on calendrical days.
    public partial class Regular12Math
    {
        /// <inheritdoc />
        /// <remarks>
        /// <para>When the result is not a valid date (roundoff > 0), returns
        /// the last day of the month.</para>
        /// </remarks>
        [Pure]
        public override Yemoda AddYears(Yemoda ymd, int years, out int roundoff)
        {
            ymd.Unpack(out int y, out int m, out int d);

            y = checked(y + years);

            // Années complètes : on doit juste vérifier l'année.
            // Par contrat, à partir du moment où l'année est dans la plage
            // d'années supportée par un schéma, on sait que les méthodes ne
            // provoqueront pas de débordements arithmétiques.
            CheckYearOverflow(y);

            // Nbre invariable de mois par an : (y, m) est une combinaison valide.
            // On vérifie qu'on ne dépasse pas le dernier jour du mois.
            int daysInMonth = Schema.CountDaysInMonth(y, m);
            roundoff = Math.Max(0, d - daysInMonth);
            // On retourne le dernier jour du mois si d > daysInMonth.
            // Si les années n'étaient pas complètes, il faudrait prendre en
            // compte le cas des années limites (Min/MaxYear).
            // Si on ignorait roundoff, on pourrait juste utiliser
            // Math.Min(d, daysInMonth).
            return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
        }

        /// <inheritdoc />
        /// <remarks>
        /// <para>When the result is not a valid date (roundoff > 0), returns
        /// the last day of the month.</para>
        /// </remarks>
        [Pure]
        public override Yemoda AddMonths(Yemoda ymd, int months, out int roundoff)
        {
            ymd.Unpack(out int y, out int m, out int d);

            // On retranche 1 à "m" pour le rendre algébrique.
            m = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsInYear, out int y0);
            y += y0;

            CheckYearOverflow(y);

            int daysInMonth = Schema.CountDaysInMonth(y, m);
            roundoff = Math.Max(0, d - daysInMonth);
            return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
        }

        /// <inheritdoc />
        [Pure]
        public override int CountYearsBetween(Yemoda start, Yemoda end, out Yemoda newStart)
        {
            // En première approximation.
            int years = end.Year - start.Year;
            // REVIEW: overflow? Avec des années complètes, je ne pense pas.
            // Idem avec CountMonthsBetween().
            newStart = AddYears(start, years, out _);

            // On vérifie qu'on n'est pas allé trop loin.
            if (start < end)
            {
                if (newStart > end)
                {
                    years--;
                    newStart = AddYears(start, years, out _);
                }
            }
            else
            {
                if (newStart < end)
                {
                    years++;
                    newStart = AddYears(start, years, out _);
                }
            }

            return years;
        }

        /// <inheritdoc />
        [Pure]
        public override int CountMonthsBetween(Yemoda start, Yemoda end, out Yemoda newStart)
        {
            start.Unpack(out int y0, out int m0);
            end.Unpack(out int y, out int m);

            // En première approximation.
            int months = (y - y0) * MonthsInYear + m - m0;
            newStart = AddMonths(start, months, out _);

            // On vérifie qu'on n'est pas allé trop loin.
            if (start < end)
            {
                if (newStart > end)
                {
                    months--;
                    newStart = AddMonths(start, months, out _);
                }
            }
            else
            {
                if (newStart < end)
                {
                    months++;
                    newStart = AddMonths(start, months, out _);
                }
            }

            return months;
        }

        /// <inheritdoc />
        [Pure]
        public override (int years, int months, int days) Subtract(Yemoda left, Yemoda right)
        {
            // Même si on utilise AddYears() et AddMonths() (qui retournent
            // parfois le jour la plus proche), le résultat final est exact car
            // on effectue les calculs de proche en proche.

            int years = CountYearsBetween(right, left, out Yemoda newStart1);
            int months = CountMonthsBetween(right, newStart1, out Yemoda newStart2);

            int days = CountDaysBetween(newStart2, left);

            return (years, months, days);

            // TODO: we should be able to re-use ICalendricalArithmetic.CountDaysBetween().
            [Pure]
            int CountDaysBetween(Yemoda start, Yemoda end)
            {
                if (end.Yemo == start.Yemo) { return end.Day - start.Day; }

                start.Unpack(out int y0, out int m0, out int d0);
                end.Unpack(out int y1, out int m1, out int d1);

                return Schema.CountDaysSinceEpoch(y1, m1, d1) - Schema.CountDaysSinceEpoch(y0, m0, d0);
            }
        }
    }

    // Operations on calendrical months.
    public partial class Regular12Math
    {
        /// <inheritdoc />
        [Pure]
        public override Yemo AddYears(Yemo ym, int years)
        {
            ym.Unpack(out int y, out int m);

            y = checked(y + years);

            CheckYearOverflow(y);

            // Nbre invariable de mois par an : (y, m) est une combinaison valide.
            return new Yemo(y, m);
        }

        /// <inheritdoc />
        [Pure]
        public override Yemo AddMonths(Yemo ym, int months)
        {
            ym.Unpack(out int y, out int m);

            m = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsInYear, out int y0);
            y += y0;

            CheckYearOverflow(y);

            // Nbre invariable de mois par an : (y, m) est une combinaison valide.
            return new Yemo(y, m);
        }

        /// <inheritdoc />
        [Pure]
        public override int CountYearsBetween(Yemo start, Yemo end) =>
            CountYearsBetween(start.StartOfMonth, end.StartOfMonth, out _);

        /// <inheritdoc />
        [Pure]
        public override int CountMonthsBetween(Yemo start, Yemo end)
        {
            start.Unpack(out int y0, out int m0);
            end.Unpack(out int y, out int m);

            return (y - y0) * MonthsInYear + m - m0;
        }
    }

    // Operations on calendrical years.
    public partial class Regular12Math
    {
        [Pure]
        public override int AddYears(int y, int years)
        {
            y = checked(y + years);

            CheckYearOverflow(y);

            return y;
        }
    }
}
