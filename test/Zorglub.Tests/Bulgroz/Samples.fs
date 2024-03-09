// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Bulgroz.Samples

#nowarn "1182" // Unused variables

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Intervals

/// A dead simple but unrealistic schema: no intercalation, 12 months of 30 days.
[<Sealed>]
type SimpleSchema() =
    inherit CalendricalSchema(Range.Create(1, 9999), 360, 30)
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

        override __.CountMonthsSinceEpoch(y, m) =
            12 * (y - 1) + m - 1

        override __.CountDaysSinceEpoch(y, m, d) =
            360 * (y - 1) + 30 * (m - 1) + d - 1

        override __.GetMonthParts(monthsSinceEpoch, y, m) =
            let m0 = monthsSinceEpoch % 12
            y <- 1 + monthsSinceEpoch / 12
            m <- 1 + m0

        override __.GetDateParts(daysSinceEpoch, y, m, d) =
            let d0y = daysSinceEpoch % 360
            y <- 1 + daysSinceEpoch / 360
            m <- 1 + d0y / 30
            d <- 1 + d0y % 30

        override __.GetYear(daysSinceEpoch) =
            1 + daysSinceEpoch / 360

        override __.GetMonth(y, doy, d) =
            let d0y = doy - 1
            let m = 1 + d0y / 30
            d <- 1 + d0y % 30
            m

        override __.GetStartOfYear(y) = 360 * (y - 1)
