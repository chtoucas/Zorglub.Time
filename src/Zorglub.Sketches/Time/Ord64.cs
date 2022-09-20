// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    using System.ComponentModel;
    using System.Globalization;

    using Zorglub.Time.Core;

    /// <summary>
    /// Represents a 64-bit "signed" ordinal numeral.
    /// <para><see cref="Ord64"/> is an immutable struct.</para>
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly partial struct Ord64 :
        IMinMaxValue<Ord64>,
        // Arithmetic
        IAdditionOperators<Ord64, long, Ord64>,
        ISubtractionOperators<Ord64, long, Ord64>,
        IDifferenceOperators<Ord64, long>,
        IIncrementOperators<Ord64>,
        IDecrementOperators<Ord64>,
        IUnaryNegationOperators<Ord64, Ord64>,
        // Comparison
        IComparisonOperators<Ord64, Ord64>
    {
        /// <summary>
        /// Represents the smallest possible algebraic value.
        /// <para>This field is a constant equal to -9_223_372_036_854_775_806.</para>
        /// </summary>
        public const long MinAlgebraicValue = Int64.MinValue + 2;

        /// <summary>
        /// Represents the largest possible algebraic value.
        /// <para>This field is a constant equal to 9_223_372_036_854_775_807.</para>
        /// </summary>
        public const long MaxAlgebraicValue = Int64.MaxValue;

        /// <summary>
        /// Represents the algebraic value of the current instance.
        /// <para>This field is in the range from <see cref="MinAlgebraicValue"/> to
        /// <see cref="MaxAlgebraicValue"/>.</para>
        /// </summary>
        private readonly long _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ord64"/> struct from the specified
        /// algebraic value.
        /// <para>This constructor does NOT validate its parameter.</para>
        /// </summary>
        private Ord64(long value)
        {
            Debug.Assert(value >= MinAlgebraicValue);

            _value = value;
        }

        /// <summary>
        /// Gets the ordinal numeral zeroth.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static Ord64 Zeroth { get; }

        /// <summary>
        /// Gets the ordinal numeral first.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static Ord64 First { get; } = new(1);

        /// <summary>
        /// Gets the smallest possible value of an <see cref="Ord64"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static Ord64 MinValue { get; } = new(MinAlgebraicValue);

        /// <summary>
        /// Gets the largest possible value of an <see cref="Ord64"/>.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static Ord64 MaxValue { get; } = new(MaxAlgebraicValue);

        /// <summary>
        /// Gets the (signed) rank of the current instance.
        /// <para>The result is never equal to zero and is in the range from
        /// -<see cref="Int64.MaxValue"/> to <see cref="Int64.MaxValue"/>.</para>
        /// </summary>
        public long Rank => _value > 0 ? _value : _value - 1;

        /// <summary>
        /// Gets the string to display in the debugger watch window for this value.
        /// </summary>
        [ExcludeFromCodeCoverage(Justification = "DebuggerDisplay")]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string DebuggerDisplay => _value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Converts the current instance to its equivalent string representation using the
        /// formatting conventions of the current culture.
        /// </summary>
        public override string ToString() => Rank.ToString(CultureInfo.CurrentCulture);

        /// <summary>
        /// Deconstructs the current instance into its components: its position, an unsigned rank,
        /// and a boolean.
        /// </summary>
        public void Deconstruct(out long pos, out bool afterZeroth)
        {
            if (_value > 0)
            {
                pos = _value;
                afterZeroth = true;
            }
            else
            {
                pos = 1 - _value;
                afterZeroth = false;
            }
        }

        /// <summary>
        /// Converts the current instance to a 64-bit signed integer.
        /// <para>The result is in the range from <see cref="MinAlgebraicValue"/> to
        /// <see cref="MaxAlgebraicValue"/>.</para>
        /// </summary>
        public static explicit operator long(Ord64 ord) => ord._value;
    }

    public partial struct Ord64 // Factories, conversions.
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Ord64"/> struct from the specified signed rank.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="rank"/> is equal to zero or
        /// <see cref="Int64.MinValue"/>.</exception>
        public static Ord64 FromRank(long rank) =>
            rank == 0 || rank == Int64.MinValue ? Throw.ArgumentOutOfRange<Ord64>(nameof(rank))
            // The next operation never overflows. It is equivalent to:
            //   rank > 0 ? Zeroth + rank : First + rank;
            : new Ord64(rank > 0 ? rank : 1 + rank);

        /// <summary>
        /// Creates a new instance of the <see cref="Ord64"/> struct from the specified algebraic
        /// value.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="value"/> is lower than
        /// <see cref="MinAlgebraicValue"/>.</exception>
        public static Ord64 FromInt64(long value) =>
            value < MinAlgebraicValue ? Throw.ArgumentOutOfRange<Ord64>(nameof(value))
            : new Ord64(value);

        /// <summary>
        /// Converts the current instance to its equivalent algebraic value, a 64-bit signed integer.
        /// <para>The result is in the range from <see cref="MinAlgebraicValue"/> to
        /// <see cref="MaxAlgebraicValue"/>.</para>
        /// </summary>
        public long ToInt64() => _value;
    }

    public partial struct Ord64 // IEquatable.
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="Ord64"/> are equal.
        /// </summary>
        public static bool operator ==(Ord64 left, Ord64 right) => left._value == right._value;

        /// <summary>
        /// Determines whether two specified instances of <see cref="Ord64"/> are not equal.
        /// </summary>
        public static bool operator !=(Ord64 left, Ord64 right) => left._value != right._value;

        /// <inheritdoc />
        public bool Equals(Ord64 other) => _value == other._value;

        /// <inheritdoc />
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Ord64 ord && Equals(ord);

        /// <inheritdoc />
        public override int GetHashCode() => _value.GetHashCode();
    }

    public partial struct Ord64 // IComparable.
    {
        /// <summary>
        /// Compares the two specified ordinal numerals to see if the left one is less than the
        /// right one.
        /// </summary>
        public static bool operator <(Ord64 left, Ord64 right) => left._value < right._value;

        /// <summary>
        /// Compares the two specified ordinal numerals to see if the left one is less than or equal
        /// to the right one.
        /// </summary>
        public static bool operator <=(Ord64 left, Ord64 right) => left._value <= right._value;

        /// <summary>
        /// Compares the two specified ordinal numerals to see if the left one is greater than the
        /// right one.
        /// </summary>
        public static bool operator >(Ord64 left, Ord64 right) => left._value > right._value;

        /// <summary>
        /// Compares the two specified ordinal numerals to see if the left one is greater than or
        /// equal to the right one.
        /// </summary>
        public static bool operator >=(Ord64 left, Ord64 right) => left._value >= right._value;

        /// <summary>
        /// Obtains the smaller of two specified ordinal numerals.
        /// </summary>
        [Pure]
        public static Ord64 Min(Ord64 left, Ord64 right) => left < right ? left : right;

        /// <summary>
        /// Obtains the larger of two specified ordinal numerals.
        /// </summary>
        [Pure]
        public static Ord64 Max(Ord64 left, Ord64 right) => left > right ? left : right;

        /// <summary>
        /// Compares the current instance to a specified ordinal numeral and returns a comparison of
        /// their relative values.
        /// </summary>
        public int CompareTo(Ord64 other) => _value.CompareTo(other._value);

        [Pure]
        int IComparable.CompareTo(object? obj) =>
            obj is null ? 1
            : obj is Ord64 ord ? CompareTo(ord)
            : Throw.NonComparable(typeof(Ord64), obj);
    }

    public partial struct Ord64 // Math ops.
    {
        /// <summary>
        /// Subtracts the two specified ordinal numerals.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int64"/>.</exception>
        public static long operator -(Ord64 left, Ord64 right) => checked(left._value - right._value);

        /// <summary>
        /// Adds an integer to a specified ordinal numeral, yielding a new ordinal
        /// numeral.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int64"/>.</exception>
        public static Ord64 operator +(Ord64 ord, long num)
        {
            long newVal = checked(ord._value + num);

            return newVal < MinAlgebraicValue ? Throw.OrdOverflow<Ord64>() : new Ord64(newVal);
        }

        /// <summary>
        /// Subtracts an integer to a specified ordinal numeral, yielding a new
        /// ordinal numeral.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int64"/>.</exception>
        public static Ord64 operator -(Ord64 ord, long num)
        {
            long newVal = checked(ord._value - num);

            return newVal < MinAlgebraicValue ? Throw.OrdOverflow<Ord64>() : new Ord64(newVal);
        }

        /// <summary>
        /// Increments by 1 the value of the specified ordinal numeral.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int64"/>.</exception>
        public static Ord64 operator ++(Ord64 ord) => ord.Increment();

        /// <summary>
        /// Decrements by 1 the value of the specified ordinal numeral.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int64"/>.</exception>
        public static Ord64 operator --(Ord64 ord) => ord.Decrement();

        /// <summary>
        /// Negates the current instance.
        /// </summary>
        public static Ord64 operator -(Ord64 ord) => ord.Negate();

        /// <summary>
        /// Subtracts the specified ordinal numeral from the current instance.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int64"/>.</exception>
        [Pure]
        public long Subtract(Ord64 other) => this - other;

        /// <summary>
        /// Adds an integer to the value of the current instance, yielding a new
        /// ordinal numeral.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int64"/>.</exception>
        [Pure]
        public Ord64 Add(long num) => this + num;

        /// <summary>
        /// Increments the value of the current instance by 1.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int64"/>.</exception>
        [Pure]
        public Ord64 Increment() =>
            this == MaxValue ? Throw.OrdOverflow<Ord64>() : new Ord64(_value + 1);

        /// <summary>
        /// Decrements the value of the current instance by 1.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int64"/>.</exception>
        [Pure]
        public Ord64 Decrement() =>
            this == MinValue ? Throw.OrdOverflow<Ord64>() : new Ord64(_value - 1);

        /// <summary>
        /// Negates the current instance.
        /// </summary>
        [Pure]
        // No need to use checked arithmetic, the op always succeeds.
        public Ord64 Negate() => new(1 - _value);
    }
}
