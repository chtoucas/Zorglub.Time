// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Astronomy
{
    using System.Runtime.InteropServices;

    using Zorglub.Time.Core;

    using static Zorglub.Time.Extensions.TimescaleExtensions;

    // TODO: ajoute calcul du (modified) Julian day number et de la fraction du
    // jour. Constructeurs alternatifs. Idem avec JulianDate.

    /// <summary>
    /// Represents a Modified Julian Date.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public readonly partial struct ModifiedJulianDate :
        IComparisonOperators<ModifiedJulianDate, ModifiedJulianDate>
    {
        /// <summary>
        /// Represents the numerical value of this instance.
        /// </summary>
        [FieldOffset(0)] private readonly double _value;

        /// <summary>
        /// Represents the timescale of this instance.
        /// </summary>
        [FieldOffset(8)] private readonly Timescale _timescale;

        /// <summary>
        /// Constructs a new instance of <see cref="ModifiedJulianDate"/> from
        /// the specified numerical value, within the TT timescale.
        /// <para>This is the right constructor to use when <paramref name="value"/>
        /// is the "Modified Julian Date" of the astrophysicists.</para>
        /// </summary>
        public ModifiedJulianDate(double value)
            : this(value, Timescale.Terrestrial) { }

        /// <summary>
        /// Constructs a new instance of <see cref="ModifiedJulianDate"/> from
        /// the specified timescale and numerical value.
        /// </summary>
        public ModifiedJulianDate(double value, Timescale timescale)
        {
            _value = value;
            _timescale = timescale;
        }

        /// <summary>
        /// Gets the numerical value from this Modified Julian Date instance.
        /// </summary>
        public double Value => _value;

        /// <summary>
        /// Gets the timescale from this Modified Julian Date instance.
        /// </summary>
        public Timescale Timescale => _timescale;

        /// <summary>
        /// Gets the modified "Julian day number" from this Modified Julian Date
        /// instance.
        /// <para>It is the number of integral days between this instant and
        /// november 17th, 1858 CE (gregorian) at midnight (0h).</para>
        /// <seealso cref="DaysSinceEpochAtMidnight"/>
        /// </summary>
        public double JulianDayNumber => Math.Truncate(_value);

        /// <summary>
        /// Gets the fraction of the day from this Modified Julian Date instance.
        /// </summary>
        public double FractionOfDay
        {
            get
            {
                double r = _value % 1;
                return r < 0 ? r + 1 : r;
            }
        }

        /// <summary>
        /// Gets the number of fractional days between this instant and
        /// november 17th, 1858 CE (gregorian) at midnight (0h).
        /// </summary>
        public double DaysSinceEpochAtMidnight => _value;

        /// <summary>
        /// Returns a culture-independent string representation of this
        /// Modified Julian Date instance.
        /// </summary>
        public override string ToString()
            => FormattableString.Invariant($"MJD({Timescale.GetAbbrName()}) {Value}");

        /// <summary>
        /// Deconstructs this Modified Julian Date instance into its components.
        /// </summary>
        public void Deconstruct(out Timescale timescale, out double value)
            => (timescale, value) = (_timescale, _value);

        /// <summary>
        /// Converts this Modified Julian Date instance to a Julian Date.
        /// <para>This transformation may incur a loss of precision.</para>
        /// </summary>
        [Pure]
        public AstronomicalJulianDate ToJulianDate()
            => new AstronomicalJulianDate(_value + JulianDateEpoch.Modified, _timescale);

        /// <summary>
        /// Obtains the Modified Julian date from the specified gregorian
        /// date and fraction of the day.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        [Pure]
        internal static double FromGregorianTime(Yemoda ymd, double fractionOfDay)
        {
            var (y, m, d) = ymd;

            int y0 = (m - 14) / 12;
            int y1 = y + y0;

            int mjdn = 1461 * (y1 + 4800) / 4
                + 367 * (m - 2 - 12 * y0) / 12
                - 3 * ((y1 + 4900) / 100) / 4
                + d - 2_432_076;

            // Pour rappel, la date julienne modifiée démarre à 0h.
            return mjdn + fractionOfDay;
        }
    }

    // Interface IEquatable<>.
    public partial struct ModifiedJulianDate
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="ModifiedJulianDate"/>
        /// are equal.
        /// </summary>
        public static bool operator ==(ModifiedJulianDate left, ModifiedJulianDate right)
            => left._timescale == right._timescale
                && MathOperations.AreApproximatelyEqual(left._value, right._value);

        /// <summary>
        /// Determines whether two specified instances of <see cref="ModifiedJulianDate"/>
        /// are not equal.
        /// </summary>
        public static bool operator !=(ModifiedJulianDate left, ModifiedJulianDate right)
            => !(left == right);

        /// <summary>
        /// Determines whether this instance is equal to the value of the
        /// specified <see cref="ModifiedJulianDate"/>.
        /// </summary>
        public bool Equals(ModifiedJulianDate other)
            => this == other;

        /// <summary>
        /// Determines whether this instance is equal to the value of the
        /// specified <see cref="ModifiedJulianDate"/> using the specified
        /// comparer for doubles.
        /// </summary>
        public bool Equals(ModifiedJulianDate other, IEqualityComparer<double> comparer)
            => comparer is null ? this == other
                : _timescale == other._timescale
                    && comparer.Equals(_value, other._value);

        /// <summary>
        /// Determines whether this instance is equal to a specified object.
        /// </summary>
        public override bool Equals([NotNullWhen(true)] object? obj)
            => obj is ModifiedJulianDate mjd && this == mjd;

        /// <summary>
        /// Obtains the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
            => HashCode.Combine(_timescale, _value);

        /// <summary>
        /// Obtains the hash code for this instance using the specified comparer
        /// for doubles.
        /// </summary>
        public int GetHashCode(IEqualityComparer<double> comparer) =>
            comparer is null ? HashCode.Combine(_timescale, _value)
            : HashCode.Combine(_timescale, comparer.GetHashCode(_value));
    }

    // Interfaces IComparable<> et IComparable.
    public partial struct ModifiedJulianDate
    {
        /// <summary>
        /// Compares the two specified Modified Julian Dates to see if the left
        /// one is strictly earlier than the right one.
        /// </summary>
        public static bool operator <(ModifiedJulianDate left, ModifiedJulianDate right)
            => left.CompareTo(right) < 0;

        /// <summary>
        /// Compares the two specified Modified Julian Dates to see if the left
        /// one is earlier than or equal to the right one.
        /// </summary>
        public static bool operator <=(ModifiedJulianDate left, ModifiedJulianDate right)
            => left.CompareTo(right) <= 0;

        /// <summary>
        /// Compares the two specified Modified Julian Dates to see if the left
        /// one is strictly later than the right one.
        /// </summary>
        public static bool operator >(ModifiedJulianDate left, ModifiedJulianDate right)
            => left.CompareTo(right) > 0;

        /// <summary>
        /// Compares the two specified Modified Julian Dates to see if the left
        /// one is later than or equal to the right one.
        /// </summary>
        public static bool operator >=(ModifiedJulianDate left, ModifiedJulianDate right)
            => left.CompareTo(right) >= 0;

        /// <summary>
        /// Indicates whether this Modified Julian Date instance is earlier,
        /// later or the same as the specified one.
        /// </summary>
        public int CompareTo(ModifiedJulianDate other)
        {
            if (_timescale != other._timescale)
                ThrowHelpers2.BadTimescale(nameof(other), _timescale, other._timescale);

            return _value.CompareTo(other._value);
        }

        /// <inheritdoc />
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is ModifiedJulianDate mjd ? CompareTo(mjd)
            : Throw.NonComparable(typeof(ModifiedJulianDate), obj);
    }
}
