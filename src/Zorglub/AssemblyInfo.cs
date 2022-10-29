// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable IDE0130 // Namespace does not match folder structure

[assembly: InternalsVisibleTo("Zorglub.Time.Extras" + Zorglub.Time.AssemblyInfo.PublicKeySuffix)]

#if !__HIDE_INTERNALS__
[assembly: InternalsVisibleTo("Benchmarks" + Zorglub.Time.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Zorglub.Sketches" + Zorglub.Time.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Zorglub.Testing" + Zorglub.Time.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Zorglub.Tests" + Zorglub.Time.AssemblyInfo.PublicKeySuffix)]
[assembly: InternalsVisibleTo("Zorglub.Tests.CSharp" + Zorglub.Time.AssemblyInfo.PublicKeySuffix)]
#endif

namespace Zorglub.Time;

/// <summary>Provides constants used in Assembly's attributes.</summary>
internal static partial class AssemblyInfo
{
    /// <summary>Gets the public key suffix suitable for use with
    /// <see cref="InternalsVisibleToAttribute"/>.</summary>
    public const string PublicKeySuffix =
#if __SIGN_ASSEMBLY__
        ",PublicKey="
        + "0024000004800000940000000602000000240000525341310004000001000100fb21e2a499fbcf"
        + "0cb875344dde9e0d1554954d090511cc735db683b9402a28e150bc1c8fcbe40eded55863b1f1b7"
        + "22c194aba08983514bdaaa241192cfc1f53d56e937e8a7f1b58ce3098df0019e6939f8e5097574"
        + "3c9a78b3b54530540c6f52918baf16ee6411cf2b3137597660f3767205ceb0b377c2a1bc343a01"
        + "2f8ab5d1";
#else
        "";
#endif
}

#if RELEASE && __ENABLE_PREVIEW_FEATURES__
#warning Built using preview features of the .NET platform
#endif
