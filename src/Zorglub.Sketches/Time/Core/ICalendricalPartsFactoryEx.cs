// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix (Naming)

namespace Zorglub.Time.Core
{
    // FIXME(api): Create() -> replace by a better API? One internal and one public.

    public interface ICalendricalPartsFactoryEx
    {
        /// <summary>
        /// Creates a new <see cref="ICalendricalPartsFactory"/> instance.
        /// <para>When <paramref name="checked"/> is true, a method will throw if the result is not
        /// representable by the system.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        [Pure]
        public static ICalendricalPartsFactory Create(ICalendricalSchema schema, bool @checked = true) =>
            @checked ? new CalendricalPartsFactoryChecked(schema)
            : schema is ICalendricalPartsFactory sch ? sch
            : new CalendricalPartsFactoryUnchecked(schema);
    }
}
