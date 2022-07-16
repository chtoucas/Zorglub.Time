// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Represents a calendar with dates within a range of years.
    /// </summary>
    public partial class MinMaxYearCalendar : NakedCalendar<MinMaxYearScope>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="AoorException"><paramref name="minYear"/> or <paramref name="maxYear"/>
        /// is outside the range of supported years by <paramref name="schema"/> or
        /// <see cref="Yemoda"/>.</exception>
        public MinMaxYearCalendar(
            string name,
            ICalendricalSchema schema,
            DayNumber epoch,
            int minYear,
            int maxYear)
            : this(name, new MinMaxYearScope(schema, epoch, minYear, maxYear)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxYearCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        public MinMaxYearCalendar(string name, MinMaxYearScope scope) : base(name, scope)
        {
            DayProvider = new MinMaxYearDayProvider(scope);
        }

        /// <summary>
        /// Gets a provider for day numbers in a year or a month.
        /// </summary>
        public IDayProvider<DayNumber> DayProvider { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="MinMaxYearCalendar"/> class with dates on or
        /// after the specified year.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="AoorException"><paramref name="minYear"/> is outside the range of
        /// supported years by <paramref name="schema"/> or
        /// <see cref="Yemoda"/>.</exception>
        [Pure]
        public static MinMaxYearCalendar WithMinYear(
            string name, ICalendricalSchema schema, DayNumber epoch, int minYear)
        {
            Requires.NotNull(schema);

            return new MinMaxYearCalendar(name, schema, epoch, minYear, schema.SupportedYears.Max);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MinMaxYearCalendar"/> class with dates on or
        /// before the specified year.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="AoorException"><paramref name="maxYear"/> is outside the range of
        /// supported years by <paramref name="schema"/> or <see cref="Yemoda"/>.</exception>
        [Pure]
        public static MinMaxYearCalendar WithMaxYear(
            string name, ICalendricalSchema schema, DayNumber epoch, int maxYear)
        {
            Requires.NotNull(schema);

            return new MinMaxYearCalendar(name, schema, epoch, schema.SupportedYears.Min, maxYear);
        }
    }

    public partial class MinMaxYearCalendar // Year, month, day infos
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int CountMonthsInYear(int year)
        {
            SupportedYears.Validate(year);
            return Schema.CountMonthsInYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInYear(int year)
        {
            SupportedYears.Validate(year);
            return Schema.CountDaysInYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return Schema.CountDaysInMonth(year, month);
        }
    }

    public partial class MinMaxYearCalendar // Dates in a given year or month
    {
        /// <inheritdoc />
        [Pure]
        public sealed override IEnumerable<DateParts> GetDaysInYear(int year)
        {
            // Check arg eagerly.
            SupportedYears.Validate(year);

            return Iterator();

            IEnumerable<DateParts> Iterator()
            {
                int monthsInYear = Schema.CountMonthsInYear(year);

                for (int m = 1; m <= monthsInYear; m++)
                {
                    int daysInMonth = Schema.CountDaysInMonth(year, m);

                    for (int d = 1; d <= daysInMonth; d++)
                    {
                        yield return new DateParts(year, m, d);
                    }
                }
            }
        }

        /// <inheritdoc />
        [Pure]
        public sealed override IEnumerable<DateParts> GetDaysInMonth(int year, int month)
        {
            // Check arg eagerly.
            Scope.ValidateYearMonth(year, month);

            return Iterator();

            IEnumerable<DateParts> Iterator()
            {
                int daysInMonth = Schema.CountDaysInMonth(year, month);

                for (int d = 1; d <= daysInMonth; d++)
                {
                    yield return new DateParts(year, month, d);
                }
            }
        }

        /// <inheritdoc />
        [Pure]
        public sealed override DateParts GetStartOfYear(int year)
        {
            SupportedYears.Validate(year);
            return DateParts.AtStartOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override DateParts GetEndOfYear(int year)
        {
            SupportedYears.Validate(year);
            return PartsAdapter.GetDatePartsAtEndOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override DateParts GetStartOfMonth(int year, int month)
        {
            Scope.ValidateYearMonth(year, month);
            return DateParts.AtStartOfMonth(year, month);
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
