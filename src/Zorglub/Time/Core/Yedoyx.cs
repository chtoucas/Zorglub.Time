// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using System.ComponentModel;

    /// <summary>
    /// Provides a compact representation of a year and a day of the year and an unspecified
    /// optional value.
    /// <para><see cref="Yedoyx"/> is an immutable struct.</para>
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly partial struct Yedoyx : IEqualityOperators<Yedoyx, Yedoyx>
    {
        #region Bit settings

        /// <summary>This field is a constant equal to 7.</summary>
        private const int DayOfYearShift = Yemodax.ExtraBits;

        /// <summary>This field is a constant equal to 17.</summary>
        private const int YearShift = DayOfYearShift + Yedoy.DayOfYearBits;

        #endregion

        /// <summary>
        /// Represents the absolute minimum value for <see cref="Year"/>.
        /// <para>This field is a constant equal to -16_384.</para>
        /// </summary>
        public const int MinYear = Yemodax.MinYear;

        /// <summary>
        /// Represents the absolute maximum value for <see cref="Year"/>.
        /// <para>This field is a constant equal to 16_383.</para>
        /// </summary>
        public const int MaxYear = Yemodax.MaxYear;

        /// <summary>
        /// Represents the absolute minimum value for <see cref="DayOfYear"/>.
        /// <para>This field is a constant equal to 1.</para>
        /// </summary>
        public const int MinDayOfYear = Yedoy.MinDayOfYear;

        /// <summary>
        /// Represents the absolute maximum value for <see cref="DayOfYear"/>.
        /// <para>This field is a constant equal to 1024.</para>
        /// </summary>
        public const int MaxDayOfYear = Yedoy.MaxDayOfYear;

        /// <summary>
        /// Represents the absolute minimum value for <see cref="Extra"/>.
        /// <para>This field is a constant equal to 0.</para>
        /// </summary>
        public const int MinExtra = Yemodax.MinExtra;

        /// <summary>
        /// Represents the absolute maximum value for <see cref="Extra"/>.
        /// <para>This field is a constant equal to 127.</para>
        /// </summary>
        public const int MaxExtra = Yemodax.MaxExtra;

        /// <summary>
        /// Represents the smallest possible value of a <see cref="Yedoyx"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        public static readonly Yedoyx MinValue = new(MinYear, MinDayOfYear, MinExtra);

        /// <summary>
        /// Represents the largest possible value of a <see cref="Yedoyx"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        public static readonly Yedoyx MaxValue = new(MaxYear, MaxDayOfYear, MaxExtra);

        /// <summary>
        /// Represents the binary data stored in this instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The data is organised as follows:
        /// <code><![CDATA[
        ///   Year          bbbb bbbb bbbb bbb
        ///   DayOfYear - 1                   b bbbb bbbb b
        ///   Extra                                        bbb bbbb
        /// ]]></code>
        /// </para>
        /// </remarks>
        private readonly int _bin;

        /// <summary>
        /// Initializes a new instance of the <see cref="Yedoyx"/> struct.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal Yedoyx(int y, int doy, int x)
        {
            Debug.Assert(MinYear <= y);
            Debug.Assert(y <= MaxYear);
            Debug.Assert(MinDayOfYear <= doy);
            Debug.Assert(doy <= MaxDayOfYear);
            Debug.Assert(MinExtra <= x);
            Debug.Assert(x <= MaxExtra);

            _bin = Pack(y, doy, x);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Yedoyx"/> struct.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        internal Yedoyx(Yedoy ydoy, int x)
        {
            Debug.Assert(MinYear <= ydoy.Year);
            Debug.Assert(ydoy.Year <= MaxYear);
            Debug.Assert(MinExtra <= x);
            Debug.Assert(x <= MaxExtra);

            _bin = (ydoy.ToBinary() << Yemodax.ExtraBits) | x;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Yedoyx"/> struct directly from the
        /// specified binary data.
        /// <para>This constructor does NOT validate its parameter.</para>
        /// </summary>
        internal Yedoyx(int bin)
        {
            _bin = bin;
        }

        /// <summary>
        /// Gets the year.
        /// </summary>
        public int Year => unchecked(_bin >> YearShift);

        /// <summary>
        /// Gets the day of the year.
        /// </summary>
        public int DayOfYear => unchecked(1 + ((_bin >> DayOfYearShift) & Yedoy.DayOfYearMask));

        /// <summary>
        /// Gets the unspecified complementary value from this instance.
        /// </summary>
        public int Extra => unchecked(_bin & Yemodax.ExtraMask);

        /// <summary>
        /// Gets the ordinal date parts from this instance.
        /// </summary>
        public Yedoy Yedoy => new(unchecked(_bin >> Yemodax.ExtraBits));

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
            Unpack(out int y, out int doy);
            return FormattableString.Invariant($"{doy:D3}/{y:D4}");
        }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int dayOfYear) => Unpack(out year, out dayOfYear);

        /// <summary>
        /// Creates a new instance of <see cref="Yedoyx"/>.
        /// </summary>
        /// <exception cref="AoorException">The specified values are not representable; one of the
        /// value is too large to be handled by the system.</exception>
        [Pure]
        public static Yedoyx Create(int year, int dayOfYear, int extra)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year);
            }
            if (dayOfYear < MinDayOfYear || dayOfYear > MaxDayOfYear)
            {
                Throw.DayOfYearOutOfRange(dayOfYear);
            }
            if (extra < MinExtra || extra > MaxExtra)
            {
                Throw.ArgumentOutOfRange(nameof(extra));
            }

            return new Yedoyx(Pack(year, dayOfYear, extra));
        }

        /// <summary>
        /// Creates a new instance of <see cref="Yedoyx"/>.
        /// </summary>
        /// <exception cref="AoorException">The specified values are not representable; one of the
        /// value is too large to be handled by the system.</exception>
        [Pure]
        public static Yedoyx Create(Yedoy ydoy, int extra)
        {
            if (ydoy.Year < MinYear || ydoy.Year > MaxYear)
            {
                Throw.YearOutOfRange(ydoy.Year, nameof(ydoy));
            }
            if (extra < MinExtra || extra > MaxExtra)
            {
                Throw.ArgumentOutOfRange(nameof(extra));
            }

            return new Yedoyx(ydoy, extra);
        }
    }

    public partial struct Yedoyx // Binary data helpers
    {
        /// <summary>
        /// Deserializes a 32-bit binary value and recreates an original serialized
        /// <see cref="Yedoyx"/> object.
        /// </summary>
        [Pure]
        public static Yedoyx FromBinary(int data) => new(data);

        /// <summary>
        /// Obtains the 32-bit binary value stored in the current instance.
        /// </summary>
        [Pure]
        public int ToBinary() => _bin;

        /// <summary>
        /// Packs the specified ordinal date parts into a single 32-bit word.
        /// </summary>
        [Pure]
        // CIL code size = 13 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Pack(int y, int doy, int x)
        {
            unchecked
            {
                return (y << YearShift) | ((doy - 1) << DayOfYearShift) | x;
            }
        }

        /// <summary>
        /// Unpacks the binary data.
        /// </summary>
        // CIL code size = 27 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Unpack(out int y, out int doy)
        {
            // Perf: local copy of the field _bin.
            int bin = _bin;

            unchecked
            {
                y = bin >> YearShift;
                doy = 1 + ((bin >> DayOfYearShift) & Yedoy.DayOfYearMask);
            }
        }
    }

    public partial struct Yedoyx // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="Yedoyx"/> are equal.
        /// </summary>
        public static bool operator ==(Yedoyx left, Yedoyx right) => left._bin == right._bin;

        /// <summary>
        /// Determines whether two specified instances of <see cref="Yedoyx"/> are not equal.
        /// </summary>
        public static bool operator !=(Yedoyx left, Yedoyx right) => left._bin != right._bin;

        /// <inheritdoc />
        [Pure]
        public bool Equals(Yedoyx other) => _bin == other._bin;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Yedoyx parts && Equals(parts);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => _bin;
    }
}
