// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using System.ComponentModel;

    // Yewe = YEar-WeekOfYear

    /// <summary>
    /// Provides a compact representation of a year and a week of year.
    /// <para><see cref="Yewe"/> is an immutable struct.</para>
    /// </summary>
    /// <remarks>
    /// <para>This type uses the lexicographic order on pairs (Year, WeekOfYear).</para>
    /// <para>A <see cref="Yewe"/> value is NOT an ordinal date since it is not attached to any
    /// calendar. As such, comparison between two instances is NOT calendrical. Still, it uses an
    /// order that most of the time matches the calendrical one.</para>
    /// </remarks>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly partial struct Yewe : IEquatable<Yewe>, IComparable<Yewe>, IComparable
    {
        #region Bit settings

        /// <summary>
        /// <see cref="WeekOfYear"/> (minus 1) is an 10-bit unsigned integer.
        /// <para>This field is a constant equal to 10.</para>
        /// </summary>
        internal const int WeekOfYearBits = 32 - Yemoda.YearBits;

        /// <summary>This field is a constant equal to 10.</summary>
        private const int YearShift = WeekOfYearBits;

        /// <summary>This field is a constant equal to 1023.</summary>
        internal const int WeekOfYearMask = (1 << WeekOfYearBits) - 1;

        /// <summary>This field is a constant equal to -1024.</summary>
        private const int UnsetWeekOfYearMask = ~((1 << YearShift) - 1);

        #endregion

        /// <summary>
        /// Represents the absolute minimum value for <see cref="Year"/>.
        /// <para>This field is a constant equal to -2_097_151 (= 1 - 2^21).</para>
        /// </summary>
        public const int MinYear = Yemoda.MinYear;

        /// <summary>
        /// Represents the absolute maximum value for <see cref="Year"/>.
        /// <para>This field is a constant equal to 2_097_152 (= 2^21).</para>
        /// </summary>
        public const int MaxYear = Yemoda.MaxYear;

        /// <summary>
        /// Represents the absolute minimum value for <see cref="WeekOfYear"/>.
        /// <para>This field is a constant equal to 1.</para>
        /// </summary>
        public const int MinWeekOfYear = 1;

        /// <summary>
        /// Represents the absolute maximum value for <see cref="WeekOfYear"/>.
        /// <para>This field is a constant equal to 1024 (= 2^10).</para>
        /// </summary>
        public const int MaxWeekOfYear = 1 << WeekOfYearBits;

        /// <summary>
        /// Represents the smallest possible value of a <see cref="Yewe"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        public static readonly Yewe MinValue = new(MinYear, MinWeekOfYear);

        /// <summary>
        /// Represents the largest possible value of a <see cref="Yewe"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        public static readonly Yewe MaxValue = new(MaxYear, MaxWeekOfYear);

        /// <summary>
        /// Represents the binary data stored in this instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The data is organised as follows:
        /// <code><![CDATA[
        ///   Year           bbbb bbbb bbbb bbbb bbbb bb
        ///   WeekOfYear - 1                            bb bbbb bbbb
        /// ]]></code>
        /// </para>
        /// </remarks>
        private readonly int _bin;

        /// <summary>
        /// Initializes a new instance of the <see cref="Yewe"/> struct from the specified year and
        /// week of year.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal Yewe(int y, int woy)
        {
            Debug.Assert(MinYear <= y);
            Debug.Assert(y <= MaxYear);
            Debug.Assert(MinWeekOfYear <= woy);
            Debug.Assert(woy <= MaxWeekOfYear);

            _bin = Pack(y, woy);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Yewe"/> struct directly from the specified
        /// binary data.
        /// <para>This constructor does NOT validate its parameter.</para>
        /// </summary>
        internal Yewe(int bin)
        {
            _bin = bin;
        }

        /// <summary>
        /// Gets the year.
        /// </summary>
        public int Year => unchecked(1 + (_bin >> YearShift));

        /// <summary>
        /// Gets the week of the year.
        /// </summary>
        public int WeekOfYear => unchecked(1 + (_bin & WeekOfYearMask));

        /// <summary>
        /// Gets the ordinal parts for the first week of the year to which belongs this instance.
        /// </summary>
        public Yewe StartOfYear => new(unchecked(_bin & UnsetWeekOfYearMask));

        /// <summary>
        /// Gets the string to display in the debugger watch window.
        /// </summary>
        [ExcludeFromCodeCoverage(Justification = "DebuggerDisplay")]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string DebuggerDisplay => Convert.ToString(_bin, 2);

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString()
        {
            Unpack(out int y, out int w);
            return FormattableString.Invariant($"{w:D3}/{y:D4}");
        }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int weekOfYear) => Unpack(out year, out weekOfYear);

        // TODO: NewYewe()
        // <para>See also <see cref="Validation.ICalendricalScope.NewYewe(int, int)"/>.</para>
        /// <summary>
        /// Creates a new instance of <see cref="Yewe"/> from the specified year and week of year.
        /// </summary>
        /// <exception cref="AoorException">The specified pair is not representable; one of the
        /// value is too large to be handled by the system.</exception>
        [Pure]
        public static Yewe Create(int year, int weekOfYear)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year);
            }
            if (weekOfYear < MinWeekOfYear || weekOfYear > MaxWeekOfYear)
            {
                Throw.ArgumentOutOfRange(nameof(weekOfYear));
            }

            return new Yewe(Pack(year, weekOfYear));
        }

        /// <summary>
        /// Creates a new instance of <see cref="Yewe"/> representing the first week of the
        /// specified year.
        /// <para>This method does NOT validate its parameter.</para>
        /// </summary>
        [Pure]
        internal static Yewe AtStartOfYear(int y)
        {
            Debug.Assert(MinYear <= y);
            Debug.Assert(y <= MaxYear);

            return new Yewe(unchecked(y << YearShift));
        }
    }

    public partial struct Yewe // Binary data helpers
    {
        /// <summary>
        /// Deserializes a 32-bit binary value and recreates an original serialized
        /// <see cref="Yewe"/> object.
        /// </summary>
        [Pure]
        public static Yewe FromBinary(int data) => new(data);

        /// <summary>
        /// Obtains the 32-bit binary value stored in the current instance.
        /// </summary>
        [Pure]
        public int ToBinary() => _bin;

        /// <summary>
        /// Packs the specified ordinal date parts into a single 32-bit word.
        /// </summary>
        [Pure]
        // CIL code size = 9 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Pack(int y, int woy)
        {
            unchecked
            {
                return ((y - 1) << YearShift) | (woy - 1);
            }
        }

        /// <summary>
        /// Unpacks the binary data.
        /// </summary>
        // CIL code size = 25 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Unpack(out int y, out int woy)
        {
            // Perf: local copy of the field _bin.
            int bin = _bin;

            unchecked
            {
                y = 1 + (bin >> YearShift);
                woy = 1 + (bin & WeekOfYearMask);
            }
        }
    }

    public partial struct Yewe // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="Yewe"/> are equal.
        /// </summary>
        public static bool operator ==(Yewe left, Yewe right) => left._bin == right._bin;

        /// <summary>
        /// Determines whether two specified instances of <see cref="Yewe"/> are not equal.
        /// </summary>
        public static bool operator !=(Yewe left, Yewe right) => left._bin != right._bin;

        /// <inheritdoc />
        [Pure]
        public bool Equals(Yewe other) => _bin == other._bin;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Yewe parts && Equals(parts);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => _bin;
    }

    public partial struct Yewe // IComparable
    {
        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on pairs (Year, WeekOfYear).
        /// </para>
        /// </remarks>
        public static bool operator <(Yewe left, Yewe right) => left._bin < right._bin;

        /// <summary>
        /// Compares the two specified instances to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on pairs (Year, WeekOfYear).
        /// </para>
        /// </remarks>
        public static bool operator <=(Yewe left, Yewe right) => left._bin <= right._bin;

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on pairs (Year, WeekOfYear).
        /// </para>
        /// </remarks>
        public static bool operator >(Yewe left, Yewe right) => left._bin > right._bin;

        /// <summary>
        /// Compares the two specified instances to see if the left one is later than or equal to
        /// the right one.
        /// </summary>
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on pairs (Year, WeekOfYear).
        /// </para>
        /// </remarks>
        public static bool operator >=(Yewe left, Yewe right) => left._bin >= right._bin;

        /// <inheritdoc />
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on pairs (Year, WeekOfYear).
        /// </para>
        /// </remarks>
        [Pure]
        public int CompareTo(Yewe other) => _bin.CompareTo(other._bin);

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is Yewe yewe ? CompareTo(yewe)
            : Throw.NonComparable(typeof(Yewe), obj);
    }
}
