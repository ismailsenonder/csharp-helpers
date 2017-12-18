using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperMethods
{
    public class DateHelper
    {
        #region CalculateWorkDays
        /// <summary>
        /// Calculates work days between two given dates (no saturday and sunday).
        /// </summary>
        /// <param name="firstDay"></param>
        /// <param name="lastDay"></param>
        /// <returns>int</returns>
        public int CalculateWorkDays(DateTime firstDay, DateTime lastDay)
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
    }
}
