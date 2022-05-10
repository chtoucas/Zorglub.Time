// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology;

    // Only for calendars with an integral number of weeks per year (leap-week
    // calendars)?
    //
    // TODO:
    // - add WeekParts
    // - add also CalendarWeekDate (year, week, dayOfWeek)
    //   => revoir la repr. binaire pour que les deux soient compatibles.
    //      s'inspirer de Yemoda.
    //   Leap-week calendars may drop the subdivision of a year in months.
    // - validate Cuid.
    // - custom CalendarRegistry.FindXXX() and SchemaProvider.
    // - math ops.

    /// <summary>
    /// Represents a calendar week.
    /// <para><see cref="CalendarWeek"/> is an immutable struct.</para>
    /// </summary>
    public readonly partial struct CalendarWeek
        : IEquatable<CalendarWeek>, IComparable<CalendarWeek>, IComparable
    {
        /// <summary>
        /// Represents the internal binary representation.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly Yewex _bin; // 4 bytes

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarWeek"/> struct.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal CalendarWeek(int y, int w, Cuid cuid)
        {
            _bin = new Yewex(y, w, (int)cuid);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarWeek"/> struct.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal CalendarWeek(Yewe yw, Cuid cuid)
        {
            _bin = new Yewex(yw, (int)cuid);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarWeek"/> struct.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal CalendarWeek(Yewex bin)
        {
            _bin = bin;
        }

        /// <summary>
        /// Gets the century of the era.
        /// </summary>
        public Ord CenturyOfEra => Ord.FromInt32(Century);

        /// <summary>
        /// Gets the century number.
        /// </summary>
        public int Century => YearNumbering.GetCentury(Year);

        /// <summary>
        /// Gets the year of the era.
        /// </summary>
        public Ord YearOfEra => Ord.FromInt32(Year);

        /// <summary>
        /// Gets the year of the century.
        /// <para>The result is in the range from 1 to 100.</para>
        /// </summary>
        public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

        /// <summary>
        /// Gets the (algebraic) year number.
        /// </summary>
        public int Year => _bin.Year;

        /// <summary>
        /// Gets the week of the year.
        /// </summary>
        public int WeekOfYear => _bin.WeekOfYear;

        /// <summary>
        /// Gets the calendar to which belongs the current instance.
        /// </summary>
        /// <remarks>
        /// <para>Performance: cache this property locally if necessary.</para>
        /// </remarks>
        public Calendar Calendar => CalendarCatalog.GetCalendarUnchecked(_bin.Extra);

        /// <summary>
        /// Gets the date parts of current instance.
        /// </summary>
        internal Yewe Parts => _bin.Yewe;

        /// <summary>
        /// Gets the ID of the calendar to which belongs the current instance.
        /// </summary>
        internal Cuid Cuid => (Cuid)_bin.Extra;

        /// <summary>
        /// Gets a read-only reference to the calendar to which belongs the
        /// current instance.
        /// </summary>
        internal ref readonly Calendar CalendarRef
        {
            // CIL code size = 17 bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref CalendarCatalog.GetCalendarUnsafe(_bin.Extra);
        }

        /// <summary>
        /// Returns a culture-independent string representation of the current
        /// instance.
        /// </summary>
        [Pure]
        public override string ToString()
        {
            _bin.Unpack(out int y, out int w);
            ref readonly var chr = ref CalendarRef;
            return FormattableString.Invariant($"{w:D3}/{y:D4} ({chr})");
        }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int weekOfYear) =>
            _bin.Unpack(out year, out weekOfYear);

        private void ValidateCuid(Cuid cuid, string paramName)
        {
            if (cuid != Cuid) Throw.BadCuid(paramName, Cuid, cuid);
        }
    }

    public partial struct CalendarWeek
    {
        /// <summary>
        /// Converts the current instance to an interval of days.
        /// </summary>
        [Pure]
        public DateRange ToInterval() => DateRange.Create(GetStartOfWeek(), GetEndOfWeek());

        /// <summary>
        /// Converts the current instance to a day range within a different
        /// calendar.
        /// </summary>
        [Pure]
        public DateRange WithCalendar(Calendar newCalendar) =>
            ToInterval().WithCalendar(newCalendar);

        /// <summary>
        /// Adjusts the year field of this week instance to the specified value,
        /// yielding a new week.
        /// </summary>
        /// <exception cref="AoorException">The resulting week would be invalid.
        /// </exception>
        [Pure]
        public CalendarWeek AdjustYear(int newYear)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adjusts the week of the year field of this week instance to the
        /// specified value, yielding a new week.
        /// </summary>
        /// <exception cref="AoorException">The resulting week would be invalid.
        /// </exception>
        [Pure]
        public CalendarWeek AdjustWeekOfYear(int newWeek)
        {
            //int y = Year;
            //Schema.ValidateWeekOfYear(y, newWeek, nameof(newWeek));
            //return new CalendarWeek(y, newWeek, Cuid);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtains the first day of this week instance.
        /// </summary>
        [Pure]
        [SuppressMessage("Design", "CA1024:Use properties where appropriate")]
        public CalendarDate GetStartOfWeek()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtains the date corresponding to the specified day of this week
        /// instance.
        /// </summary>
        [Pure]
        public CalendarDate GetDayOfWeek(DayOfWeek dayOfWeek)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtains the last day of this week instance.
        /// </summary>
        [Pure]
        [SuppressMessage("Design", "CA1024:Use properties where appropriate")]
        public CalendarDate GetEndOfWeek()
        {
            throw new NotImplementedException();
        }
    }

    public partial struct CalendarWeek // IEquatable.
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="CalendarWeek"/>
        /// are equal.
        /// </summary>
        public static bool operator ==(CalendarWeek left, CalendarWeek right) =>
            left._bin == right._bin;

        /// <summary>
        /// Determines whether two specified instances of <see cref="CalendarWeek"/>
        /// are not equal.
        /// </summary>
        public static bool operator !=(CalendarWeek left, CalendarWeek right) => !(left == right);

        /// <inheritdoc />
        [Pure]
        public bool Equals(CalendarWeek other) => this == other;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is CalendarWeek week && this == week;

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => HashCode.Combine(_bin, Cuid);
    }

    public partial struct CalendarWeek // IComparable.
    {
        /// <summary>
        /// Compares the two specified instances to see if the left one is
        /// strictly earlier than the right one.
        /// </summary>
        public static bool operator <(CalendarWeek left, CalendarWeek right) =>
            left.CompareTo(right) < 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is
        /// earlier than or equal to the right one.
        /// </summary>
        public static bool operator <=(CalendarWeek left, CalendarWeek right) =>
            left.CompareTo(right) <= 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is
        /// strictly later than the right one.
        /// </summary>
        public static bool operator >(CalendarWeek left, CalendarWeek right) =>
            left.CompareTo(right) > 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is
        /// later than or equal to the right one.
        /// </summary>
        public static bool operator >=(CalendarWeek left, CalendarWeek right) =>
            left.CompareTo(right) >= 0;

        /// <summary>
        /// Obtains the earlier date of two specified dates.
        /// </summary>
        [Pure]
        public static CalendarWeek Min(CalendarWeek left, CalendarWeek right) =>
            left < right ? left : right;

        /// <summary>
        /// Obtains the later date of two specified dates.
        /// </summary>
        [Pure]
        public static CalendarWeek Max(CalendarWeek left, CalendarWeek right) =>
            left > right ? left : right;

        /// <summary>
        /// Indicates whether this instance is earlier, later or the same as the
        /// specified one.
        /// </summary>
        [Pure]
        public int CompareTo(CalendarWeek other)
        {
            ValidateCuid(other.Cuid, nameof(other));

            return Parts.CompareTo(other.Parts);
        }

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is CalendarWeek week ? CompareTo(week)
            : Throw.NonComparable(typeof(CalendarWeek), obj);
    }
}
