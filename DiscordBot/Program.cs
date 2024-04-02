using DiscordBot;
using DiscordBot.AccountManager;
using DiscordBot.Modules;
using System;

namespace TutorialBot
{
    public class Program
    {
        static void Main(string[] args)
        {
            BotManager manager = new BotManager();
            manager.RunBot().GetAwaiter().GetResult();
        }
    }
}