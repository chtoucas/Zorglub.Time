// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Represents the mathematical operations suitable for use by calendars with a fixed number of
    /// months per year.
    /// <para>This class uses the default <see cref="AdditionRules"/> to resolve ambiguities.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed class RegularMath : RegularMathBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegularMath"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="calendar"/> is not regular.
        /// </exception>
        public RegularMath(Calendar calendar) : base(calendar) { }

        /// <inheritdoc />
        [Pure]
        protected internal override CalendarDate AddMonthsCore(CalendarDate date, int months)
        {
            Debug.Assert(date.Cuid == Cuid);

            date.Parts.Unpack(out int y, out int m, out int d);
            m = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsInYear, out int y0);
            y += y0;

            YearOverflowChecker.Check(y);

            // NB: DateAdditionRule.EndOfMonth.
            d = Math.Min(d, Schema.CountDaysInMonth(y, m));
            return new CalendarDate(new Yemoda(y, m, d), Cuid);
        }
    }
}
