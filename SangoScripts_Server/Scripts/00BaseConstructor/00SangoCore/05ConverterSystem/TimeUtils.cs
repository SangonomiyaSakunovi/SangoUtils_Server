﻿namespace SangoUtils_Server_Scripts.Converter
{
    public static class TimeUtils
    {
        private static DateTime _startTime = new(1970, 1, 1, 0, 0, 0);

        public static long GetUnixDateTimeSeconds(DateTime dateTime)
        {
            TimeSpan timeSpan = dateTime - _startTime;
            return Convert.ToInt64(timeSpan.TotalSeconds);
        }

        public static DateTime GetDateTimeFromDateNumber(int year, int month, int day)
        {
            if (year < 1970 || year > 2100 || month < 1 || month > 12)
            {
                return DateTime.MinValue;
            }
            int daysInMonth = DateTime.DaysInMonth(year, month);
            if (day < 1 || day > daysInMonth)
            {
                return DateTime.MinValue;
            }
            return new DateTime(year, month, day, 0, 0, 0);
        }
    }
}
