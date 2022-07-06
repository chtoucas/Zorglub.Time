// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;

    using Endpoint = CalendricalSegment.Endpoint;

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
        private readonly PartsFactory _partsFactory;

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

            _partsFactory = new PartsFactory(schema);

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
        private Range<int> Domain => _schema.Domain;

        /// <summary>
        /// Gets the pre-validator for this schema.
        /// </summary>
        private ICalendricalPreValidator PreValidator => _schema.PreValidator;

        /// <summary>
        /// Builds the segment.
        /// </summary>
        /// <exception cref="InvalidOperationException">The segment was not buildable.</exception>
        [Pure]
        public CalendricalSegment BuildSegment() => new(_schema, Start, End);
    }

    public partial class CalendricalSegmentBuilder // Builder methods
    {
        /// <summary>
        /// Sets the start of the segment to the specified number of consecutive days from the epoch.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        public void SetMinDaysSinceEpoch(int daysSinceEpoch)
        {
            if (Domain.Contains(daysSinceEpoch) == false)
            {
                Throw.ArgumentOutOfRange(nameof(daysSinceEpoch));
            }

            Start = new Endpoint
            {
                DaysSinceEpoch = daysSinceEpoch,
                DateParts = _partsFactory.GetDateParts(daysSinceEpoch),
                OrdinalParts = _partsFactory.GetOrdinalParts(daysSinceEpoch),
            };
        }

        /// <summary>
        /// Sets the end of the segment to the specified number of consecutive days from the epoch.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        public void SetMaxDaysSinceEpoch(int daysSinceEpoch)
        {
            if (Domain.Contains(daysSinceEpoch) == false)
            {
                Throw.ArgumentOutOfRange(nameof(daysSinceEpoch));
            }

            End = new Endpoint
            {
                DaysSinceEpoch = daysSinceEpoch,
                DateParts = _partsFactory.GetDateParts(daysSinceEpoch),
                OrdinalParts = _partsFactory.GetOrdinalParts(daysSinceEpoch),
            };
        }

        /// <summary>
        /// Sets the start of the segment to the specified date.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        public void SetMinDate(DateParts parts)
        {
            var (y, m, d) = parts;

            if (SupportedYears.Contains(y) == false) Throw.YearOutOfRange(y, nameof(parts));
            PreValidator.ValidateMonthDay(y, m, d, nameof(parts));

            Start = new Endpoint
            {
                DaysSinceEpoch = _schema.CountDaysSinceEpoch(y, m, d),
                DateParts = parts,
                OrdinalParts = _partsFactory.GetOrdinalParts(y, m, d),
            };
        }

        /// <summary>
        /// Sets the end of the segment to the specified date.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        public void SetMaxDate(DateParts parts)
        {
            var (y, m, d) = parts;

            if (SupportedYears.Contains(y) == false) Throw.YearOutOfRange(y, nameof(parts));
            PreValidator.ValidateMonthDay(y, m, d, nameof(parts));

            End = new Endpoint
            {
                DaysSinceEpoch = _schema.CountDaysSinceEpoch(y, m, d),
                DateParts = parts,
                OrdinalParts = _partsFactory.GetOrdinalParts(y, m, d),
            };
        }

        /// <summary>
        /// Sets the start of the segment to the specified ordinal date.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        public void SetMinOrdinal(OrdinalParts parts)
        {
            var (y, doy) = parts;

            if (SupportedYears.Contains(y) == false) Throw.YearOutOfRange(y, nameof(parts));
            PreValidator.ValidateDayOfYear(y, doy, nameof(parts));

            Start = new Endpoint
            {
                DaysSinceEpoch = _schema.CountDaysSinceEpoch(y, doy),
                DateParts = _partsFactory.GetDateParts(y, doy),
                OrdinalParts = parts,
            };
        }

        /// <summary>
        /// Sets the end of the segment to the specified ordinal date.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        public void SetMaxOrdinal(OrdinalParts parts)
        {
            var (y, doy) = parts;

            if (SupportedYears.Contains(y) == false) Throw.YearOutOfRange(y, nameof(parts));
            PreValidator.ValidateDayOfYear(y, doy, nameof(parts));

            End = new Endpoint
            {
                DaysSinceEpoch = _schema.CountDaysSinceEpoch(y, doy),
                DateParts = _partsFactory.GetDateParts(y, doy),
                OrdinalParts = parts,
            };
        }

        /// <summary>
        /// Sets the start of the segment to the start of the specified year.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        public void SetMinYear(int year)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year);

            Start = new Endpoint
            {
                DaysSinceEpoch = _schema.GetStartOfYear(year),
                DateParts = PartsFactory.GetDatePartsAtStartOfYear(year),
                OrdinalParts = PartsFactory.GetOrdinalPartsAtStartOfYear(year),
            };
        }

        /// <summary>
        /// Sets the end of the segment to the end of the specified year.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        public void SetMaxYear(int year)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year);

            End = new Endpoint
            {
                DaysSinceEpoch = _schema.GetEndOfYear(year),
                DateParts = _partsFactory.GetDatePartsAtEndOfYear(year),
                OrdinalParts = _partsFactory.GetOrdinalPartsAtEndOfYear(year),
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

        ///// <summary>
        ///// Sets the start of the segment to the start of the earliest supported year, and the end
        ///// of the segment to the end of the latest supported year.
        ///// </summary>
        //[Pure]
        //public void UseMaximalRange(bool onOrAfterEpoch)
        //{
        //    UseMinSupportedYear(onOrAfterEpoch);
        //    UseMaxSupportedYear();
        //}

        [Pure]
        internal void SetSupportedYears(Range<int> supportedYears)
        {
            SetMinYear(supportedYears.Min);
            SetMaxYear(supportedYears.Max);
        }
    }
}
