// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    public sealed partial class RegularCalendricalMath : CalendricalMath
    {
        /// <summary>
        /// Represents the total number of months in a year.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _monthsInYear;

        public RegularCalendricalMath(ICalendricalSchema schema) : base(schema)
        {
            Debug.Assert(schema != null);
            if (schema.IsRegular(out int monthsInYear) == false) Throw.Argument(nameof(schema));

            _monthsInYear = monthsInYear;
        }
    }

    public partial class RegularCalendricalMath // Yemoda
    {
        /// <inheritdoc />
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
        [Pure]
        public override Yemoda AddMonths(Yemoda ymd, int months, out int roundoff)
        {
            ymd.Unpack(out int y, out int m, out int d);

            // On retranche 1 à "m" pour le rendre algébrique.
            m = 1 + MathZ.Modulo(checked(m - 1 + months), _monthsInYear, out int y0);
            y += y0;

            CheckYearOverflow(y);

            int daysInMonth = Schema.CountDaysInMonth(y, m);
            roundoff = Math.Max(0, d - daysInMonth);
            return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
        }
    }

    public partial class RegularCalendricalMath // Yemo
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
        public override int CountYearsBetween(Yemo start, Yemo end) =>
            CountYearsBetween(start.StartOfMonth, end.StartOfMonth, out _);
    }
}
