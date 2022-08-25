// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Horology;

    /// <summary>
    /// Represents a clock for a simple calendar.
    /// </summary>
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
        private readonly IClock _clock;

        /// <summary>
        /// Called from constructors in derived classes to initialize the <see cref="SimpleClock"/>
        /// class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
        private SimpleClock(SimpleCalendar calendar, IClock clock)
        {
            Debug.Assert(calendar != null);

            _calendar = calendar;
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
        /// Gets the epoch.
        /// </summary>
        private DayNumber Epoch => _calendar.Epoch;

        /// <summary>
        /// Gets the domain.
        /// </summary>
        private Range<DayNumber> Domain => _calendar.Domain;

        /// <summary>
        /// Creates a new instance of the <see cref="SimpleClock"/> class for the specified calendar
        /// and clock.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
        [Pure]
        public static SimpleClock Create(SimpleCalendar calendar, IClock clock)
        {
            Requires.NotNull(calendar);

            return calendar.IsUserDefined ? CreateDefault(calendar, clock)
                : CreateSystem(calendar, clock);
        }

        /// <summary>
        /// Obtains the default clock for the specified calendar and clock.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
        [Pure]
        internal static SimpleClock CreateDefault(SimpleCalendar calendar, IClock clock)
        {
            Debug.Assert(calendar != null);

            return new DefaultClock(calendar, clock);
        }

        /// <summary>
        /// Obtains the default clock for the specified system calendar and clock.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
        [Pure]
        internal static SimpleClock CreateSystem(SimpleCalendar calendar, IClock clock)
        {
            Debug.Assert(calendar != null);
            Debug.Assert(calendar.IsUserDefined == false);

            return calendar.Id == Cuid.Civil || calendar.Id == Cuid.Gregorian
                ? new GregorianClock(calendar, clock)
                : new SystemClock(calendar, clock);
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
        /// Obtains a <see cref="CalendarDay"/> value representing the current day.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by the
        /// underlying calendar.</exception>
        [Pure] public abstract CalendarDay GetCurrentDay();

#pragma warning restore CA1024

        // Requirements:
        // - System calendar (no validation)
        // - Gregorian schema and epoch
        private sealed class GregorianClock : SimpleClock
        {
            public GregorianClock(SimpleCalendar calendar, IClock clock)
                : base(calendar, clock)
            {
                Debug.Assert(calendar.Id == Cuid.Gregorian || calendar.Id == Cuid.Civil);
                Debug.Assert(calendar.Epoch == DayZero.NewStyle);
            }

            [Pure]
            public override CalendarYear GetCurrentYear()
            {
                var today = _clock.Today();
                int y = GregorianFormulae.GetYear(today.DaysSinceZero);
                return new CalendarYear(y, Id);
            }

            [Pure]
            public override CalendarMonth GetCurrentMonth()
            {
                var today = _clock.Today();
                var ymd = GregorianFormulae.GetDateParts(today.DaysSinceZero);
                return new CalendarMonth(ymd.Yemo, Id);
            }

            [Pure]
            public override CalendarDay GetCurrentDay()
            {
                var today = _clock.Today();
                return new(today.DaysSinceZero, Id);
            }
        }

        // Requirement: system calendar (no validation).
        private sealed class SystemClock : SimpleClock
        {
            public SystemClock(SimpleCalendar calendar, IClock clock) : base(calendar, clock)
            {
                Debug.Assert(calendar.IsUserDefined == false);
            }

            [Pure]
            public override CalendarYear GetCurrentYear()
            {
                var today = _clock.Today();
                int y = Schema.GetYear(today - Epoch);
                return new CalendarYear(y, Id);
            }

            [Pure]
            public override CalendarMonth GetCurrentMonth()
            {
                var today = _clock.Today();
                var ymd = Schema.GetDateParts(today - Epoch);
                return new CalendarMonth(ymd.Yemo, Id);
            }

            [Pure]
            public override CalendarDay GetCurrentDay()
            {
                var today = _clock.Today();
                return new(today - Epoch, Id);
            }
        }

        private sealed class DefaultClock : SimpleClock
        {
            public DefaultClock(SimpleCalendar calendar, IClock clock)
                : base(calendar, clock) { }

            [Pure]
            public override CalendarYear GetCurrentYear()
            {
                var today = _clock.Today();
                Domain.Validate(today);
                int y = Schema.GetYear(today - Epoch);
                return new CalendarYear(y, Id);
            }

            [Pure]
            public override CalendarMonth GetCurrentMonth()
            {
                var today = _clock.Today();
                Domain.Validate(today);
                var ymd = Schema.GetDateParts(today - Epoch);
                return new CalendarMonth(ymd.Yemo, Id);
            }

            [Pure]
            public override CalendarDay GetCurrentDay()
            {
                var today = _clock.Today();
                Domain.Validate(today);
                return new CalendarDay(today - Epoch, Id);
            }
        }
    }
}
