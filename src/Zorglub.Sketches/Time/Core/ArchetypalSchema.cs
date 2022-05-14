// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using Zorglub.Time.Core.Intervals;

    #region Developer Notes

    // The code is NOT meant to be very efficient.
    //
    // Usefulness? ICalendricalKernel is easy to implement and test (see for
    // instance GregorianKernel), which might not be the case of
    // ICalendricalSchema. With ArchetypalSchema, we can then
    // - quickly prototype a new schema
    // - validate test data independently
    // At the same time, it demonstrates that an ICalendricalKernel instance is
    // all we need to build a full schema.
    //
    // Apart from the core kernel methods, the only other methods available
    // implicitely are the abstract/virtual ones.
    //
    // ### Abstract Methods
    //
    // - MinDaysInYear
    // - MinDaysInMonth
    // - SupportedYears
    // - PreValidator
    // - Arithmetic
    //
    // These are the properties defined by ICalendricalSchema not inherited from
    // ICalendricalKernel.
    //
    // ### Virtual Methods
    //
    // - CountDaysInYearBeforeMonth()
    // - GetYear()
    // - GetMonth()
    // - GetStartOfYear()
    //
    // WARNING: the default impl of GetYear() and GetStartOfYear() are extremely
    // slow if the values of "y" or "daysSinceEpoch" are big. See SupportedYears.
    //
    // Notice that in Zorglub.Time.Core.Schemas, we use purely computational
    // formulae (faster, no IndexOutOfRangeException) mostly obtained by
    // geometric means.
    //
    // - DayForm (trivial)
    // - MonthForm
    //   - CountDaysInYearBeforeMonth(m)
    //   - CountDaysInMonth(m)
    //   - GetMonth(d0y, out d0)
    // - YearForm
    //   - CountDaysInYear(y)
    //   - GetYear(daysSinceEpoch, out d0y)
    //   - GetStartOfYear(y)

    #endregion

    /// <summary>
    /// Represents an archetypal implementation of the <see cref="ICalendricalSchemaPlus"/> interface
    /// and provides a base for derived classes.
    /// </summary>
    public abstract partial class ArchetypalSchema :
        ICalendricalKernel,
        ICalendricalSchema,
        ICalendricalSchemaPlus
    {
        /// <summary>
        /// Represents the kernel of a schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalKernel _kernel;

        /// <summary>
        /// Represents a partial <see cref="ICalendricalSchema"/> view of this instance.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly SchemaProxy _proxy;

        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="ArchetypalSchema"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="kernel"/> is null.</exception>
        protected ArchetypalSchema(ICalendricalKernel kernel!!)
        {
            _kernel = kernel;
            _proxy = new SchemaProxy(this);
        }

        // Another solution could have been to cast "this" to ICalendricalSchema.
        private sealed class SchemaProxy
        {
            private readonly ICalendricalSchema _this;

            public SchemaProxy(ArchetypalSchema @this)
            {
                Debug.Assert(@this != null);
                _this = @this;
            }

            /// <summary>Conversion (y, m, d) -> (y, doy).</summary>
            [Pure]
            public int GetDayOfYear(int y, int m, int d) => _this.GetDayOfYear(y, m, d);

            /// <summary>Conversion daysSinceEpoch -> (y, m, d).</summary>
            public void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d) =>
                _this.GetDateParts(daysSinceEpoch, out y, out m, out d);
        }
    }

    public partial class ArchetypalSchema // ICalendricalKernel
    {
#pragma warning disable CA1033 // Interface methods should be callable by child types (Design)

        CalendricalAlgorithm ICalendricalKernel.Algorithm => _kernel.Algorithm;

        CalendricalFamily ICalendricalKernel.Family => _kernel.Family;

        CalendricalAdjustments ICalendricalKernel.PeriodicAdjustments => _kernel.PeriodicAdjustments;

        [Pure]
        bool ICalendricalKernel.IsRegular(out int monthsInYear) => _kernel.IsRegular(out monthsInYear);

#pragma warning restore CA1033

        /// <inheritdoc />
        [Pure]
        public int CountMonthsInYear(int y) => _kernel.CountMonthsInYear(y);

        /// <inheritdoc />
        [Pure]
        public int CountDaysInYear(int y) => _kernel.CountDaysInYear(y);

        /// <inheritdoc />
        [Pure]
        public int CountDaysInMonth(int y, int m) => _kernel.CountDaysInMonth(y, m);

        /// <inheritdoc />
        [Pure]
        public bool IsLeapYear(int y) => _kernel.IsLeapYear(y);

        /// <inheritdoc />
        [Pure]
        public bool IsIntercalaryMonth(int y, int m) => _kernel.IsIntercalaryMonth(y, m);

        /// <inheritdoc />
        [Pure]
        public bool IsIntercalaryDay(int y, int m, int d) => _kernel.IsIntercalaryDay(y, m, d);

        /// <inheritdoc />
        [Pure]
        public bool IsSupplementaryDay(int y, int m, int d) => _kernel.IsSupplementaryDay(y, m, d);
    }

    public partial class ArchetypalSchema // ICalendricalSchema (1)
    {
        // We limit the range of supported years because the default impl of
        // GetYear() and GetStartOfYear() are extremely slow if the values of
        // "y" or "daysSinceEpoch" are big.
        // Only override this property if both methods can handle big values
        // efficiently.

        /// <inheritdoc />
        public virtual Range<int> SupportedYears { get; } = Range.Create(-9998, 9999);

        /// <inheritdoc />
        [Pure]
        public virtual int CountDaysInYearBeforeMonth(int y, int m)
        {
            // Faster alternatives:
            // - Use a purely computational formula.
            // - Pre-compute the result and use an array lookup.
            //   A quick improvement would be to use GetDaysInMonthDistribution(leap)
            //   from IDaysInMonthDistribution which would avoid the repeated
            //   calls to CountDaysInMonth(y, m).

            int count = 0;
            for (int i = 1; i < m; i++)
            {
                count += CountDaysInMonth(y, i);
            }
            return count;
        }

        /// <inheritdoc />
        [Pure]
        public virtual int GetYear(int daysSinceEpoch, out int doy)
        {
            // Faster alternatives:
            // - Use a purely computational formula.
            // - Start with an approximation of the result, see PrototypalSchema.

            // Find the year for which (daysSinceEpoch - startOfYear) = d0y
            // has the smallest value >= 0.
            if (daysSinceEpoch < 0)
            {
                int y = 0;
                int startOfYear = -CountDaysInYear(0);

                while (daysSinceEpoch < startOfYear)
                {
                    startOfYear -= CountDaysInYear(--y);
                }

                // Notice that, as expected, doy >= 1.
                doy = 1 + daysSinceEpoch - startOfYear;
                return y;
            }
            else
            {
                int y = 1;
                int startOfYear = 0;

                while (daysSinceEpoch >= startOfYear)
                {
                    int startOfNextYear = startOfYear + CountDaysInYear(y);
                    if (daysSinceEpoch < startOfNextYear) { break; }
                    y++;
                    startOfYear = startOfNextYear;
                }
                Debug.Assert(daysSinceEpoch >= startOfYear);

                // Notice that, as expected, doy >= 1.
                doy = 1 + daysSinceEpoch - startOfYear;
                return y;
            }
        }

        /// <inheritdoc />
        [Pure]
        public virtual int GetMonth(int y, int doy, out int d)
        {
            // Faster alternatives:
            // - Use a purely computational formula.
            // - Start with an approximation of the result, see PrototypalSchema.

            int m = 1;
            int daysInYearBeforeMonth = 0;

            int monthsInYear = CountMonthsInYear(y);
            while (m < monthsInYear)
            {
                int daysInYearBeforeNextMonth = CountDaysInYearBeforeMonth(y, m + 1);
                if (doy <= daysInYearBeforeNextMonth) { break; }

                daysInYearBeforeMonth = daysInYearBeforeNextMonth;
                m++;
            }

            // Notice that, as expected, d >= 1.
            d = doy - daysInYearBeforeMonth;
            return m;
        }

        /// <inheritdoc />
        [Pure]
        public virtual int GetStartOfYear(int y)
        {
            // Faster alternatives:
            // - Use a purely computational formula.
            // - Cache the result, see PrototypalSchema.

            int daysSinceEpoch = 0;

            if (y < 1)
            {
                for (int i = y; i < 1; i++)
                {
                    daysSinceEpoch -= CountDaysInYear(i);
                }
            }
            else
            {
                for (int i = 1; i < y; i++)
                {
                    daysSinceEpoch += CountDaysInYear(i);
                }
            }

            return daysSinceEpoch;
        }
    }

    public partial class ArchetypalSchema // ICalendricalSchema (2)
    {
        /// <inheritdoc />
        public abstract int MinDaysInYear { get; }
        /// <inheritdoc />
        public abstract int MinDaysInMonth { get; }

        /// <inheritdoc />
        public abstract ICalendricalPreValidator PreValidator { get; }
        /// <inheritdoc />
        public abstract ICalendricalArithmetic Arithmetic { get; }

#pragma warning disable CA1033 // Interface methods should be callable by child types (Design)

        [Pure]
        int ICalendricalSchema.CountDaysSinceEpoch(int y, int m, int d) =>
            GetStartOfYear(y) + CountDaysInYearBeforeMonth(y, m) + d - 1;

        [Pure]
        int ICalendricalSchema.CountDaysSinceEpoch(int y, int doy) =>
            GetStartOfYear(y) + doy - 1;

        void ICalendricalSchema.GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
        {
            y = GetYear(daysSinceEpoch, out int doy);
            m = GetMonth(y, doy, out d);
        }

        [Pure]
        int ICalendricalSchema.GetDayOfYear(int y, int m, int d) =>
            CountDaysInYearBeforeMonth(y, m) + d;

        [Pure]
        int ICalendricalSchema.GetEndOfYear(int y) =>
            GetStartOfYear(y) + CountDaysInYear(y) - 1;

        [Pure]
        int ICalendricalSchema.GetStartOfMonth(int y, int m) =>
            GetStartOfYear(y) + CountDaysInYearBeforeMonth(y, m);

        [Pure]
        int ICalendricalSchema.GetEndOfMonth(int y, int m) =>
            GetStartOfYear(y) + CountDaysInYearBeforeMonth(y, m) + CountDaysInMonth(y, m) - 1;

#pragma warning restore CA1033
    }

    public partial class ArchetypalSchema // ICalendricalSchemaPlus
    {
#pragma warning disable CA1033 // Interface methods should be callable by child types (Design)

        [Pure]
        int ICalendricalSchemaPlus.CountDaysInYearAfterMonth(int y, int m) =>
            CountDaysInYear(y) - CountDaysInMonth(y, m) - CountDaysInYearBeforeMonth(y, m);

        #region CountDaysInYearBefore()

        [Pure]
        int ICalendricalSchemaPlus.CountDaysInYearBefore(int y, int m, int d)
        {
            // Conversion (y, m, d) -> (y, doy)
            int doy = _proxy.GetDayOfYear(y, m, d);
            return CountDaysInYearBeforeImpl(doy);
        }

        [Pure]
        int ICalendricalSchemaPlus.CountDaysInYearBefore(int y, int doy) =>
            CountDaysInYearBeforeImpl(doy);

        [Pure]
        int ICalendricalSchemaPlus.CountDaysInYearBefore(int daysSinceEpoch)
        {
            // Conversion daysSinceEpoch -> (y, ydoy)
            _ = GetYear(daysSinceEpoch, out int doy);
            return CountDaysInYearBeforeImpl(doy);
        }

        // "Natural" version, based on (y, ydoy).
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CountDaysInYearBeforeImpl(int doy) => doy - 1;

        #endregion
        #region CountDaysInYearAfter()

        [Pure]
        int ICalendricalSchemaPlus.CountDaysInYearAfter(int y, int m, int d)
        {
            // Conversion (y, m, d) -> (y, doy)
            int doy = _proxy.GetDayOfYear(y, m, d);
            return CountDaysInYearAfterImpl(y, doy);
        }

        [Pure]
        int ICalendricalSchemaPlus.CountDaysInYearAfter(int y, int doy) =>
            CountDaysInYearAfterImpl(y, doy);

        [Pure]
        int ICalendricalSchemaPlus.CountDaysInYearAfter(int daysSinceEpoch)
        {
            // Conversion daysSinceEpoch -> (y, doy)
            int y = GetYear(daysSinceEpoch, out int doy);
            return CountDaysInYearAfterImpl(y, doy);
        }

        // "Natural" version, based on (y, ydoy).
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CountDaysInYearAfterImpl(int y, int doy) => CountDaysInYear(y) - doy;

        #endregion
        #region CountDaysInMonthBefore()

        [Pure]
        int ICalendricalSchemaPlus.CountDaysInMonthBefore(int y, int m, int d) =>
            CountDaysInMonthBeforeImpl(d);

        /// <inheritdoc />
        [Pure]
        int ICalendricalSchemaPlus.CountDaysInMonthBefore(int y, int doy)
        {
            // Conversion (y, doy) -> (y, m, d)
            _ = GetMonth(y, doy, out int d);
            return CountDaysInMonthBeforeImpl(d);
        }

        [Pure]
        int ICalendricalSchemaPlus.CountDaysInMonthBefore(int daysSinceEpoch)
        {
            // Conversion daysSinceEpoch -> (y, m, d)
            _proxy.GetDateParts(daysSinceEpoch, out _, out _, out int d);
            return CountDaysInMonthBeforeImpl(d);
        }

        // "Natural" version, based on (y, m, d).
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CountDaysInMonthBeforeImpl(int d) => d - 1;

        #endregion
        #region CountDaysInMonthAfter()

        [Pure]
        int ICalendricalSchemaPlus.CountDaysInMonthAfter(int y, int m, int d) =>
            CountDaysInMonthAfterImpl(y, m, d);

        [Pure]
        int ICalendricalSchemaPlus.CountDaysInMonthAfter(int y, int doy)
        {
            // Conversion (y, doy) -> (y, m, d)
            int m = GetMonth(y, doy, out int d);
            return CountDaysInMonthAfterImpl(y, m, d);
        }

        [Pure]
        int ICalendricalSchemaPlus.CountDaysInMonthAfter(int daysSinceEpoch)
        {
            // Conversion daysSinceEpoch -> (y, m, d)
            _proxy.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
            return CountDaysInMonthAfterImpl(y, m, d);
        }

        // "Natural" version, based on (y, m, d).
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CountDaysInMonthAfterImpl(int y, int m, int d) => CountDaysInMonth(y, m) - d;

        #endregion

#pragma warning restore CA1033
    }
}
