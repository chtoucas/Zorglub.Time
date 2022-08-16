// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System.Linq;

    using Zorglub.Time;
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Represents a calendar with dates within a range of years.
    /// </summary>
    public partial class MinMaxYearNakedCalendar :
        MinMaxYearCalendar,
        INakedCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearNakedCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public MinMaxYearNakedCalendar(string name, MinMaxYearScope scope) : base(name, scope)
        {
            PartsAdapter = new PartsAdapter(Schema);
            DatePartsProvider = new DatePartsProvider_(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearNakedCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="scope"/> is not complete.</exception>
        public MinMaxYearNakedCalendar(string name, CalendarScope scope) : base(name, scope)
        {
            PartsAdapter = new PartsAdapter(Schema);
            DatePartsProvider = new DatePartsProvider_(this);
        }

        /// <summary>
        /// Gets the adapter for calendrical parts.
        /// </summary>
        protected PartsAdapter PartsAdapter { get; }

        public IDateProvider<DateParts> DatePartsProvider { get; }
    }

    public partial class MinMaxYearNakedCalendar // Conversions
    {
        /// <inheritdoc />
        [Pure]
        public DateParts GetDateParts(DayNumber dayNumber)
        {
            Domain.Validate(dayNumber);
            return PartsAdapter.GetDateParts(dayNumber - Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public DateParts GetDateParts(int year, int dayOfYear)
        {
            Scope.ValidateOrdinal(year, dayOfYear);
            return PartsAdapter.GetDateParts(year, dayOfYear);
        }

        /// <inheritdoc />
        [Pure]
        public OrdinalParts GetOrdinalParts(DayNumber dayNumber)
        {
            Domain.Validate(dayNumber);
            return PartsAdapter.GetOrdinalParts(dayNumber - Epoch);
        }

        /// <inheritdoc />
        [Pure]
        public OrdinalParts GetOrdinalParts(int year, int month, int day)
        {
            Scope.ValidateYearMonthDay(year, month, day);
            return PartsAdapter.GetOrdinalParts(year, month, day);
        }
    }

    public partial class MinMaxYearNakedCalendar // Dates in a given year or month
    {
        /// <inheritdoc/>
        [Pure]
        public IEnumerable<DayNumber> GetDaysInYear(int year)
        {
            YearsValidator.Validate(year);

            int startOfYear = Schema.GetStartOfYear(year);
            int daysInYear = Schema.CountDaysInYear(year);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfYear, daysInYear)
                   select Epoch + daysSinceEpoch;
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<DayNumber> GetDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);

            int startOfMonth = Schema.GetStartOfMonth(year, month);
            int daysInMonth = Schema.CountDaysInMonth(year, month);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfMonth, daysInMonth)
                   select Epoch + daysSinceEpoch;
        }

        /// <inheritdoc/>
        [Pure]
        public DayNumber GetStartOfYear(int year)
        {
            YearsValidator.Validate(year);
            int daysSinceEpoch = Schema.GetStartOfYear(year);
            return Epoch + daysSinceEpoch;
        }

        /// <inheritdoc/>
        [Pure]
        public DayNumber GetEndOfYear(int year)
        {
            YearsValidator.Validate(year);
            int daysSinceEpoch = Schema.GetEndOfYear(year);
            return Epoch + daysSinceEpoch;
        }

        /// <inheritdoc/>
        [Pure]
        public DayNumber GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetStartOfMonth(year, month);
            return Epoch + daysSinceEpoch;
        }

        /// <inheritdoc/>
        [Pure]
        public DayNumber GetEndOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            int daysSinceEpoch = Schema.GetEndOfMonth(year, month);
            return Epoch + daysSinceEpoch;
        }
    }

    public partial class MinMaxYearNakedCalendar // Date provider
    {
        private sealed class DatePartsProvider_ : IDateProvider<DateParts>
        {
            private readonly MinMaxYearNakedCalendar _this;

            public DatePartsProvider_(MinMaxYearNakedCalendar @this)
            {
                Debug.Assert(@this != null);

                _this = @this;
            }

            [Pure]
            public IEnumerable<DateParts> GetDaysInYear(int year)
            {
                // Check arg eagerly.
                _this.YearsValidator.Validate(year);

                return Iterator();

                IEnumerable<DateParts> Iterator()
                {
                    int monthsInYear = _this.Schema.CountMonthsInYear(year);

                    for (int m = 1; m <= monthsInYear; m++)
                    {
                        int daysInMonth = _this.Schema.CountDaysInMonth(year, m);

                        for (int d = 1; d <= daysInMonth; d++)
                        {
                            yield return new DateParts(year, m, d);
                        }
                    }
                }
            }

            [Pure]
            public IEnumerable<DateParts> GetDaysInMonth(int year, int month)
            {
                // Check arg eagerly.
                _this.Scope.ValidateYearMonth(year, month);

                return Iterator();

                IEnumerable<DateParts> Iterator()
                {
                    int daysInMonth = _this.Schema.CountDaysInMonth(year, month);

                    for (int d = 1; d <= daysInMonth; d++)
                    {
                        yield return new DateParts(year, month, d);
                    }
                }
            }

            [Pure]
            public DateParts GetStartOfYear(int year)
            {
                _this.YearsValidator.Validate(year);
                return DateParts.AtStartOfYear(year);
            }

            [Pure]
            public DateParts GetEndOfYear(int year)
            {
                _this.YearsValidator.Validate(year);
                return _this.PartsAdapter.GetDatePartsAtEndOfYear(year);
            }

            [Pure]
            public DateParts GetStartOfMonth(int year, int month)
            {
                _this.Scope.ValidateYearMonth(year, month);
                return DateParts.AtStartOfMonth(year, month);
            }

            [Pure]
            public DateParts GetEndOfMonth(int year, int month)
            {
                _this.Scope.ValidateYearMonth(year, month);
                return _this.PartsAdapter.GetDatePartsAtEndOfMonth(year, month);
            }
        }
    }
}
