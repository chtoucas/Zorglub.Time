// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using Zorglub.Time.Core;

    using static Zorglub.Time.Extensions.Unboxing;

    /// <summary>
    /// Provides extension methods for <see cref="Box{TSchema}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class BoxExtensions
    {
        // The following methods are for those who do not feel at ease w/ LINQ
        // for Objects. Besides that, TryCreateCalendar() is much simpler than
        // the equivalent written using LINQ.

        /// <summary>
        /// Creates a calendar from the specified (boxed) schema, (unique) key and reference epoch.
        /// <para>If a calendar with the same key already exists, this method returns it.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException">One of the parameters is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="this"/> is empty but see also
        /// <see cref="SimpleCatalog.GetOrAdd(string, SystemSchema, DayNumber, bool)"/>.
        /// </exception>
        /// <exception cref="OverflowException">For details, see
        /// <see cref="SimpleCatalog.GetOrAdd(string, SystemSchema, DayNumber, bool)"/>.
        /// </exception>
        [Pure]
        public static SimpleCalendar GetOrCreateCalendar<TSchema>(
            this Box<TSchema> @this, string key, DayNumber epoch, bool proleptic = false)
            where TSchema : SystemSchema
        {
            Requires.NotNull(@this);

            var q = from schema in @this
                    select SimpleCatalog.GetOrAdd(key, schema, epoch, proleptic);
            return q.TryUnbox(out var chr) ? chr : Throw.BadBox<SimpleCalendar>(nameof(@this));
        }

        /// <summary>
        /// Creates a calendar from the specified (boxed) schema, (unique) key and reference epoch.
        /// </summary>
        /// <exception cref="ArgumentNullException">One of the parameters is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="this"/> is empty but see also
        /// <see cref="SimpleCatalog.Add(string, SystemSchema, DayNumber, bool)"/>.
        /// </exception>
        /// <exception cref="OverflowException">For details, see
        /// <see cref="SimpleCatalog.Add(string, SystemSchema, DayNumber, bool)"/>.
        /// </exception>
        [Pure]
        public static SimpleCalendar CreateCalendar<TSchema>(
            this Box<TSchema> @this, string key, DayNumber epoch, bool proleptic = false)
            where TSchema : SystemSchema
        {
            Requires.NotNull(@this);

            var q = from schema in @this
                    select SimpleCatalog.Add(key, schema, epoch, proleptic);
            return q.TryUnbox(out var chr) ? chr : Throw.BadBox<SimpleCalendar>(nameof(@this));
        }

        /// <summary>
        /// Attempts to create a calendar from the specified (boxed) schema, (unique) key and
        /// reference epoch; the result is given in an output parameter.
        /// </summary>
        /// <exception cref="ArgumentNullException">One of the parameters is null.</exception>
        [Pure]
        public static bool TryCreateCalendar<TSchema>(
            this Box<TSchema> @this,
            string key,
            DayNumber epoch,
            [NotNullWhen(true)] out SimpleCalendar? calendar,
            bool proleptic = false)
            where TSchema : SystemSchema
        {
            Requires.NotNull(@this);

            if (@this.TryUnbox(out var sch))
            {
                return SimpleCatalog.TryAdd(key, sch, epoch, proleptic, out calendar);
            }
            else
            {
                calendar = null;
                return false;
            }
        }
    }
}
