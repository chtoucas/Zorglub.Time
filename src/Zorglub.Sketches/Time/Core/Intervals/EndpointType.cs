// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Core.Intervals;

/// <summary>
/// Specifies whether an endpoint is a member of the interval or not.
/// </summary>
public enum EndpointType
{
    /// <summary>
    /// The endpoint does not belong to the interval.
    /// </summary>
    Open = 0,

    /// <summary>
    /// The endpoint belongs to the interval.
    /// </summary>
    Closed
}
