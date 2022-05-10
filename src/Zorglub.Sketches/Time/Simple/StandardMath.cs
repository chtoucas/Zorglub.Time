// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    // TODO(code): fix validation of result, is it even meaningful to have it here?

    using Zorglub.Time.Core.Arithmetic;

    // The "standard" operations are also available on the objects themselves.
    // Here, there is the advantage of not having to perform a "Calendar" lookup.
    // Also, the "fast" versions are not actually available on the calendrical
    // objects (to be useful we need also a way to communicate the value of
    // MaxDaysFast, but it would seem awkward to have it in CalendarDate or
    // OrdinalDate).

    /// <summary>
    /// Defines the mathematical operations suitable for use with a given calendar and provides a
    /// base for derived classes.
    /// </summary>
    public sealed partial class StandardMath
    {
        /// <summary>
        /// Represents the ID of the underlying calendar.
        /// <para>This field is a read-only.</para>
        /// </summary>
        private readonly Cuid _cuid;

        /// <summary>
        /// Represents the arithmetic engine.
        /// <para>This field is a read-only.</para>
        /// </summary>
        private readonly FastArithmetic _arithmetic;

        /// <summary>
        /// Called from constructors in derived classes to initialize the <see cref="StandardMath"/>
        /// class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is null.</exception>
        public StandardMath(Calendar calendar!!)
        {
            _cuid = calendar.Id;
            // Ne doit pas pouvoir être changée à moins que cette modification
            // se propage automatiquement au niveau de Calendar (question de
            // cohérence).
            _arithmetic = calendar.Arithmetic;
        }

        /// <summary>
        /// Validates the specified <see cref="Simple.Cuid"/>.
        /// </summary>
        /// <exception cref="ArgumentException">The validation failed.</exception>
        private void ValidateCuid(Cuid cuid, string paramName)
        {
            if (cuid != _cuid) Throw.BadCuid(paramName, _cuid, cuid);
        }
    }

    public partial class StandardMath // CalendarDate
    {
        /// <summary>
        /// Adds a number of days to the day field of the specified date.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="date"/> does not belong to the
        /// underlying calendar.</exception>
        /// <exception cref="OverflowException">The operation would overflow either the range of
        /// supported dates.</exception>
        [Pure]
        public CalendarDate AddDays(CalendarDate date, int days)
        {
            ValidateCuid(date.Cuid, nameof(date));

            var ymd = _arithmetic.AddDays(date.Parts, days);
            return new CalendarDate(ymd, _cuid);
        }

        /// <summary>
        /// Obtains the day after the specified date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the latest supported
        /// date.</exception>
        [Pure]
        public CalendarDate NextDay(CalendarDate date)
        {
            ValidateCuid(date.Cuid, nameof(date));

            var ymd = _arithmetic.NextDay(date.Parts);
            return new CalendarDate(ymd, _cuid);
        }

        /// <summary>
        /// Obtains the day before the specified date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the latest supported
        /// date.</exception>
        [Pure]
        public CalendarDate PreviousDay(CalendarDate date)
        {
            ValidateCuid(date.Cuid, nameof(date));

            var ymd = _arithmetic.PreviousDay(date.Parts);
            return new CalendarDate(ymd, _cuid);
        }

        /// <summary>
        /// Counts the number of days between the two specified dates.
        /// </summary>
        /// <exception cref="ArgumentException">One of the paramaters does not belong to the
        /// underlying calendar.</exception>
        [Pure]
        public int CountDaysBetween(CalendarDate start, CalendarDate end)
        {
            ValidateCuid(start.Cuid, nameof(start));
            ValidateCuid(end.Cuid, nameof(end));

            return _arithmetic.CountDaysBetween(start.Parts, end.Parts);
        }
    }

    public partial class StandardMath // OrdinalDate
    {
        /// <summary>
        /// Adds a number of days to the day field of the specified ordinal date.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="date"/> does not belong to the
        /// underlying calendar.</exception>
        /// <exception cref="OverflowException">The operation would overflow either the range of
        /// supported dates.</exception>
        [Pure]
        public OrdinalDate AddDays(OrdinalDate date, int days)
        {
            ValidateCuid(date.Cuid, nameof(date));

            var ydoy = _arithmetic.AddDays(date.Parts, days);
            return new OrdinalDate(ydoy, _cuid);
        }

        /// <summary>
        /// Obtains the day after the specified ordinal date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the latest supported
        /// date.</exception>
        /// <exception cref="ArgumentException"><paramref name="date"/> does not belong to the
        /// underlying calendar.</exception>
        [Pure]
        public OrdinalDate NextDay(OrdinalDate date)
        {
            ValidateCuid(date.Cuid, nameof(date));

            var ydoy = _arithmetic.NextDay(date.Parts);
            return new OrdinalDate(ydoy, _cuid);
        }

        /// <summary>
        /// Obtains the day before the specified ordinal date.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the latest supported
        /// date.</exception>
        /// <exception cref="ArgumentException"><paramref name="date"/> does not belong to the
        /// underlying calendar.</exception>
        [Pure]
        public OrdinalDate PreviousDay(OrdinalDate date)
        {
            ValidateCuid(date.Cuid, nameof(date));

            var ydoy = _arithmetic.PreviousDay(date.Parts);
            return new OrdinalDate(ydoy, _cuid);
        }

        /// <summary>
        /// Counts the number of days between the two specified ordinal dates.
        /// </summary>
        /// <exception cref="ArgumentException">One of the paramaters does not belong to the
        /// underlying calendar.</exception>
        [Pure]
        public int CountDaysBetween(OrdinalDate start, OrdinalDate end)
        {
            ValidateCuid(start.Cuid, nameof(start));
            ValidateCuid(end.Cuid, nameof(end));

            return _arithmetic.CountDaysBetween(start.Parts, end.Parts);
        }
    }
}
