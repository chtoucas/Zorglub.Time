// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Astronomy;

using System.Runtime.InteropServices;

using static Zorglub.Time.Extensions.JulianDateVersionExtensions;
using static Zorglub.Time.Extensions.TimescaleExtensions;

// FIXME: à complètement revoir. JulianDateCoordSystem ? Utiliser SplitJD ?

/// <summary>
/// Represents a modified Julian Date.
/// </summary>
[StructLayout(LayoutKind.Explicit, Pack = 1)]
public readonly partial struct OtherJulianDate :
    IEqualityOperators<OtherJulianDate, OtherJulianDate>
{
    /// <summary>
    /// Represents the numerical value of this instance.
    /// </summary>
    [FieldOffset(0)] private readonly double _value;

    /// <summary>
    /// Represents the time scale of this instance.
    /// </summary>
    [FieldOffset(8)] private readonly Timescale _timescale;

    /// <summary>
    /// Represents the version of this instance.
    /// </summary>
    [FieldOffset(9)] private readonly JulianDateVersion _version;

    /// <summary>
    /// Initializes a new instance of the <see cref="OtherJulianDate"/> struct from
    /// the specified numerical value, within the TT time scale and with the
    /// default version.
    /// <para>This is the right constructor to use when <paramref name="value"/>
    /// is the "Modified Julian Date" of the astrophysicists.</para>
    /// </summary>
    public OtherJulianDate(double value)
        : this(value, Timescale.Terrestrial, JulianDateVersion.Modified) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="OtherJulianDate"/> struct from
    /// the specified time scale and numerical value, with the default
    /// version.
    /// </summary>
    public OtherJulianDate(double value, Timescale timescale)
        : this(value, timescale, JulianDateVersion.Modified) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="OtherJulianDate"/> struct from
    /// the specified numerical value and version, within the TT time scale.
    /// </summary>
    public OtherJulianDate(double value, JulianDateVersion version)
        : this(value, Timescale.Terrestrial, version) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="OtherJulianDate"/> struct from
    /// the specified time scale, numerical value and version.
    /// </summary>
    public OtherJulianDate(
        double value, Timescale timescale, JulianDateVersion version)
    {
        _value = value;
        _timescale = timescale;
        _version = version;
    }

    /// <summary>
    /// Gets the numerical value from this instance.
    /// </summary>
    public double Value => _value;

    /// <summary>
    /// Gets the time scale from this instance.
    /// </summary>
    public Timescale Timescale => _timescale;

    /// <summary>
    /// Gets the version of Julian Date from this instance.
    /// </summary>
    public JulianDateVersion Version => _version;

    /// <summary>
    /// Returns a culture-independent string representation of this instance.
    /// </summary>
    public override string ToString()
        => FormattableString.Invariant(
            $"{Version.GetShortName()}({Timescale.GetAbbrName()}) {Value}");

    /// <summary>
    /// Deconstructs this instance into its components.
    /// </summary>
    public void Deconstruct(
        out Timescale timescale, out double value, out JulianDateVersion version)
        => (timescale, value, version) = (_timescale, _value, _version);

    /// <summary>
    /// Converts this instance to a Julian Date.
    /// <para>This transformation may incur a loss of precision.</para>
    /// </summary>
    [Pure]
    public AstronomicalJulianDate ToJulianDate()
    {
        double value = _version switch
        {
            JulianDateVersion.Astronomical => _value,
            JulianDateVersion.Modified => _value + JulianDateEpoch.Modified,
            JulianDateVersion.Ccsds => _value + JulianDateEpoch.Ccsds,
            JulianDateVersion.Cnes => _value + JulianDateEpoch.Cnes,
            JulianDateVersion.Dublin => _value + JulianDateEpoch.Dublin,
            JulianDateVersion.Reduced => _value + JulianDateEpoch.Reduced,
            JulianDateVersion.Truncated => _value + JulianDateEpoch.Truncated,
            _ => Throw.Unreachable<double>(),
        };
        return new AstronomicalJulianDate(value, _timescale);
    }

    ///// <summary>
    ///// Converts this instance to the specified version of the Julian Date.
    ///// <para>This transformation may incur a loss of precision.</para>
    ///// </summary>
    //[Pure]
    //public OtherJulianDate ToOtherJulianDate(JulianDateVersion version)
    //{
    //    double value = version switch
    //    {
    //        ModifiedJulianDateVersion.Astronomical => _value,
    //        ModifiedJulianDateVersion.Modified => _value - JulianDateEpoch.Modified,
    //        ModifiedJulianDateVersion.Ccsds => _value - JulianDateEpoch.Ccsds,
    //        ModifiedJulianDateVersion.Cnes => _value - JulianDateEpoch.Cnes,
    //        ModifiedJulianDateVersion.Dublin => _value - JulianDateEpoch.Dublin,
    //        ModifiedJulianDateVersion.Reduced => _value - JulianDateEpoch.Reduced,
    //        ModifiedJulianDateVersion.Truncated => _value - JulianDateEpoch.Truncated,
    //        _ => throw new ArgumentOutOfRangeException(nameof(version))
    //    };
    //    return new OtherJulianDate(value, _timescale, version);
    //}
}

// Interface IEquatable<>.
public partial struct OtherJulianDate
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="OtherJulianDate"/>
    /// are equal.
    /// </summary>
    public static bool operator ==(OtherJulianDate left, OtherJulianDate right) =>
        left._timescale == right._timescale
        && left._value == right._value
        && left._version == right._version;

    /// <summary>
    /// Determines whether two specified instances of <see cref="OtherJulianDate"/>
    /// are not equal.
    /// </summary>
    public static bool operator !=(OtherJulianDate left, OtherJulianDate right) => !(left == right);

    /// <summary>
    /// Determines whether this instance is equal to the value of the
    /// specified <see cref="OtherJulianDate"/>.
    /// </summary>
    public bool Equals(OtherJulianDate other) => this == other;

    /// <summary>
    /// Determines whether this instance is equal to a specified object.
    /// </summary>
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is OtherJulianDate jd && this == jd;

    /// <summary>
    /// Obtains the hash code for this instance.
    /// </summary>
    public override int GetHashCode() => HashCode.Combine(_timescale, _value, _version);
}
