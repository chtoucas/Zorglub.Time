// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Validation;

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
        /// Represents the range of supported years.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly SupportedYears _supportedYears;

        /// <summary>
        /// Represents the range of supported values for the number of consecutive days from the epoch.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly SupportedDays _supportedDays;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendricalSegmentBuilder"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        public CalendricalSegmentBuilder(ICalendricalSchema schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));

            _partsAdapter = new PartsAdapter(schema);
            _supportedYears = new SupportedYears(schema.SupportedYears);
            _supportedDays = new SupportedDays(schema.SupportedYears);
        }

        /// <summary>
        /// Returns true if the minimum has been set; otherwise returns false.
        /// </summary>
        public bool HasMin => _min != null;

        /// <summary>
        /// Returns true if the maximum has been set; otherwise returns false.
        /// </summary>
        public bool HasMax => _max != null;

        /// <summary>
        /// Returns true if both minimum and maximum have been set; otherwise returns false.
        /// </summary>
        public bool IsBuildable => HasMin && HasMax;

        private Endpoint? _min;
        /// <summary>
        /// Gets the minimum of the segment.
        /// </summary>
        private Endpoint Min
        {
            get => _min ?? Throw.InvalidOperation<Endpoint>();
            set
            {
                if (value > _max) Throw.ArgumentOutOfRange(nameof(value));
                _min = value;
            }
        }

        private Endpoint? _max;
        /// <summary>
        /// Gets the maximum of the segment.
        /// </summary>
        private Endpoint Max
        {
            get => _max ?? Throw.InvalidOperation<Endpoint>();
            set
            {
                if (value < _min) Throw.ArgumentOutOfRange(nameof(value));
                _max = value;
            }
        }

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
            var min = FixEndpoint(Min);
            var max = FixEndpoint(Max);

            bool complete =
                min.OrdinalParts == OrdinalParts.AtStartOfYear(min.Year)
                && max.OrdinalParts == _partsAdapter.GetOrdinalPartsAtEndOfYear(max.Year);

            return new CalendricalSegment(_schema, min, max) { IsComplete = complete };

            [Pure]
            Endpoint FixEndpoint(Endpoint endpoint)
            {
                var (y, m) = endpoint.MonthParts;
                endpoint.MonthsSinceEpoch = _schema.CountMonthsSinceEpoch(y, m);
                return endpoint;
            }
        }
    }

    public partial class CalendricalSegmentBuilder // Builder methods
    {
        /// <summary>
        /// Sets the minimum to the specified number of consecutive days from the epoch.
        /// </summary>
        public void SetMinDaysSinceEpoch(int daysSinceEpoch) =>
            Min = GetEndpoint(daysSinceEpoch);

        /// <summary>
        /// Sets the maximum to the specified number of consecutive days from the epoch.
        /// </summary>
        public void SetMaxDaysSinceEpoch(int daysSinceEpoch) =>
            Max = GetEndpoint(daysSinceEpoch);

        /// <summary>
        /// Sets the minimum to the specified date.
        /// </summary>
        public void SetMinDate(int year, int month, int day) =>
            Min = GetEndpoint(year, month, day);

        /// <summary>
        /// Sets the maximum to the specified date.
        /// </summary>
        public void SetMaxDate(int year, int month, int day) =>
            Max = GetEndpoint(year, month, day);

        /// <summary>
        /// Sets the minimum to the specified ordinal date.
        /// </summary>
        public void SetMinOrdinal(int year, int dayOfYear) =>
            Min = GetEndpoint(year, dayOfYear);

        /// <summary>
        /// Sets the maximum to the specified ordinal date.
        /// </summary>
        public void SetMaxOrdinal(int year, int dayOfYear) =>
            Max = GetEndpoint(year, dayOfYear);

        /// <summary>
        /// Sets the minimum to the start of the specified year.
        /// </summary>
        public void SetMinYear(int year)
        {
            _supportedYears.Validate(year);

            Min = new Endpoint
            {
                DaysSinceEpoch = _schema.GetStartOfYear(year),
                DateParts = DateParts.AtStartOfYear(year),
                OrdinalParts = OrdinalParts.AtStartOfYear(year),
            };
        }

        /// <summary>
        /// Sets the maximum to the end of the specified year.
        /// </summary>
        public void SetMaxYear(int year)
        {
            _supportedYears.Validate(year);

            Max = new Endpoint
            {
                DaysSinceEpoch = _schema.GetEndOfYear(year),
                DateParts = _partsAdapter.GetDatePartsAtEndOfYear(year),
                OrdinalParts = _partsAdapter.GetOrdinalPartsAtEndOfYear(year),
            };
        }

        /// <summary>
        /// Sets the minimum to the start of the earliest supported year.
        /// </summary>
        /// <exception cref="ArgumentException">The range of supported years by the schema
        /// does not contain the year 1.</exception>
        public void UseMinSupportedYear(bool onOrAfterEpoch)
        {
            if (onOrAfterEpoch)
            {
                // Compute the earliest supported year >= 1.
                var set = Interval.Intersect(_schema.SupportedYears, Range.StartingAt(1));
                if (set.IsEmpty) Throw.Argument(nameof(onOrAfterEpoch));

                SetMinYear(set.Range.Min);
            }
            else
            {
                SetMinYear(_supportedYears.MinYear);
            }
        }

        /// <summary>
        /// Sets the maximum to the end of the latest supported year.
        /// </summary>
        public void UseMaxSupportedYear() => SetMaxYear(_supportedYears.MaxYear);

        internal void SetSupportedYears(Range<int> supportedYears)
        {
            SetMinYear(supportedYears.Min);
            SetMaxYear(supportedYears.Max);
        }

        //
        // Private helpers
        //

        [Pure]
        private Endpoint GetEndpoint(int daysSinceEpoch)
        {
            _supportedDays.Validate(daysSinceEpoch);

            return new Endpoint
            {
                DaysSinceEpoch = daysSinceEpoch,
                DateParts = _partsAdapter.GetDateParts(daysSinceEpoch),
                OrdinalParts = _partsAdapter.GetOrdinalParts(daysSinceEpoch),
            };
        }

        [Pure]
        private Endpoint GetEndpoint(int year, int month, int day)
        {
            _supportedYears.Validate(year);
            PreValidator.ValidateMonthDay(year, month, day);

            return new Endpoint
            {
                DaysSinceEpoch = _schema.CountDaysSinceEpoch(year, month, day),
                DateParts = new DateParts(year, month, day),
                OrdinalParts = _partsAdapter.GetOrdinalParts(year, month, day),
            };
        }

        [Pure]
        private Endpoint GetEndpoint(int year, int dayOfYear)
        {
            _supportedYears.Validate(year);
            PreValidator.ValidateDayOfYear(year, dayOfYear);

            return new Endpoint
            {
                DaysSinceEpoch = _schema.CountDaysSinceEpoch(year, dayOfYear),
                DateParts = _partsAdapter.GetDateParts(year, dayOfYear),
                OrdinalParts = new OrdinalParts(year, dayOfYear),
            };
        }
    }
}
