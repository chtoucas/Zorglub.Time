﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Hemerology;

using System.Linq;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Validation;
using Zorglub.Time.Hemerology.Scopes;

/// <summary>
/// Represents a calendar without a dedicated companion date type and with dates within a range
/// of years.
/// </summary>
public partial class MinMaxYearCalendar : MinMaxYearBasicCalendar, INakedCalendar
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MinMaxYearCalendar"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
    public MinMaxYearCalendar(string name, MinMaxYearScope scope) : base(name, scope)
    {
        PartsAdapter = new PartsAdapter(Schema);

        DatePartsProvider = new DatePartsProvider_(this);
        OrdinalPartsProvider = new OrdinalPartsProvider_(this);
    }

    /// <summary>
    /// Gets the provider for date parts.
    /// </summary>
    public IDateProvider<DateParts> DatePartsProvider { get; }

    /// <summary>
    /// Gets the provider for ordinal parts.
    /// </summary>
    public IDateProvider<OrdinalParts> OrdinalPartsProvider { get; }

    /// <summary>
    /// Gets the adapter for calendrical parts.
    /// </summary>
    protected PartsAdapter PartsAdapter { get; }
}

public partial class MinMaxYearCalendar // Conversions
{
    /// <inheritdoc />
    [Pure]
    public DateParts GetDateParts(DayNumber dayNumber)
    {
        Domain.Validate(dayNumber);
        return PartsAdapter.GetDateParts(dayNumber - Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public DateParts GetDateParts(int year, int dayOfYear)
    {
        Scope.ValidateOrdinal(year, dayOfYear);
        return PartsAdapter.GetDateParts(year, dayOfYear);
    }

    /// <inheritdoc />
    [Pure]
    public OrdinalParts GetOrdinalParts(DayNumber dayNumber)
    {
        Domain.Validate(dayNumber);
        return PartsAdapter.GetOrdinalParts(dayNumber - Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public OrdinalParts GetOrdinalParts(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);
        return PartsAdapter.GetOrdinalParts(year, month, day);
    }
}

public partial class MinMaxYearCalendar // IDateProvider<DayNumber>
{
    /// <inheritdoc/>
    [Pure]
    public IEnumerable<DayNumber> GetDaysInYear(int year)
    {
        YearsValidator.Validate(year);

        int startOfYear = Schema.GetStartOfYear(year);
        int daysInYear = Schema.CountDaysInYear(year);

        return from daysSinceEpoch
               in Enumerable.Range(startOfYear, daysInYear)
               select Epoch + daysSinceEpoch;
    }

    /// <inheritdoc/>
    [Pure]
    public IEnumerable<DayNumber> GetDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);

        int startOfMonth = Schema.GetStartOfMonth(year, month);
        int daysInMonth = Schema.CountDaysInMonth(year, month);

        return from daysSinceEpoch
               in Enumerable.Range(startOfMonth, daysInMonth)
               select Epoch + daysSinceEpoch;
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber GetStartOfYear(int year)
    {
        YearsValidator.Validate(year);
        return Epoch + Schema.GetStartOfYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber GetEndOfYear(int year)
    {
        YearsValidator.Validate(year);
        return Epoch + Schema.GetEndOfYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber GetStartOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return Epoch + Schema.GetStartOfMonth(year, month);
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber GetEndOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return Epoch + Schema.GetEndOfMonth(year, month);
    }
}

public partial class MinMaxYearCalendar // Parts providers
{
    private sealed class DatePartsProvider_ : IDateProvider<DateParts>
    {
        private readonly MinMaxYearCalendar _this;

        public DatePartsProvider_(MinMaxYearCalendar @this)
        {
            Debug.Assert(@this != null);

            _this = @this;
        }

        [Pure]
        public IEnumerable<DateParts> GetDaysInYear(int year)
        {
            // Check arg eagerly.
            _this.YearsValidator.Validate(year);

            return iterator();

            IEnumerable<DateParts> iterator()
            {
                int monthsInYear = _this.Schema.CountMonthsInYear(year);

                for (int m = 1; m <= monthsInYear; m++)
                {
                    int daysInMonth = _this.Schema.CountDaysInMonth(year, m);

                    for (int d = 1; d <= daysInMonth; d++)
                    {
                        yield return new DateParts(year, m, d);
                    }
                }
            }
        }

        [Pure]
        public IEnumerable<DateParts> GetDaysInMonth(int year, int month)
        {
            // Check arg eagerly.
            _this.Scope.ValidateYearMonth(year, month);

            return iterator();

            IEnumerable<DateParts> iterator()
            {
                int daysInMonth = _this.Schema.CountDaysInMonth(year, month);

                for (int d = 1; d <= daysInMonth; d++)
                {
                    yield return new DateParts(year, month, d);
                }
            }
        }

        [Pure]
        public DateParts GetStartOfYear(int year)
        {
            _this.YearsValidator.Validate(year);
            return DateParts.AtStartOfYear(year);
        }

        [Pure]
        public DateParts GetEndOfYear(int year)
        {
            _this.YearsValidator.Validate(year);
            return _this.PartsAdapter.GetDatePartsAtEndOfYear(year);
        }

        [Pure]
        public DateParts GetStartOfMonth(int year, int month)
        {
            _this.Scope.ValidateYearMonth(year, month);
            return DateParts.AtStartOfMonth(year, month);
        }

        [Pure]
        public DateParts GetEndOfMonth(int year, int month)
        {
            _this.Scope.ValidateYearMonth(year, month);
            return _this.PartsAdapter.GetDatePartsAtEndOfMonth(year, month);
        }
    }

    private sealed class OrdinalPartsProvider_ : IDateProvider<OrdinalParts>
    {
        private readonly MinMaxYearCalendar _this;

        public OrdinalPartsProvider_(MinMaxYearCalendar @this)
        {
            Debug.Assert(@this != null);

            _this = @this;
        }

        [Pure]
        public IEnumerable<OrdinalParts> GetDaysInYear(int year)
        {
            // Check arg eagerly.
            _this.YearsValidator.Validate(year);

            return iterator();

            IEnumerable<OrdinalParts> iterator()
            {
                int daysInYear = _this.Schema.CountDaysInYear(year);

                for (int doy = 1; doy <= daysInYear; doy++)
                {
                    yield return new OrdinalParts(year, doy);
                }
            }
        }

        [Pure]
        public IEnumerable<OrdinalParts> GetDaysInMonth(int year, int month)
        {
            // Check arg eagerly.
            _this.Scope.ValidateYearMonth(year, month);

            return iterator();

            IEnumerable<OrdinalParts> iterator()
            {
                int startOfMonth = _this.Schema.CountDaysInYearBeforeMonth(year, month);
                int daysInMonth = _this.Schema.CountDaysInMonth(year, month);

                for (int d = 1; d <= daysInMonth; d++)
                {
                    yield return new OrdinalParts(year, startOfMonth + d);
                }
            }
        }

        [Pure]
        public OrdinalParts GetStartOfYear(int year)
        {
            _this.YearsValidator.Validate(year);
            return OrdinalParts.AtStartOfYear(year);
        }

        [Pure]
        public OrdinalParts GetEndOfYear(int year)
        {
            _this.YearsValidator.Validate(year);
            return _this.PartsAdapter.GetOrdinalPartsAtEndOfYear(year);
        }

        [Pure]
        public OrdinalParts GetStartOfMonth(int year, int month)
        {
            _this.Scope.ValidateYearMonth(year, month);
            return _this.PartsAdapter.GetOrdinalPartsAtStartOfMonth(year, month);
        }

        [Pure]
        public OrdinalParts GetEndOfMonth(int year, int month)
        {
            _this.Scope.ValidateYearMonth(year, month);
            return _this.PartsAdapter.GetOrdinalPartsAtEndOfMonth(year, month);
        }
    }
}
