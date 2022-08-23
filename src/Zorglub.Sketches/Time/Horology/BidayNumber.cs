// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    // TODO: calculer plus précisément les Min et Max ?

    internal static class MathEx
    {
        private const int HalfOneMin = Int32.MinValue / 10;
        private const int HalfOneMax = Int32.MaxValue / 10;

        // REVIEW: AddHalfOne/SubtractHalfOne en passer par un long quand on
        // sort des limites HalfOneMin/Max.
        // On pourrait aussi traiter le cas général (1 <= b <= 9),
        //public static decimal Add(int num, byte b) =>
        //    num >= 0 ? new decimal(checked(10 * num + b), 0x00000000, 0x00000000, false, 0x0001)
        //    : new decimal(checked(-10 * num - b), 0x00000000, 0x00000000, true, 0x0001);

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal AddHalfOne(int num) =>
            num < HalfOneMin || num > HalfOneMax
            ? num + .5m
            : num >= 0
                ? new decimal(10 * num + 5, 0x00000000, 0x00000000, false, 0x0001)
                : new decimal(-10 * num - 5, 0x00000000, 0x00000000, true, 0x0001);

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal SubtractHalfOne(int num) =>
            num < HalfOneMin || num > HalfOneMax
            ? num - .5m
            : num > 0
                ? new decimal(10 * num - 5, 0x00000000, 0x00000000, false, 0x0001)
                : new decimal(-10 * num + 5, 0x00000000, 0x00000000, true, 0x0001);
    }

    public readonly partial struct BidayNumber :
        IComparisonOperators<BidayNumber, BidayNumber>,
        IMinMaxValue<BidayNumber>
    {
        /// <summary>
        /// Represents the smallest possible value of <see cref="NychthemeronsAtMidnight"/>.
        /// </summary>
        public const int MinNychthemeronsAtMidnight = Int32.MinValue >> 1;

        /// <summary>
        /// Represents the largest possible value of <see cref="NychthemeronsAtMidnight"/>.
        /// </summary>
        public const int MaxNychthemeronsAtMidnight = Int32.MaxValue >> 1;

        /// <summary>
        /// Represents the count of half-days since <see cref="Zero"/>.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _halfDaysSinceZero;

        /// <summary>
        /// Constructs a new instance of <see cref="BidayNumber"/> from the number of consecutive days
        /// since the epoch of the root dayscale, at midnight or noon.
        /// </summary>
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

        /// <summary>
        /// Gets the smallest possible value of a <see cref="BidayNumber"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static BidayNumber MinValue { get; } = new BidayNumber(MinNychthemeronsAtMidnight, true);

        /// <summary>
        /// Gets the largest possible value of a <see cref="BidayNumber"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static BidayNumber MaxValue { get; } = new BidayNumber(MaxNychthemeronsAtMidnight, false);

        /// <summary>
        /// Gets the count of consecutive days aka nychthemerons since <see cref="Zero"/>.
        /// </summary>
        public int NychthemeronsSinceZero =>
            // Ça marche car la division entière arrondit "towards zero".
            _halfDaysSinceZero / 2;

        /// <summary>
        /// Gets the count of consecutive days aka nychthemerons from this instance at midnight (Oh)
        /// to <see cref="Zero"/>.
        /// </summary>
        internal int NychthemeronsAtMidnight =>
            StartAtMidnight ? _halfDaysSinceZero >> 1 : (_halfDaysSinceZero - 1) >> 1;

        /// <summary>
        /// Returns true if this instance starts at midnight; otherwise returns false.
        /// </summary>
        public bool StartAtMidnight => (_halfDaysSinceZero & 1) == 0;

        /// <summary>
        /// Gets the number of fractional days since <see cref="Zero"/>.
        /// </summary>
        public decimal DaysSinceZero => ((decimal)_halfDaysSinceZero) / 2;

        /// <summary>
        /// Gets the fractional part of <see cref="DaysSinceZero"/> from this instance.
        /// </summary>
        public decimal FractionOfDay => StartAtMidnight ? 0m : .5m;

        ///// <summary>
        ///// Gets the day of the week from this instance.
        ///// </summary>
        //public DayOfWeek DayOfWeek
        //    => DayOfWeekEx.GetDayOfWeek(DayOfWeek.Monday, NychthemeronsAtMidnight);

        /// <summary>
        /// Returns a culture-independent string representation of this instance.
        /// </summary>
        public override string ToString() =>
            StartAtMidnight ? FormattableString.Invariant($"{NychthemeronsAtMidnight} (0h)")
            : FormattableString.Invariant($"{NychthemeronsAtMidnight} (12h)");

        /// <summary>
        /// Deconstructs this instance into its components.
        /// </summary>
        public void Deconstruct(
            out int nychthemeronsAtMidnight, out bool startAtMidnight)
        {
            (nychthemeronsAtMidnight, startAtMidnight)
                = (NychthemeronsAtMidnight, StartAtMidnight);
        }

        /// <summary>
        /// Obtains the day number in the root dayscale from this instance.
        /// </summary>
        [Pure]
        public DayNumber ToDayNumber() => new(checked(1 + NychthemeronsAtMidnight));
    }

    public partial struct BidayNumber
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="BidayNumber"/> are equal.
        /// </summary>
        public static bool operator ==(BidayNumber left, BidayNumber right) =>
            left._halfDaysSinceZero == right._halfDaysSinceZero;

        /// <summary>
        /// Determines whether two specified instances of <see cref="BidayNumber"/> are not equal.
        /// </summary>
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
        /// <summary>
        /// Compares the two specified epochs to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        public static bool operator <(BidayNumber left, BidayNumber right) =>
            left._halfDaysSinceZero < right._halfDaysSinceZero;

        /// <summary>
        /// Compares the two specified epochs to see if the left one is earlier than or equal to the
        /// right one.
        /// </summary>
        public static bool operator <=(BidayNumber left, BidayNumber right) =>
            left._halfDaysSinceZero <= right._halfDaysSinceZero;

        /// <summary>
        /// Compares the two specified epochs to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        public static bool operator >(BidayNumber left, BidayNumber right) =>
            left._halfDaysSinceZero > right._halfDaysSinceZero;

        /// <summary>
        /// Compares the two specified epochs to see if the left one is later than or equal to the
        /// right one.
        /// </summary>
        public static bool operator >=(BidayNumber left, BidayNumber right) =>
            left._halfDaysSinceZero >= right._halfDaysSinceZero;

        /// <summary>
        /// Indicates whether this epoch instance is earlier, later or the same as the specified one.
        /// </summary>
        public int CompareTo(BidayNumber other) =>
            _halfDaysSinceZero.CompareTo(other._halfDaysSinceZero);

        /// <inheritdoc />
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is BidayNumber epoch ? CompareTo(epoch)
            : Throw.NonComparable(typeof(BidayNumber), obj);
    }

    public partial struct BidayNumber
    {
        /// <summary>
        /// Subtracts the two specified epochs and returns the number of <i>half-days</i> between
        /// them.
        /// </summary>
        [Pure]
        public static int operator -(BidayNumber left, BidayNumber right) =>
            checked(left._halfDaysSinceZero - right._halfDaysSinceZero);

        /// <summary>
        /// Counts the number of fractional days between the two specified epochs.
        /// </summary>
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
                : halfDays > 0 ? MathEx.AddHalfOne(days) // days + .5m
                : MathEx.SubtractHalfOne(days); // days - .5m;
        }

        /// <summary>
        /// Counts the number of consecutive days between the two specified epochs.
        /// </summary>
        [Pure]
        public static int CountNychthemeronsBetween(BidayNumber start, BidayNumber end) =>
            // rounds towards zero.
            (end - start) / 2;

        /// <summary>
        /// Subtracts the two specified epochs and returns the number of <i>half-days</i> between
        /// them.
        /// </summary>
        [Pure]
        public static int Subtract(BidayNumber left, BidayNumber right) => left - right;

        /// <summary>
        /// Subtracts the two specified epochs and returns the number of <i>half-days</i> between
        /// them.
        /// </summary>
        [Pure]
        public int Minus(BidayNumber other) => this - other;
    }
}
