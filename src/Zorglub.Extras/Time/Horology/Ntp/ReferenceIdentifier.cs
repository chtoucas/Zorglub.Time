// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

using System.Globalization;
using System.Text;

public readonly partial struct ReferenceIdentifier :
    IEqualityOperators<ReferenceIdentifier, ReferenceIdentifier>
{
    private readonly uint _value;

    [CLSCompliant(false)]
    public ReferenceIdentifier(uint value)
    {
        _value = value;
    }

    /// <summary>
    /// Obtains a binary view, four bytes in network order, of the current instance.
    /// </summary>
    public ReadOnlySpan<byte> AsBinary() => BitConverter.GetBytes(_value).AsSpan();

    /// <summary>
    /// Returns a culture-independent string representation of the current instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        var bin = AsBinary();
        return FormattableString.Invariant($"{bin[0]:x2}{bin[1]:x2}{bin[2]:x2}{bin[3]:x2}");
    }

    public static ReferenceIdentifier ReadFrom(ReadOnlySpan<byte> buf) =>
        new(BitConverter.ToUInt32(buf));

    public string GetCode(NtpStratum stratum)
    {
        var r = AsBinary();

        return stratum switch
        {
            NtpStratum.Unspecified => ReadKissCode(r),
            NtpStratum.PrimaryReference => ReadReferenceCode(r),
            NtpStratum.SecondaryReference => ReadSecondaryReference(r),
            _ => String.Empty,
        };

        // Kiss-o'-Death Code (unspecified stratum).
        string ReadKissCode(ReadOnlySpan<byte> r) => Encoding.ASCII.GetString(r);

        // Gets a human-friendly code identifying the particular reference clock
        // (primary reference).
        string ReadReferenceCode(ReadOnlySpan<byte> r)
        {
            var code = Encoding.ASCII.GetString(r);
            return FormattableString.Invariant($".{code}.");
        }

        string ReadSecondaryReference(ReadOnlySpan<byte> r)
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
            //   See
            //   https://support.ntp.org/bin/view/Support/RefidFormat
            //
            // See also
            // https://support.ntp.org/bin/view/Dev/UpdatingTheRefidFormat
            // https://github.com/ntp-project/ntp/blob/master-no-authorname/README.leapsmear

            if (r[0] == 254)
            {
                // The remaining 3 bytes encode the current smear value.
                int secs = (r[1] << 16) | (r[2] << 8) | r[3];

                return secs.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                return FormattableString.Invariant($"{r[0]}.{r[1]}.{r[2]}.{r[3]}");
            }
        }
    }
}

public partial struct ReferenceIdentifier
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="ReferenceIdentifier"/> are
    /// equal.
    /// </summary>
    public static bool operator ==(ReferenceIdentifier left, ReferenceIdentifier right) =>
        left._value == right._value;

    /// <summary>
    /// Determines whether two specified instances of <see cref="ReferenceIdentifier"/> are not
    /// equal.
    /// </summary>
    public static bool operator !=(ReferenceIdentifier left, ReferenceIdentifier right) =>
        left._value != right._value;

    /// <inheritdoc />
    [Pure]
    public bool Equals(ReferenceIdentifier other) => _value == other._value;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is ReferenceIdentifier duration && Equals(duration);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _value.GetHashCode();
}
