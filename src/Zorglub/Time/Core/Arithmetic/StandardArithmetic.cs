// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;

    /// <summary>
    /// Defines the core mathematical operations on dates and provides a base for derived classes.
    /// </summary>
    /// <remarks>
    /// <para>Operations are <i>lenient</i>, they assume that their parameters are valid from a
    /// calendrical point of view, nevertheless they MUST ensure that all returned values are valid
    /// when the previous condition is met.</para>
    /// </remarks>
    internal abstract partial class StandardArithmetic : ICalendricalArithmetic
    {
        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="StandardArithmetic"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> and <see cref="Yemoda.SupportedYears"/> are disjoint.
        /// </exception>
        protected StandardArithmetic(ICalendricalSchema schema)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));

            var set = Interval.Intersect(schema.SupportedYears, Yemoda.SupportedYears);
            if (set.IsEmpty) Throw.Argument(nameof(schema));
            SupportedYears = set.Range;

            // REVIEW(code): checked or unchecked factory?
            PartsFactory = ICalendricalPartsFactory.Create(schema, @checked: false);

            var minMaxYear = SupportedYears.Endpoints;
            (MinYear, MaxYear) = minMaxYear;
            (MinDaysSinceEpoch, MaxDaysSinceEpoch) =
                minMaxYear.Select(schema.GetStartOfYear, schema.GetEndOfYear);

            MaxDaysViaDayOfYear = schema.MinDaysInYear;
            MaxDaysViaDayOfMonth = schema.MinDaysInMonth;
        }

        /// <inheritdoc/>
        public Range<int> SupportedYears { get; }

        /// <summary>
        /// Gets the underlying schema.
        /// </summary>
        protected ICalendricalSchema Schema { get; }

        /// <summary>
        /// Gets the factory for calendrical parts.
        /// </summary>
        protected ICalendricalPartsFactory PartsFactory { get; }

        /// <summary>
        /// Gets the earliest supported year.
        /// </summary>
        protected int MinYear { get; }

        /// <summary>
        /// Gets the latest supported year.
        /// </summary>
        protected int MaxYear { get; }

        /// <summary>
        /// Gets the minimum possible value for the number of consecutive days from the epoch.
        /// </summary>
        protected int MinDaysSinceEpoch { get; }

        /// <summary>
        /// Gets the maximum possible value for the number of consecutive days from the epoch.
        /// </summary>
        protected int MaxDaysSinceEpoch { get; }

        /// <summary>
        /// Gets the maximum absolute value for a parameter "days" for the method
        /// <see cref="AddDaysViaDayOfYear(Yedoy, int)"/>.
        /// </summary>
        public int MaxDaysViaDayOfYear { get; }

        // FIXME(doc): MaxDaysViaDayOfMonth.

        /// <summary>
        /// Gets the maximum absolute value for a parameter "days" for the method
        /// <see cref="AddDaysViaDayOfMonth(Yemoda, int)"/>.
        /// </summary>
        /// <remarks>
        /// <para>The value is guaranteed to be greater than or equal to
        /// <see cref="FastArithmetic.MinMinDaysInMonth"/>.</para>
        /// </remarks>
        public int MaxDaysViaDayOfMonth { get; }

        /// <summary>
        /// Creates the default fast arithmetic engine.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="schema"/> contains at least one
        /// month whose length is strictly less than <see cref="MinMinDaysInMonth"/>.</exception>
        [Pure]
        public static StandardArithmetic Create(CalendricalSchema schema)
        {
            Requires.NotNull(schema);

            // NB: there is no real gain to expect in trying to improve the perf
            // for regular schemas. Not convinced? Check the code, we only
            // call CountMonthsInYear() in two corner cases.
            return schema.Profile switch
            {
                CalendricalProfile.Solar12 =>
                    schema is GregorianSchema gr
                    ? new GregorianArithmetic(gr)
                    : new Solar12Arithmetic(schema),
                CalendricalProfile.Solar13 => new Solar13Arithmetic(schema),
                CalendricalProfile.Lunar => new LunarArithmetic(schema),
                CalendricalProfile.Lunisolar => new LunisolarArithmetic(schema),

                // WARNING: if we change this, we MUST update
                // CalendricalSchema.TryGetCustomArithmetic() too.
                _ => schema.MinDaysInMonth >= FastArithmetic.MinMinDaysInMonth
                    ? new PlainFastArithmetic(schema)
                    // We do not provide a fast arithmetic for schemas with a
                    // virtual thirteen month.
                    : new PlainArithmetic(schema)
            };
        }
    }

    internal partial class StandardArithmetic // ICalendricalArithmetic
    {
        //
        // Operations on Yemoda
        //

        /// <inheritdoc />
        [Pure] public abstract Yemoda AddDays(Yemoda ymd, int days);

        /// <inheritdoc />
        [Pure] public abstract Yemoda NextDay(Yemoda ymd);

        /// <inheritdoc />
        [Pure] public abstract Yemoda PreviousDay(Yemoda ymd);

        /// <inheritdoc />
        [Pure] public abstract int CountDaysBetween(Yemoda start, Yemoda end);

        //
        // Operations on Yedoy
        //

        /// <inheritdoc />
        [Pure] public abstract Yedoy AddDays(Yedoy ydoy, int days);

        /// <inheritdoc />
        [Pure] public abstract Yedoy NextDay(Yedoy ydoy);

        /// <inheritdoc />
        [Pure] public abstract Yedoy PreviousDay(Yedoy ydoy);

        /// <inheritdoc />
        [Pure] public abstract int CountDaysBetween(Yedoy start, Yedoy end);
    }

    internal partial class StandardArithmetic // Fast operations
    {
        // AddDaysViaDayOfYear().
        // Only when we know in advance that |days| <= MaxDaysViaDayOfYear.
        // The limits are chosen such that (ymd + days) is guaranteed to
        // stay in the range from (y - 1) to (y + 1).
        // - Faster when "days is small" (no change of month) or "big" (slow
        //   track).
        // - Slower for "days in between", where we have to compute the day
        //   of the year.

        /// <summary>
        /// Adds a number of days to the specified date, yielding a new date within the same month
        /// or one of the two contiguous months.
        /// <para><paramref name="days"/> MUST be in the range
        /// [-<see cref="MaxDaysViaDayOfMonth"/>..<see cref="MaxDaysViaDayOfMonth"/>].</para>
        /// <para>This method is used to adjust the day of the week, which means that we must ensure
        /// that <see cref="MaxDaysViaDayOfMonth"/> is greater than or equal to 7.</para>
        /// </summary>
        /// <exception cref="AoorException"><paramref name="days"/> is not in the range
        /// [-<see cref="MaxDaysViaDayOfMonth"/>..<see cref="MaxDaysViaDayOfMonth"/>].</exception>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] protected internal abstract Yemoda AddDaysViaDayOfMonth(Yemoda ymd, int days);

        /// <summary>
        /// Adds a number of days to the specified ordinal date, yielding a new ordinal date within
        /// the same year or one of the two contiguous years.
        /// <para><paramref name="days"/> MUST be in the range
        /// [-<see cref="MaxDaysViaDayOfYear"/>..<see cref="MaxDaysViaDayOfYear"/>].</para>
        /// </summary>
        /// <exception cref="AoorException"><paramref name="days"/> is not in the range
        /// [-<see cref="MaxDaysViaDayOfYear"/>..<see cref="MaxDaysViaDayOfYear"/>].</exception>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] protected internal abstract Yedoy AddDaysViaDayOfYear(Yedoy ydoy, int days);
    }
}