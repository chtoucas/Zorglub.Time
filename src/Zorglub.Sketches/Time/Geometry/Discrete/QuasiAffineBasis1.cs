// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Discrete;

/// <summary>
/// Formules pour une base de numération quasi-affine d'ordre 1.
/// </summary>
public class QuasiAffineBasis1
{
    protected QuasiAffineBasis1(QuasiAffineForm form1, QuasiAffineForm form0)
    {
        Form1 = form1 ?? throw new ArgumentNullException(nameof(form1));
        Form0 = form0 ?? throw new ArgumentNullException(nameof(form0));
    }

    public QuasiAffineForm Form1 { get; }
    public QuasiAffineForm Form0 { get; }

    [Pure]
    public int Recompose(int x1, int x0) => Form1.ValueAt(x1) + Form0.ValueAt(x0);

    public void Decompose(int n, out int x1, out int x0)
    {
        x1 = Form1.Divide(n, out int r1);
        x0 = Form0.Divide(r1, out _);
    }
}
