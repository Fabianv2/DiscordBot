using Discord.Commands;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class WeatherModule : ModuleBase<SocketCommandContext>
    {
        

        //[Command("Wetter")]
        //[Alias("wetter", "Weather", "weather")]
        public async Task GetWeather([Remainder] string location)
        {
            string serverFiles = AppDomain.CurrentDomain.BaseDirectory;
            string apiFile = Path.Combine(serverFiles, "apiKey", "apiKey.txt");
            StreamReader sr = new StreamReader(apiFile);
            string apiKey = sr.ReadToEnd();

            string currWeatherAPIurl = $"http://api.weatherapi.com/v1/current.json?key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                string apiURL = $"{currWeatherAPIurl}?q={location}";

                HttpResponseMessage response = await client.GetAsync(apiURL);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    await Context.Channel.SendFileAsync(data);
                }
            }
        }

    }
}
