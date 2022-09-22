// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Prototypes;

using Zorglub.Bulgroz.Prototypes;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Validation;

// TODO(code): the props Arithmetic and PreValidator are too simplistic.
// Arithmetic:
// CountMonthsBetween() is a subtraction of two CountMonthsSinceEpoch(),
// we just need to compute CountMonthsSince the "other" date.
//
//    return end.Year == start.Year ? end.Month - start.Month
//        : start < end ? CountCore(start, end)
//        : -CountCore(end, start);
//
//    int CountCore(Yemo start, Yemo end)
//    {
//        Debug.Assert(start < end);
//
//        start.Parts.Unpack(out int y0, out int m0);
//        end.Parts.Unpack(out int y1, out int m1);
//
//        // This can certainly be optimized for calendars for which we
//        // know the number of months in a leap-cycle. For instance, this
//        // is the case of Lunisolar calendars.
//        // Of course, if the calendar is regular, the loop can be
//        // replaced by a simple formula (see RegularMathXXX).
//        int months = Schema.CountMonthsInYear(y0) - m0;
//        for (int y = y0 + 1; y < y1; y++)
//        {
//            months += Schema.CountMonthsInYear(y);
//        }
//        months += m1;
//        return months;
//    }
//
// Cache: right now, it's just a quick test. It might not work, cache size
// & co are specific to NodaTime.
// GetMonth(), DisableCustomGetMonth, is it worth it?
//
// For GetStartOfYear(), we could first check that there is a nearby year
// already in the cache. Pre-compute the values at the start of each century.
//
// Migrate ZCalendar to use the ICalendricalKernel and a prototype which
// includes some of the simple possible optimizations (see comments in
// PrototypalSchema)?
//
// GeometricPrototype, based on quasi-affine forms?
// Minimal schema interface: is it doable with only forms?

/// <summary>
/// Represents an archetypal implementation of the <see cref="ICalendricalSchemaPlus"/> interface.
/// </summary>
public class ArchetypalSchema : PrototypalSchema
{
    /// <summary>
    /// Represents the cache for <see cref="GetStartOfYear(int)"/>.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly StartOfYearCache[] _startOfYearCache = StartOfYearCache.Create();

    /// <summary>
    /// Initializes a new instance of the <see cref="ArchetypalSchema"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="kernel"/> is null.</exception>
    public ArchetypalSchema(
        ICalendricalKernel kernel,
        int minDaysInYear,
        int minDaysInMonth)
        : base(kernel)
    {
        if (minDaysInYear <= 0) Throw.ArgumentOutOfRange(nameof(minDaysInYear));
        if (minDaysInMonth <= 0) Throw.ArgumentOutOfRange(nameof(minDaysInMonth));

        MinDaysInYear = minDaysInYear;
        MinDaysInMonth = minDaysInMonth;

        // See GetMonth() for an explanation of the formula.
        ApproxMonthsInYear = 1 + (minDaysInYear - 1) / minDaysInMonth;
    }

    /// <inheritdoc />
    public sealed override int MinDaysInYear { get; }

    /// <inheritdoc />
    public sealed override int MinDaysInMonth { get; }

    /// <inheritdoc />
    public override ICalendricalPreValidator PreValidator => new BasicPreValidator(this);

    /// <summary>
    /// Gets or sets a value indicating whether the cache for <see cref="GetStartOfYear(int)"/>
    /// is disabled or not.
    /// <para>The default value is false.</para>
    /// </summary>
    public bool DisableStartOfYearCache { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the overriden version of
    /// <see cref="PrototypalSchema.GetMonth(int, int, out int)"/> is disabled or not.
    /// <para>The default value is false.</para>
    /// </summary>
    public bool DisableCustomGetMonth { get; set; }

    protected int ApproxMonthsInYear { get; }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch, out int doy)
    {
        // It's very similar to what we do in PrototypalSchema, but when we
        // start the loop we are much closer to the actual value of the year.

        // To get our first approximation of the value of the year, we pretend
        // that the years have a constant length equal to MinDaysInYear.
        // > y = 1 + MathZ.Divide(daysSinceEpoch, MinDaysInYear, out int d0y);
        // Notice that the division gives us a zero-based year.
        int y = 1 + MathZ.Divide(daysSinceEpoch, MinDaysInYear);
        int startOfYear = GetStartOfYear(y);

        // TODO(code): explain the algorithm, idem with PrototypalSchema.

        if (daysSinceEpoch >= 0)
        {
            // Notice that the first approximation for the value of the year
            // is greater than or equal to the actual value.
            while (daysSinceEpoch < startOfYear)
            {
                startOfYear -= CountDaysInYear(--y);
            }
        }
        else
        {
            while (daysSinceEpoch >= startOfYear)
            {
                int startOfNextYear = startOfYear + CountDaysInYear(y);
                if (daysSinceEpoch < startOfNextYear) { break; }
                y++;
                startOfYear = startOfNextYear;
            }
            Debug.Assert(daysSinceEpoch >= startOfYear);
        }

        // Notice that, as expected, doy >= 1.
        doy = 1 + daysSinceEpoch - startOfYear;
        return y;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d)
    {
        if (DisableCustomGetMonth) { return base.GetMonth(y, doy, out d); }

        // Algorithm:
        // > int m = GetMonth(y, doy);
        // > d = doy - CountDaysInYearBeforeMonth(y, m);
        // > return m;

        int monthsInYear = CountMonthsInYear(y);

        // Base method: at most (monthsInYear - 1) iteration steps.
        // Local method: at most
        //   Math.Min(ApproxMonthsInYear, monthsInYear) - 1
        // iteration steps if doy = MinDaysInYear.
        //if (ApproxMonthsInYear > 2 * monthsInYear) { return base.GetMonth(y, doy, out d); }

        // To get our first approximation of the value of the month, we pretend
        // that the months have a constant length equal to MinDaysInMonth.
        // > int m = MathN.AugmentedDivide(doy - 1, MinDaysInMonth, out int d);
        // Notice that the division gives us a zero-based month.
        // We can ignore the remainder of the division which gives a theoretical
        // but wrong (except if the months do actually have a constant length)
        // value for the day of the month.
        int m = Math.Min(1 + (doy - 1) / MinDaysInMonth, monthsInYear);
        int daysInYearBeforeMonth = CountDaysInYearBeforeMonth(y, m);

        while (doy < 1 + daysInYearBeforeMonth)
        {
            //if (m == 1) { daysInYearBeforeMonth = 0; break; }
            daysInYearBeforeMonth -= CountDaysInMonth(y, --m);
        }

        // Notice that, as expected, d >= 1.
        d = doy - daysInYearBeforeMonth;
        return m;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYear(int y)
    {
        if (DisableStartOfYearCache) { return GetStartOfYearCore(y); }

        // TODO(code): caching.
        // Currently, we just copied the cache class from NodaTime.
        // https://github.com/bitfaster/BitFaster.Caching

        int index = StartOfYearCache.GetIndex(y);
        StartOfYearCache value = _startOfYearCache[index];

        if (value.IsValidForYear(y) == false)
        {
            int startOfYear = GetStartOfYearCore(y);
            value = new StartOfYearCache(y, startOfYear);
            _startOfYearCache[index] = value;
        }

        return value.StartOfYear;
    }

    /// <summary>
    /// Counts the number of consecutive days from the epoch to the first day of the specified
    /// year.
    /// </summary>
    [Pure]
    protected virtual int GetStartOfYearCore(int y) => base.GetStartOfYear(y);
}
