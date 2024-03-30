using Discord;
using Discord.Commands;
using DiscordBot.API;
using DiscordBot.Modules;
using DiscordBot.Services;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class WeatherModule : ModuleBase<SocketCommandContext>
    {
        ConvertValues cv = new ConvertValues();
        Localtime lt = new Localtime();

        [Command("Wetter")]
        [Alias("wetter", "Weather", "weather")]
        public async Task GetWeather(string location)
        {
            string apiFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "apiKey", "apiKey.txt");
            StreamReader sr = new StreamReader(apiFile);
            string apiKey = sr.ReadToEnd();

            using (HttpClient client = new HttpClient())
            {
                //string url = $"https://api.openweathermap.org/data/2.5/weather?q={location}&APPID={apiKey}";
                string url = $"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={location}";
                var json = await client.GetStringAsync(url);
                WeatherInfo.root Info = JsonConvert.DeserializeObject<WeatherInfo.root>(json);
                await DisplayWeather(Info);
            }

            await Task.CompletedTask;
        }

        public async Task DisplayWeather(WeatherInfo.root Info)
        {
            var builder = new EmbedBuilder()
            {
                Title = $"**Wetter in {Info.location.name}, {Info.location.country}**",
                Description = $"**{Info.current.condition.text}** with **{Info.current.humidity}%** humidity and **{cv.MilesToKilometers(Info.current.wind_mph)} km/h** winds.",
                //ThumbnailUrl = "https://cdn.jim-nielsen.com/ios/512/weather-2021-12-07.png",
                ThumbnailUrl = $"https:{Info.current.condition.icon}",
                Color = new Color(93, 64, 242)
            };
            builder.AddField(x =>
            {
                x.Name = ":thermometer: **Temp:**";
                x.Value = $"**{Info.current.temp_c} °C**";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = ":person_shrugging: **Feels:**";
                x.Value = $"**{Info.current.feelslike_c} °C**";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = ":sweat_drops: **Humidity:**";
                x.Value = $"**{Info.current.humidity} %**";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = ":cloud: **Cloudcover:**";
                x.Value = $"{Info.current.cloud} %";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = ":eyes: **Visibility:**";
                x.Value = $"{Info.current.vis_km} KM";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = ":clock4: **Local time:**";
                x.Value = $"{Info.location.localtime}";
                x.IsInline = true;
            });
            builder.WithFooter(footer =>
            {
                footer.Text = $"For NA: Temp: {Info.current.temp_f} F | Feels: {Info.current.feelslike_f} F";
            });
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}