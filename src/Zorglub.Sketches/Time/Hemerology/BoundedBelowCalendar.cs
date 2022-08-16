// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology.Scopes;

    // FIXME(code): we no longer require that minDate != startOfYear and
    // maxDate != endOfYear when building a scope.
    // Without that the behaviour of BoundedBelowNakedCalendar.GetStartOfYear()
    // is broken.

    /// <summary>
    /// Represents a calendar with dates on or after a given date.
    /// <para>The aforementioned date can NOT be the start of a year.</para>
    /// </summary>
    public class BoundedBelowCalendar : BasicCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedBelowCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public BoundedBelowCalendar(string name, BoundedBelowScope scope) : base(name, scope)
        {
            Debug.Assert(scope != null);

            MinYear = scope.MinYear;
            MinDateParts = scope.MinDateParts;
            MinOrdinalParts = scope.MinOrdinalParts;
            MinMonthParts = scope.MinMonthParts;
        }

        /// <summary>
        /// Gets the earliest supported year.
        /// </summary>
        public int MinYear { get; }

        /// <summary>
        /// Gets the earliest supported month parts.
        /// </summary>
        public MonthParts MinMonthParts { get; }

        /// <summary>
        /// Gets the earliest supported date parts.
        /// </summary>
        public DateParts MinDateParts { get; }

        /// <summary>
        /// Gets the earliest supported ordinal date parts.
        /// </summary>
        public OrdinalParts MinOrdinalParts { get; }

        // NB : pour optimiser les choses on pourrait traiter d'abord le cas
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
            YearsValidator.Validate(year);
            return year == MinYear
                ? CountMonthsInFirstYear()
                : Schema.CountMonthsInYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInYear(int year)
        {
            YearsValidator.Validate(year);
            return year == MinYear
                ? CountDaysInFirstYear()
                : Schema.CountDaysInYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return new MonthParts(year, month) == MinMonthParts
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
}
