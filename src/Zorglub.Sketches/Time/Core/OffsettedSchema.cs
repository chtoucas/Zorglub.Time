// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;

    // J'aurais bien aimé pouvoir faire la même chose avec CalendricalSchema
    // mais en l'état cela me semble compliqué. Par exemple, Min/MaxYear
    // ne sont plus les mêmes, il faudrait tenir compte de Offset, mais ces
    // propriétés sont "fermées".

    public sealed class OffsettedSchema : ICalendricalSchema
    {
        /// <summary>
        /// Represents the genuine schema.
        /// <para>This field is read-ony.</para>
        /// </summary>
        private readonly ICalendricalSchema _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="OffsettedSchema"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        // REVIEW(api): Internal ctor?
        public OffsettedSchema(ICalendricalSchema schema, int offset)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));

            Offset = offset;
        }

        public int Offset { get; }

        /// <inheritdoc />
        public ICalendricalPreValidator PreValidator => _schema.PreValidator;

        /// <inheritdoc />
        public CalendricalAlgorithm Algorithm => _schema.Algorithm;
        /// <inheritdoc />
        public CalendricalFamily Family => _schema.Family;
        /// <inheritdoc />
        public CalendricalAdjustments PeriodicAdjustments => _schema.PeriodicAdjustments;

        /// <inheritdoc />
        public int MinDaysInYear => _schema.MinDaysInYear;
        /// <inheritdoc />
        public int MinDaysInMonth => _schema.MinDaysInMonth;

        /// <inheritdoc />
        public Range<int> SupportedYears => _schema.SupportedYears;

        private Range<int>? _supportedMonths;
        /// <inheritdoc />
        public Range<int> SupportedMonths =>
            _supportedMonths ??= new Range<int>(SupportedYears.Endpoints.Select(GetStartOfYearInMonths, GetEndOfYearInMonths));

        private Range<int>? _supportedDays;
        /// <inheritdoc />
        public Range<int> SupportedDays =>
            _supportedDays ??= new Range<int>(SupportedYears.Endpoints.Select(GetStartOfYear, GetEndOfYear));

        /// <inheritdoc />
        [Pure] public bool IsRegular(out int monthsInYear) => _schema.IsRegular(out monthsInYear);

        /// <inheritdoc />
        [Pure] public bool IsLeapYear(int y) => _schema.IsLeapYear(y - Offset);
        /// <inheritdoc />
        [Pure] public bool IsIntercalaryMonth(int y, int m) => _schema.IsIntercalaryMonth(y - Offset, m);
        /// <inheritdoc />
        [Pure] public bool IsIntercalaryDay(int y, int m, int d) => _schema.IsIntercalaryDay(y - Offset, m, d);
        /// <inheritdoc />
        [Pure] public bool IsSupplementaryDay(int y, int m, int d) => _schema.IsSupplementaryDay(y - Offset, m, d);

        /// <inheritdoc />
        [Pure] public int CountMonthsInYear(int y) => _schema.CountMonthsInYear(y - Offset);
        /// <inheritdoc />
        [Pure] public int CountDaysInYear(int y) => _schema.CountDaysInYear(y - Offset);
        /// <inheritdoc />
        [Pure] public int CountDaysInMonth(int y, int m) => _schema.CountDaysInMonth(y - Offset, m);
        /// <inheritdoc />
        [Pure] public int CountDaysInYearBeforeMonth(int y, int m) => _schema.CountDaysInYearBeforeMonth(y - Offset, m);

#if false // ICalendricalSchemaPlus
        /// <inheritdoc />
        [Pure] public int CountDaysInYearAfterMonth(int y, int m) => _schema.CountDaysInYearAfterMonth(y - Offset, m);
        /// <inheritdoc />
        [Pure] public int CountDaysInYearBefore(int y, int m, int d) => _schema.CountDaysInYearBefore(y - Offset, m, d);
        /// <inheritdoc />
        [Pure] public int CountDaysInYearBefore(int y, int doy) => _schema.CountDaysInYearBefore(y - Offset, doy);
        /// <inheritdoc />
        [Pure] public int CountDaysInYearBefore(int daysSinceEpoch) => throw new NotImplementedException();
        /// <inheritdoc />
        [Pure] public int CountDaysInYearAfter(int y, int m, int d) => _schema.CountDaysInYearAfter(y - Offset, m, d);
        /// <inheritdoc />
        [Pure] public int CountDaysInYearAfter(int y, int doy) => _schema.CountDaysInYearAfter(y - Offset, doy);
        /// <inheritdoc />
        [Pure] public int CountDaysInYearAfter(int daysSinceEpoch) => throw new NotImplementedException();
        /// <inheritdoc />
        [Pure] public int CountDaysInMonthBefore(int y, int m, int d) => _schema.CountDaysInMonthBefore(y - Offset, m, d);
        /// <inheritdoc />
        [Pure] public int CountDaysInMonthBefore(int y, int doy) => _schema.CountDaysInMonthBefore(y - Offset, doy);
        /// <inheritdoc />
        [Pure] public int CountDaysInMonthBefore(int daysSinceEpoch) => throw new NotImplementedException();
        /// <inheritdoc />
        [Pure] public int CountDaysInMonthAfter(int y, int m, int d) => _schema.CountDaysInMonthAfter(y - Offset, m, d);
        /// <inheritdoc />
        [Pure] public int CountDaysInMonthAfter(int y, int doy) => _schema.CountDaysInMonthAfter(y - Offset, doy);
        /// <inheritdoc />
        [Pure] public int CountDaysInMonthAfter(int daysSinceEpoch) => throw new NotImplementedException();
#endif

        /// <inheritdoc />
        [Pure] public int CountMonthsSinceEpoch(int y, int m) => _schema.CountMonthsSinceEpoch(y - Offset, m);
        /// <inheritdoc />
        [Pure] public int CountDaysSinceEpoch(int y, int m, int d) => _schema.CountDaysSinceEpoch(y - Offset, m, d);
        /// <inheritdoc />
        [Pure] public int CountDaysSinceEpoch(int y, int doy) => _schema.CountDaysSinceEpoch(y - Offset, doy);
        /// <inheritdoc />
        public void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
        {
            _schema.GetMonthParts(monthsSinceEpoch, out y, out m);
            y += Offset;
        }
        /// <inheritdoc />
        public void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
        {
            _schema.GetDateParts(daysSinceEpoch, out y, out m, out d);
            y += Offset;
        }
        /// <inheritdoc />
        [Pure] public int GetYear(int daysSinceEpoch, out int doy) => Offset + _schema.GetYear(daysSinceEpoch, out doy);
        /// <inheritdoc />
        [Pure] public int GetMonth(int y, int doy, out int d) => _schema.GetMonth(y - Offset, doy, out d);
        /// <inheritdoc />
        [Pure] public int GetDayOfYear(int y, int m, int d) => _schema.GetDayOfYear(y - Offset, m, d);

        /// <inheritdoc />
        [Pure] public int GetStartOfYearInMonths(int y) => _schema.GetStartOfYearInMonths(y - Offset);
        /// <inheritdoc />
        [Pure] public int GetEndOfYearInMonths(int y) => _schema.GetEndOfYearInMonths(y - Offset);
        /// <inheritdoc />
        [Pure] public int GetStartOfYear(int y) => _schema.GetStartOfYear(y - Offset);
        /// <inheritdoc />
        [Pure] public int GetEndOfYear(int y) => _schema.GetEndOfYear(y - Offset);
        /// <inheritdoc />
        [Pure] public int GetStartOfMonth(int y, int m) => _schema.GetStartOfMonth(y - Offset, m);
        /// <inheritdoc />
        [Pure] public int GetEndOfMonth(int y, int m) => _schema.GetEndOfMonth(y - Offset, m);

#if false // GetEndOfYearParts
        /// <inheritdoc />
        [Pure] public void GetEndOfYearParts(int y, out int m, out int d) => _schema.GetEndOfYearParts(y - Offset, out m, out d);
#endif
    }
}
