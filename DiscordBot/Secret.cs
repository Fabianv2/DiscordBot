using System;
using System.IO;

namespace DiscordBot
{
    public class Secret
    {
        public static string GetToken()
        {
            string tokenFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TOKEN", "TOKEN.txt");
            StreamReader srToken = new StreamReader(tokenFile);
            return srToken.ReadToEnd();
        }
    }
}
