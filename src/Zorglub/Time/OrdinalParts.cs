// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology;

    /// <summary>
    /// Represents a pair of a year and a day of the year.
    /// <para><see cref="OrdinalParts"/> is an immutable struct.</para>
    /// </summary>
    /// <remarks>
    /// <para>An <see cref="OrdinalParts"/> does NOT represent an ordinal date.</para>
    /// </remarks>
    public readonly partial struct OrdinalParts : IEqualityOperators<OrdinalParts, OrdinalParts>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrdinalParts"/> struct from the specified
        /// ordinal parts.
        /// </summary>
        public OrdinalParts(Yedoy parts)
        {
            parts.Unpack(out int y, out int doy);
            Year = y;
            DayOfYear = doy;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdinalParts"/> struct from the specified
        /// year and day of the year values.
        /// </summary>
        public OrdinalParts(int year, int dayOfYear)
        {
            Year = year;
            DayOfYear = dayOfYear;
        }

        /// <summary>
        /// Gets the (algebraic) year number.
        /// </summary>
        public int Year { get; }

        /// <summary>
        /// Gets the day of the year.
        /// </summary>
        public int DayOfYear { get; }

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

        /// <summary>
        /// Validates the current instance with the specified scope then converts it to a
        /// <see cref="Yedoy"/> value.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="AoorException">The current instance is not valid according to the
        /// specified scope.</exception>
        [Pure]
        public Yedoy ToYedoy(ICalendarScope scope)
        {
            Requires.NotNull(scope);

            scope.ValidateOrdinal(Year, DayOfYear);
            return new Yedoy(Year, DayOfYear);
        }
    }

    public partial struct OrdinalParts // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="OrdinalParts"/> are equal.
        /// </summary>
        public static bool operator ==(OrdinalParts left, OrdinalParts right) =>
            left.Year == right.Year && left.DayOfYear == right.DayOfYear;

        /// <summary>
        /// Determines whether two specified instances of <see cref="OrdinalParts"/> are not equal.
        /// </summary>
        public static bool operator !=(OrdinalParts left, OrdinalParts right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(OrdinalParts other) => this == other;

        /// <inheritdoc />
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is OrdinalParts parts && this == parts;

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Year, DayOfYear);
    }
}
