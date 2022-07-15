// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    using System.Globalization;

    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology;

    /// <summary>
    /// Represents a 64-bit day number which counts the number of consecutive days since the Monday
    /// 1st of January, 1 CE within the Gregorian calendar.
    /// <para><see cref="DayNumber64"/> is an immutable struct.</para>
    /// </summary>
    public readonly partial struct DayNumber64 :
        IFixedDay<DayNumber64>,
        IMinMaxValue<DayNumber64>
    {
        public const long MinDaysSinceZero = Int64.MinValue + 1;
        public const long MaxDaysSinceZero = Int64.MaxValue - 1;

        public static readonly DayNumber64 Zero;

        #region Gregorian/Julian
        // With our current choice for Min/MaxGregorianYear, we have
        //   -2^43 < DaysSinceZero < 2^43
        // In theory, we can go much further with Int64, more or less 2^52 for
        // a year, but it's meaningless. Indeed, the current estimation for the
        // age of the universe is ~14 billion years.

        public const long MinSupportedYear = -13_999_999_999L;
        public const long MaxSupportedYear = 14_000_000_000;

        #endregion
        #region Gregorian

        private const long MinGregorianDaysSinceZero = -5_113_395_000_000L;
        private const long MaxGregorianDaysSinceZero = 5_113_394_999_999L;

        private static readonly DayNumber64 s_MinGregorianValue = new(MinGregorianDaysSinceZero);
        private static readonly DayNumber64 s_MaxGregorianValue = new(MaxGregorianDaysSinceZero);
        public static Range<DayNumber64> GregorianDomain =>
            Range.Create(s_MinGregorianValue, s_MaxGregorianValue);

        #endregion
        #region Julian

        private const long MinJulianDaysSinceZero = -5_113_500_000_002L;
        private const long MaxJulianDaysSinceZero = 5_113_499_999_997L;

        private static readonly DayNumber64 s_MinJulianValue = new(MinJulianDaysSinceZero);
        private static readonly DayNumber64 s_MaxJulianValue = new(MaxJulianDaysSinceZero);
        public static Range<DayNumber64> JulianDomain =>
            Range.Create(s_MinJulianValue, s_MaxJulianValue);

        #endregion

        private readonly long _daysSinceZero;

        private DayNumber64(long daysSinceZero)
        {
            Debug.Assert(daysSinceZero >= MinDaysSinceZero);
            Debug.Assert(daysSinceZero <= MaxDaysSinceZero);

            _daysSinceZero = daysSinceZero;
        }

        public static DayNumber64 MinValue { get; } = new(MinDaysSinceZero);
        public static DayNumber64 MaxValue { get; } = new(MaxDaysSinceZero);

        public long DaysSinceZero => _daysSinceZero;

        public Ord64 Ordinal => Ord64.First + _daysSinceZero;

        public DayOfWeek DayOfWeek =>
            (DayOfWeek)MathZ.Modulo(
                (long)DayOfWeek.Monday + _daysSinceZero,
                CalendricalConstants.DaysInWeek);

        public IsoDayOfWeek IsoDayOfWeek =>
            (IsoDayOfWeek)MathZ.AdjustedModulo(
                (long)DayOfWeek.Monday + _daysSinceZero,
                CalendricalConstants.DaysInWeek);

        [Pure]
        public override string ToString() => _daysSinceZero.ToString(CultureInfo.CurrentCulture);
    }

    public partial struct DayNumber64
    {
        #region Factories

        [Pure]
        public static DayNumber64 Today()
        {
            long daysSinceZero = TemporalArithmetic.DivideByTicksPerDay(DateTime.Now.Ticks);
            return new DayNumber64(daysSinceZero);
        }

        [Pure]
        public static DayNumber64 UtcToday()
        {
            long daysSinceZero = TemporalArithmetic.DivideByTicksPerDay(DateTime.UtcNow.Ticks);
            return new DayNumber64(daysSinceZero);
        }

        #endregion
        #region Conversions

        public static explicit operator DayNumber(DayNumber64 dayNumber) =>
            DayNumber.MaxDaysSinceZero <= dayNumber._daysSinceZero
                && dayNumber._daysSinceZero <= DayNumber.MaxDaysSinceZero
            ? DayNumber.Zero + (int)dayNumber._daysSinceZero
            : Throw.InvalidOperation<DayNumber>();

        public static implicit operator DayNumber64(DayNumber dayNumber) =>
            new(dayNumber.DaysSinceZero);

        [Pure]
        public static DayNumber64 FromDayNumber(DayNumber dayNumber) => new(dayNumber.DaysSinceZero);

        [Pure]
        public DayNumber ToDayNumber() =>
            DayNumber.MaxDaysSinceZero <= _daysSinceZero
                && _daysSinceZero <= DayNumber.MaxDaysSinceZero
            ? DayNumber.Zero + (int)_daysSinceZero
            : Throw.InvalidOperation<DayNumber>();

        #endregion
        #region Gregorian

        [Pure]
        public static DayNumber64 FromGregorianParts(long year, int month, int day)
        {
            ValidateYear(year);
            GregorianPreValidator.ValidateMonthDay(year, month, day);

            long daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(year, month, day);
            return new DayNumber64(daysSinceZero);
        }

        [Pure]
        public static DayNumber64 FromGregorianOrdinalParts(long year, int dayOfYear)
        {
            ValidateYear(year);
            GregorianPreValidator.ValidateDayOfYear(year, dayOfYear);

            long daysSinceZero = GregorianFormulae.GetStartOfYear(year) + dayOfYear - 1;
            return new DayNumber64(daysSinceZero);
        }

        public void GetGregorianParts(out long year, out int month, out int day)
        {
            CheckGregorianOverflow();

            GregorianFormulae.GetDateParts(_daysSinceZero, out year, out month, out day);
        }

        public void GetGregorianOrdinalParts(out long year, out int dayOfYear)
        {
            CheckGregorianOverflow();

            year = GregorianFormulae.GetYear(_daysSinceZero);
            dayOfYear = (int)(1 + _daysSinceZero - GregorianFormulae.GetStartOfYear(year));
        }

        [Pure]
        public long GetGregorianYear()
        {
            CheckGregorianOverflow();

            return GregorianFormulae.GetYear(_daysSinceZero);
        }

        #endregion
        #region Julian

        // This is DayZero.OldStyle - DayNumber.NewStyle.
        private const int DaysFromJulianEpochToZero = 2;

        [Pure]
        public static DayNumber64 FromJulianParts(long year, int month, int day)
        {
            ValidateYear(year);
            JulianPreValidator.ValidateMonthDay(year, month, day);

            long daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(year, month, day);
            long daysSinceZero = daysSinceEpoch - DaysFromJulianEpochToZero;
            return new DayNumber64(daysSinceZero);
        }

        [Pure]
        public static DayNumber64 FromJulianOrdinalParts(long year, int dayOfYear)
        {
            ValidateYear(year);
            JulianPreValidator.ValidateDayOfYear(year, dayOfYear);

            long daysSinceEpoch = JulianFormulae.GetStartOfYear(year) + dayOfYear - 1;
            long daysSinceZero = daysSinceEpoch - DaysFromJulianEpochToZero;
            return new DayNumber64(daysSinceZero);
        }

        public void GetJulianParts(out long year, out int month, out int day)
        {
            CheckJulianOverflow();

            long daysSinceEpoch = DaysFromJulianEpochToZero + _daysSinceZero;
            JulianFormulae.GetDateParts(daysSinceEpoch, out year, out month, out day);
        }

        public void GetJulianOrdinalParts(out long year, out int dayOfYear)
        {
            CheckJulianOverflow();

            long daysSinceEpoch = DaysFromJulianEpochToZero + _daysSinceZero;
            year = JulianFormulae.GetYear(daysSinceEpoch);
            dayOfYear = (int)(1 + daysSinceEpoch - JulianFormulae.GetStartOfYear(year));
        }

        [Pure]
        public long GetJulianYear()
        {
            CheckJulianOverflow();

            long daysSinceEpoch = DaysFromJulianEpochToZero + _daysSinceZero;
            return JulianFormulae.GetYear(daysSinceEpoch);
        }

        #endregion
        #region Gregorian/Julian helpers

        private static void ValidateYear(long year)
        {
            if (year < MinSupportedYear || year > MaxSupportedYear)
            {
                Throw.YearOutOfRange(year);
            }
        }

        private void CheckGregorianOverflow()
        {
            if (_daysSinceZero < MinGregorianDaysSinceZero
                || _daysSinceZero > MaxGregorianDaysSinceZero)
            {
                Throw.DateOverflow();
            }
        }

        private void CheckJulianOverflow()
        {
            if (_daysSinceZero < MinJulianDaysSinceZero || _daysSinceZero > MaxJulianDaysSinceZero)
            {
                Throw.DateOverflow();
            }
        }

        #endregion
    }

    public partial struct DayNumber64 // Adjust the day of the week
    {
        private static readonly DayNumber64 s_ThreeDaysBeforeMaxValue = MaxValue - 3;

        [Pure]
        public DayNumber64 Previous(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return this + (δ >= 0 ? δ - CalendricalConstants.DaysInWeek : δ);
        }

        [Pure]
        public DayNumber64 PreviousOrSame(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return δ == 0 ? this : this + (δ > 0 ? δ - CalendricalConstants.DaysInWeek : δ);
        }

        [Pure]
        public DayNumber64 Nearest(DayOfWeek dayOfWeek) =>
            // WARNING:
            // - PreviousOrSameCore() fails near MaxValue.
            // - NextOrSameCore() fails near MinValue.
            this > s_ThreeDaysBeforeMaxValue
            ? NextOrSameCore(this, dayOfWeek, -3, 0)
            : PreviousOrSameCore(this, dayOfWeek, 3, 0);

        [Pure]
        public DayNumber64 NextOrSame(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return δ == 0 ? this : this + (δ < 0 ? δ + CalendricalConstants.DaysInWeek : δ);
        }

        [Pure]
        public DayNumber64 Next(DayOfWeek dayOfWeek)
        {
            Requires.Defined(dayOfWeek);

            int δ = dayOfWeek - DayOfWeek;
            return this + (δ <= 0 ? δ + CalendricalConstants.DaysInWeek : δ);
        }

        //
        // Helpers
        //

        [Pure]
        internal static DayNumber64 PreviousOrSameCore(
            DayNumber64 dayNumber,
            DayOfWeek dayOfWeek,
            int dayShift,
            int weeks)
        {
            Debug.Assert(dayShift >= 3);
            Requires.Defined(dayOfWeek);

            long daysSinceZero;
            checked
            {
                daysSinceZero = dayNumber.DaysSinceZero + dayShift;
                // DayNumber.Zero is a Monday.
                daysSinceZero -= MathZ.Modulo(daysSinceZero + (DayOfWeek.Monday - dayOfWeek), CalendricalConstants.DaysInWeek);
                daysSinceZero -= CalendricalConstants.DaysInWeek * weeks;
            }

            return Zero + daysSinceZero;
        }

        [Pure]
        internal static DayNumber64 NextOrSameCore(
            DayNumber64 dayNumber,
            DayOfWeek dayOfWeek,
            int dayShift,
            int weeks)
        {
            Debug.Assert(dayShift <= -3);
            Requires.Defined(dayOfWeek);

            long daysSinceZero;
            checked
            {
                daysSinceZero = dayNumber.DaysSinceZero + dayShift;
                // DayNumber.Zero is a Monday.
                daysSinceZero += MathZ.Modulo(-daysSinceZero - (DayOfWeek.Monday - dayOfWeek), CalendricalConstants.DaysInWeek);
                daysSinceZero += CalendricalConstants.DaysInWeek * weeks;
            }

            return Zero + daysSinceZero;
        }
    }

    public partial struct DayNumber64 // IEquatable
    {
        public static bool operator ==(DayNumber64 left, DayNumber64 right) =>
            left._daysSinceZero == right._daysSinceZero;

        public static bool operator !=(DayNumber64 left, DayNumber64 right) =>
            left._daysSinceZero != right._daysSinceZero;

        [Pure]
        public bool Equals(DayNumber64 other) => _daysSinceZero == other._daysSinceZero;

        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is DayNumber64 date && Equals(date);

        [Pure]
        public override int GetHashCode() => _daysSinceZero.GetHashCode();
    }

    public partial struct DayNumber64 // IComparable
    {
        public static bool operator <(DayNumber64 left, DayNumber64 right) =>
            left._daysSinceZero < right._daysSinceZero;

        public static bool operator <=(DayNumber64 left, DayNumber64 right) =>
            left._daysSinceZero <= right._daysSinceZero;

        public static bool operator >(DayNumber64 left, DayNumber64 right) =>
            left._daysSinceZero > right._daysSinceZero;

        public static bool operator >=(DayNumber64 left, DayNumber64 right) =>
            left._daysSinceZero >= right._daysSinceZero;

        [Pure]
        public static DayNumber64 Min(DayNumber64 x, DayNumber64 y) => x < y ? x : y;

        [Pure]
        public static DayNumber64 Max(DayNumber64 x, DayNumber64 y) => x > y ? x : y;

        [Pure]
        public int CompareTo(DayNumber64 other) => _daysSinceZero.CompareTo(other._daysSinceZero);

        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is DayNumber64 dayNumber ? _daysSinceZero.CompareTo(dayNumber._daysSinceZero)
            : Throw.NonComparable(typeof(DayNumber64), obj);
    }

    public partial struct DayNumber64 // Math ops
    {
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

        public static long operator -(DayNumber64 left, DayNumber64 right) =>
            checked(left._daysSinceZero - right._daysSinceZero);

        public static DayNumber64 operator +(DayNumber64 value, long days)
        {
            long newDays = checked(value._daysSinceZero + days);
            if (newDays < MinDaysSinceZero || MaxDaysSinceZero < newDays)
            {
                Throw.DayNumberOverflow();
            }
            return new DayNumber64(newDays);
        }

        public static DayNumber64 operator -(DayNumber64 value, long days)
        {
            long newDays = checked(value._daysSinceZero - days);
            if (newDays < MinDaysSinceZero || MaxDaysSinceZero < newDays)
            {
                Throw.DayNumberOverflow();
            }
            return new DayNumber64(newDays);
        }

        public static DayNumber64 operator ++(DayNumber64 value) => value.NextDay();

        public static DayNumber64 operator --(DayNumber64 value) => value.PreviousDay();

        public static DayNumber64 operator +(DayNumber64 left, int right) => left + (long)right;

        public static DayNumber64 operator -(DayNumber64 left, int right) => left - (long)right;

        static int IDifferenceOperators<DayNumber64, int>.operator -(DayNumber64 left, DayNumber64 right) =>
            checked((int)(left - right));

#pragma warning restore CA2225

        [Pure]
        public long CountDaysSince(DayNumber64 other) => this - other;

        [Pure]
        public DayNumber64 PlusDays(long days) => this + days;

        [Pure]
        public DayNumber64 NextDay() =>
            this == MaxValue ? Throw.DayNumberOverflow<DayNumber64>() : new DayNumber64(_daysSinceZero + 1);

        [Pure]
        public DayNumber64 PreviousDay() =>
            this == MinValue ? Throw.DayNumberOverflow<DayNumber64>() : new DayNumber64(_daysSinceZero - 1);

        [Pure]
        int IStandardArithmetic<DayNumber64>.CountDaysSince(DayNumber64 other) =>
            checked((int)(this - other));

        [Pure]
        DayNumber64 IStandardArithmetic<DayNumber64>.PlusDays(int days) => this + days;
    }
}
