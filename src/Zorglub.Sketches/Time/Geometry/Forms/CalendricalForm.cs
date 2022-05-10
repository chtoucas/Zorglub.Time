// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Forms
{
    using Zorglub.Time.Core;

    // Autant que faire se peut, on optera plutôt pour des formes dont le
    // reste est >= 0. En effet, dans ce cas, ValueAt(x) pour x >= 0 ne
    // nécessite pas d'en passer par MathOperations.Divide().

    /// <summary>
    /// Represents the (quasi-affine) calendrical form.
    /// </summary>
    public record CalendricalForm
    {
        public static readonly Yemoda Epoch = Yemoda.StartOfYear1;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendricalForm"/>
        /// record.
        /// </summary>
        public CalendricalForm(int A, int B, int Remainder)
        {
            if (A <= 0) Throw.ArgumentOutOfRange(nameof(A));
            if (B <= 0) Throw.ArgumentOutOfRange(nameof(B));

            this.A = A;
            this.B = B;
            this.Remainder = Remainder;
        }

        /// <summary>
        /// Gets the remainder of the current form instance.
        /// </summary>
        public int Remainder { get; init; }

        /// <summary>
        /// Gets or initializes the origin of the current form instance.
        /// </summary>
        public Yemoda Origin { get; init; } = Epoch;

        // We hide A and B, then use more meaningful names in derived records.
        // Also, if we change our mind and decide to make these props public,
        // we do not want to make them init-only props, unless we add param
        // checking here. See QuasiAffineForm.
        private int A { get; }
        private int B { get; }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int a, out int b, out int r) => (a, b, r) = (A, B, Remainder);

        /// <summary>
        /// Obtains the inverse of the current form instance.
        /// </summary>
        [Pure]
        public CalendricalForm Reverse() => new(B, A, B - 1 - Remainder);

        /// <summary>
        /// Computes the value of the current form instance for
        /// <paramref name="x"/>.
        /// </summary>
        //
        // REVIEW: checked
        // On effectue systématiquement les opérations avec des Int64 pour
        // éviter autant que faire se peut les débordements arithmétiques,
        // chose qui peut arriver dès que A est grand. C'est un peu brutal mais
        // on ne se soucie pas des problèmes de perfomance ici.
        [Pure]
        protected int ValueAt(int x) =>
            checked((int)MathZ.Divide(A * (long)x + Remainder, B));

        /// <summary>
        /// Divides <paramref name="n"/> using the current form instance.
        /// </summary>
        [Pure]
        protected int Divide(int n) => Reverse().ValueAt(n);

        /// <summary>
        /// Divides <paramref name="n"/> using the current form instance and
        /// also returns the remainder in an output parameter.
        /// </summary>
        [Pure]
        protected int Divide(int n, out int remainder)
        {
            int x = Reverse().ValueAt(n);
            remainder = checked(n - ValueAt(x));
            return x;
        }

        /// <summary>
        /// Computes the code of the current form instance for <paramref name="x"/>.
        /// </summary>
        [Pure]
        protected int CodeAt(int x) => ValueAt(x + 1) - ValueAt(x);
    }
}
