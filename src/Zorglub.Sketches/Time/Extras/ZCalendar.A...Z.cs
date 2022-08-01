// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extras
{
    using Zorglub.Time.Simple;

    // The Gregorian calendar is initialized in ZCatalog.

    internal static class JulianZCalendar
    {
        public static string Key => SimpleCalendar.Julian.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(SimpleCalendar.Julian);
    }

    internal static class ArmenianZCalendar
    {
        public static string Key => SimpleCalendar.Armenian.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(SimpleCalendar.Armenian);
    }

    internal static class CopticZCalendar
    {
        public static string Key => SimpleCalendar.Coptic.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(SimpleCalendar.Coptic);
    }

    internal static class EthiopicZCalendar
    {
        public static string Key => SimpleCalendar.Ethiopic.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(SimpleCalendar.Ethiopic);
    }

    internal static class TabularIslamicZCalendar
    {
        public static string Key => SimpleCalendar.TabularIslamic.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(SimpleCalendar.TabularIslamic);
    }

    internal static class ZoroastrianZCalendar
    {
        public static string Key => SimpleCalendar.Zoroastrian.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(SimpleCalendar.Zoroastrian);
    }
}
