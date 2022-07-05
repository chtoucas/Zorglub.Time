// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    /// <summary>
    /// Provides methods you can use to create new calendrical parts.
    /// <para>A factory for parts assumes that input parameters are valid for the underlying
    /// calendrical schema.</para>
    /// </summary>
    internal sealed partial class PartsProvider
    {
        /// <summary>
        /// Represents the schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalSchema _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartsProvider"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        public PartsProvider(ICalendricalSchema schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }
    }

    internal partial class PartsProvider // Conversions
    {
        /// <summary>
        /// Obtains the date parts for the specified month count (the number of consecutive months
        /// from the epoch to a date).
        /// </summary>
        [Pure]
        public MonthParts GetMonthParts(int monthsSinceEpoch)
        {
            _schema.GetMonthParts(monthsSinceEpoch, out int y, out int m);
            return new MonthParts(y, m);
        }

        /// <summary>
        /// Obtains the date parts for the specified day count (the number of consecutive days from
        /// the epoch to a date).
        /// </summary>
        [Pure]
        public DateParts GetDateParts(int daysSinceEpoch)
        {
            _schema.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
            return new DateParts(y, m, d);
        }

        /// <summary>
        /// Obtains the ordinal date parts for the specified day count (the number of consecutive
        /// days from the epoch to a date).
        /// </summary>
        [Pure]
        public OrdinalParts GetOrdinalParts(int daysSinceEpoch)
        {
            int y = _schema.GetYear(daysSinceEpoch, out int doy);
            return new OrdinalParts(y, doy);
        }

        /// <summary>
        /// Obtains the ordinal date parts for the specified date.
        /// </summary>
        [Pure]
        public OrdinalParts GetOrdinalParts(int y, int m, int d)
        {
            int doy = _schema.GetDayOfYear(y, m, d);
            return new OrdinalParts(y, doy);
        }

        /// <summary>
        /// Obtains the date parts for the specified ordinal date.
        /// </summary>
        [Pure]
        public DateParts GetDateParts(int y, int doy)
        {
            int m = _schema.GetMonth(y, doy, out int d);
            return new DateParts(y, m, d);
        }
    }

    internal partial class PartsProvider // Dates in a given year or month
    {
        /// <summary>
        /// Obtains the month parts for the first month of the specified year.
        /// </summary>
        [Pure]
        public static MonthParts GetMonthPartsAtStartOfYear(int y) => new(y, 1);

        /// <summary>
        /// Obtains the date parts for the first day of the specified year.
        /// </summary>
        [Pure]
        public static DateParts GetDatePartsAtStartOfYear(int y) => new(y, 1, 1);

        /// <summary>
        /// Obtains the ordinal date parts for the first day of the specified year.
        /// </summary>
        [Pure]
        public static OrdinalParts GetOrdinalPartsAtStartOfYear(int y) => new(y, 1);

        /// <summary>
        /// Obtains the date parts for the last month of the specified year.
        /// </summary>
        [Pure]
        public MonthParts GetMonthPartsAtEndOfYear(int y)
        {
            int m = _schema.CountMonthsInYear(y);
            return new MonthParts(y, m);
        }

        /// <summary>
        /// Obtains the date parts for the last day of the specified year.
        /// </summary>
        [Pure]
        public DateParts GetDatePartsAtEndOfYear(int y)
        {
            int m = _schema.CountMonthsInYear(y);
            int d = _schema.CountDaysInMonth(y, m);
            return new DateParts(y, m, d);
        }

        /// <summary>
        /// Obtains the ordinal date parts for the last day of the specified year.
        /// </summary>
        [Pure]
        public OrdinalParts GetOrdinalPartsAtEndOfYear(int y)
        {
            int doy = _schema.CountDaysInYear(y);
            return new OrdinalParts(y, doy);
        }

        /// <summary>
        /// Obtains the date parts for the first day of the specified month.
        /// </summary>
        [Pure]
        public static DateParts GetDatePartsAtStartOfMonth(int y, int m) => new(y, m, 1);

        /// <summary>
        /// Obtains the ordinal date parts for the first day of the specified month.
        /// </summary>
        [Pure]
        public OrdinalParts GetOrdinalPartsAtStartOfMonth(int y, int m)
        {
            int doy = _schema.GetDayOfYear(y, m, 1);
            return new OrdinalParts(y, doy);
        }

        /// <summary>
        /// Obtains the date parts for the last day of the specified month.
        /// </summary>
        [Pure]
        public DateParts GetDatePartsAtEndOfMonth(int y, int m)
        {
            int d = _schema.CountDaysInMonth(y, m);
            return new DateParts(y, m, d);
        }

        /// <summary>
        /// Obtains the ordinal date parts for the last day of the specified month.
        /// </summary>
        [Pure]
        public OrdinalParts GetOrdinalPartsAtEndOfMonth(int y, int m)
        {
            int d = _schema.CountDaysInMonth(y, m);
            int doy = _schema.GetDayOfYear(y, m, d);
            return new OrdinalParts(y, doy);
        }
    }
}
