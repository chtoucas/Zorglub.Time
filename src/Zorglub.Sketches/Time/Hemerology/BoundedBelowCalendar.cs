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
    /// Represents a calendar without a dedicated companion date type and with dates on or after a
    /// given date.
    /// <para>The aforementioned date can NOT be the start of a year.</para>
    /// </summary>
    public partial class BoundedBelowCalendar : BoundedBelowBasicCalendar, INakedCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedBelowCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public BoundedBelowCalendar(string name, BoundedBelowScope scope) : base(name, scope)
        {
            PartsAdapter = new PartsAdapter(Schema);
            DatePartsProvider = new DatePartsProvider_(this);
        }

        /// <inheritdoc />
        public IDateProvider<DateParts> DatePartsProvider { get; }

        /// <summary>
        /// Gets the adapter for calendrical parts.
        /// </summary>
        protected PartsAdapter PartsAdapter { get; }
    }

    public partial class BoundedBelowCalendar // Conversions
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

    public partial class BoundedBelowCalendar // IDateProvider<DayNumber>
    {
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

    public partial class BoundedBelowCalendar // Parts providers
    {
        private sealed class DatePartsProvider_ : IDateProvider<DateParts>
        {
            private readonly BoundedBelowCalendar _this;

            public DatePartsProvider_(BoundedBelowCalendar @this)
            {
                Debug.Assert(@this != null);

                _this = @this;
            }

            [Pure]
            public IEnumerable<DateParts> GetDaysInYear(int year)
            {
                throw new NotImplementedException();
            }

            [Pure]
            public IEnumerable<DateParts> GetDaysInMonth(int year, int month)
            {
                throw new NotImplementedException();
            }

            [Pure]
            public DateParts GetStartOfYear(int year)
            {
                _this.YearsValidator.Validate(year);
                return year == _this.MinYear
                    ? Throw.ArgumentOutOfRange<DateParts>(nameof(year))
                    : DateParts.AtStartOfYear(year);
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
                return new MonthParts(year, month) == _this.MinMonthParts
                    ? Throw.ArgumentOutOfRange<DateParts>(nameof(month))
                    : DateParts.AtStartOfMonth(year, month);
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
