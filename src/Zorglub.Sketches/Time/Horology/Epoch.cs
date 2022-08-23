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

    /// <summary>
    /// Represents the epoch of a dayscale.
    /// <para>An epoch is a moment at midnight or noon in the root dayscale.</para>
    /// <para><see cref="Epoch"/> is an immutable struct.</para>
    /// </summary>
    public readonly partial struct Epoch :
        IComparisonOperators<Epoch, Epoch>,
        IMinMaxValue<Epoch>
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
        /// Represents the number of half-days from this instance to the epoch of the root dayscale.
        /// </summary>
        private readonly int _halfDays;

        /// <summary>
        /// Constructs a new instance of <see cref="Epoch"/> from the number of
        /// integral days since the epoch of the root dayscale, at midnight or
        /// noon.
        /// </summary>
        public Epoch(int nychthemeronsAtMidnight, bool startAtMidnight)
        {
            if (nychthemeronsAtMidnight < MinNychthemeronsAtMidnight
                || nychthemeronsAtMidnight > MaxNychthemeronsAtMidnight)
            {
                throw new ArgumentOutOfRangeException(nameof(nychthemeronsAtMidnight));
            }

            _halfDays = checked((nychthemeronsAtMidnight << 1) + (startAtMidnight ? 0 : 1));
        }

        /// <summary>
        /// Gets the smallest possible value of an <see cref="Epoch"/>.
        /// </summary>
        public static Epoch MinValue { get; } = new Epoch(MinNychthemeronsAtMidnight, true);

        /// <summary>
        /// Gets the largest possible value of an <see cref="Epoch"/>.
        /// </summary>
        public static Epoch MaxValue { get; } = new Epoch(MaxNychthemeronsAtMidnight, false);

        /// <summary>
        /// Gets the number of integral days aka nychthemerons from this instance to the epoch of
        /// the root dayscale.
        /// </summary>
        public int Nychthemerons =>
            // Ça marche car la division entière arrondit "towards zero".
            _halfDays / 2;

        /// <summary>
        /// Gets the number of integral days aka nychthemerons from this instance at midnight (Oh)
        /// to the epoch of the root dayscale.
        /// </summary>
        internal int NychthemeronsAtMidnight =>
            (_halfDays & 1) == 0 ? _halfDays >> 1 : (_halfDays - 1) >> 1;

        /// <summary>
        /// Returns true if this instance starts at midnight; otherwise returns false.
        /// </summary>
        public bool StartAtMidnight => (_halfDays & 1) == 0;

        /// <summary>
        /// Gets the number of fractional days from this instance to the epoch of the root dayscale.
        /// </summary>
        public decimal Days => ((decimal)_halfDays) / 2;

        /// <summary>
        /// Gets the fractional part of <see cref="Days"/> from this instance.
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

    public partial struct Epoch
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="Epoch"/> are equal.
        /// </summary>
        public static bool operator ==(Epoch left, Epoch right) => left._halfDays == right._halfDays;

        /// <summary>
        /// Determines whether two specified instances of <see cref="Epoch"/> are not equal.
        /// </summary>
        public static bool operator !=(Epoch left, Epoch right) => left._halfDays != right._halfDays;

        /// <summary>
        /// Determines whether this instance is equal to the value of the specified
        /// <see cref="Epoch"/>.
        /// </summary>
        public bool Equals(Epoch other) => _halfDays == other._halfDays;

        /// <summary>
        /// Determines whether this instance is equal to a specified object.
        /// </summary>
        public override bool Equals(object? obj) => obj is Epoch epoch && this == epoch;

        /// <summary>
        /// Obtains the hash code for this instance.
        /// </summary>
        public override int GetHashCode() => _halfDays.GetHashCode();
    }

    public partial struct Epoch
    {
        /// <summary>
        /// Compares the two specified epochs to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        public static bool operator <(Epoch left, Epoch right) => left._halfDays < right._halfDays;

        /// <summary>
        /// Compares the two specified epochs to see if the left one is earlier than or equal to the
        /// right one.
        /// </summary>
        public static bool operator <=(Epoch left, Epoch right) => left._halfDays <= right._halfDays;

        /// <summary>
        /// Compares the two specified epochs to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        public static bool operator >(Epoch left, Epoch right) => left._halfDays > right._halfDays;

        /// <summary>
        /// Compares the two specified epochs to see if the left one is later than or equal to the
        /// right one.
        /// </summary>
        public static bool operator >=(Epoch left, Epoch right) => left._halfDays >= right._halfDays;

        /// <summary>
        /// Indicates whether this epoch instance is earlier, later or the same as the specified one.
        /// </summary>
        public int CompareTo(Epoch other) => _halfDays.CompareTo(other._halfDays);

        int IComparable.CompareTo(object? obj) =>
            obj is null ? 1
            : obj is Epoch epoch ? CompareTo(epoch)
            : Throw.NonComparable(typeof(Epoch), obj);
    }

    public partial struct Epoch
    {
        /// <summary>
        /// Counts the number of fractional days between the two specified epochs.
        /// </summary>
        [Pure]
        public static decimal CountDaysBetween(Epoch start, Epoch end)
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
        /// Counts the number of integral days between the two specified epochs.
        /// </summary>
        [Pure]
        public static int CountNychthemeronsBetween(Epoch start, Epoch end) =>
            // rounds towards zero.
            (end - start) / 2;

        /// <summary>
        /// Subtracts the two specified epochs and returns the number of HALF-DAYS between them.
        /// </summary>
        [Pure]
        public static int operator -(Epoch left, Epoch right) =>
            checked(left._halfDays - right._halfDays);

        /// <summary>
        /// Subtracts the two specified epochs and returns the number of HALF-DAYS between them.
        /// </summary>
        [Pure]
        public static int Subtract(Epoch left, Epoch right) => left - right;

        /// <summary>
        /// Subtracts the two specified epochs and returns the number of HALF-DAYS between them.
        /// </summary>
        [Pure]
        public int Minus(Epoch other) => this - other;
    }
}
