// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Represents a scope with dates on or after a given date, but not the first day of a year.
    /// <para>This class can only represent subintervals of <see cref="Yemoda.SupportedYears"/>.
    /// </para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class BoundedBelowScope : CalendarScope
    {
        /// <summary>
        /// Represents the earliest supported "date".
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly Yemoda _minDateParts;

        /// <summary>
        /// Represents the earliest supported ordinal "date".
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly Yedoy _minOrdinalParts;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedBelowScope"/>
        /// class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="AoorException"><paramref name="year"/> or <paramref name="maxYear"/> is
        /// outside the range of years supported by <paramref name="schema"/> or <see cref="Yemoda"/>.
        /// </exception>
        public BoundedBelowScope(
            ICalendricalSchema schema,
            DayNumber epoch,
            int year,
            int month,
            int day,
            int? maxYear)
            : base(schema, epoch, GetSegment(schema, year, month, day, maxYear))
        {
            _minDateParts = MinMaxDateParts.LowerValue;
            _minOrdinalParts = MinMaxOrdinalParts.LowerValue;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CalendricalSegment"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="AoorException">The min date is not representable by the system.</exception>
        /// <exception cref="AoorException">The max year is out of range.</exception>
        /// <exception cref="ArgumentException">The range of years supported by
        /// <paramref name="schema"/> is a strict superset of <see cref="Yemoda.SupportedYears"/>.
        /// </exception>
        // Internal for testing.
        internal static CalendricalSegment GetSegment(
            ICalendricalSchema schema,
            int year,
            int month,
            int day,
            int? maxYear)
        {
            var minDate = Yemoda.Create(year, month, day);

            var builder = new CalendricalSegmentBuilder(schema);
            builder.SetMinDate(minDate);
            if (maxYear.HasValue)
            {
                builder.SetMaxYear(maxYear.Value);
            }
            else
            {
                builder.UseMaxSupportedYear();
            }
            return builder.BuildSegment();
        }

        /// <inheritdoc />
        public sealed override void ValidateYearMonth(int year, int month, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            PreValidator.ValidateMonth(year, month, paramName);

            // Small optimization: we first check "year".
            if (year == MinYear && new Yemo(year, month) < _minDateParts.Yemo)
            {
                Throw.MonthOutOfRange(month, paramName);
            }
        }

        /// <inheritdoc />
        public sealed override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            PreValidator.ValidateMonthDay(year, month, day, paramName);

            // Small optimization: we first check "year".
            if (year == MinYear)
            {
                // We check the month parts first even if it is not necessary.
                // Reason: identify the guilty part.
                var ymd = new Yemoda(year, month, day);
                if (ymd.Yemo < _minDateParts.Yemo)
                {
                    Throw.MonthOutOfRange(month, paramName);
                }
                else if (ymd < _minDateParts)
                {
                    Throw.DayOutOfRange(day, paramName);
                }
            }
        }

        /// <inheritdoc />
        public sealed override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
            {
                Throw.YearOutOfRange(year, paramName);
            }
            PreValidator.ValidateDayOfYear(year, dayOfYear, paramName);

            // Small optimization: we first check "year".
            if (year == MinYear && new Yedoy(year, dayOfYear) < _minOrdinalParts)
            {
                Throw.DayOfYearOutOfRange(dayOfYear, paramName);
            }
        }

        //
        // Partial validation helpers
        //

#if false // Partial validation helpers
        public void ValidateMonth(int y, int month, string? paramName = null)
        {
            Validator.ValidateMonth(y, month, paramName ?? nameof(month));

            // The first month must be handled with care.
            if (new Yemo(y, month) < FirstMonth)
            {
                Throw.OutOfRange(paramName ?? nameof(month));
            }
        }

        public void ValidateMonthDay(int y, int month, int day, string? paramName = null)
        {
            Validator.ValidateMonthDay(y, month, day, paramName);

            // The first month must be handled with care.
            if (y == MinYear)
            {
                var yemo = new Yemo(y, month);
                if (yemo < FirstMonth)
                {
                    Throw.MonthOutOfRange(month, paramName);
                }
                else if (yemo == FirstMonth && day < FirstDate.Day)
                {
                    Throw.DayOutOfRange(day, paramName);
                }
            }
        }

        //internal void ValidateDayOfMonth(int y, int m, int dayOfMonth, string? paramName = null)
        //{
        //    if (dayOfMonth < 1 || dayOfMonth > Schema.CountDaysInMonth(y, m))
        //    {
        //        Throw.OutOfRange(paramName ?? nameof(dayOfMonth));
        //    }

        //    // The first month must be handled with care.
        //    if (new Yemo(y, m) == FirstMonth && dayOfMonth < FirstDate.Day)
        //    {
        //        Throw.OutOfRange(paramName ?? nameof(dayOfMonth));
        //    }
        //}

        public void ValidateDayOfYear(int y, int dayOfYear, string? paramName = null)
        {
            Validator.ValidateDayOfYear(y, dayOfYear, paramName);

            // The first year must be handled with care.
            if (new Yedoy(y, dayOfYear) < FirstOrdinal)
            {
                Throw.DayOfYearOutOfRange(dayOfYear, paramName);
            }
        }
#endif
    }
}
