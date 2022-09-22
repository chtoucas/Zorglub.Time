// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Astronomy;

using static Zorglub.Time.Core.TemporalConstants;

// Terrestrial Time (TT): a coordinate time whose mean rate is close to the
// mean rate of the proper time of an observer located on the rotating geoid.
// At 1977 January 1.0 TAI exactly, the value of TT was 1977 January
// 1.0003725 exactly. It is related to the Geocentric Coordinate Time (TCG)
// by a conventional linear transformation provided by IAU 2000 Resolution
// B1.9. TT may be used as the independent time argument for geocentric
// ephemerides. An accurate realization of TT is TT (TAI) = TAI + 32s.184.
// In the past TT was called Terrestrial Dynamical Time (TDT).
// [NFA Glossary](https://syrte.obspm.fr/iauWGnfa/NFA_Glossary.html)
//
// Geoid: The equipotential surface of the Earth's gravity field which best
// fits, in a least squares sense, global mean sea level.
// [What is the geoid?](https://www.ngs.noaa.gov/GEOID/geoid_def.html)
// According the Technical Note n°36 of the IERS, this is the definition
// adopted by the IERS.

/// <summary>
/// Represents a Terrestrial Time (TT).
/// <para><see cref="TerrestrialTime"/> is an immutable struct.</para>
/// </summary>
/// <remarks>
/// <para>TT is a theoretical ideal time scale for clocks at sea level
/// (earth's geoid). Formally, it is a coordinate time in a four-dimensional
/// space-time reference system within General Relativity.</para>
/// <para>The unit of TT is the SI second.</para>
/// <para>Two instances of <see cref="TerrestrialTime"/> are considered equal
/// if they have the same internal representation (a two-part Julian date),
/// this is NOT the same as numerical equality. As a consequence, two
/// instances may ultimately represent the same point in time but be
/// regarded as different by .NET.</para>
/// </remarks>
public readonly partial struct TerrestrialTime :
    IComparisonOperators<TerrestrialTime, TerrestrialTime>
{
    /// <summary>
    /// Represents the two-part Julian date of this time instance.
    /// </summary>
    private readonly SplitJD _splitJD;

    /// <summary>
    /// Initializes a new instance of the <see cref="TerrestrialTime"/> struct from the
    /// specified two-part Julian date.
    /// </summary>
    internal TerrestrialTime(SplitJD splitJD) => _splitJD = splitJD;
}

// Fabriques.
public partial struct TerrestrialTime
{
    /// <summary>
    /// Creates a new instance of <see cref="TerrestrialTime"/> from the
    /// specified Julian Date.
    /// </summary>
    public static TerrestrialTime FromJulianDate(AstronomicalJulianDate jd)
    {
        if (jd.Timescale != Timescale.Terrestrial)
            ThrowHelpers2.BadTimescale(nameof(jd), Timescale.Terrestrial, jd.Timescale);

        var splitJD = SplitJD.FromJulianDate(jd.Value);
        return new TerrestrialTime(splitJD);
    }

    /// <summary>
    /// Creates a new instance of <see cref="TerrestrialTime"/> from the
    /// specified Modified Julian Date.
    /// </summary>
    public static TerrestrialTime FromModifiedJulianDate(ModifiedJulianDate mjd)
    {
        if (mjd.Timescale != Timescale.Terrestrial)
            ThrowHelpers2.BadTimescale(nameof(mjd), Timescale.Terrestrial, mjd.Timescale);

        var splitJD = SplitJD.FromModifiedJulianDate(mjd.Value);
        return new TerrestrialTime(splitJD);
    }
}

// Conversions.
public partial struct TerrestrialTime
{
    /// <summary>
    /// Converts this instance to a Day Number in the Astronomical Julian
    /// dayscale and also returns the fraction of the day in an output
    /// parameter.
    /// </summary>
    [Pure]
    public DayNumber ToDayNumber(out double fractionOfDay)
    {
        int jdn = _splitJD.ToJulianDayNumber(out fractionOfDay);
        //return new DayNumber(1 + jdn, DayscaleId.AstronomicalJulian);
        throw new NotImplementedException();
    }

    /// <summary>
    /// Converts this instance to an Astronomical Julian Date within the TT time scale.
    /// </summary>
    [Pure]
    public AstronomicalJulianDate ToJulianDate() => new(_splitJD.JulianDate);

    /// <summary>
    /// Converts this instance to a Modified Julian Date within the TT time scale.
    /// </summary>
    [Pure]
    public ModifiedJulianDate ToModifiedJulianDate() => new(_splitJD.ModifiedJulianDate);

    /// <summary>
    /// Converts this instance to an Atomic Time.
    /// </summary>
    [Pure]
    public AtomicTime ToAtomicTime() => new(_splitJD - DeltaTAT);

    /// <summary>
    /// Converts this instance to a Universal Time.
    /// </summary>
    /// <remarks>
    /// <paramref name="deltaT"/> is the time correction ΔT = TT-UT1 in
    /// seconds, which increased quadratically over time.
    /// </remarks>
    [Pure]
    public UniversalTime ToUniversalTime(double deltaT) => new(_splitJD - deltaT / SecondsPerDay);
}

// Interface IEquatable<>.
public partial struct TerrestrialTime
{
    /// <summary>
    /// Determines whether two specified instances of
    /// <see cref="TerrestrialTime"/> are equal.
    /// </summary>
    public static bool operator ==(TerrestrialTime left, TerrestrialTime right)
        => left._splitJD == right._splitJD;

    /// <summary>
    /// Determines whether two specified instances of
    /// <see cref="TerrestrialTime"/> are not equal.
    /// </summary>
    public static bool operator !=(TerrestrialTime left, TerrestrialTime right)
        => left._splitJD != right._splitJD;

    /// <summary>
    /// Determines whether this instance is equal to the value of the
    /// specified <see cref="TerrestrialTime"/>.
    /// </summary>
    public bool Equals(TerrestrialTime other)
        => _splitJD == other._splitJD;

    /// <summary>
    /// Determines whether this instance is equal to a specified object.
    /// </summary>
    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is TerrestrialTime time && this == time;

    /// <summary>
    /// Obtains the hash code for this instance.
    /// </summary>
    public override int GetHashCode() => _splitJD.GetHashCode();
}

// Interfaces IComparable<> et IComparable.
public partial struct TerrestrialTime
{
    /// <summary>
    /// Compares the two specified times to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(TerrestrialTime left, TerrestrialTime right)
        => left._splitJD < right._splitJD;

    /// <summary>
    /// Compares the two specified times to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(TerrestrialTime left, TerrestrialTime right)
        => left._splitJD <= right._splitJD;

    /// <summary>
    /// Compares the two specified times to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(TerrestrialTime left, TerrestrialTime right)
        => left._splitJD > right._splitJD;

    /// <summary>
    /// Compares the two specified times to see if the left one is later
    /// than or equal to the right one.
    /// </summary>
    public static bool operator >=(TerrestrialTime left, TerrestrialTime right)
        => left._splitJD >= right._splitJD;

    /// <summary>
    /// Indicates whether this time instance is earlier, later or the same
    /// as the specified one.
    /// </summary>
    [Pure]
    public int CompareTo(TerrestrialTime other)
        => _splitJD.CompareTo(other._splitJD);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is TerrestrialTime time ? CompareTo(time)
        : Throw.NonComparable(typeof(TerrestrialTime), obj);
}
