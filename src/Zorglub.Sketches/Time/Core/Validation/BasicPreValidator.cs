// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Validation
{
    #region Developer Notes

    // _schema is an ICalendrical instead of an ICalendricalSchema to be able to
    // check that we only rely on the core methods. The ctor could have required
    // only an ICalendrical object but it would have been idea. We don't want
    // for instance to initialize this class with a calendar (ICalendar), we
    // really want a schema. As explained in ICalendricalPreValidator, a
    // pre-validator is just a part of ICalendricalSchema that has been
    // extracted for technical reasons only.

    #endregion

    /// <summary>
    /// Provides a reference implementation for <see cref="ICalendricalPreValidator"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class BasicPreValidator : ICalendricalPreValidator
    {
        /// <summary>
        /// Represents the schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalKernel _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicPreValidator"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        public BasicPreValidator(ICalendricalKernel schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        /// <inheritdoc />
        public void ValidateMonth(int y, int month, string? paramName = null)
        {
            if (month < 1 || month > _schema.CountMonthsInYear(y))
            {
                Throw.MonthOutOfRange(month, paramName);
            }
        }

        /// <inheritdoc />
        public void ValidateMonthDay(int y, int month, int day, string? paramName = null)
        {
            if (month < 1 || month > _schema.CountMonthsInYear(y))
            {
                Throw.MonthOutOfRange(month, paramName);
            }
            if (day < 1 || day > _schema.CountDaysInMonth(y, month))
            {
                Throw.DayOutOfRange(day, paramName);
            }
        }

        /// <inheritdoc />
        public void ValidateDayOfYear(int y, int dayOfYear, string? paramName = null)
        {
            if (dayOfYear < 1 || dayOfYear > _schema.CountDaysInYear(y))
            {
                Throw.DayOfYearOutOfRange(dayOfYear, paramName);
            }
        }
    }
}
