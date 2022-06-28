// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    /// <summary>
    /// Provides methods you can use to create new calendrical parts.
    /// <para>Like a schema, this class does not check the input parameters. It only checks that it
    /// can create a <see cref="Yemoda"/> or a <see cref="Yedoy"/> instance.</para>
    /// </summary>
    internal sealed partial class CalendricalPartsFactoryChecked : ICalendricalPartsFactory
    {
        /// <summary>
        /// Represents the schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalSchema _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendricalPartsFactoryChecked"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        public CalendricalPartsFactoryChecked(ICalendricalSchema schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }
    }

    internal partial class CalendricalPartsFactoryChecked // Conversions
    {
        /// <inheritdoc />
        [Pure]
        public Yemo GetMonthParts(int monthsSinceEpoch)
        {
            _schema.GetMonthParts(monthsSinceEpoch, out int y, out int m);
            return Yemo.Create(y, m);
        }

        /// <inheritdoc />
        [Pure]
        public Yemoda GetDateParts(int daysSinceEpoch)
        {
            _schema.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
            return Yemoda.Create(y, m, d);
        }

        /// <inheritdoc />
        [Pure]
        public Yedoy GetOrdinalParts(int daysSinceEpoch)
        {
            int y = _schema.GetYear(daysSinceEpoch, out int doy);
            return Yedoy.Create(y, doy);
        }

        /// <inheritdoc />
        [Pure]
        public Yedoy GetOrdinalParts(int y, int m, int d)
        {
            int doy = _schema.GetDayOfYear(y, m, d);
            return Yedoy.Create(y, doy);
        }

        /// <inheritdoc />
        [Pure]
        public Yemoda GetDateParts(int y, int doy)
        {
            int m = _schema.GetMonth(y, doy, out int d);
            return Yemoda.Create(y, m, d);
        }
    }

    internal partial class CalendricalPartsFactoryChecked // Dates in a given year or month
    {
        /// <inheritdoc />
        [Pure]
        public Yemo GetMonthPartsAtStartOfYear(int y)
        {
            if (y < Yemoda.MinYear || y > Yemoda.MaxYear) Throw.YearOutOfRange(y);
            return Yemo.AtStartOfYear(y);
        }

        /// <inheritdoc />
        [Pure]
        public Yemoda GetDatePartsAtStartOfYear(int y)
        {
            if (y < Yemoda.MinYear || y > Yemoda.MaxYear) Throw.YearOutOfRange(y);
            return Yemoda.AtStartOfYear(y);
        }

        /// <inheritdoc />
        [Pure]
        public Yedoy GetOrdinalPartsAtStartOfYear(int y)
        {
            if (y < Yemoda.MinYear || y > Yemoda.MaxYear) Throw.YearOutOfRange(y);
            return Yedoy.AtStartOfYear(y);
        }

        /// <inheritdoc />
        [Pure]
        public Yemo GetMonthPartsAtEndOfYear(int y)
        {
            int m = _schema.CountMonthsInYear(y);
            return Yemo.Create(y, m);
        }

        /// <inheritdoc />
        [Pure]
        public Yemoda GetDatePartsAtEndOfYear(int y)
        {
            int m = _schema.CountMonthsInYear(y);
            int d = _schema.CountDaysInMonth(y, m);
            return Yemoda.Create(y, m, d);
        }

        /// <inheritdoc />
        [Pure]
        public Yedoy GetOrdinalPartsAtEndOfYear(int y)
        {
            int doy = _schema.CountDaysInYear(y);
            return Yedoy.Create(y, doy);
        }

        /// <inheritdoc />
        [Pure]
        public Yemoda GetDatePartsAtStartOfMonth(int y, int m)
        {
            if (y < Yemoda.MinYear || y > Yemoda.MaxYear) Throw.YearOutOfRange(y);
            return Yemoda.AtStartOfMonth(y, m);
        }

        /// <inheritdoc />
        [Pure]
        public Yedoy GetOrdinalPartsAtStartOfMonth(int y, int m)
        {
            // Conversion (y, m, d) -> (y, doy)
            int doy = _schema.GetDayOfYear(y, m, 1);
            return Yedoy.Create(y, doy);
        }

        /// <inheritdoc />
        [Pure]
        public Yemoda GetDatePartsAtEndOfMonth(int y, int m)
        {
            int d = _schema.CountDaysInMonth(y, m);
            return Yemoda.Create(y, m, d);
        }

        /// <inheritdoc />
        [Pure]
        public Yedoy GetOrdinalPartsAtEndOfMonth(int y, int m)
        {
            int d = _schema.CountDaysInMonth(y, m);
            // Conversion (y, m, d) -> (y, doy)
            int doy = _schema.GetDayOfYear(y, m, d);
            return Yedoy.Create(y, doy);
        }
    }
}
