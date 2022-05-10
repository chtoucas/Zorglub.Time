// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#if false

namespace Zorglub.Time.Extensions
{
    using Zorglub.Time.Core.Schemas;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology;

    public static partial class NakedCalendarFeaturettesTests
    {
        private static volatile MinMaxYearCalendar<PositivistSchema>? s_Positivist;
        public static MinMaxYearCalendar<PositivistSchema> Positivist =>
            s_Positivist ??= MinMaxYearCalendar.CreateRetroCalendar(
                "Positivist", CalendricalEpochs.Positivist, new PositivistSchema());
    }

    // Interface IBlankDayFeaturette.
    public partial class NakedCalendarFeaturettesTests
    {
        public static TheoryData<int, int, int, int, bool, bool> SamplePositivistDates =>
            new PositivistData().DateInfoData;

        [Fact]
        public static void IsBlankDay_InvalidCalendar()
        {
            // Arrange
            MinMaxYearCalendar<PositivistSchema> calendar = null!;
            // Act
            Assert.ThrowsAnexn("calendar", () => calendar.IsBlankDay(1, 1, 1));
        }

        [Fact]
        public static void IsBlankDay_IsValidating()
        {
            // Arrange
            var schema = new PositivistSchema();
            var scope = FauxCalendricalScope.Create(schema, CalendricalEpochs.Positivist);
            var chr = FakeNakedCalendar.Create(scope);
            // Act
            _ = chr.IsBlankDay(1, 1, 1);
            // Assert
            Assert.True(scope.ValidateYearMonthDayWasCalled);
        }

        [Theory, MemberData(nameof(SamplePositivistDates))]
        public static void IsBlankDay(int y, int m, int d, int _4, bool _5, bool isSupplementary)
        {
            // Act
            bool actual = Positivist.IsBlankDay(y, m, d);
            // Assert
            Assert.Equal(isSupplementary, actual);
        }
    }

    // Interface IEpagomenalFeaturette.
    public partial class NakedCalendarFeaturettesTests
    {
        private static readonly MinMaxYearCalendar<Coptic12Schema> s_CopticCalendar =
            MinMaxYearCalendar.CreateRetroCalendar(
                "Coptic", CalendricalEpochs.Coptic, new Coptic12Schema());

        public static TheoryData<int, int, int, int, bool, bool> SampleCopticDates =>
            new Coptic12Data().DateInfoData;

        [Fact]
        public static void IsEpagomenalDay_InvalidCalendar()
        {
            // Arrange
            MinMaxYearCalendar<Coptic12Schema> calendar = null!;
            // Act
            Assert.ThrowsAnexn("calendar", () => calendar.IsEpagomenalDay(1, 1, 1, out _));
        }

        [Fact]
        public static void IsEpagomenalDay_IsValidating()
        {
            // Arrange
            var schema = new Coptic12Schema();
            var scope = FauxCalendricalScope.Create(schema, CalendricalEpochs.Coptic);
            var chr = FakeNakedCalendar.Create(scope);
            // Act
            _ = chr.IsEpagomenalDay(1, 1, 1, out _);
            // Assert
            Assert.True(scope.ValidateYearMonthDayWasCalled);
        }

        [Theory, MemberData(nameof(SampleCopticDates))]
        public static void IsEpagomenalDay(int y, int m, int d, int _4, bool _5, bool isEpagomenal)
        {
            // Act
            bool isEpagomenalA = s_CopticCalendar.IsEpagomenalDay(y, m, d, out int epanum);
            // Assert
            Assert.Equal(isEpagomenal, isEpagomenalA);
            if (isEpagomenal)
            {
                Assert.True(epanum > 0);
            }
            else
            {
                Assert.Equal(0, epanum);
            }
        }

        [Theory, MemberData(nameof(Coptic12Data.EpagomenalDays), MemberType = typeof(Coptic12Data))]
        public static void IsEpagomenalDay_EpagomenalNumber(int y, int m, int d, int epanum)
        {
            // Act
            bool isEpagomenal = s_CopticCalendar.IsEpagomenalDay(y, m, d, out int epanumA);
            // Assert
            Assert.True(isEpagomenal);
            Assert.Equal(epanum, epanumA);
        }
    }
}

#endif