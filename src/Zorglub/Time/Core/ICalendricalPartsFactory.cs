﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    // TODO(api): add GetStartOfYearMonthParts() and GetEndOfYearMonthParts().

    #region Developer Notes

    // Types Implementing ICalendricalPartsFactory
    // -------------------------------------------
    //
    // ICalendricalPartsFactory
    // ├─ CalendricalSchemaPlusChecked
    // ├─ CalendricalSchemaPlusUnchecked
    // └─ SystemSchema
    //
    // Comments
    // --------
    // The methods here are not part of ICalendricalSchema because the schema
    // may support a range of years larger than the one supported by
    // Yemoda/Yedoy.
    //
    // Unchecked constructions of Yemoda/Yedoy objects being only visible to
    // "friendly" assemblies, implementing this interface outside this assembly
    // can be unefficient. Solution? For instance, when creating a new type of
    // calendar or date type, instead of ICalendricalSchemaPlus.GetDateParts(),
    // use ICalendricalSchema.GetDateParts(), then validate the result and
    // create the parts in one step using PartsFactory.

    #endregion

    /// <summary>
    /// Provides methods you can use to create new calendrical parts.
    /// </summary>
    public partial interface ICalendricalPartsFactory
    {
        // REVIEW(api): better solution?

        /// <summary>
        /// Creates a new <see cref="ICalendricalPartsFactory"/> instance.
        /// <para>When <paramref name="checked"/> is true, a method will throw if the result is not
        /// representable by the system.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        [Pure]
        public static ICalendricalPartsFactory Create(ICalendricalSchema schema, bool @checked = true) =>
            @checked ? new CalendricalPartsFactoryChecked(schema)
            : schema is ICalendricalPartsFactory sch ? sch
            : new CalendricalPartsFactoryUnchecked(schema);
    }

    public partial interface ICalendricalPartsFactory // Conversions
    {
        /// <summary>
        /// Obtains the date parts for the specified month count (the number of consecutive months
        /// from the epoch to a date).
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure] Yemo GetMonthParts(int monthsSinceEpoch);

        /// <summary>
        /// Obtains the date parts for the specified day count (the number of consecutive days from
        /// the epoch to a date).
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure] Yemoda GetDateParts(int daysSinceEpoch);

        /// <summary>
        /// Obtains the ordinal date parts for the specified day count (the number of consecutive
        /// days from the epoch to a date).
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure] Yedoy GetOrdinalParts(int daysSinceEpoch);

        /// <summary>
        /// Obtains the ordinal date parts for the specified date.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure] Yedoy GetOrdinalParts(int y, int m, int d);

        /// <summary>
        /// Obtains the date parts for the specified ordinal date.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure] Yemoda GetDateParts(int y, int doy);
    }

    public partial interface ICalendricalPartsFactory // Dates in a given year or month
    {
        /// <summary>
        /// Obtains the date parts for the first day of the specified year.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure] Yemoda GetStartOfYearParts(int y);

        /// <summary>
        /// Obtains the ordinal date parts for the first day of the specified year.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure] Yedoy GetStartOfYearOrdinalParts(int y);

        /// <summary>
        /// Obtains the date parts for the last day of the specified year.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure] Yemoda GetEndOfYearParts(int y);

        /// <summary>
        /// Obtains the ordinal date parts for the last day of the specified year.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure] Yedoy GetEndOfYearOrdinalParts(int y);

        /// <summary>
        /// Obtains the date parts for the first day of the specified month.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure] Yemoda GetStartOfMonthParts(int y, int m);

        /// <summary>
        /// Obtains the ordinal date parts for the first day of the specified month.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure] Yedoy GetStartOfMonthOrdinalParts(int y, int m);

        /// <summary>
        /// Obtains the date parts for the last day of the specified month.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure] Yemoda GetEndOfMonthParts(int y, int m);

        /// <summary>
        /// Obtains the ordinal date parts for the last day of the specified month.
        /// </summary>
        /// <exception cref="AoorException">The result is not representable by the system.</exception>
        [Pure] Yedoy GetEndOfMonthOrdinalParts(int y, int m);
    }
}
