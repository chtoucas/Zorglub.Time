// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Extras.GregorianReformTests

open Zorglub.Testing

open Xunit

module Specialized =
    open Zorglub.Time.Extras.Specialized
    open Zorglub.Time.Specialized

    [<Fact>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``Property Official`` () =
        let official = GregorianReform.Official

        official.FirstGregorianDate === new GregorianDate(1582, 10, 15)
        official.LastJulianDate     === new JulianDate(1582, 10, 4)
        official.SecularShift       === 10

    [<Fact>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``Property Official (from last Julian date)`` () =
        let date = new JulianDate(1582, 10, 4)
        let reform = GregorianReform.FromLastJulianDate(date)

        reform === GregorianReform.Official

    [<Fact>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``Property Official (from first Gregorian date)`` () =
        let date = new GregorianDate(1582, 10, 15)
        let reform = GregorianReform.FromFirstGregorianDate(date)

        reform === GregorianReform.Official

module Simple =
    open Zorglub.Time.Extras.Simple
    open Zorglub.Time.Simple

    [<Fact>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``Property Official`` () =
        let official = GregorianReform.Official

        official.FirstGregorianDate === new CalendarDate(1582, 10, 15)
        official.LastJulianDate     === SimpleCalendar.Julian.GetCalendarDate(1582, 10, 4)
        official.SecularShift       === 10

    [<Fact>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``Property Official (from last Julian date)`` () =
        let reform = GregorianReform.FromLastJulianDate(1582, 10, 4)

        reform === GregorianReform.Official

    [<Fact>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``Property Official (from first Gregorian date)`` () =
        let reform = GregorianReform.FromFirstGregorianDate(1582, 10, 15)

        reform === GregorianReform.Official
