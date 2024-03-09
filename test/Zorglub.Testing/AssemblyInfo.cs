// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: InternalsVisibleTo("Zorglub.Tests" + Zorglub.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Zorglub.Tests.CSharp" + Zorglub.AssemblyInfo.PublicKeySuffix)]

[assembly: CLSCompliant(false)] // Xunit Data Theory
[assembly: ComVisible(false)]
