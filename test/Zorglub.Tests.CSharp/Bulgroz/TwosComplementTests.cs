// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//#define CHECKED_TESTS

namespace Zorglub.Bulgroz;

using Xunit;

internal static class TwosComplement
{
    public static sbyte ByteToSByte(byte uy) => (sbyte)(uy < 128 ? uy : uy - 256);
    public static sbyte ByteToSByte1(byte uy) => (sbyte)((uy & 0x80) == 0 ? uy : uy - 256);
    public static sbyte ByteToSByte2(byte uy) => (sbyte)((uy & 0x80) == 0x80 ? uy - 256 : uy);
    public static sbyte ByteToSByteUnchecked(byte i) => unchecked((sbyte)i);

    public static sbyte ByteToSByteChecked(byte i) =>
#if CHECKED_TESTS
        // Of course, the cast overflows in a checked context.
        checked((sbyte)i);
#else
        0;
#endif
}

public static class TwosComplementTests
{
    public static readonly TheoryData<byte, sbyte> Data = new()
    {
        { 0, 0 },
        { 1, 1 },
        { 2, 2 },
        // ...
        { 125, 125 },
        { 126, 126 },
        { 127, 127 },
        { 128, -128 },
        { 129, -127 },
        { 130, -126 },
        // ...
        { 253, -3 },
        { 254, -2 },
        { 255, -1 },
    };

    [Theory, MemberData(nameof(Data))]
    public static void ByteToSByte(byte uy, sbyte y) =>
        Assert.Equal(y, TwosComplement.ByteToSByte(uy));

    [Theory, MemberData(nameof(Data))]
    public static void ByteToSByte1(byte uy, sbyte y) =>
        Assert.Equal(y, TwosComplement.ByteToSByte1(uy));

    [Theory, MemberData(nameof(Data))]
    public static void ByteToSByte2(byte uy, sbyte y) =>
        Assert.Equal(y, TwosComplement.ByteToSByte2(uy));

    [Theory, MemberData(nameof(Data))]
    public static void ByteToSByteUnchecked(byte uy, sbyte y) =>
        Assert.Equal(y, TwosComplement.ByteToSByteUnchecked(uy));

    [Theory, MemberData(nameof(Data))]
    public static void ByteToSByteChecked(byte uy, sbyte y) =>
#if CHECKED_TESTS
        Assert.Equal(y, TwosComplement.ByteToSByteChecked(uy));
#else
        Assert.Equal(0, TwosComplement.ByteToSByteChecked(uy));
#endif
}