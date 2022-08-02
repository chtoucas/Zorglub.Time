// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized
{
    // FIXME(api): temp interface? where ? name: IInvariantNamed, IGlobalName
    // On a un problème similaire avec BasicCalendar car Name est indépendant de
    // la culture.

    public interface INamedCalendar
    {
        /// <summary>
        /// Gets the culture-invariant name of the calendar.
        /// </summary>
        string Name { get; }
    }
}
