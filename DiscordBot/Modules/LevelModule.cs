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
        public async Task AddExperience(SocketUserMessage context)
        {
            string currServer = (context.Channel as SocketGuildChannel).Guild.ToString();
            string serverFiles = AppDomain.CurrentDomain.BaseDirectory;
            string jsonServerFile = Path.Combine(serverFiles, "DiscordGuilds", currServer + ".json");
            string jsonText = File.ReadAllText(jsonServerFile);

            ServerData serverData = JsonConvert.DeserializeObject<ServerData>(jsonText);
            var userAddEXP = serverData.Users.Find(user => user.Username == context.Author.Username.ToString());

            double lvlUpThreshold = (userAddEXP.Level + 1) * 5;

            if (userAddEXP.EXP >= lvlUpThreshold)
            {
                userAddEXP.Level += 1;
                userAddEXP.EXP = 0;
                Console.WriteLine($"{context.Author.Username} ist um 1 aufgesteigen. Aktuelles Level: {userAddEXP.Level}");
                await context.Channel.SendMessageAsync($"{context.Author.Mention}, du bist um 1 aufgesteigen. Aktuelles Level: {userAddEXP.Level}");
            }

            userAddEXP.EXP += 0.5;
            Console.WriteLine(userAddEXP.EXP);

            string updatedJson = JsonConvert.SerializeObject(serverData, Formatting.Indented);
            File.WriteAllText(jsonServerFile, updatedJson);
        }
    }
}
