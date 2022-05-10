// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

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
        // Arrange
        var input = new CodeArray(987654321);
        var analyzer = new TroeschAnalyzer(input);
        // Act & Assert
        Assert.Equal(input, analyzer.Input);
    }

    [Fact]
    public static void Analysis_BeforeAnalyze()
    {
        // Arrange
        var analyzer = new TroeschAnalyzer(new CodeArray(987654321));
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => analyzer.Analysis);
    }

    [Fact]
    public static void MakeForm_BeforeAnalyze()
    {
        // Arrange
        var analyzer = new TroeschAnalyzer(new CodeArray(987654321));
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => analyzer.MakeForm());
    }

    [Fact]
    public static void ReverseAnalysis_BeforeAnalyze()
    {
        // Arrange
        var analyzer = new TroeschAnalyzer(new CodeArray(987654321));
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => analyzer.ReverseAnalysis());
    }

    [Fact]
    public static void Transformer_BeforeAnalyze()
    {
        // Arrange
        var analyzer = new TroeschAnalyzer(new CodeArray(987654321));
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => analyzer.Transformer);
    }

    [Fact]
    public static void Analyze_ConstantIrreducibleCode()
    {
        // Arrange
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
        Assert.Equal(1, codes.Count);
        Assert.Equal(input, codes[0]);

        var transformations = analysis.Transformations;
        Assert.Equal(0, transformations.Count);
    }

    [Fact]
    public static void Analyze_IrreducibleCode()
    {
        // Arrange
        var input = new CodeArray(new int[] { 1, 3 });
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
        Assert.Equal(1, codes.Count);
        Assert.Equal(input, codes[0]);

        var transformations = analysis.Transformations;
        Assert.Equal(0, transformations.Count);
    }
}
