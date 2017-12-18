using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperMethods
{
    public static class ConvertHelper
    {
        #region CelsiusToFahrenheit
        /// <summary>
        /// Converts given Celcius value to Fahrenheit
        /// </summary>
        /// <param name="temperatureCelsius">Celcius value</param>
        /// <returns>double</returns>

        public static double CelsiusToFahrenheit(string temperatureCelsius)
        {
            return (Double.Parse(temperatureCelsius) * 9 / 5) + 32;
        }
        #endregion

        #region FahrenheitToCelsius
        /// <summary>
        /// Converts given Fahrenheit value to Celcius
        /// </summary>
        /// <param name="temperatureFahrenheit">Fahrenheit value</param>
        /// <returns>double</returns>
        public static double FahrenheitToCelsius(string temperatureFahrenheit)
        {
            return (Double.Parse(temperatureFahrenheit) - 32) * 5 / 9;
        }
        #endregion

    }
}
