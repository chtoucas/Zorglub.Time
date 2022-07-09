// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    /// <summary>
    /// Provides methods you can use to create new calendrical parts.
    /// <para>This class assumes that input parameters are valid for the underlying calendrical
    /// schema.</para>
    /// </summary>
    public sealed partial class PartsAdapter
    {
        /// <summary>
        /// Represents the schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalSchema _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartsAdapter"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        public PartsAdapter(ICalendricalSchema schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }
    }

    public partial class PartsAdapter // Conversions
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

    public partial class PartsAdapter // Dates in a given year or month
    {
#if DEBUG
        // Contrary to SystemSchema, the "box principle" does not apply here,
        // therefore some methods may be static, nervetheless I prefer to
        // exclude them from the API as they are just aliases of other methods.

        /// <summary>
        /// Obtains the month parts for the first month of the specified year.
        /// </summary>
        [Pure, ExcludeFromCodeCoverage]
        public static MonthParts GetMonthPartsAtStartOfYear(int y) => MonthParts.AtStartOfYear(y);

        /// <summary>
        /// Obtains the date parts for the first day of the specified year.
        /// </summary>
        [Pure, ExcludeFromCodeCoverage]
        public static DateParts GetDatePartsAtStartOfYear(int y) => DateParts.AtStartOfYear(y);

        /// <summary>
        /// Obtains the ordinal date parts for the first day of the specified year.
        /// </summary>
        [Pure, ExcludeFromCodeCoverage]
        public static OrdinalParts GetOrdinalPartsAtStartOfYear(int y) =>
            OrdinalParts.AtStartOfYear(y);
#endif

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

#if DEBUG
        /// <summary>
        /// Obtains the date parts for the first day of the specified month.
        /// </summary>
        [Pure, ExcludeFromCodeCoverage]
        public static DateParts GetDatePartsAtStartOfMonth(int y, int m) =>
            DateParts.AtStartOfMonth(y, m);
#endif

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
