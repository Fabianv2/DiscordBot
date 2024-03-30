using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.API
{
    public class WeatherInfo
    {
        public class coord
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        public class weather
        {
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

        public class main
        {
            public double temp { get; set; }
            public double feels_like { get; set; }
            public double humidity { get; set; }
        }

        public class wind
        {
            public double speed { get; set; }
        }

        public class clouds
        {
            public int all { get; set; }
        }

        public class sys
        {
            public string country { get; set; }
            public long sunrise { get; set; }
            public long sunset { get; set; }
        }

        public class root
        {
            public coord coord { get; set; }
            public List<weather> weather { get; set;}
            public main main { get; set; }
            public int visibility { get; set; }
            public wind wind { get; set; }
            public clouds clouds { get; set; }
            public int dt { get; set; }
            public sys sys { get; set; }
            public int timezone { get; set; }
            public string name { get; set; }
        }
    }
}
