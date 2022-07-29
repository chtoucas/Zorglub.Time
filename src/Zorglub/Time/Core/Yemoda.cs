// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using System.ComponentModel;

    using Zorglub.Time.Core.Intervals;

    // TODO(doc): to be updated, default value and IMinMaxValue<>.
    // Default value: when used as a binary repr for a struct date type, one
    // would expect that default(Yemoda) = epoch that is 01/01/0001.
    // With default(Yemoda) = 01/01/0000, Yemoda can no longer be used with
    // a calendar supporting only dates >= epoch. In other words, it puts an
    // unusual constraint on the calendar scope.

    #region Developer Notes

    // Yemoda = YEar-MOnth-DAy
    //
    // We pack the date parts into a single 32-bit word.
    // From left to right, we have
    // Year: 22-bit two's complement signed integer.
    // Month: 4-bit unsigned integer (or rather Month - 1).
    //   Ne convient pas aux calendriers qui ne rendent pas compte des cycles
    //   lunaires et peuvent donc utiliser des cycles de mois plus longs, comme
    //   par exemple le calendrier Maya qui utilise 20 mois.
    // Day: 6-bit unsigned integer (or rather Day - 1).
    //   35 jours nécessaires pour pouvoir représenter les calendriers égyptien,
    //   copte, etc. (à moins d'utiliser un treizième mois virtuel).
    // The order is important, first Year then Month and Day, ie the signed
    // integer must stay on the left. It is also a requirement for the
    // comparison operators to work correctly.
    //
    // On a related matter, Yemoda uses the lexicographic order. This happens to
    // be the right order for most calendars. If a calendar does not agree with
    // that, it should not be too hard to make the necessary adaptations
    // (mostly date objects, math ops).
    //
    // Month and Day start at 1, otherwise it will break many things (for
    // instance, we assume that the last day of a month matches the number of
    // days in the month).
    //
    // To ensure that default(Yemoda) returns a well-formed "date", we
    // substract 1 to both Day and Month; it represents the 01/01/0000. Notice
    // the year 0. This might be problematic for real date structs for which the
    // year 0 is not valid. The binary value of the theoretical zero (01/01/0001,
    // see StartOfYear1) is not equal to zero but to (1 << 11) = 2048. Read also
    // the comments further below; see the method Pack().
    //
    // WARNING: do not use (y - 1) to ensure that default(Yemoda) =
    // 01/01/0001. This will increase the code size of Unpack() and
    // reduces the chances for it to be inlined. Anyway, even if
    // 01/01/0001 has a special meaning, it is no better than
    // 01/01/0000 as a default value --- the epoch is not even
    // considered valid in some calendars, it marks a theoretical
    // origin, not the first genuine date from which the calendar
    // has been in use. This would have been a different
    // story if the year zero was not legal, but most calendars in
    // this project being proleptic, I stick with default(Yemoda) =
    // 01/01/0000. See CivilDate for an example where the year 0 is
    // not legal.
    //
    // We do NOT inherit IMinMaxValue<>: comparison is not calendrical which
    // means that most values are invalid in the context they are meant to be
    // used, that is a calendar.

    #endregion

    /// <summary>
    /// Provides a compact representation of a year, a month and a day.
    /// <para><see cref="Yemoda"/> is an immutable struct.</para>
    /// </summary>
    /// <remarks>
    /// <para>This type uses the lexicographic order on triples (Year, Month, Day).</para>
    /// <para>A <see cref="Yemoda"/> value is NOT a date since it is not attached to any calendar.
    /// As such, comparison between two instances is NOT calendrical. Still, it uses an order that
    /// most of the time matches the calendrical one.</para>
    /// </remarks>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly partial struct Yemoda :
        ISerializable<Yemoda, int>,
        IComparisonOperators<Yemoda, Yemoda>,
        IMinMaxValue<Yemoda>
    {
        #region Bit settings

        /// <summary>
        /// <see cref="Year"/> is a 22-bit two's complement signed integer.
        /// <para>This field is a constant equal to 22.</para>
        /// </summary>
        internal const int YearBits = 32 - MonthBits - DayBits;

        /// <summary>
        /// <see cref="Month"/> (minus 1) is a 4-bit unsigned integer.
        /// <para>This field is a constant equal to 4.</para>
        /// </summary>
        internal const int MonthBits = 4;

        /// <summary>
        /// <see cref="Day"/> (minus 1) is a 6-bit unsigned integer.
        /// <para>This field is a constant equal to 6.</para>
        /// </summary>
        internal const int DayBits = 6;

        //
        // Shifts.
        //

        /// <summary>This field is a constant equal to 6.</summary>
        internal const int MonthShift = DayBits;

        /// <summary>This field is a constant equal to 10.</summary>
        internal const int YearShift = MonthShift + MonthBits;

        //
        // Masks.
        //

        /// <summary>This field is a constant equal to 63.</summary>
        // _bin & DayMask efface tous les bits dans les positions > 6 :
        //   0000 0000 0000 0000 0000 0000 0011 1111    DayMask
        // car
        //   0000 0000 0000 0000 0000 0000 0100 0000    1 << DayMask = 1 << 6
        // Pour effacer tous les bits dans les positions <= 6, on prend plutôt
        //   1111 1111 1111 1111 1111 1111 1100 0000    ~DayMask
        internal const int DayMask = (1 << DayBits) - 1;

        /// <summary>This field is a constant equal to 15.</summary>
        // Instead of
        //   Month = (_bin >> MonthShift) & MonthMask
        // if we wanted:
        //   Month = (_bin & MonthMask) >> MonthShift
        // we would have set:
        //   MonthMask = ((1 << MonthBits) - 1) << DayBits
        // NB: MonthMask = 1984.
        internal const int MonthMask = (1 << MonthBits) - 1;

        /// <summary>This field is a constant equal to -64.</summary>
        // Pour "effacer" le champs jour (Day = 1) :
        //   1111 1111 1111 1111 1111 1111 1100 0000
        internal const int UnsetDayMask = ~((1 << MonthShift) - 1);

        /// <summary>This field is a constant equal to -1024.</summary>
        // Pour "effacer" les champs jour et mois (Month = Day = 1) :
        //   1111 1111 1111 1111 1111 1000 0000 0000
        internal const int UnsetMonthDayMask = ~((1 << YearShift) - 1);

        #endregion

        /// <summary>
        /// Represents the absolute minimum value for <see cref="Year"/>.
        /// <para>This field is a constant equal to -2_097_151 (= 1 - 2^21).</para>
        /// </summary>
        public const int MinYear = 1 - (1 << (YearBits - 1));

        /// <summary>
        /// Represents the absolute maximum value for <see cref="Year"/>.
        /// <para>This field is a constant equal to 2_097_152 (= 2^21).</para>
        /// </summary>
        public const int MaxYear = 1 << (YearBits - 1);

        /// <summary>
        /// Represents the absolute minimum value for <see cref="Month"/>.
        /// <para>This field is a constant equal to 1.</para>
        /// </summary>
        public const int MinMonth = 1;

        /// <summary>
        /// Represents the absolute maximum value for <see cref="Month"/>.
        /// <para>This field is a constant equal to 16 (= 2^4).</para>
        /// </summary>
        public const int MaxMonth = 1 << MonthBits;

        /// <summary>
        /// Represents the absolute minimum value for <see cref="Day"/>
        /// <para>This field is a constant equal to 1.</para>
        /// </summary>
        public const int MinDay = 1;

        /// <summary>
        /// Represents the absolute maximum value for <see cref="Day"/>
        /// <para>This field is a constant equal to 64 (= 2^6).</para>
        /// </summary>
        public const int MaxDay = 1 << DayBits;

        /// <summary>
        /// Represents the binary data stored in this instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The data is organised as follows:
        /// <code><![CDATA[
        ///   Year      bbbb bbbb bbbb bbbb bbbb bb
        ///   Month - 1                            bb bb
        ///   Day - 1                                   bb bbbb
        /// ]]></code>
        /// </para>
        /// </remarks>
        private readonly int _bin;

        /// <summary>
        /// Initializes a new instance of the <see cref="Yemoda"/> struct from the specified year,
        /// month and day.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal Yemoda(int y, int m, int d)
        {
            Debug.Assert(MinYear <= y);
            Debug.Assert(y <= MaxYear);
            Debug.Assert(MinMonth <= m);
            Debug.Assert(m <= MaxMonth);
            Debug.Assert(MinDay <= d);
            Debug.Assert(d <= MaxDay);

            _bin = Pack(y, m, d);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Yemoda"/> struct directly from the
        /// specified binary data.
        /// <para>This constructor does NOT validate its parameter.</para>
        /// </summary>
        internal Yemoda(int bin)
        {
            _bin = bin;
        }

        /// <summary>
        /// Gets the smallest possible value of a <see cref="Yemoda"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static Yemoda MinValue { get; } = new(MinYear, MinMonth, MinDay);

        /// <summary>
        /// Gets the largest possible value of a <see cref="Yemoda"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static Yemoda MaxValue { get; } = new(MaxYear, MaxMonth, MaxDay);

        /// <summary>
        /// Gets the interval [<see cref="MinYear"/>..<see cref="MaxYear"/>].
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static Range<int> SupportedYears { get; } = new(MinYear, MaxYear);

        /// <summary>
        /// Gets the algebraic year from this instance.
        /// </summary>
        public int Year => unchecked(1 + (_bin >> YearShift));

        /// <summary>
        /// Gets the month of the year from this instance.
        /// </summary>
        public int Month => unchecked(1 + ((_bin >> MonthShift) & MonthMask));

        /// <summary>
        /// Gets the day of the month from this instance.
        /// </summary>
        public int Day => unchecked(1 + (_bin & DayMask));

        /// <summary>
        /// Gets the date parts for the first day of the year to which belongs this instance.
        /// </summary>
        public Yemoda StartOfYear => new(unchecked(_bin & UnsetMonthDayMask));

        /// <summary>
        /// Gets the date parts for the first day of the month to which belongs this instance.
        /// </summary>
        public Yemoda StartOfMonth => new(unchecked(_bin & UnsetDayMask));

        /// <summary>
        /// Gets the month parts from this instance.
        /// </summary>
        public Yemo Yemo => new(unchecked(_bin & UnsetDayMask));

        /// <summary>
        /// Gets the string to display in the debugger watch window.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string DebuggerDisplay => Convert.ToString(_bin, 2);

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString()
        {
            Unpack(out int y, out int m, out int d);
            return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4}");
        }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int month, out int day) =>
            Unpack(out year, out month, out day);

        /// <summary>
        /// Creates a new instance of <see cref="Yemoda"/> from the specified year, month and day.
        /// </summary>
        /// <exception cref="AoorException">The specified triple is not representable; one of the
        /// value is too large to be handled by the system.</exception>
        [Pure]
        public static Yemoda Create(int year, int month, int day)
        {
            if (year < MinYear || year > MaxYear) Throw.YearOutOfRange(year);
            if (month < MinMonth || month > MaxMonth) Throw.MonthOutOfRange(month);
            if (day < MinDay || day > MaxDay) Throw.DayOutOfRange(day);

            return new Yemoda(Pack(year, month, day));
        }

        /// <summary>
        /// Creates a new instance of <see cref="Yemoda"/> representing the first day of the
        /// specified year.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        [Pure]
        // CIL code size = 12 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Yemoda AtStartOfYear(int y)
        {
            Debug.Assert(MinYear <= y);
            Debug.Assert(y <= MaxYear);

            return new Yemoda(unchecked((y - 1) << YearShift));
        }

        /// <summary>
        /// Creates a new instance of <see cref="Yemoda"/> representing the first day of the
        /// specified month.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        [Pure]
        // CIL code size = 18 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Yemoda AtStartOfMonth(int y, int m)
        {
            Debug.Assert(MinYear <= y);
            Debug.Assert(y <= MaxYear);
            Debug.Assert(MinMonth <= m);
            Debug.Assert(m <= MaxMonth);

            return new Yemoda(unchecked(((y - 1) << YearShift) | ((m - 1) << MonthShift)));
        }
    }

    public partial struct Yemoda // Binary data helpers
    {
        /// <summary>
        /// Deserializes a 32-bit binary value and recreates an original serialized
        /// <see cref="Yemoda"/> object.
        /// </summary>
        [Pure]
        public static Yemoda FromBinary(int data) => new(data);

        // REVIEW(api): ext. ser. w/ other data types.

        /// <summary>
        /// Deserializes a 64-bit binary value and recreates an original serialized
        /// <see cref="Yemoda"/> object.
        /// </summary>
        [Pure]
        [CLSCompliant(false)]
        public static Yemoda FromBinary(long data, out uint extraData)
        {
            const long Mask = (1L << 32) - 1;

            checked
            {
                extraData = (uint)(data & Mask);
                return FromBinary((int)(data >> 32));
            }
        }

        /// <summary>
        /// Serializes the current <see cref="Yemoda"/> object to a 32-bit binary value that
        /// subsequently can be used to recreate the <see cref="Yemoda"/> object.
        /// </summary>
        [Pure]
        public int ToBinary() => _bin;

        /// <summary>
        /// Serializes the current <see cref="Yemoda"/> object to a 64-bit binary value that
        /// subsequently can be used to recreate the <see cref="Yemoda"/> object.
        /// </summary>
        [Pure]
        [CLSCompliant(false)]
        public long ToBinary(uint extraData)
        {
            if (extraData > Int32.MaxValue) Throw.ArgumentOutOfRange(nameof(extraData));

            return ((long)ToBinary() << 32) | extraData;
        }

        /// <summary>
        /// Packs the specified date parts into a single 32-bit word.
        /// </summary>
        [Pure]
        // CIL code size = 17 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Pack(int y, int m, int d)
        {
            unchecked
            {
                return ((y - 1) << YearShift) | ((m - 1) << MonthShift) | (d - 1);
            }
        }

        /// <summary>
        /// Unpacks the binary data.
        /// </summary>
        // CIL code size = 34 bytes > 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Unpack(out int y, out int m, out int d)
        {
            // Perf: local copy of the field _bin.
            int bin = _bin;

            unchecked
            {
                y = 1 + (bin >> YearShift);
                m = 1 + ((bin >> MonthShift) & MonthMask);
                d = 1 + (bin & DayMask);
            }
        }

        /// <summary>
        /// Unpacks the binary data.
        /// </summary>
        // CIL code size = 26 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Unpack(out int y, out int m)
        {
            // Perf: local copy of the field _bin.
            int bin = _bin;

            unchecked
            {
                y = 1 + (bin >> YearShift);
                m = 1 + ((bin >> MonthShift) & MonthMask);
            }
        }
    }

    public partial struct Yemoda // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="Yemoda"/> are equal.
        /// </summary>
        public static bool operator ==(Yemoda left, Yemoda right) => left._bin == right._bin;

        /// <summary>
        /// Determines whether two specified instances of <see cref="Yemoda"/> are not equal.
        /// </summary>
        public static bool operator !=(Yemoda left, Yemoda right) => left._bin != right._bin;

        /// <inheritdoc />
        [Pure]
        public bool Equals(Yemoda other) => _bin == other._bin;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Yemoda parts && Equals(parts);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => _bin;
    }

    public partial struct Yemoda // IComparable
    {
        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on triples (Year, Month, Day).
        /// </para>
        /// </remarks>
        public static bool operator <(Yemoda left, Yemoda right) => left._bin < right._bin;

        /// <summary>
        /// Compares the two specified instances to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on triples (Year, Month, Day).
        /// </para>
        /// </remarks>
        public static bool operator <=(Yemoda left, Yemoda right) => left._bin <= right._bin;

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on triples (Year, Month, Day).
        /// </para>
        /// </remarks>
        public static bool operator >(Yemoda left, Yemoda right) => left._bin > right._bin;

        /// <summary>
        /// Compares the two specified instances to see if the left one is later than or equal to
        /// the right one.
        /// </summary>
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on triples (Year, Month, Day).
        /// </para>
        /// </remarks>
        public static bool operator >=(Yemoda left, Yemoda right) => left._bin >= right._bin;

        /// <inheritdoc />
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on triples (Year, Month, Day).
        /// </para>
        /// </remarks>
        [Pure]
        public int CompareTo(Yemoda other) => _bin.CompareTo(other._bin);

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is Yemoda ymd ? CompareTo(ymd)
            : Throw.NonComparable(typeof(Yemoda), obj);
    }
}
