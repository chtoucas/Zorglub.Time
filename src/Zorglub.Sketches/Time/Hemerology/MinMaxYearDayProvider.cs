// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System.Linq;

    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology.Scopes;

    // REVIEW: Name? Today() -> move this to the scope?
    // [Pure]
    // public DayNumber Today()
    // {
    //     // We cannot know in advance if today is valid.
    //     var today = DayNumber.Today();
    //     Scope.ValidateDayNumber(today);
    //     return today;
    // }

    public sealed class MinMaxYearDayProvider : IDayProvider<DayNumber>
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
        /// Initializes a new instance of the <see cref="MinMaxYearDayProvider"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public MinMaxYearDayProvider(MinMaxYearScope scope)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));

            _epoch = scope.Epoch;
            _schema = scope.Schema;

            SupportedYears = scope.Segment.SupportedYears;
        }

        /// <summary>
        /// Gets the calendrical scope, an object providing validation methods.
        /// </summary>
        public CalendarScope Scope { get; }

        /// <summary>
        /// Gets the range of supported years.
        /// </summary>
        private SupportedYears SupportedYears { get; }

        /// <inheritdoc />
        [Pure]
        public IEnumerable<DayNumber> GetDaysInYear(int year)
        {
            // Check arg eagerly.
            SupportedYears.Validate(year);

            return Iterator();

            IEnumerable<DayNumber> Iterator()
            {
                int startOfYear = _schema.GetStartOfYear(year);
                int daysInYear = _schema.CountDaysInYear(year);

                return from daysSinceEpoch
                       in Enumerable.Range(startOfYear, daysInYear)
                       select _epoch + daysSinceEpoch;
            }
        }

        /// <inheritdoc />
        [Pure]
        public IEnumerable<DayNumber> GetDaysInMonth(int year, int month)
        {
            // Check arg eagerly.
            Scope.ValidateYearMonth(year, month);

            return Iterator();

            IEnumerable<DayNumber> Iterator()
            {
                int startOfMonth = _schema.GetStartOfMonth(year, month);
                int daysInMonth = _schema.CountDaysInMonth(year, month);

                return from daysSinceEpoch
                       in Enumerable.Range(startOfMonth, daysInMonth)
                       select _epoch + daysSinceEpoch;
            }
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetStartOfYear(int year)
        {
            SupportedYears.Validate(year);
            return _epoch + _schema.GetStartOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetEndOfYear(int year)
        {
            SupportedYears.Validate(year);
            return _epoch + _schema.GetEndOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return _epoch + _schema.GetStartOfMonth(year, month);
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
