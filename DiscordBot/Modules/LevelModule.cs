using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.AccountManager;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class LevelModule : ModuleBase<SocketCommandContext>
    {
        ServerDataManager _serverDataManager = new ServerDataManager();

        #region Commands
        [Command("Level")]
        [Alias("level")]
        public async Task ShowPlayerLevel(IGuildUser username = null)
        {
            if (username == null)
            {
                await Context.Channel.SendMessageAsync($"{Context.User.Mention}, du bist level {GetPlayerLevel(Context, username)}");
            }
            else
            {
                await Context.Channel.SendMessageAsync($"{username.Mention}, du bist level {GetPlayerLevel(Context, username)}");
            }
            
        }

        [Command("XP")]
        [Alias("xp")]
        public async Task ShowPlayerXP(IGuildUser username = null)
        {
            double currPlayerXP = (Convert.ToDouble(GetPlayerLevel(Context))+ 1) * 5;
            if (username == null)
            {
                await Context.Channel.SendMessageAsync($"{Context.User.Mention}, dein XP-Stand beträgt: {GetPlayerXP(Context, username)} von {currPlayerXP}");
            }
            else
            {
                await Context.Channel.SendMessageAsync($"{username.Mention}, dein XP-Stand beträgt: {GetPlayerXP(Context, username)} von {currPlayerXP}");
            }
        }
        #endregion Commands

        #region Methoden
        public async Task AddExperience(SocketUserMessage context)
        {
            string currServer = (context.Channel as SocketGuildChannel).Guild.ToString();
            var serverData = _serverDataManager.LoadServerData((context.Channel as SocketGuildChannel).Guild);
            var currUser = serverData.Users.Find(user => user.Username == context.Author.Username.ToString());

            double lvlUpThreshold = (currUser.Level + 1) * 5;

            if (currUser.EXP >= lvlUpThreshold)
            {
                currUser.Level += 1;
                currUser.EXP = 0;
                Console.WriteLine($"{context.Author.Username} ist um 1 aufgesteigen. Aktuelles Level: {currUser.Level}");
                await context.Channel.SendMessageAsync($"{context.Author.Mention}, du bist um 1 aufgesteigen. Aktuelles Level: {currUser.Level}");
            }
            currUser.EXP += 0.5;
            Console.WriteLine(currUser.EXP);

            _serverDataManager.SaveServerData(currServer, serverData);
        }

        public string GetPlayerLevel(ICommandContext context, IGuildUser username = null)
        {
            ServerData serverData = _serverDataManager.LoadServerData((context.Channel as SocketGuildChannel).Guild);
            
            if (username == null)
            {
                var currUserLevel = serverData.Users.Find(user => user.Username == context.User.ToString());
                return currUserLevel.Level.ToString();
            }
            else
            {
                var currUserLevel = serverData.Users.Find(user => user.Username == username.ToString());
                return currUserLevel.Level.ToString();
            }
        }

        public string GetPlayerXP(ICommandContext context, IGuildUser username = null)
        {
            ServerData serverData = _serverDataManager.LoadServerData((context.Channel as SocketGuildChannel).Guild);
            
            if (username == null)
            {
                var currUserLevel = serverData.Users.Find(user => user.Username == context.User.ToString());
                return (currUserLevel.EXP + 0.5).ToString();
            }
            else
            {
                var currUserLevel = serverData.Users.Find(user => user.Username == username.ToString());
                return (currUserLevel.EXP + 0.5).ToString();
            }
        }
        #endregion Methoden
    }
}
