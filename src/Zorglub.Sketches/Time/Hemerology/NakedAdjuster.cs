// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Validation;
using Zorglub.Time.Hemerology.Scopes;

// Not convinced that this class is useful at all.

// Moyennant quelques changements, cette classe pourrait fonctionner avec
// n'importe quel type date implémentant IFixedDay (c-à-d un IDate puisqu'on
// demande aussi IDateable), mais à la seule condition que celui-ci ne soit
// pas lié à un système de calendriers pluriel. En effet, on serait amener à
// utiliser TDate.FromDayNumber() qui ne peut créer des nouvelles dates que
// dans le calendrier par défaut du système.

/// <summary>
/// Defines an adjuster for <see cref="DayNumber"/>.
/// </summary>
public class NakedAdjuster : IDateAdjuster<DayNumber>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NakedAdjuster"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
    public NakedAdjuster(CalendarScope scope)
    {
        Scope = scope ?? throw new ArgumentNullException(nameof(scope));
    }

    /// <inheritdoc/>
    public CalendarScope Scope { get; }

    /// <summary>
    /// Gets the epoch.
    /// </summary>
    protected DayNumber Epoch => Scope.Epoch;

    /// <summary>
    /// Gets the domain.
    /// </summary>
    protected Range<DayNumber> Domain => Scope.Domain;

    /// <summary>
    /// Gets the schema.
    /// </summary>
    protected ICalendricalSchema Schema => Scope.Schema;

    /// <inheritdoc />
    /// <exception cref="AoorException"><paramref name="date"/> is not within the domain.</exception>
    [Pure]
    public DayNumber GetStartOfYear(DayNumber date)
    {
        Domain.Validate(date, nameof(date));
        int y = Schema.GetYear(date - Epoch, out _);
        var dayNumber = Epoch + Schema.GetStartOfYear(y);
        Domain.Validate(dayNumber, nameof(date));
        return dayNumber;
    }

    /// <inheritdoc />
    /// <exception cref="AoorException"><paramref name="date"/> is not within the domain.</exception>
    [Pure]
    public DayNumber GetEndOfYear(DayNumber date)
    {
        Domain.Validate(date, nameof(date));
        int y = Schema.GetYear(date - Epoch, out _);
        var dayNumber = Epoch + Schema.GetEndOfYear(y);
        Domain.Validate(dayNumber, nameof(date));
        return dayNumber;
    }

    /// <inheritdoc />
    /// <exception cref="AoorException"><paramref name="date"/> is not within the domain.</exception>
    [Pure]
    public DayNumber GetStartOfMonth(DayNumber date)
    {
        Domain.Validate(date, nameof(date));
        Schema.GetDateParts(date - Epoch, out int y, out int m, out _);
        var dayNumber = Epoch + Schema.GetStartOfMonth(y, m);
        Domain.Validate(dayNumber, nameof(date));
        return dayNumber;
    }

    /// <inheritdoc />
    /// <exception cref="AoorException"><paramref name="date"/> is not within the domain.</exception>
    [Pure]
    public DayNumber GetEndOfMonth(DayNumber date)
    {
        Domain.Validate(date, nameof(date));
        Schema.GetDateParts(date - Epoch, out int y, out int m, out _);
        var dayNumber = Epoch + Schema.GetEndOfMonth(y, m);
        Domain.Validate(dayNumber, nameof(date));
        return dayNumber;
    }

    //
    // Adjustments for the core parts
    //

    /// <inheritdoc />
    /// <exception cref="AoorException"><paramref name="date"/> is not within the domain.</exception>
    [Pure]
    public DayNumber AdjustYear(DayNumber date, int newYear)
    {
        Domain.Validate(date, nameof(date));
        Schema.GetDateParts(date - Epoch, out _, out int m, out int d);
        Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));

        return Epoch + Schema.CountDaysSinceEpoch(newYear, m, d);
    }

    /// <inheritdoc />
    /// <exception cref="AoorException"><paramref name="date"/> is not within the domain.</exception>
    [Pure]
    public DayNumber AdjustMonth(DayNumber date, int newMonth)
    {
        Domain.Validate(date, nameof(date));
        Schema.GetDateParts(date - Epoch, out int y, out _, out int d);
        Schema.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        return Epoch + Schema.CountDaysSinceEpoch(y, newMonth, d);
    }

    /// <inheritdoc />
    /// <exception cref="AoorException"><paramref name="date"/> is not within the domain.</exception>
    [Pure]
    public DayNumber AdjustDay(DayNumber date, int newDay)
    {
        Domain.Validate(date, nameof(date));
        Schema.GetDateParts(date - Epoch, out int y, out int m, out _);
        if (newDay < 1
            || (newDay > Schema.MinDaysInMonth
                && newDay > Schema.CountDaysInMonth(y, m)))
        {
            Throw.ArgumentOutOfRange(nameof(newDay));
        }

        return Epoch + Schema.CountDaysSinceEpoch(y, m, newDay);
    }

    /// <inheritdoc />
    /// <exception cref="AoorException"><paramref name="date"/> is not within the domain.</exception>
    [Pure]
    public DayNumber AdjustDayOfYear(DayNumber date, int newDayOfYear)
    {
        Domain.Validate(date, nameof(date));
        int y = Schema.GetYear(date - Epoch, out _);
        Schema.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        return Epoch + Schema.CountDaysSinceEpoch(y, newDayOfYear);
    }
}
