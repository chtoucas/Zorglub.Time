// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.
//
// Adapted from SOFA source code.

namespace Zorglub.Time.Horology.Astronomy
{
    using Zorglub.Time.Core;

    using static Zorglub.Time.Core.TemporalConstants;

    // International Atomic Time (TAI): a widely used practical realization of
    // TT with a fixed shift from the latter due to historical reasons (see TT);
    // it is a continuous time scale, now calculated at the Bureau International
    // des Poids et Mesures (BIPM), using data from some three hundred atomic
    // clocks in over fifty national laboratories in accordance with the
    // definition of the SI second.
    // [NFA Glossary](https://syrte.obspm.fr/iauWGnfa/NFA_Glossary.html)

    /// <summary>
    /// Represents an International Atomic Time (TAI).
    /// <para><see cref="AtomicTime"/> is an immutable struct.</para>
    /// </summary>
    /// <remarks>
    /// <para>The unit of TAI is the SI second.</para>
    /// <para>Two instances of <see cref="AtomicTime"/> are considered equal
    /// if they have the same internal representation (a two-part Julian date),
    /// this is NOT the same as numerical equality. As a consequence, two
    /// instances may ultimately represent the same point in time but be
    /// regarded as different by .NET.</para>
    /// </remarks>
    public readonly partial struct AtomicTime :
        IComparisonOperators<AtomicTime, AtomicTime>
    {
        /// <summary>
        /// Represents the two-part Julian date of this time instance.
        /// </summary>
        private readonly SplitJD _splitJD;

        /// <summary>
        /// Constructs a new instance of <see cref="AtomicTime"/> from the
        /// specified two-part Julian date.
        /// </summary>
        internal AtomicTime(SplitJD splitJD) => _splitJD = splitJD;
    }

    // Fabriques.
    public partial struct AtomicTime
    {
        /// <summary>
        /// Creates a new instance of <see cref="AtomicTime"/> from the
        /// specified Julian Date.
        /// </summary>
        public static AtomicTime FromJulianDate(AstronomicalJulianDate jd)
        {
            if (jd.Timescale != Timescale.Atomic)
                ThrowHelpers2.BadTimescale(nameof(jd), Timescale.Atomic, jd.Timescale);

            var splitJD = SplitJD.FromJulianDate(jd.Value);
            return new AtomicTime(splitJD);
        }

        /// <summary>
        /// Creates a new instance of <see cref="AtomicTime"/> from the
        /// specified Modified Julian Date.
        /// </summary>
        public static AtomicTime FromModifiedJulianDate(ModifiedJulianDate mjd)
        {
            if (mjd.Timescale != Timescale.Atomic)
                ThrowHelpers2.BadTimescale(nameof(mjd), Timescale.Atomic, mjd.Timescale);

            var splitJD = SplitJD.FromModifiedJulianDate(mjd.Value);
            return new AtomicTime(splitJD);
        }

        /// <summary>
        /// Creates a new instance of <see cref="AtomicTime"/> from the specified
        /// clock time.
        /// </summary>
        [Pure]
        public static AtomicTime FromClockTime(ClockTime0 clockTime)
        {
            if (clockTime is null)
            {
                throw new ArgumentNullException(nameof(clockTime));
            }

            // REVIEW: SOFA construit le SplitJD différemment :
            // > long mjdn = ModifiedJulianDate.FromGregorianDate(clockTime.Yemoda);
            // > double high = JulianDateEpoch.Modified + mjdn;
            // > double low = clockTime.FractionOfDay;

            double mjd = ModifiedJulianDate.FromGregorianTime(
                clockTime.Yemoda, clockTime.FractionOfDay);

            return new AtomicTime(SplitJD.FromModifiedJulianDate(mjd));
        }
    }

    // Conversions.
    public partial struct AtomicTime
    {
        /// <summary>
        /// Converts this instance to a <see cref="DayNumber"/> in the
        /// Astronomical Julian dayscale and also returns the fraction of the
        /// day in an output parameter.
        /// </summary>
        [Pure]
        public DayNumber ToDayNumber(out double fractionOfDay)
        {
            //int jdn = _splitJD.ToJulianDayNumber(out fractionOfDay);
            //return new DayNumber(1 + jdn, DayscaleId.AstronomicalJulian);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts this instance to an Astronomical Julian Date within the TAI
        /// timescale.
        /// </summary>
        [Pure]
        public AstronomicalJulianDate ToJulianDate() => new(_splitJD.JulianDate, Timescale.Atomic);

        /// <summary>
        /// Converts this instance to a Modified Julian Date within the TAI
        /// timescale.
        /// </summary>
        [Pure]
        public ModifiedJulianDate ToModifiedJulianDate() =>
            new(_splitJD.ModifiedJulianDate, Timescale.Atomic);

        /// <summary>
        /// Obtains the gregorian date and the time of the day for this time
        /// instance.
        /// </summary>
        [Pure]
        public (Yemoda ymd, int hour, int minute, int second, int fractionOfSecond)
            ToClockTime(int decimalPlaces)
        {
            var ymd = _splitJD.ToYemoda(out double fod);
            var (hh, mm, ss, fos) = ClockTime0.GetTimeOfDay(fod, decimalPlaces, isUtc: false);
            return (ymd, hh, mm, ss, fos);
        }

        /// <summary>
        /// Converts this instance to a Terrestrial Time.
        /// </summary>
        [Pure]
        public TerrestrialTime ToTerrestrialTime() => new(_splitJD + DeltaTAT);

        /// <summary>
        /// Converts this instance to a Universal Time.
        /// </summary>
        /// <remarks>
        /// <paramref name="deltaTA"/> is the difference between UT1 and TAI,
        /// ΔTA = UT1-TAI in seconds, available from IERS Bulletins.
        /// </remarks>
        [Pure]
        public UniversalTime ToUniversalTime(double deltaTA) =>
            new(_splitJD + deltaTA / SecondsPerDay);
    }

    // Interface IEquatable<>.
    public partial struct AtomicTime
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="AtomicTime"/>
        /// are equal.
        /// </summary>
        public static bool operator ==(AtomicTime left, AtomicTime right) =>
            left._splitJD == right._splitJD;

        /// <summary>
        /// Determines whether two specified instances of <see cref="AtomicTime"/>
        /// are not equal.
        /// </summary>
        public static bool operator !=(AtomicTime left, AtomicTime right) =>
            left._splitJD != right._splitJD;

        /// <summary>
        /// Determines whether this instance is equal to the value of the
        /// specified <see cref="AtomicTime"/>.
        /// </summary>
        public bool Equals(AtomicTime other) => _splitJD == other._splitJD;

        /// <summary>
        /// Determines whether this instance is equal to a specified object.
        /// </summary>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is AtomicTime time && Equals(time);

        /// <summary>
        /// Obtains the hash code for this instance.
        /// </summary>
        public override int GetHashCode() => _splitJD.GetHashCode();
    }

    // Interfaces IComparable<> et IComparable.
    public partial struct AtomicTime
    {
        /// <summary>
        /// Compares the two specified times to see if the left one is strictly
        /// earlier than the right one.
        /// </summary>
        public static bool operator <(AtomicTime left, AtomicTime right) =>
            left._splitJD < right._splitJD;

        /// <summary>
        /// Compares the two specified times to see if the left one is earlier
        /// than or equal to the right one.
        /// </summary>
        public static bool operator <=(AtomicTime left, AtomicTime right) =>
            left._splitJD <= right._splitJD;

        /// <summary>
        /// Compares the two specified times to see if the left one is strictly
        /// later than the right one.
        /// </summary>
        public static bool operator >(AtomicTime left, AtomicTime right) =>
            left._splitJD > right._splitJD;

        /// <summary>
        /// Compares the two specified times to see if the left one is later
        /// than or equal to the right one.
        /// </summary>
        public static bool operator >=(AtomicTime left, AtomicTime right) =>
            left._splitJD >= right._splitJD;

        /// <summary>
        /// Indicates whether this time instance is earlier, later or the same
        /// as the specified one.
        /// </summary>
        public int CompareTo(AtomicTime other) => _splitJD.CompareTo(other._splitJD);

        /// <inheritdoc />
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is AtomicTime time ? CompareTo(time)
            : Throw.NonComparable(typeof(AtomicTime), obj);
    }
}
