// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Simple;

    // Temporary type.

    internal readonly record struct ZIdent(int Value)
    {
        /// <summary>
        /// Represents the maximun value for the ident of a calendar.
        /// <para>This field is a constant equal to 255.</para>
        /// </summary>
        public const int MaxId = Byte.MaxValue;

        public const int MinUserId = CalendarCatalog.MaxId + 1;

        public const int Invalid = Int32.MaxValue;

        public static implicit operator ZIdent(int value) => new(value);

        public static explicit operator int(ZIdent ident) => ident.Value;
    }
}
