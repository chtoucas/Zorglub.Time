// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Hemerology;

    // FIXME(api): remove IEpagomenalCalendar<>, interface and in ZoroastrianSimpleCalendar.

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
    internal sealed class ArmenianSimpleCalendar : SimpleCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArmenianSimpleCalendar"/>
        /// class.
        /// </summary>
        private ArmenianSimpleCalendar() : this(new Egyptian12Schema()) { }

        private ArmenianSimpleCalendar(Egyptian12Schema schema)
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
        public static ArmenianSimpleCalendar Instance { get; } = new ArmenianSimpleCalendar();
    }

    /// <summary>
    /// Represents the Coptic calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed class CopticSimpleCalendar : SimpleCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CopticSimpleCalendar"/> class.
        /// </summary>
        private CopticSimpleCalendar() : this(new Coptic12Schema()) { }

        private CopticSimpleCalendar(Coptic12Schema schema)
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
        public static CopticSimpleCalendar Instance { get; } = new CopticSimpleCalendar();
    }

    /// <summary>
    /// Represents the Ethiopic calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed class EthiopicSimpleCalendar : SimpleCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EthiopicSimpleCalendar"/>
        /// class.
        /// </summary>
        private EthiopicSimpleCalendar() : this(new Coptic12Schema()) { }

        private EthiopicSimpleCalendar(Coptic12Schema schema)
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
        public static EthiopicSimpleCalendar Instance { get; } = new EthiopicSimpleCalendar();
    }

    /// <summary>
    /// Represents the proleptic Gregorian calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed class GregorianSimpleCalendar : SimpleCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianSimpleCalendar"/>
        /// class.
        /// </summary>
        private GregorianSimpleCalendar()
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
        public static GregorianSimpleCalendar Instance { get; } = new GregorianSimpleCalendar();

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
    internal sealed class JulianSimpleCalendar : SimpleCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JulianSimpleCalendar"/> class.
        /// </summary>
        private JulianSimpleCalendar()
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
        public static JulianSimpleCalendar Instance { get; } = new JulianSimpleCalendar();

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
    internal sealed class TabularIslamicSimpleCalendar : SimpleCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabularIslamicSimpleCalendar"/>
        /// class.
        /// </summary>
        private TabularIslamicSimpleCalendar()
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
        public static TabularIslamicSimpleCalendar Instance { get; } = new TabularIslamicSimpleCalendar();
    }

    /// <summary>
    /// Represents the Zoroastrian calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed class ZoroastrianSimpleCalendar : SimpleCalendar, IEpagomenalCalendar<CalendarDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZoroastrianSimpleCalendar"/>
        /// class.
        /// </summary>
        private ZoroastrianSimpleCalendar() : this(new Egyptian12Schema()) { }

        private ZoroastrianSimpleCalendar(Egyptian12Schema schema)
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
        public static ZoroastrianSimpleCalendar Instance { get; } = new ZoroastrianSimpleCalendar();

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
