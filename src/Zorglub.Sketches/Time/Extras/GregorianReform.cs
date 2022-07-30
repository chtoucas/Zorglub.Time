// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extras
{
    using Zorglub.Time.Specialized;

    // See Zorglub.Time.Simple.GregorianReform

    public sealed record GregorianReform
    {
        public static readonly GregorianReform Official = new();

        private GregorianReform()
            : this(
                  new JulianDate(1582, 10, 4),
                  new GregorianDate(1582, 10, 15),
                 null)
        { }

        private GregorianReform(
            JulianDate lastJulianDate,
            GregorianDate firstGregorianDate,
            DayNumber? switchover)
        {
            LastJulianDate = lastJulianDate;
            FirstGregorianDate = firstGregorianDate;
            Switchover = switchover ?? FirstGregorianDate.DayNumber;
        }

        public JulianDate LastJulianDate { get; }
        public GregorianDate FirstGregorianDate { get; }
        public DayNumber Switchover { get; }

        private int? _secularShift;
        public int SecularShift => _secularShift ??= InitSecularShift();

        [Pure]
        public static GregorianReform FromLastJulianDate(JulianDate date)
        {
            if (date < Official.LastJulianDate) Throw.ArgumentOutOfRange(nameof(date));

            var switchover = date.DayNumber + 1;
            var firstGregorianDate = new GregorianDate(switchover);

            return new GregorianReform(date, firstGregorianDate, switchover);
        }

        [Pure]
        public static GregorianReform FromFirstGregorianDate(GregorianDate date)
        {
            if (date < Official.FirstGregorianDate) Throw.ArgumentOutOfRange(nameof(date));

            var switchover = date.DayNumber;
            var lastJulianDate = new JulianDate(switchover - 1);

            return new GregorianReform(lastJulianDate, date, switchover);
        }

        [Pure]
        public int InitSecularShift()
        {
            var (y, m, d) = FirstGregorianDate;
            var dayNumber = new JulianDate(y, m, d).DayNumber;
            return dayNumber - Switchover;
        }
    }
}
