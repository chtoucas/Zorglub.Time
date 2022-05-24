// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Collections;

    using Zorglub.Time.Hemerology;

    /// <summary>
    /// Represents a range of consecutive days that is a finite and closed
    /// interval between two given dates.
    /// <para>This class can ONLY be inherited from within friend assemblies.</para>
    /// <para>This type follows the rules of structural equality.</para>
    /// </summary>
    [Obsolete("Use Range<CalendarDate> instead.")]
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "Interval first.")]
    public partial class DateRangeV0 :
        IDateRange<DateRangeV0, CalendarDate>,
        IEquatable<DateRangeV0>,
        IEnumerable<CalendarDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangeV0"/> class.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        private protected DateRangeV0(CalendarDate start, CalendarDate end, int length)
        {
            Debug.Assert(start <= end);
            Debug.Assert(length >= 1);
            Debug.Assert(length == 1 + (end - start));

            Start = start;
            End = end;
            Length = length;

            Cuid = start.Cuid;
            Calendar = start.Calendar;
        }

        /// <inheritdoc />
        public CalendarDate Start { get; }

        /// <inheritdoc />
        public CalendarDate End { get; }

        /// <inheritdoc />
        public int Length { get; }

        /// <summary>
        /// Gets the calendar to which belongs the current instance.
        /// </summary>
        public Calendar Calendar { get; }

        /// <summary>
        /// Gets the ID of the calendar to which belongs the current
        /// instance.
        /// </summary>
        private protected Cuid Cuid { get; }

        /// <summary>
        /// Returns a culture-independent string representation of the current
        /// instance.
        /// </summary>
        [Pure]
        public override string ToString() =>
            FormattableString.Invariant($"[{Start}, {End}] ({Calendar})");

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out CalendarDate start, out CalendarDate end) =>
            (start, end) = (Start, End);

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out CalendarDate start, out CalendarDate end, out int length) =>
            (start, end, length) = (Start, End, Length);

        private void ValidateCuid(Cuid cuid, string paramName)
        {
            if (cuid != Cuid) Throw.BadCuid(paramName, Cuid, cuid);
        }
    }

    public partial class DateRangeV0 // Factories
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DateRangeV0"/> class from the
        /// specified start and end.
        /// </summary>
        [Pure]
        public static DateRangeV0 Create(CalendarDate start, CalendarDate end)
        {
            if (end < start) Throw.ArgumentOutOfRange(nameof(end));
            return CreateCore(start, end);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DateRangeV0"/> class from the
        /// specified start and length.
        /// </summary>
        [Pure]
        public static DateRangeV0 Create(CalendarDate start, int length)
        {
            if (length < 1) Throw.ArgumentOutOfRange(nameof(length));
            return CreateCore(start, start + (length - 1), length);
        }

        [Pure]
        private static DateRangeV0 CreateCore(CalendarDate start, CalendarDate end) =>
            CreateCore(start, end, 1 + (end - start));

        [Pure]
        private static DateRangeV0 CreateCore(CalendarDate start, CalendarDate end, int length)
        {
            start.Parts.Unpack(out int y0, out int m0);
            end.Parts.Unpack(out int y, out int m);

            return y == y0
                ? m == m0
                    ? new WithinMonth(start, end, length)
                    : new WithinYear(start, end, length)
                : new DateRangeV0(start, end, length);
        }
    }

    public partial class DateRangeV0 // Set-theoretical operations
    {
        /// <inheritdoc />
        [Pure]
        public bool Contains(CalendarDate date)
        {
            ValidateCuid(date.Cuid, nameof(date));

            return ContainsCore(date);
        }

        /// <summary>
        /// Determines whether this range instance contains the specified month or not.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="month"/> does
        /// not to the calendar of the current instance.</exception>
        [Pure]
        public bool Contains(CalendarMonth month)
        {
            ValidateCuid(month.Cuid, nameof(month));

            return ContainsCore(month);
        }

        /// <summary>
        /// Determines whether this range instance contains the specified year or not.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="year"/> does
        /// not to the calendar of the current instance.</exception>
        [Pure]
        public bool Contains(CalendarYear year)
        {
            ValidateCuid(year.Cuid, nameof(year));

            return ContainsCore(year);
        }

        /// <inheritdoc />
        [Pure]
        public bool IsSupersetOf(DateRangeV0 range)
        {
            Requires.NotNull(range);
            ValidateCuid(range.Cuid, nameof(range));

            return IsSupersetOfCore(range);
        }

        [Pure]
        private bool ContainsCore(CalendarDate date)
        {
            Debug.Assert(date.Cuid == Cuid);

            // CompareFast() to avoid double checking the Cuid.
            return Start.CompareFast(date) <= 0 && date.CompareFast(End) <= 0;
        }

        [Pure]
        private protected virtual bool ContainsCore(CalendarMonth month)
        {
            Debug.Assert(month.Cuid == Cuid);

            var startOfMonth = month.FirstDay;
            var endOfMonth = month.LastDay;
            return Start.CompareFast(startOfMonth) <= 0 && endOfMonth.CompareFast(End) <= 0;
        }

        [Pure]
        private protected virtual bool ContainsCore(CalendarYear year)
        {
            Debug.Assert(year.Cuid == Cuid);

            var startOfYear = year.FirstDay.ToCalendarDate();
            var endOfYear = year.LastDay.ToCalendarDate();
            return Start.CompareFast(startOfYear) <= 0 && endOfYear.CompareFast(End) <= 0;
        }

        [Pure]
        private bool IsSupersetOfCore(DateRangeV0 range)
        {
            Debug.Assert(range != null);
            Debug.Assert(range.Cuid == Cuid);

            return Start.CompareFast(range.Start) <= 0 && range.End.CompareFast(End) <= 0;
        }
    }

    public partial class DateRangeV0 // Other set ops, conversions
    {
        /// <inheritdoc />
        [Pure]
        public DateRangeV0? Intersect(DateRangeV0 range)
        {
            Requires.NotNull(range);
            ValidateCuid(range.Cuid, nameof(range));

            return IsSupersetOfCore(range) ? range
                : range.IsSupersetOfCore(this) ? this
                : range.ContainsCore(Start) ? CreateCore(Start, range.End)
                : range.ContainsCore(End) ? CreateCore(range.Start, End)
                : null;
        }

        /// <inheritdoc />
        [Pure]
        public DateRangeV0? Union(DateRangeV0 range)
        {
            Requires.NotNull(range);

            // NB: no need to check the Cuid's, CalendarDate.Min/Max will do the
            // job for us.

            var start = CalendarDate.Min(Start, range.Start);
            var end = CalendarDate.Max(End, range.End);
            int length = 1 + (end - start);

            return length > Length + range.Length ? null
                : CreateCore(start, end, length);
        }

        /// <summary>
        /// Interconverts the current instance to a range within a different calendar.
        /// </summary>
        /// <remarks>
        /// <para>This method always performs the conversion whether it's necessary or not. To avoid
        /// an expensive operation, it's better to check before that <paramref name="newCalendar"/>
        /// is actually different from the calendar of the current instance.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="newCalendar"/> is null.
        /// </exception>
        [Pure]
        public DateRangeV0 WithCalendar(Calendar newCalendar)
        {
            Requires.NotNull(newCalendar);

            var start = Calendar.GetDayNumber(Start);
            var end = Calendar.GetDayNumber(End);

            return CreateCore(
                newCalendar.GetCalendarDateOn(start),
                newCalendar.GetCalendarDateOn(end),
                Length);
        }
    }

    public partial class DateRangeV0 // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="DateRangeV0"/>
        /// are equal.
        /// </summary>
        public static bool operator ==(DateRangeV0? left, DateRangeV0? right) =>
            left is null ? right is null : left.Equals(right);

        /// <summary>
        /// Determines whether two specified instances of <see cref="DateRangeV0"/>
        /// are not equal.
        /// </summary>
        public static bool operator !=(DateRangeV0? left, DateRangeV0? right) =>
            left is null ? right is not null : !left.Equals(right);

        /// <inheritdoc />
        [Pure]
        public bool Equals(DateRangeV0? other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }
            return Start == other.Start && End == other.End;
        }

        /// <inheritdoc />
        [Pure] public override bool Equals(object? obj) => Equals(obj as DateRangeV0);

        /// <inheritdoc />
        [Pure] public override int GetHashCode() => HashCode.Combine(Start, End);
    }

    public partial class DateRangeV0 // IEnumerable
    {
        /// <summary>
        /// Obtains an enumerator for the dates in this range instance, borders
        /// included.
        /// </summary>
        [Pure]
        public virtual IEnumerator<CalendarDate> GetEnumerator()
        {
            var date = Start;

            int length = Length;
            for (int i = 1; i < length; i++)
            {
                yield return date;
                date = date.NextDay();
            }
            yield return date;
        }

        /// <inheritdoc/>
        [Pure] IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public partial class DateRangeV0 // Within the boundaries of a year
    {
        /// <summary>
        /// Converts the specified year to a range of days.
        /// </summary>
        [Pure]
        public static DateRangeV0 FromYear(CalendarYear year) => new DaysInYear(year);

        // Intervalle confiné dans une année.
        private class WithinYear : DateRangeV0
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="WithinYear"/> class.
            /// <para>This constructor does NOT validate its parameters.</para>
            /// </summary>
            public WithinYear(CalendarDate start, CalendarDate end, int length)
                : base(start, end, length)
            {
                Debug.Assert(start.Year == end.Year);
            }

            /// <inheritdoc/>
            [Pure]
            public override IEnumerator<CalendarDate> GetEnumerator()
            {
                var sch = Calendar.Schema;
                int y = Start.Year;

                int first = Start.DayOfYear;
                int last = first + Length - 1;
                for (int doy = first; doy <= last; doy++)
                {
                    var ymd = sch.GetDateParts(y, doy);
                    yield return new CalendarDate(ymd, Cuid);
                }
            }
        }

        private sealed class DaysInYear : WithinYear
        {
            private readonly CalendarYear _year;

            public DaysInYear(CalendarYear year)
                : base(
                      year.FirstDay.ToCalendarDate(),
                      year.LastDay.ToCalendarDate(),
                      year.CountDaysInYear())
            {
                _year = year;
            }

            /// <inheritdoc/>
            [Pure]
            private protected override bool ContainsCore(CalendarMonth month) =>
                month.Year == _year.Year;

            /// <inheritdoc/>
            [Pure]
            private protected override bool ContainsCore(CalendarYear year) => year == _year;

            /// <inheritdoc/>
            [Pure]
            public override IEnumerator<CalendarDate> GetEnumerator()
            {
                int y = _year.Year;
                var sch = _year.Calendar.Schema;

                int monthsInYear = sch.CountMonthsInYear(y);
                for (int m = 1; m <= monthsInYear; m++)
                {
                    int daysInMonth = sch.CountDaysInMonth(y, m);
                    for (int d = 1; d <= daysInMonth; d++)
                    {
                        yield return new CalendarDate(y, m, d, Cuid);
                    }
                }
            }
        }
    }

    public partial class DateRangeV0 // Within the boundaries of a month
    {
        /// <summary>
        /// Converts the specified month to a range of days.
        /// </summary>
        [Pure]
        public static DateRangeV0 FromMonth(CalendarMonth month) => new DaysInMonth(month);

        // Intervalle confiné dans un mois.
        private class WithinMonth : WithinYear
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="WithinMonth"/> class.
            /// <para>This constructor does NOT validate its parameters.</para>
            /// </summary>
            public WithinMonth(CalendarDate start, CalendarDate end, int length)
                : base(start, end, length)
            {
                Debug.Assert(start.Year == end.Year);
                Debug.Assert(start.Month == end.Month);
            }

            /// <inheritdoc/>
            [Pure]
            private protected sealed override bool ContainsCore(CalendarYear year) => false;

            /// <inheritdoc/>
            [Pure]
            public sealed override IEnumerator<CalendarDate> GetEnumerator()
            {
                Start.Parts.Unpack(out int y, out int m);

                int first = Start.Day;
                int last = first + Length - 1;
                for (int d = first; d <= last; d++)
                {
                    yield return new CalendarDate(y, m, d, Cuid);
                }
            }
        }

        private sealed class DaysInMonth : WithinMonth
        {
            private readonly CalendarMonth _month;

            public DaysInMonth(CalendarMonth month)
                : base(
                      month.FirstDay,
                      month.LastDay,
                      month.CountDaysInMonth())
            {
                _month = month;
            }

            /// <inheritdoc/>
            [Pure]
            private protected override bool ContainsCore(CalendarMonth month) => _month == month;
        }
    }
}
