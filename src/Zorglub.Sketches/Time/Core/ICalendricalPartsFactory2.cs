// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    using System;

    public interface ICalendricalPartsFactory2
    {
        /// <summary>
        /// Creates a new <see cref="ICalendricalPartsFactory"/> instance.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        [Pure]
        public static ICalendricalPartsFactory Create(ICalendricalSchema schema)
        {
            Requires.NotNull(schema);

            return
                schema is SystemSchema sch ? sch
                : schema.SupportedYears.IsSubsetOf(Yemoda.SupportedYears) ? new PartsFactorySlim(schema)
                : new PartsFactoryChecked(schema);
        }
    }
}
