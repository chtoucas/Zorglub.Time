// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple.Specialized
{
    // TODO(api): épacte, lettre dominicale, indiction, cycle solaire, etc.
    // Algorithmes alternatifs. Nom : RomanLiturgicalYear ?

    #region Developer Notes

    // Calendrier liturgique romain de l'Église catholique.
    // "General Roman Calendar".
    // Fêtes chrétiennes.
    //
    // Références :
    // - https://fr.wikipedia.org/wiki/Calendrier_chr%C3%A9tien
    // - https://fr.wikipedia.org/wiki/Calendrier_liturgique_romain
    // - https://en.wikipedia.org/wiki/General_Roman_Calendar
    // - [Computus](https://en.wikipedia.org/wiki/Computus)
    // - [Easter via doomsday](https://www.faqs.org/faqs/astronomy/faq/part3/index.html)
    // - [Calcul de la date de Pâques](https://fr.wikipedia.org/wiki/Calcul_de_la_date_de_P%C3%A2ques)
    // - https://aa.usno.navy.mil/faq/docs/easter.php
    // - https://stackoverflow.com/questions/2510383/how-can-i-calculate-what-date-good-friday-falls-on-given-a-year
    // - https://www.codeproject.com/Articles/10860/Calculating-Christian-Holidays

    #endregion

    public sealed partial class RomanKalendar
    {
        private readonly int _year;

        public RomanKalendar(int year)
        {
            if (year <= 0) Throw.ArgumentOutOfRange(nameof(year));

            _year = year;
        }
    }

    public partial class RomanKalendar // Fêtes fixes
    {
        // Pour l'Église catholique, l'Épiphanie est célébrée à une date fixe,
        // le 6 janvier, cependant afin de permettre aux fidèles de se rendre
        // à la messe, l'Épiphanie peut être fixée au dimanche inclus dans la
        // période du 2 au 8 janvier. C'est cette dernière qui est appliquée en
        // France où l'Épiphanie n'est pas un jour férié.
        //
        // Ce n'est pas la même chose que le dimanche le plus proche du 6 janvier.
        // En effet si le 6 janvier tombe un jeudi, le dimanche le plus proche est
        // le 9 janvier qui n'est pas dans la période du 2 au 8 janvier.

        // L'Épiphanie, le 6 janvier.
        public CalendarDate Epiphany => new(_year, 1, 6);

        // La Chandeleur, le 2 février.
        public CalendarDate Candlemas => new(_year, 2, 2);

        // L'Annonciation, le 25 mars.
        public CalendarDate Annunciation => new(_year, 3, 25);

        // L'Assomption de Marie, le 15 août.
        // La Dormition de Marie pour les orthodoxes.
        public CalendarDate AssumptionOfMary => new(_year, 8, 15);

        // L'Immaculée Conception, le 8 décembre.
        public CalendarDate ImmaculateConception => new(_year, 12, 8);

        // Noël, le 25 décembre.
        public CalendarDate Christmas => new(_year, 12, 25);
    }

    public partial class RomanKalendar // Fêtes mobiles
    {
        // Le 1er dimanche de janvier sauf si ce dimanche tombe le jour de l'an.
        public CalendarDate EpiphanySunday =>
            new CalendarDate(_year, 1, 1).Next(DayOfWeek.Sunday);

        // Premier dimanche de l'Avent, 1er jour de l'année liturgique romaine.
        // Dimanche le plus proche du 30 novembre (4ème dimanche avant Noël).
        // Premier jour de l'année liturgique, entre le 27 novembre et, au plus
        // tard, le 3 décembre.
        // "First Sunday of Advent" ou "First Advent Sunday".
        public CalendarDate AdventSunday =>
            new CalendarDate(_year, 11, 30).Nearest(DayOfWeek.Sunday);

        // Pâques catholique ou protestante.
        // Au plus tôt le 22 mars, au plus tard le 25 avril.
        private CalendarDate? _easter;
        public CalendarDate Easter => _easter ??= InitEaster(_year);

        // Mardi gras (Shrove Tuesday), précède le mercredi des cendres.
        // Pâques - 6 semaines - 5 jours -> mardi.
        public CalendarDate FatTuesday => Easter - 47;

        // Mercredi des cendres, 46 jours avant Pâques.
        // Pâques - 6 semaines - 4 jours -> mercredi.
        public CalendarDate AshWednesday => Easter - 46;

        // Dimanche des Rameaux, dernier dimanche avant Pâques.
        public CalendarDate PalmSunday => Easter - 7;

        // Jeudi saint, jeudi précédent Pâques.
        public CalendarDate MaundyThursday => Easter - 3;

        // Vendredi saint, vendredi précédent Pâques.
        // Pâques - 2 jours -> vendredi.
        public CalendarDate GoodFriday => Easter - 2;

        // Ascension, 39 jours après Pâques.
        // Pâques + 5 semaines + 4 jours -> jeudi.
        public CalendarDate AscensionThursday => Easter + 39;

        // Pentecôte, 49 jours après Pâques.
        // Pâques + 7 semaines -> dimanche.
        public CalendarDate Pentecost => Easter + 49;

        // Lune ecclésiastique ou lune pascale.
        private CalendarDate? _paschalMoon;
        public CalendarDate PaschalMoon => _paschalMoon ??= InitPaschalMoon(_year);
    }

    public partial class RomanKalendar // Comput ecclésiastique
    {
        // Pâques catholique ou protestante.
        // Au plus tôt le 22 mars, au plus tard le 25 avril.
        // D.&.R (8.3) p. 117, easter()

        [Pure]
        private static CalendarDate InitPaschalMoon(int year)
        {
            int epact = GetEpact(year);
            return new CalendarDate(year, 4, 19) - epact;
        }

        [Pure]
        private static CalendarDate InitEaster(int year)
        {
            var paschalMoon = InitPaschalMoon(year);
            return paschalMoon.Next(DayOfWeek.Sunday);
        }

        [Pure]
        private static int GetEpact(int year)
        {
            Debug.Assert(year >= 0);

            int century = 1 + (year / 100);

            // Épacte = nombre de jours entre la dernière nouvelle lune de
            // l'année précédente et le 1er janvier de l'année.
            int epact30 = 14 + 11 * (year % 19) - ((3 * century) >> 2) + (5 + 8 * century) / 25;
            int epact = epact30 % 30;

            // Épacte corrigée.
            if (epact == 0 || (epact == 1 && (year % 19) > 10))
            {
                epact++;
            }

            return epact;
        }
    }
}
