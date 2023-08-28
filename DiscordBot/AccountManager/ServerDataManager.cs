using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.IO;

namespace DiscordBot.AccountManager
{
    public class ServerDataManager
    {
        public ServerData LoadServerData(SocketGuild guild)
        {
            string jsonServerFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DiscordGuilds", $"{guild.Name}.json");

            if (!File.Exists(jsonServerFile))
            {
                return null;
            }

            string jsonText = File.ReadAllText(jsonServerFile);

            if (string.IsNullOrWhiteSpace(jsonText))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ServerData>(jsonText);
        }

        public void SaveServerData(string serverName, ServerData serverData)
        {
            string jsonServerFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DiscordGuilds", serverName + ".json");
            string updatedJson = JsonConvert.SerializeObject(serverData, Formatting.Indented);
            File.WriteAllText(jsonServerFile, updatedJson);
        }
    }
}
