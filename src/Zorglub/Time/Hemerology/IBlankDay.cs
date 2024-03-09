// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Hemerology;

/// <summary>Defines methods specific to calendars featuring blank days.</summary>
/// <remarks>A blank day does not belong to any month and is kept outside the weekday cycle.</remarks>
public interface IBlankDay : IDateable
{
    /// <summary>Returns true if the current instance is a blank day; otherwise returns false.
    /// </summary>
    bool IsBlank { get; }
}
