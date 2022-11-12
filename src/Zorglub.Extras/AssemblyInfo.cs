// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#if !__HIDE_INTERNALS__
[assembly: InternalsVisibleTo("Benchmarks" + Zorglub.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Zorglub.Sketches" + Zorglub.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Zorglub.Testing" + Zorglub.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Zorglub.Tests" + Zorglub.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Zorglub.Tests.CSharp" + Zorglub.AssemblyInfo.PublicKeySuffix)]
#endif

#if RELEASE && __ENABLE_PREVIEW_FEATURES__
#warning Built using preview features of the .NET platform
#endif
