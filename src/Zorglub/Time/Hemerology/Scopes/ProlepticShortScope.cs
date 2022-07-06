// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Schemas;

    /// <summary>
    /// Represents the proleptic short scope of a schema and provides a base for derived classes.
    /// </summary>
    internal abstract class ProlepticShortScope : ShortScope
    {
        /// <summary>
        /// Represents the earliest supported year.
        /// <para>This field is a constant equal to -9998.</para>
        /// </summary>
        public const int MinYear = -9998;

        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="ProlepticShortScope"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> does not contain the interval [-9998..9999].</exception>
        protected ProlepticShortScope(ICalendricalSchema schema, DayNumber epoch)
            : base(schema, epoch, MinYear) { }

        /// <summary>
        /// Gets the checker for overflows of the range of years.
        /// </summary>
        public static IOverflowChecker<int> YearOverflowChecker { get; } = new YearOverflowChecker_();

        /// <summary>
        /// Creates the default proleptic short scope for the specified schema
        /// and epoch.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> does not contain the interval [-9998..9999].</exception>
        [Pure]
        public static ProlepticShortScope Create(ICalendricalSchema schema, DayNumber epoch)
        {
            Requires.NotNull(schema);

            return schema is GregorianSchema gr ? new GregorianProlepticShortScope(gr, epoch)
                : new PlainProlepticShortScope(schema, epoch);
        }

        /// <inheritdoc />
        public sealed override void ValidateYear(int year, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
        }

        private sealed class YearOverflowChecker_ : IOverflowChecker<int>
        {
            public void Check(int year)
            {
                if (year < MinYear || year > MaxYear) Throw.DateOverflow();
            }

            public void CheckUpperBound(int year)
            {
                if (year > MaxYear) Throw.DateOverflow();
            }

            public void CheckLowerBound(int year)
            {
                if (year < MinYear) Throw.DateOverflow();
            }
        }
    }
}
