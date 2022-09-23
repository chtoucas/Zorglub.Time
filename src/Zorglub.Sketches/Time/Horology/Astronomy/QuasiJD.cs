// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

// Adapted from SOFA source code.

namespace Zorglub.Time.Horology.Astronomy;

using Zorglub.Time.Core;

using static Zorglub.Time.Core.TemporalConstants;

/// <summary>
/// Represents a quasi Julian date.
/// <para><see cref="QuasiJD"/> is an immutable struct.</para>
/// </summary>
internal readonly partial struct QuasiJD : IEquatable<QuasiJD>, IComparable<QuasiJD>
{
    /// <summary>
    /// Represents the smallest possible value of a Julian date.
    /// <para>This field is constant.</para>
    /// </summary>
    public const double MinValue = SplitJD.MinValue;

    /// <summary>
    /// Represents the smallest possible value of a modified Julian date.
    /// <para>This field is constant.</para>
    /// </summary>
    public const double MinModifiedValue = SplitJD.MinModifiedValue;

    /// <summary>
    /// Represents the first invalid value (in the future) of a Julian date.
    /// <para>This field is constant.</para>
    /// </summary>
    public const double FirstInvalidValue = SplitJD.FirstInvalidValue;

    /// <summary>
    /// Represents the first invalid value (in the future) of a modified
    /// Julian date.
    /// <para>This field is constant.</para>
    /// </summary>
    public const double FirstInvalidModifiedValue = SplitJD.FirstInvalidModifiedValue;

    /// <summary>
    /// Represents the high-order part of this instance.
    /// </summary>
    private readonly double _high;

    /// <summary>
    /// Represents the low-order part of this instance.
    /// </summary>
    private readonly double _low;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuasiJD"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    private QuasiJD(double high, double low)
    {
        _high = high;
        _low = low;
    }

    /// <summary>
    /// Gets the Julian date from this instance.
    /// </summary>
    public double JulianDate => _high + _low;

    /// <summary>
    /// Gets the modified Julian date from this instance.
    /// </summary>
    public double ModifiedJulianDate =>
        _high == JulianDateEpoch.Modified ? _low
        : JulianDate - JulianDateEpoch.Modified;

    /// <summary>
    /// Deconstructs this instance into its high-order and low-order parts.
    /// </summary>
    public void Deconstruct(out double high, out double low) => (high, low) = (_high, _low);

    // fractionOfDay = almost always mean the fraction of the day from this
    // instance, which must be computed separately.
    [Pure]
    private Yemoda Tomorrow(double fractionOfDay)
    {
        // Principe : jd = _high + _low, on ajoute .5 pour se positionner
        // à 0h, on retranche fractionOfDay pour se retrouver ainsi au début
        // du jour, il suffit ensuite d'ajouter 1 pour obtenir le jour
        // d'après.
        int jdn = Jdn.FromJD(_high + 1.5, _low - fractionOfDay, out _);
        return Jdn.ToYemoda(jdn);
    }
}

// Fabriques.
internal partial struct QuasiJD
{
    /// <summary>
    /// Creates a new instance of <see cref="QuasiJD"/> from the specified
    /// Julian date.
    /// </summary>
    [Pure]
    public static QuasiJD FromJulianDate(double jd)
    {
        if (jd < MinValue || jd >= FirstInvalidValue)
        {
            throw new AoorException(nameof(jd));
        }

        return new QuasiJD(jd, 0);
    }

    /// <summary>
    /// Creates a new instance of <see cref="QuasiJD"/> from the specified
    /// Modified Julian date.
    /// </summary>
    [Pure]
    public static QuasiJD FromModifiedJulianDate(double mjd)
    {
        if (mjd < MinModifiedValue || mjd >= FirstInvalidModifiedValue)
        {
            throw new AoorException(nameof(mjd));
        }

        return new QuasiJD(JulianDateEpoch.Modified, mjd);
    }

    /// <summary>
    /// Creates a new instance of <see cref="UtcTime"/> from the specified
    /// gregorian date and time of the day.
    /// </summary>
    /// <remarks>Adapted from SOFA::iauDtf2d.</remarks>
    [Pure]
    public static QuasiJD FromClockTime(ClockTime0 clockTime)
    {
        // TODO: param validation.
        var (ymd, hh, mm, secs) = clockTime;

        // quasiJD = ymd at 0h (fractionOfDay = 0).
        var quasiJD = FromModifiedJulianDate(
            Horology.Astronomy.ModifiedJulianDate.FromGregorianTime(ymd, 0));
        Yemoda tomorrow = quasiJD.Tomorrow(0);

        double drift = UtcDrift.Compute(ymd, tomorrow);
        // If leap second day, correct the day and final minute lengths.
        double secondsInDay = SecondsPerDay + drift;

        // Attention, le jour peut durer 86401 ou 86399 secondes.
        //double seclim = 60;
        //if (hh == 23 && mm == 59)
        //{
        //    seclim += drift;
        //}

        // TODO: validate the time.

        double fractionOfDay = (3600 * hh + 60 * mm + secs) / secondsInDay;

        return new QuasiJD(quasiJD.JulianDate, fractionOfDay);
    }
}

// Conversions.
internal partial struct QuasiJD
{
    /// <summary>
    /// Converts this instance to a Julian day number and also returns the
    /// fraction of the day in an output parameter.
    /// </summary>
    [Pure]
    public int ToJulianDayNumber(out double fractionOfDay)
        => Jdn.FromJD(_high, _low, out fractionOfDay);

    /// <summary>
    /// Converts this instance to a Gregorian date and also returns the
    /// fraction of the day in an output parameter.
    /// </summary>
    [Pure]
    public Yemoda ToYemoda(out double fractionOfDay)
    {
        int jdn = ToJulianDayNumber(out fractionOfDay);
        return Jdn.ToYemoda(jdn);
    }

    /// <summary>
    /// Obtains the gregorian date and the time of the day for this time
    /// instance.
    /// </summary>
    /// <remarks>Adapted from SOFA::iauD2dtf.</remarks>
    [Pure]
    public (Yemoda ymd, int hour, int minute, int second, int fractionOfSecond)
        ToClockTime(int decimalPlaces)
    {
        var ymd = ToYemoda(out double fod);
        Yemoda tomorrow = Tomorrow(fod);
        double drift = UtcDrift.Compute(ymd, tomorrow);

        // If leap second day, scale the fraction of a day into SI.
        bool isLeapSecondDay = drift != 0;
        if (isLeapSecondDay)
        {
            fod += fod * drift / SecondsPerDay;
        }

        // REVIEW: même après correction, on a fod >= 0, non ?
        var (hh, mm, ss, fos) = ClockTime0.GetTimeOfDay(Math.Abs(fod), decimalPlaces, true);

        if (hh <= 23)
        {
            // The (rounded) time does not past 24h.
            return (ymd, hh, mm, ss, fos);
        }

        // Use tomorrow at 0h if one of the 3 conditions is met:
        // - today is not a leap second day.
        // - we past the leap second itself.
        // - rounding to 10s or coarser.
        if (!isLeapSecondDay || ss > 0 || decimalPlaces < 0)
        {
            return (Tomorrow(fod), 0, 0, 0, fos);
        }
        else
        {
            // Use 23 59 60... today.
            return (ymd, 23, 59, 60, fos);
        }
    }
}

// Interface IEquatable<>.
internal partial struct QuasiJD
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="QuasiJD"/>
    /// are equal.
    /// </summary>
    public static bool operator ==(QuasiJD left, QuasiJD right)
        => MathOperations.AreApproximatelyEqual(left._high, right._high)
            && MathOperations.AreApproximatelyEqual(left._low, right._low);

    /// <summary>
    /// Determines whether two specified instances of <see cref="QuasiJD"/>
    /// are not equal.
    /// </summary>
    public static bool operator !=(QuasiJD left, QuasiJD right) => !(left == right);

    /// <summary>
    /// Determines whether this instance is equal to the value of the
    /// specified <see cref="QuasiJD"/>.
    /// </summary>
    public bool Equals(QuasiJD other) => this == other;

    /// <summary>
    /// Determines whether this instance is equal to the value of the
    /// specified <see cref="QuasiJD"/> using the specified comparer for
    /// doubles.
    /// </summary>
    public bool Equals(QuasiJD other, IEqualityComparer<double> comparer) =>
        comparer is null ? this == other
        : comparer.Equals(_high, other._high)
            && comparer.Equals(_low, other._low);

    /// <summary>
    /// Determines whether this instance is equal to a specified object.
    /// </summary>
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is QuasiJD quasiJD && this == quasiJD;

    /// <summary>
    /// Obtains the hash code for this instance.
    /// </summary>
    public override int GetHashCode() => HashCode.Combine(_high, _low);

    /// <summary>
    /// Obtains the hash code for this instance using the specified comparer
    /// for doubles.
    /// </summary>
    public int GetHashCode(IEqualityComparer<double> comparer) =>
        comparer is null ? HashCode.Combine(_high, _low)
        : HashCode.Combine(
            comparer.GetHashCode(_high),
            comparer.GetHashCode(_low));
}

// Interface IComparable<>.
// Pour comparer deux valeurs, on utilise la date julienne car, dans bien
// des cas, on construit un "quasi JD" à partir d'une date julienne.
internal partial struct QuasiJD
{
    /// <summary>
    /// Compares the two specified quasi JDs to see if the left one is
    /// strictly earlier than the right one.
    /// </summary>
    public static bool operator <(QuasiJD left, QuasiJD right) =>
        left.JulianDate < right.JulianDate;

    /// <summary>
    /// Compares the two specified quasi JDs to see if the left one is
    /// earlier than or equal to the right one.
    /// </summary>
    public static bool operator <=(QuasiJD left, QuasiJD right) =>
        left.JulianDate <= right.JulianDate;

    /// <summary>
    /// Compares the two specified quasi JDs to see if the left one is
    /// strictly later than the right one.
    /// </summary>
    public static bool operator >(QuasiJD left, QuasiJD right) =>
        left.JulianDate > right.JulianDate;

    /// <summary>
    /// Compares the two specified quasi JDs to see if the left one is
    /// later than or equal to the right one.
    /// </summary>
    public static bool operator >=(QuasiJD left, QuasiJD right) =>
        left.JulianDate >= right.JulianDate;

    /// <summary>
    /// Indicates whether this quasi JD instance is earlier, later or the
    /// same as the specified one.
    /// </summary>
    public int CompareTo(QuasiJD other) => JulianDate.CompareTo(other.JulianDate);
}
