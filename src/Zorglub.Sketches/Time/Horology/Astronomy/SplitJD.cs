// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

// Adapted from SOFA source code.

namespace Zorglub.Time.Horology.Astronomy
{
    using Zorglub.Time.Core;

    // FIXME: inégalités (idem avec QuasiJD, JulianDate et ModifiedJulianDate).
    // Document the rules for equality in all time types.

    /// <summary>
    /// Represents a two-part Julian date.
    /// <para><see cref="SplitJD"/> is an immutable struct.</para>
    /// </summary>
    /// <remarks>
    /// The main advantage of a two-part Julian date over a Julian date is
    /// numerical accuracy. For instance, a Modified Julian date is stored
    /// exactly, the high-order part is the julian date of the MJD epoch and the
    /// low-order part is the MJD.
    /// </remarks>
    internal readonly partial struct SplitJD : IEquatable<SplitJD>, IComparable<SplitJD>
    {
        /// <summary>
        /// Represents the smallest possible value of a Julian date.
        /// <para>This field is constant.</para>
        /// </summary>
        // SOFA utilise -68.569,5 càd
        // - le 1er mars -4900 (grégorien) à 0h.
        // - le 8 avril -4901 (julien) à 0h.
        // Pourquoi ces valeurs ? En ce qui nous concerne, on va plutôt s'adapter
        // à ClockTime.MinYear.
        // TODO: on se place dans l'échelle TAI, que se passe-t-il dans les
        // autres ?
        public const double MinValue = 37.5;

        /// <summary>
        /// Represents the smallest possible value of a modified Julian date.
        /// <para>This field is constant.</para>
        /// </summary>
        public const double MinModifiedValue = -2_399_963;

        /// <summary>
        /// Represents the first invalid value (in the future) of a Julian date.
        /// <para>This field is constant.</para>
        /// </summary>
        // SOFA utilise 1e9. On préfère se synchroniser avec ClockTime.
        // Précisément, il s'agit de la borne supérieure de l'ensemble
        // [1/1/-4712 à 0h, 1/1/10.000 à 0h]. La dernière valeur valide est
        // l'instant **précédant** cette borne supérieure.
        public const double FirstInvalidValue = 5_373_484.5;

        /// <summary>
        /// Represents the first invalid value (in the future) of a modified
        /// Julian date.
        /// <para>This field is constant.</para>
        /// </summary>
        public const double FirstInvalidModifiedValue = 2_973_484;

        /// <summary>
        /// Represents the high-order part of this instance.
        /// </summary>
        private readonly double _high;

        /// <summary>
        /// Represents the low-order part of this instance.
        /// </summary>
        private readonly double _low;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitJD"/> struct.
        /// <para>This constructor does NOT validate its parameters.</para>
        /// </summary>
        private SplitJD(double high, double low)
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
        public double ModifiedJulianDate
            => _high == JulianDateEpoch.Modified ? _low
                : JulianDate - JulianDateEpoch.Modified;

        /// <summary>
        /// Deconstructs this instance into its high-order and low-order parts.
        /// </summary>
        public void Deconstruct(out double high, out double low)
            => (high, low) = (_high, _low);

        /// <summary>
        /// Obtains the high-order and low-order parts such that |hi| > |lo|.
        /// </summary>
        public void GetOrderedParts(out double hi, out double lo)
        {
            if (Math.Abs(_high) > Math.Abs(_low))
            {
                hi = _high;
                lo = _low;
            }
            else
            {
                hi = _low;
                lo = _high;
            }
        }
    }

    // Fabriques.
    internal partial struct SplitJD
    {
        /// <summary>
        /// Creates a new instance of <see cref="SplitJD"/> from the specified
        /// Julian date.
        /// </summary>
        [Pure]
        public static SplitJD FromJulianDate(double jd)
        {
            if (jd < MinValue || jd >= FirstInvalidValue)
            {
                throw new ArgumentOutOfRangeException(nameof(jd));
            }

            return new SplitJD(jd, 0);
        }

        /// <summary>
        /// Creates a new instance of <see cref="SplitJD"/> from the specified
        /// Modified Julian date.
        /// </summary>
        [Pure]
        public static SplitJD FromModifiedJulianDate(double mjd)
        {
            if (mjd < MinModifiedValue || mjd >= FirstInvalidModifiedValue)
            {
                throw new ArgumentOutOfRangeException(nameof(mjd));
            }

            return new SplitJD(JulianDateEpoch.Modified, mjd);
        }

        /// <summary>
        /// Creates a new instance of <see cref="SplitJD"/> from the specified
        /// numerical value and Julian date version.
        /// </summary>
        [Pure]
        public static SplitJD Create(double value, JulianDateVersion version)
            // TODO: param validation.
            => version switch
            {
                JulianDateVersion.Astronomical => new SplitJD(value, 0),
                JulianDateVersion.Modified => new SplitJD(JulianDateEpoch.Modified, value),
                JulianDateVersion.Ccsds => new SplitJD(JulianDateEpoch.Ccsds, value),
                JulianDateVersion.Cnes => new SplitJD(JulianDateEpoch.Cnes, value),
                JulianDateVersion.Dublin => new SplitJD(JulianDateEpoch.Dublin, value),
                JulianDateVersion.Reduced => new SplitJD(JulianDateEpoch.Reduced, value),
                JulianDateVersion.Truncated => new SplitJD(JulianDateEpoch.Truncated, value),
                _ => Throw.Unreachable<SplitJD>(),
            };
    }

    // Conversions.
    internal partial struct SplitJD
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
    }

    // Interface IEquatable<>.
    internal partial struct SplitJD
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="SplitJD"/>
        /// are equal.
        /// </summary>
        public static bool operator ==(SplitJD left, SplitJD right)
            => MathOperations.AreApproximatelyEqual(left._high, right._high)
                && MathOperations.AreApproximatelyEqual(left._low, right._low);

        /// <summary>
        /// Determines whether two specified instances of <see cref="SplitJD"/>
        /// are not equal.
        /// </summary>
        public static bool operator !=(SplitJD left, SplitJD right)
            => !(left == right);

        /// <summary>
        /// Determines whether this instance is equal to the value of the
        /// specified <see cref="SplitJD"/>.
        /// </summary>
        public bool Equals(SplitJD other)
            => this == other;

        /// <summary>
        /// Determines whether this instance is equal to the value of the
        /// specified <see cref="SplitJD"/> using the specified comparer for
        /// doubles.
        /// </summary>
        public bool Equals(SplitJD other, IEqualityComparer<double> comparer)
            => comparer is null ? this == other
                : comparer.Equals(_high, other._high)
                    && comparer.Equals(_low, other._low);

        /// <summary>
        /// Determines whether this instance is equal to a specified object.
        /// </summary>
        public override bool Equals([NotNullWhen(true)] object? obj)
            => obj is SplitJD splitJD && this == splitJD;

        /// <summary>
        /// Obtains the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
            => HashCode.Combine(_high, _low);

        /// <summary>
        /// Obtains the hash code for this instance using the specified comparer
        /// for doubles.
        /// </summary>
        // REVIEW: on devrait utiliser un IEqualityComparer<SplitJD>. Ce n'est
        // pas vraiment fonctionnel en l'état non ?
        public int GetHashCode(IEqualityComparer<double> comparer)
            => comparer is null ? HashCode.Combine(_high, _low)
                : HashCode.Combine(
                    comparer.GetHashCode(_high),
                    comparer.GetHashCode(_low));
    }

    // Interface IComparable<>.
    // Pour comparer deux valeurs, on utilise la date julienne modifiée car,
    // dans bien des cas, on construit un "two-part JD" à partir d'une date
    // julienne modifiée.
    internal partial struct SplitJD
    {
        /// <summary>
        /// Compares the two specified two-part JDs to see if the left one is
        /// strictly earlier than the right one.
        /// </summary>
        public static bool operator <(SplitJD left, SplitJD right)
            => left.ModifiedJulianDate < right.ModifiedJulianDate;

        /// <summary>
        /// Compares the two specified two-part JDs to see if the left one is
        /// earlier than or equal to the right one.
        /// </summary>
        public static bool operator <=(SplitJD left, SplitJD right)
            => left.ModifiedJulianDate <= right.ModifiedJulianDate;

        /// <summary>
        /// Compares the two specified two-part JDs to see if the left one is
        /// strictly later than the right one.
        /// </summary>
        public static bool operator >(SplitJD left, SplitJD right)
            => left.ModifiedJulianDate > right.ModifiedJulianDate;

        /// <summary>
        /// Compares the two specified two-part JDs to see if the left one is
        /// later than or equal to the right one.
        /// </summary>
        public static bool operator >=(SplitJD left, SplitJD right)
            => left.ModifiedJulianDate >= right.ModifiedJulianDate;

        /// <summary>
        /// Indicates whether this two-part JD instance is earlier, later or the
        /// same as the specified one.
        /// </summary>
        public int CompareTo(SplitJD other)
            => ModifiedJulianDate.CompareTo(other.ModifiedJulianDate);
    }

    // Opérations arithmétiques.
    internal partial struct SplitJD
    {
        /// <summary>
        /// Adds an offset to the specified two-part JD, yielding a new
        /// two-part JD.
        /// </summary>
        public static SplitJD operator +(SplitJD value, double offset)
        {
            value.GetOrderedParts(out double hi, out double lo);
            return new SplitJD(hi, lo + offset);
        }

        /// <summary>
        /// Subtracts an offset to the specified two-part JD, yielding a new
        /// two-part JD.
        /// </summary>
        public static SplitJD operator -(SplitJD value, double offset)
        {
            value.GetOrderedParts(out double hi, out double lo);
            return new SplitJD(hi, lo - offset);
        }
    }
}
