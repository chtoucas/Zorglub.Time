// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology;

    /// <summary>
    /// Represents a pair of a year and a month.
    /// <para><see cref="MonthFields"/> is an immutable struct.</para>
    /// </summary>
    /// <remarks>
    /// <para>A <see cref="MonthFields"/> does NOT represent a month.</para>
    /// </remarks>
    public readonly partial struct MonthFields : IEqualityOperators<MonthFields, MonthFields>
    {
        // We make sure that default(MonthFields) is such that Month = 1,
        // not 0 which is surely invalid.

        /// <summary>
        /// Represents the algebraic month of the year (start at zero).
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _month0;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthFields"/> struct from the specified
        /// month parts.
        /// </summary>
        public MonthFields(Yemo parts)
        {
            parts.Unpack(out int y, out int m);
            Year = y;
            _month0 = m - 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthFields"/> struct from the specified
        /// year and month values.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="month"/> is a negative integer.
        /// </exception>
        public MonthFields(int year, int month)
        {
            if (month < 1) Throw.MonthOutOfRange(month);

            Year = year;
            _month0 = month - 1;
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
        /// Gets the month of the year.
        /// </summary>
        public int Month => _month0 + 1;

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString() => FormattableString.Invariant($"{Month:D2}/{Year:D4}");

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int month) => (year, month) = (Year, Month);

        /// <summary>
        /// Converts the current instance to a <see cref="Yemo"/> value.
        /// </summary>
        /// <exception cref="AoorException">The current instance is not representable by a
        /// <see cref="Yemo"/>; one of the value is too large to be handled by the system.
        /// </exception>
        [Pure]
        public Yemo ToYemo() => Yemo.Create(Year, Month);
    }

    public partial struct MonthFields // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="MonthFields"/> are equal.
        /// </summary>
        public static bool operator ==(MonthFields left, MonthFields right) =>
            left.Year == right.Year && left._month0 == right._month0;

        /// <summary>
        /// Determines whether two specified instances of <see cref="MonthFields"/> are not equal.
        /// </summary>
        public static bool operator !=(MonthFields left, MonthFields right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(MonthFields other) => this == other;

        /// <inheritdoc />
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is MonthFields parts && this == parts;

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Year, Month);
    }
}
