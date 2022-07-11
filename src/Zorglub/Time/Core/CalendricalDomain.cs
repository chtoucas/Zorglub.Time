// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;

    /// <summary>
    /// Defines a range of days or months.
    /// </summary>
    public sealed class CalendricalDomain
    {
        /// <summary>
        /// Represents the lower value.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _lowerValue;

        /// <summary>
        /// Represents the upper value.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _upperValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendricalDomain"/> class.
        /// </summary>
        public CalendricalDomain(Range<int> range)
        {
            Range = range;
            (_lowerValue, _upperValue) = range.Endpoints;
        }

        /// <summary>
        /// Gets the range of supported values.
        /// </summary>
        public Range<int> Range { get; }

        ///// <summary>
        ///// Returns true if the current domain contains the specified value; otherwise returns false.
        ///// </summary>
        //[Pure]
        //public bool Contains(int value) => _lowerValue <= value && value <= _upperValue;

        /// <summary>
        /// Validates the specified value.
        /// </summary>
        /// <exception cref="AoorException">The validation failed.</exception>
        public void Validate(int value)
        {
            if (value < _lowerValue || value > _upperValue) Throw.ArgumentOutOfRange(nameof(value));
        }

        /// <summary>
        /// Checks whether the specified value is outside the range of supported values or not.
        /// </summary>
        /// <exception cref="OverflowException">The value is outside the range of supported values.
        /// </exception>
        public void CheckOverflow(int value)
        {
            if (value < _lowerValue || value > _upperValue) Throw.DateOverflow();
        }

        /// <summary>
        /// Checks whether the specified value is greater than the upper bound of the range of
        /// supported values or not.
        /// </summary>
        /// <exception cref="OverflowException">The value is greater than the upper bound of the
        /// range of supported values.</exception>
        public void CheckUpperBound(int value)
        {
            if (value > _upperValue) Throw.DateOverflow();
        }

        /// <summary>
        /// Checks whether the specified value is less than the lower bound of the range of
        /// supported values or not.
        /// </summary>
        /// <exception cref="OverflowException">The value is less than the lower bound of the range
        /// of supported values.</exception>
        public void CheckLowerBound(int value)
        {
            if (value < _lowerValue) Throw.DateOverflow();
        }
    }
}
