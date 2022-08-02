// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#if VISIBLE_INTERNALS // WARNING: when true we can no longer build the other projects.
[assembly: InternalsVisibleTo("Benchmarks" + Zorglub.Time.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Zorglub.Sketches" + Zorglub.Time.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Zorglub.Testing" + Zorglub.Time.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Zorglub.Tests" + Zorglub.Time.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Zorglub.Tests.CSharp" + Zorglub.Time.AssemblyInfo.PublicKeySuffix)]
#endif