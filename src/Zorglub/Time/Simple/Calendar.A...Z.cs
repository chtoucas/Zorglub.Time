// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Hemerology;

    // FIXME(api): internal, remove IEpagomenalCalendar<>.

    #region Developer Notes

    // Calendars could "implement" IEpagomenalFeaturette, but they don't.
    // The rationale is that the method sould be on the date type, not on the
    // calendar. We already encounters this in the base class with
    // ICalendricalKernel.
    // IEpagomenalFeaturette.IsEpagomenalDay() is also static and the scope is
    // not, so too "much" work to do for little benefit.

    #endregion

    /// <summary>
    /// Represents the Armenian calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class SimpleArmenian : SimpleCalendar, IEpagomenalCalendar<CalendarDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleArmenian"/>
        /// class.
        /// </summary>
        private SimpleArmenian() : this(new Egyptian12Schema()) { }

        private SimpleArmenian(Egyptian12Schema schema)
            : base(
                  CalendarId.Armenian,
                  schema,
                  CalendarEpoch.Armenian,
                  proleptic: false)
        { }

        /// <summary>
        /// Gets a singleton instance of the Armenian calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SimpleArmenian Instance { get; } = new SimpleArmenian();

        /// <summary>
        /// Determines whether the specified date is an epagomenal day or not.
        /// </summary>
        [Pure]
        public bool IsEpagomenalDay(CalendarDate date, out int epagomenalNumber)
        {
            ValidateCuid(date.Cuid, nameof(date));

            date.Parts.Unpack(out int y, out int m, out int d);
            return Egyptian12Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
        }
    }

    /// <summary>
    /// Represents the Coptic calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class SimpleCoptic : SimpleCalendar, IEpagomenalCalendar<CalendarDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCoptic"/> class.
        /// </summary>
        private SimpleCoptic() : this(new Coptic12Schema()) { }

        private SimpleCoptic(Coptic12Schema schema)
            : base(
                  CalendarId.Coptic,
                  schema,
                  CalendarEpoch.Coptic,
                  proleptic: false)
        { }

        /// <summary>
        /// Gets a singleton instance of the Coptic calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SimpleCoptic Instance { get; } = new SimpleCoptic();

        /// <summary>
        /// Determines whether the specified date is an epagomenal day or not.
        /// </summary>
        [Pure]
        public bool IsEpagomenalDay(CalendarDate date, out int epagomenalNumber)
        {
            ValidateCuid(date.Cuid, nameof(date));

            date.Parts.Unpack(out int y, out int m, out int d);
            return Coptic12Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
        }
    }

    /// <summary>
    /// Represents the Ethiopic calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class SimpleEthiopic : SimpleCalendar, IEpagomenalCalendar<CalendarDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleEthiopic"/>
        /// class.
        /// </summary>
        private SimpleEthiopic() : this(new Coptic12Schema()) { }

        private SimpleEthiopic(Coptic12Schema schema)
            : base(
                  CalendarId.Ethiopic,
                  schema,
                  CalendarEpoch.Ethiopic,
                  proleptic: false)
        { }

        /// <summary>
        /// Gets a singleton instance of the Ethiopic calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SimpleEthiopic Instance { get; } = new SimpleEthiopic();

        /// <summary>
        /// Determines whether the specified date is an epagomenal day or not.
        /// </summary>
        [Pure]
        public bool IsEpagomenalDay(CalendarDate date, out int epagomenalNumber)
        {
            ValidateCuid(date.Cuid, nameof(date));

            date.Parts.Unpack(out int y, out int m, out int d);
            return Coptic12Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
        }
    }

    /// <summary>
    /// Represents the proleptic Gregorian calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class SimpleGregorian : SimpleCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleGregorian"/>
        /// class.
        /// </summary>
        private SimpleGregorian()
            : base(
                  CalendarId.Gregorian,
                  new GregorianSchema(),
                  DayZero.NewStyle,
                  proleptic: true)
        { }

        /// <summary>
        /// Gets a singleton instance of the proleptic Gregorian calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SimpleGregorian Instance { get; } = new SimpleGregorian();

        /// <inheritdoc />
        [Pure]
        internal override DayOfWeek GetDayOfWeek(CalendarDate date)
        {
            // Faster than the base method which relies on CountDaysSinceEpoch().
            // Furthermore, it only works with non-proleptic calendars.
            Debug.Assert(date.Cuid == Id);

            date.Parts.Unpack(out int y, out int m, out int d);
            int doomsday = DoomsdayRule.GetGregorianDoomsday(y, m);
            return (DayOfWeek)MathZ.Modulo(doomsday + d, CalendricalConstants.DaysInWeek);
        }

        /// <inheritdoc />
        [Pure]
        internal override DayOfWeek GetDayOfWeek(OrdinalDate date)
        {
            // The base method only works with non-proleptic calendars.
            Debug.Assert(date.Cuid == Id);

            date.Parts.Unpack(out int y, out int doy);
            // The Gregorian epoch is a Monday.
            int days = (int)DayOfWeek.Monday + Schema.CountDaysSinceEpoch(y, doy);
            return (DayOfWeek)MathZ.Modulo(days, CalendricalConstants.DaysInWeek);
        }
    }

    /// <summary>
    /// Represents the proleptic Julian calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class SimpleJulian : SimpleCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleJulian"/> class.
        /// </summary>
        private SimpleJulian()
            : base(
                  CalendarId.Julian,
                  new JulianSchema(),
                  DayZero.OldStyle,
                  proleptic: true)
        { }

        /// <summary>
        /// Gets a singleton instance of the proleptic Julian calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SimpleJulian Instance { get; } = new SimpleJulian();

        /// <inheritdoc />
        [Pure]
        internal override DayOfWeek GetDayOfWeek(CalendarDate date)
        {
            // Faster than the base method which relies on CountDaysSinceEpoch().
            // Furthermore, it only works with non-proleptic calendars.
            Debug.Assert(date.Cuid == Id);

            date.Parts.Unpack(out int y, out int m, out int d);
            int doomsday = DoomsdayRule.GetJulianDoomsday(y, m);
            return (DayOfWeek)MathZ.Modulo(doomsday + d, CalendricalConstants.DaysInWeek);
        }

        /// <inheritdoc />
        [Pure]
        internal override DayOfWeek GetDayOfWeek(OrdinalDate date)
        {
            // The base method only works with non-proleptic calendars.
            Debug.Assert(date.Cuid == Id);

            date.Parts.Unpack(out int y, out int doy);
            // The Julian epoch is a Saturday.
            int days = (int)DayOfWeek.Saturday + Schema.CountDaysSinceEpoch(y, doy);
            return (DayOfWeek)MathZ.Modulo(days, CalendricalConstants.DaysInWeek);
        }
    }

    /// <summary>
    /// Represents the Tabular Islamic calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class SimpleTabularIslamic : SimpleCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTabularIslamic"/>
        /// class.
        /// </summary>
        private SimpleTabularIslamic()
            : base(
                  CalendarId.TabularIslamic,
                  new TabularIslamicSchema(),
                  CalendarEpoch.TabularIslamic,
                  proleptic: false)
        { }

        /// <summary>
        /// Gets a singleton instance of the Tabular Islamic calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SimpleTabularIslamic Instance { get; } = new SimpleTabularIslamic();
    }

    /// <summary>
    /// Represents the Zoroastrian calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class SimpleZoroastrian : SimpleCalendar, IEpagomenalCalendar<CalendarDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleZoroastrian"/>
        /// class.
        /// </summary>
        private SimpleZoroastrian() : this(new Egyptian12Schema()) { }

        private SimpleZoroastrian(Egyptian12Schema schema)
            : base(
                  CalendarId.Zoroastrian,
                  schema,
                  CalendarEpoch.Zoroastrian,
                  proleptic: false)
        { }

        /// <summary>
        /// Gets a singleton instance of the Zoroastrian calendar.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SimpleZoroastrian Instance { get; } = new SimpleZoroastrian();

        /// <summary>
        /// Determines whether the specified date is an epagomenal day or not.
        /// </summary>
        [Pure]
        public bool IsEpagomenalDay(CalendarDate date, out int epagomenalNumber)
        {
            ValidateCuid(date.Cuid, nameof(date));

            date.Parts.Unpack(out int y, out int m, out int d);
            return Egyptian12Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
        }
    }
}
