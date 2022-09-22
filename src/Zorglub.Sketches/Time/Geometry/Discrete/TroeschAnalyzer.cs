// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Discrete;

/// <summary>
/// Defines a Troesch analyzer.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class TroeschAnalyzer
{
    private AnalyzeResult? _result;

    /// <summary>
    /// Initializes a new instance of the <see cref="TroeschAnalyzer"/> class with the specified
    /// code.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="input"/> is null.</exception>
    public TroeschAnalyzer(CodeArray input)
    {
        Input = input ?? throw new ArgumentNullException(nameof(input));
    }

    public CodeArray Input { get; }

    public bool Analyzed => _result?.Completed ?? false;

    // InvalidOperationException
    public TroeschAnalysis Analysis => new(Result);

    // InvalidOperationException
    internal TroeschTransformer Transformer => new(Result);

    // InvalidOperationException
    private AnalyzeResult Result => _result ?? Throw.InvalidOperation<AnalyzeResult>();

    /// <summary>
    /// Returns true if <paramref name="input"/> is the code of a segment of a discrete straight
    /// line; otherwise returns false.
    /// </summary>
    [Pure]
    public static bool TryConvertCodeToForm(
        CodeArray input,
        [NotNullWhen(true)] out QuasiAffineForm? form)
    {
        var analyzer = new TroeschAnalyzer(input);
        form = analyzer.Analyze() ? analyzer.MakeForm() : null;
        return analyzer.Result.Output.Constant;
    }
}

public partial class TroeschAnalyzer // Analysis
{
    // Only executed once. Further calls do nothing.
    // No attr [Pure], the result is optional, see Analysis.Successful.
    public bool Analyze()
    {
        if (_result is null)
        {
            (_result = AnalyzeCore()).Completed = true;
        }

        return Result.Output.Constant;
    }

    [Pure]
    private AnalyzeResult AnalyzeCore()
    {
        return AnalyzeRecursively(Input, new AnalyzeResult(Input));

        [Pure]
        static AnalyzeResult AnalyzeRecursively(CodeArray code, AnalyzeResult result)
        {
            if (code.StrictlyReducible == false) { return result; }

            // Shear mapping.
            // NB: boolArr.Count < code.Count, this is not a never ending loop!
            var boolArr = code.ToBoolArray();

            // Oblique symmetry (complement)?
            bool negated = !boolArr.IsTrueIsolated();
            var newBoolArr = negated ? boolArr.Negate() : boolArr;

            // Orthogonal symmetry, then translation.
            var newCode = newBoolArr.Slice().RemoveMinorExternals(out int g);

            result.Append(newCode, new TroeschMap(code.Min, negated, g));

            return AnalyzeRecursively(newCode, result);
        }
    }

    internal sealed class AnalyzeResult
    {
        private readonly List<CodeArray> _codes;
        private readonly List<TroeschMap> _transformations = new();

        public AnalyzeResult(CodeArray input)
        {
            Debug.Assert(input != null);

            _codes = new List<CodeArray> { input };
        }

        // Returns true when the analysis has been completed; otherwise return false.
        public bool Completed { get; set; }

        // Do NOT use "Codes" here (in case Completed = false).
        public CodeArray Input => _codes[0];

        // Properties below should only be called when Completed = true.
        // This is not mandatory, but doing otherwise will give wrong results.

        // If Input is not strictly reducible, Output = Input!
        public CodeArray Output
        {
            get { Debug.Assert(Completed); return Codes[^1]; }
        }

        public List<CodeArray> Codes
        {
            get { Debug.Assert(Completed); return _codes; }
        }

        public List<TroeschMap> Transformations
        {
            get { Debug.Assert(Completed); return _transformations; }
        }

        public void Append(CodeArray code, TroeschMap transformation)
        {
            Debug.Assert(code != null);
            Debug.Assert(transformation != null);

            _codes.Add(code);
            _transformations.Add(transformation);
        }
    }
}

public partial class TroeschAnalyzer // Reversed analysis
{
    // InvalidOperationException
    [Pure]
    public IReadOnlyList<QuasiAffineForm> ReverseAnalysis()
    {
        var output = Result.Output;
        if (output.Constant == false) Throw.InvalidOperation();

        return Transformer.TransformBackWalkthru(output.ToQuasiAffineForm()).AsReadOnly();
    }

    // InvalidOperationException
    [Pure]
    public QuasiAffineForm MakeForm()
    {
        var output = Result.Output;
        if (output.Constant == false) Throw.InvalidOperation();

        return Transformer.ApplyBack(output.ToQuasiAffineForm());
    }
}
