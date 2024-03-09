// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Horology.Astronomy;

using System.Numerics;

// TODO: calculer plus précisément les Min et Max ?

public readonly partial struct BidayNumber :
    IComparisonOperators<BidayNumber, BidayNumber>,
    IMinMaxValue<BidayNumber>
{
    public const int MinNychthemeronsAtMidnight = Int32.MinValue >> 1;
    public const int MaxNychthemeronsAtMidnight = Int32.MaxValue >> 1;

    private readonly int _halfDaysSinceZero;

    public BidayNumber(int nychthemeronsAtMidnight, bool startAtMidnight)
    {
        if (nychthemeronsAtMidnight < MinNychthemeronsAtMidnight
            || nychthemeronsAtMidnight > MaxNychthemeronsAtMidnight)
        {
            Throw.ArgumentOutOfRange(nameof(nychthemeronsAtMidnight));
        }

        _halfDaysSinceZero = checked((nychthemeronsAtMidnight << 1) + (startAtMidnight ? 0 : 1));
    }

    public static BidayNumber Zero { get; }
    public static BidayNumber MinValue { get; } = new BidayNumber(MinNychthemeronsAtMidnight, true);
    public static BidayNumber MaxValue { get; } = new BidayNumber(MaxNychthemeronsAtMidnight, false);

    public int NychthemeronsSinceZero =>
        // Ça marche car la division entière arrondit "towards zero".
        _halfDaysSinceZero / 2;

    internal int NychthemeronsAtMidnight =>
        StartAtMidnight ? _halfDaysSinceZero >> 1 : (_halfDaysSinceZero - 1) >> 1;

    public bool StartAtMidnight => (_halfDaysSinceZero & 1) == 0;

    public decimal DaysSinceZero => ((decimal)_halfDaysSinceZero) / 2;

    public decimal FractionOfDay => StartAtMidnight ? 0m : .5m;

    ///// <summary>
    ///// Gets the day of the week from this instance.
    ///// </summary>
    //public DayOfWeek DayOfWeek
    //    => DayOfWeekEx.GetDayOfWeek(DayOfWeek.Monday, NychthemeronsAtMidnight);

    public override string ToString() =>
        StartAtMidnight ? FormattableString.Invariant($"{NychthemeronsAtMidnight} (0h)")
        : FormattableString.Invariant($"{NychthemeronsAtMidnight} (12h)");

    public void Deconstruct(
        out int nychthemeronsAtMidnight, out bool startAtMidnight)
    {
        (nychthemeronsAtMidnight, startAtMidnight)
            = (NychthemeronsAtMidnight, StartAtMidnight);
    }

    [Pure]
    public DayNumber ToDayNumber() => new(checked(1 + NychthemeronsAtMidnight));
}

public partial struct BidayNumber
{
    public static bool operator ==(BidayNumber left, BidayNumber right) =>
        left._halfDaysSinceZero == right._halfDaysSinceZero;

    public static bool operator !=(BidayNumber left, BidayNumber right) =>
        left._halfDaysSinceZero != right._halfDaysSinceZero;

    /// <inheritdoc />
    public bool Equals(BidayNumber other) => _halfDaysSinceZero == other._halfDaysSinceZero;

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is BidayNumber epoch && Equals(epoch);

    /// <inheritdoc />
    public override int GetHashCode() => _halfDaysSinceZero;
}

public partial struct BidayNumber
{
    public static bool operator <(BidayNumber left, BidayNumber right) =>
        left._halfDaysSinceZero < right._halfDaysSinceZero;

    public static bool operator <=(BidayNumber left, BidayNumber right) =>
        left._halfDaysSinceZero <= right._halfDaysSinceZero;

    public static bool operator >(BidayNumber left, BidayNumber right) =>
        left._halfDaysSinceZero > right._halfDaysSinceZero;

    public static bool operator >=(BidayNumber left, BidayNumber right) =>
        left._halfDaysSinceZero >= right._halfDaysSinceZero;

    [Pure]
    public int CompareTo(BidayNumber other) =>
        _halfDaysSinceZero.CompareTo(other._halfDaysSinceZero);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is BidayNumber epoch ? CompareTo(epoch)
        : Throw.NonComparable(typeof(BidayNumber), obj);
}

public partial struct BidayNumber
{
    [Pure]
    public static int operator -(BidayNumber left, BidayNumber right) =>
        checked(left._halfDaysSinceZero - right._halfDaysSinceZero);

    [Pure]
    public static decimal CountDaysBetween(BidayNumber start, BidayNumber end)
    {
        // Normalement, on écrirait
        // > decimal days = (decimal)(end - start) / 2;
        // mais on souhaite éviter la division "décimale".
        int halfDays = end - start;
        int days = halfDays / 2; // rounds towards zero.

        return (halfDays & 1) == 0 ? days
            // Nombre impair de demi-journées.
            : halfDays > 0 ? MathOperations.AddHalfOne(days) // days + .5m
            : MathOperations.SubtractHalfOne(days); // days - .5m;
    }

    [Pure]
    public static int CountNychthemeronsBetween(BidayNumber start, BidayNumber end) =>
        // rounds towards zero.
        (end - start) / 2;

    [Pure]
    public static int Subtract(BidayNumber left, BidayNumber right) => left - right;

    [Pure]
    public int Minus(BidayNumber other) => this - other;
}
