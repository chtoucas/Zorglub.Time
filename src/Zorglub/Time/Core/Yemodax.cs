// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using System.ComponentModel;

    /// <summary>
    /// Provides a compact representation of a year, a month, a day and an unspecified optional
    /// value.
    /// <para><see cref="Yemodax"/> is an immutable struct.</para>
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly partial struct Yemodax :
        IEqualityOperators<Yemodax, Yemodax>,
        IMinMaxValue<Yemodax>
    {
        #region Bit settings

        /// <summary>
        /// Short <see cref="Year"/> is a 15-bit two's complement signed integer.
        /// <para>This field is a constant equal to 15.</para>
        /// </summary>
        internal const int YearBits = 15;

        /// <summary>
        /// <see cref="Extra"/> is a 7-bit unsigned integer.
        /// <para>This field is a constant equal to 7.</para>
        /// </summary>
        internal const int ExtraBits = 32 - YearBits - Yemoda.MonthBits - Yemoda.DayBits;

        //
        // Shifts.
        //

        /// <summary>This field is a constant equal to 7.</summary>
        internal const int DayShift = ExtraBits;

        /// <summary>This field is a constant equal to 13.</summary>
        internal const int MonthShift = DayShift + Yemoda.DayBits;

        /// <summary>This field is a constant equal to 17.</summary>
        internal const int YearShift = MonthShift + Yemoda.MonthBits;

        //
        // Masks.
        //

        /// <summary>This field is a constant equal to 127.</summary>
        internal const int ExtraMask = (1 << ExtraBits) - 1;

        /// <summary>This field is a constant equal to 63.</summary>
        internal const int DayMask = (1 << Yemoda.DayBits) - 1;

        /// <summary>This field is a constant equal to 15.</summary>
        internal const int MonthMask = (1 << Yemoda.MonthBits) - 1;

        #endregion

        /// <summary>
        /// Represents the absolute minimum value for <see cref="Year"/>.
        /// <para>This field is a constant equal to -16_384 (= -2^14).</para>
        /// </summary>
        public const int MinYear = -(1 << (YearBits - 1));

        /// <summary>
        /// Represents the absolute maximum value for <see cref="Year"/>.
        /// <para>This field is a constant equal to 16_383 (= 2^14 - 1).</para>
        /// </summary>
        public const int MaxYear = (1 << (YearBits - 1)) - 1;

        /// <summary>
        /// Represents the absolute minimum value for <see cref="Month"/>.
        /// <para>This field is a constant equal to 1.</para>
        /// </summary>
        public const int MinMonth = Yemoda.MinMonth;

        /// <summary>
        /// Represents the absolute maximum value for <see cref="Month"/>.
        /// <para>This field is a constant equal to 16.</para>
        /// </summary>
        public const int MaxMonth = Yemoda.MaxMonth;

        /// <summary>
        /// Represents the absolute minimum value for <see cref="Day"/>
        /// <para>This field is a constant equal to 1.</para>
        /// </summary>
        public const int MinDay = Yemoda.MinDay;

        /// <summary>
        /// Represents the absolute maximum value for <see cref="Day"/>
        /// <para>This field is a constant equal to 64.</para>
        /// </summary>
        public const int MaxDay = Yemoda.MaxDay;

        /// <summary>
        /// Represents the absolute minimum value for <see cref="Extra"/>.
        /// <para>This field is a constant equal to 0.</para>
        /// </summary>
        public const int MinExtra = 0;

        /// <summary>
        /// Represents the absolute maximum value for <see cref="Extra"/>.
        /// <para>This field is a constant equal to 127 (= 2^7 - 1).</para>
        /// </summary>
        public const int MaxExtra = (1 << ExtraBits) - 1;

        /// <summary>
        /// Represents the smallest possible value of a <see cref="Yemodax"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        internal static readonly Yemodax MinValue = new(MinYear, MinMonth, MinDay, MinExtra);

        /// <summary>
        /// Represents the largest possible value of a <see cref="Yemodax"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        internal static readonly Yemodax MaxValue = new(MaxYear, MaxMonth, MaxDay, MaxExtra);

        /// <summary>
        /// Represents the binary data stored in this instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The data is organised as follows:
        /// <code><![CDATA[
        ///   Year      bbbb bbbb bbbb bbb
        ///   Month - 1                   b bbb
        ///   Day - 1                          b bbbb b
        ///   Extra                                    bbb bbbb
        /// ]]></code>
        /// </para>
        /// </remarks>
        private readonly int _bin;

        /// <summary>
        /// Initializes a new instance of the <see cref="Yemodax"/> struct.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal Yemodax(int y, int m, int d, int x)
        {
            Debug.Assert(MinYear <= y);
            Debug.Assert(y <= MaxYear);
            Debug.Assert(MinMonth <= m);
            Debug.Assert(m <= MaxMonth);
            Debug.Assert(MinDay <= d);
            Debug.Assert(d <= MaxDay);
            Debug.Assert(MinExtra <= x);
            Debug.Assert(x <= MaxExtra);

            _bin = Pack(y, m, d, x);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Yemodax"/> struct.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal Yemodax(Yemoda ymd, int x)
        {
            Debug.Assert(MinYear <= ymd.Year);
            Debug.Assert(ymd.Year <= MaxYear);
            Debug.Assert(MinExtra <= x);
            Debug.Assert(x <= MaxExtra);

            _bin = (ymd.ToBinary() << ExtraBits) | x;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Yemodax"/> struct.
        /// <para>This constructor does NOT validate its parameter.</para>
        /// </summary>
        internal Yemodax(int bin)
        {
            _bin = bin;
        }

        static Yemodax IMinMaxValue<Yemodax>.MinValue => MinValue;
        static Yemodax IMinMaxValue<Yemodax>.MaxValue => MaxValue;

        /// <summary>
        /// Gets the algebraic year from this instance.
        /// </summary>
        public int Year => unchecked(_bin >> YearShift);

        /// <summary>
        /// Gets the month of the year from this instance.
        /// </summary>
        public int Month => unchecked(1 + ((_bin >> MonthShift) & MonthMask));

        /// <summary>
        /// Gets the day of the month from this instance.
        /// </summary>
        public int Day => unchecked(1 + ((_bin >> DayShift) & DayMask));

        /// <summary>
        /// Gets the unspecified complementary value from this instance.
        /// </summary>
        public int Extra => unchecked(_bin & ExtraMask);

        /// <summary>
        /// Gets the date parts from this instance.
        /// </summary>
        public Yemoda Yemoda => new(unchecked(_bin >> ExtraBits));

        /// <summary>
        /// Gets the month parts from this instance.
        /// </summary>
        public Yemo Yemo => new(unchecked((_bin >> ExtraBits) & Yemoda.UnsetDayMask));

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
            Deconstruct(out int y, out int m, out int d);
            return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4}");
        }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int month, out int day)
        {
            // Perf: local copy of the field _bin.
            int bin = _bin;

            unchecked
            {
                year = bin >> YearShift;
                month = 1 + ((bin >> MonthShift) & MonthMask);
                day = 1 + ((bin >> DayShift) & DayMask);
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="Yemodax"/>.
        /// </summary>
        /// <exception cref="AoorException">The specified values are not representable; one of the
        /// value is too large to be handled by the system.</exception>
        [Pure]
        public static Yemodax Create(int year, int month, int day, int extra)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year);
            }
            if (month < MinMonth || month > MaxMonth)
            {
                Throw.MonthOutOfRange(month);
            }
            if (day < MinDay || day > MaxDay)
            {
                Throw.DayOutOfRange(day);
            }
            if (extra < MinExtra || extra > MaxExtra)
            {
                Throw.ArgumentOutOfRange(nameof(extra));
            }

            return new Yemodax(Pack(year, month, day, extra));
        }

        /// <summary>
        /// Creates a new instance of <see cref="Yemodax"/>.
        /// </summary>
        /// <exception cref="AoorException">The specified values are not representable; one of the
        /// value is too large to be handled by the system.</exception>
        [Pure]
        public static Yemodax Create(Yemoda ymd, int extra)
        {
            if (ymd.Year < MinYear || ymd.Year > MaxYear)
            {
                Throw.YearOutOfRange(ymd.Year, nameof(ymd));
            }
            if (extra < MinExtra || extra > MaxExtra)
            {
                Throw.ArgumentOutOfRange(nameof(extra));
            }

            return new Yemodax(ymd, extra);
        }
    }

    public partial struct Yemodax // Binary data helpers
    {
        /// <summary>
        /// Deserializes a 32-bit binary value and recreates an original serialized
        /// <see cref="Yemodax"/> object.
        /// </summary>
        [Pure]
        public static Yemodax FromBinary(int data) => new(data);

        /// <summary>
        /// Serializes the current <see cref="Yemodax"/> object to a 32-bit binary value that
        /// subsequently can be used to recreate the <see cref="Yemodax"/> object.
        /// </summary>
        [Pure]
        public int ToBinary() => _bin;

        /// <summary>
        /// Packs the specified time parts into a single 32-bit word.
        /// </summary>
        [Pure]
        // CIL code size = 20 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Pack(int y, int m, int d, int x)
        {
            unchecked
            {
                return (y << YearShift) | ((m - 1) << MonthShift) | ((d - 1) << DayShift) | x;
            }
        }

        // No method Unpack(y, m, d): 35 bytes. Use Deconstruct() instead.

        /// <summary>
        /// Unpacks the binary data.
        /// </summary>
        // CIL code size = 25 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Unpack(out int y, out int m)
        {
            // Perf: local copy of the field _bin.
            int bin = _bin;

            unchecked
            {
                y = bin >> YearShift;
                m = 1 + ((bin >> MonthShift) & MonthMask);
            }
        }
    }

    public partial struct Yemodax // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="Yemodax"/> are equal.
        /// </summary>
        public static bool operator ==(Yemodax left, Yemodax right) => left._bin == right._bin;

        /// <summary>
        /// Determines whether two specified instances of <see cref="Yemodax"/> are not equal.
        /// </summary>
        public static bool operator !=(Yemodax left, Yemodax right) => left._bin != right._bin;

        /// <inheritdoc />
        [Pure]
        public bool Equals(Yemodax other) => _bin == other._bin;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Yemodax parts && Equals(parts);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => _bin;
    }
}
