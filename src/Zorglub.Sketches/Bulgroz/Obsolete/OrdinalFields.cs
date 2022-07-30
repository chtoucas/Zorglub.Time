// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Obsolete
{
    using Zorglub.Time;
    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology;

    /// <summary>
    /// Represents a pair of a year and a day of the year.
    /// <para><see cref="OrdinalFields"/> is an immutable struct.</para>
    /// </summary>
    /// <remarks>
    /// <para>An <see cref="OrdinalFields"/> does NOT represent an ordinal date.</para>
    /// </remarks>
    public readonly partial struct OrdinalFields : IEqualityOperators<OrdinalFields, OrdinalFields>
    {
        // We make sure that default(OrdinalFields) is such that DayOfYear = 1,
        // not 0 which is surely invalid.

        /// <summary>
        /// Represents the algebraic day of the year (start at zero).
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _day0fYear;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdinalFields"/> struct from the specified
        /// ordinal parts.
        /// </summary>
        public OrdinalFields(Yedoy parts)
        {
            parts.Unpack(out int y, out int doy);
            Year = y;
            _day0fYear = doy - 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdinalFields"/> struct from the specified
        /// year and day of the year values.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayOfYear"/> is a negative integer.
        /// </exception>
        public OrdinalFields(int year, int dayOfYear)
        {
            if (dayOfYear < 1) Throw.DayOfYearOutOfRange(dayOfYear);

            Year = year;
            _day0fYear = dayOfYear - 1;
        }

        /// <summary>
        /// Gets the century of the era.
        /// </summary>
        public Ord CenturyOfEra => Ord.FromInt32(Century);

        /// <summary>
        /// Gets the century number.
        /// </summary>
        public int Century => YearNumbering.GetCentury(Year);

        /// <summary>
        /// Gets the year of the era.
        /// </summary>
        /// <exception cref="AoorException"><see cref="Year"/> is lower than
        /// <see cref="Ord.MinAlgebraicValue"/>.</exception>
        public Ord YearOfEra => Ord.FromInt32(Year);

        /// <summary>
        /// Gets the year of the century.
        /// <para>The result is in the range from 1 to 100.</para>
        /// </summary>
        public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

        /// <summary>
        /// Gets the (algebraic) year number.
        /// </summary>
        public int Year { get; }

        /// <summary>
        /// Gets the day of the year.
        /// </summary>
        public int DayOfYear => _day0fYear + 1;

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString() =>
            FormattableString.Invariant($"{DayOfYear:D3}/{Year:D4}");

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int dayOfYear) =>
            (year, dayOfYear) = (Year, DayOfYear);

        /// <summary>
        /// Converts the current instance to a <see cref="Yedoy"/> value.
        /// </summary>
        /// <exception cref="AoorException">The current instance is not representable by a
        /// <see cref="Yedoy"/>; one of the value is too large to be handled by the system.
        /// </exception>
        [Pure]
        public Yedoy ToYedoy() => Yedoy.Create(Year, DayOfYear);
    }

    public partial struct OrdinalFields // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="OrdinalFields"/> are equal.
        /// </summary>
        public static bool operator ==(OrdinalFields left, OrdinalFields right) =>
            left.Year == right.Year && left._day0fYear == right._day0fYear;

        /// <summary>
        /// Determines whether two specified instances of <see cref="OrdinalFields"/> are not equal.
        /// </summary>
        public static bool operator !=(OrdinalFields left, OrdinalFields right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(OrdinalFields other) => this == other;

        /// <inheritdoc />
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is OrdinalFields parts && this == parts;

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Year, _day0fYear);
    }
}
