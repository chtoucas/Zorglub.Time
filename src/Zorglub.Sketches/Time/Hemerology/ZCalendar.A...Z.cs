// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Simple;

    internal static class JulianZCalendar
    {
        public static string Key => JulianCalendar.Instance.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(JulianCalendar.Instance);
    }

    internal static class ArmenianZCalendar
    {
        public static string Key => ArmenianCalendar.Instance.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(ArmenianCalendar.Instance);
    }

    internal static class CopticZCalendar
    {
        public static string Key => CopticCalendar.Instance.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(CopticCalendar.Instance);
    }

    internal static class EthiopicZCalendar
    {
        public static string Key => EthiopicCalendar.Instance.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(EthiopicCalendar.Instance);
    }

    internal static class TabularIslamicZCalendar
    {
        public static string Key => TabularIslamicCalendar.Instance.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(TabularIslamicCalendar.Instance);
    }

    internal static class ZoroastrianZCalendar
    {
        public static string Key => ZoroastrianCalendar.Instance.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(ZoroastrianCalendar.Instance);
    }
}
