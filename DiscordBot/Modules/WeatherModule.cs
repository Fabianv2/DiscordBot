using Discord;
using Discord.Commands;
using DiscordBot.API;
using DiscordBot.Modules;
using DiscordBot.Services;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class WeatherModule : ModuleBase<SocketCommandContext>
    {
        ConvertTemperatures ct = new ConvertTemperatures();
        Localtime lt = new Localtime();

        [Command("Wetter")]
        [Alias("wetter", "Weather", "weather")]
        public async Task GetWeather(string location)
        {
            string serverFiles = AppDomain.CurrentDomain.BaseDirectory;
            string apiFile = Path.Combine(serverFiles, "apiKey", "apiKey.txt");
            StreamReader sr = new StreamReader(apiFile);
            string apiKey = sr.ReadToEnd();

            using (WebClient web = new WebClient())
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={location}&APPID={apiKey}";
                var json = web.DownloadString(url);
                WeatherInfo.root Info = JsonConvert.DeserializeObject<WeatherInfo.root>(json);
                await DisplayWeather(Info);
            }

            await Task.CompletedTask;
        }

        public async Task DisplayWeather(WeatherInfo.root Info)
        {
            var builder = new EmbedBuilder()
            {
                Title = $"**Wetter in {Info.name}, {Info.sys.country}**",
                Description = $"**{Info.weather[0].description}** with **{Info.main.humidity}%** humidity and **{Info.wind.speed} km/h** winds.",
                ThumbnailUrl = "https://cdn.jim-nielsen.com/ios/512/weather-2021-12-07.png",
                Color = new Color(93, 64, 242)
            };
            builder.AddField(x =>
            {
                x.Name = ":thermometer: **Temp:**";
                x.Value = $"**{ct.KelvinToCelcius(Info.main.temp)} °C**";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = ":person_shrugging: **Feels:**";
                x.Value = $"**{ct.KelvinToCelcius(Info.main.temp)} °C**";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = ":sweat_drops: **Humidity:**";
                x.Value = $"**{Info.main.humidity} %**";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = ":cloud: **Cloudcover:**";
                x.Value = $"{Info.clouds.all} %";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = ":eyes: **Visibility:**";
                x.Value = $"{Info.visibility / 1000} KM";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = ":clock4: **Local time:**";
                x.Value = $"{lt.GetLocaltime(Info.dt, Info.timezone)}";
                x.IsInline = true;
            });
            builder.WithFooter(footer =>
            {
                footer.Text = $"For NA: Temp: {ct.KelvinToFahrenheit(Info.main.temp)} F | Feels: {ct.KelvinToFahrenheit(Info.main.feels_like)} F";
            });
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}