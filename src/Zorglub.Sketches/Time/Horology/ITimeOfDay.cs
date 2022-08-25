// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    /// <summary>
    /// Defines a time of the day (hour:minute:second).
    /// </summary>
    public interface ITimeOfDay
    {
        /// <summary>
        /// Gets the hour of day.
        /// <para>The result is in the range from 0 to 23.</para>
        /// </summary>
        int Hour { get; }

        /// <summary>
        /// Gets the hour using a 12-hour clock.
        /// <para>The result is in the range from 1 to 12.</para>
        /// </summary>
        int HourOfHalfDay { get; }

        /// <summary>
        /// Returns true if the current instance is before midday; otherwise
        /// returns false.
        /// </summary>
        bool IsAnteMeridiem { get; }

        /// <summary>
        /// Gets the minute of hour.
        /// <para>The result is in the range from 0 to 59.</para>
        /// </summary>
        int Minute { get; }

        /// <summary>
        /// Gets the second of minute.
        /// <para>The result is in the range from 0 to 59.</para>
        /// </summary>
        int Second { get; }

        /// <summary>
        /// Gets the millisecond of second.
        /// <para>The result is in the range from 0 to 999.</para>
        /// </summary>
        int Millisecond { get; }

        /// <summary>
        /// Gets the number of elapsed seconds since midnight.
        /// <para>The result is in the range from 0 to 86_399.</para>
        /// </summary>
        int SecondOfDay { get; }

        /// <summary>
        /// Gets the number of elapsed milliseconds since midnight.
        /// <para>The result is in the range from 0 to 86_399_999.</para>
        /// </summary>
        int MillisecondOfDay { get; }

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        void Deconstruct(out int hour, out int minute, out int second);
    }
}
