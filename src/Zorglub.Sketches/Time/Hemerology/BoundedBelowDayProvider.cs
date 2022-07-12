// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System.Linq;

    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology.Scopes;

    public sealed class BoundedBelowDayProvider : IDayProvider<DayNumber>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedBelowDayProvider"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public BoundedBelowDayProvider(BoundedBelowScope scope)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));

            MinDayNumber = scope.Domain.Min;
            MinYear = scope.Segment.SupportedYears.Min;
        }

        /// <summary>
        /// Gets the calendrical scope, an object providing validation methods.
        /// </summary>
        public BoundedBelowScope Scope { get; }

        /// <summary>
        /// Gets the epoch of the scope.
        /// </summary>
        private DayNumber Epoch => Scope.Epoch;

        /// <summary>
        /// Gets the underlying calendrical schema.
        /// </summary>
        private ICalendricalSchema Schema => Scope.Schema;

        /// <summary>
        /// Gets the earliest supported <see cref="DayNumber"/>
        /// </summary>
        private DayNumber MinDayNumber { get; }

        /// <summary>
        /// Gets the earliest supported year.
        /// </summary>
        private int MinYear { get; }

        /// <summary>
        /// Gets the earliest supported month parts.
        /// </summary>
        private MonthParts MinMonthParts => Scope.MinMonthParts;

        /// <summary>
        /// Gets the earliest supported "date".
        /// </summary>
        private DateParts MinDateParts => Scope.MinDateParts;

        /// <summary>
        /// Gets the earliest supported ordinal "date".
        /// </summary>
        private OrdinalParts MinOrdinalParts => Scope.MinOrdinalParts;

        // TODO(code): shouldn't be here.

        /// <summary>
        /// Obtains the number of days in the first supported year.
        /// </summary>
        [Pure]
        public int CountDaysInFirstYear() =>
            Schema.CountDaysInYear(MinYear) - MinOrdinalParts.DayOfYear + 1;

        /// <summary>
        /// Obtains the number of days in the first supported month.
        /// </summary>
        [Pure]
        public int CountDaysInFirstMonth()
        {
            var (y, m, d) = MinDateParts;
            return Schema.CountDaysInMonth(y, m) - d + 1;
        }

        /// <inheritdoc />
        [Pure]
        public IEnumerable<DayNumber> GetDaysInYear(int year)
        {
            Scope.ValidateYear(year);
            int startOfYear, daysInYear;
            if (year == MinYear)
            {
                startOfYear = MinDayNumber - Epoch;
                daysInYear = CountDaysInFirstYear();
            }
            else
            {
                startOfYear = Schema.GetStartOfYear(year);
                daysInYear = Schema.CountDaysInYear(year);
            }

            return Iterator();

            IEnumerable<DayNumber> Iterator()
            {
                return from daysSinceEpoch
                       in Enumerable.Range(startOfYear, daysInYear)
                       select Epoch + daysSinceEpoch;
            }
        }

        /// <inheritdoc />
        [Pure]
        public IEnumerable<DayNumber> GetDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int startOfMonth, daysInMonth;
            if (new MonthParts(year, month) == MinMonthParts)
            {
                startOfMonth = MinDayNumber - Epoch;
                daysInMonth = CountDaysInFirstMonth();
            }
            else
            {
                startOfMonth = Schema.GetStartOfMonth(year, month);
                daysInMonth = Schema.CountDaysInMonth(year, month);
            }

            return Iterator();

            IEnumerable<DayNumber> Iterator()
            {
                return from daysSinceEpoch
                       in Enumerable.Range(startOfMonth, daysInMonth)
                       select Epoch + daysSinceEpoch;
            }
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetStartOfYear(int year)
        {
            Scope.ValidateYear(year);
            return year == MinYear
                ? Throw.ArgumentOutOfRange<DayNumber>(nameof(year))
                : Epoch + Schema.GetStartOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetEndOfYear(int year)
        {
            Scope.ValidateYear(year);
            return Epoch + Schema.GetEndOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return new MonthParts(year, month) == MinMonthParts
                ? Throw.ArgumentOutOfRange<DayNumber>(nameof(month))
                : Epoch + Schema.GetStartOfMonth(year, month);
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetEndOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return Epoch + Schema.GetEndOfMonth(year, month);
        }
    }
}
