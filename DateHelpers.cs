using System;

namespace HelperMethods
{
    public static class DateHelpers
    {
        #region CalculateWorkDays
        /// <summary>
        /// Calculates work days between two given dates (excluding weekend).
        /// </summary>
        public static int CalculateWorkDays(DateTime firstDay, DateTime lastDay)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;

            if (businessDays > fullWeekCount * 7)
            {
                int firstDayOfWeek = firstDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)firstDay.DayOfWeek;
                int lastDayOfWeek = lastDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)lastDay.DayOfWeek;

                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)
                    businessDays -= 1;
            }

            businessDays -= fullWeekCount + fullWeekCount;
            return businessDays;
        }
        #endregion

        #region TimeAgo
        /// <summary>
        /// Gives appropriate string value for the timespan between now and given date.
        /// </summary>
        public static string TimeAgo(this DateTime dateInput)
        {
            if (dateInput > DateTime.Now)
                return "coming soon...";
            TimeSpan span = DateTime.Now - dateInput;

            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("{0} {1} ago", years, years == 1 ? "year" : "years");
            }

            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("{0} {1} ago", months, months == 1 ? "month" : "months");
            }

            if (span.Days > 0)
                return String.Format("{0} {1} ago", span.Days, span.Days == 1 ? "day" : "days");

            if (span.Hours > 0)
                return String.Format("{0} {1} ago", span.Hours, span.Hours == 1 ? "hour" : "hours");

            if (span.Minutes > 0)
                return String.Format("{0} {1} ago", span.Minutes, span.Minutes == 1 ? "minute" : "minutes");

            if (span.Seconds > 5)
                return String.Format("{0} seconds ago", span.Seconds);

            if (span.Seconds <= 5)
                return "now";

            return string.Empty;
        }
        #endregion
    }
}
