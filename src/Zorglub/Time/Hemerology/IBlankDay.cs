// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

public interface IBlankDay : IDateable
{
    /// <summary>Returns true if the current instance is a blank day; otherwise returns false.
    /// </summary>
    bool IsBlank { get; }
}
