// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Defines the mathematical operations suitable for use by regular calendars and provides a
    /// base for derived classes.
    /// <para>This class uses the default <see cref="AdditionRules"/> to resolve ambiguities.</para>
    /// </summary>
    internal sealed class RegularMath : CalendarMath
    {
        /// <summary>
        /// Represents the total number of months in a year.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _monthsInYear;

        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="RegularMath"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="calendar"/> is not regular.
        /// </exception>
        public RegularMath(Calendar calendar)
            : base(
                  calendar,
                  // Regular calendar => month addition is NOT ambiguous.
                  // The result is exact and matches the last month of the year,
                  // therefore we could specify either MonthAdditionRule.Exact
                  // or MonthAdditionRule.EndOfYear. I prefer the later to
                  // emphasize that this math engine uses the default rules.
                  default)
        {
            Debug.Assert(calendar != null);
            if (calendar.IsRegular(out int monthsInYear) == false) Throw.Argument(nameof(calendar));

            _monthsInYear = monthsInYear;
        }

        /// <inheritdoc />
        [Pure]
        protected internal sealed override CalendarDate AddYearsCore(CalendarDate date, int years)
        {
            Debug.Assert(date.Cuid == Cuid);

            date.Parts.Unpack(out int y, out int m, out int d);
            y = checked(y + years);

            YearOverflowChecker.Check(y);

            // NB: DateAdditionRule.EndOfMonth.
            d = Math.Min(d, Schema.CountDaysInMonth(y, m));
            return new CalendarDate(new Yemoda(y, m, d), Cuid);
        }

        /// <inheritdoc />
        [Pure]
        protected internal sealed override CalendarDate AddMonthsCore(CalendarDate date, int months)
        {
            Debug.Assert(date.Cuid == Cuid);

            // We could have used Schema.Arithmetic.AddMonths() as in PlainMath,
            // but here we avoid the double validation by copying its code.
            date.Parts.Unpack(out int y, out int m, out int d);
            m = 1 + MathZ.Modulo(checked(m - 1 + months), _monthsInYear, out int y0);
            y += y0;

            YearOverflowChecker.Check(y);

            // NB: DateAdditionRule.EndOfMonth.
            d = Math.Min(d, Schema.CountDaysInMonth(y, m));
            return new CalendarDate(new Yemoda(y, m, d), Cuid);
        }

        /// <inheritdoc />
        [Pure]
        protected internal sealed override OrdinalDate AddYearsCore(OrdinalDate date, int years)
        {
            Debug.Assert(date.Cuid == Cuid);

            date.Parts.Unpack(out int y, out int doy);
            y = checked(y + years);

            YearOverflowChecker.Check(y);

            // NB: OrdinalAdditionRule.EndOfYear.
            doy = Math.Min(doy, Schema.CountDaysInYear(y));
            return new OrdinalDate(new Yedoy(y, doy), Cuid);
        }

        /// <inheritdoc />
        [Pure]
        protected internal sealed override CalendarMonth AddYearsCore(CalendarMonth month, int years)
        {
            Debug.Assert(month.Cuid == Cuid);

            month.Parts.Unpack(out int y, out int m);
            y = checked(y + years);

            YearOverflowChecker.Check(y);

            // NB: MonthAdditionRule.EndOfYear.
            // The operation is always exact, and it's compatible with "EndOfYear".
            return new CalendarMonth(new Yemo(y, m), Cuid);
        }
    }
}
