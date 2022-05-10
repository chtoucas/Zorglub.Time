// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry;

using Zorglub.Time.Geometry.Discrete;

// LeapYearForm, leap years distribution.
public sealed partial class HebrewLeapYearFormTests : AnalyzerFacts
{
    private static readonly int[] s_LeapYears =
        new int[14] {
            3, 3, 2, 3, 3, 3, 2, /* Next cycle */
            3, 3, 2, 3, 3, 3, 2 };

    public HebrewLeapYearFormTests() : base(s_LeapYears) { }

    public override CodeArray CodeArray0 { get; } = new(2);

    /// <summary>(19, 7, 5)</summary>
    public override QuasiAffineForm Form { get; } = new(19, 7, 5);

    public override IEnumerable<QuasiAffineForm> FormList
    {
        get
        {
            yield return Form0;
            yield return new(1, 2, 1);
            yield return new(7, 2, 1);
            yield return new(2, 7, 1);
            yield return new(5, 7, 5);
            yield return Form;
        }
    }

    public override IEnumerable<CodeArray> CodeArrayList
    {
        get
        {
            yield return CodeArray;
            yield return new(new[] { 4, 3, 4 });
            yield return CodeArray0;
        }
    }

    public override IEnumerable<TroeschMap> TroeschMapList
    {
        get
        {
            yield return new(2, true, 3);
            yield return new(3, false, 1);
        }
    }

    private QuasiAffineForm[] RotatedForms => new QuasiAffineForm[14] {
        Form,
        new(19, 7, 3),
        new(19, 7, 1),
        new(19, 7, 6),
        new(19, 7, 4),
        new(19, 7, 2),
        new(19, 7, 0),
        // Next cycle.
        new(19, 7, 5),
        new(19, 7, 3),
        new(19, 7, 1),
        new(19, 7, 6),
        new(19, 7, 4),
        new(19, 7, 2),
        new(19, 7, 0),
    };

    [Fact]
    public override void TryConvertCodeToForm_RotatedCode() =>
        TryConvertCodeToForm_AllVersions(RotatedForms);

    [Fact]
    public void TryConvertCodeToForm_SingleCycle()
    {
        // Arrange
        var code = new CodeArray(new[] { 3, 3, 2, 3, 3, 3, 2 });
        // Act & Assert
        Assert.True(TroeschAnalyzer.TryConvertCodeToForm(code, out var formA));
        Assert.Equal(new(11, 4, 2), formA);
    }
}

// A chaserah year (Hebrew for "deficient" or "incomplete") is 353 or 383
// days long. Both Cheshvan and Kislev have 29 days. The Hebrew letter ח
// "het" is used in the keviyah.
//
// A kesidrah year ("regular" or "in-order") is 354 or 384 days long.
// Cheshvan has 29 days while Kislev has 30 days. The Hebrew letter כ
// "kaf" is used in the keviyah.
//
// A shlemah year ("complete" or "perfect", also "abundant") is 355 or
// 385 days long. Both Cheshvan and Kislev have 30 days. The Hebrew
// letter ש "shin" is used in the keviyah.

#pragma warning disable CA1034 // Nested types should not be visible (Design) 👈 Tests

// MonthForm's.
public static class HebrewMonthFormTests
{
    public sealed class DeficientCommonYear : AnalyzerFacts
    {
        private static readonly int[] s_MonthLengths =
            new int[12] { /*Tebeth*/29, 30, 29, /*Nissan*/30, 29, 30, 29, 30, 29, /*Tsiri*/30, 29, 29 };

        public DeficientCommonYear() : base(s_MonthLengths) { }

        public override CodeArray CodeArray0 { get; } = new(5);

        /// <summary>(324, 11, 5)</summary>
        public override QuasiAffineForm Form { get; } = new(324, 11, 5);

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
                yield return new(29, false, 2);
                yield return new(2, false, 0);
            }
        }

        private (bool, QuasiAffineForm?)[] RotatedForms => new (bool, QuasiAffineForm?)[12] {
            (true, Form), // Tebeth
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (false, null), // Tsiri
            (false, null),
            (true, new(324, 11, 0)), // Elloul
        };

        [Fact]
        public override void TryConvertCodeToForm_RotatedCode() =>
            TryConvertCodeToForm_SomeVersions(RotatedForms);
    }

    public sealed class DeficientEmbolismicYear : AnalyzerFacts
    {
        private static readonly int[] s_MonthLengths =
            new int[13] { /*Tebeth*/29, 30, 30, 29, /*Nissan*/30, 29, 30, 29, 30, 29, /*Tsiri*/30, 29, 29 };

        public DeficientEmbolismicYear() : base(s_MonthLengths[0..^1]) { }

        public override CodeArray CodeArray0 { get; } = new(5);

        /// <summary>(325, 11, 4)</summary>
        public override QuasiAffineForm Form { get; } = new(325, 11, 4);

        public override IEnumerable<QuasiAffineForm> FormList
        {
            get
            {
                yield return Form0;
                yield return new(1, 5, 4);
                yield return new(11, 5, 4);
                yield return new(5, 11, 6);
                yield return new(6, 11, 4);
                yield return Form;
            }
        }

        public override IEnumerable<CodeArray> CodeArrayList
        {
            get
            {
                yield return CodeArray;
                yield return new(new[] { 3, 2, 2, 2, 2 });
                yield return CodeArray0;
            }
        }

        public override IEnumerable<TroeschMap> TroeschMapList
        {
            get
            {
                yield return new(29, true, 1);
                yield return new(2, false, 1);
            }
        }

        private (bool, QuasiAffineForm?)[] RotatedForms => new (bool, QuasiAffineForm?)[12] {
            (true, Form), // Tebeth
            (false, null),
            (true, new(324, 11, 10)), // Adar
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (false, null),
            (false, null),
        };

        // On examine aussi les autres combinaisons même si, ici, cela ne sert
        // pas à grand chose puisqu'on travaille sur un tableau tronqué et donc
        // nécessairement fixe.
        [Fact]
        public override void TryConvertCodeToForm_RotatedCode() =>
            TryConvertCodeToForm_SomeVersions(RotatedForms);

        [Fact]
        public void MonthLengths_IsNotSegment()
        {
            // Arrange
            var code = new CodeArray(s_MonthLengths);
            // Act & Assert
            Assert.True(code.StrictlyReducible);

            Assert.False(TroeschAnalyzer.TryConvertCodeToForm(code, out _));

            // Even after rotation, the sequence is still not a segment.
            for (int i = 1; i < code.Count; i++)
            {
                Assert.False(TroeschAnalyzer.TryConvertCodeToForm(code.Rotate(i), out _));
            }

            // Rotation THEN removing the end.
            // We ignore i = 0 since this is what works (NB: Rotate() would throw anyway).
            for (int i = 1; i < code.Count; i++)
            {
                Assert.False(TroeschAnalyzer.TryConvertCodeToForm(code.Rotate(i)[0..^1], out _));
            }

            // Conclusion: only s_MonthLengths[0..^1] works!
        }

        [Fact]
        public void Form_FailsForLastMonth()
        {
            // Arrange
            int last = s_MonthLengths.Length - 1;
            // Act & Assert
            Assert.NotEqual(s_MonthLengths[^1], Form.CodeAt(last));
        }
    }

    public sealed class RegularCommonYear : AnalyzerFacts
    {
        private static readonly int[] s_MonthLengths =
            new int[12] { /*Nissan*/30, 29, 30, 29, 30, 29, /*Tsiri*/30, 29, 30, 29, 30, 29 };

        public RegularCommonYear() : base(s_MonthLengths) { }

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
            Form, // Nissan
            new(59, 2, 0),
            new(59, 2, 1),
            new(59, 2, 0),
            new(59, 2, 1),
            new(59, 2, 0),
            new(59, 2, 1), // Tsiri
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

    public sealed class RegularEmbolimiscYear : AnalyzerFacts
    {
        // NB: The first twelve elements are identical to the Tabular Islamic ones.
        private static readonly int[] s_MonthLengths =
            new int[13] { /*Nissan*/30, 29, 30, 29, 30, 29, /*Tsiri*/30, 29, 30, 29, 30, 30, 29 };

        public RegularEmbolimiscYear() : base(s_MonthLengths) { }

        public override CodeArray CodeArray0 { get; } = new(5);

        /// <summary>(324, 11, 5)</summary>
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

        private QuasiAffineForm[] RotatedForms => new QuasiAffineForm[13] {
            Form, // Nissan
            new(325, 11, 0),
            new(266, 9, 4),
            new(266, 9, 0),
            new(207, 7, 3),
            new(207, 7, 0),
            new(266, 9, 6), // Tsiri
            new(266, 9, 2),
            new(325, 11, 9),
            new(325, 11, 4),
            new(384, 13, 12),
            new(59, 2, 1),
            new(384, 13, 0),
        };

        [Fact]
        public override void TryConvertCodeToForm_RotatedCode() =>
            TryConvertCodeToForm_AllVersions(RotatedForms);
    }

    public sealed class AbundantCommonYear : AnalyzerFacts
    {
        private static readonly int[] s_MonthLengths =
            new int[12] { /*Kislev*/30, 29, 30, 29, /*Nissan*/30, 29, 30, 29, 30, 29, /*Tsiri*/30, 30 };

        public AbundantCommonYear() : base(s_MonthLengths) { }

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
            (true, Form), // Kislev
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
            (true, new(325, 11, 10)), // Heswan
        };

        [Fact]
        public override void TryConvertCodeToForm_RotatedCode() =>
            TryConvertCodeToForm_SomeVersions(RotatedForms);
    }

    public sealed class AbundantEmbolismicYear : AnalyzerFacts
    {
        private static readonly int[] s_MonthLengths =
            new int[13] { /*Kislev*/30, 29, 30, 30, 29, /*Nissan*/30, 29, 30, 29, 30, 29, /*Tsiri*/30, 30 };

        public AbundantEmbolismicYear() : base(s_MonthLengths) { }

        public override CodeArray CodeArray0 { get; } = new(4);

        /// <summary>(266, 9, 7)</summary>
        public override QuasiAffineForm Form { get; } = new(266, 9, 7);

        public override IEnumerable<QuasiAffineForm> FormList
        {
            get
            {
                yield return Form0;
                yield return new(1, 4, 3);
                yield return new(9, 4, 3);
                yield return new(4, 9, 1);
                yield return new(5, 9, 7);
                yield return Form;
            }
        }

        public override IEnumerable<CodeArray> CodeArrayList
        {
            get
            {
                yield return CodeArray;
                yield return new(new[] { 3, 2, 2, 2, 3 });
                yield return CodeArray0;
            }
        }

        public override IEnumerable<TroeschMap> TroeschMapList
        {
            get
            {
                yield return new(29, true, 2);
                yield return new(2, false, 1);
            }
        }

        private (bool, QuasiAffineForm?)[] RotatedForms => new (bool, QuasiAffineForm?)[13] {
            (true, Form), // Kislev
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
            (false, null),
            (false, null),
        };

        [Fact]
        public override void TryConvertCodeToForm_RotatedCode() =>
            TryConvertCodeToForm_SomeVersions(RotatedForms);
    }
}

#pragma warning restore CA1034
