// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Schemas
{
    using Ptolemaic12 = PtolemaicSchema.Twelve;

    /// <summary>
    /// Represents the Egyptian schema; see also <seealso cref="Egyptian13Schema"/>.
    /// <para>This class cannot be inherited.</para>
    /// <para>This class can ONLY be initialized from within friend assemblies.</para>
    /// </summary>
    /// <remarks>
    /// <para>A year is divided into 12 months of 30 days each, followed by 5 epagomenal days.</para>
    /// <para>The epagomenal days are outside any month but, for technical reasons, we attach them
    /// to the twelfth month: 31/12 to 35/12.</para>
    /// </remarks>
    public sealed partial class Egyptian12Schema :
        EgyptianSchema,
        IEpagomenalFeaturette,
        IDaysInMonthDistribution,
        IBoxable<Egyptian12Schema>
    {
        /// <summary>
        /// Represents the number of months in a year.
        /// <para>This field is a constant equal to 12.</para>
        /// </summary>
        private const int MonthsPerYear = 12;

        /// <summary>
        /// Initializes a new instance of the <see cref="Egyptian12Schema"/> class.
        /// </summary>
        internal Egyptian12Schema() : base(30) { }

        /// <inheritdoc />
        public sealed override int MonthsInYear => MonthsPerYear;

        /// <summary>
        /// Creates a new (boxed) instance of the <see cref="Egyptian12Schema"/> class.
        /// </summary>
        [Pure]
        public static Box<Egyptian12Schema> GetInstance() => Box.Create(new Egyptian12Schema());

        /// <inheritdoc />
        [Pure]
        static ReadOnlySpan<byte> IDaysInMonthDistribution.GetDaysInMonthDistribution(bool leap) =>
            // No leap years.
            new byte[12] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 35 };
    }

    public partial class Egyptian12Schema // Year, month or day infos
    {
        /// <inheritdoc />
        [Pure]
        public static bool IsEpagomenalDay(int y, int m, int d, out int epagomenalNumber) =>
            Ptolemaic12.IsEpagomenalDay(d, out epagomenalNumber);

        /// <inheritdoc />
        [Pure]
        public sealed override bool IsSupplementaryDay(int y, int m, int d) =>
            Ptolemaic12.IsSupplementaryDay(d);
    }

    public partial class Egyptian12Schema // Counting months and days within a year or a month
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int CountMonthsInYear(int y) => MonthsPerYear;

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysInMonth(int y, int m) => m == 12 ? 35 : 30;
    }

    public partial class Egyptian12Schema // Conversions
    {
        /// <inheritdoc />
        [Pure]
        public sealed override int CountMonthsSinceEpoch(int y, int m) =>
            IRegularSchema.Twelve.CountMonthsSinceEpoch(y, m);

        /// <inheritdoc />
        public sealed override void GetMonthParts(int monthsSinceEpoch, out int y, out int m) =>
            IRegularSchema.Twelve.GetMonthParts(monthsSinceEpoch, out y, out m);

        /// <inheritdoc />
        public sealed override void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
        {
            y = 1 + MathZ.Divide(daysSinceEpoch, DaysInYear, out int d0y);
            m = Ptolemaic12.GetMonth(d0y, out d);
        }

        /// <inheritdoc />
        [Pure]
        public sealed override int GetMonth(int y, int doy, out int d) =>
            Ptolemaic12.GetMonth(doy - 1, out d);
    }

    public partial class Egyptian12Schema // Dates in a given year or month
    {
        /// <inheritdoc />
        public sealed override void GetDatePartsAtEndOfYear(int y, out int m, out int d)
        {
            m = MonthsPerYear; d = 35;
        }
    }
}
