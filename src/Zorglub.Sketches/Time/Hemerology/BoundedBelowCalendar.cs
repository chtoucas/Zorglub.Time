// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology.Scopes;

    // FIXME(code): we no longer require that minDate != startOfYear and
    // maxDate != endOfYear when building a scope.
    // Without that the behaviour of BoundedBelowCalendar.GetStartOfYear()
    // is broken.

    /// <summary>
    /// Represents a calendar with dates on or after a given date.
    /// <para>The aforementioned date can NOT be the start of a year.</para>
    /// </summary>
    public partial class BoundedBelowCalendar : NakedCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedBelowCalendar"/>
        /// class.
        /// </summary>
        public BoundedBelowCalendar(
            string name,
            ICalendricalSchema schema,
            DayNumber epoch,
            int year,
            int month,
            int day,
            int? maxYear = null)
            : this(
                name,
                schema,
                new BoundedBelowScope(schema, epoch, year, month, day, maxYear))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedBelowCalendar"/>
        /// class.
        /// </summary>
        public BoundedBelowCalendar(string name, ICalendricalSchema schema, BoundedBelowScope scope)
            : base(name, schema, scope)
        {
            DayProvider = new BoundedBelowDayProvider(scope);

            (MinYear, MaxYear) = scope.SupportedYears.Endpoints;

            var seg = scope.Segment;
            MinDateParts = seg.MinMaxDateParts.LowerValue;
            MinOrdinalParts = seg.MinMaxOrdinalParts.LowerValue;
        }

        /// <summary>
        /// Gets a provider for day numbers in a year or a month.
        /// </summary>
        public IDayProvider<DayNumber> DayProvider { get; }

        /// <summary>
        /// Gets the earliest supported year.
        /// </summary>
        protected int MinYear { get; }

        /// <summary>
        /// Gets the latest supported year.
        /// </summary>
        protected int MaxYear { get; }

        /// <summary>
        /// Gets the earliest supported "date".
        /// </summary>
        protected DateParts MinDateParts { get; }

        /// <summary>
        /// Gets the earliest supported ordinal "date".
        /// </summary>
        protected OrdinalParts MinOrdinalParts { get; }
    }

    public partial class BoundedBelowCalendar // Year, month, day infos
    {
        // NB : pour opimtiser les choses on pourrait traiter d'abord le cas
        // limite (première année ou premier mois) puis le cas général.
        // Attention, il ne faudrait alors pas écrire
        // > if (new Yemo(year, month) == MinYemoda.Yemo) { ... }
        // mais plutôt
        // > if (year == MinYear && month == MinYemoda.Month) { ... }
        // car on n'a justement pas validé les paramètres.

        /// <inheritdoc />
        [Pure]
        public sealed override int CountMonthsInYear(int year)
        {
            Scope.ValidateYear(year);
            return year == MinYear
                ? CountMonthsInFirstYear()
                : Schema.CountMonthsInYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInYear(int year)
        {
            Scope.ValidateYear(year);
            return year == MinYear
                ? CountDaysInFirstYear()
                : Schema.CountDaysInYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return new MonthParts(year, month) == MinDateParts.MonthParts
                ? CountDaysInFirstMonth()
                : Schema.CountDaysInMonth(year, month);
        }

        /// <summary>
        /// Obtains the number of months in the first supported year.
        /// </summary>
        [Pure]
        public int CountMonthsInFirstYear() =>
            Schema.CountMonthsInYear(MinYear) - MinDateParts.Month + 1;

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
    }

    public partial class BoundedBelowCalendar // Dates in a given year or month
    {
        /// <inheritdoc />
        [Pure]
        public sealed override IEnumerable<DateParts> GetDaysInYear(int year)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [Pure]
        public sealed override IEnumerable<DateParts> GetDaysInMonth(int year, int month)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [Pure]
        public sealed override DateParts GetStartOfYear(int year)
        {
            Scope.ValidateYear(year);
            return year == MinYear
                ? Throw.ArgumentOutOfRange<DateParts>(nameof(year))
                : PartsAdapter.GetDatePartsAtStartOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override DateParts GetEndOfYear(int year)
        {
            Scope.ValidateYear(year);
            return PartsAdapter.GetDatePartsAtEndOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override DateParts GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return new MonthParts(year, month) == MinDateParts.MonthParts
                ? Throw.ArgumentOutOfRange<DateParts>(nameof(month))
                : PartsAdapter.GetDatePartsAtStartOfMonth(year, month);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override DateParts GetEndOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return PartsAdapter.GetDatePartsAtEndOfMonth(year, month);
        }
    }
}
