// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Horology;

    public abstract class SimpleClock
    {
        /// <summary>
        /// Represents the calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly SimpleCalendar _calendar;

        /// <summary>
        /// Represents the timepiece.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ITimepiece _timepiece;

        /// <summary>
        /// Called from constructors in derived classes to initialize the <see cref="SimpleClock"/>
        /// class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="timepiece"/> is null.</exception>
        private SimpleClock(SimpleCalendar calendar, ITimepiece timepiece)
        {
            Debug.Assert(calendar != null);

            _calendar = calendar;
            _timepiece = timepiece ?? throw new ArgumentNullException(nameof(timepiece));
        }

        /// <summary>
        /// Gets the ID of the current instance.
        /// </summary>
        private Cuid Id => _calendar.Id;

        /// <summary>
        /// Gets the calendrical schema.
        /// </summary>
        private SystemSchema Schema => _calendar.Schema;

        /// <summary>
        /// Returns true if the current instance is user-defined; otherwise returns false.
        /// </summary>
        private bool IsUserDefined => _calendar.IsUserDefined;

        /// <summary>
        /// Gets the epoch.
        /// </summary>
        private DayNumber Epoch => _calendar.Epoch;

        /// <summary>
        /// Gets the domain.
        /// </summary>
        private Range<DayNumber> Domain => _calendar.Domain;

        /// <summary>
        /// Creates a new instance of the <see cref="SimpleClock"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="timepiece"/> is null.</exception>
        [Pure]
        public static SimpleClock Create(SimpleCalendar calendar, ITimepiece timepiece)
        {
            Requires.NotNull(calendar);

            return CreateCore(calendar, timepiece);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SimpleClock"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="timepiece"/> is null.</exception>
        [Pure]
        internal static SimpleClock CreateCore(SimpleCalendar calendar, ITimepiece timepiece)
        {
            Debug.Assert(calendar != null);

            return
                calendar.Id == Cuid.Gregorian ? new Gregorian(calendar, timepiece)
                : calendar.IsUserDefined ? new UserDefined(calendar, timepiece)
                : new System(calendar, timepiece);
        }

        // Identical to the conversion methods found in SimpleCalendar but
        // without the validation part for system calendars as we know for sure
        // that today is always admissible.

#pragma warning disable CA1024 // Use properties where appropriate (Design)
        // These methods SHOULD NOT be properties.
        // Let's not repeat the mistake of DateTime.Now.

        /// <summary>
        /// Obtains a <see cref="CalendarYear"/> value representing the current year.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by the
        /// underlying calendar.</exception>
        [Pure] public abstract CalendarYear GetCurrentYear();

        /// <summary>
        /// Obtains a <see cref="CalendarMonth"/> value representing the current month.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by the
        /// underlying calendar.</exception>
        [Pure] public abstract CalendarMonth GetCurrentMonth();

        /// <summary>
        /// Obtains a <see cref="CalendarDate"/> value representing the current date.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by the
        /// underlying calendar.</exception>
        [Pure] public abstract CalendarDate GetCurrentDate();

        /// <summary>
        /// Obtains a <see cref="CalendarDay"/> value representing the current date.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by the
        /// underlying calendar.</exception>
        [Pure] public abstract CalendarDay GetCurrentDay();

        /// <summary>
        /// Obtains a <see cref="OrdinalDate"/> value representing the current date.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by the
        /// underlying calendar.</exception>
        [Pure] public abstract OrdinalDate GetCurrentOrdinal();

#pragma warning restore CA1024

        private sealed class Gregorian : SimpleClock
        {
            public Gregorian(SimpleCalendar calendar, ITimepiece timepiece)
                : base(calendar, timepiece)
            {
                Debug.Assert(calendar.Id == Cuid.Gregorian);
            }

            [Pure]
            public override CalendarYear GetCurrentYear()
            {
                var today = _timepiece.Today();
                int y = GregorianFormulae.GetYear(today.DaysSinceZero);
                return new CalendarYear(y, Cuid.Gregorian);
            }

            [Pure]
            public override CalendarMonth GetCurrentMonth()
            {
                var today = _timepiece.Today();
                var ymd = GregorianFormulae.GetDateParts(today.DaysSinceZero);
                return new CalendarMonth(ymd.Yemo, Cuid.Gregorian);
            }

            [Pure]
            public override CalendarDate GetCurrentDate()
            {
                var today = _timepiece.Today();
                var ymd = GregorianFormulae.GetDateParts(today.DaysSinceZero);
                return new CalendarDate(ymd, Cuid.Gregorian);
            }

            [Pure]
            public override CalendarDay GetCurrentDay()
            {
                var today = _timepiece.Today();
                return new(today.DaysSinceZero, Cuid.Gregorian);
            }

            [Pure]
            public override OrdinalDate GetCurrentOrdinal()
            {
                var today = _timepiece.Today();
                var ydoy = GregorianFormulae.GetOrdinalParts(today.DaysSinceZero);
                return new OrdinalDate(ydoy, Cuid.Gregorian);
            }
        }

        private sealed class System : SimpleClock
        {
            public System(SimpleCalendar calendar, ITimepiece timepiece) : base(calendar, timepiece)
            {
                Debug.Assert(calendar.IsUserDefined == false);
            }

            [Pure]
            public override CalendarYear GetCurrentYear()
            {
                var today = _timepiece.Today();
                int y = Schema.GetYear(today - Epoch);
                return new CalendarYear(y, Id);
            }

            [Pure]
            public override CalendarMonth GetCurrentMonth()
            {
                var today = _timepiece.Today();
                var ymd = Schema.GetDateParts(today - Epoch);
                return new CalendarMonth(ymd.Yemo, Id);
            }

            [Pure]
            public override CalendarDate GetCurrentDate()
            {
                var today = _timepiece.Today();
                var ymd = Schema.GetDateParts(today - Epoch);
                return new CalendarDate(ymd, Id);
            }

            [Pure]
            public override CalendarDay GetCurrentDay()
            {
                var today = _timepiece.Today();
                return new(today - Epoch, Id);
            }

            [Pure]
            public override OrdinalDate GetCurrentOrdinal()
            {
                var today = _timepiece.Today();
                var ydoy = Schema.GetOrdinalParts(today - Epoch);
                return new OrdinalDate(ydoy, Id);
            }
        }

        private sealed class UserDefined : SimpleClock
        {
            public UserDefined(SimpleCalendar calendar, ITimepiece timepiece)
                : base(calendar, timepiece)
            {
                Debug.Assert(calendar.IsUserDefined);
            }

            [Pure]
            public override CalendarYear GetCurrentYear()
            {
                var today = _timepiece.Today();
                if (IsUserDefined) { Domain.Validate(today); }
                int y = Schema.GetYear(today - Epoch);
                return new CalendarYear(y, Id);
            }

            [Pure]
            public override CalendarMonth GetCurrentMonth()
            {
                var today = _timepiece.Today();
                if (IsUserDefined) { Domain.Validate(today); }
                var ymd = Schema.GetDateParts(today - Epoch);
                return new CalendarMonth(ymd.Yemo, Id);
            }

            [Pure]
            public override CalendarDate GetCurrentDate()
            {
                var today = _timepiece.Today();
                if (IsUserDefined) { Domain.Validate(today); }
                var ymd = Schema.GetDateParts(today - Epoch);
                return new CalendarDate(ymd, Id);
            }

            [Pure]
            public override CalendarDay GetCurrentDay()
            {
                var today = _timepiece.Today();
                if (IsUserDefined) { Domain.Validate(today); }
                return new(today - Epoch, Id);
            }

            [Pure]
            public override OrdinalDate GetCurrentOrdinal()
            {
                var today = _timepiece.Today();
                if (IsUserDefined) { Domain.Validate(today); }
                var ydoy = Schema.GetOrdinalParts(today - Epoch);
                return new OrdinalDate(ydoy, Id);
            }
        }
    }
}
