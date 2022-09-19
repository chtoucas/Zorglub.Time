// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

/// <summary>Specifies the kind of algorithm governing the calendar.</summary>
public enum CalendricalAlgorithm
{
    /// <summary>The calendrical algorithm is unknown.</summary>
    Unknown = 0,

    /// <summary>The calendrical algorithm is made from perpetual rules, it is purely computational.
    /// <para>Perpetual calendars having a different meaning, we prefer to say arithmetical for
    /// this type of calendars.</para></summary>
    Arithmetical,

    /// <summary>The calendrical algorithm is made from an astronomical model.</summary>
    Astronomical,

    /// <summary>The calendrical algorithm is based on observations of the sky or some other natural
    /// phenomenon.
    /// <para>This value has been added for completeness but, for obvious reasons, we won't
    /// actually use it.</para></summary>
    Observational,
}
