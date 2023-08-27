using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.AccountManager;
using DiscordBot.Modules;
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
            BotClient.Ready += BotIstBereit;
            BotClient.SlashCommandExecuted += SlashCommandHandler;

            await Task.Delay(-1);
        }

        public Task BotHatWasGelogged(LogMessage message)
        {
            Console.WriteLine("Bot hat was: " + message);
            return Task.CompletedTask;
        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            await command.RespondAsync($"You Executed {command.Data.Name}");
        }

        public IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<BotManager>()
                .AddSingleton<TestCommandsModule>()
                .AddSingleton<GameModule>()
                .AddSingleton<LanguageModule>()
                .AddSingleton<WeatherModule>()
                .AddSingleton<HelpModule>()
                .AddSingleton<BankingModule>()
                .AddSingleton<LevelModule>()
                .BuildServiceProvider();
        }


        public async Task BotIstBereit()
        {
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);
            await BotClient.SetGameAsync("In Wartung");
            await UpdateUser();
            BotClient.MessageReceived += Nachricht;

            var guild = BotClient.GetGuild(877244280425566248);
            var guildCommand = new SlashCommandBuilder();
            guildCommand.WithName("first-command");
            guildCommand.WithDescription("This is my first guild slash command!");
            var globalCommand = new SlashCommandBuilder();
            globalCommand.WithName("first-global-command");
            globalCommand.WithDescription("This is my first global slash command");
            try
            {
                await guild.CreateApplicationCommandAsync(guildCommand.Build());
                await BotClient.CreateGlobalApplicationCommandAsync(globalCommand.Build());
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
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

        public async Task UpdateUser()
        {
            List<string> serverList = new List<string>();
            foreach (var foundServer in BotClient.Guilds)
            {
                serverList.Add(foundServer.ToString());
            }

            foreach (var toUpdateServer in serverList)
            {
                string serverNameConverted = toUpdateServer.ToString();
                string serverFiles = AppDomain.CurrentDomain.BaseDirectory;
                string jsonServerFile = Path.Combine(serverFiles, "DiscordGuilds", toUpdateServer + ".json");

                try
                {
                    FileInfo fileInfo = new FileInfo(jsonServerFile);
                    if (File.Exists(jsonServerFile) && fileInfo.Length == 0)
                    {
                        using (StreamWriter sw = new StreamWriter(jsonServerFile))
                        {
                            var jsonLayout = "{\n" +
                                            "    \"ServerName\": \"" + serverNameConverted + "\",\n" +
                                            "    \"Users\": [\n" +
                                            "        \n" +
                                            "    ]\n" +
                                            "}";
                            sw.Write(jsonLayout);
                        }
                    }
                    else if (!File.Exists(jsonServerFile))
                    {
                        using (FileStream fs = File.Create(jsonServerFile)) { }
                        using (StreamWriter sw = new StreamWriter(jsonServerFile))
                        {
                            var jsonLayout = "{\n" +
                                            "    \"ServerName\": \"" + serverNameConverted + "\",\n" +
                                            "    \"Users\": [\n" +
                                            "        \n" +
                                            "    ]\n" +
                                            "}";
                            sw.Write(jsonLayout);
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

                            if (!searchedUser.IsBot)
                            {
                                if (userData == null)
                                {
                                    UserData newUser = new UserData
                                    {
                                        Username = searchedUser.ToString(),
                                        Konto = 10.0
                                    };
                                    serverData.Users.Add(newUser);
                                }
                            }
                        }

                        string updatedJSON = JsonConvert.SerializeObject(serverData, Formatting.Indented);
                        Console.Write(updatedJSON);
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
    }
}
