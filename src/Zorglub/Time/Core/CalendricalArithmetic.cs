// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Provides a plain implementation for <see cref="ICalendricalArithmetic"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed partial class CalendricalArithmetic : ICalendricalArithmetic
    {
        /// <summary>
        /// Represents the earliest month number.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _minMonthsSinceEpoch;

        /// <summary>
        /// Represents the latest month number.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _maxMonthsSinceEpoch;

        /// <summary>
        /// Represents the earliest day number.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _minDaysSinceEpoch;

        /// <summary>
        /// Represents the latest day number.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _maxDaysSinceEpoch;

        /// <summary>
        /// Represents the schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalSchema _schema;

        /// <summary>
        /// Represents the factory for calendrical parts.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalPartsFactory _partsFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendricalArithmetic"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> and <see cref="Yemoda"/> are disjoint.
        /// </exception>
        public CalendricalArithmetic(ICalendricalSchema schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
            _partsFactory = ICalendricalPartsFactory.Create(schema, @checked: false);

            var segment = CalendricalSegment.CreateMaximal(schema, onOrAfterEpoch: false);
            SupportedYears = Range.FromEndpoints(segment.MinMaxYear);
            (_minDaysSinceEpoch, _maxDaysSinceEpoch) = segment.MinMaxDaysSinceEpoch;
            (_minMonthsSinceEpoch, _maxMonthsSinceEpoch) = segment.MinMaxMonthsSinceEpoch;
        }

        /// <inheritdoc/>
        public Range<int> SupportedYears { get; }
    }

    public partial class CalendricalArithmetic // Operations on Yemoda
    {
        /// <inheritdoc />
        [Pure]
        public Yemoda AddDays(Yemoda ymd, int days)
        {
            ymd.Unpack(out int y, out int m, out int d);

            int daysSinceEpoch = checked(_schema.CountDaysSinceEpoch(y, m, d) + days);
            if (daysSinceEpoch < _minDaysSinceEpoch || daysSinceEpoch > _maxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return _partsFactory.GetDateParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public Yemoda NextDay(Yemoda ymd) => AddDays(ymd, 1);

        /// <inheritdoc />
        [Pure]
        public Yemoda PreviousDay(Yemoda ymd) => AddDays(ymd, -1);

        /// <inheritdoc />
        [Pure]
        public int CountDaysBetween(Yemoda start, Yemoda end)
        {
            start.Unpack(out int y0, out int m0, out int d0);
            end.Unpack(out int y1, out int m1, out int d1);

            return _schema.CountDaysSinceEpoch(y1, m1, d1) - _schema.CountDaysSinceEpoch(y0, m0, d0);
        }
    }

    public partial class CalendricalArithmetic // Operations on Yedoy
    {
        /// <inheritdoc />
        [Pure]
        public Yedoy AddDays(Yedoy ydoy, int days)
        {
            ydoy.Unpack(out int y, out int doy);

            int daysSinceEpoch = checked(_schema.CountDaysSinceEpoch(y, doy) + days);
            if (daysSinceEpoch < _minDaysSinceEpoch || daysSinceEpoch > _maxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return _partsFactory.GetOrdinalParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public Yedoy NextDay(Yedoy ydoy) => AddDays(ydoy, 1);

        /// <inheritdoc />
        [Pure]
        public Yedoy PreviousDay(Yedoy ydoy) => AddDays(ydoy, -1);

        /// <inheritdoc />
        [Pure]
        public int CountDaysBetween(Yedoy start, Yedoy end)
        {
            start.Unpack(out int y0, out int doy0);
            end.Unpack(out int y1, out int doy1);

            return _schema.CountDaysSinceEpoch(y1, doy1) - _schema.CountDaysSinceEpoch(y0, doy0);
        }
    }

    public partial class CalendricalArithmetic // Operations on Yemo
    {
        /// <inheritdoc />
        [Pure]
        public Yemo AddMonths(Yemo ym, int months)
        {
            ym.Unpack(out int y, out int m);

            int monthsSinceEpoch = checked(_schema.CountMonthsSinceEpoch(y, m) + months);
            if (monthsSinceEpoch < _minMonthsSinceEpoch || monthsSinceEpoch > _maxMonthsSinceEpoch)
            {
                Throw.MonthOverflow();
            }

            return _partsFactory.GetMonthParts(monthsSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public Yemo NextMonth(Yemo ym) => AddMonths(ym, 1);

        /// <inheritdoc />
        [Pure]
        public Yemo PreviousMonth(Yemo ym) => AddMonths(ym, -1);

        /// <inheritdoc />
        [Pure]
        public int CountMonthsBetween(Yemo start, Yemo end)
        {
            start.Unpack(out int y0, out int m0);
            end.Unpack(out int y1, out int m1);

            return _schema.CountMonthsSinceEpoch(y1, m1) - _schema.CountMonthsSinceEpoch(y0, m0);
        }
    }
}
