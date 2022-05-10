// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Geometry;

using Zorglub.Time.Geometry.Discrete;
using Zorglub.Time.Geometry.Forms;

public abstract partial class AnalyzerBasicFacts
{
    private readonly int[] _sums;

    protected AnalyzerBasicFacts(int[] codes)
    {
        CodeArray = new(codes);

        _sums = ArrayHelpers.ConvertToCumulativeArray(codes);
    }

    public CodeArray CodeArray { get; }
    public abstract CodeArray CodeArray0 { get; }

    public abstract QuasiAffineForm Form { get; }
    public QuasiAffineForm Form0 => CodeArray0.ToQuasiAffineForm();

    //
    // Helpers.
    //

    protected void Form_Equals<TForm>(TForm form) where TForm : CalendricalForm
    {
        var (a, b, r) = form;
        // Assert
        Assert.Equal(new QuasiAffineForm(a, b, r), Form);
    }

    protected void TryConvertCodeToForm_AllVersions(QuasiAffineForm[] forms!!)
    {
        if (forms.Length != CodeArray.Count) { throw new ArgumentException(null, nameof(forms)); }

        // For i = 0, see TryConvertCodeToForm() below.
        Assert.Equal(Form, forms[0]);

        for (int i = 1; i < CodeArray.Count; i++)
        {
            TryConvertCodeToForm_AtIndex(CodeArray, i, true, forms[i]);
        }
    }

    protected void TryConvertCodeToForm_SomeVersions((bool Success, QuasiAffineForm? FormE)[] results!!)
    {
        if (results.Length != CodeArray.Count) { throw new ArgumentException(null, nameof(results)); }

        // For i = 0, see TryConvertCodeToForm() below.
        Assert.True(results[0].Success);
        Assert.Equal(Form, results[0].FormE);

        for (int i = 1; i < CodeArray.Count; i++)
        {
            TryConvertCodeToForm_AtIndex(CodeArray, i, results[i].Success, results[i].FormE);
        }
    }

    private static void TryConvertCodeToForm_AtIndex(
        CodeArray code,
        int i,
        bool success,
        QuasiAffineForm? formE)
    {
        Debug.Assert(i > 0);
        Debug.Assert(i < code.Count);

        var newCode = code.Rotate(i);
        // Act
        var result = TroeschAnalyzer.TryConvertCodeToForm(newCode, out var formA);
        // Assert
        if (result != success)
        {
            Assert.Fails($"Index {i}: '{result}' instead of '{success}'.");
        }
        if (success)
        {
            Assert.True(newCode.StrictlyReducible);
            Assert.Equal((i, formE), (i, formA));
            Assert.NotNull(formA);

            for (int c = 0; c < newCode.Count; c++)
            {
                Assert.Equal((i, c, newCode[c]), (i, c, formA!.CodeAt(c)));
            }
        }
    }
}

// We don't test a CalendricalForm directly but through Form.
public partial class AnalyzerBasicFacts
{
    [Fact]
    public void Form_CodeAt()
    {
        for (int i = 0; i < CodeArray.Count; i++)
        {
            // MonthForm.CountDaysInMonth()
            // YearForm.CountDaysInYear()
            // CenturyForm.CountDaysInCentury()
            Assert.Equal((i, CodeArray[i]), (i, Form.CodeAt(i)));
        }
    }

    [Fact]
    public void Form_ValueAt()
    {
        for (int i = 0; i < CodeArray.Count; i++)
        {
            // MonthForm.CountDaysInYearBeforeMonth()
            // YearForm.GetStartOfYear()
            // CenturyForm.GetStartOfCentury()
            Assert.Equal((i, _sums[i]), (i, Form.ValueAt(i)));
        }
    }

    [Fact]
    public void Form_Divide()
    {
        for (int i = 0; i < CodeArray.Count; i++)
        {
            // MonthForm.GetMonth()
            // YearForm.GetYear()
            // CenturyForm.GetCentury()
            Assert.Equal(i, Form.Divide(_sums[i], out int d0));
            Assert.Equal((i, 0), (i, d0));
        }
    }
}

// Core methods.
public partial class AnalyzerBasicFacts
{
    [Fact]
    public void TryConvertCodeToForm()
    {
        Assert.True(CodeArray.StrictlyReducible);
        Assert.True(TroeschAnalyzer.TryConvertCodeToForm(CodeArray, out var formA));
        Assert.Equal(Form, formA);
    }

    [Fact]
    public void Analyze()
    {
        var analyzer = new TroeschAnalyzer(CodeArray);

        // Act & Assert
        Assert.False(analyzer.Analyzed);
        Assert.True(analyzer.Analyze());
        Assert.True(analyzer.Analyzed);

        var analysis = analyzer.Analysis;
        Assert.True(analysis.Successful);
        Assert.Equal(CodeArray, analysis.Input);
        Assert.Equal(CodeArray0, analysis.Output);

        var codes = analysis.Codes;
        // NB: we suppose that CodeArray is not constant, otherwise
        // codes.Count can be equal to 1.
        Assert.True(codes.Count > 0);
        Assert.Equal(CodeArray, codes[0]);
        Assert.Equal(CodeArray0, codes[^1]);
    }

    [Fact]
    public void MakeForm()
    {
        var analyzer = new TroeschAnalyzer(CodeArray);
        analyzer.Analyze();
        // Act & Assert
        Assert.Equal(Form, analyzer.MakeForm());
    }

    [Fact]
    public void Transformer()
    {
        var analyzer = new TroeschAnalyzer(CodeArray);
        analyzer.Analyze();
        var transformer = analyzer.Transformer;

        // Act & Assert
        Assert.Equal(Form0, transformer.Apply(Form));
        Assert.Equal(Form0, transformer.Transform(Form));

        Assert.Equal(Form, transformer.ApplyBack(Form0));
        Assert.Equal(Form, transformer.TransformBack(Form0));
    }
}
