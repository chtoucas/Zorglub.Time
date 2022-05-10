// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Geometry.Discrete;
using Zorglub.Time.Geometry.Forms;

#pragma warning disable CA1034 // Nested types should not be visible (Design) 👈 Tests

// MonthForm's.
// NB: the resulting form is the same whether the year is leap or not.
public static class Tropicalia3130MonthFormTests
{
    public sealed class CommonYear : AnalyzerBasicFacts
    {
        /// <summary>31, 30, 31, 30, 31, 30, 31, 30, 31, 30, 31, 29.</summary>
        private static readonly int[] s_MonthLengths =
            SchemaHelpers.GetDaysInMonthArray<Tropicalia3130Schema>(false);

        public CommonYear() : base(s_MonthLengths[0..^1]) { }

        public override CodeArray CodeArray0 => new(2, 5);

        /// <summary>(61, 2, 1)</summary>
        public override QuasiAffineForm Form { get; } = new(61, 2, 1);

        private QuasiAffineForm[] RotatedForms => new QuasiAffineForm[11] {
            Form,
            new(336, 11, 0),
            new(275, 9, 4),
            new(275, 9, 0),
            new(214, 7, 3),
            new(214, 7, 0),
            new(214, 7, 4),
            new(214, 7, 1),
            new(275, 9, 7),
            new(275, 9, 3),
            new(336, 11, 10),
        };

        [Fact]
        public void TryConvertCodeToForm_RotatedCode() =>
            TryConvertCodeToForm_AllVersions(RotatedForms);

        [Fact]
        public void MonthLengths_IsNotReducible()
        {
            // Arrange
            var code = new CodeArray(s_MonthLengths);
            // Act & Assert
            Assert.False(code.Reducible);
        }

        [Fact]
        public void MonthLengths_IsAlmostReducible()
        {
            // Arrange
            var code = new CodeArray(s_MonthLengths);
            var exp = new CodeArray(s_MonthLengths[0..^1]);
            // Act
            var isAlmostReducible = code.IsAlmostReducible(out var newCode, out int newIndex);
            // Assert
            Assert.True(isAlmostReducible);
            Assert.Equal(exp, newCode);
            Assert.Equal(0, newIndex);
        }

        [Fact]
        public void Form_FailsForLastMonth()
        {
            // Arrange
            int last = s_MonthLengths.Length - 1;
            // Act & Assert
            Assert.NotEqual(s_MonthLengths[^1], Form.CodeAt(last));
        }

        [Fact]
        public void MonthForm_Values() => Form_Equals(TropicalistaGeometry.MonthForm3130);
    }

    public sealed class LeapYear : AnalyzerBasicFacts
    {
        /// <summary>31, 30, 31, 30, 31, 30, 31, 30, 31, 30, 31, 30.</summary>
        private static readonly int[] s_MonthLengths =
            SchemaHelpers.GetDaysInMonthArray<Tropicalia3130Schema>(true);

        public LeapYear() : base(s_MonthLengths) { }

        public override CodeArray CodeArray0 => new(2, 5);

        /// <summary>(61, 2, 1)</summary>
        public override QuasiAffineForm Form { get; } = new(61, 2, 1);

        private QuasiAffineForm[] RotatedForms => new QuasiAffineForm[12] {
            Form,
            new(61, 2, 0),
            new(61, 2, 1),
            new(61, 2, 0),
            new(61, 2, 1),
            new(61, 2, 0),
            new(61, 2, 1),
            new(61, 2, 0),
            new(61, 2, 1),
            new(61, 2, 0),
            new(61, 2, 1),
            new(61, 2, 0),
        };

        [Fact]
        public void TryConvertCodeToForm_RotatedCode() =>
            TryConvertCodeToForm_AllVersions(RotatedForms);

        [Fact]
        public void MonthForm_Values()
        {
            // Arrange
            var form = TropicalistaGeometry.MonthForm3130;

            // Act & Assert
            Form_Equals(form);

            Assert.Equal(new MonthForm(61, 2, 1), form);
            Assert.Equal(new CalendricalForm(2, 61, 0), form.Reverse());

            var ordForm = form.WithOrdinalNumbering();
            Assert.Equal(new MonthForm(61, 2, -60, MonthFormNumbering.Ordinal), ordForm);
            Assert.Equal(new CalendricalForm(2, 61, 61), ordForm.Reverse());
        }
    }
}

#pragma warning restore CA1034
