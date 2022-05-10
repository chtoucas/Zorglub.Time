// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

//#define TROESCH_MAP_SYMMETRY_THEN_TRANSLATION

namespace Zorglub.Time.Geometry.Discrete
{
    // TODO: Shear, Translate > 0. Inverse Translate params.
    // Complement could be optional and only depend on the form.

    public sealed partial record TroeschMap(int Shear, bool Complement, int Translate);

    // Transform/Apply.
    public partial record TroeschMap
    {
        [Pure]
        public QuasiAffineForm Apply(QuasiAffineForm form!!)
        {
            var (a, b, r) = form;

            if (Complement)
            {
                var B = (Shear + 1) * b - a;
#if TROESCH_MAP_SYMMETRY_THEN_TRANSLATION
                // Orthogonal symmetry then translation.
                return new(b, B, MathZ.Modulo(r - b + Translate * b, B));
#else
                // Translation then orthogonal symmetry.
                return new(b, B, B - 1 - MathZ.Modulo(-1 - r - Translate * a, b));
#endif
            }
            else
            {
                var B = a - Shear * b;
#if TROESCH_MAP_SYMMETRY_THEN_TRANSLATION
                // Orthogonal symmetry then translation.
                return new(b, B, MathZ.Modulo(a - 1 - r, B));
#else
                // Translation then orthogonal symmetry.
                return new(b, B, B - 1 - MathZ.Modulo(r + Translate * a, b));
#endif
            }
        }

        // Geometric Apply().
        [Pure]
        internal QuasiAffineForm Transform(QuasiAffineForm form)
        {
            Debug.Assert(form != null);

            return Complement ? Transform4() : Transform3();

            QuasiAffineForm Transform3() =>
                form.ApplyVerticalShear(-Shear)
                    .ApplyTranslation(-Translate)
                    .ApplyOrthogonalSymmetry();

            QuasiAffineForm Transform4() =>
                form.ApplyVerticalShear(-Shear)
                    .ApplyObliqueSymmetry()
                    .ApplyTranslation(-Translate)
                    .ApplyOrthogonalSymmetry();
        }

        // Transform() detailed.
        [Pure]
        internal QuasiAffineForm[] TransformWalkthru(QuasiAffineForm form)
        {
            Debug.Assert(form != null);

            return Complement ? Transform4() : Transform3();

            QuasiAffineForm[] Transform3()
            {
                var form1 = form.ApplyVerticalShear(-Shear);

                return new QuasiAffineForm[2]
                {
                    form1,
                    form1.ApplyTranslation(-Translate).ApplyOrthogonalSymmetry()
                };
            }

            QuasiAffineForm[] Transform4()
            {
                var form1 = form.ApplyVerticalShear(-Shear);
                var form2 = form1.ApplyObliqueSymmetry();

                return new QuasiAffineForm[3]
                {
                    form1,
                    form2,
                    form2.ApplyTranslation(-Translate).ApplyOrthogonalSymmetry()
                };
            }
        }
    }

    // Inverse transformation.
    public partial record TroeschMap
    {
        [Pure]
        public QuasiAffineForm ApplyBack(QuasiAffineForm form!!)
        {
            var (a, b, r) = form;

            var rem = MathZ.Modulo(b - 1 - r - Translate * b, a);

            return Complement ? new(Shear * a + a - b, a, a - 1 - rem)
                : new(Shear * a + b, a, rem);
        }

        // Geometric ApplyBack().
        [Pure]
        internal QuasiAffineForm TransformBack(QuasiAffineForm form)
        {
            Debug.Assert(form != null);

            return Complement ? TransformBack4() : TransformBack3();

            QuasiAffineForm TransformBack3() =>
                form.ApplyBackOrthogonalSymmetry()
                    .ApplyTranslation(Translate)
                    .ApplyVerticalShear(Shear);

            QuasiAffineForm TransformBack4() =>
                 form.ApplyBackOrthogonalSymmetry()
                    .ApplyTranslation(Translate)
                    .ApplyObliqueSymmetry()
                    .ApplyVerticalShear(Shear);
        }

        // TransformBack() detailed.
        [Pure]
        internal QuasiAffineForm[] TransformBackWalkthru(QuasiAffineForm form)
        {
            Debug.Assert(form != null);

            return Complement ? TransformBack4() : TransformBack3();

            QuasiAffineForm[] TransformBack3()
            {
                var form1 = form.ApplyBackOrthogonalSymmetry().ApplyTranslation(Translate);

                return new QuasiAffineForm[2]
                {
                    form1,
                    form1.ApplyVerticalShear(Shear)
                };
            }

            QuasiAffineForm[] TransformBack4()
            {
                var form1 = form.ApplyBackOrthogonalSymmetry().ApplyTranslation(Translate);
                var form2 = form1.ApplyObliqueSymmetry();

                return new QuasiAffineForm[3]
                {
                    form1,
                    form2,
                    form2.ApplyVerticalShear(Shear)
                };
            }
        }
    }
}
