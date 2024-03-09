// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

/// <summary>
/// The exception that is thrown when an NTP error occurs.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class NtpException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NtpException"/> class.
    /// </summary>
    public NtpException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NtpException"/> class with the specified
    /// message.
    /// </summary>
    public NtpException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NtpException"/> class with the specified
    /// message and inner exception.
    /// </summary>
    public NtpException(string message, Exception innerException)
        : base(message, innerException) { }

    [DoesNotReturn]
    internal static void Throw(string message) => throw new NtpException(message);
}
