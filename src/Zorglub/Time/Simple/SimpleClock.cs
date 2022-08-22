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
        /// Represents the clock.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ITimepiece _clock;

        /// <summary>
        /// Called from constructors in derived classes to initialize the <see cref="SimpleClock"/>
        /// class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
        protected SimpleClock(SimpleCalendar calendar, ITimepiece clock)
        {
            _calendar = calendar ?? throw new ArgumentNullException(nameof(clock));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
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

        [Pure]
        public static SimpleClock Create(SimpleCalendar calendar, ITimepiece clock)
        {
            Requires.NotNull(calendar);

            return CreateCore(calendar, clock);
        }

        [Pure]
        internal static SimpleClock CreateCore(SimpleCalendar calendar, ITimepiece clock)
        {
            Debug.Assert(calendar != null);

            return
                calendar.Id == Cuid.Gregorian ? new Gregorian(calendar, clock)
                : calendar.IsUserDefined ? new UserDefined(calendar, clock)
                : new System(calendar, clock);
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
            public Gregorian(SimpleCalendar calendar, ITimepiece clock) : base(calendar, clock)
            {
                Debug.Assert(calendar.Id == Cuid.Gregorian);
            }

            [Pure]
            public sealed override CalendarYear GetCurrentYear()
            {
                var today = _clock.Today();
                int y = GregorianFormulae.GetYear(today.DaysSinceZero);
                return new CalendarYear(y, Cuid.Gregorian);
            }

            [Pure]
            public sealed override CalendarMonth GetCurrentMonth()
            {
                var today = _clock.Today();
                var ymd = GregorianFormulae.GetDateParts(today.DaysSinceZero);
                return new CalendarMonth(ymd.Yemo, Cuid.Gregorian);
            }

            [Pure]
            public sealed override CalendarDate GetCurrentDate()
            {
                var today = _clock.Today();
                var ymd = GregorianFormulae.GetDateParts(today.DaysSinceZero);
                return new CalendarDate(ymd, Cuid.Gregorian);
            }

            [Pure]
            public sealed override CalendarDay GetCurrentDay()
            {
                var today = _clock.Today();
                return new(today.DaysSinceZero, Cuid.Gregorian);
            }

            [Pure]
            public sealed override OrdinalDate GetCurrentOrdinal()
            {
                var today = _clock.Today();
                var ydoy = GregorianFormulae.GetOrdinalParts(today.DaysSinceZero);
                return new OrdinalDate(ydoy, Cuid.Gregorian);
            }
        }

        private sealed class System : SimpleClock
        {
            public System(SimpleCalendar calendar, ITimepiece clock) : base(calendar, clock)
            {
                Debug.Assert(calendar.IsUserDefined == false);
            }

            [Pure]
            public sealed override CalendarYear GetCurrentYear()
            {
                var today = _clock.Today();
                int y = Schema.GetYear(today - Epoch);
                return new CalendarYear(y, Id);
            }

            [Pure]
            public sealed override CalendarMonth GetCurrentMonth()
            {
                var today = _clock.Today();
                var ymd = Schema.GetDateParts(today - Epoch);
                return new CalendarMonth(ymd.Yemo, Id);
            }

            [Pure]
            public sealed override CalendarDate GetCurrentDate()
            {
                var today = _clock.Today();
                var ymd = Schema.GetDateParts(today - Epoch);
                return new CalendarDate(ymd, Id);
            }

            [Pure]
            public sealed override CalendarDay GetCurrentDay()
            {
                var today = _clock.Today();
                return new(today - Epoch, Id);
            }

            [Pure]
            public sealed override OrdinalDate GetCurrentOrdinal()
            {
                var today = _clock.Today();
                var ydoy = Schema.GetOrdinalParts(today - Epoch);
                return new OrdinalDate(ydoy, Id);
            }
        }

        private sealed class UserDefined : SimpleClock
        {
            public UserDefined(SimpleCalendar calendar, ITimepiece clock) : base(calendar, clock)
            {
                Debug.Assert(calendar.IsUserDefined);
            }

            [Pure]
            public sealed override CalendarYear GetCurrentYear()
            {
                var today = _clock.Today();
                if (IsUserDefined) { Domain.Validate(today); }
                int y = Schema.GetYear(today - Epoch);
                return new CalendarYear(y, Id);
            }

            [Pure]
            public sealed override CalendarMonth GetCurrentMonth()
            {
                var today = _clock.Today();
                if (IsUserDefined) { Domain.Validate(today); }
                var ymd = Schema.GetDateParts(today - Epoch);
                return new CalendarMonth(ymd.Yemo, Id);
            }

            [Pure]
            public sealed override CalendarDate GetCurrentDate()
            {
                var today = _clock.Today();
                if (IsUserDefined) { Domain.Validate(today); }
                var ymd = Schema.GetDateParts(today - Epoch);
                return new CalendarDate(ymd, Id);
            }

            [Pure]
            public sealed override CalendarDay GetCurrentDay()
            {
                var today = _clock.Today();
                if (IsUserDefined) { Domain.Validate(today); }
                return new(today - Epoch, Id);
            }

            [Pure]
            public sealed override OrdinalDate GetCurrentOrdinal()
            {
                var today = _clock.Today();
                if (IsUserDefined) { Domain.Validate(today); }
                var ydoy = Schema.GetOrdinalParts(today - Epoch);
                return new OrdinalDate(ydoy, Id);
            }
        }
    }
}
