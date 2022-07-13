// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;
    using Zorglub.Time.Core.Validation;

    // FIXME(api): should only work with SystemSchema (because of Yemoda & co).
    // Add supportedYears to ctor, add CreateYewe().

    // Public factory methods for calendrical parts (ctors are internal).
    // Methods useful to avoid a double validation (validator and Yemoda own
    // validation) when creating a new instance of Yemoda & co.

    /// <summary>
    /// Provides factory methods to create new calendrical objects.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class PartsFactory
    {
        /// <summary>
        /// Represents a calendrical validator.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalValidator _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartsFactory"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="validator"/> is null.</exception>
        private PartsFactory(ICalendricalValidator validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        /// <summary>
        /// Creates a new instance of the <see cref="PartsFactory"/> class for which the validator
        /// allows all years within the range of supported years by <paramref name="schema"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        public static PartsFactory Create(SystemSchema schema)
        {
            Requires.NotNull(schema);

            return new(new CalendricalValidator(schema, schema.SupportedYears));
        }

        public static PartsFactory Create(SystemSchema schema, Range<int> supportedYears) =>
            new(new CalendricalValidator(schema, supportedYears));

        /// <summary>
        /// Validates the specified year, month and day then creates a new instance of
        /// <see cref="Yemoda"/> struct from them.
        /// </summary>
        /// <exception cref="AoorException">The input is not valid.</exception>
        [Pure]
        public Yemoda CreateYemoda(int year, int month, int day)
        {
            _validator.ValidateYearMonthDay(year, month, day);
            return new Yemoda(year, month, day);
        }

        /// <summary>
        /// Validates the specified year and month then creates a new instance of <see cref="Yemo"/>
        /// struct from them.
        /// </summary>
        /// <exception cref="AoorException">The input is not valid.</exception>
        [Pure]
        public Yemo CreateYemo(int year, int month)
        {
            _validator.ValidateYearMonth(year, month);
            return new Yemo(year, month);
        }

        /// <summary>
        /// Validates the specified year and day of the year then creates a new instance of
        /// <see cref="Yedoy"/> struct from them.
        /// </summary>
        /// <exception cref="AoorException">The input is not valid.</exception>
        [Pure]
        public Yedoy CreateYedoy(int year, int dayOfYear)
        {
            _validator.ValidateOrdinal(year, dayOfYear);
            return new Yedoy(year, dayOfYear);
        }

#if false
        /// <summary>
        /// Validates the specified year and week of the year then creates a new instance of
        /// <see cref="Yewe"/> struct from them.
        /// </summary>
        /// <exception cref="AoorException">The input is not valid.</exception>
        [Pure]
        public Yewe CreateYewe(int year, int weekOfYear)
        {
            return new Yewe(year, weekOfYear);
        }
#endif
    }
}
