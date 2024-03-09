// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks;

using System.Runtime.CompilerServices;

using NodaTime;

using Zorglub.Time;
using Zorglub.Time.Core;

public abstract class BenchmarkBase
{
    protected BenchmarkBase() { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    protected static void Consume(in bool _) { }

    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.NoInlining)]
    protected static void Consume(in uint _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    protected static void Consume(in int _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    protected static void Consume(in long _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    protected static void Consume(in DayOfWeek _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    protected static void Consume(in IsoWeekday _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    protected static void Consume(in IsoDayOfWeek _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    protected static void Consume(in Yemoda _) { }

    [MethodImpl(MethodImplOptions.NoInlining)]
    protected static void Consume(in Yemo _) { }
}
