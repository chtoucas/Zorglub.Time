// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Facts.Geometry;

using System.Linq;

using Zorglub.Time.Geometry.Discrete;

public abstract class AnalyzerFacts : AnalyzerBasicFacts
{
    protected AnalyzerFacts(int[] codes) : base(codes) { }

    public abstract IEnumerable<QuasiAffineForm> FormList { get; }
    public abstract IEnumerable<CodeArray> CodeArrayList { get; }
    public abstract IEnumerable<TroeschMap> TroeschMapList { get; }

    // We didn't put this method in AbstractAnalyzerBasicTests on purpose.
    // We don't want to force us to test the rotated codes.
    // NB: If we get a warning xUnit1013 (Public method should be marked as
    // test), it simply means that one of the derived classes did not mark
    // the overriden method with the attribute Fact.
    public abstract void TryConvertCodeToForm_RotatedCode();

    [Fact]
    public void Analysis_Prop()
    {
        var analyzer = new TroeschAnalyzer(CodeArray);
        analyzer.Analyze();
        var analysis = analyzer.Analysis;
        // Act & Assert
        Assert.Equal(CodeArrayList, analysis.Codes);
        Assert.Equal(TroeschMapList, analysis.Transformations);
    }

    [Fact]
    public void ReverseAnalysis()
    {
        var analyzer = new TroeschAnalyzer(CodeArray);
        analyzer.Analyze();
        // Act
        var forms = analyzer.ReverseAnalysis();
        // Assert
        Assert.Equal(FormList, forms);
    }

    [Fact]
    public void Transformer_Prop_TransformWalkthru()
    {
        var analyzer = new TroeschAnalyzer(CodeArray);
        analyzer.Analyze();
        // Act
        var forms = analyzer.Transformer.TransformWalkthru(Form);
        // Assert
        Assert.Equal(FormList.Reverse(), forms);
    }

    [Fact]
    public void Transformer_Prop_TransformBackWalkthru()
    {
        var analyzer = new TroeschAnalyzer(CodeArray);
        analyzer.Analyze();
        // Act
        var forms = analyzer.Transformer.TransformBackWalkthru(Form0);
        // Assert
        Assert.Equal(FormList, forms);
    }
}
