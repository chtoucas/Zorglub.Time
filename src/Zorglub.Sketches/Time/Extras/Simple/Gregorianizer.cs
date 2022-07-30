// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extras.Simple
{
    using Zorglub.Time.Simple;

    // Version minimaliste de la chronologie julienne réformée.
    public sealed class Gregorianizer
    {
        private readonly OrdinalDate _lastJulianOrdinal;
        private readonly OrdinalDate _firstGregorianOrdinal;

        public Gregorianizer() : this("Papal States", GregorianReform.Official) { }

        public Gregorianizer(string? regionName, GregorianReform reform)
        {
            RegionName = regionName ?? "Unspecified region";
            Reform = reform ?? throw new ArgumentNullException(nameof(reform));

            _lastJulianOrdinal = reform.LastJulianDate.ToOrdinalDate();
            _firstGregorianOrdinal = reform.FirstGregorianDate.ToOrdinalDate();
        }

        public string RegionName { get; }

        public GregorianReform Reform { get; }

        public CalendarDate Gregorianize(DayNumber dayNumber) =>
            dayNumber < Reform.Switchover
            ? JulianCalendar.Instance.GetCalendarDate(dayNumber)
            : GregorianCalendar.Instance.GetCalendarDate(dayNumber);

        public CalendarDay Gregorianize(CalendarDay date) =>
            date.Cuid switch
            {
                Cuid.Gregorian =>
                    date.DayNumber >= Reform.Switchover ? date
                        : date.WithCalendar(JulianCalendar.Instance),

                Cuid.Julian =>
                    date.DayNumber < Reform.Switchover ? date
                        : date.WithCalendar(GregorianCalendar.Instance),

                _ => Throw.Argument<CalendarDay>(nameof(date)),
            };

        public CalendarDate Gregorianize(CalendarDate date) =>
            date.Cuid switch
            {
                Cuid.Gregorian =>
                    date >= Reform.FirstGregorianDate ? date
                        : date.WithCalendar(JulianCalendar.Instance),

                Cuid.Julian =>
                    date <= Reform.LastJulianDate ? date
                        : date.WithCalendar(GregorianCalendar.Instance),

                _ => Throw.Argument<CalendarDate>(nameof(date)),
            };

        public OrdinalDate Gregorianize(OrdinalDate date) =>
            date.Cuid switch
            {
                Cuid.Gregorian =>
                    date >= _firstGregorianOrdinal ? date
                        : date.WithCalendar(JulianCalendar.Instance),

                Cuid.Julian =>
                    date <= _lastJulianOrdinal ? date
                        : date.WithCalendar(GregorianCalendar.Instance),

                _ => Throw.Argument<OrdinalDate>(nameof(date)),
            };
    }
}
