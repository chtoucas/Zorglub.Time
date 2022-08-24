// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.
//
// Adapted from SOFA source code.

namespace Zorglub.Time.Horology.Astronomy
{
    using Zorglub.Time.Core;

    using static Zorglub.Time.Core.TemporalConstants;

    // Universal Time (UT): a measure of time that conforms, within a close
    // approximation, to the mean diurnal motion of the Sun and serves as the
    // basis of all civil timekeeping. The term "UT" is used to designate a
    // member of the family of Universal Time scales (e.g. UTC, UT1).
    //
    // Universal Time (UT1): angle of the Earth's rotation about the CIP axis
    // defined by its conventional linear relation to the Earth Rotation Angle
    // (ERA). It is related to Greenwich apparent sidereal time through the ERA
    // (see equation of the origins). It is determined by observations (currently
    // from VLBI observations of the diurnal motions of distant radio sources).
    // UT1 can be regarded as a time determined by the rotation of the Earth.
    // It can be obtained from the uniform time scale UTC by using the quantity
    // UT1 − UTC, which is provided by the IERS.
    // [NFA Glossary](https://syrte.obspm.fr/iauWGnfa/NFA_Glossary.html)

    /// <summary>
    /// Represents a Universal Time (UT1).
    /// <para><see cref="UniversalTime"/> is an immutable struct.</para>
    /// </summary>
    /// <remarks>
    /// <para>UT1 is the rotation angle about the Earth pole.</para>
    /// <para>The unit of UT1 is NOT the SI second.</para>
    /// <para>Two instances of <see cref="UniversalTime"/> are considered equal
    /// if they have the same internal representation (a two-part Julian date),
    /// this is NOT the same as numerical equality. As a consequence, two
    /// instances may ultimately represent the same point in time but be
    /// regarded as different by .NET.</para>
    /// </remarks>
    public readonly partial struct UniversalTime
        : IEquatable<UniversalTime>, IComparable<UniversalTime>, IComparable
    {
        /// <summary>
        /// Represents the two-part Julian date of this time instance.
        /// </summary>
        private readonly SplitJD _splitJD;

        /// <summary>
        /// Constructs a new instance of <see cref="UniversalTime"/> from the
        /// specified two-part Julian date.
        /// </summary>
        internal UniversalTime(SplitJD splitJD) => _splitJD = splitJD;
    }

    // Fabriques.
    public partial struct UniversalTime
    {
        /// <summary>
        /// Creates a new instance of <see cref="UniversalTime"/> from the
        /// specified Julian Date.
        /// </summary>
        public static UniversalTime FromJulianDate(JulianDate jd)
        {
            if (jd.Timescale != Timescale.Universal)
            {
                throw EF2.Timescales.UnexpectedTimescale(
                    nameof(jd), Timescale.Universal, jd.Timescale);
            }

            var splitJD = SplitJD.FromJulianDate(jd.Value);
            return new UniversalTime(splitJD);
        }

        /// <summary>
        /// Creates a new instance of <see cref="UniversalTime"/> from the
        /// specified Modified Julian Date.
        /// </summary>
        public static UniversalTime FromModifiedJulianDate(ModifiedJulianDate mjd)
        {
            if (mjd.Timescale != Timescale.Universal)
            {
                throw EF2.Timescales.UnexpectedTimescale(
                    nameof(mjd), Timescale.Universal, mjd.Timescale);
            }

            var splitJD = SplitJD.FromModifiedJulianDate(mjd.Value);
            return new UniversalTime(splitJD);
        }
    }

    // Conversions.
    public partial struct UniversalTime
    {
        /// <summary>
        /// Converts this instance to a Julian day number and also returns the
        /// fraction of the day in an output parameter.
        /// </summary>
        /// <remarks>
        /// According to the resolution B1 of the IERS, the Julian day number is
        /// the continuous count of SOLAR days since noon 1st January 4713 BCE
        /// (julian), and recall that a day in Universal Time is by definition a
        /// mean solar day. Still, it seems in contradiction with the other
        /// part that claims that a Julian Date should at the same time expressed
        /// in Terrestrial Time, and be the Julian day number for the preceding
        /// noon plus the fraction of the day since that instant.
        /// </remarks>
        /// <returns>A <see cref="DayNumber"/> in the Astronomical Julian
        /// dayscale.</returns>
        [Pure]
        public DayNumber ToJulianDayNumber(out double fractionOfDay)
        {
            int jdn = _splitJD.ToJulianDayNumber(out fractionOfDay);
            //return new DayNumber(1 + jdn, DayscaleId.AstronomicalJulian);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts this instance to an Astronomical Julian Date within the UT1
        /// timescale.
        /// </summary>
        [Pure]
        public JulianDate ToJulianDate() => new(_splitJD.JulianDate, Timescale.Universal);

        /// <summary>
        /// Converts this instance to a Modified Julian Date within the UT1
        /// timescale.
        /// </summary>
        [Pure]
        public ModifiedJulianDate ToModifiedJulianDate() => new(_splitJD.ModifiedJulianDate, Timescale.Universal);

        /// <summary>
        /// Converts this instance to a Atomic Time.
        /// </summary>
        /// <remarks>
        /// <paramref name="deltaTA"/> is the difference between UT1 and TAI,
        /// ΔTA = UT1-TAI in seconds, available from IERS Bulletins.
        /// </remarks>
        [Pure]
        public AtomicTime ToAtomicTime(double deltaTA) => new(_splitJD - deltaTA / SecondsPerDay);

        /// <summary>
        /// Converts this instance to a Universal Time.
        /// </summary>
        /// <remarks>
        /// <paramref name="deltaT"/> is the time correction ΔT = TT-UT1 in
        /// seconds, which increased quadratically over time.
        /// </remarks>
        [Pure]
        public TerrestrialTime ToTerrestrialTime(double deltaT) => new(_splitJD + deltaT / SecondsPerDay);

#pragma warning disable CA1822

        /// <summary>
        /// Converts this instance to a UTC Time.
        /// </summary>
        /// <remarks>
        /// <paramref name="deltaUT1"/> is the difference between UT1 and UTC,
        /// ΔUT1 = UT1-UTC in seconds, available from IERS Bulletins.
        /// </remarks>
        [Pure]
        public UtcTime ToUtcTime(double deltaUT1)
        {
            double offset = deltaUT1 / SecondsPerDay;
            throw new NotImplementedException();
        }
    }

    // Interface IEquatable<>.
    public partial struct UniversalTime
    {
        /// <summary>
        /// Determines whether two specified instances of
        /// <see cref="UniversalTime"/> are equal.
        /// </summary>
        public static bool operator ==(UniversalTime left, UniversalTime right)
            => left._splitJD == right._splitJD;

        /// <summary>
        /// Determines whether two specified instances of
        /// <see cref="UniversalTime"/> are not equal.
        /// </summary>
        public static bool operator !=(UniversalTime left, UniversalTime right)
            => left._splitJD != right._splitJD;

        /// <summary>
        /// Determines whether this instance is equal to the value of the
        /// specified <see cref="UniversalTime"/>.
        /// </summary>
        public bool Equals(UniversalTime other)
            => _splitJD == other._splitJD;

        /// <summary>
        /// Determines whether this instance is equal to a specified object.
        /// </summary>
        public override bool Equals(object? obj)
            => obj is UniversalTime time && this == time;

        /// <summary>
        /// Obtains the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
            => _splitJD.GetHashCode();
    }

    // Interfaces IComparable<> et IComparable.
    public partial struct UniversalTime
    {
        /// <summary>
        /// Compares the two specified times to see if the left one is strictly
        /// earlier than the right one.
        /// </summary>
        public static bool operator <(UniversalTime left, UniversalTime right)
            => left._splitJD < right._splitJD;

        /// <summary>
        /// Compares the two specified times to see if the left one is earlier
        /// than or equal to the right one.
        /// </summary>
        public static bool operator <=(UniversalTime left, UniversalTime right)
            => left._splitJD <= right._splitJD;

        /// <summary>
        /// Compares the two specified times to see if the left one is strictly
        /// later than the right one.
        /// </summary>
        public static bool operator >(UniversalTime left, UniversalTime right)
            => left._splitJD > right._splitJD;

        /// <summary>
        /// Compares the two specified times to see if the left one is later
        /// than or equal to the right one.
        /// </summary>
        public static bool operator >=(UniversalTime left, UniversalTime right)
            => left._splitJD >= right._splitJD;

        /// <summary>
        /// Indicates whether this time instance is earlier, later or the same
        /// as the specified one.
        /// </summary>
        public int CompareTo(UniversalTime other)
            => _splitJD.CompareTo(other._splitJD);

        /// <inheritdoc />
        int IComparable.CompareTo(object? obj)
            => obj is null ? 1
                : obj is UniversalTime time ? CompareTo(time)
                : throw EF.InvalidType(nameof(obj), typeof(UniversalTime), obj);
    }
}
