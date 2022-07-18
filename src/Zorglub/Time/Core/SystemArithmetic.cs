// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Arithmetic;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Core.Validation;

    // TODO(doc): better explanation for the meaning of MinMinDaysInMonth and
    // MaxDaysViaDayOfMonth.
    // Explain that we require complete years (SystemSegment).

    #region Developer Notes

    // Types Derived from SystemArithmetic
    // -----------------------------------
    //
    // SystemArithmetic [A]  (SystemSchema)
    // ├─ GregorianSystemArithmetic     (GregorianSchema)
    // ├─ LunarSystemArithmetic         (-)
    // ├─ LunisolarSystemArithmetic     (-)
    // ├─ PlainSystemArithmetic         (-)
    // ├─ RegularSystemArithmetic       (-)
    // └─ SolarSystemArithmetic [A]     (-)
    //    ├─ Solar12SystemArithmetic    (-)
    //    └─ Solar13SystemArithmetic    (-)
    //
    // Annotation: [A] = abstract
    //
    // Comments
    // --------
    // SystemArithmetic is more naturally part of SystemSchema but
    // the code being the same for very different types of schemas, adding the
    // members of this interface to SystemSchema would lead to a lot of
    // duplications. Therefore this is just an implementation detail and one
    // should really use the public property ICalendricalSchema.Arithmetic.
    //
    // An implementation of SystemArithmetic should follow the rules of
    // ICalendricalSchema: no overflow, lenient methods, same range of years,
    // etc.
    //
    // All methods assume that a Yemoda (Yemo, or Yedoy) input forms a valid
    // object for the underlying schema.

    #endregion

    /// <summary>
    /// Defines the core mathematical operations on dates and months, and provides a base for derived
    /// classes.
    /// </summary>
    /// <remarks>
    /// <para>Operations are <i>lenient</i>, they assume that their parameters are valid from a
    /// calendrical point of view. They MUST ensure that all returned values are valid when the
    /// previous condition is met.</para>
    /// </remarks>
    public abstract partial class SystemArithmetic
    {
        /// <summary>
        /// Represents the absolute minimum value admissible for the minimum total number of days
        /// there is at least in a month.
        /// <para>This field is a constant equal to 7.</para>
        /// </summary>
        // The value has been chosen such that we can call AddDaysViaDayOfMonth()
        // safely when adjusting the day of the week.
        private protected const int MinMinDaysInMonth = 7;

        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="SystemArithmetic"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="segment"/> is null.</exception>
        protected SystemArithmetic(SystemSegment segment)
        {
            Segment = segment ?? throw new ArgumentNullException(nameof(segment));

            Schema = segment.Schema;

            SupportedDays = new SupportedDays(segment.SupportedDays);
            SupportedMonths = new SupportedMonths(segment.SupportedMonths);
            SupportedYears = new SupportedYears(segment.SupportedYears);

            (MinYear, MaxYear) = segment.SupportedYears.Endpoints;
            MaxDaysViaDayOfYear = Schema.MinDaysInYear;
            MaxDaysViaDayOfMonth = Schema.MinDaysInMonth;
        }

        /// <summary>
        /// Gets the segment of supported days.
        /// </summary>
        public SystemSegment Segment { get; }

        /// <summary>
        /// Gets the range of supported days.
        /// </summary>
        protected SupportedDays SupportedDays { get; }

        /// <summary>
        /// Gets the range of supported months.
        /// </summary>
        protected SupportedMonths SupportedMonths { get; }

        /// <summary>
        /// Gets the range of supported years.
        /// </summary>
        protected SupportedYears SupportedYears { get; }

        /// <summary>
        /// Gets the underlying schema.
        /// </summary>
        protected SystemSchema Schema { get; }

        /// <summary>
        /// Gets the earliest supported year.
        /// </summary>
        protected int MinYear { get; }

        /// <summary>
        /// Gets the latest supported year.
        /// </summary>
        protected int MaxYear { get; }

        /// <summary>
        /// Gets the maximum absolute value for a parameter "days" for the method
        /// <see cref="AddDaysViaDayOfYear(Yedoy, int)"/>.
        /// </summary>
        public int MaxDaysViaDayOfYear { get; init; }

        /// <summary>
        /// Gets the maximum absolute value for a parameter "days" for the method
        /// <see cref="AddDaysViaDayOfMonth(Yemoda, int)"/>.
        /// </summary>
        public int MaxDaysViaDayOfMonth { get; init; }

        /// <summary>
        /// Creates the default arithmetic object for the specified schema and range of supported
        /// years.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="AoorException"><paramref name="supportedYears"/> is NOT a subinterval
        /// of the range of supported years by <paramref name="schema"/>.</exception>
        [Pure]
        public static SystemArithmetic CreateDefault(SystemSchema schema, Range<int> supportedYears) =>
            CreateDefault(SystemSegment.Create(schema, supportedYears));

        /// <summary>
        /// Creates the default arithmetic object for the specified segment.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="segment"/> is null.</exception>
        [Pure]
        public static SystemArithmetic CreateDefault(SystemSegment segment)
        {
            Requires.NotNull(segment);

            var sch = segment.Schema;

            if (sch is GregorianSchema)
            {
                return new GregorianSystemArithmetic(segment);
            }

            return sch.Profile switch
            {
                CalendricalProfile.Solar12 => new Solar12SystemArithmetic(segment),
                CalendricalProfile.Solar13 => new Solar13SystemArithmetic(segment),
                CalendricalProfile.Lunar => new LunarSystemArithmetic(segment),
                CalendricalProfile.Lunisolar => new LunisolarSystemArithmetic(segment),

                // (no longer true)
                // NB: there is no real gain to expect in trying to improve the
                // perf for regular schemas except for month ops. Not convinced?
                // Check the code, we only call CountMonthsInYear() in two
                // corner cases.
                _ => sch.MinDaysInMonth >= MinMinDaysInMonth && sch.IsRegular(out _)
                    ? new RegularSystemArithmetic(segment)
                    : new PlainSystemArithmetic(segment)
            };
        }
    }

    public partial class SystemArithmetic // Standard operations
    {
        //
        // Operations on Yemoda
        //

        /// <summary>
        /// Adds a number of days to the specified date, yielding a new date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] public abstract Yemoda AddDays(Yemoda ymd, int days);

        /// <summary>
        /// Obtains the day after the specified date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] public abstract Yemoda NextDay(Yemoda ymd);

        /// <summary>
        /// Obtains the day before the specified date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] public abstract Yemoda PreviousDay(Yemoda ymd);

        /// <summary>
        /// Counts the number of days between the two specified dates.
        /// </summary>
        [Pure]
        public int CountDaysBetween(Yemoda start, Yemoda end)
        {
            if (end.Yemo == start.Yemo) { return end.Day - start.Day; }

            start.Unpack(out int y0, out int m0, out int d0);
            end.Unpack(out int y1, out int m1, out int d1);

            return Schema.CountDaysSinceEpoch(y1, m1, d1) - Schema.CountDaysSinceEpoch(y0, m0, d0);
        }

        //
        // Operations on Yedoy
        //

        /// <summary>
        /// Adds a number of days to the specified ordinal date, yielding a new ordinal date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] public abstract Yedoy AddDays(Yedoy ydoy, int days);

        /// <summary>
        /// Obtains the day after the specified ordinal date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] public abstract Yedoy NextDay(Yedoy ydoy);

        /// <summary>
        /// Obtains the day before the specified ordinal date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] public abstract Yedoy PreviousDay(Yedoy ydoy);

        /// <summary>
        /// Counts the number of days between the two specified ordinal dates.
        /// </summary>
        [Pure]
        public int CountDaysBetween(Yedoy start, Yedoy end)
        {
            if (end.Year == start.Year) { return end.DayOfYear - start.DayOfYear; }

            start.Unpack(out int y0, out int doy0);
            end.Unpack(out int y1, out int doy1);

            return Schema.CountDaysSinceEpoch(y1, doy1) - Schema.CountDaysSinceEpoch(y0, doy0);
        }

        //
        // Operations on Yemo
        //

        /// <summary>
        /// Adds a number of months to the specified month, yielding a new month.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure]
        public abstract Yemo AddMonths(Yemo ym, int months);

        // REVIEW(code): optimize Next/PreviousMonth(). See GregorianSystemArithmetic.

        /// <summary>
        /// Obtains the month after the specified month.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure]
        public Yemo NextMonth(Yemo ym) => AddMonths(ym, 1);

        /// <summary>
        /// Obtains the month before the specified month.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure]
        public Yemo PreviousMonth(Yemo ym) => AddMonths(ym, -1);

        /// <summary>
        /// Counts the number of months between the two specified months.
        /// </summary>
        [Pure]
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "F# & VB.NET End statement.")]
        public abstract int CountMonthsBetween(Yemo start, Yemo end);
    }

    public partial class SystemArithmetic // Non-standard operations
    {
        /// <summary>
        /// Adds a number of years to the year field of the specified date.
        /// </summary>
        /// <returns>The end of the target month (resp. year) when the naive result is not a valid
        /// day (resp. month).</returns>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        [Pure] public abstract Yemoda AddYears(Yemoda ymd, int years, out int roundoff);

        /// <summary>
        /// Adds a number of months to the specified date.
        /// </summary>
        /// <returns>The last day of the month when the naive result is not a valid day
        /// (roundoff > 0).</returns>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        [Pure] public abstract Yemoda AddMonths(Yemoda ymd, int months, out int roundoff);

        /// <summary>
        /// Adds a number of years to the year field of the specified ordinal date.
        /// </summary>
        /// <returns>The last day of the year when the naive result is not a valid day
        /// (roundoff > 0).</returns>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        [Pure] public abstract Yedoy AddYears(Yedoy ydoy, int years, out int roundoff);

        /// <summary>
        /// Adds a number of years to the year field of the specified month.
        /// </summary>
        /// <returns>The last month of the year when the naive result is not a valid month
        /// (roundoff > 0).</returns>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        [Pure] public abstract Yemo AddYears(Yemo ym, int years, out int roundoff);
    }

    public partial class SystemArithmetic // Fast operations
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
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] protected internal abstract Yemoda AddDaysViaDayOfMonth(Yemoda ymd, int days);

        /// <summary>
        /// Adds a number of days to the specified ordinal date, yielding a new ordinal date within
        /// the same year or one of the two contiguous years.
        /// <para><paramref name="days"/> MUST be in the range
        /// [-<see cref="MaxDaysViaDayOfYear"/>..<see cref="MaxDaysViaDayOfYear"/>].</para>
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the range of supported
        /// values.</exception>
        [Pure] protected internal abstract Yedoy AddDaysViaDayOfYear(Yedoy ydoy, int days);
    }
}