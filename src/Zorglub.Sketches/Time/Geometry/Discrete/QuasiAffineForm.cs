// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Discrete;

// TODO: normalization. Integral form (pendant de CodeArray.Constant)
// + conversion form <-> codeArray (cf. Seed / ToQuasiAffineForm()).
// A, B > 0?

/// <summary>
/// Represents a quasi-affine form.
/// </summary>
public partial record QuasiAffineForm
{
    private readonly int _a;
    private readonly int _b;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuasiAffineForm"/> record.
    /// </summary>
#pragma warning disable IDE1006 // Naming Styles
    public QuasiAffineForm(int A, int B, int Remainder)
    {
        _a = A != 0 ? A : Throw.ArgumentOutOfRange<int>(nameof(A));
        _b = B != 0 ? B : Throw.ArgumentOutOfRange<int>(nameof(B));
        R = Remainder;
    }
#pragma warning restore IDE1006

    // A != 0 means that all forms can be reversed.
    public int A
    {
        get => _a;
        init => _a = value != 0 ? value : Throw.ArgumentOutOfRange<int>(nameof(value));
    }

    // B != 0 means that we can always compute the value of the form.
    public int B
    {
        get => _b;
        init => _b = value != 0 ? value : Throw.ArgumentOutOfRange<int>(nameof(value));
    }

    /// <summary>
    /// Gets the remainder of the current form instance.
    /// </summary>
    public int R { get; init; }

    /// <summary>
    /// Deconstructs the current instance into its components.
    /// </summary>
    public void Deconstruct(out int a, out int b, out int r) => (a, b, r) = (A, B, R);

    /// <summary>
    /// Obtains the inverse of the current form instance.
    /// <code><![CDATA[
    ///   (a, b, r) -> (b, a, b - 1 - r)
    /// ]]></code>
    /// </summary>
    [Pure]
    public QuasiAffineForm Reverse() => new(B, A, B - 1 - R);

    /// <summary>
    /// Computes the value of the current form instance for <paramref name="x"/>.
    /// <code><![CDATA[
    ///   [(a * x + r) / b]
    /// ]]></code>
    /// </summary>
    [Pure]
    public int ValueAt(int x) => MathZ.Divide(A * x + R, B);

    /// <summary>
    /// Divides <paramref name="n"/> using the current form instance.
    /// <code><![CDATA[
    ///   [(b * x + b - 1 - r) / a]
    /// ]]></code>
    /// </summary>
    [Pure]
    public int Divide(int n) => Reverse().ValueAt(n);

    /// <summary>
    /// Divides <paramref name="n"/> using the current form instance and also returns the
    /// remainder in an output parameter.
    /// </summary>
    [Pure]
    public int Divide(int n, out int remainder)
    {
        int x = Reverse().ValueAt(n);
        remainder = checked(n - ValueAt(x));
        return x;
    }

    /// <summary>
    /// Computes the code of the current form instance for <paramref name="x"/>.
    /// </summary>
    // Deux valeurs possibles : [a/b] et [a/b] + 1.
    // En particulier si a < b, les seules valeurs possibles sont 0 et 1.
    [Pure]
    public int CodeAt(int x) => ValueAt(x + 1) - ValueAt(x);
}

// Transformations géométriques élémentaires dans le plan.
// Certaines opérations peuvent échouer : a et b != 0.
public partial record QuasiAffineForm
{
    #region Shear mappings aka transvections

    // Vertical shear (parallel to the y-axis) of parameter m (the shear
    // factor): (x, y) -> (x, y + mx) where m is a given integer.
    // Its inverse is the vertical shear of parameter -m.
    // Horizontal shear (parallel to the x-axis): (x, y) -> (x + my, y).

    // (a, b, r) -> (a + p * b, b, r)
    [Pure]
    public QuasiAffineForm ApplyVerticalShear(int p) => new(A + p * B, B, R);

    // (a, b, r) -> (a, b + q * a, r)
    [Pure]
    public QuasiAffineForm ApplyHozizontalShear(int q) => new(A, B + q * A, R);

    #endregion
    #region Symétrie oblique d'axe y = x/2 et de direction l'axe vertical : (x, y) -> (x, y - x)

    // (a, b, r) -> (b - a, b, b - 1 - r)
    [Pure]
    public QuasiAffineForm ApplyObliqueSymmetry() => new(B - A, B, B - 1 - R);

    #endregion
    #region Symétrie orthogonale (réflexion par rapport à la diagonale y = x) : (x, y) -> (y, x)

    // (a, b, r) -> (b, a, a - 1 - r)
    [Pure]
    public QuasiAffineForm ApplyOrthogonalSymmetry() => new(B, A, A - 1 - R);

    // (a, b, r) -> (b, a, b - 1 - r)
    [Pure]
    public QuasiAffineForm ApplyBackOrthogonalSymmetry() => new(B, A, B - 1 - R);

    #endregion
    #region Translation de vecteur (g, 1) : (x, y) -> (x - g, y - 1)

    // FIXME: On pourrait utiliser aussi (a, b, r - ga + b).
    // https://en.wikipedia.org/wiki/Transformation_matrix#Examples_in_2_dimensions

    // (a, b, r) -> (a, b, r - ga + b)
    // Vérifier b < 0 ne pose pas de problème.
    // Quand g = 0, on pourrait simplement retourner "form".
    [Pure]
    public QuasiAffineForm ApplyTranslation(int x0) => new(A, B, MathZ.Modulo(R - x0 * A, B));

    //// (a, b, r) -> (a, b, r - x0 * a + y0 * b)
    //[Pure]
    //public QuasiAffineForm ApplyTranslation(int x0, int y0) => new(A, B, R - x0 * A + y0 * B);

    #endregion
}
