// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;

    using Endpoint = CalendricalSegment.Endpoint;

    // We validate before and after calling a method from the method:
    // - before, to respect the schema layout (_supportedYears)
    // - after, to stay within the limits of Yemoda/Yedoy (_partsFactory)

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
        /// Represents the adapter for calendrical parts.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly PartsAdapter _partsAdapter;

        /// <summary>
        /// Represents the earliest supported year &gt;= 1.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int? _minYearOnOrAfterYear1;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendricalSegmentBuilder"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        public CalendricalSegmentBuilder(ICalendricalSchema schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));

            _partsAdapter = new PartsAdapter(schema);

            var set = Interval.Intersect(schema.SupportedYears, Range.StartingAt(1));
            _minYearOnOrAfterYear1 = set.IsEmpty ? null : set.Range.Min;
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

        private Endpoint? _start;
        /// <summary>
        /// Gets the start of the segment.
        /// </summary>
        internal Endpoint Start
        {
            get => _start ?? Throw.InvalidOperation<Endpoint>();
            private set
            {
                if (value > _end) Throw.ArgumentOutOfRange(nameof(value));
                _start = value;
            }
        }

        private Endpoint? _end;
        /// <summary>
        /// Gets the end of the segment.
        /// </summary>
        internal Endpoint End
        {
            get => _end ?? Throw.InvalidOperation<Endpoint>();
            private set
            {
                if (value < _start) Throw.ArgumentOutOfRange(nameof(value));
                _end = value;
            }
        }

        /// <summary>
        /// Gets the range of supported years.
        /// </summary>
        private Range<int> SupportedYears => _schema.SupportedYears;

        /// <summary>
        /// Gets the range of supported values for the number of consecutive days from the epoch.
        /// </summary>
        private Range<int> SupportedDays => _schema.SupportedDays;

        /// <summary>
        /// Gets the pre-validator for this schema.
        /// </summary>
        private ICalendricalPreValidator PreValidator => _schema.PreValidator;

        /// <summary>
        /// Builds the segment.
        /// </summary>
        /// <exception cref="InvalidOperationException">The segment was not buildable.</exception>
        [Pure]
        public CalendricalSegment BuildSegment()
        {
            bool complete =
                Start.OrdinalParts == OrdinalParts.AtStartOfYear(Start.Year)
                && End.OrdinalParts == _partsAdapter.GetOrdinalPartsAtEndOfYear(End.Year);

            return new CalendricalSegment(_schema, Start, End) { IsComplete = complete };
        }
    }

    public partial class CalendricalSegmentBuilder // Builder methods
    {
        /// <summary>
        /// Sets the start of the segment to the specified number of consecutive days from the epoch.
        /// </summary>
        public void SetMinDaysSinceEpoch(int daysSinceEpoch)
        {
            if (SupportedDays.Contains(daysSinceEpoch) == false)
            {
                Throw.ArgumentOutOfRange(nameof(daysSinceEpoch));
            }

            Start = new Endpoint(_schema)
            {
                DaysSinceEpoch = daysSinceEpoch,
                DateParts = _partsAdapter.GetDateParts(daysSinceEpoch),
                OrdinalParts = _partsAdapter.GetOrdinalParts(daysSinceEpoch),
            };
        }

        /// <summary>
        /// Sets the end of the segment to the specified number of consecutive days from the epoch.
        /// </summary>
        public void SetMaxDaysSinceEpoch(int daysSinceEpoch)
        {
            if (SupportedDays.Contains(daysSinceEpoch) == false)
            {
                Throw.ArgumentOutOfRange(nameof(daysSinceEpoch));
            }

            End = new Endpoint(_schema)
            {
                DaysSinceEpoch = daysSinceEpoch,
                DateParts = _partsAdapter.GetDateParts(daysSinceEpoch),
                OrdinalParts = _partsAdapter.GetOrdinalParts(daysSinceEpoch),
            };
        }

        /// <summary>
        /// Sets the start of the segment to the specified date.
        /// </summary>
        public void SetMinDate(int year, int month, int day)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year);
            PreValidator.ValidateMonthDay(year, month, day);

            Start = new Endpoint(_schema)
            {
                DaysSinceEpoch = _schema.CountDaysSinceEpoch(year, month, day),
                DateParts = new DateParts(year, month, day),
                OrdinalParts = _partsAdapter.GetOrdinalParts(year, month, day),
            };
        }

        /// <summary>
        /// Sets the end of the segment to the specified date.
        /// </summary>
        public void SetMaxDate(int year, int month, int day)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year);
            PreValidator.ValidateMonthDay(year, month, day);

            End = new Endpoint(_schema)
            {
                DaysSinceEpoch = _schema.CountDaysSinceEpoch(year, month, day),
                DateParts = new DateParts(year, month, day),
                OrdinalParts = _partsAdapter.GetOrdinalParts(year, month, day),
            };
        }

        /// <summary>
        /// Sets the start of the segment to the specified ordinal date.
        /// </summary>
        public void SetMinOrdinal(int year, int dayOfYear)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year);
            PreValidator.ValidateDayOfYear(year, dayOfYear);

            Start = new Endpoint(_schema)
            {
                DaysSinceEpoch = _schema.CountDaysSinceEpoch(year, dayOfYear),
                DateParts = _partsAdapter.GetDateParts(year, dayOfYear),
                OrdinalParts = new OrdinalParts(year, dayOfYear),
            };
        }

        /// <summary>
        /// Sets the end of the segment to the specified ordinal date.
        /// </summary>
        public void SetMaxOrdinal(int year, int dayOfYear)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year);
            PreValidator.ValidateDayOfYear(year, dayOfYear);

            End = new Endpoint(_schema)
            {
                DaysSinceEpoch = _schema.CountDaysSinceEpoch(year, dayOfYear),
                DateParts = _partsAdapter.GetDateParts(year, dayOfYear),
                OrdinalParts = new OrdinalParts(year, dayOfYear),
            };
        }

        /// <summary>
        /// Sets the start of the segment to the start of the specified year.
        /// </summary>
        public void SetMinYear(int year)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year);

            Start = new Endpoint(_schema)
            {
                DaysSinceEpoch = _schema.GetStartOfYear(year),
                DateParts = DateParts.AtStartOfYear(year),
                OrdinalParts = OrdinalParts.AtStartOfYear(year),
            };
        }

        /// <summary>
        /// Sets the end of the segment to the end of the specified year.
        /// </summary>
        public void SetMaxYear(int year)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year);

            End = new Endpoint(_schema)
            {
                DaysSinceEpoch = _schema.GetEndOfYear(year),
                DateParts = _partsAdapter.GetDatePartsAtEndOfYear(year),
                OrdinalParts = _partsAdapter.GetOrdinalPartsAtEndOfYear(year),
            };
        }

        /// <summary>
        /// Sets the start of the segment to the start of the earliest supported year.
        /// </summary>
        /// <exception cref="ArgumentException">The range of supported years by the schema
        /// does not contain the year 1.</exception>
        public void UseMinSupportedYear(bool onOrAfterEpoch)
        {
            int minYear =
                onOrAfterEpoch
                ? _minYearOnOrAfterYear1 ?? Throw.Argument<int>(nameof(onOrAfterEpoch))
                : SupportedYears.Min;
            SetMinYear(minYear);
        }

        /// <summary>
        /// Sets the end of the segment to the end of the latest supported year.
        /// </summary>
        public void UseMaxSupportedYear() => SetMaxYear(SupportedYears.Max);

        internal void SetSupportedYears(Range<int> supportedYears)
        {
            SetMinYear(supportedYears.Min);
            SetMaxYear(supportedYears.Max);
        }
    }
}
