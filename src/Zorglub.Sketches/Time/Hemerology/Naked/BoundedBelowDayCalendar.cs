// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Naked
{
    using System.Linq;

    using Zorglub.Time;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Represents a calendar with dates on or after a given date.
    /// <para>The aforementioned date can NOT be the start of a year.</para>
    /// </summary>
    public sealed class BoundedBelowDayCalendar : BoundedBelowCalendar, ICalendar<DayNumber>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedBelowDayCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public BoundedBelowDayCalendar(string name, BoundedBelowScope scope) : base(name, scope) { }

        /// <inheritdoc />
        [Pure]
        public IEnumerable<DayNumber> GetDaysInYear(int year)
        {
            YearsValidator.Validate(year);
            int startOfYear, daysInYear;
            if (year == MinYear)
            {
                startOfYear = Domain.Min - Epoch;
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
                startOfMonth = Domain.Min - Epoch;
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
            YearsValidator.Validate(year);
            return year == MinYear
                ? Throw.ArgumentOutOfRange<DayNumber>(nameof(year))
                : Epoch + Schema.GetStartOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetEndOfYear(int year)
        {
            YearsValidator.Validate(year);
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
