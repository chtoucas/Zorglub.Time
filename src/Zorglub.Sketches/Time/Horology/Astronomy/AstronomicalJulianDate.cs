// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Astronomy;

using System.Runtime.InteropServices;

using Zorglub.Time.Core;

using static Zorglub.Time.Extensions.TimescaleExtensions;

// Julian date: the interval of time in days and fractions of a day since
// 4713 B.C. January 1, Greenwich noon, approximately. The Julian date can
// be used with any time scale (TCG, TCB, TT, TDB). In precise work, the
// Julian date in TT, TCG and TCB, has its origin fixed according to the IAU
// 1991 Resolutions by the condition that on 1977 January 1, 00h 00m 00s TAI
// at the geocenter, the readings of TT, TCG and TCB are 1977 January 1,
// 00h 00m 32s.184 (JD 244 3144.5003725). The equivalent TDB reading depends
// on the adopted ephemeris (for example, the same reading for TDB(DE405) is
// JD 244 3144.5003725 − 65.564518 µs). The Modified Julian date is
// JD − 240 0000.5.
// [NFA Glossary](https://syrte.obspm.fr/iauWGnfa/NFA_Glossary.html)

// Resolution B1 of the IERS
// -------------------------
// https://www.iers.org/IERS/EN/Science/Recommendations/resolutionB1.html
//
// ON THE USE OF JULIAN DATES
// The XXIIIrd International Astronomical Union General Assembly,
//
// RECOGNIZING
//
// a. the need for a system of continuous dating for the purpose of
// analyzing time-varying astronomical data, and
//
// b. that both Julian Dates and Modified Julian Dates have been employed
// for this purpose in astronomy, geodesy, and geophysics.
//
// RECOMMENDS
//
// a. that Julian Date (as defined in the appendix) be used to record the
// instants of the occurrences of astronomical phenomena,
//
// b. that for those cases where it is convenient to employ a day beginning
// at midnight, the Modified Julian Date (equivalent to the Julian Date minus
// 2 400 000.5) be used, and
//
// c. that where there is any possibility of doubt regarding the usage of
// Modified Julian Date, care be exercised to state its definition
// specifically,
//
// d. that, in all languages, Julian Date be abbreviated by "JD" and Modified
// Julian Date be abbreviated by "MJD".
//
// APPENDIX. PROPOSED DEFINITIONS
//
// The following definitions are recommended
//
// 1. Julian day number (JDN)
//
// The Julian day number associated with the solar day is the number
// assigned to a day in a continuous count of days beginning with the Julian
// day number 0 assigned to the day starting at Greenwich mean noon on 1
// January 4713 BC, Julian proleptic calendar -4712.
//
// 2. Julian Date (JD)
//
// The Julian Date (JD) of any instant is the Julian day number for the
// preceding noon plus the fraction of the day since that instant. A Julian
// Date begins at 12h 0m 0s and is composed of 86400 seconds. To determine
// time intervals in a uniform time system it is necessary to express the JD
// in a uniform time scale. For that purpose it is recommended that JD be
// specified as SI seconds in Terrestrial Time (TT) where the length of day
// is 86,400 SI seconds.
//
// In some cases it may be necessary to specify Julian Date using a different
// time scale. (See Seidelmann, 1992, for an explanation of the various time
// scales in use). The time scale used should be indicated when required such
// as JD(UT1). It should be noted that time intervals calculated from
// differences of Julian Dates specified in non-uniform time scales, such as
// UTC, may need to be corrected for changes in time scales (e.g. leap seconds).
//
// An instant in time known in UTC can be converted to Terrestrial Time if
// such precision is required. Values of TT-UT are available using tables in
// McCarthy and Babcock (1986) and Stephenson and Morrison (1984, 1995).
// Table 1 provides the difference between TAI and UTC from 1961 through
// 1 January 1996. The difference between TT and UTC can be calculated
// knowing that TT = TAI + 32.184s. The Annual Reports of the International
// Earth Rotation Service should be consulted for dates after 1996.
//
// The data of Table 1 are also available electronically at
//
// http://hpiers.obspm.fr or
// ftp://hpiers.obspm.fr/iers/bul/bulc/UTC-TAI.history
// or at
// http://maia.usno.navy.mil or
// ftp://maia.usno.navy.mil/ser7/tai-utc.dat.
//
// Differences between the TAI and UTC time scales.
//
// TT-UTC can be calculated by adding 32.184s to TAI-UTC.
// -----------------------------------------------------------------
//
// 1961 Jan 1 JD 2437 300.5 TAI-UTC = 1.4228180s + (MJD - 37300.) x 0.001296s
// 1961 Aug 1 JD 2437 512.5 TAI-UTC = 1.3728180s + (MJD - 37300.) x 0.001296s
// 1962 Jan 1 JD 2437 665.5 TAI-UTC = 1.8458580s + (MJD - 37665.) x 0.0011232s
// 1963 Nov 1 JD 2438 334.5 TAI-UTC = 1.9458580s + (MJD - 37665.) x 0.0011232s
// 1964 Jan 1 JD 2438 395.5 TAI-UTC = 3.2401300s + (MJD - 38761.) x 0.001296s
// 1964 Apr 1 JD 2438 486.5 TAI-UTC = 3.3401300s + (MJD - 38761.) x 0.001296s
// 1964 Sep 1 JD 2438 639.5 TAI-UTC = 3.4401300s + (MJD - 38761.) x 0.001296s
// 1965 Jan 1 JD 2438 761.5 TAI-UTC = 3.5401300s + (MJD - 38761.) x 0.001296s
// 1965 Mar 1 JD 2438 820.5 TAI-UTC = 3.6401300s + (MJD - 38761.) x 0.001296s
// 1965 Jul 1 JD 2438 942.5 TAI-UTC = 3.7401300s + (MJD - 38761.) x 0.001296s
// 1965 Sep 1 JD 2439 004.5 TAI-UTC = 3.8401300s + (MJD - 38761.) x 0.001296s
// 1966 Jan 1 JD 2439 126.5 TAI-UTC = 4.3131700s + (MJD - 39126.) x 0.002592s
// 1968 Feb 1 JD 2439 887.5 TAI-UTC = 4.2131700s + (MJD - 39126.) x 0.002592s
// 1972 Jan 1 JD 2441 317.5 TAI-UTC = 10.0s
// 1972 Jul 1 JD 2441 499.5 TAI-UTC = 11.0s
// 1973 Jan 1 JD 2441 683.5 TAI-UTC = 12.0s
// 1974 Jan 1 JD 2442 048.5 TAI-UTC = 13.0s
// 1975 Jan 1 JD 2442 413.5 TAI-UTC = 14.0s
// 1976 Jan 1 JD 2442 778.5 TAI-UTC = 15.0s
// 1977 Jan 1 JD 2443 144.5 TAI-UTC = 16.0s
// 1978 Jan 1 JD 2443 509.5 TAI-UTC = 17.0s
// 1979 Jan 1 JD 2443 874.5 TAI-UTC = 18.0s
// 1980 Jan 1 JD 2444 239.5 TAI-UTC = 19.0s
// 1981 Jul 1 JD 2444 786.5 TAI-UTC = 20.0s
// 1982 Jul 1 JD 2445 151.5 TAI-UTC = 21.0s
// 1983 Jul 1 JD 2445 516.5 TAI-UTC = 22.0s
// 1985 Jul 1 JD 2446 247.5 TAI-UTC = 23.0s
// 1988 Jan 1 JD 2447 161.5 TAI-UTC = 24.0s
// 1990 Jan 1 JD 2447 892.5 TAI-UTC = 25.0s
// 1991 Jan 1 JD 2448 257.5 TAI-UTC = 26.0s
// 1992 Jul 1 JD 2448 804.5 TAI-UTC = 27.0s
// 1993 Jul 1 JD 2449 169.5 TAI-UTC = 28.0s
// 1994 Jul 1 JD 2449 534.5 TAI-UTC = 29.0s
// 1996 Jan 1 JD 2450 083.5 TAI-UTC = 30.0s
//
// Created: 1 Jan 2001. Text provided by the former Central Bureau.

/// <summary>
/// Represents an Astronomical Julian Date or simply Julian Date.
/// </summary>
[StructLayout(LayoutKind.Explicit, Pack = 1)]
public readonly partial struct AstronomicalJulianDate :
    IComparisonOperators<AstronomicalJulianDate, AstronomicalJulianDate>
{
    /// <summary>
    /// Represents the numerical value of this Julian Date instance.
    /// </summary>
    [FieldOffset(0)] private readonly double _value;

    /// <summary>
    /// Represents the time scale of this Julian Date instance.
    /// </summary>
    [FieldOffset(8)] private readonly Timescale _timescale;

    /// <summary>
    /// Initializes a new instance of the <see cref="AstronomicalJulianDate"/> struct from the
    /// specified numerical value, within the TT time scale.
    /// <para>This is the right constructor to use when <paramref name="value"/>
    /// is the "Julian Date" of the astrophysicists.</para>
    /// </summary>
    public AstronomicalJulianDate(double value)
        : this(value, Timescale.Terrestrial) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AstronomicalJulianDate"/> struct from the
    /// specified time scale and numerical value.
    /// </summary>
    public AstronomicalJulianDate(double value, Timescale timescale)
    {
        _value = value;
        _timescale = timescale;
    }

    /// <summary>
    /// Gets the numerical value from this Julian Date instance.
    /// </summary>
    public double Value => _value;

    /// <summary>
    /// Gets the time scale from this Julian Date instance.
    /// </summary>
    public Timescale Timescale => _timescale;

    /// <summary>
    /// Gets the "Julian day number" from this Julian Date instance.
    /// <para>It is the number of integral days between this instant and
    /// january 1st, 4713 BC (julian) at noon (12h).</para>
    /// <seealso cref="DaysSinceEpochAtMidnight"/>
    /// </summary>
    // Identique à Moment.NychthemeronsSinceEpoch
    public double JulianDayNumber => Math.Truncate(_value);

    /// <summary>
    /// Gets the fraction of the day from this Julian Date instance.
    /// </summary>
    public double FractionOfDay
    {
        get
        {
            double r = (_value + .5) % 1;
            return r < 0 ? r + 1 : r;
        }
    }

    /// <summary>
    /// Gets the number of fractional days between this instant and
    /// january 1st, 4713 BC (julian) at midnight (0h).
    /// </summary>
    public double DaysSinceEpochAtMidnight => _value + .5;

    // TODO: formatting w/ decimalPlaces.

    /// <summary>
    /// Returns a culture-independent string representation of this Julian
    /// Date instance.
    /// </summary>
    public override string ToString() =>
        FormattableString.Invariant($"JD({Timescale.GetAbbrName()}) {Value}");

    /// <summary>
    /// Deconstructs this Julian Date instance into its components.
    /// </summary>
    public void Deconstruct(out Timescale timescale, out double value) =>
        (timescale, value) = (_timescale, _value);

    /// <summary>
    /// Converts this Julian Date instance to a Modified Julian Date.
    /// <para>This transformation may incur a loss of precision.</para>
    /// </summary>
    [Pure]
    public ModifiedJulianDate ToModifiedJulianDate() =>
        new(_value - JulianDateEpoch.Modified, _timescale);

    /// <summary>
    /// Obtains the Modified Julian date from the specified gregorian
    /// date and fraction of the day.
    /// <para>This method does NOT validate its parameters.</para>
    /// </summary>
    [Pure]
    internal static double FromGregorianTime(Yemoda ymd, double fractionOfDay) =>
        // Pour rappel, la date julienne démarre à 12h.
        Jdn.FromYemoda(ymd) + (fractionOfDay - .5);
}

// Interface IEquatable<>.
public partial struct AstronomicalJulianDate
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="AstronomicalJulianDate"/>
    /// are equal.
    /// </summary>
    public static bool operator ==(AstronomicalJulianDate left, AstronomicalJulianDate right) =>
        left._timescale == right._timescale
            && MathOperations.AreApproximatelyEqual(left._value, right._value);

    /// <summary>
    /// Determines whether two specified instances of <see cref="AstronomicalJulianDate"/>
    /// are not equal.
    /// </summary>
    public static bool operator !=(AstronomicalJulianDate left, AstronomicalJulianDate right) => !(left == right);

    /// <summary>
    /// Determines whether this instance is equal to the value of the
    /// specified <see cref="AstronomicalJulianDate"/>.
    /// </summary>
    public bool Equals(AstronomicalJulianDate other) => this == other;

    /// <summary>
    /// Determines whether this instance is equal to the value of the
    /// specified <see cref="AstronomicalJulianDate"/> using the specified comparer for
    /// doubles.
    /// </summary>
    public bool Equals(AstronomicalJulianDate other, IEqualityComparer<double> comparer) =>
        comparer is null ? this == other
        : _timescale == other._timescale
            && comparer.Equals(_value, other._value);

    /// <summary>
    /// Determines whether this instance is equal to a specified object.
    /// </summary>
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is AstronomicalJulianDate jd && this == jd;

    /// <summary>
    /// Obtains the hash code for this instance.
    /// </summary>
    public override int GetHashCode() =>
        HashCode.Combine(_timescale, _value);

    /// <summary>
    /// Obtains the hash code for this instance using the specified comparer
    /// for doubles.
    /// </summary>
    public int GetHashCode(IEqualityComparer<double> comparer) =>
        comparer is null ? HashCode.Combine(_timescale, _value)
        : HashCode.Combine(_timescale, comparer.GetHashCode(_value));
}

// Interfaces IComparable<> et IComparable.
public partial struct AstronomicalJulianDate
{
    /// <summary>
    /// Compares the two specified Julian Dates to see if the left one is
    /// strictly earlier than the right one.
    /// </summary>
    public static bool operator <(AstronomicalJulianDate left, AstronomicalJulianDate right)
        => left.CompareTo(right) < 0;

    /// <summary>
    /// Compares the two specified Julian Dates to see if the left one is
    /// earlier than or equal to the right one.
    /// </summary>
    public static bool operator <=(AstronomicalJulianDate left, AstronomicalJulianDate right)
        => left.CompareTo(right) <= 0;

    /// <summary>
    /// Compares the two specified Julian Dates to see if the left one is
    /// strictly later than the right one.
    /// </summary>
    public static bool operator >(AstronomicalJulianDate left, AstronomicalJulianDate right)
        => left.CompareTo(right) > 0;

    /// <summary>
    /// Compares the two specified Julian Dates to see if the left one is
    /// later than or equal to the right one.
    /// </summary>
    public static bool operator >=(AstronomicalJulianDate left, AstronomicalJulianDate right)
        => left.CompareTo(right) >= 0;

    /// <summary>
    /// Indicates whether this Julian Date instance is earlier, later or the
    /// same as the specified one.
    /// </summary>
    public int CompareTo(AstronomicalJulianDate other)
    {
        if (_timescale != other._timescale)
            ThrowHelpers2.BadTimescale(nameof(other), _timescale, other._timescale);

        return _value.CompareTo(other._value);
    }

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is AstronomicalJulianDate jd ? CompareTo(jd)
        : Throw.NonComparable(typeof(AstronomicalJulianDate), obj);
}
