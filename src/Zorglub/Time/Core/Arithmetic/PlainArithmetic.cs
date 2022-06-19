// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    /// <summary>
    /// Defines a plain implementation for <see cref="SystemArithmetic"/> and provides a base for
    /// derived classes.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal abstract partial class PlainArithmetic : SystemArithmetic
    {
        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="PlainArithmetic"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> and <see cref="Yemoda"/> are disjoint.
        /// </exception>
        protected PlainArithmetic(ICalendricalSchema schema) : base(schema) { }

        /// <summary>
        /// Creates the plain arithmetic engine.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        [Pure]
        public static PlainArithmetic Create(ICalendricalSchema schema)
        {
            Requires.NotNull(schema);

            return schema.MinDaysInMonth >= MinMinDaysInMonth
                ? new PlainFastArithmetic(schema)
                : new PlainSlowArithmetic(schema);
        }
    }

    internal partial class PlainArithmetic // Operations on Yemoda
    {
        /// <inheritdoc />
        [Pure]
        public sealed override Yemoda PreviousDay(Yemoda ymd)
        {
            ymd.Unpack(out int y, out int m, out int d);

            return
                // Same month, the day before.
                d > 1 ? new Yemoda(y, m, d - 1)
                // Same year, end of previous month.
                : m > 1 ? PartsFactory.GetEndOfMonthParts(y, m - 1)
                // End of previous year...
                : y > MinYear ? PartsFactory.GetEndOfYearParts(y - 1)
                // ... or overflow.
                : Throw.DateOverflow<Yemoda>();
        }
    }

    internal partial class PlainArithmetic // Operations on Yedoy
    {
        /// <inheritdoc />
        [Pure]
        public sealed override Yedoy AddDays(Yedoy ydoy, int days)
        {
            // Fast track.
            if (-MaxDaysViaDayOfYear <= days && days <= MaxDaysViaDayOfYear)
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
            Debug.Assert(-MaxDaysViaDayOfYear <= days);
            Debug.Assert(days <= MaxDaysViaDayOfYear);

            ydoy.Unpack(out int y, out int doy);

            // No need to use checked arithmetic here.
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
                doy < MaxDaysViaDayOfYear || doy < Schema.CountDaysInYear(y) ? new Yedoy(y, doy + 1)
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

    internal partial class PlainArithmetic // Operations on Yemo
    {
        /// <inheritdoc />
        [Pure]
        public override int CountMonthsBetween(Yemo start, Yemo end)
        {
            start.Unpack(out int y0, out int m0);
            end.Unpack(out int y1, out int m1);

            return Schema.CountMonthsSinceEpoch(y1, m1) - Schema.CountMonthsSinceEpoch(y0, m0);
        }
    }
}
