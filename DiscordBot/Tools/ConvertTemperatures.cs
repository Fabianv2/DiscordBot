using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class ConvertTemperatures
    {
        public double KelvinToCelcius(double kelvin)
        {
            return kelvin - 273.15;
        }

        public double KelvinToFahrenheit(double kelvin)
        {
            return 1.8 * (kelvin - 273) + 32;
        }
    }
}
