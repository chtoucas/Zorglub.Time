// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Geometry;

using Zorglub.Time.Geometry.Discrete;

public static class TroeschAnalyzerTests
{
    [Fact]
    public static void Constructor_InvalidInput() =>
        Assert.ThrowsAnexn("input", () => new TroeschAnalyzer(null!));

    [Fact]
    public static void Input()
    {
        var input = new CodeArray(987654321);
        var analyzer = new TroeschAnalyzer(input);
        // Act & Assert
        Assert.Equal(input, analyzer.Input);
    }

    [Fact]
    public static void Analysis_BeforeAnalyze()
    {
        var analyzer = new TroeschAnalyzer(new CodeArray(987654321));
        // Act & Assert
        _ = Assert.Throws<InvalidOperationException>(() => analyzer.Analysis);
    }

    [Fact]
    public static void MakeForm_BeforeAnalyze()
    {
        var analyzer = new TroeschAnalyzer(new CodeArray(987654321));
        // Act & Assert
        _ = Assert.Throws<InvalidOperationException>(analyzer.MakeForm);
    }

    [Fact]
    public static void ReverseAnalysis_BeforeAnalyze()
    {
        var analyzer = new TroeschAnalyzer(new CodeArray(987654321));
        // Act & Assert
        _ = Assert.Throws<InvalidOperationException>(analyzer.ReverseAnalysis);
    }

    [Fact]
    public static void Transformer_BeforeAnalyze()
    {
        var analyzer = new TroeschAnalyzer(new CodeArray(987654321));
        // Act & Assert
        _ = Assert.Throws<InvalidOperationException>(() => analyzer.Transformer);
    }

    [Fact]
    public static void Analyze_ConstantIrreducibleCode()
    {
        var input = new CodeArray(987654321, 123);
        var analyzer = new TroeschAnalyzer(input);

        // Act & Assert
        Assert.True(input.Constant);
        Assert.False(input.StrictlyReducible);

        Assert.False(analyzer.Analyzed);
        Assert.True(analyzer.Analyze());
        Assert.True(analyzer.Analyzed);

        var analysis = analyzer.Analysis;
        Assert.NotNull(analysis);
        Assert.True(analysis.Successful);

        Assert.Equal(input, analysis.Input);
        Assert.Equal(input, analysis.Output);

        var codes = analysis.Codes;
        var code = Assert.Single(codes);
        Assert.Equal(input, code);

        var transformations = analysis.Transformations;
        Assert.Empty(transformations);
    }

    [Fact]
    public static void Analyze_IrreducibleCode()
    {
        var input = new CodeArray([1, 3]);
        var analyzer = new TroeschAnalyzer(input);

        // Act & Assert
        Assert.False(input.Constant);
        Assert.False(input.StrictlyReducible);

        Assert.False(analyzer.Analyzed);
        Assert.False(analyzer.Analyze());
        Assert.True(analyzer.Analyzed);

        var analysis = analyzer.Analysis;
        Assert.NotNull(analysis);
        Assert.False(analysis.Successful);

        Assert.Equal(input, analysis.Input);
        Assert.Equal(input, analysis.Output);

        var codes = analysis.Codes;
        var code = Assert.Single(codes);
        Assert.Equal(input, code);

        var transformations = analysis.Transformations;
        Assert.Empty(transformations);
    }
}
