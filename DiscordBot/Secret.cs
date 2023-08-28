using System;
using System.IO;

namespace DiscordBot
{
    public class Secret
    {
        public static string GetToken()
        {
            string serverFiles = AppDomain.CurrentDomain.BaseDirectory;
            string tokenFile = Path.Combine(serverFiles, "TOKEN", "TOKEN.txt");
            StreamReader srToken = new StreamReader(tokenFile);
            return srToken.ReadToEnd();
        }
    }
}
