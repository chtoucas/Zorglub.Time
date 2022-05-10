// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Discrete
{
    public sealed class TroeschAnalysis
    {
        internal TroeschAnalysis(TroeschAnalyzer.AnalyzeResult result)
        {
            Debug.Assert(result != null);
            Debug.Assert(result.Completed);

            Input = result.Input;
            Output = result.Output;

            Codes = result.Codes.AsReadOnly();
            Transformations = result.Transformations.AsReadOnly();
        }

        public bool Successful => Output.Constant;

        public CodeArray Input { get; }

        // If Input is not strictly reducible, Output = Input!
        public CodeArray Output { get; }

        public IReadOnlyList<CodeArray> Codes { get; }

        public IReadOnlyList<TroeschMap> Transformations { get; }
    }
}
