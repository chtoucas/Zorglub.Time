// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    // TODO: add ops for Yedoy.

    // Un "calculateur" ne dépend que du "schéma" calendaire et du champs
    // d'application du calendrier.
    //
    // Pour des raisons techniques, la dépendance vis-à-vis du schéma n'est pas
    // visible ici ; voir p.ex. CustomCalculator. On aurait pu tout de même
    // rendre celle-ci explicite, mais comme ce n'est pas nécessaire, on s'en
    // passe très bien.
    //
    // Quant au champs d'application, celui étant fixe dans le cas présent
    // (calendriers proleptiques), on constate qu'on peut se contenter de
    // vérifier le champs année, ie on peut très bien se passer du "scope".
    //
    // Les méthods AddDays() et CountDaysBetween() ne sont pas incluses ici
    // mais dans Calendar. La raison en est qu'elles sont déjà largement
    // optimisées et qu'elles ne souffrent d'aucun choix arbitraire,
    // contrairement à AddMonths() p.ex. Notons aussi que AddDays() a besoin de
    // connaître Min/MaxDayNumber c-à-d le "scope" sous-jacent ; encore une
    // bonne raison de ne pas inclure cette méthode ici.

    /// <summary>
    /// Defines a calendrical calculator and provides a base for derived classes.
    /// <para>This class focuses on non-standard arithmetical operations.</para>
    /// </summary>
    public abstract partial class CalendricalMath
    {
        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="CalendricalMath"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        protected CalendricalMath(ICalendricalSchema schema, AdditionRules additionRules)
        {
            // FIXME(code): on doit respecter les props schema.Min/MaxYear
            // mais aussi gérer le cas où MinYear < Yemoda.MinYear ou
            // MaxYear > Yemoda.MaxYear. Voir DefaultArithmetic.
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));
            AdditionRules = additionRules;
        }

        /// <summary>
        /// Gets the underlying schema.
        /// </summary>
        protected ICalendricalSchema Schema { get; }

        /// <summary>
        /// Gets the strategy employed to resolve ambiguities.
        /// </summary>
        public AdditionRules AdditionRules { get; }

        /// <summary>
        /// Checks that the specified year does not overflow the range of supported values or not.
        /// </summary>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        protected internal static void CheckYearOverflow(int year)
        {
            if (year < Yemoda.MinYear || year > Yemoda.MaxYear) Throw.DateOverflow();
        }
    }

    public partial class CalendricalMath // Ops on calendrical days
    {
        /// <summary>
        /// Adds a number of years to the year field of the specified date.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        [Pure] public abstract Yemoda AddYears(Yemoda ymd, int years, out int roundoff);

        /// <summary>
        /// Adds a number of months to the month field of the specified date.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        [Pure] public abstract Yemoda AddMonths(Yemoda ymd, int months, out int roundoff);

        /// <summary>
        /// Counts the number of years between the two specified dates.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "end", Justification = "F# & VB.NET End statement.")]
        [Pure] public abstract int CountYearsBetween(Yemoda start, Yemoda end, out Yemoda newStart);

        /// <summary>
        /// Counts the number of months between the two specified dates.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "end", Justification = "F# & VB.NET End statement.")]
        [Pure] public abstract int CountMonthsBetween(Yemoda start, Yemoda end, out Yemoda newStart);

        /// <summary>
        /// Calculates the exact difference between the two specified dates.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        [Pure] public abstract (int years, int months, int days) Subtract(Yemoda left, Yemoda right);
    }

    public partial class CalendricalMath // Ops on calendrical months
    {
        /// <summary>
        /// Adds a number of years to the year field of the specified month.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        [Pure] public abstract Yemo AddYears(Yemo ym, int years);

        /// <summary>
        /// Adds a number of months to the month field of the specified month.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported values.</exception>
        [Pure] public abstract Yemo AddMonths(Yemo ym, int months);

        /// <summary>
        /// Counts the number of years between the two specified months.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "end", Justification = "F# & VB.NET End statement.")]
        [Pure] public abstract int CountYearsBetween(Yemo start, Yemo end);

        /// <summary>
        /// Counts the number of months between the two specified months.
        /// <para>This method does NOT validate its parameters.</para>
        /// </summary>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "end", Justification = "F# & VB.NET End statement.")]
        [Pure] public abstract int CountMonthsBetween(Yemo start, Yemo end);
    }

    public partial class CalendricalMath // Ops on calendrical years
    {
        [Pure] public abstract int AddYears(int y, int years);

        // This method is trivial, only added for completeness.
        [Pure]
        internal static int CountYearsBetween(int start, int end) => end - start;
    }
}
