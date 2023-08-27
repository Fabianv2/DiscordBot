using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.AccountManager
{
    public class ServerData
    {
        public string ServerName { get; set; }
        public List<UserData> Users { get; set; }
    }
}
