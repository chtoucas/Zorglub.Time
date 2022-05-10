// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Geometry.Discrete;
using Zorglub.Time.Geometry.Forms;

#pragma warning disable CA1034 // Nested types should not be visible (Design) 👈 Tests

// MonthForm's.
public static class Tropicalia3031MonthFormTests
{
    public sealed partial class CommonYear : AnalyzerFacts
    {
        /// <summary>30, 31, 30, 31, 30, 31, 30, 31, 30, 31, 30, 30.</summary>
        private static readonly int[] s_MonthLengths =
            SchemaHelpers.GetDaysInMonthArray<Tropicalia3031Schema>(false);

        public CommonYear() : base(s_MonthLengths) { }

        public override CodeArray CodeArray0 { get; } = new(5);

        /// <summary>(335, 11, 5)</summary>
        public override QuasiAffineForm Form { get; } = new(335, 11, 5);

        public override IEnumerable<QuasiAffineForm> FormList
        {
            get
            {
                yield return Form0;
                yield return new(1, 5, 0);
                yield return new(11, 5, 0);
                yield return new(5, 11, 5);
                yield return Form;
            }
        }

        public override IEnumerable<CodeArray> CodeArrayList
        {
            get
            {
                yield return CodeArray;
                yield return new(new[] { 2, 2, 2, 2, 3 });
                yield return CodeArray0;
            }
        }

        public override IEnumerable<TroeschMap> TroeschMapList
        {
            get
            {
                yield return new(30, false, 2);
                yield return new(2, false, 0);
            }
        }

        private (bool, QuasiAffineForm?)[] RotatedForms => new (bool, QuasiAffineForm?)[12] {
            (true, Form),
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (true, new(335, 11, 0)),
        };

        [Fact]
        public override void TryConvertCodeToForm_RotatedCode() =>
            TryConvertCodeToForm_SomeVersions(RotatedForms);

        [Fact]
        public void MonthForm_Values()
        {
            var form = TropicalistaGeometry.MonthForm3031ForCommonYear;

            // Act & Assert
            Form_Equals(form);

            Assert.Equal(new MonthForm(335, 11, 5), form);
            Assert.Equal(new CalendricalForm(11, 335, 5), form.Reverse());

            var ordForm = form.WithOrdinalNumbering();
            Assert.Equal(new MonthForm(335, 11, -330, MonthFormNumbering.Ordinal), ordForm);
            Assert.Equal(new CalendricalForm(11, 335, 340), ordForm.Reverse());
        }
    }

    public sealed class LeapYear : AnalyzerFacts
    {
        /// <summary>30, 31, 30, 31, 30, 31, 30, 31, 30, 31, 30, 31.</summary>
        private static readonly int[] s_MonthLengths =
            SchemaHelpers.GetDaysInMonthArray<Tropicalia3031Schema>(true);

        public LeapYear() : base(s_MonthLengths) { }

        public override CodeArray CodeArray0 { get; } = new(2, 5);

        /// <summary>(61, 2, 0)</summary>
        public override QuasiAffineForm Form { get; } = new(61, 2, 0);

        public override IEnumerable<QuasiAffineForm> FormList
        {
            get
            {
                yield return Form0;
                yield return new(1, 2, 0);
                yield return Form;
            }
        }

        public override IEnumerable<CodeArray> CodeArrayList
        {
            get
            {
                yield return CodeArray;
                yield return CodeArray0;
            }
        }

        public override IEnumerable<TroeschMap> TroeschMapList
        {
            get
            {
                yield return new(30, false, 2);
            }
        }

        private QuasiAffineForm[] RotatedForms => new QuasiAffineForm[12] {
            Form,
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
            new(61, 2, 1),
        };

        [Fact]
        public override void TryConvertCodeToForm_RotatedCode() =>
            TryConvertCodeToForm_AllVersions(RotatedForms);

        [Fact]
        public void MonthForm_Values()
        {
            var form = TropicalistaGeometry.MonthForm3031ForLeapYear;

            // Act & Assert
            Form_Equals(form);

            Assert.Equal(new MonthForm(61, 2, 0), form);
            Assert.Equal(new CalendricalForm(2, 61, 1), form.Reverse());

            var ordForm = form.WithOrdinalNumbering();
            Assert.Equal(new MonthForm(61, 2, -61, MonthFormNumbering.Ordinal), ordForm);
            Assert.Equal(new CalendricalForm(2, 61, 62), ordForm.Reverse());
        }
    }
}

#pragma warning restore CA1034
