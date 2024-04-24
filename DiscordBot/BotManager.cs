using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.AccountManager;
using DiscordBot.Modules;
using DSharpPlus.Net;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class BotManager
    {
        LevelModule _levelModule = new LevelModule();
        HelpModule _helpModule = new HelpModule();

        public static DiscordSocketClient BotClient;
        public static CommandService Commands;
        public static IServiceProvider Services;
        public const string PREFIX = "#";

        public async Task RunBot()
        {
            var config = new DiscordSocketConfig()
            { GatewayIntents = GatewayIntents.All };

            BotClient = new DiscordSocketClient(config);

            Commands = new CommandService();
            Services = ConfigureServices();
            await BotClient.LoginAsync(Discord.TokenType.Bot, Secret.GetToken());
            await BotClient.StartAsync();
            BotClient.Log += BotHatWasGelogged;
            BotClient.Ready += Client_Ready;
            BotClient.SlashCommandExecuted += SlashCommandHandler;

            TimerManager _TimerManager = new TimerManager(BotClient);
            _TimerManager.Start();

            await Task.Delay(-1);
        }

        public Task BotHatWasGelogged(LogMessage message)
        {
            Console.WriteLine("Bot hat was: " + message);
            return Task.CompletedTask;
        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            await _helpModule.SlashCommandShowHelp(command);
        }

        public IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<BotManager>()
                .AddSingleton<GameModule>()
                .AddSingleton<LanguageModule>()
                .AddSingleton<WeatherModule>()
                .AddSingleton<HelpModule>()
                .AddSingleton<BankingModule>()
                .AddSingleton<LevelModule>()
                .AddSingleton<ModeratorModule>()
                .BuildServiceProvider();
        }

        public async Task Client_Ready()
        {
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);
            await BotClient.SetGameAsync("In Wartung - #help");
            await UpdateUser();
            BotClient.MessageReceived += Nachricht;
        }

        public async Task Nachricht(SocketMessage arg)
        {
            SocketUserMessage message = arg as SocketUserMessage;
            int commandPosition = 0;
            if (message.HasStringPrefix(PREFIX, ref commandPosition))
            {
                SocketCommandContext context = new SocketCommandContext(BotClient, message);
                IResult result = await Commands.ExecuteAsync(context, commandPosition, Services);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason + ": " + message);
                }
                if (result.Error.Equals(CommandError.UnmetPrecondition))
                {
                    await message.Channel.SendMessageAsync(result.ErrorReason);
                }
            }




            string blacklistFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BlacklistedWords", "Blacklist.txt");
            StreamReader sr = new StreamReader(blacklistFile);
            foreach (var blacklistedWord in sr.ReadLine())
            {
                if (message.Content == blacklistedWord.ToString())
                {
                    await message.DeleteAsync();
                }
            }

            if (message.Author.IsBot)
            {
                return;
            }
            else
            {
                Console.WriteLine("-----------------------------------------------------------------------------");
                Console.WriteLine($"({message.CreatedAt.ToLocalTime()}) \nServer: {(message.Channel as IGuildChannel)?.Guild?.Name ?? ""} \nUser: {message.Author.Username} \nMessage: {message.Content}");
                Console.WriteLine("-----------------------------------------------------------------------------");

                await _levelModule.AddExperience(message);
            }
        }

        public async Task UpdateUser([Remainder] DiscordSocketClient botClient = null)
        {
            List<string> serverList = new List<string>();
            foreach (var foundServer in BotClient.Guilds)
            {
                serverList.Add(foundServer.ToString());
            }

            foreach (var toUpdateServer in serverList)
            {
                string serverNameConverted = toUpdateServer.ToString();
                string jsonServerFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DiscordGuilds", toUpdateServer + ".json");

                try
                {
                    FileInfo fileInfo = new FileInfo(jsonServerFile);
                    if (File.Exists(jsonServerFile) && fileInfo.Length == 0)
                    {
                        using (StreamWriter sw = new StreamWriter(jsonServerFile))
                        {
                            sw.Write(GetJsonLayout(serverNameConverted));
                        }
                    }
                    else if (!File.Exists(jsonServerFile))
                    {
                        using (FileStream fs = File.Create(jsonServerFile))
                        using (StreamWriter sw = new StreamWriter(jsonServerFile))
                        {
                            sw.Write(GetJsonLayout(serverNameConverted));
                        }
                    }

                    if (toUpdateServer != null)
                    {
                        string jsonText = File.ReadAllText(jsonServerFile);
                        ServerData serverData = JsonConvert.DeserializeObject<ServerData>(jsonText);
                        var targetServer = BotClient.Guilds.FirstOrDefault(guild => guild.Name == toUpdateServer);

                        foreach (var searchedUser in targetServer.Users)
                        {
                            UserData userData = serverData.Users.Find(user => user.Username == searchedUser.ToString());

                            if (userData == null)
                            {
                                UserData newUser = new UserData
                                {
                                    Username = searchedUser.ToString(),
                                    Nickname = searchedUser.Nickname,
                                    Konto = 10.0,
                                    JoinedAt = searchedUser.JoinedAt.Value.LocalDateTime.ToString(),
                                    IsBot = searchedUser.IsBot
                                };
                                serverData.Users.Add(newUser);
                            }
                            else if (userData.Nickname != searchedUser.Nickname && searchedUser.Nickname != null)
                            {
                                userData.PrevNicknames += $"|{searchedUser.Nickname}";
                                userData.Nickname = searchedUser.Nickname;
                            }
                            else if (searchedUser.Nickname == null && userData.Nickname != null)
                            {
                                userData.Nickname = null;
                            }
                        }

                        string updatedJSON = JsonConvert.SerializeObject(serverData, Formatting.Indented);
                        Console.WriteLine(updatedJSON);
                        Console.WriteLine("");
                        File.WriteAllText(jsonServerFile, updatedJSON);
                    }
                    await Task.CompletedTask;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private string GetJsonLayout(string serverNameConverted)
        {
            return "{\n" +
                   "    \"ServerName\": \"" + serverNameConverted + "\",\n" +
                   "    \"Users\": [\n" +
                   "        \n" +
                   "    ]\n" +
                   "}";
        }
    }
}
