// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System.Linq;

    using Zorglub.Time.Core;

    public sealed class BoundedBelowDayProvider : IDayProvider<DayNumber>
    {
        /// <summary>
        /// Represents the epoch of the scope.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly DayNumber _epoch;

        /// <summary>
        /// Represents the underlying calendrical schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalSchema _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedBelowDayProvider"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public BoundedBelowDayProvider(BoundedBelowScope scope)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));

            _schema = scope.Schema;
            _epoch = scope.Epoch;

            MinDayNumber = scope.Domain.Min;

            MinYear = scope.SupportedYears.Min;

            MinDateParts = scope.MinMaxDateParts.LowerValue;
            MinOrdinalParts = scope.MinMaxOrdinalParts.LowerValue;
        }

        /// <summary>
        /// Gets the calendrical scope, an object providing validation methods.
        /// </summary>
        public ICalendarScope Scope { get; }

        /// <summary>
        /// Gets the earliest supported <see cref="DayNumber"/>
        /// </summary>
        private DayNumber MinDayNumber { get; }

        /// <summary>
        /// Gets the earliest supported year.
        /// </summary>
        private int MinYear { get; }

        /// <summary>
        /// Gets the earliest supported "date".
        /// </summary>
        private DateParts MinDateParts { get; }

        /// <summary>
        /// Gets the earliest supported ordinal "date".
        /// </summary>
        private OrdinalParts MinOrdinalParts { get; }

        // TODO(code): shouldn't be here.

        /// <summary>
        /// Obtains the number of days in the first supported year.
        /// </summary>
        [Pure]
        public int CountDaysInFirstYear() =>
            _schema.CountDaysInYear(MinYear) - MinOrdinalParts.DayOfYear + 1;

        /// <summary>
        /// Obtains the number of days in the first supported month.
        /// </summary>
        [Pure]
        public int CountDaysInFirstMonth()
        {
            var (y, m, d) = MinDateParts;
            return _schema.CountDaysInMonth(y, m) - d + 1;
        }

        /// <inheritdoc />
        [Pure]
        public IEnumerable<DayNumber> GetDaysInYear(int year)
        {
            Scope.ValidateYear(year);
            int startOfYear, daysInYear;
            if (year == MinYear)
            {
                startOfYear = MinDayNumber - _epoch;
                daysInYear = CountDaysInFirstYear();
            }
            else
            {
                startOfYear = _schema.GetStartOfYear(year);
                daysInYear = _schema.CountDaysInYear(year);
            }

            return Iterator();

            IEnumerable<DayNumber> Iterator()
            {
                return from daysSinceEpoch
                       in Enumerable.Range(startOfYear, daysInYear)
                       select _epoch + daysSinceEpoch;
            }
        }

        /// <inheritdoc />
        [Pure]
        public IEnumerable<DayNumber> GetDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int startOfMonth, daysInMonth;
            if (new MonthParts(year, month) == MinDateParts.MonthParts)
            {
                startOfMonth = MinDayNumber - _epoch;
                daysInMonth = CountDaysInFirstMonth();
            }
            else
            {
                startOfMonth = _schema.GetStartOfMonth(year, month);
                daysInMonth = _schema.CountDaysInMonth(year, month);
            }

            return Iterator();

            IEnumerable<DayNumber> Iterator()
            {
                return from daysSinceEpoch
                       in Enumerable.Range(startOfMonth, daysInMonth)
                       select _epoch + daysSinceEpoch;
            }
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetStartOfYear(int year)
        {
            Scope.ValidateYear(year);
            return year == MinYear
                ? Throw.ArgumentOutOfRange<DayNumber>(nameof(year))
                : _epoch + _schema.GetStartOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetEndOfYear(int year)
        {
            Scope.ValidateYear(year);
            return _epoch + _schema.GetEndOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return new MonthParts(year, month) == MinDateParts.MonthParts
                ? Throw.ArgumentOutOfRange<DayNumber>(nameof(month))
                : _epoch + _schema.GetStartOfMonth(year, month);
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetEndOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return _epoch + _schema.GetEndOfMonth(year, month);
        }
    }
}
