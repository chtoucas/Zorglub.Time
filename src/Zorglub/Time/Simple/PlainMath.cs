// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Provides a plain implementation for <see cref="CalendarMath"/>.
    /// <para>This class uses the default <see cref="AdditionRuleset"/> to resolve ambiguities.</para>
    /// <para>In practice, we only use this mathematic with non-regular schema; see
    /// <see cref="CalendarMath.CreateDefault(Calendar)"/>.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed class PlainMath : CalendarMath
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlainMath"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        public PlainMath(Calendar calendar) : base(calendar, default) { }

        /// <inheritdoc />
        [Pure]
        protected internal override CalendarDate AddYearsCore(CalendarDate date, int years)
        {
            Debug.Assert(date.Cuid == Cuid);

            date.Parts.Unpack(out int y, out int m, out int d);
            y = checked(y + years);

            SupportedYears.Check(y);

            var sch = Schema;
            // NB: AdditionRule.Truncate.
            m = Math.Min(m, sch.CountMonthsInYear(y));
            d = Math.Min(d, sch.CountDaysInMonth(y, m));
            return new CalendarDate(new Yemoda(y, m, d), Cuid);
        }

        /// <inheritdoc />
        [Pure]
        protected internal override CalendarDate AddMonthsCore(CalendarDate date, int months)
        {
            Debug.Assert(date.Cuid == Cuid);

            var (y, m) = Arithmetic.AddMonths(date.Parts.Yemo, months);

            // NB: AdditionRule.Truncate.
            int d = Math.Min(date.Day, Schema.CountDaysInMonth(y, m));
            return new CalendarDate(new Yemoda(y, m, d), Cuid);
        }

        /// <inheritdoc />
        [Pure]
        protected internal override OrdinalDate AddYearsCore(OrdinalDate date, int years)
        {
            Debug.Assert(date.Cuid == Cuid);

            date.Parts.Unpack(out int y, out int doy);
            y = checked(y + years);

            SupportedYears.Check(y);

            // NB: AdditionRule.Truncate.
            doy = Math.Min(doy, Schema.CountDaysInYear(y));
            return new OrdinalDate(new Yedoy(y, doy), Cuid);
        }

        /// <inheritdoc />
        [Pure]
        protected internal override CalendarMonth AddYearsCore(CalendarMonth month, int years)
        {
            Debug.Assert(month.Cuid == Cuid);

            month.Parts.Unpack(out int y, out int m);
            y = checked(y + years);

            SupportedYears.Check(y);

            // NB: AdditionRule.Truncate.
            m = Math.Min(m, Schema.CountMonthsInYear(y));
            return new CalendarMonth(new Yemo(y, m), Cuid);
        }
    }
}
