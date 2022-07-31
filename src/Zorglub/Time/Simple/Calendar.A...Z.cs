// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Hemerology;

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
    public sealed class ArmenianCalendar : SimpleCalendar, IEpagomenalCalendar<CalendarDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArmenianCalendar"/>
        /// class.
        /// </summary>
        private ArmenianCalendar() : this(new Egyptian12Schema()) { }

        private ArmenianCalendar(Egyptian12Schema schema)
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
        public static ArmenianCalendar Instance { get; } = new ArmenianCalendar();

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
    public sealed class CopticCalendar : SimpleCalendar, IEpagomenalCalendar<CalendarDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CopticCalendar"/> class.
        /// </summary>
        private CopticCalendar() : this(new Coptic12Schema()) { }

        private CopticCalendar(Coptic12Schema schema)
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
        public static CopticCalendar Instance { get; } = new CopticCalendar();

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
    public sealed class EthiopicCalendar : SimpleCalendar, IEpagomenalCalendar<CalendarDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EthiopicCalendar"/>
        /// class.
        /// </summary>
        private EthiopicCalendar() : this(new Coptic12Schema()) { }

        private EthiopicCalendar(Coptic12Schema schema)
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
        public static EthiopicCalendar Instance { get; } = new EthiopicCalendar();

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
    public sealed class GregorianCalendar : SimpleCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianCalendar"/>
        /// class.
        /// </summary>
        private GregorianCalendar()
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
        public static GregorianCalendar Instance { get; } = new GregorianCalendar();

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
    public sealed class JulianCalendar : SimpleCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JulianCalendar"/> class.
        /// </summary>
        private JulianCalendar()
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
        public static JulianCalendar Instance { get; } = new JulianCalendar();

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
    public sealed class TabularIslamicCalendar : SimpleCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabularIslamicCalendar"/>
        /// class.
        /// </summary>
        private TabularIslamicCalendar()
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
        public static TabularIslamicCalendar Instance { get; } = new TabularIslamicCalendar();
    }

    /// <summary>
    /// Represents the Zoroastrian calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class ZoroastrianCalendar : SimpleCalendar, IEpagomenalCalendar<CalendarDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZoroastrianCalendar"/>
        /// class.
        /// </summary>
        private ZoroastrianCalendar() : this(new Egyptian12Schema()) { }

        private ZoroastrianCalendar(Egyptian12Schema schema)
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
        public static ZoroastrianCalendar Instance { get; } = new ZoroastrianCalendar();

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
