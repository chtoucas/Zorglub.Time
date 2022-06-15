// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Geometry.Discrete;
using Zorglub.Time.Geometry.Forms;

// LeapYearForm, leap years distribution.
public sealed class TabularIslamicLeapYearFormTests : AnalyzerFacts
{
    private static readonly int[] s_LeapYears =
        new int[22] {
            3, 2, 3, 3, 3, 2, 3, 3, 2, 3, 3,
            /* Next cycle */
            3, 2, 3, 3, 3, 2, 3, 3, 2, 3, 3 };

    public TabularIslamicLeapYearFormTests() : base(s_LeapYears) { }

    public override CodeArray CodeArray0 { get; } = new(3);

    /// <summary>(30, 11, 4)</summary>
    public override QuasiAffineForm Form { get; } = new(30, 11, 4);

    public override IEnumerable<QuasiAffineForm> FormList
    {
        get
        {
            yield return Form0;
            yield return new(1, 3, 1);
            yield return new(2, 3, 1);
            yield return new(11, 3, 1);
            yield return new(3, 11, 6);
            yield return new(8, 11, 4);
            yield return Form;
        }
    }

    public override IEnumerable<CodeArray> CodeArrayList
    {
        get
        {
            yield return CodeArray;
            yield return new(new[] { 4, 3, 4, 4, 3 });
            yield return CodeArray0;
        }
    }

    public override IEnumerable<TroeschMap> TroeschMapList
    {
        get
        {
            yield return new(2, true, 2);
            yield return new(3, true, 2);
        }
    }

    private QuasiAffineForm[] RotatedForms => new QuasiAffineForm[22] {
        Form,
        new(30, 11, 1),
        new(30, 11, 9),
        new(30, 11, 6),
        new(30, 11, 3),
        new(30, 11, 0),
        new(30, 11, 8),
        new(30, 11, 5),
        new(30, 11, 2),
        new(30, 11, 10),
        new(30, 11, 7),

        // Next cycle.
        Form,
        new(30, 11, 1),
        new(30, 11, 9),
        new(30, 11, 6),
        new(30, 11, 3),
        new(30, 11, 0),
        new(30, 11, 8),
        new(30, 11, 5),
        new(30, 11, 2),
        new(30, 11, 10),
        new(30, 11, 7),
    };

    [Fact]
    public override void TryConvertCodeToForm_RotatedCode() =>
        TryConvertCodeToForm_AllVersions(RotatedForms);

    [Fact]
    public void TryConvertCodeToForm_SingleCycle()
    {
        var code = new CodeArray(new[] { 3, 2, 3, 3, 3, 2, 3, 3, 2, 3, 3 });
        // Act & Assert
        Assert.True(TroeschAnalyzer.TryConvertCodeToForm(code, out var formA));
        Assert.Equal(new(19, 7, 3), formA);
    }
}

// YearForm's.
public sealed class TabularIslamicYearFormTests : AnalyzerFacts
{
    // Years = 0 to 59.
    private static readonly int[] s_YearLengths =
        new int[60] {
            /* First 30-year cycle */
            354, 354, 355,
            354, 354, 355,
            354, 355,
            354, 354, 355,
            354, 354, 355,
            354, 354, 355,
            354, 355,
            354, 354, 355,
            354, 354, 355,
            354, 355,
            354, 354, 355,
            /* Second 30-year cycle */
            354, 354, 355,
            354, 354, 355,
            354, 355,
            354, 354, 355,
            354, 354, 355,
            354, 354, 355,
            354, 355,
            354, 354, 355,
            354, 354, 355,
            354, 355,
            354, 354, 355,
        };

    public TabularIslamicYearFormTests() : base(s_YearLengths) { }

    public override CodeArray CodeArray0 { get; } = new(3);

    /// <summary>(10_631, 30, 3)</summary>
    public override QuasiAffineForm Form { get; } = new(10_631, 30, 3);

    public override IEnumerable<QuasiAffineForm> FormList
    {
        get
        {
            yield return Form0;
            yield return new(1, 3, 1);
            yield return new(2, 3, 1);
            yield return new(11, 3, 1);
            yield return new(3, 11, 3);
            yield return new(8, 11, 7);
            yield return new(30, 11, 7);
            yield return new(11, 30, 3);
            yield return Form;
        }
    }

    public override IEnumerable<CodeArray> CodeArrayList
    {
        get
        {
            yield return CodeArray;
            yield return new(new[] { 3, 3, 2, 3, 3, 3, 2, 3, 3, 2, 3, 3, 3, 2, 3, 3, 3, 2, 3, 3, 2, 3 });
            yield return new(new[] { 4, 3, 4, 4, 3 });
            yield return CodeArray0;
        }
    }

    public override IEnumerable<TroeschMap> TroeschMapList
    {
        get
        {
            yield return new(354, false, 0);
            yield return new(2, true, 3);
            yield return new(3, true, 2);
        }
    }

    private QuasiAffineForm[] RotatedForms => new QuasiAffineForm[60] {
        Form,
        new(10_631, 30, 14),
        new(10_631, 30, 25),
        new(10_631, 30, 6),
        new(10_631, 30, 17),
        new(10_631, 30, 28),
        new(10_631, 30, 9),
        new(10_631, 30, 20),
        new(10_631, 30, 1),
        new(10_631, 30, 12),
        new(10_631, 30, 23),
        new(10_631, 30, 4),
        new(10_631, 30, 15),
        new(10_631, 30, 26),
        new(10_631, 30, 7),
        new(10_631, 30, 18),
        new(10_631, 30, 29),
        new(10_631, 30, 10),
        new(10_631, 30, 21),
        new(10_631, 30, 2),
        new(10_631, 30, 13),
        new(10_631, 30, 24),
        new(10_631, 30, 5),
        new(10_631, 30, 16),
        new(10_631, 30, 27),
        new(10_631, 30, 8),
        new(10_631, 30, 19),
        new(10_631, 30, 0),
        new(10_631, 30, 11),
        new(10_631, 30, 22),

        // Next cycle.
        Form,
        new(10_631, 30, 14),
        new(10_631, 30, 25),
        new(10_631, 30, 6),
        new(10_631, 30, 17),
        new(10_631, 30, 28),
        new(10_631, 30, 9),
        new(10_631, 30, 20),
        new(10_631, 30, 1),
        new(10_631, 30, 12),
        new(10_631, 30, 23),
        new(10_631, 30, 4),
        new(10_631, 30, 15),
        new(10_631, 30, 26),
        new(10_631, 30, 7),
        new(10_631, 30, 18),
        new(10_631, 30, 29),
        new(10_631, 30, 10),
        new(10_631, 30, 21),
        new(10_631, 30, 2),
        new(10_631, 30, 13),
        new(10_631, 30, 24),
        new(10_631, 30, 5),
        new(10_631, 30, 16),
        new(10_631, 30, 27),
        new(10_631, 30, 8),
        new(10_631, 30, 19),
        new(10_631, 30, 0),
        new(10_631, 30, 11),
        new(10_631, 30, 22),
    };

    [Fact]
    public override void TryConvertCodeToForm_RotatedCode() =>
        TryConvertCodeToForm_AllVersions(RotatedForms);

    [Fact]
    public void TryConvertCodeToForm_SingleCycle()
    {
        // Years = 0 to 29.
        var codes = new int[30] {
            354, 354, 355,
            354, 354, 355,
            354, 355,
            354, 354, 355,
            354, 354, 355,
            354, 354, 355,
            354, 355,
            354, 354, 355,
            354, 354, 355,
            354, 355,
            354, 354, 355
        };
        var code = new CodeArray(codes);
        // Act & Assert
        Assert.True(TroeschAnalyzer.TryConvertCodeToForm(code, out var formA));
        Assert.Equal(new(6733, 19, 1), formA);
    }

    [Fact]
    public void YearForm_Values()
    {
        var form0 = TabularIslamicGeometry.YearForm0;
        var form = TabularIslamicGeometry.YearForm;
        // The sequence starting at year 1 (epoch) is s_YearLengths.Rotate(1)
        // and, according to RotatedForms, the matching form is (10_631, 30, 14).
        // Be careful, we can't use it as it, first we must normalize it
        // (here it is just the transformation y -> y -1).
        var form1 = new YearForm(10_631, 30, 14);

        // Act & Assert
        Form_Equals(form0);

        Assert.Equal(new YearForm(10_631, 30, 3) { Origin = new Yemoda(0, 1, 1) }, form0);
        Assert.Equal(new CalendricalForm(30, 10_631, 26), form0.Reverse());

        Assert.Equal(new YearForm(10_631, 30, -10_617), form);
        Assert.Equal(new CalendricalForm(30, 10_631, 10_646), form.Reverse());

        Assert.Equal(form, form0.Normalize()); // here Origin = (0, 1, 1).
        Assert.Equal(form, form1.Normalize()); // here Origin = Epoch.
    }
}

// MonthForm's.
public static class TabularIslamicMonthFormTests
{
    public sealed class CommonYear : AnalyzerFacts
    {
        /// <summary>30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 29.</summary>
        private static readonly int[] s_MonthLengths =
            SchemaHelpers.GetDaysInMonthArray<TabularIslamicSchema>(false);

        public CommonYear() : base(s_MonthLengths) { }

        public override CodeArray CodeArray0 { get; } = new(2, 5);

        /// <summary>(59, 2, 1)</summary>
        public override QuasiAffineForm Form { get; } = new(59, 2, 1);

        public override IEnumerable<QuasiAffineForm> FormList
        {
            get
            {
                yield return Form0;
                yield return new(1, 2, 1);
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
                yield return new(29, false, 1);
            }
        }

        private QuasiAffineForm[] RotatedForms => new QuasiAffineForm[12] {
            Form,
            new(59, 2, 0),
            new(59, 2, 1),
            new(59, 2, 0),
            new(59, 2, 1),
            new(59, 2, 0),
            new(59, 2, 1),
            new(59, 2, 0),
            new(59, 2, 1),
            new(59, 2, 0),
            new(59, 2, 1),
            new(59, 2, 0),
        };

        [Fact]
        public override void TryConvertCodeToForm_RotatedCode() =>
            TryConvertCodeToForm_AllVersions(RotatedForms);
    }

    public sealed class LeapYear : AnalyzerFacts
    {
        /// <summary>30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 30.</summary>
        private static readonly int[] s_MonthLengths =
            SchemaHelpers.GetDaysInMonthArray<TabularIslamicSchema>(true);

        public LeapYear() : base(s_MonthLengths) { }

        public override CodeArray CodeArray0 { get; } = new(5);

        /// <summary>(325, 11, 5)</summary>
        public override QuasiAffineForm Form { get; } = new(325, 11, 5);

        public override IEnumerable<QuasiAffineForm> FormList
        {
            get
            {
                yield return Form0;
                yield return new(1, 5, 0);
                yield return new(11, 5, 0);
                yield return new(5, 11, 5);
                yield return new(6, 11, 5);
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
                yield return new(29, true, 2);
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
            (true, new(325, 11, 10)),
        };

        [Fact]
        public override void TryConvertCodeToForm_RotatedCode() =>
            TryConvertCodeToForm_SomeVersions(RotatedForms);

        [Fact]
        public void MonthForm_Values()
        {
            var form = TabularIslamicGeometry.MonthFormForLeapYear;

            // Act & Assert
            Form_Equals(form);

            Assert.Equal(new MonthForm(325, 11, 5), form);
            Assert.Equal(new CalendricalForm(11, 325, 5), form.Reverse());

            var ordForm = form.WithOrdinalNumbering();
            Assert.Equal(new MonthForm(325, 11, -320, MonthFormNumbering.Ordinal), ordForm);
            Assert.Equal(new CalendricalForm(11, 325, 330), ordForm.Reverse());
        }
    }
}
