// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extras.Simple
{
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Simple;

    // https://en.wikipedia.org/wiki/Gregorian_calendar
    // https://en.wikipedia.org/wiki/Adoption_of_the_Gregorian_calendar
    // https://en.wikipedia.org/wiki/List_of_adoption_dates_of_the_Gregorian_calendar_by_country
    // https://en.wikipedia.org/wiki/Calendar_(New_Style)_Act_1750
    // https://norbyhus.dk/calendar.php

    public sealed record GregorianReform
    {
        // Introduction officielle de la réforme grégorienne.
        public static readonly GregorianReform Official = new();

        // La réforme grégorienne entraîne une rupture dans la suite des jours
        // ou des mois. Le passage officiel du calendrier julien au calendrier
        // grégorien se fait le lendemain du jeudi 4 octobre 1582 (julien), on
        // est alors le vendredi 15 octobre 1582 (grégorien), c-à-d le vendredi
        // 5 octobre 1582 (julien).
        private GregorianReform()
            : this(
                SimpleCalendar.Julian.GetCalendarDate(1582, 10, 4),
                new CalendarDate(1582, 10, 15),
                null)
        { }

        private GregorianReform(
            CalendarDate lastJulianDate,
            CalendarDate firstGregorianDate,
            DayNumber? switchover)
        {
            LastJulianDate = lastJulianDate;
            FirstGregorianDate = firstGregorianDate;
            Switchover = switchover ?? FirstGregorianDate.ToDayNumber();
            SecularShift = InitSecularShift();
        }

        /// <summary>
        /// Gets the last Julian <see cref="CalendarDate"/>.
        /// </summary>
        public CalendarDate LastJulianDate { get; }

        /// <summary>
        /// Gets the first Gregorian <see cref="CalendarDate"/>.
        /// </summary>
        public CalendarDate FirstGregorianDate { get; }

        /// <summary>
        /// Gets the first Gregorian <see cref="DayNumber"/>.
        /// </summary>
        public DayNumber Switchover { get; }

        /// <summary>
        /// Gets the initial secular shift.
        /// </summary>
        public int SecularShift { get; }

        [Pure]
        public static GregorianReform FromLastJulianDate(int year, int month, int day)
        {
            var lastJulianDate = SimpleCalendar.Julian.GetCalendarDate(year, month, day);
            if (lastJulianDate < Official.LastJulianDate) Throw.YearOutOfRange(year);

            var switchover = lastJulianDate.ToDayNumber() + 1;
            var firstGregorianDate = CalendarDate.FromDayNumber(switchover);

            return new GregorianReform(lastJulianDate, firstGregorianDate, switchover);
        }

        [Pure]
        public static GregorianReform FromFirstGregorianDate(int year, int month, int day)
        {
            var firstGregorianDate = new CalendarDate(year, month, day);
            if (firstGregorianDate < Official.FirstGregorianDate) Throw.YearOutOfRange(year);

            var switchover = firstGregorianDate.ToDayNumber();
            var lastJulianDate = SimpleCalendar.Julian.GetCalendarDate(switchover - 1);

            return new GregorianReform(lastJulianDate, firstGregorianDate, switchover);
        }

        // Calcul du décalage séculaire initial.
        [Pure]
        private int InitSecularShift()
        {
            // REVIEW(code): et si Switchover ou LastJulian est bissextile?
            var (y, m, d) = FirstGregorianDate;
            var dayNumber = ((ICalendar)SimpleCalendar.Julian).GetDayNumber(y, m, d);
            return dayNumber - Switchover;
        }

        //// Calcul du décalage séculaire initial.
        //[Pure]
        //public int InitSecularShift1()
        //{
        //    // Le calendrier grégorien comporte moins d'années bissextiles.
        //    // Le nombre d'années bissextiles depuis l'an 1 est :
        //    // - grégorien : [y/4] - [y/100] + [y/400]
        //    // - julien : [y/4]
        //    // Le décalage entre les deux calendriers est donc : [y/100] - [y/400].
        //    // En 1582, année de la réforme, ce décalage est égal à
        //    // 395 - 383 = 12 jours. Cependant, on doit encore retrancher 2 au
        //    // décalage calculé précédemment (origines différentes, voir DayZero).
        //    // On peut effectuer tous les calculs dans N car
        //    // Switchover >= MinSwitchover.
        //    var (y, m, d) = FirstGregorianDate;
        //    int c = y / 100;
        //    int secularShift = c - (c >> 2) - 2;
        //    // Avant le 28 février d'une année séculaire commune dans
        //    // le calendrier grégorien, le décalage n'a pas encore lieu.
        //    return m <= 2 && d <= 28 && y % 100 == 0 && (c & 3) != 0
        //        ? secularShift - 1
        //        : secularShift;
        //}
    }
}
