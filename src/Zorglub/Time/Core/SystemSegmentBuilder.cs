// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;

    using Endpoint = SystemSegment.Endpoint;

    /// <summary>
    /// Represents a builder for <see cref="SystemSegment"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal sealed class SystemSegmentBuilder
    {
        /// <summary>
        /// Represents the schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly SystemSchema _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSegmentBuilder"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        public SystemSegmentBuilder(SystemSchema schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        private Endpoint? _start;
        /// <summary>
        /// Gets the start of the segment.
        /// </summary>
        public Endpoint Start
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
        public Endpoint End
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
        /// Builds the segment.
        /// </summary>
        /// <exception cref="InvalidOperationException">The segment was not buildable.</exception>
        [Pure]
        public SystemSegment BuildSegment() => new(_schema, Start, End);

        //
        // Builder methods
        //

        /// <summary>
        /// Sets the start of the segment to the start of the specified year.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="year"/> was outside the range of
        /// supported years by the schema.</exception>
        public void SetMinYear(int year)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year);

            Start = new Endpoint
            {
                DaysSinceEpoch = _schema.GetStartOfYear(year),
                DateParts = _schema.GetDatePartsAtStartOfYear(year),
                OrdinalParts = _schema.GetOrdinalPartsAtStartOfYear(year),
            };
        }

        /// <summary>
        /// Sets the end of the segment to the end of the specified year.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="year"/> was outside the range of
        /// supported years by the schema.</exception>
        public void SetMaxYear(int year)
        {
            if (SupportedYears.Contains(year) == false) Throw.YearOutOfRange(year);

            End = new Endpoint
            {
                DaysSinceEpoch = _schema.GetEndOfYear(year),
                DateParts = _schema.GetDatePartsAtEndOfYear(year),
                OrdinalParts = _schema.GetOrdinalPartsAtEndOfYear(year),
            };
        }

        public void SetSupportedYears(Range<int> supportedYears)
        {
            SetMinYear(supportedYears.Min);
            SetMaxYear(supportedYears.Max);
        }
    }
}
