// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    using static Zorglub.Time.Core.CalendricalConstants;

    /// <summary>
    /// Defines the core mathematical operations on dates for schemas with profile
    /// <see cref="CalendricalProfile.Solar12"/> or <see cref="CalendricalProfile.Solar13"/>, and provides a
    /// base for derived classes.
    /// </summary>
    internal abstract partial class SolarArithmetic : FastArithmetic
    {
        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="SolarArithmetic"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        protected SolarArithmetic(SystemSchema schema) : base(schema)
        {
            // Disabled, otherwise we cannot test the derived constructors.
            // Not that important since this class is internal.
            //Debug.Assert(schema != null);
            //Debug.Assert(schema.Profile == CalendricalProfile.Solar12
            //    || schema.Profile == CalendricalProfile.Solar13);
        }

        /// <inheritdoc />
        public sealed override int MaxDaysViaDayOfYear => Solar.MinDaysInYear;

        /// <inheritdoc />
        public sealed override int MaxDaysViaDayOfMonth => Solar.MinDaysInMonth;
    }

    internal partial class SolarArithmetic // Operations on Yemoda.
    {
        /// <inheritdoc />
        [Pure]
        public sealed override Yemoda AddDays(Yemoda ymd, int days)
        {
            // Fast tracks.
            if (-Solar.MinDaysInMonth <= days && days <= Solar.MinDaysInMonth)
            {
                return AddDaysViaDayOfMonth(ymd, days);
            }

            ymd.Unpack(out int y, out int m, out int d);
            if (-Solar.MinDaysInYear <= days && days <= Solar.MinDaysInYear)
            {
                int doy = Schema.GetDayOfYear(y, m, d);
                var (newY, newDoy) = AddDaysViaDayOfYear(y, doy, days);
                return Schema.GetDateParts(newY, newDoy);
            }

            // Slow track.
            int daysSinceEpoch = checked(Schema.CountDaysSinceEpoch(y, m, d) + days);
            if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return Schema.GetDateParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override Yemoda PreviousDay(Yemoda ymd)
        {
            ymd.Unpack(out int y, out int m, out int d);

            return
                d > 1 ? new Yemoda(y, m, d - 1)
                : m > 1 ? Schema.GetEndOfMonthParts(y, m - 1)
                : y > MinYear ? Schema.GetEndOfYearParts(y - 1)
                : Throw.DateOverflow<Yemoda>();
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysBetween(Yemoda start, Yemoda end)
        {
            if (end.Yemo == start.Yemo) { return end.Day - start.Day; }

            start.Unpack(out int y0, out int m0, out int d0);
            end.Unpack(out int y1, out int m1, out int d1);

            return Schema.CountDaysSinceEpoch(y1, m1, d1) - Schema.CountDaysSinceEpoch(y0, m0, d0);
        }
    }

    internal partial class SolarArithmetic // Operations on Yedoy.
    {
        /// <inheritdoc />
        [Pure]
        public sealed override Yedoy AddDays(Yedoy ydoy, int days)
        {
            ydoy.Unpack(out int y, out int doy);

            // Fast track.
            if (-Solar.MinDaysInYear <= days && days <= Solar.MinDaysInYear)
            {
                return AddDaysViaDayOfYear(y, doy, days);
            }

            // Slow track.
            int daysSinceEpoch = checked(Schema.CountDaysSinceEpoch(y, doy) + days);
            if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return Schema.GetOrdinalParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        protected internal sealed override Yedoy AddDaysViaDayOfYear(int y, int doy, int days)
        {
            Debug.Assert(-Schema.MinDaysInYear <= days);
            Debug.Assert(days <= Schema.MinDaysInYear);

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
                doy < Solar.MinDaysInYear || doy < Schema.CountDaysInYear(y)
                    ? new Yedoy(y, doy + 1)
                : y < MaxYear ? Yedoy.AtStartOfYear(y + 1)
                : Throw.DateOverflow<Yedoy>();
        }

        /// <inheritdoc />
        [Pure]
        public sealed override Yedoy PreviousDay(Yedoy ydoy)
        {
            ydoy.Unpack(out int y, out int doy);

            return
                doy > 1 ? new Yedoy(y, doy - 1)
                : y > MinYear ? Schema.GetEndOfYearOrdinalParts(y - 1)
                : Throw.DateOverflow<Yedoy>();
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysBetween(Yedoy start, Yedoy end)
        {
            if (end.Year == start.Year) { return end.DayOfYear - start.DayOfYear; }

            start.Unpack(out int y0, out int doy0);
            end.Unpack(out int y1, out int doy1);

            return Schema.CountDaysSinceEpoch(y1, doy1) - Schema.CountDaysSinceEpoch(y0, doy0);
        }
    }
}
