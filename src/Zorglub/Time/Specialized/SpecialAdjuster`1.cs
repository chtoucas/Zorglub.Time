// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized
{
    using Zorglub.Time.Hemerology;
    using Zorglub.Time.Hemerology.Scopes;

    /// <summary>
    /// Defines common adjusters for <typeparamref name="TDate"/> and provides a base for derived
    /// classes.
    /// <para>This class can ONLY be inherited from within friend assemblies.</para>
    /// </summary>
    /// <typeparam name="TDate">The type of date object.</typeparam>
    public abstract class SpecialAdjuster<TDate> : DateableAdjuster<TDate>
        // IDateableOrdinally: not necessary, but it should largely prevent the
        // use of this class with date types not based on daysSinceEpoch.
        where TDate : IDateable, IDateableOrdinally
    {
        /// <summary>
        /// Called from constructors in derived classes to initialize the
        /// <see cref="SpecialAdjuster{TDate}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="ArgumentException">paramref name="scope"/> is NOT complete.</exception>
        private protected SpecialAdjuster(CalendarScope scope) : base(scope)
        {
            Debug.Assert(scope != null);

            // To avoid an unnecessary validation, a derived class is expected
            // to override GetDate(), but this can only be justified when the
            // scope is complete.
            if (scope.IsComplete == false) Throw.Argument(nameof(scope));
        }
    }
}
