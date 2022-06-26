// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Discrete
{
    using System.Linq;

    // Cela aurait été plus élégant si on avait distingué TroeschTransformer de
    // son inverse (idem avec TroeschTransformation) mais le code en aurait été
    // compliqué inutilement.

    internal sealed class TroeschTransformer
    {
        private readonly List<TroeschMap> _transformations;
        private readonly List<TroeschMap> _reversedTransformations;

        public TroeschTransformer(TroeschAnalyzer.AnalyzeResult result)
        {
            Debug.Assert(result != null);
            Debug.Assert(result.Completed);

            _transformations = result.Transformations;

            // Inverse transformations: we want to fold from right to left, but
            // Aggregate() only folds from left to right. Simple solution:
            // reverse the list.
            _reversedTransformations = Reverse(_transformations);
        }

        [Pure]
        private static List<T> Reverse<T>(List<T> list)
        {
            var newList = new List<T>(list);
            newList.Reverse();
            return newList;
        }

        //
        // Direct transformations
        //

        [Pure]
        public QuasiAffineForm Apply(QuasiAffineForm form) =>
            _transformations.Aggregate(form, (acc, t) => t.Apply(acc));

        [Pure]
        public QuasiAffineForm Transform(QuasiAffineForm form) =>
            _transformations.Aggregate(form, (acc, t) => t.Transform(acc));

        [Pure]
        public List<QuasiAffineForm> TransformWalkthru(QuasiAffineForm form)
        {
            var seed = new List<QuasiAffineForm> { form };

            return _transformations
                .Aggregate(
                    seed,
                    (acc, t) =>
                    {
                        var range = t.TransformWalkthru(acc[^1]);
                        acc.AddRange(range);
                        return acc;
                    }
                );
        }

        //
        // Inverse transformations
        //

        [Pure]
        public QuasiAffineForm ApplyBack(QuasiAffineForm form) =>
            _reversedTransformations.Aggregate(form, (acc, t) => t.ApplyBack(acc));

        [Pure]
        public QuasiAffineForm TransformBack(QuasiAffineForm form) =>
            _reversedTransformations.Aggregate(form, (acc, t) => t.TransformBack(acc));

        [Pure]
        public List<QuasiAffineForm> TransformBackWalkthru(QuasiAffineForm form)
        {
            var seed = new List<QuasiAffineForm> { form };

            return _reversedTransformations
                .Aggregate(
                    seed,
                    (acc, t) =>
                    {
                        var range = t.TransformBackWalkthru(acc[^1]);
                        acc.AddRange(range);
                        return acc;
                    }
                );
        }
    }
}
