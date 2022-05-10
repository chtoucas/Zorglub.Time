// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#if false

namespace Zorglub.Time.Extensions
{
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology;

    public static partial class NakedCalendarExtensionsTests
    {
        private static volatile MinMaxYearCalendar<PaxSchema>? s_Pax;
        public static MinMaxYearCalendar<PaxSchema> Pax =>
            s_Pax ??= MinMaxYearCalendar.CreateRetroCalendar(
                "Pax", CalendricalEpochs.SundayBeforeGregorian, new PaxSchema());
    }

    // Abstract class LeapWeekSchema.
    public partial class NakedCalendarExtensionsTests
    {
        #region GetWeekdateParts()

        [Fact]
        public static void GetWeekdateParts_InvalidCalendar()
        {
            MinMaxYearCalendar<PaxSchema> calendar = null!;
            // Act
            Assert.ThrowsAnexn("this",
                () => calendar.GetWeekdateParts(default, out _, out _, out _));
        }

        [Fact]
        public static void GetWeekdateParts_InvalidDayNumber()
        {
            var minDayNumber = Pax.MinDayNumber;
            var maxDayNumber = Pax.MaxDayNumber;
            // Act & Assert
            Assert.ThrowsAoorexn("dayNumber",
                () => Pax.GetWeekdateParts(minDayNumber - 1, out _, out _, out _));
            Assert.ThrowsAoorexn("dayNumber",
                () => Pax.GetWeekdateParts(maxDayNumber + 1, out _, out _, out _));
        }

        [Theory, MemberData(nameof(PaxData.MoreSampleDayNumbers), MemberType = typeof(PaxData))]
        public static void GetWeekdateParts(DayNumber dayNumber, int _2, int _3, DayOfWeek _4) =>
            Assert.Throws<NotImplementedException>(
                () => Pax.GetWeekdateParts(dayNumber, out _, out _, out _));

        #endregion
    }

    // Abstract class PaxSchema.
    public partial class NakedCalendarExtensionsTests
    {
        public static TheoryData<int, int, int, int, bool> SamplePaxMonths =>
            new PaxData().MonthInfoData;

        #region IsPaxMonth()

        [Fact]
        public static void IsPaxMonth_InvalidCalendar()
        {
            MinMaxYearCalendar<PaxSchema> calendar = null!;
            // Act
            Assert.ThrowsAnexn("this", () => calendar.IsPaxMonth(1, 1));
        }

        [Fact]
        public static void IsPaxMonth_IsValidating()
        {
            var schema = new PaxSchema();
            var scope = FauxCalendricalScope.Create(schema, CalendricalEpochs.SundayBeforeGregorian);
            var chr = FakeNakedCalendar.Create(scope);
            // Act
            _ = chr.IsPaxMonth(1, 1);
            // Assert
            Assert.True(scope.ValidateYearMonthWasCalled);
        }

        [Theory, MemberData(nameof(PaxData.MoreSampleMonths), MemberType = typeof(PaxData))]
        public static void IsPaxMonth(int y, int m, bool isPaxMonth, bool _4)
        {
            // Act
            bool actual = Pax.IsPaxMonth(y, m);
            // Assert
            Assert.Equal(isPaxMonth, actual);
        }

        #endregion

        #region IsLastMonthOfYear()

        [Fact]
        public static void IsLastMonthOfYear_InvalidCalendar()
        {
            MinMaxYearCalendar<PaxSchema> calendar = null!;
            // Act
            Assert.ThrowsAnexn("this", () => calendar.IsLastMonthOfYear(1, 1));
        }

        [Fact]
        public static void IsLastMonthOfYear_IsValidating()
        {
            var schema = new PaxSchema();
            var scope = FauxCalendricalScope.Create(schema, CalendricalEpochs.SundayBeforeGregorian);
            var chr = FakeNakedCalendar.Create(scope);
            // Act
            _ = chr.IsLastMonthOfYear(1, 1);
            // Assert
            Assert.True(scope.ValidateYearMonthWasCalled);
        }

        [Theory, MemberData(nameof(PaxData.MoreSampleMonths), MemberType = typeof(PaxData))]
        public static void IsLastMonthOfYear(int y, int m, bool _3, bool isLastMonthOfYear)
        {
            // Act
            bool actual = Pax.IsLastMonthOfYear(y, m);
            // Assert
            Assert.Equal(isLastMonthOfYear, actual);
        }

        #endregion
    }
}

#endif