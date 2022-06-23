// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    using Zorglub.Time.Core.Intervals;

    using __Solar = CalendricalConstants.Solar;

    /// <summary>
    /// Defines the core mathematical operations on dates for schemas with profile
    /// <see cref="CalendricalProfile.Solar12"/> or <see cref="CalendricalProfile.Solar13"/>, and
    /// provides a base for derived classes.
    /// </summary>
    internal abstract partial class SolarArithmetic : SystemArithmetic
    {
        protected const int MinDaysInYear = __Solar.MinDaysInYear;
        protected const int MinDaysInMonth = __Solar.MinDaysInMonth;
        protected const int MaxDaysViaDayOfYear_ = MinDaysInYear;
        protected const int MaxDaysViaDayOfMonth_ = MinDaysInMonth;

        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="SolarArithmetic"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> and <see cref="Yemoda"/> are disjoint.
        /// </exception>
        protected SolarArithmetic(CalendricalSchema schema, Range<int>? supportedYears)
            : base(schema, supportedYears)
        {
            // Disabled, otherwise we cannot test the derived constructors.
            // Not that important since this class is internal.
            Debug.Assert(MaxDaysViaDayOfMonth_ >= MinMinDaysInMonth);
            //Debug.Assert(schema.Profile == CalendricalProfile.Solar12
            //    || schema.Profile == CalendricalProfile.Solar13);

            MaxDaysViaDayOfYear = MaxDaysViaDayOfYear_;
            MaxDaysViaDayOfMonth = MaxDaysViaDayOfMonth_;
        }
    }

    internal partial class SolarArithmetic // Operations on Yemoda
    {
        /// <inheritdoc />
        [Pure]
        public sealed override Yemoda AddDays(Yemoda ymd, int days)
        {
            // Fast tracks.
            if (-MaxDaysViaDayOfMonth_ <= days && days <= MaxDaysViaDayOfMonth_)
            {
                return AddDaysViaDayOfMonth(ymd, days);
            }

            ymd.Unpack(out int y, out int m, out int d);
            if (-MaxDaysViaDayOfYear_ <= days && days <= MaxDaysViaDayOfYear_)
            {
                int doy = Schema.GetDayOfYear(y, m, d);
                var (newY, newDoy) = AddDaysViaDayOfYear(new Yedoy(y, doy), days);
                return PartsFactory.GetDateParts(newY, newDoy);
            }

            // Slow track.
            int daysSinceEpoch = checked(Schema.CountDaysSinceEpoch(y, m, d) + days);
            if (Domain.Contains(daysSinceEpoch) == false) Throw.DateOverflow();

            return PartsFactory.GetDateParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override Yemoda PreviousDay(Yemoda ymd)
        {
            ymd.Unpack(out int y, out int m, out int d);

            return
                d > 1 ? new Yemoda(y, m, d - 1)
                : m > 1 ? PartsFactory.GetEndOfMonthParts(y, m - 1)
                : y > MinYear ? PartsFactory.GetEndOfYearParts(y - 1)
                : Throw.DateOverflow<Yemoda>();
        }
    }

    internal partial class SolarArithmetic // Operations on Yedoy
    {
        /// <inheritdoc />
        [Pure]
        public sealed override Yedoy AddDays(Yedoy ydoy, int days)
        {
            // Fast track.
            if (-MaxDaysViaDayOfYear_ <= days && days <= MaxDaysViaDayOfYear_)
            {
                return AddDaysViaDayOfYear(ydoy, days);
            }

            ydoy.Unpack(out int y, out int doy);

            // Slow track.
            int daysSinceEpoch = checked(Schema.CountDaysSinceEpoch(y, doy) + days);
            if (Domain.Contains(daysSinceEpoch) == false) Throw.DateOverflow();

            return PartsFactory.GetOrdinalParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        protected internal sealed override Yedoy AddDaysViaDayOfYear(Yedoy ydoy, int days)
        {
            Debug.Assert(-MaxDaysViaDayOfYear_ <= days);
            Debug.Assert(days <= MaxDaysViaDayOfYear_);

            ydoy.Unpack(out int y, out int doy);

            doy += days;
            if (doy < 1)
            {
                if (y == MinYear) Throw.DateOverflow();
                y--;
                doy += Schema.CountDaysInYear(y);
            }
            else
            {
                int daysInYear = Schema.CountDaysInYear(y);
                if (doy > daysInYear)
                {
                    if (y == MaxYear) Throw.DateOverflow();
                    y++;
                    doy -= daysInYear;
                }
            }

            return new Yedoy(y, doy);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override Yedoy NextDay(Yedoy ydoy)
        {
            ydoy.Unpack(out int y, out int doy);

            return
                doy < MinDaysInYear || doy < Schema.CountDaysInYear(y)
                    ? new Yedoy(y, doy + 1)
                : y < MaxYear ? Yedoy.AtStartOfYear(y + 1)
                : Throw.DateOverflow<Yedoy>();
        }

        /// <inheritdoc />
        [Pure]
        public sealed override Yedoy PreviousDay(Yedoy ydoy)
        {
            ydoy.Unpack(out int y, out int doy);

            return doy > 1 ? new Yedoy(y, doy - 1)
                : y > MinYear ? PartsFactory.GetEndOfYearOrdinalParts(y - 1)
                : Throw.DateOverflow<Yedoy>();
        }
    }

    internal partial class SolarArithmetic // ICalendricalArithmeticPlus
    {
        /// <inheritdoc />
        [Pure]
        public sealed override Yemoda AddYears(Yemoda ymd, int years, out int roundoff)
        {
            ymd.Unpack(out int y, out int m, out int d);

            y = checked(y + years);

            if (SupportedYears.Contains(y) == false) Throw.DateOverflow();

            int daysInMonth = Schema.CountDaysInMonth(y, m);
            roundoff = Math.Max(0, d - daysInMonth);
            // On retourne le dernier jour du mois si d > daysInMonth.
            return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override Yedoy AddYears(Yedoy ydoy, int years, out int roundoff)
        {
            ydoy.Unpack(out int y, out int doy);

            y = checked(y + years);

            if (SupportedYears.Contains(y) == false) Throw.DateOverflow();

            int daysInYear = Schema.CountDaysInYear(y);
            roundoff = Math.Max(0, doy - daysInYear);
            return new Yedoy(y, roundoff > 0 ? daysInYear : doy);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override Yemo AddYears(Yemo ym, int years, out int roundoff)
        {
            ym.Unpack(out int y, out int m);

            y = checked(y + years);

            if (SupportedYears.Contains(y) == false) Throw.MonthOverflow();

            roundoff = 0;
            return new Yemo(y, m);
        }
    }
}
