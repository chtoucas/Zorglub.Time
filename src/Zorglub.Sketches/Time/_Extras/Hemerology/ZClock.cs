// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Horology;

    public abstract class ZClock
    {
        /// <summary>
        /// Represents the calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ZCalendar _calendar;

        /// <summary>
        /// Represents the timepiece.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ITimepiece _timepiece;

        /// <summary>
        /// Called from constructors in derived classes to initialize the <see cref="ZClock"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="timepiece"/> is null.</exception>
        private ZClock(ZCalendar calendar, ITimepiece timepiece)
        {
            Debug.Assert(calendar != null);

            _calendar = calendar;
            _timepiece = timepiece ?? throw new ArgumentNullException(nameof(timepiece));
        }

        /// <summary>
        /// Gets the ID of the current instance.
        /// </summary>
        protected int Id => _calendar.Id;

        /// <summary>
        /// Returns true if the current instance is user-defined; otherwise returns false.
        /// </summary>
        protected bool IsUserDefined => _calendar.IsUserDefined;

        /// <summary>
        /// Gets the epoch.
        /// </summary>
        protected DayNumber Epoch => _calendar.Epoch;

        /// <summary>
        /// Gets the domain.
        /// </summary>
        protected Range<DayNumber> Domain => _calendar.Domain;

        /// <summary>
        /// Creates a new instance of the <see cref="ZClock"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="timepiece"/> is null.</exception>
        [Pure]
        public static ZClock Create(ZCalendar calendar, ITimepiece timepiece)
        {
            Requires.NotNull(calendar);

            return CreateCore(calendar, timepiece);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ZClock"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="timepiece"/> is null.</exception>
        [Pure]
        internal static ZClock CreateCore(ZCalendar calendar, ITimepiece timepiece)
        {
            Debug.Assert(calendar != null);

            return
                calendar.IsUserDefined ? new DefaultClock(calendar, timepiece)
                : calendar.Epoch == DayZero.NewStyle ? new GregorianClock(calendar, timepiece)
                : new SystemClock(calendar, timepiece);
        }

        /// <summary>
        /// Obtains a <see cref="ZDate"/> value representing the current date.
        /// </summary>
        /// <exception cref="AoorException">Today is outside the range of values supported by the
        /// underlying calendar.</exception>
        [Pure]
        [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "<Pending>")]
        public abstract ZDate GetCurrentDate();

        // Requirement: system calendar (no validation).
        private sealed class SystemClock : ZClock
        {
            public SystemClock(ZCalendar calendar, ITimepiece timepiece) : base(calendar, timepiece)
            {
                Debug.Assert(calendar.IsUserDefined == false);
            }

            [Pure]
            public override ZDate GetCurrentDate()
            {
                var today = _timepiece.Today();
                return new ZDate(today - Epoch, Id);
            }
        }

        // Requirements:
        // - System calendar (no validation)
        // - Gregorian epoch
        private sealed class GregorianClock : ZClock
        {
            public GregorianClock(ZCalendar calendar, ITimepiece timepiece)
                : base(calendar, timepiece)
            {
                Debug.Assert(calendar.IsUserDefined == false);
                Debug.Assert(calendar.Epoch == DayZero.NewStyle);
            }

            [Pure]
            public override ZDate GetCurrentDate()
            {
                var today = _timepiece.Today();
                return new ZDate(today.DaysSinceZero, Id);
            }
        }

        private sealed class DefaultClock : ZClock
        {
            public DefaultClock(ZCalendar calendar, ITimepiece timepiece)
                : base(calendar, timepiece) { }

            [Pure]
            public override ZDate GetCurrentDate()
            {
                var today = _timepiece.Today();
                Domain.Validate(today);
                return new ZDate(today - Epoch, Id);
            }
        }
    }
}
