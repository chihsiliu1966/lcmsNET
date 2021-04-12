﻿using System;
using System.Runtime.InteropServices;

namespace lcmsNET
{
    /// <summary>
    /// Represents a calendar date and time broken into its components.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Tm
    {
        /// <summary>
        /// Seconds after the minute in the range 0..61.
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public int sec;
        /// <summary>
        /// Minutes after the hour in the range 0..59.
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public int min;
        /// <summary>
        /// Hours since midnight in the range 0..23.
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public int hour;
        /// <summary>
        /// Day of the month in the range 1..31.
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public int mday;
        /// <summary>
        /// Month of the year in the range 0..11.
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public int mon;
        /// <summary>
        /// Years since 1900.
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public int year;
        /// <summary>
        /// Days since Sunday in the range 0..6.
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public int wday;
        /// <summary>
        /// Days since January 1 in the range 0..365.
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public int yday;
        /// <summary>
        /// Daylight saving time flag. Greater than zero if Daylight Savings is in effect,
        /// zero if not in effect or less than zero if information is not available.
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public int isdst;
        // The glibc version of struct tm has additional fields
        //   long tm_gmtoff;           /* Seconds east of UTC */
        //   const char *tm_zone;      /* Timezone abbreviation */
        // defined when _BSD_SOURCE was set before including <time.h>.
        //
        // Use filler to ensure sufficient size when marshaling this type in this runtime.
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = FILL_SIZE)]
        private readonly byte[] fill;

        /// <summary>
        /// Creates a <see cref="Tm"/> from the supplied handle.
        /// </summary>
        /// <param name="handle">A handle to an existing calendar date and time.</param>
        /// <returns>A new <see cref="Tm"/> instance referencing an existing calendar date and time.</returns>
        public static Tm FromHandle(IntPtr handle)
        {
            return Marshal.PtrToStructure<Tm>(handle);
        }

        /// <summary>
        /// Implicitly converts a <see cref="Tm"/> to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="tm">The <see cref="Tm"/> to be converted.</param>
        public static implicit operator DateTime(in Tm tm)
        {
            return new DateTime(tm.year + 1900, tm.mon + 1, tm.mday, tm.hour, tm.min, tm.sec);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Tm"/> structure.
        /// </summary>
        /// <param name="date">A <see cref="DateTime"/> instance.</param>
        public Tm(DateTime date)
        {
            sec = date.Second;
            min = date.Minute;
            hour = date.Hour;
            mday = date.Day;
            mon = date.Month - 1;
            year = (date.Year >= 1900) ? date.Year - 1900 : 0;
            wday = (int)date.DayOfWeek;
            yday = date.DayOfYear - 1;
            isdst = -1;
            fill = new byte[FILL_SIZE];
        }

        private const int FILL_SIZE = 16;
    }
}
