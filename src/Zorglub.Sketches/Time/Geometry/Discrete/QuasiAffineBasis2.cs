// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Discrete
{
    /// <summary>
    /// Formules pour une base de numération quasi-affine d'ordre 2.
    /// </summary>
    public class QuasiAffineBasis2
    {
        protected QuasiAffineBasis2(
            QuasiAffineForm form2,
            QuasiAffineForm form1,
            QuasiAffineForm form0)
        {
            Form2 = form2 ?? throw new ArgumentNullException(nameof(form2));
            Form1 = form1 ?? throw new ArgumentNullException(nameof(form1));
            Form0 = form0 ?? throw new ArgumentNullException(nameof(form0));
        }

        public QuasiAffineForm Form2 { get; }
        public QuasiAffineForm Form1 { get; }
        public QuasiAffineForm Form0 { get; }

        [Pure]
        public int Recompose(int x2, int x1, int x0) =>
            Form2.ValueAt(x2) + Form1.ValueAt(x1) + Form0.ValueAt(x0);

        public void Decompose(int n, out int x2, out int x1, out int x0)
        {
            x2 = Form2.Divide(n, out int r2);
            x1 = Form1.Divide(r2, out int r1);
            x0 = Form0.Divide(r1, out _);
        }
    }
}
