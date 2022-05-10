// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Simple;

    internal static class WideJulianCalendar
    {
        public static string Key => JulianCalendar.Instance.Key;

        public static readonly WideCalendar Instance =
            WideCatalog.InitSystemCalendar(JulianCalendar.Instance);
    }

    internal static class WideArmenianCalendar
    {
        public static string Key => ArmenianCalendar.Instance.Key;

        public static readonly WideCalendar Instance =
            WideCatalog.InitSystemCalendar(ArmenianCalendar.Instance);
    }

    internal static class WideCopticCalendar
    {
        public static string Key => CopticCalendar.Instance.Key;

        public static readonly WideCalendar Instance =
            WideCatalog.InitSystemCalendar(CopticCalendar.Instance);
    }

    internal static class WideEthiopicCalendar
    {
        public static string Key => EthiopicCalendar.Instance.Key;

        public static readonly WideCalendar Instance =
            WideCatalog.InitSystemCalendar(EthiopicCalendar.Instance);
    }

    internal static class WideTabularIslamicCalendar
    {
        public static string Key => TabularIslamicCalendar.Instance.Key;

        public static readonly WideCalendar Instance =
            WideCatalog.InitSystemCalendar(TabularIslamicCalendar.Instance);
    }

    internal static class WideZoroastrianCalendar
    {
        public static string Key => ZoroastrianCalendar.Instance.Key;

        public static readonly WideCalendar Instance =
            WideCatalog.InitSystemCalendar(ZoroastrianCalendar.Instance);
    }
}
