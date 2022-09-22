// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using System.ComponentModel;

/// <summary>
/// Provides a compact representation of a year and a week of year.
/// <para><see cref="Yewex"/> is an immutable struct.</para>
/// </summary>
[DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
public readonly partial struct Yewex : IEquatable<Yewex>
{
    #region Bit settings

    /// <summary>This field is a constant equal to 7.</summary>
    private const int WeekOfYearShift = Yemodax.ExtraBits;

    /// <summary>This field is a constant equal to 17.</summary>
    private const int YearShift = WeekOfYearShift + Yewe.WeekOfYearBits;

    #endregion

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
    /// Represents the absolute minimum value for <see cref="WeekOfYear"/>.
    /// <para>This field is a constant equal to 1.</para>
    /// </summary>
    public const int MinWeekOfYear = Yewe.MinWeekOfYear;

    /// <summary>
    /// Represents the absolute maximum value for <see cref="WeekOfYear"/>.
    /// <para>This field is a constant equal to 1024.</para>
    /// </summary>
    public const int MaxWeekOfYear = Yewe.MaxWeekOfYear;

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
    /// Represents the smallest possible value of a <see cref="Yewex"/>.
    /// <para>This field is read-only.</para>
    /// </summary>
    public static readonly Yewex MinValue = new(MinYear, MinWeekOfYear, MinExtra);

    /// <summary>
    /// Represents the largest possible value of a <see cref="Yewex"/>.
    /// <para>This field is read-only.</para>
    /// </summary>
    public static readonly Yewex MaxValue = new(MaxYear, MaxWeekOfYear, MaxExtra);

    /// <summary>
    /// Represents the binary data stored in this instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The data is organised as follows:
    /// <code><![CDATA[
    ///   Year           bbbb bbbb bbbb bbb
    ///   WeekOfYear - 1                   b bbbb bbbb b
    ///   Extra                                         bbb bbbb
    /// ]]></code>
    /// </para>
    /// </remarks>
    private readonly int _bin;

    /// <summary>
    /// Initializes a new instance of the <see cref="Yewex"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal Yewex(int y, int w, int x)
    {
        Debug.Assert(MinYear <= y);
        Debug.Assert(y <= MaxYear);
        Debug.Assert(MinWeekOfYear <= w);
        Debug.Assert(w <= MaxWeekOfYear);
        Debug.Assert(MinExtra <= x);
        Debug.Assert(x <= MaxExtra);

        _bin = Pack(y, w, x);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Yewex"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal Yewex(Yewe yw, int x)
    {
        Debug.Assert(MinYear <= yw.Year);
        Debug.Assert(yw.Year <= MaxYear);
        Debug.Assert(MinExtra <= x);
        Debug.Assert(x <= MaxExtra);

        _bin = (yw.ToBinary() << Yemodax.ExtraBits) | x;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Yewex"/> struct
    /// directly from the specified binary data.
    /// <para>This constructor does NOT validate its parameter.</para>
    /// </summary>
    internal Yewex(int bin)
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
    public int WeekOfYear => unchecked(1 + ((_bin >> WeekOfYearShift) & Yewe.WeekOfYearMask));

    /// <summary>
    /// Gets the unspecified complementary value from this instance.
    /// </summary>
    public int Extra => unchecked(_bin & Yemodax.ExtraMask);

    /// <summary>
    /// Gets the ordinal date parts from this instance.
    /// </summary>
    public Yewe Yewe => new(unchecked(_bin >> Yemodax.ExtraBits));

    /// <summary>
    /// Gets the string to display in the debugger watch window.
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "DebuggerDisplay")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private string DebuggerDisplay => Convert.ToString(_bin, 2);

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
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

    /// <summary>
    /// Creates a new instance of <see cref="Yewex"/>.
    /// </summary>
    /// <exception cref="AoorException">The specified values are not
    /// representable; one of the value is too large to be handled by the
    /// system.</exception>
    [Pure]
    public static Yewex Create(int year, int weekOfYear, int extra)
    {
        if (year < MinYear || year > MaxYear)
        {
            Throw.ArgumentOutOfRange(nameof(year));
        }
        if (weekOfYear < MinWeekOfYear || weekOfYear > MaxWeekOfYear)
        {
            Throw.ArgumentOutOfRange(nameof(weekOfYear));
        }
        if (extra < MinExtra || extra > MaxExtra)
        {
            Throw.ArgumentOutOfRange(nameof(extra));
        }

        return new Yewex(Pack(year, weekOfYear, extra));
    }

    /// <summary>
    /// Creates a new instance of <see cref="Yewex"/>.
    /// </summary>
    /// <exception cref="AoorException">The specified values are not
    /// representable; one of the value is too large to be handled by the
    /// system.</exception>
    [Pure]
    public static Yewex Create(Yewe parts, int extra)
    {
        if (parts.Year < MinYear || parts.Year > MaxYear)
        {
            Throw.ArgumentOutOfRange(nameof(parts));
        }
        if (extra < MinExtra || extra > MaxExtra)
        {
            Throw.ArgumentOutOfRange(nameof(extra));
        }

        return new Yewex(parts, extra);
    }
}

public partial struct Yewex // Binary data helpers
{
    /// <summary>
    /// Deserializes a 32-bit binary value and recreates an original
    /// serialized <see cref="Yewex"/> object.
    /// </summary>
    [Pure]
    public static Yewex FromBinary(int data) => new(data);

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
    private static int Pack(int y, int w, int x)
    {
        unchecked
        {
            return ((y - 1) << YearShift) | ((w - 1) << WeekOfYearShift) | x;
        }
    }

    /// <summary>
    /// Unpacks the binary data.
    /// </summary>
    // CIL code size = 27 bytes <= 32 bytes.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Unpack(out int y, out int w)
    {
        // Perf: local copy of the field _bin.
        int bin = _bin;

        unchecked
        {
            y = 1 + (bin >> YearShift);
            w = 1 + ((bin >> WeekOfYearShift) & Yewe.WeekOfYearMask);
        }
    }
}

public partial struct Yewex // IEquatable
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="Yewex"/>
    /// are equal.
    /// </summary>
    public static bool operator ==(Yewex left, Yewex right) => left._bin == right._bin;

    /// <summary>
    /// Determines whether two specified instances of <see cref="Yewex"/>
    /// are not equal.
    /// </summary>
    public static bool operator !=(Yewex left, Yewex right) => left._bin != right._bin;

    /// <inheritdoc />
    [Pure]
    public bool Equals(Yewex other) => _bin == other._bin;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Yewex parts && this == parts;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _bin;
}
