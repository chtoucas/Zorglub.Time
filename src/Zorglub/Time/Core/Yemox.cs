// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using System.ComponentModel;

/// <summary>
/// Provides a compact representation of a year, a month and an unspecified optional value.
/// <para><see cref="Yemox"/> is an immutable struct.</para>
/// </summary>
[DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
public readonly partial struct Yemox : IEqualityOperators<Yemox, Yemox>
{
    /// <summary>
    /// Represents the absolute minimum value for <see cref="Year"/>.
    /// <para>This field is a constant equal to -16_383.</para>
    /// </summary>
    public const int MinYear = Yemodax.MinYear;

    /// <summary>
    /// Represents the absolute maximum value for <see cref="Year"/>.
    /// <para>This field is a constant equal to 16_384.</para>
    /// </summary>
    public const int MaxYear = Yemodax.MaxYear;

    /// <summary>
    /// Represents the absolute minimum value for <see cref="Month"/>.
    /// <para>This field is a constant equal to 1.</para>
    /// </summary>
    public const int MinMonth = Yemodax.MinMonth;

    /// <summary>
    /// Represents the absolute maximum value for <see cref="Month"/>.
    /// <para>This field is a constant equal to 16.</para>
    /// </summary>
    public const int MaxMonth = Yemodax.MaxMonth;

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

    // Do NOT make MinValue & MaxValue public, it would give the impression
    // that all values in between are legal, which is not the case by far.

    /// <summary>
    /// Represents the smallest possible value of a <see cref="Yemox"/>.
    /// <para>This field is read-only.</para>
    /// </summary>
    internal static readonly Yemox MinValue = new(MinYear, MinMonth, MinExtra);

    /// <summary>
    /// Gets the largest possible value of a <see cref="Yemox"/>.
    /// <para>This field is read-only.</para>
    /// </summary>
    internal static readonly Yemox MaxValue = new(MaxYear, MaxMonth, MaxExtra);

    /// <summary>
    /// Represents the binary data stored in this instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The data is organised as follows:
    /// <code><![CDATA[
    ///   Year - 1  bbbb bbbb bbbb bbb
    ///   Month - 1                   b bbb
    ///   (Day - 1)                        0 0000 0
    ///   Extra                                    bbb bbbb
    /// ]]></code>
    /// </para>
    /// </remarks>
    private readonly int _bin;

    /// <summary>
    /// Initializes a new instance of the <see cref="Yemox"/>.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal Yemox(int y, int m, int x)
    {
        Debug.Assert(MinYear <= y);
        Debug.Assert(y <= MaxYear);
        Debug.Assert(MinMonth <= m);
        Debug.Assert(m <= MaxMonth);
        Debug.Assert(MinExtra <= x);
        Debug.Assert(x <= MaxExtra);

        _bin = Pack(y, m, x);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Yemox"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal Yemox(Yemo ym, int x)
    {
        Debug.Assert(MinYear <= ym.Year);
        Debug.Assert(ym.Year <= MaxYear);
        Debug.Assert(MinExtra <= x);
        Debug.Assert(x <= MaxExtra);

        _bin = (ym.ToBinary() << Yemodax.ExtraBits) | x;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Yemox"/> struct directly from the specified
    /// binary data.
    /// <para>This constructor does NOT validate its parameter.</para>
    /// </summary>
    internal Yemox(int bin)
    {
        DebugCheckBinaryData(bin);

        _bin = bin;
    }

    /// <summary>
    /// Gets the algebraic year from this instance.
    /// </summary>
    public int Year => unchecked(1 + (_bin >> Yemodax.YearShift));

    /// <summary>
    /// Gets the month of the year from this instance.
    /// </summary>
    public int Month => unchecked(1 + ((_bin >> Yemodax.MonthShift) & Yemodax.MonthMask));

    /// <summary>
    /// Gets the unspecified complementary value from this instance.
    /// </summary>
    public int Extra => unchecked(_bin & Yemodax.ExtraMask);

    /// <summary>
    /// Gets the month parts from this instance.
    /// </summary>
    public Yemo Yemo => new(unchecked(_bin >> Yemodax.ExtraBits));

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
        Unpack(out int y, out int m);
        return FormattableString.Invariant($"{m:D2}/{y:D4}");
    }

    /// <summary>
    /// Deconstructs the current instance into its components.
    /// </summary>
    public void Deconstruct(out int year, out int month) => Unpack(out year, out month);

    /// <summary>
    /// Creates a new instance of <see cref="Yemox"/> from the specified year and month.
    /// </summary>
    /// <exception cref="AoorException">The specified values are not representable; one of the
    /// value is too large to be handled by the system.</exception>
    [Pure]
    public static Yemox Create(int year, int month, int extra)
    {
        if (year < MinYear || year > MaxYear) Throw.YearOutOfRange(year);
        if (month < MinMonth || month > MaxMonth) Throw.MonthOutOfRange(month);
        if (extra < MinExtra || extra > MaxExtra) Throw.ArgumentOutOfRange(nameof(extra));

        return new Yemox(Pack(year, month, extra));
    }

    /// <summary>
    /// Creates a new instance of <see cref="Yemodax"/>.
    /// </summary>
    /// <exception cref="AoorException">The specified values are not representable; one of the
    /// value is too large to be handled by the system.</exception>
    [Pure]
    public static Yemox Create(Yemo ym, int extra)
    {
        if (ym.Year < MinYear || ym.Year > MaxYear) Throw.YearOutOfRange(ym.Year, nameof(ym));
        if (extra < MinExtra || extra > MaxExtra) Throw.ArgumentOutOfRange(nameof(extra));

        return new Yemox(ym, extra);
    }
}

public partial struct Yemox // Binary data helpers
{
    /// <summary>
    /// Deserializes a 32-bit binary value and recreates an original serialized
    /// <see cref="Yemox"/> object.
    /// </summary>
    /// <exception cref="ArgumentException">The specified binary data is not well-formed.
    /// </exception>
    [Pure]
    public static Yemox FromBinary(int data)
    {
        ValidateBinaryData(data);
        return new(data);
    }

    /// <summary>
    /// Obtains the 32-bit binary value stored in the current instance.
    /// </summary>
    [Pure]
    public int ToBinary() => _bin;

    /// <summary>
    /// Packs the specified month parts into a single 32-bit word.
    /// </summary>
    [Pure]
    // CIL code size = 16 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Pack(int y, int m, int x)
    {
        unchecked
        {
            return ((y - 1) << Yemodax.YearShift) | ((m - 1) << Yemodax.MonthShift) | x;
        }
    }

    /// <summary>
    /// Unpacks the binary data.
    /// </summary>
    // CIL code size = 27 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Unpack(out int y, out int m)
    {
        // Perf: local copy of the field _bin.
        int bin = _bin;

        unchecked
        {
            y = 1 + (bin >> Yemodax.YearShift);
            m = 1 + ((bin >> Yemodax.MonthShift) & Yemodax.MonthMask);
        }
    }

    /// <summary>
    /// Validates the specified binary data.
    /// </summary>
    /// <exception cref="ArgumentException">The specified binary data is not well-formed.
    /// </exception>
    private static void ValidateBinaryData(int data)
    {
        int d0 = (data >> Yemodax.DayShift) & Yemodax.DayMask;
        if (d0 != 0)
        {
            Throw.BadBinaryInput();
        }
    }

    [Conditional("DEBUG")]
    [ExcludeFromCodeCoverage]
    private static void DebugCheckBinaryData(int bin) => ValidateBinaryData(bin);
}

public partial struct Yemox // IEquatable
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="Yemox"/> are equal.
    /// </summary>
    public static bool operator ==(Yemox left, Yemox right) => left._bin == right._bin;

    /// <summary>
    /// Determines whether two specified instances of <see cref="Yemox"/> are not equal.
    /// </summary>
    public static bool operator !=(Yemox left, Yemox right) => left._bin != right._bin;

    /// <inheritdoc />
    [Pure]
    public bool Equals(Yemox other) => _bin == other._bin;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Yemox parts && Equals(parts);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _bin;
}
