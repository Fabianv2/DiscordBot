﻿using DiscordBot;

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