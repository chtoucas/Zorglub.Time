// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

using System.Globalization;
using System.Text;

/// <summary>
/// Represents the identifier of a server or a reference clock.
/// <para><see cref="ReferenceId"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct ReferenceId :
    IEqualityOperators<ReferenceId, ReferenceId>
{
    private readonly uint _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReferenceId"/> struct.
    /// </summary>
    internal ReferenceId(uint value)
    {
        _value = value;
    }

    internal uint Value => _value;

    /// <summary>
    /// Returns a culture-independent string representation of the current instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        var bytes = AsBytes();
        return FormattableString.Invariant($"{bytes[0]:x2}{bytes[1]:x2}{bytes[2]:x2}{bytes[3]:x2}");
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ReferenceId"/> struct from the specified read-only
    /// byte span.
    /// </summary>
    /// <exception cref="AoorException">The length of <paramref name="value"/> is less than 4.</exception>
    public static ReferenceId Create(ReadOnlySpan<byte> value) => new(BitConverter.ToUInt32(value));

    /// <summary>
    /// Obtains a binary view, four bytes in network order, of the current instance.
    /// </summary>
    public ReadOnlySpan<byte> AsBytes() => BitConverter.GetBytes(_value).AsSpan();
}

public partial struct ReferenceId
{
    // RFC 5905 Section 7.3 p.21.
    // "The authoritative list of Reference Identifiers is maintained by IANA."
    // See https://www.iana.org/assignments/ntp-parameters/ntp-parameters.xhtml

    // Any string beginning with the ASCII character "X" is reserved for
    // unregistered experimentation and development.
    private const char Reserved = 'X';

    private static readonly HashSet<string> s_Codes = new()
    {
        // RFC 5905 Section 7.3, p.21.
        "GOES", // Geosynchronous Orbit Environment Satellite
        "GPS\0",// Global Position System
        "GAL\0",// Galileo Positioning System
        "PPS\0",// Generic pulse-per-second
        "IRIG", // Inter-Range Instrumentation Group
        "WWVB", // LF Radio WWVB Ft. Collins, CO 60 kHz
        "DCF\0",// LF Radio DCF77 Mainflingen, DE 77.5 kHz
        "HBG\0",// LF Radio HBG Prangins, HB 75 kHz
        "MSF\0",// LF Radio MSF Anthorn, UK 60 kHz
        "JJY\0",// LF Radio JJY Fukushima, JP 40 kHz, Saga, JP 60 kHz
        "LORC", // MF Radio LORAN C station, 100 kHz
        "TDF\0",// MF Radio Allouis, FR 162 kHz
        "CHU\0",// HF Radio CHU Ottawa, Ontario
        "WWV\0",// HF Radio WWV Ft. Collins, CO
        "WWVH", // HF Radio WWVH Kauai, HI
        "NIST", // NIST telephone modem
        "ACTS", // NIST telephone modem
        "USNO", // USNO telephone modem
        "PTB\0",// European telephone modem
    };

    private static readonly HashSet<string> s_KissCodes = new()
    {
        // RFC 5905 Section 7.4, p.24.
        "ACST", // The association belongs to a unicast server
        "AUTH", // Server authentication failed
        "AUTO", // Autokey sequence failed
        "BCST", // The association belongs to a broadcast server
        "CRYP", // Cryptographic authentication or identification failed
        "DENY", // Access denied by remote server
        "DROP", // Lost peer in symmetric mode
        "RSTR", // Access denied due to local policy
        "INIT", // The association has not yet synchronized for the first time
        "MCST", // The association belongs to a dynamically discovered server
        "NKEY", // No key found. Either the key was never installed or is not trusted
        "RATE", // Rate exceeded. The server has temporarily denied access because
                // the client exceeded the rate threshold
        "RMOT", // Alteration of association from a remote host running ntpdc
        "STEP", // A step change in system time has occurred, but the association
                // has not yet resynchronized

        // RFC 8915 Section 5.7
        "NTSN", // Network Time Security (NTS) negative-acknowledgment (NAK)
    };

    /// <summary>
    /// Obtains the NTP code identifying the particular server or reference clock or a "kiss code".
    /// </summary>
    internal NtpCode GetCode(NtpStratum stratum)
    {
        var bytes = AsBytes();

        return stratum switch
        {
            NtpStratum.Unspecified => ReadKissCode(bytes),
            NtpStratum.PrimaryReference => ReadCode(bytes),
            NtpStratum.SecondaryReference => ReadSecondaryReference(bytes),
            _ => new NtpCode(NtpCodeType.Unknown, ToString()),
        };

        static NtpCode ReadKissCode(ReadOnlySpan<byte> bytes)
        {
            var code = Encoding.ASCII.GetString(bytes);
            var known = s_KissCodes.Contains(code);

            return new NtpCode(
                code[0] == Reserved ? NtpCodeType.ReservedKissCode
                : known ? NtpCodeType.KissCode
                : NtpCodeType.UnknownKissCode,
                code);
        }

        static NtpCode ReadCode(ReadOnlySpan<byte> bytes)
        {
            var code = Encoding.ASCII.GetString(bytes);
            var known = s_Codes.Contains(code);

            return new NtpCode(
                code[0] == Reserved ? NtpCodeType.ReservedIdentifier
                : known ? NtpCodeType.Identifier
                : NtpCodeType.UnknownIdentifier,
                FormattableString.Invariant($".{code}."));
        }

        static NtpCode ReadSecondaryReference(ReadOnlySpan<byte> bytes)
        {
            // NTPv4 is a mess... the various RFCs (2030, 4330, 5905) seem to be
            // contradictory: IPv4 address, or the first four bytes of the MD5
            // digest of the IPv6 address, or even the low order 32 bits of a
            // timestamp.
            //
            //   Currently, ntpq has no way to know which type of Refid the
            //   server is sending and always displays the Refid value in
            //   dotted-quad format -- which means that any IPv6 Refids will be
            //   listed as if they were IPv4 addresses, even though they are not.
            //   See https://support.ntp.org/bin/view/Support/RefidFormat
            //
            // See also
            // https://support.ntp.org/bin/view/Dev/UpdatingTheRefidFormat
            // https://github.com/ntp-project/ntp/blob/master-no-authorname/README.leapsmear

            if (bytes[0] == 254)
            {
                // Only implemented by private servers.
                // The remaining 3 bytes (big-endian) encode the smear value.
                int smear = (bytes[1] << 16) | (bytes[2] << 8) | bytes[3];

                return new NtpCode(
                    NtpCodeType.LeapSecondSmearing,
                    smear.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                return new NtpCode(
                    NtpCodeType.IPAddressMaybe,
                    FormattableString.Invariant($"{bytes[0]}.{bytes[1]}.{bytes[2]}.{bytes[3]}"));
            }
        }
    }
}

public partial struct ReferenceId // IEquatable
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="ReferenceId"/> are
    /// equal.
    /// </summary>
    public static bool operator ==(ReferenceId left, ReferenceId right) =>
        left._value == right._value;

    /// <summary>
    /// Determines whether two specified instances of <see cref="ReferenceId"/> are not
    /// equal.
    /// </summary>
    public static bool operator !=(ReferenceId left, ReferenceId right) =>
        left._value != right._value;

    /// <inheritdoc />
    [Pure]
    public bool Equals(ReferenceId other) => _value == other._value;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is ReferenceId id && Equals(id);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _value.GetHashCode();
}
