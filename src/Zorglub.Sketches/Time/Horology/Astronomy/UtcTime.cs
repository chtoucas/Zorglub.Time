// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

// Adapted from SOFA source code.

namespace Zorglub.Time.Horology.Astronomy;

using Zorglub.Time.Core;

using static Zorglub.Time.Core.TemporalConstants;

/// <summary>
/// Represents a Coordinated Universal Time (UTC).
/// <para><see cref="UtcTime"/> is an immutable struct.</para>
/// </summary>
/// <remarks>
/// <para>UTC is the time standard used as a basis for civil timekeeping,
/// but is is not the Civil Time which is tied to a Time Zone and possibly
/// accounts for Daylight Saving Time (DST) adjustments.</para>
/// <para>The unit of UTC is the SI second.</para>
/// <para>The UTC time scale is not uniform, it occasionally adds a leap
/// second to keep the offset between UT1 and UTC below 0.9 seconds in
/// absolute value.</para>
/// <para>Two instances of <see cref="UtcTime"/> are considered equal
/// if they have the same internal representation (a quasi Julian date),
/// this is NOT the same as numerical equality. As a consequence, two
/// instances may ultimately represent the same point in time but be
/// regarded as different by .NET.</para>
/// </remarks>
public readonly partial struct UtcTime :
    IComparisonOperators<UtcTime, UtcTime>
{
    /// <summary>
    /// Represents the quasi Julian date of this time instance.
    /// </summary>
    private readonly QuasiJD _quasiJD;

    /// <summary>
    /// Initializes a new instance of the <see cref="UtcTime"/> struct from the
    /// specified quasi Julian date.
    /// </summary>
    internal UtcTime(QuasiJD quasiJD)
    {
        _quasiJD = quasiJD;
    }
}

// Fabriques.
public partial struct UtcTime
{
    /// <summary>
    /// Creates a new instance of <see cref="UtcTime"/> from the specified
    /// Julian Date.
    /// </summary>
    [Pure]
    public static UtcTime FromJulianDate(AstronomicalJulianDate jd)
    {
        if (jd.Timescale != Timescale.Utc)
            ThrowHelpers2.BadTimescale(nameof(jd), Timescale.Utc, jd.Timescale);

        var quasiJD = QuasiJD.FromJulianDate(jd.Value);
        return new UtcTime(quasiJD);
    }

    /// <summary>
    /// Creates a new instance of <see cref="UtcTime"/> from the specified
    /// Modified Julian Date.
    /// </summary>
    [Pure]
    public static UtcTime FromModifiedJulianDate(ModifiedJulianDate mjd)
    {
        if (mjd.Timescale != Timescale.Utc)
            ThrowHelpers2.BadTimescale(nameof(mjd), Timescale.Utc, mjd.Timescale);

        var quasiJD = QuasiJD.FromModifiedJulianDate(mjd.Value);
        return new UtcTime(quasiJD);
    }

    /// <summary>
    /// Creates a new instance of <see cref="UtcTime"/> from the specified
    /// clock time.
    /// </summary>
    [Pure]
    public static UtcTime FromClockTime(ClockTime0 clockTime) =>
        new(QuasiJD.FromClockTime(clockTime));
}

// Conversions.
public partial struct UtcTime
{
    /// <summary>
    /// Converts this instance to a <see cref="DayNumber"/> in the
    /// Astronomical Julian dayscale and also returns the fraction of the
    /// day in an output parameter.
    /// </summary>
    [Pure]
    public DayNumber ToDayNumber(out double fractionOfDay)
    {
        int jdn = _quasiJD.ToJulianDayNumber(out fractionOfDay);
        //return new DayNumber(1 + jdn, DayscaleId.AstronomicalJulian);
        throw new NotImplementedException();
    }

    /// <summary>
    /// Converts this instance to an Astronomical Julian Date within the UTC time scale.
    /// </summary>
    [Pure]
    public AstronomicalJulianDate ToJulianDate() => new(_quasiJD.JulianDate, Timescale.Utc);

    /// <summary>
    /// Converts this instance to a Modified Julian Date within the UTC
    /// time scale.
    /// </summary>
    [Pure]
    public ModifiedJulianDate ToModifiedJulianDate() => new(_quasiJD.ModifiedJulianDate, Timescale.Utc);

    /// <summary>
    /// Obtains the gregorian date and the time of the day for this time
    /// instance.
    /// </summary>
    [Pure]
    public (Yemoda ymd, int hour, int minute, int second, int fractionOfSecond)
        ToClockTime(int decimalPlaces)
        => _quasiJD.ToClockTime(decimalPlaces);

#pragma warning disable CA1822

    /// <summary>
    /// Converts this instance to an Atomic Time.
    /// </summary>
    [Pure]
    public AtomicTime ToAtomicTime()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Converts this instance to a Universal Time.
    /// </summary>
    /// <remarks>
    /// <paramref name="deltaUT1"/> is the difference between UT1 and UTC,
    /// ΔUT1 = UT1-UTC in seconds, available from IERS Bulletins.
    /// </remarks>
    [Pure]
    public UniversalTime ToUniversalTime(double deltaUT1)
    {
        double offset = deltaUT1 / SecondsPerDay;
        throw new NotImplementedException();
    }
}

// Interface IEquatable<>.
public partial struct UtcTime
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="UtcTime"/>
    /// are equal.
    /// </summary>
    public static bool operator ==(UtcTime left, UtcTime right)
        => left._quasiJD == right._quasiJD;

    /// <summary>
    /// Determines whether two specified instances of <see cref="UtcTime"/>
    /// are not equal.
    /// </summary>
    public static bool operator !=(UtcTime left, UtcTime right)
        => left._quasiJD != right._quasiJD;

    /// <summary>
    /// Determines whether this instance is equal to the value of the
    /// specified <see cref="UtcTime"/>.
    /// </summary>
    public bool Equals(UtcTime other)
        => _quasiJD == other._quasiJD;

    /// <summary>
    /// Determines whether this instance is equal to a specified object.
    /// </summary>
    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is UtcTime time && this == time;

    /// <summary>
    /// Obtains the hash code for this instance.
    /// </summary>
    public override int GetHashCode() => _quasiJD.GetHashCode();
}

// Interfaces IComparable<> et IComparable.
public partial struct UtcTime
{
    /// <summary>
    /// Compares the two specified times to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(UtcTime left, UtcTime right)
        => left._quasiJD < right._quasiJD;

    /// <summary>
    /// Compares the two specified times to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(UtcTime left, UtcTime right)
        => left._quasiJD <= right._quasiJD;

    /// <summary>
    /// Compares the two specified times to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(UtcTime left, UtcTime right)
        => left._quasiJD > right._quasiJD;

    /// <summary>
    /// Compares the two specified times to see if the left one is later
    /// than or equal to the right one.
    /// </summary>
    public static bool operator >=(UtcTime left, UtcTime right)
        => left._quasiJD >= right._quasiJD;

    /// <summary>
    /// Indicates whether this time instance is earlier, later or the same
    /// as the specified one.
    /// </summary>
    [Pure]
    public int CompareTo(UtcTime other)
        => _quasiJD.CompareTo(other._quasiJD);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is UtcTime time ? CompareTo(time)
        : Throw.NonComparable(typeof(UtcTime), obj);
}
