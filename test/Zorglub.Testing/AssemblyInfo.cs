// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: InternalsVisibleTo("Zorglub.Tests" + Zorglub.Time.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Zorglub.Tests.CSharp" + Zorglub.Time.AssemblyInfo.PublicKeySuffix)]

[assembly: CLSCompliant(false)] // Xunit Data Theory
[assembly: ComVisible(false)]
