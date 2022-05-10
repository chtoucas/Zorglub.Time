// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Bulgroz.Samples

#nowarn "1182" // Unused variables

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Intervals

/// A dead simple but unrealistic schema: no intercalation, 12 months of 30 days.
[<Sealed>]
type SimpleSchema() =
    inherit CalendricalSchema(Range.Create(1, 9999), 360, 30) with
        override __.Family = CalendricalFamily.Other
        override __.PeriodicAdjustments = CalendricalAdjustments.None

        override __.IsRegular(monthsInYear) =
            monthsInYear <- 12
            true

        override __.IsLeapYear(y) = false
        override __.IsIntercalaryMonth(y, m) = false
        override __.IsIntercalaryDay(y, m, d) = false
        override __.IsSupplementaryDay(y, m, d) = false

        override __.CountMonthsInYear(y) = 12
        override __.CountDaysInYear(y) = 360
        override __.CountDaysInYearBeforeMonth(y, m) = 30 * (m - 1)
        override __.CountDaysInMonth(y, m) = 30

        override __.CountDaysSinceEpoch(y, m, d) =
            360 * (y - 1) + 30 * (m - 1) + d - 1

        override __.GetDateParts(daysSinceEpoch, y, m, d) =
            let d0y = daysSinceEpoch % 360
            y <- 1 + daysSinceEpoch / 360
            m <- 1 + d0y / 30
            d <- 1 + d0y % 30

        override x.GetYear(daysSinceEpoch, doy) =
            let y = 1 + daysSinceEpoch / 360
            doy <- 1 + daysSinceEpoch - x.GetStartOfYear(y)
            y

        override __.GetMonth(y, doy, d) =
            let d0y = doy - 1
            let m = 1 + d0y / 30
            d <- 1 + d0y % 30
            m

        override __.GetStartOfYear(y) = 360 * (y - 1)
