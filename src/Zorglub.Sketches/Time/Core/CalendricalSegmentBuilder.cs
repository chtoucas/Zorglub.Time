// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;

    // We validate before and after calling a method from the method:
    // - before, to respect the schema layout
    //   eg _minYear <= year <= _maxYear
    // - after, to stay within the limits of Yemoda/Yedoy
    //   via ICalendricalSchemaPlus.Promote(schema, checked: true)

    /// <summary>
    /// Represents a builder for <see cref="CalendricalSegment"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed partial class CalendricalSegmentBuilder
    {
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
        /// Represents the earliest supported year >= 1.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int? _minYearOnOrAfterYear1;

        /// <summary>
        /// Represents the earliest supported year.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _minYear;

        /// <summary>
        /// Represents the latest supported year.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _maxYear;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendricalSegmentBuilder"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> and <see cref="Yemoda.SupportedYears"/> are disjoint.
        /// </exception>
        public CalendricalSegmentBuilder(ICalendricalSchema schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
            _partsFactory = ICalendricalPartsFactory.Create(schema, @checked: true);

            // Normalement, un ICalendricalSchemaPlus satisfait la condition
            // schema.MinMaxYear ⊆ Yemoda.SupportedYears, néanmoins rien ne nous
            // permet d'affirmer que c'est réellement le cas.
            var set = Interval.Intersect(schema.SupportedYears, Yemoda.SupportedYears);
            if (set.IsEmpty) Throw.Argument(nameof(schema));
            var range = set.Range.Value;

            (_minYear, _maxYear) = range.Endpoints;

            var set1 = Interval.Intersect(range, Range.StartingAt(1));
            _minYearOnOrAfterYear1 = set1.IsEmpty ? null : set1.Range.Value.Min;
        }

        /// <summary>
        /// Returns true if the start of the segment has been set; otherwise returns false.
        /// </summary>
        public bool HasStart => _start != null;

        /// <summary>
        /// Returns true if the end of the segment has been set; otherwise returns false.
        /// </summary>
        public bool HasEnd => _end != null;

        /// <summary>
        /// Returns true if both start and end have been set; otherwise returns false.
        /// </summary>
        public bool IsBuildable => HasStart && HasEnd;

        private CalendricalEndpoint? _start;
        /// <summary>
        /// Gets the start of the segment.
        /// </summary>
        internal CalendricalEndpoint Start
        {
            get => _start ?? Throw.InvalidOperation<CalendricalEndpoint>();
            private set
            {
                if (value > _end) Throw.ArgumentOutOfRange(nameof(value));
                _start = value;
            }
        }

        private CalendricalEndpoint? _end;
        /// <summary>
        /// Gets the end of the segment.
        /// </summary>
        internal CalendricalEndpoint End
        {
            get => _end ?? Throw.InvalidOperation<CalendricalEndpoint>();
            private set
            {
                if (value < _start) Throw.ArgumentOutOfRange(nameof(value));
                _end = value;
            }
        }

        private int? _minDaysSinceEpoch;
        /// <summary>
        /// Gets the minimum possible value for the number of consecutive days from the epoch.
        /// </summary>
        private int MinDaysSinceEpoch => _minDaysSinceEpoch ??= _schema.GetStartOfYear(_minYear);

        private int? _maxDaysSinceEpoch;
        /// <summary>
        /// Gets the maximum possible value for the number of consecutive days from the epoch.
        /// </summary>
        private int MaxDaysSinceEpoch => _maxDaysSinceEpoch ??= _schema.GetEndOfYear(_maxYear);

        /// <summary>
        /// Obtains the segment.
        /// </summary>
        [Pure]
        [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "<Pending>")]
        public CalendricalSegment GetSegment() => new(_schema, Start, End);
    }

    public partial class CalendricalSegmentBuilder // Builder methods
    {
        /// <summary>
        /// Sets the start of the segment to the specified number of consecutive days from the epoch.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        public void SetMinDaysSinceEpoch(int daysSinceEpoch)
        {
            if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            {
                Throw.ArgumentOutOfRange(nameof(daysSinceEpoch));
            }

            Start = new CalendricalEndpoint
            {
                DaysSinceEpoch = daysSinceEpoch,
                DateParts = _partsFactory.GetDateParts(daysSinceEpoch),
                OrdinalParts = _partsFactory.GetOrdinalParts(daysSinceEpoch)
            };
        }

        /// <summary>
        /// Sets the end of the segment to the specified number of consecutive days from the epoch.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        public void SetMaxDaysSinceEpoch(int daysSinceEpoch)
        {
            if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            {
                Throw.ArgumentOutOfRange(nameof(daysSinceEpoch));
            }

            End = new CalendricalEndpoint
            {
                DaysSinceEpoch = daysSinceEpoch,
                DateParts = _partsFactory.GetDateParts(daysSinceEpoch),
                OrdinalParts = _partsFactory.GetOrdinalParts(daysSinceEpoch)
            };
        }

        /// <summary>
        /// Sets the start of the segment to the specified date.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        public void SetMinDate(Yemoda ymd)
        {
            var (y, m, d) = ymd;

            if (y < _minYear || y > _maxYear) Throw.YearOutOfRange(y, nameof(ymd));
            _schema.PreValidator.ValidateMonthDay(y, m, d, nameof(ymd));

            Start = new CalendricalEndpoint
            {
                DaysSinceEpoch = _schema.CountDaysSinceEpoch(y, m, d),
                DateParts = ymd,
                OrdinalParts = _partsFactory.GetOrdinalParts(y, m, d),
            };
        }

        /// <summary>
        /// Sets the end of the segment to the specified date.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        public void SetMaxDate(Yemoda ymd)
        {
            var (y, m, d) = ymd;

            if (y < _minYear || y > _maxYear) Throw.YearOutOfRange(y, nameof(ymd));
            _schema.PreValidator.ValidateMonthDay(y, m, d, nameof(ymd));

            End = new CalendricalEndpoint
            {
                DaysSinceEpoch = _schema.CountDaysSinceEpoch(y, m, d),
                DateParts = ymd,
                OrdinalParts = _partsFactory.GetOrdinalParts(y, m, d),
            };
        }

        /// <summary>
        /// Sets the start of the segment to the specified ordinal date.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        public void SetMinOrdinal(Yedoy ydoy)
        {
            var (y, doy) = ydoy;

            if (y < _minYear || y > _maxYear) Throw.YearOutOfRange(y, nameof(ydoy));
            _schema.PreValidator.ValidateDayOfYear(y, doy, nameof(ydoy));

            Start = new CalendricalEndpoint
            {
                DaysSinceEpoch = _schema.CountDaysSinceEpoch(y, doy),
                DateParts = _partsFactory.GetDateParts(y, doy),
                OrdinalParts = ydoy,
            };
        }

        /// <summary>
        /// Sets the end of the segment to the specified ordinal date.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        public void SetMaxOrdinal(Yedoy ydoy)
        {
            var (y, doy) = ydoy;

            if (y < _minYear || y > _maxYear) Throw.YearOutOfRange(y, nameof(ydoy));
            _schema.PreValidator.ValidateDayOfYear(y, doy, nameof(ydoy));

            End = new CalendricalEndpoint
            {
                DaysSinceEpoch = _schema.CountDaysSinceEpoch(y, doy),
                DateParts = _partsFactory.GetDateParts(y, doy),
                OrdinalParts = ydoy,
            };
        }

        /// <summary>
        /// Sets the start of the segment to the start of the specified year.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        public void SetMinYear(int year)
        {
            if (year < _minYear || year > _maxYear) Throw.YearOutOfRange(year);

            Start = new CalendricalEndpoint
            {
                DaysSinceEpoch = _schema.GetStartOfYear(year),
                DateParts = _partsFactory.GetStartOfYearParts(year),
                OrdinalParts = _partsFactory.GetStartOfYearOrdinalParts(year),
            };
        }

        /// <summary>
        /// Sets the start of the segment to the start of the earliest supported year.
        /// </summary>
        public void SetMinSupportedYear(bool onOrAfterEpoch)
        {
            int minYear =
                onOrAfterEpoch
                ? _minYearOnOrAfterYear1 ?? Throw.InvalidOperation<int>()
                : _minYear;
            SetMinYear(minYear);
        }

        /// <summary>
        /// Sets the end of the segment to the end of the specified year.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        public void SetMaxYear(int year)
        {
            if (year < _minYear || year > _maxYear) Throw.YearOutOfRange(year);

            End = new CalendricalEndpoint
            {
                DaysSinceEpoch = _schema.GetEndOfYear(year),
                DateParts = _partsFactory.GetEndOfYearParts(year),
                OrdinalParts = _partsFactory.GetEndOfYearOrdinalParts(year),
            };
        }

        /// <summary>
        /// Sets the end of the segment to the end of the latest supported year.
        /// </summary>
        public void SetMaxSupportedYear() => SetMaxYear(_maxYear);
    }
}
