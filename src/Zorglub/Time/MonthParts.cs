﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology;

    /// <summary>
    /// Represents a pair of a year and a month.
    /// <para><see cref="MonthParts"/> is an immutable struct.</para>
    /// </summary>
    /// <remarks>
    /// <para>A <see cref="MonthParts"/> does NOT represent a month.</para>
    /// </remarks>
    public readonly partial struct MonthParts : IEqualityOperators<MonthParts, MonthParts>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonthParts"/> struct from the specified
        /// month parts.
        /// </summary>
        public MonthParts(Yemo parts)
        {
            parts.Unpack(out int y, out int m);
            Year = y;
            Month = m;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthParts"/> struct from the specified
        /// year and month values.
        /// </summary>
        public MonthParts(int year, int month)
        {
            Year = year;
            Month = month;
        }

        /// <summary>
        /// Gets the (algebraic) year number.
        /// </summary>
        public int Year { get; }

        /// <summary>
        /// Gets the month of the year.
        /// </summary>
        public int Month { get; }

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

        /// <summary>
        /// Validates the current instance with the specified scope then converts it to a
        /// <see cref="Yemo"/> value.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="AoorException">The current instance is not valid according to the
        /// specified scope.</exception>
        [Pure]
        public Yemo ToYemo(ICalendarScope scope)
        {
            Requires.NotNull(scope);

            scope.ValidateYearMonth(Year, Month);
            return new Yemo(Year, Month);
        }
    }

    public partial struct MonthParts // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="MonthParts"/> are equal.
        /// </summary>
        public static bool operator ==(MonthParts left, MonthParts right) =>
            left.Year == right.Year && left.Month == right.Month;

        /// <summary>
        /// Determines whether two specified instances of <see cref="MonthParts"/> are not equal.
        /// </summary>
        public static bool operator !=(MonthParts left, MonthParts right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(MonthParts other) => this == other;

        /// <inheritdoc />
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is MonthParts fields && this == fields;

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Year, Month);
    }
}
