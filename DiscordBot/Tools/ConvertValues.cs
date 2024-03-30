using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class ConvertValues
    {
        public double KelvinToCelcius(double kelvin)
        {
            return Math.Round(kelvin - 273.15, 2);
        }

        public double KelvinToFahrenheit(double kelvin)
        {
            return Math.Round(1.8 * (kelvin - 273) + 32, 2);
        }

        public double MilesToKilometers(double mph)
        {
            return Math.Round(mph / 0.62137, 2);
        }
    }
}
