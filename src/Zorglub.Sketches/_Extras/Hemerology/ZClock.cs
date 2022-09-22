// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

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
    /// Represents the clock.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly IClock _clock;

    /// <summary>
    /// Called from constructors in derived classes to initialize the <see cref="ZClock"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
    private ZClock(ZCalendar calendar, IClock clock)
    {
        Debug.Assert(calendar != null);

        _calendar = calendar;
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
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
    /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
    [Pure]
    public static ZClock Create(ZCalendar calendar, IClock clock)
    {
        Requires.NotNull(calendar);

        return CreateCore(calendar, clock);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ZClock"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
    [Pure]
    internal static ZClock CreateCore(ZCalendar calendar, IClock clock)
    {
        Debug.Assert(calendar != null);

        return
            calendar.IsUserDefined ? new DefaultClock(calendar, clock)
            : calendar.Epoch == DayZero.NewStyle ? new GregorianClock(calendar, clock)
            : new SystemClock(calendar, clock);
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
        public SystemClock(ZCalendar calendar, IClock clock) : base(calendar, clock)
        {
            Debug.Assert(calendar.IsUserDefined == false);
        }

        [Pure]
        public override ZDate GetCurrentDate()
        {
            var today = _clock.Today();
            return new ZDate(today - Epoch, Id);
        }
    }

    // Requirements:
    // - System calendar (no validation)
    // - Gregorian epoch
    private sealed class GregorianClock : ZClock
    {
        public GregorianClock(ZCalendar calendar, IClock clock)
            : base(calendar, clock)
        {
            Debug.Assert(calendar.IsUserDefined == false);
            Debug.Assert(calendar.Epoch == DayZero.NewStyle);
        }

        [Pure]
        public override ZDate GetCurrentDate()
        {
            var today = _clock.Today();
            return new ZDate(today.DaysSinceZero, Id);
        }
    }

    private sealed class DefaultClock : ZClock
    {
        public DefaultClock(ZCalendar calendar, IClock clock)
            : base(calendar, clock) { }

        [Pure]
        public override ZDate GetCurrentDate()
        {
            var today = _clock.Today();
            Domain.Validate(today);
            return new ZDate(today - Epoch, Id);
        }
    }
}
