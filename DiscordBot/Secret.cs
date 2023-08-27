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
            StreamReader sr = new StreamReader(tokenFile);
            string token = sr.ReadToEnd();
            return token;
        }
    }
}
