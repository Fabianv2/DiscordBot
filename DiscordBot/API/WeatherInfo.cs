using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.API
{
    public class WeatherInfo
    {
        public class location
        {
            public string name { get; set; }
            public string region { get; set; }
            public string country { get; set; }
            public string localtime_epoch { get; set; }
            public string localtime { get; set; }
        }

        public class condition
        {
            public string text { get; set; }
            public string icon { get; set; }
        }

        public class current
        {
            public double temp_c { get; set; }
            public double temp_f { get; set; }
            public condition condition { get; set; }
            public double wind_mph { get; set; }
            public double precip_mm { get; set; }
            public int humidity { get; set; }
            public int cloud { get; set; }
            public double feelslike_c { get; set; }
            public double feelslike_f { get; set; }
            public double vis_km { get; set; }
        }
        
        public class root
        {
            public location location { get; set; }
            public current current { get; set; }
        }
    }
}