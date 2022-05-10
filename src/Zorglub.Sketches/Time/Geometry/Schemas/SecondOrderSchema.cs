// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Schemas
{
    using Zorglub.Time.Geometry.Forms;

    // There are many "empirical" formulae to compute a Julian Day from a date
    // and vice-versa. Here we present formulae "rigorously" obtained by
    // techniques originating from digital geometry [1]. They are not always
    // applicable but, when the calendar is sufficiently regular, they offer
    // a pretty straightforward algorithm. The results do not always give the
    // simplest formulae (or the most efficient) but we surely gain some
    // insights and above all confidence in the code.
    //
    // Advantages:
    // - algorithmic construction
    // - purely computational (no branching)
    // - only use integral numbers
    // Disadvantages:
    // - more opportunities to overflow especially for calendars with long
    //   cycles (that's the main defect)
    // - there are simpler alternatives
    // - only apply to sufficiently regular calendars
    // Possible use-case:
    // - check another calendar implementation
    //
    // Concerning the implementation.
    // Beware, the formulae may overflow, they are not production-ready,
    // temporary conversion to a long might be necessary
    // (NB: CheckForOverflowUnderflow is set to true in this project),
    // this is especially true with long cycles (see DaysPerCycle below).
    // All methods work with negative years.
    //
    // Description
    // -----------
    //
    // Cas le plus simple où le jour bissextil se trouve en fin d'année.
    //
    // Formes quasi-affines:
    //   (DaysPerCycle, YearsPerCycle, Remainder)
    //   (A, B, R)
    //   (1, 1, -1)
    //
    // Première forme (a, b, r) encode le nombre de jours écoulés depuis l'epoch
    // du calendrier jusqu'au début de l'année.
    //   a = nombre de jours dans un cycle complet
    //   b = longueur du cycle en années
    //   r = dépend de la répartition des années bissextiles dans un cycle
    //
    // La deuxième forme encode le nombre de jours dans l'année avant le début
    // d'un mois, elle dépend uniquement de la partition des jours en mois dans
    // une année. Dans le cadre du calendrier musulman, on est dans le cas
    // simple où le jour bissextil est placé en fin d'année.
    //
    // La dernière forme permet juste de passer d'une numérotation algébrique
    // des jours d'un mois à la numérotation ordinale (commençant à 1).
    //
    // L'inverse de la première forme permet de calculer l'année correspondant
    // à un décompte de jours.
    //
    // L'inverse de la deuxième forme permet d'obtenir le mois en fonction du
    // jour dans l'année.
    //
    // L'inverse de la troisième forme permet de passer de la numérotation
    // standard des jours d'un mois à une numérotation algébrique.
    //
    // Avertissements
    // --------------
    //
    // Les formules obtenues sont absconses et ne sont pas nécessairement
    // meilleures. En particulier, les constantes multiplicatives peuvent être
    // trop grandes pour que le résultat tienne dans un entier 32-bit même quand
    // l'année est dans l'intervalle défini dans CalendricalConstants.
    //
    // References
    // ----------
    //
    // [1] "Droites discrètes et calendriers", 1998, A.Troesch.
    //     There are typos in the article. Also, now it seems that there are
    //     better algorithms for recognizing discrete straight lines (DSL).
    // [2] "La saga des calendriers", 1998, J.Lefort.

    /// <summary>
    /// Represents a geometric schema of order 2.
    /// <para>Distribution of leap years fully encoded by YearForm.</para>
    /// <para><i>Intercalary days positioned at the end of a leap year.</i></para>
    /// </summary>
    /// <remarks>
    /// <para>Convient aux calendriers julien et musulman. Devrait convenir (je
    /// n'ai pas vérifié) aux calendriers copte et égyptien.</para>
    /// <para>Il s'agit tout simplement des formules pour une base de numération
    /// quasi-affine d'ordre 2 .</para>
    /// </remarks>
    public sealed partial class SecondOrderSchema : GeometricSchema
    {
        public SecondOrderSchema(YearForm yearForm, MonthForm monthForm)
            : base(yearForm, monthForm) { }

        /// <inheritdoc />
        [Pure]
        public sealed override int CountDaysSinceEpoch(int y, int m, int d) =>
            // We recover the standard formula:
            //   GetStartOfYear(y) + CountDaysInYearBeforeMonth(y, m) + (d -1)
            YearForm.GetStartOfYear(y)
            + MonthForm.CountDaysInYearBeforeMonth(m)
            + DayForm.CountDaysInMonthBeforeDay(d);

        /// <inheritdoc />
        public sealed override void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
        {
            // We can recognize the standard algorithm.
            y = YearForm.GetYear(daysSinceEpoch, out int d0y);
            m = MonthForm.GetMonth(d0y, out int d0);
            d = DayForm.GetDay(d0);
        }
    }

    public partial class SecondOrderSchema
    {
        /// <summary>
        /// Code de la première forme quasi-affine (YearForm).
        /// </summary>
        [Pure]
        public int CountDaysInYear(int y)
        {
            if (!YearForm.Normal) Throw.InvalidOperation();

            return YearForm.CountDaysInYear(y);
        }

        /// <summary>
        /// Valeur de l'inverse de la première forme quasi-affine (YearForm).
        /// </summary>
        [Pure]
        public int GetYear(int daysSinceEpoch, out int doy)
        {
            if (!YearForm.Normal) Throw.InvalidOperation();

            int y = YearForm.GetYear(daysSinceEpoch, out int d0y);
            doy = 1 + d0y;
            return y;
        }

        /// <summary>
        /// Valeur de la première forme quasi-affine (YearForm).
        /// <para>Retourne le nombre de jours entre le 01/01/0001 et le début de
        /// l'année.</para>
        /// </summary>
        //
        // La plupart du temps, il est plus simple de compter le nombre de
        // jours depuis l'epoch en prétendant que toutes les années sont
        // communes, puis d'ajouter le nombre de jours bissextils depuis
        // l'epoch du calendrier.
        [Pure]
        public int GetStartOfYear(int y)
        {
            if (!YearForm.Normal) Throw.InvalidOperation();

            return YearForm.GetStartOfYear(y);
        }
    }
}
