// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using static TemporalConstants;

    // REVIEW(code): do we need to use checked ops?

    // I observe very tiny performance gains only with:
    // - DivideByNanosecondsPerHour
    // - MultiplyByNanosecondsPerHour
    // - DivideByNanosecondsPerMinute
    // For the other methods, whether we use the plain ops or the "optimized"
    // ones, it does no seem to make any difference.
    //
    // NanosecondsPerHour:          3_600_000_000_000 = 2^13 x 5^11 x 3^2
    // TicksPerDay:                   864_000_000_000 = 2^14 x 5^9  x 3^3
    // NanosecondsPerMinute:           60_000_000_000 = 2^11 x 5^10 x 3
    // NanosecondsPerSecond:            1_000_000_000 = 2^9  x 5^9
    // NanosecondsPerMillisecond:           1_000_000 = 2^6  x 5^6
    //
    // For the multiplication, we have the choice of shift then multiply or
    // multiply then shift, but perf is all the same. Nevertheless, I opt for
    // mumtiply then shift to avoid a cast.
    //
    // We don't bother with NanosecondsPerDay which is only multiplied or
    // divided with a double or decimal.

    /// <summary>
    /// Provides fast arithmetical operations related to
    /// <see cref="TemporalConstants"/>.
    /// </summary>
    internal static partial class TemporalArithmetic { }

    internal partial class TemporalArithmetic // TicksPerDay
    {
        /// <summary>
        /// Represents the 2-adic valuation of <see cref="TicksPerDay"/>.
        /// <para>This field is a constant equal to 14.</para>
        /// </summary>
        private const int TicksPerDayTwoAdicOrder = 14;

        /// <summary>
        /// Represents the product of odd prime factors of <see cref="TicksPerDay"/>.
        /// <para>This field is a constant equal to 52_734_375.</para>
        /// </summary>
        private const long TicksPerDayOddPart = TicksPerDay >> TicksPerDayTwoAdicOrder;

        /// <summary><c>daysSinceZero = ticksSinceZero / TicksPerDay</c></summary>
        /// <remarks><para><paramref name="ticksSinceZero"/> MUST be &gt;= 0.</para></remarks>
        [Pure]
        // CIL code size = 12 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long DivideByTicksPerDay(long ticksSinceZero)
        {
            Debug.Assert(ticksSinceZero >= 0);

            return (long)((ulong)(ticksSinceZero >> TicksPerDayTwoAdicOrder) / TicksPerDayOddPart);
        }

        /// <summary><c>ticksSinceZero = TicksPerDay * daysSinceZero</c></summary>
        [Pure]
        // CIL code size = 12 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long MultiplyByTicksPerDay(long daysSinceZero) =>
            //(daysSinceZero << TicksPerDayTwoAdicOrder) * TicksPerDayOddPart;
            (daysSinceZero * TicksPerDayOddPart) << TicksPerDayTwoAdicOrder;
    }

    internal partial class TemporalArithmetic // NanosecondsPerHour
    {
        /// <summary>
        /// Represents the 2-adic valuation of <see cref="NanosecondsPerHour"/>.
        /// <para>This field is a constant equal to 13.</para>
        /// </summary>
        private const int NanosecondsPerHourTwoAdicOrder = 13;

        /// <summary>
        /// Represents the product of odd prime factors of <see cref="NanosecondsPerHour"/>.
        /// <para>This field is a constant equal to 439_453_125.</para>
        /// </summary>
        private const long NanosecondsPerHourOddPart =
            NanosecondsPerHour >> NanosecondsPerHourTwoAdicOrder;

        /// <summary><c>hourOfDay = nanosecondOfDay / NanosecondsPerHour</c></summary>
        /// <remarks><para><paramref name="nanosecondOfDay"/> MUST be &gt;= 0.</para></remarks>
        [Pure]
        // CIL code size = 13 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DivideByNanosecondsPerHour(long nanosecondOfDay)
        {
            Debug.Assert(nanosecondOfDay >= 0);
            // If the following condition is not met, the cast to int may fail.
            Debug.Assert(nanosecondOfDay < NanosecondsPerDay);

            return (int)((ulong)(nanosecondOfDay >> NanosecondsPerHourTwoAdicOrder) / NanosecondsPerHourOddPart);
        }

        /// <summary><c>nanosecondOfDay = NanosecondsPerHour * hourOfDay</c></summary>
        [Pure]
        // CIL code size = 13 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long MultiplyByNanosecondsPerHour(int hourOfDay)
        {
            Debug.Assert(hourOfDay >= 0);
            Debug.Assert(hourOfDay < HoursPerDay);

            //return ((long)hourOfDay << NanosecondsPerHourTwoAdicOrder) * NanosecondsPerHourOddPart;
            return (hourOfDay * NanosecondsPerHourOddPart) << NanosecondsPerHourTwoAdicOrder;
        }
    }

    internal partial class TemporalArithmetic // NanosecondsPerMinute
    {
        /// <summary>
        /// Represents the 2-adic valuation of <see cref="NanosecondsPerMinute"/>.
        /// <para>This field is a constant equal to 11.</para>
        /// </summary>
        private const int NanosecondsPerMinuteTwoAdicOrder = 11;

        /// <summary>
        /// Represents the product of odd prime factors of <see cref="NanosecondsPerMinute"/>.
        /// <para>This field is a constant equal to 29_296_875.</para>
        /// </summary>
        private const long NanosecondsPerMinuteOddPart =
            NanosecondsPerMinute >> NanosecondsPerMinuteTwoAdicOrder;

        /// <summary><c>minuteOfDay = nanosecondOfDay / NanosecondsPerMinute</c></summary>
        /// <remarks><para><paramref name="nanosecondOfDay"/> MUST be &gt;= 0.</para></remarks>
        [Pure]
        // CIL code size = 13 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DivideByNanosecondsPerMinute(long nanosecondOfDay)
        {
            Debug.Assert(nanosecondOfDay >= 0);
            // If the following condition is not met, the cast to int may fail.
            Debug.Assert(nanosecondOfDay < NanosecondsPerDay);

            return (int)((ulong)(nanosecondOfDay >> NanosecondsPerMinuteTwoAdicOrder) / NanosecondsPerMinuteOddPart);
        }

        /// <summary><c>nanosecondOfDay = NanosecondsPerMinute * minuteOfDay</c></summary>
        [Pure]
        // CIL code size = 13 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long MultiplyByNanosecondsPerMinute(int minuteOfDay)
        {
            Debug.Assert(minuteOfDay >= 0);
            Debug.Assert(minuteOfDay < MinutesPerDay);

            //return ((long)minuteOfDay << NanosecondsPerMinuteTwoAdicOrder) * NanosecondsPerMinuteOddPart;
            return (minuteOfDay * NanosecondsPerMinuteOddPart) << NanosecondsPerMinuteTwoAdicOrder;
        }
    }

    internal partial class TemporalArithmetic // NanosecondsPerSecond (disabled)
    {
#if false // NanosecondsPerSecond
        /// <summary>
        /// Represents the 2-adic valuation of <see cref="NanosecondsPerSecond"/>.
        /// <para>This field is a constant equal to 9.</para>
        /// </summary>
        private const int NanosecondsPerSecondTwoAdicOrder = 9;

        /// <summary>
        /// Represents the product of odd prime factors of <see cref="NanosecondsPerSecond"/>.
        /// <para>This field is a constant equal to 1_953_125.</para>
        /// </summary>
        private const long NanosecondsPerSecondOddPart =
            NanosecondsPerSecond >> NanosecondsPerSecondTwoAdicOrder;

        /// <summary><c>secondOfDay = nanosecondOfDay / NanosecondsPerSecond</c></summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DivideByNanosecondsPerSecond(long nanosecondOfDay)
        {
            Debug.Assert(nanosecondOfDay >= 0);
            Debug.Assert(nanosecondOfDay < NanosecondsPerDay);

            return (nanosecondOfDay >> NanosecondsPerSecondTwoAdicOrder) / NanosecondsPerSecondOddPart;
        }

        /// <summary><c>nanosecondOfDay = NanosecondsPerSecond * secondOfDay</c></summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long MultiplyByNanosecondsPerSecond(int secondOfDay)
        {
            Debug.Assert(secondOfDay < SecondsPerDay);

            return (secondOfDay << NanosecondsPerSecondTwoAdicOrder) * NanosecondsPerSecondOddPart;
        }
#endif
    }

    internal partial class TemporalArithmetic // NanosecondsPerMillisecond (disabled)
    {
#if false // NanosecondsPerMillisecond
        /// <summary>
        /// Represents the 2-adic valuation of <see cref="NanosecondsPerMillisecond"/>.
        /// <para>This field is a constant equal to 6.</para>
        /// </summary>
        private const int NanosecondsPerMillisecondTwoAdicOrder = 6;

        /// <summary>
        /// Represents the product of odd prime factors of <see cref="NanosecondsPerMillisecond"/>.
        /// <para>This field is a constant equal to 15_625.</para>
        /// </summary>
        private const long NanosecondsPerMillisecondOddPart =
            NanosecondsPerMillisecond >> NanosecondsPerMillisecondTwoAdicOrder;

        /// <summary><c>millisecondOfDay = nanosecondOfDay / NanosecondsPerMillisecond</c></summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DivideByNanosecondsPerMillisecond(long nanosecondOfDay)
        {
            Debug.Assert(nanosecondOfDay >= 0);
            Debug.Assert(nanosecondOfDay < NanosecondsPerDay);

            return (nanosecondOfDay >> NanosecondsPerMillisecondTwoAdicOrder) / NanosecondsPerMillisecondOddPart;
        }

        /// <summary><c>nanosecondOfDay = NanosecondsPerMillisecond * millisecondOfDay</c></summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long MultiplyByNanosecondsPerMillisecond(int millisecondOfDay)
        {
            Debug.Assert(millisecondOfDay < MillisecondsPerDay);

            return (millisecondOfDay << NanosecondsPerMillisecondTwoAdicOrder) * NanosecondsPerMillisecondOddPart;
        }
#endif
    }
}
