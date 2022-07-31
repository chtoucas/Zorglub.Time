// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extras
{
    using Zorglub.Time.Simple;

    internal static class JulianZCalendar
    {
        public static string Key => SimpleJulian.Instance.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(SimpleJulian.Instance);
    }

    internal static class ArmenianZCalendar
    {
        public static string Key => SimpleArmenian.Instance.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(SimpleArmenian.Instance);
    }

    internal static class CopticZCalendar
    {
        public static string Key => SimpleCoptic.Instance.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(SimpleCoptic.Instance);
    }

    internal static class EthiopicZCalendar
    {
        public static string Key => SimpleEthiopic.Instance.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(SimpleEthiopic.Instance);
    }

    internal static class TabularIslamicZCalendar
    {
        public static string Key => SimpleTabularIslamic.Instance.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(SimpleTabularIslamic.Instance);
    }

    internal static class ZoroastrianZCalendar
    {
        public static string Key => SimpleZoroastrian.Instance.Key;

        public static readonly ZCalendar Instance =
            ZCatalog.InitSystemCalendar(SimpleZoroastrian.Instance);
    }
}
