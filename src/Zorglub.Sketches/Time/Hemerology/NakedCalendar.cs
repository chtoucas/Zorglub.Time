// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Validation;
    using Zorglub.Time.Hemerology.Scopes;

    // REVIEW(code): l'absence d'un objet date dédié fait qu'on doit revalider
    // les données à chaque fois.
    // Add
    // - GetFirstMonth() -> MonthParts.
    // - GetMonthsInYear() -> MonthParts.
    // - GetLastMonth() -> MonthParts.

    #region Developer Notes

    // To simplify, NakedCalendar do not implement ICalendar<OrdinalParts>.
    //
    // New props/methods not found in ICalendar<>.
    // - Properties
    //   - Name (could be useful for formatting)
    //   - Min/MaxDateParts
    // - Day info (no date type)
    //   - IsIntercalaryDay(y, m, d)
    //   - IsSupplementaryDay(y, m, d)
    // - Conversions
    //   - GetDateParts(dayNumber)
    //   - GetDateParts(y, doy)
    //   - GetOrdinalParts(dayNumber)
    //   - GetOrdinalParts(y, m, d)
    // - Arithmetic
    //   - AddDays(dayNumber, days)

    #endregion

    /// <summary>
    /// Represents a calendar without a dedicated companion date type and provides a base for
    /// derived classes.
    /// </summary>
    public abstract partial class NakedCalendar : BasicCalendar, ICalendar<DateParts>
    {
        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="NakedCalendar"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        protected NakedCalendar(string name, CalendarScope scope) : base(name, scope)
        {
            PartsAdapter = new PartsAdapter(Schema);
        }

        /// <summary>
        /// Gets the adapter for calendrical parts.
        /// </summary>
        protected PartsAdapter PartsAdapter { get; }
    }

    public partial class NakedCalendar // Factories, conversions
    {
        /// <summary>
        /// Obtains the current date on this machine.
        /// </summary>
        /// <exception cref="AoorException">Today is not within the calendar boundaries.</exception>
        [Pure]
        public DateParts Today() => GetDateParts(DayNumber.Today());

        /// <summary>
        /// Obtains the date parts for the specified day number.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by this calendar.</exception>
        [Pure]
        public DateParts GetDateParts(DayNumber dayNumber)
        {
            Domain.Validate(dayNumber);
            return PartsAdapter.GetDateParts(dayNumber - Epoch);
        }

        /// <summary>
        /// Obtains the date parts for the specified ordinal date.
        /// </summary>
        /// <exception cref="AoorException">The ordinal date is either invalid or outside the range
        /// of supported dates.</exception>
        [Pure]
        public DateParts GetDateParts(int year, int dayOfYear)
        {
            Scope.ValidateOrdinal(year, dayOfYear);
            return PartsAdapter.GetDateParts(year, dayOfYear);
        }

        /// <summary>
        /// Obtains the ordinal date parts for the specified day number.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by this calendar.</exception>
        [Pure]
        public OrdinalParts GetOrdinalParts(DayNumber dayNumber)
        {
            Domain.Validate(dayNumber);
            return PartsAdapter.GetOrdinalParts(dayNumber - Epoch);
        }

        /// <summary>
        /// Obtains the ordinal date parts for the specified date.
        /// </summary>
        /// <exception cref="AoorException">The date is either invalid or outside the range of
        /// supported dates.</exception>
        [Pure]
        public OrdinalParts GetOrdinalParts(int year, int month, int day)
        {
            Scope.ValidateYearMonthDay(year, month, day);
            return PartsAdapter.GetOrdinalParts(year, month, day);
        }
    }

    public partial class NakedCalendar // Dates in a given year or month
    {
        /// <inheritdoc />
        [Pure] public abstract IEnumerable<DateParts> GetDaysInYear(int year);

        /// <inheritdoc />
        [Pure] public abstract IEnumerable<DateParts> GetDaysInMonth(int year, int month);

        /// <inheritdoc />
        [Pure] public abstract DateParts GetStartOfYear(int year);

        /// <inheritdoc />
        [Pure] public abstract DateParts GetEndOfYear(int year);

        /// <inheritdoc />
        [Pure] public abstract DateParts GetStartOfMonth(int year, int month);

        /// <inheritdoc />
        [Pure] public abstract DateParts GetEndOfMonth(int year, int month);
    }

    public partial class NakedCalendar // Arithmetic
    {
        // REVIEW(api): supprimer AddDays().

        /// <summary>
        /// Adds a number of days to the specified day number.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You only need this method if you do NOT intend to pass the result to another calendar
        /// method, and you want to be sure that the result is valid for the current calendar.
        /// Otherwise always prefer the simpler and faster plain addition: dayNumber + days.
        /// </para>
        /// </remarks>
        /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
        /// values supported by this calendar.</exception>
        /// <exception cref="OverflowException">The calculation would overflow the range of
        /// supported dates.</exception>
        [Pure]
        public DayNumber AddDays(DayNumber dayNumber, int days)
        {
            // Ajouter un nombre de jours à un dayNumber est toujours une
            // opération légitime et se fait en dehors de tout calendrier. Ici,
            // il s'agit d'une version stricte de DayNumber.PlusDays() qui
            // garantit que le résultat reste dans les limites du calendrier.
            // Cependant comme la plupart du temps on passe immédiatement le
            // résultat à GetDateParts() qui valide le dayNumber, on peut souvent
            // ignorer cette méthode et se contenter d'écrire
            // > GetDateParts(dayNumber + days);
            //
            // On pourrait ne pas valider dayNumber mais seulement le résultat,
            // cependant pour rester cohérent avec le reste de cette classe,
            // on valide à l'entrée et à la sortie.
            Domain.Validate(dayNumber);
            var dayNumberOut = dayNumber + days;
            Domain.CheckOverflow(dayNumberOut);
            return dayNumberOut;
        }
    }
}
