// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Schemas
{
    using Zorglub.Time.Geometry.Forms;

    public static class GeometricSchemaExtensions
    {
        [Pure]
        public static IGeometricSchema Rebase(
            this IGeometricSchema @this, int daysToTargetEpoch)
        {
            // FIXME: Ne devrait être possible que si la numérotation des années
            // est déjà dans sa forme finale (après un éventuel Transpose()).
            // Autrement dit, tout dépend de MonthForm.
            return daysToTargetEpoch == 0 ? @this
                : new RebasedSchema(@this, daysToTargetEpoch);
        }

        [Pure]
        public static SecondOrderSchema Rebase(
            this SecondOrderSchema @this!!, int daysToTargetEpoch)
        {
            // FIXME: Ne devrait être possible que si la numérotation des années
            // est déjà dans sa forme finale (après un éventuel Transpose()).
            // Autrement dit, tout dépend de MonthForm: ExceptionalMonth = MonthsInYear?
            // On devrait pouvoir faire la même chose avec ThirdOrderSchema, etc.
            return daysToTargetEpoch == 0 ? @this
                : new SecondOrderSchema(@this.YearForm.PatchValue(daysToTargetEpoch), @this.MonthForm);
        }

        // REVIEW: we should add MonthsInYear and ExceptionalMonth to MonthForm.
        [Pure]
        public static IGeometricSchema Transpose(
            this GeometricSchema @this!!,
            int monthsInYear,
            int exceptionalMonth)
        {
            var monthForm = @this.MonthForm;

            // Cas trivial.
            // Par ex., pour le calendrier islamique, le changement de
            // numérotation des mois est élémentaire, il n'affecte pas les
            // années.
            if (exceptionalMonth == monthsInYear)
            {
                return monthForm.Numbering switch
                {
                    MonthFormNumbering.Algebraic => new TransposedSchema(@this, new TrivialRegularizer_()),
                    // Rien à faire : la forme des mois est déjà dans sa bonne version.
                    MonthFormNumbering.Ordinal => @this,
                    _ => Throw.NotSupported<IGeometricSchema>()
                };
            }

            MonthRegularizer regularizer =
                MonthRegularizerFactory.CreateRegularizer(monthForm, monthsInYear, exceptionalMonth);

            return new TransposedSchema(@this, regularizer);
        }

        // Passage de la numérotation algébrique à la numérotation ordinale.
        private sealed class TrivialRegularizer_ : IMonthRegularizer
        {
            public void Regularize(ref int y, ref int m) => m--;

            public void Deregularize(ref int y0, ref int m0) => m0++;
        }
    }
}
