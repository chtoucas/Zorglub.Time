// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time;

/// <summary>Represents a pair of a year and a month.
/// <para>This type uses the lexicographic order on pairs (Year, Month).</para>
/// <para><see cref="MonthParts"/> does NOT represent a month, its default value is not even a
/// valid month.</para>
/// <para><see cref="MonthParts"/> is an immutable struct.</para></summary>
/// <param name="Year">Algebraic year number.</param>
/// <param name="Month">Month of the year.</param>
public readonly record struct MonthParts(int Year, int Month) :
    IComparisonOperators<MonthParts, MonthParts>
{
    /// <summary>Creates a new instance of the <see cref="MonthParts"/> struct representing the
    /// first month of the specified year.</summary>
    [Pure]
    public static MonthParts AtStartOfYear(int y) => new(y, 1);

    /// <inheritdoc />
    public static bool operator <(MonthParts left, MonthParts right) => left.CompareTo(right) < 0;

    /// <inheritdoc />
    public static bool operator <=(MonthParts left, MonthParts right) => left.CompareTo(right) <= 0;

    /// <inheritdoc />
    public static bool operator >(MonthParts left, MonthParts right) => left.CompareTo(right) > 0;

    /// <inheritdoc />
    public static bool operator >=(MonthParts left, MonthParts right) => left.CompareTo(right) >= 0;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(MonthParts other)
    {
        int c = Year.CompareTo(other.Year);
        return c == 0 ? Month.CompareTo(other.Month) : c;
    }

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is MonthParts parts ? CompareTo(parts)
        : Throw.NonComparable(typeof(MonthParts), obj);
}
