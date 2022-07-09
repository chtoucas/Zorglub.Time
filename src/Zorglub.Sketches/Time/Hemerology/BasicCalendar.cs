// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Hemerology.Scopes;

    // REVIEW(api): use ICalendarScope instead of CalendarScope, but then we
    // cannot use scope.Schema.

    /// <summary>
    /// Represents a basic calendar and provides a base for derived classes.
    /// <para>This class only supports subintervals of <see cref="Yemoda.SupportedYears"/>.</para>
    /// </summary>
    public abstract partial class BasicCalendar : ICalendar
    {
        /// <summary>
        /// Called from constructors in derived classes to initialize the <see cref="BasicCalendar"/>
        /// class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        protected BasicCalendar(CalendarScope scope)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));

            Schema = scope.Schema;
            Epoch = scope.Epoch;
            SupportedYears = scope.SupportedYears;
            Domain = scope.Domain;
            PreValidator = Schema.PreValidator;
        }

        /// <inheritdoc />
        public DayNumber Epoch { get; }

        /// <inheritdoc />
        public CalendricalAlgorithm Algorithm => Schema.Algorithm;

        /// <inheritdoc />
        public CalendricalFamily Family => Schema.Family;

        /// <inheritdoc />
        public CalendricalAdjustments PeriodicAdjustments => Schema.PeriodicAdjustments;

        /// <inheritdoc />
        public Range<int> SupportedYears { get; }

        /// <inheritdoc />
        public Range<DayNumber> Domain { get; }

        /// <inheritdoc />
        public ICalendarScope Scope { get; }

        /// <summary>
        /// Gets the underlying calendrical schema.
        /// </summary>
        protected internal ICalendricalSchema Schema { get; }

        /// <summary>
        /// Gets the pre-validator.
        /// </summary>
        protected internal ICalendricalPreValidator PreValidator { get; }

        /// <inheritdoc />
        [Pure]
        public bool IsRegular(out int monthsInYear) => Schema.IsRegular(out monthsInYear);
    }

    public partial class BasicCalendar // Year, month, day infos
    {
#pragma warning disable CA1725 // Parameter names should match base declaration (Naming)

        /// <inheritdoc />
        /// <exception cref="AoorException">The year is outside the range of supported years.
        /// </exception>
        [Pure]
        public bool IsLeapYear(int year)
        {
            Scope.ValidateYear(year);
            return Schema.IsLeapYear(year);
        }

        /// <inheritdoc />
        /// <exception cref="AoorException">The month is either invalid or outside the range of
        /// supported months.</exception>
        [Pure]
        public bool IsIntercalaryMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return Schema.IsIntercalaryMonth(year, month);
        }

        /// <inheritdoc />
        /// <exception cref="AoorException">The date is either invalid or outside the range of
        /// supported dates.</exception>
        [Pure]
        public bool IsIntercalaryDay(int year, int month, int day)
        {
            Scope.ValidateYearMonthDay(year, month, day);
            return Schema.IsIntercalaryDay(year, month, day);
        }

        /// <inheritdoc />
        /// <exception cref="AoorException">The date is either invalid or outside the range of
        /// supported dates.</exception>
        [Pure]
        public bool IsSupplementaryDay(int year, int month, int day)
        {
            Scope.ValidateYearMonthDay(year, month, day);
            return Schema.IsSupplementaryDay(year, month, day);
        }

        /// <inheritdoc />
        /// <exception cref="AoorException">The year is outside the range of supported years.
        /// </exception>
        [Pure] public abstract int CountMonthsInYear(int year);

        /// <inheritdoc />
        /// <exception cref="AoorException">The year is outside the range of supported years.
        /// </exception>
        [Pure] public abstract int CountDaysInYear(int year);

        /// <inheritdoc />
        /// <exception cref="AoorException">The month is either invalid or outside the range of
        /// supported months.</exception>
        [Pure] public abstract int CountDaysInMonth(int year, int month);

#pragma warning restore CA1725 // Parameter names should match base declaration
    }

    public partial class BasicCalendar // Conversions
    {
        /// <inheritdoc />
        [Pure]
        public DayNumber GetDayNumberOn(int year, int month, int day)
        {
            Scope.ValidateYearMonthDay(year, month, day);
            return Epoch + Schema.CountDaysSinceEpoch(year, month, day);
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetDayNumberOn(int year, int dayOfYear)
        {
            Scope.ValidateOrdinal(year, dayOfYear);
            return Epoch + Schema.CountDaysSinceEpoch(year, dayOfYear);
        }
    }
}
